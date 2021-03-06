using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Xamarin.Forms.Internals;
using Expression = System.Linq.Expressions.Expression;


namespace Xamarin.Forms.Platform.WPF
{
	public static class Forms
	{
		static bool s_isInitialized;

		public static UIElement ConvertPageToUIElement(this Page page, WpfApplicationPage applicationPage)
		{
			var application = new DefaultApplication();
			application.MainPage = page;
			var result = new Platform(applicationPage);
			result.SetPage(page);
			return result.GetCanvas();
		}

		public static void Init()
		{
			if (s_isInitialized)
				return;

			// Needed to prevent stripping of System.Windows.Interactivity
			// which is current only referenced in the XAML DataTemplates
			var eventTrigger = new System.Windows.Interactivity.EventTrigger();

			string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

			System.Windows.Application.Current.Resources.MergedDictionaries.Add(new System.Windows.ResourceDictionary
			{
				Source = new Uri(string.Format("/{0};component/WPResources.xaml", assemblyName), UriKind.Relative)
			});

			var accent = System.Windows.Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
			System.Windows.Media.Color color = accent.Color;
			Color.Accent = Color.FromRgba(color.R, color.G, color.B, color.A);

			Log.Listeners.Add(new DelegateLogListener((c, m) => Console.WriteLine("[{0}] {1}", m, c)));

			Device.OS = TargetPlatform.WinPhone;
			Device.PlatformServices = new WP8PlatformServices();
			Device.Info = new WP8DeviceInfo();

			Registrar.RegisterAll(new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) });

			Ticker.Default = new WinPhoneTicker();

			Device.Idiom = TargetIdiom.Phone;

			ExpressionSearch.Default = new WinPhoneExpressionSearch();

			s_isInitialized = true;
		}

		class DefaultApplication : Application
		{
		}

		internal class WP8DeviceInfo : DeviceInfo
		{
			internal const string BWPorientationChangedName = "Xamarin.WP8.OrientationChanged";
			readonly double _scalingFactor;

			public WP8DeviceInfo()
			{
				MessagingCenter.Subscribe(this, BWPorientationChangedName, (FormsApplicationPage page, DeviceOrientation orientation) => { CurrentOrientation = orientation; });

				FrameworkElement content = System.Windows.Application.Current.MainWindow.Content as FrameworkElement;

			    if (content == null)
			    {
			        // Scaling Factor for Windows Phone 8 is relative to WVGA: https://msdn.microsoft.com/en-us/library/windows/apps/jj206974(v=vs.105).aspx
			        // _scalingFactor = content.ScaleFactor / 100d;

			        PixelScreenSize = new Size(content.ActualWidth * _scalingFactor, content.ActualHeight * _scalingFactor);
			        ScaledScreenSize = new Size(content.ActualWidth, content.ActualHeight);
			    }
			}

			public override Size PixelScreenSize { get; }

			public override Size ScaledScreenSize { get; }

			public override double ScalingFactor
			{
				get { return _scalingFactor; }
			}

			protected override void Dispose(bool disposing)
			{
				MessagingCenter.Unsubscribe<FormsApplicationPage, DeviceOrientation>(this, BWPorientationChangedName);
				base.Dispose(disposing);
			}
		}

		sealed class WinPhoneExpressionSearch : IExpressionSearch
		{
			List<object> _results;
			Type _targeType;

			public List<T> FindObjects<T>(Expression expression) where T : class
			{
				_results = new List<object>();
				_targeType = typeof(T);

				Visit(expression);

				return _results.Select(o => o as T).ToList();
			}

			void Visit(Expression expression)
			{
				if (expression == null)
					return;

				switch (expression.NodeType)
				{
					case ExpressionType.Negate:
					case ExpressionType.NegateChecked:
					case ExpressionType.Not:
					case ExpressionType.Convert:
					case ExpressionType.ConvertChecked:
					case ExpressionType.ArrayLength:
					case ExpressionType.Quote:
					case ExpressionType.TypeAs:
					case ExpressionType.UnaryPlus:
						Visit(((UnaryExpression)expression).Operand);
						break;
					case ExpressionType.Add:
					case ExpressionType.AddChecked:
					case ExpressionType.Subtract:
					case ExpressionType.SubtractChecked:
					case ExpressionType.Multiply:
					case ExpressionType.MultiplyChecked:
					case ExpressionType.Divide:
					case ExpressionType.Modulo:
					case ExpressionType.Power:
					case ExpressionType.And:
					case ExpressionType.AndAlso:
					case ExpressionType.Or:
					case ExpressionType.OrElse:
					case ExpressionType.LessThan:
					case ExpressionType.LessThanOrEqual:
					case ExpressionType.GreaterThan:
					case ExpressionType.GreaterThanOrEqual:
					case ExpressionType.Equal:
					case ExpressionType.NotEqual:
					case ExpressionType.Coalesce:
					case ExpressionType.ArrayIndex:
					case ExpressionType.RightShift:
					case ExpressionType.LeftShift:
					case ExpressionType.ExclusiveOr:
						var binary = (BinaryExpression)expression;
						Visit(binary.Left);
						Visit(binary.Right);
						Visit(binary.Conversion);
						break;
					case ExpressionType.TypeIs:
						Visit(((TypeBinaryExpression)expression).Expression);
						break;
					case ExpressionType.Conditional:
						var conditional = (ConditionalExpression)expression;
						Visit(conditional.Test);
						Visit(conditional.IfTrue);
						Visit(conditional.IfFalse);
						break;
					case ExpressionType.MemberAccess:
						VisitMemberAccess((MemberExpression)expression);
						break;
					case ExpressionType.Call:
						var methodCall = (MethodCallExpression)expression;
						Visit(methodCall.Object);
						VisitList(methodCall.Arguments, Visit);
						break;
					case ExpressionType.Lambda:
						Visit(((LambdaExpression)expression).Body);
						break;
					case ExpressionType.New:
						VisitList(((NewExpression)expression).Arguments, Visit);
						break;
					case ExpressionType.NewArrayInit:
					case ExpressionType.NewArrayBounds:
						VisitList(((NewArrayExpression)expression).Expressions, Visit);
						break;
					case ExpressionType.Invoke:
						var invocation = (InvocationExpression)expression;
						VisitList(invocation.Arguments, Visit);
						Visit(invocation.Expression);
						break;
					case ExpressionType.MemberInit:
						var init = (MemberInitExpression)expression;
						VisitList(init.NewExpression.Arguments, Visit);
						VisitList(init.Bindings, VisitBinding);
						break;
					case ExpressionType.ListInit:
						var init1 = (ListInitExpression)expression;
						VisitList(init1.NewExpression.Arguments, Visit);
						VisitList(init1.Initializers, initializer => VisitList(initializer.Arguments, Visit));
						break;
					case ExpressionType.Constant:
						break;
					default:
						throw new ArgumentException(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
				}
			}

			void VisitBinding(MemberBinding binding)
			{
				switch (binding.BindingType)
				{
					case MemberBindingType.Assignment:
						Visit(((MemberAssignment)binding).Expression);
						break;
					case MemberBindingType.MemberBinding:
						VisitList(((MemberMemberBinding)binding).Bindings, VisitBinding);
						break;
					case MemberBindingType.ListBinding:
						VisitList(((MemberListBinding)binding).Initializers, initializer => VisitList(initializer.Arguments, Visit));
						break;
					default:
						throw new ArgumentException(string.Format("Unhandled binding type '{0}'", binding.BindingType));
				}
			}

			static void VisitList<TList>(IEnumerable<TList> list, Action<TList> visitor)
			{
				foreach (TList element in list)
					visitor(element);
			}

			void VisitMemberAccess(MemberExpression member)
			{
				if (member.Expression is ConstantExpression && member.Member is FieldInfo)
				{
					object container = ((ConstantExpression)member.Expression).Value;
					object value = ((FieldInfo)member.Member).GetValue(container);

					if (_targeType.IsInstanceOfType(value))
						_results.Add(value);
				}
				Visit(member.Expression);
			}
		}
	}
}