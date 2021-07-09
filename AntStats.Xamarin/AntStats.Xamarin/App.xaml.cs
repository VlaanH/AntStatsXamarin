using Xamarin.Forms;


namespace AntStats.Xamarin
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();
            
            
            Current.MainPage = new NavigationPage(new Stats());

        }

    }
}
