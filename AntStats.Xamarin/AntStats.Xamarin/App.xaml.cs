using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


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
