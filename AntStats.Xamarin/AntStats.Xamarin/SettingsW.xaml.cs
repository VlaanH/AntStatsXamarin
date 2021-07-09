using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using AntStatsCore;
using AntStatsCore.Database;

namespace AntStats.Xamarin
{
    public partial class SettingsW : ContentPage,INotifyPropertyChanged
    {
        private static string XamarinPatch = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public SettingsW()
        {
            InitializeComponent();


            NavigationPage.SetHasNavigationBar(this, false);
      
            LoaudingSettings();
        }

        async void LoaudingSettings()
        {
            await Task.Delay(300);
            var settings = await Settings.Get(XamarinPatch);

       
            SetSetting(settings);
        }

        void SetSetting(SettingsData settings)
        {
            if (settings.IP != null)
                Tip.Text = settings.IP;
            if (settings.User != null)
                Tuser.Text = settings.User;

            if (settings.Pass != null)
               Tpassword.Text = settings.Pass;

            if (settings.Port != null)
               Tport.Text = settings.Port;

            if (settings.DataBaseName != null)
                TDataBase.Text = settings.DataBaseName;

            if (settings.NameTable != null)
                TnameTable.Text = settings.NameTable;

            if (settings.DatabasePass != null)
                MysqlTpassword.Text = settings.DatabasePass;


            if (settings.DatabaseUser != null)
                MysqlTuser.Text = settings.DatabaseUser;

            if (settings.DatabaseIP != null)
                MysqlTip.Text = settings.DatabaseIP;


            if (settings.AutoUpdate != null  )
            {
                AutoUpdate.IsChecked = settings.AutoUpdate;
                if (settings.AutoUpdateValue!=null)
                    AutoUpdateSlider.Value = double.Parse(settings.AutoUpdateValue);

            }



            if (settings.DataBase != null)
            {
                ToggleSwitchMySql.IsChecked = settings.DataBase;


            }

            if (settings.Server != null)
            {
                ToggleSwitchServer.IsChecked = settings.Server;

                
            }
            
        }


        SettingsData GetSetting()
        {
            SettingsData settings = new SettingsData();

            settings.IP = Tip.Text;
            
            settings.User = Tuser.Text;

            settings.Pass = Tpassword.Text;

            
            settings.Port = Tport.Text;

            settings.DataBaseName = TDataBase.Text;

            settings.NameTable = TnameTable.Text;

            settings.DatabasePass = MysqlTpassword.Text;

            settings.DatabaseUser = MysqlTuser.Text;

            settings.DatabaseIP = MysqlTip.Text;

            
            settings.AutoUpdateValue = AutoUpdateSlider.Value.ToString();

            settings.AutoUpdate = AutoUpdate.IsChecked;
            
            settings.DataBase = ToggleSwitchMySql.IsChecked;

            settings.Server = ToggleSwitchServer.IsChecked;

            return settings;
        }



        async private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            
            var settings = GetSetting();


            Settings.Save(settings,XamarinPatch);

          
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
