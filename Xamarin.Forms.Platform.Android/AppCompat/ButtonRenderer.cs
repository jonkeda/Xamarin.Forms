using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using GlobalResource = Android.Resource;
using Object = Java.Lang.Object;

namespace Xamarin.Forms.Platform.Android.AppCompat
{
	public class ButtonRenderer : ViewRenderer<Button, AppCompatButton>, global::Android.Views.View.IOnAttachStateChangeListener
	{
		TextColorSwitcher _textColorSwitcher;
		float _defaultFontSize;
		Typeface _defaultTypeface;
		bool _isDisposed;

		public ButtonRenderer()
		{
			AutoPackage = false;
		}

		global::Android.Widget.Button NativeButton => Control;

		void IOnAttachStateChangeListener.OnViewAttachedToWindow(global::Android.Views.View attachedView)
		{
			UpdateText();
		}

		void IOnAttachStateChangeListener.OnViewDetachedFromWindow(global::Android.Views.View detachedView)
		{
		}

		public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			UpdateText();
			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		protected override AppCompatButton CreateNativeControl()
		{
			return new AppCompatButton(Context);
		}

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			_isDisposed = true;

			if (disposing)
			{
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
			}

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					AppCompatButton button = CreateNativeControl();

					button.SetOnClickListener(ButtonClickListener.Instance.Value);
					button.Tag = this;
					_textColorSwitcher = new TextColorSwitcher(button.TextColors);  
					SetNativeControl(button);

					button.AddOnAttachStateChangeListener(this);
				}

				UpdateAll();
				UpdateBackgroundColor();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Button.TextProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Button.TextColorProperty.PropertyName)
				UpdateTextColor();
			else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
				UpdateEnabled();
			else if (e.PropertyName == Button.FontProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == Button.ImageProperty.PropertyName)
				UpdateBitmap();
			else if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
				UpdateText();

			base.OnElementPropertyChanged(sender, e);
		}

		protected override void UpdateBackgroundColor()
		{
			if (Element == null || Control == null)
				return;

			Color backgroundColor = Element.BackgroundColor;
			if (backgroundColor.IsDefault)
			{
				if (Control.SupportBackgroundTintList != null)
				{
					Context context = Context;
					int id = GlobalResource.Attribute.ButtonTint;
					unchecked
					{
						using (var value = new TypedValue())
						{
							try
							{
								Resources.Theme theme = context.Theme;
								if (theme != null && theme.ResolveAttribute(id, value, true))
#pragma warning disable 618
									Control.SupportBackgroundTintList = Resources.GetColorStateList(value.Data);
#pragma warning restore 618
								else
									Control.SupportBackgroundTintList = new ColorStateList(ColorExtensions.States, new[] { (int)0xffd7d6d6, 0x7fd7d6d6 });
							}
							catch (Exception ex)
							{
								Log.Warning("Xamarin.Forms.Platform.Android.ButtonRenderer", "Could not retrieve button background resource: {0}", ex);
								Control.SupportBackgroundTintList = new ColorStateList(ColorExtensions.States, new[] { (int)0xffd7d6d6, 0x7fd7d6d6 });
							}
						}
					}
				}
			}
			else
			{
				int intColor = backgroundColor.ToAndroid().ToArgb();
				int disableColor = backgroundColor.MultiplyAlpha(0.5).ToAndroid().ToArgb();
				Control.SupportBackgroundTintList = new ColorStateList(ColorExtensions.States, new[] { intColor, disableColor });
			}
		}

		void UpdateAll()
		{
			UpdateFont();
			UpdateText();
			UpdateBitmap();
			UpdateTextColor();
			UpdateEnabled();
		}

		void UpdateBitmap()
		{
			var elementImage = Element.Image;
			var imageFile = elementImage?.File;

			if (elementImage == null || string.IsNullOrEmpty(imageFile))
			{
				Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, null);
				return;
			}

			var image = Context.Resources.GetDrawable(imageFile);

			if (string.IsNullOrEmpty(Element.Text))
			{
				// No text, so no need for relative position; just center the image
				// There's no option for just plain-old centering, so we'll use Top 
				// (which handles the horizontal centering) and some tricksy padding 
				// to handle the vertical centering
				Control.SetCompoundDrawablesWithIntrinsicBounds(null, image, null, null);
				Control.SetPadding(0, Control.PaddingTop, 0, -Control.PaddingTop);
				image?.Dispose();
				return;
			}

			var layout = Element.ContentLayout;

			Control.CompoundDrawablePadding = (int)layout.Spacing;

			switch (layout.Position)
			{
				case Button.ButtonContentLayout.ImagePosition.Top:
					Control.SetCompoundDrawablesWithIntrinsicBounds(null, image, null, null);
					break;
				case Button.ButtonContentLayout.ImagePosition.Bottom:
					Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, image);
					break;
				case Button.ButtonContentLayout.ImagePosition.Right:
					Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, image, null);
					break;
				default:
					// Defaults to image on the left
					Control.SetCompoundDrawablesWithIntrinsicBounds(image, null, null, null);
					break;
			}

			image?.Dispose();
		}

		void UpdateEnabled()
		{
			Control.Enabled = Element.IsEnabled;
		}

		void UpdateFont()
		{
			Button button = Element;
			Font font = button.Font;

			if (font == Font.Default && _defaultFontSize == 0f)
				return;

			if (_defaultFontSize == 0f)
			{
				_defaultTypeface = NativeButton.Typeface;
				_defaultFontSize = NativeButton.TextSize;
			}

			if (font == Font.Default)
			{
				NativeButton.Typeface = _defaultTypeface;
				NativeButton.SetTextSize(ComplexUnitType.Px, _defaultFontSize);
			}
			else
			{
				NativeButton.Typeface = font.ToTypeface();
				NativeButton.SetTextSize(ComplexUnitType.Sp, font.ToScaledPixel());
			}
		}

		void UpdateText()
		{
			NativeButton.Text = Element.Text;
		}

		void UpdateTextColor()
		{
			_textColorSwitcher?.UpdateTextColor(Control, Element.TextColor);
		}

		class ButtonClickListener : Object, IOnClickListener
		{
			#region Statics

			public static readonly Lazy<ButtonClickListener> Instance = new Lazy<ButtonClickListener>(() => new ButtonClickListener());

			#endregion

			public void OnClick(global::Android.Views.View v)
			{
				var renderer = v.Tag as ButtonRenderer;
				((IButtonController)renderer?.Element)?.SendClicked();
			}
		}
	}
}