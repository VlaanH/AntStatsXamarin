using Xamarin.Forms;

namespace AntStats.Xamarin
{
    public class CustomItem
    {
        public static Label AddLabel(string nameLabel)
        {
            Label labelCustom = new Label();
        
            
            labelCustom.Text = "-";
            labelCustom.FontSize = 16;
            labelCustom.Margin = new Thickness(1);
            labelCustom.VerticalOptions = LayoutOptions.Center;
            labelCustom.HorizontalOptions = LayoutOptions.Center;
            labelCustom.TextColor=Color.Azure;

            
            return labelCustom;
        }




    }
}