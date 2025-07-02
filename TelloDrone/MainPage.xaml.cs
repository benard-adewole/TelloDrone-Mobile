namespace TelloDrone;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	

    void PanGestureRecognizer_PanUpdated(System.Object sender, Microsoft.Maui.Controls.PanUpdatedEventArgs e)
    {
		switch (e.StatusType)
		{
			case GestureStatus.Running:
				BoxView boxView = (sender as BoxView);

                Frame frame = boxView.Parent as Frame;
				var x_max = (frame.Width / 2) - (boxView.Width/2);
				var y_max = (frame.Height / 2) - (boxView.Height/2);

				if (Math.Abs(e.TotalX) <= x_max && Math.Abs(e.TotalX) > Math.Abs(e.TotalY))
				{
                    boxView.TranslationX = e.TotalX;
                }
                if (Math.Abs(e.TotalY) <= y_max && Math.Abs(e.TotalY) > Math.Abs(e.TotalX))
                {
                    boxView.TranslationY = e.TotalY;
                }

                break;
            case GestureStatus.Completed:
                ((BoxView)sender).TranslationX = 0;
                ((BoxView)sender).TranslationY = 0;
                break;
            default:
				break;
		}
	}
}


