using System;

namespace Xamarin.Forms.Platform.WPF
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ExportRendererAttribute : HandlerAttribute
	{
		public ExportRendererAttribute(Type handler, Type target) : base(handler, target)
		{
		}
	}
}