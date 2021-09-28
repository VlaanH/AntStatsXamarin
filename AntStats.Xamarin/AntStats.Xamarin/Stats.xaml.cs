using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AntStats.Xamarin.Column;
using AntStats.Xamarin.Profile;
using Xamarin.Forms;
using AntStatsCore;
using AntStatsCore.Database;

namespace AntStats.Xamarin
{
   
    public partial class Stats : ContentPage,INotifyPropertyChanged
    {
        private static string XamarinPatch = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+"/";
        public Stats()
        {
            InitializeComponent();
            AddBasicElements();
            NavigationPage.SetHasNavigationBar(this, false);

            
          

        }



        List<Label> ColumnAdd(string columnName, int columnId)
        {
            List<Label> column = new List<Label>();
            for (int i = 1; i < 10; i++)
            {
                
                var label = CustomItem.CustomItem.AddLabel(columnName + i);
                MainW.Children.Add(label);
                Grid.SetColumn(label, columnId);
                Grid.SetRow(label, i);

                column.Add(label);
            }

            return column;
        }



        void AddBasicElements()
        {


            ColumnList.GHRT = ColumnAdd("GH/S(RT)", 3);

            ColumnList.HW = ColumnAdd("HW", 2);

            ColumnList.TempChip = ColumnAdd("TempChip", 1);

            ColumnList.Chain = ColumnAdd("Chain", 0);

        }

        private void SetAsicColumnTable(AsicStandardStatsObject asicColumn)
        {
            string contentDefault = "-";
            int maxI = 9;
            for (int i = 0; i < maxI; i++)
            {

                try
                {
                    if (asicColumn.LasicAsicColumnStats[i].TempChip != null)
                        ColumnList.TempChip[i].Text = asicColumn.LasicAsicColumnStats[i].TempChip;
                }
                catch (Exception e)
                {
                    ColumnList.TempChip[i].Text = contentDefault;
                }

                try
                {
                    if (asicColumn.LasicAsicColumnStats[i].HW != null)
                        ColumnList.HW[i].Text = asicColumn.LasicAsicColumnStats[i].HW;
                }
                catch (Exception e)
                {
                    ColumnList.HW[i].Text = contentDefault;
                }

                try
                {
                    if (asicColumn.LasicAsicColumnStats[i].GHRT != null)
                        ColumnList.GHRT[i].Text = asicColumn.LasicAsicColumnStats[i].GHRT;
                }
                catch (Exception e)
                {
                    ColumnList.GHRT[i].Text = contentDefault;
                }
                
                try
                {
                    if (asicColumn.LasicAsicColumnStats[i].Chain != null)
                        ColumnList.Chain[i].Text = asicColumn.LasicAsicColumnStats[i].Chain;
                }
                catch (Exception e)
                {
                    ColumnList.Chain[i].Text = contentDefault;
                }
            }
        }



        private bool _errors = false;

        
        private double _progress=0;

        private void ShowMessage(string errorText)
        {
            
            ProgressLabel.IsVisible = true;
            ProgressLabel.Text = errorText;
            ProgressLabel.IsEnabled = true;

        }
        public  double ProgressBar
        {
            get { return this._progress; }
            set 
            {
                this._progress = value;
                NotifyPropertyChange("ProgressBar");
            }
        }
        
        private void BlockButtons(string messages=default)
        {
            ProgressLabel.IsVisible = false;

            ProgressLabel.IsVisible = true;
            ProgressLabel.Text = messages;
            if(ButtonStats.Text=="GET")
               ButtonStats.IsEnabled = false;
            ButtonSettings.IsEnabled = false;
            
        }
        private void UnlockButtons(bool error,string message)
        {
            if (error==false)
                ProgressLabel.Text = message;
            
            ButtonStats.IsEnabled = true;
            ButtonSettings.IsEnabled = true;  
        }

        async void GetStats(bool autoUpdate)
        {   
            
            ProgressBar = 0;
            DatabaseProgressBar.IsVisible = false;
            
            BindingContext = this;
            _errors = false;
            
         
            
            
           
           
            
            SettingsData settings = new SettingsData();
            AsicStandardStatsObject statsObject = new AsicStandardStatsObject();
            
            
      

            
            
               await Task.Run(() =>
               {  settings = Settings.Get(ProfileManagement.GlobalSelectedProfile,XamarinPatch).Result; });

      
               
               
                   
               AsicStats asicStats = new AsicStats(settings);

               if (settings.DataBase==true)
               {
                   try
                   { 
                       BlockButtons("Getting from DataBase");
                       await Task.Run(() => 
                           { statsObject = asicStats.GetDataBase(); });
                   }
                   catch (Exception exception)
                   {
                       ShowMessage("DataBase Error");
                       _errors = true;
                   }
                   
               }
               else
               {
                   try
                   { 
                       
                        BlockButtons("Getting from Localhost");
                       
                       await Task.Run(() =>
                           { statsObject = asicStats.GetLocalhost(); });
                       
                   }
                   catch (Exception exception)
                   {
                       ShowMessage("Localhost Error");
                       _errors = true;
                   }

        
                   
                   if (settings.Server == true & _errors == false)
                   {
                     
                        BlockButtons("Update Database");
                       
                       DatabaseProgressBar.IsVisible = true;
                       Result res = Result.ErrorExist;
                       int progress = 0;
                       bool update = true;
                       new Thread(() =>
                       { 
                           do
                           {   Thread.Sleep(500);
                               ProgressBar = Convert.ToDouble(progress)/100;
                           } while (update == true);
                       }).Start();    
                       
                       await Task.Run(() => { res=asicStats.SetDataBase(statsObject, ref progress); });
                       
                       if (res == Result.ErrorExist)
                       {
                           ShowMessage("DataBase Error"); 
                           _errors = true;

                       }
                       DatabaseProgressBar.IsVisible = false;
             
                           

                       
                       update = false;
                   }

               }



               if (autoUpdate == false | ButtonStats.Text == "GET")
                   UnlockButtons(_errors, "AntStats Xamarin");
               else
                   ProgressLabel.Text = default;
          
              
               
               if(_errors==false)
                   SetAsicColumnTable(statsObject);
            
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            SettingsData settings = new SettingsData();
            await Task.Run(() =>
                {  settings = Settings.Get(ProfileManagement.GlobalSelectedProfile,XamarinPatch).Result; });

            if (settings.AutoUpdate == true | ButtonSettings.IsEnabled==false)
            {
                if (ButtonSettings.IsEnabled==true)
                {
                    ButtonSettings.IsEnabled = false;
                    ButtonStats.Text = "Stop";
                }
                else
                {
                    ButtonSettings.IsEnabled = true;
                    ButtonStats.Text = "GET";
                    ProgressLabel.Text = "AntStats Xamarin";
                }

                while (ButtonSettings.IsEnabled==false)
                {
                  
                    GetStats(true);
                    
                    for (int i = 0; i < int.Parse(settings.AutoUpdateValue)*60 & ButtonSettings.IsEnabled==false; i++)
                    {
                        ButtonStats.Text = "Stop"+(Convert.ToInt32(settings.AutoUpdateValue)*60-i)+"s";
                        
                        await Task.Delay(1000);
                    }
                   
                }

                ButtonStats.Text = "GET";
            }
            else
                GetStats(false);
        }


       

        async private void ButtonSettings_Clicked(object sender, EventArgs e)
        {

            var pg = new NavigationPage(new SettingsW());

            pg.Title = "Settings AntStats Xamarin";
            pg.BarBackgroundColor= Color.Aqua;
       
            await Navigation.PushAsync(pg);
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