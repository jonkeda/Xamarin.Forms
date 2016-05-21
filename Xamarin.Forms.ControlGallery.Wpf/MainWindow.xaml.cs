using System.Windows;

namespace Xamarin.Forms.ControlGallery.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Xamarin.Forms.Platform.WPF.Forms.Init();

            Controls.App app = new Controls.App();

            Platform.WPF.Platform platform = app.MainPage.GetPlatform(this); ;

            cp.Content = (UIElement)platform;
        }
    }
}
