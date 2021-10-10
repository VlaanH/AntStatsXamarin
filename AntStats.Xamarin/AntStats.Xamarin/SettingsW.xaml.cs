using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AntStats.Xamarin.Profile;
using Xamarin.Forms;
using AntStatsCore;
using AntStatsCore.Database;

namespace AntStats.Xamarin
{
    public partial class SettingsW : ContentPage,INotifyPropertyChanged
    {
        private static string XamarinPatch = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+"/";
        public SettingsW()
        {
            InitializeComponent();


            NavigationPage.SetHasNavigationBar(this, false);
      
            LoadingSettings();
        }

        async void LoadingSettings()
        {
            ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings = new List<ProfileXamarinObj>();
            try
            {
                AddingExistingSettingsProfiles();
               
                var profiles= await Settings.GetProfiles(XamarinPatch);



                if (profiles.Count==0)
                {
                    SettingsData settingsData = new SettingsData();
                    
                    settingsData.NameProfile = ProfileManagement.GetRandomProfileName();
                    
                    
                    
                    SetSetting(settingsData);
                }
                else if (profiles.Count>0 & ProfileManagement.GlobalSelectedProfile==default)
                {
                    var settings = await Settings.Get(profiles[0].NameProfile,XamarinPatch);
                    
                    ProfileManagement.SelectProfile(ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings,profiles[0].NameProfile);

                    SetSetting(settings); 
                }
                else
                {
                    var settings = await Settings.Get(ProfileManagement.GlobalSelectedProfile,XamarinPatch);
                    
                    ProfileManagement.SelectProfile(ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings,ProfileManagement.GlobalSelectedProfile);
                    
                    SetSetting(settings); 
                }
            }
            catch (Exception e)
            {
                
            }
        }
        void AddProfile(string profileName,bool isEnabled)
        {
            
            ProfileXamarinObj profilesuAvalonObj = new ProfileXamarinObj();
            profilesuAvalonObj.profButton.Text = profileName;
            profilesuAvalonObj.profButton.IsEnabled = isEnabled;
           

            
            profilesuAvalonObj.minusButton.Text = "-";
            profilesuAvalonObj.minusButton.WidthRequest = 30;
           
            
           
            
            StackLayout stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Horizontal;
            stackLayout.Spacing = 0;
                
            
            stackLayout.Children.Add(profilesuAvalonObj.profButton);
            stackLayout.Children.Add(profilesuAvalonObj.minusButton);

            profilesuAvalonObj.stackLayout = stackLayout;
            
            ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings.Add(profilesuAvalonObj);
        
            stackLayout.Orientation = StackOrientation.Horizontal;
            Profiles.Children.Add(stackLayout);
            
            
            
            profilesuAvalonObj.minusButton.Clicked += (s, e) =>
            { 
               
                Profiles.Children.Remove(stackLayout);
                
                Settings.DeleteSettingsProfile(profileName,XamarinPatch);
                
            };
            
            
            profilesuAvalonObj.profButton.Clicked += async (s, e) =>
            {
                
                ProfileManagement.SelectProfile(ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings,profileName);
                

                profilesuAvalonObj.profButton.IsEnabled = false;
                
                
                var settings = await Settings.Get(profileName,XamarinPatch);
                
                SetSetting(settings); 
                
            };
        }
        
        
     
        private void ButtonAddProfile_OnClick(object sender, EventArgs e)
        { 
            SettingsData settingsData = new SettingsData();
            
            SetSetting(settingsData);
           
            NameProfileEntry.Text = "P"+new Random().Next(0,2000);
          
        }
        
        void SetSetting(SettingsData settings)
        {
            
            if (settings.NameProfile != null)
                NameProfileEntry.Text = settings.NameProfile;
            else
                NameProfileEntry.Text = default;

            if (settings.IP != null)
                Tip.Text = settings.IP;
            else
                Tip.Text = default;

            if (settings.User != null)
                Tuser.Text = settings.User;
            else
                Tuser.Text = default;
            
            if(settings.Pass!=null)
             Tpassword.Text = settings.Pass;
            else
                Tpassword.Text = default;
            
            if(settings.Port!=null)
                Tport.Text = settings.Port;
            else
                Tport.Text = default;
            
            
            if(settings.DataBaseName!=null)
                TDataBase.Text = settings.DataBaseName;
            else
                TDataBase.Text = default;
            
            
            if(settings.NameTable!=null)
                TnameTable.Text = settings.NameTable;
            else
                TnameTable.Text = default;

            if(settings.DatabasePass!=null)
                MysqlTpassword.Text = settings.DatabasePass;
            else
                MysqlTpassword.Text = default;
          
            if(settings.DatabaseUser!=null)
                MysqlTuser.Text=settings.DatabaseUser;
            else
                MysqlTuser.Text = default;
                    
            if(settings.DatabaseIP!=null)
                MysqlTip.Text = settings.DatabaseIP  ;
            else
                MysqlTip.Text = default;
            

            if (settings.AutoUpdate != null & settings.AutoUpdateValue!=null)
            {
                AutoUpdate.IsChecked = settings.AutoUpdate;
                if (AutoUpdate.IsChecked==true)
                {
                    GridAutoUpdate.IsVisible = true;
                    AutoUpdateSlider.IsVisible = true;
                    AutoUpdateSlider.Value = double.Parse(settings.AutoUpdateValue);

                }
                else
                {

                    AutoUpdate.IsChecked = false;
                    GridAutoUpdate.IsVisible = false;
                    
                }
            }
            else
            {   AutoUpdate.IsChecked = false;
                GridAutoUpdate.IsVisible = false;
                
            }
                


            if (settings.DataBase != null)
            {
                ToggleSwitchMySql.IsChecked = settings.DataBase;

            }

            if (settings.Server != null)
            {
                ToggleSwitchServer.IsChecked=settings.Server;

            }

            
        }


        SettingsData GetSetting()
        {
            SettingsData settings = new SettingsData();

            Console.WriteLine(XamarinPatch);
            
            settings.IP = Tip.Text;
            
            settings.User = Tuser.Text;

            settings.Pass = Tpassword.Text;

            settings.NameProfile = NameProfileEntry.Text;
            
            settings.Port = Tport.Text;

            settings.DataBaseName = TDataBase.Text;

            settings.NameTable = TnameTable.Text;

            settings.DatabasePass = MysqlTpassword.Text;

            settings.DatabaseUser = MysqlTuser.Text;

            settings.DatabaseIP = MysqlTip.Text;

            
            settings.AutoUpdateValue = Convert.ToInt32(AutoUpdateSlider.Value).ToString() ;

            settings.AutoUpdate = AutoUpdate.IsChecked;
            
            settings.DataBase = ToggleSwitchMySql.IsChecked;

            settings.Server = ToggleSwitchServer.IsChecked;

            return settings;
        }


        private void RemoveSettingsProfiles()
        {
            for (int i = 0; i <  ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings.Count; i++)
            {
                Profiles.Children.Remove(ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings[i].stackLayout);
            }

            ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings = new List<ProfileXamarinObj>();

        }
        
        private async void AddingExistingSettingsProfiles()
        {
            var profiles = await Settings.GetProfiles(XamarinPatch);
            if (profiles!=default)
            {
                for (int i = 0; i < profiles.Count; i++)
                {
                    bool enable = profiles[i].NameProfile!=ProfileManagement.GlobalSelectedProfile;


                    AddProfile(profiles[i].NameProfile,enable);
                
                }
                //selection of the first profile in the list if no profile is selected
                if (ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings.Count > 0 & ProfileManagement.GlobalSelectedProfile == default)
                {
                
                    ProfileManagement.SelectProfile(ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings,(string)ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings[0].profButton.Text);
                    ProfileManagement.GlobalSelectedProfile = (string) ProfilesAvaloniaObjList.ListProfilesuAvalonObj_Settings[0].profButton.Text;
                }
            }
            

        }
        
        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            
            var settings = GetSetting();
            settings.NameProfile = NameProfileEntry.Text;

            Settings.Save(settings,XamarinPatch); 
            
            RemoveSettingsProfiles();
            AddingExistingSettingsProfiles();
            
            
            
        }

        
        private void ShowError(string errorText)
        {
            
            CreatingTableProgressBar.IsVisible = false;

            CreatingTableLabel.IsVisible = true;
            CreatingTableLabel.Text = errorText;
            ButtonTable.IsEnabled = true;
            
        }

        private double _progress=0;

        public  double ProgressBar
        {
            get { return this._progress; }
            set 
            {
                this._progress = value;
                NotifyPropertyChange("ProgressBar");
            }
        }

        void EnableProgressBar()
        {
            GridCreatingTable.IsVisible = true;
            CreatingTableProgressBar.IsVisible = true;
            CreatingTableLabel.Text = "Creating Table";
        }

        void DisableProgressBar()
        {
            GridCreatingTable.IsVisible = false;
        }

        private async void ButtonTable_OnClicked(object sender, EventArgs e)
        { 
            BindingContext = this;
            DisableProgressBar();
            EnableProgressBar();
            var settingsClass = GetSetting();
           
                
            AsicStats asicStats = new AsicStats(settingsClass);

            
            try
            {
                Result res = Result.NoError;
                int progress = 0;
                bool update = true;
                new Thread(() =>
                { 
                    do
                    {   Thread.Sleep(100);
                        ProgressBar = Convert.ToDouble(progress)/100;
                        
                    } while (update == true);
                }).Start();    
                
                await Task.Run(() =>
                {
                    res = asicStats.CreateDataBaseTable(ref progress);
             
                });
                
                update = false;

                
                
                if (res==Result.ErrorExist)
                {
                    ShowError("Table Exist");

                }
                else
                {
                    DisableProgressBar();
                }
            }
            catch (Exception exception)
            {
                ShowError("DataBase Error");
            }



            settingsClass.NameProfile = ProfileManagement.GlobalSelectedProfile;
            Settings.Save(settingsClass,XamarinPatch);
            
            
        }

        private void AutoUpdate_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            GridAutoUpdate.IsVisible = AutoUpdate.IsChecked;
        }

        private void ToggleSwitchServer_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ToggleSwitchServer.IsChecked==true)
                ToggleSwitchMySql.IsChecked = false; 
        }

        private void ToggleSwitchMySql_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ToggleSwitchMySql.IsChecked==true)
                ToggleSwitchServer.IsChecked = false;
        }
        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

      
    }
}
