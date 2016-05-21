namespace Xamarin.Forms.Platform.WPF
{
    public delegate void GestureEventHandler(object sender, GestureEventArgs e);

    public partial class TableView : System.Windows.Controls.Grid
	{
		public TableView()
		{
			InitializeComponent();
		}

        // todo
	    public event GestureEventHandler Tap;// { get; set; }

	    public event GestureEventHandler Hold;// { get; set; }
	}
}