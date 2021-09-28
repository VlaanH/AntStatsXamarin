using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AntStats.Xamarin.Profile
{
    
    public class ProfileXamarinObj
    {
        public Button minusButton = new Button();
            
        public Button profButton = new Button();
        
        public StackLayout stackLayout = new StackLayout();
    }
    
    public static class ProfilesAvaloniaObjList
    {
        public static List<ProfileXamarinObj> ListProfilesuAvalonObj_Settings = new List<ProfileXamarinObj>();
    }
    
    
    
    
    public static class ProfileManagement
    {

        public static string GlobalSelectedProfile = default;
        
        
        //function select the desired profile
        public static void SelectProfile(List<ProfileXamarinObj>profileList,string profileName )
        {
           
            for (int i = 0; i < profileList.Count; i++)
                if ((string) profileList[i].profButton.Text==profileName)
                {
                    for (int j = 0; j < profileList.Count; j++)
                    {
                        profileList[j].profButton.IsEnabled = true;
                    }

                    profileList[i].profButton.IsEnabled = false; 
                }

            GlobalSelectedProfile = profileName;

        }

        public static string GetRandomProfileName()
        {
            return "P" + new Random().Next(1, 999);
        }



    }

}