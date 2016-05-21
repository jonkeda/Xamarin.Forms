namespace Xamarin.Forms.Platform.WPF
{
    public enum  SlideTransitionMode
    {
        SlideUpFadeIn,
        SlideDownFadeOut
    }

    public class SlideTransition
    {
        public SlideTransitionMode Mode { get; set; }

        internal ITransition GetTransition(System.Windows.Controls.Border border)
        {
            throw new System.NotImplementedException();
        }
    }
}