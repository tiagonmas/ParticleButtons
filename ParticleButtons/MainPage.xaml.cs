using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using ParticleButtons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xamarin.Essentials;
using Android.Provider;

namespace ParticleButtons
{
    public partial class MainPage : ContentPage
    {
        RestService rest;
        public MainPage()
        {
            InitializeComponent();
            rest = new RestService();


        }

        /// <summary>
        /// Control how many times buttons have been used, to force in app purchase
        /// </summary>
        /// <returns></returns>
        internal bool IncAppCounter()
        {
            try
            {
                string appCounter = SecureStorage.GetAsync("appCounter").Result;
                
                if (appCounter == null)
                {
                    SecureStorage.SetAsync("appCounter", "1");
                }
                else
                {
                    int count = Convert.ToInt32(appCounter);
                    if (count > 20)
                    {
                        return false;
                    }
                    SecureStorage.SetAsync("appCounter", (count + 1).ToString());
                }
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
            return true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var pfBtns = new List<pfButtons>();

            var files = Directory.EnumerateFiles(App.FolderPath, "*.txt");
            foreach (var filename in files)
            {
                pfButtons pfb= new pfButtons
                {
                    Filename = filename,
                    Text = File.ReadAllText(filename),
                    Date = File.GetCreationTime(filename)
                };
                
                var pFunc=JsonConvert.DeserializeObject<ParticleFunction>(pfb.Text);
                pfb.pFunc = pFunc;

                pfBtns.Add(pfb);
            }

            listView.ItemsSource = pfBtns
                .OrderBy(d => d.pFunc.Order)
                .ToList();
        }

        async void OnAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = new pfButtons()
            });
        }

        async void OnHelpClicked(object sender, EventArgs e)
        {
            try
            {
                await Xamarin.Essentials.Browser.OpenAsync("http://www.github.com", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
                lblStatus.Text = "Error " + ex.ToString();
            }
        }

        public async void ButtonClickedSettings(object sender, System.EventArgs e)
        {
            var btn = (ImageButton)sender;
            var pf = btn.CommandParameter as pfButtons;
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = pf
            }); ;
        }
       
        public void ButtonClickedClearLog(object sender, System.EventArgs e) 
        {
            lblStatus.Text = "";
        }
        public async void ButtonClickedCallFunction(object sender, System.EventArgs e)
        {
            if (!IncAppCounter())
            {
                lblStatus.Text = "Exceeded uses. Please buy license";
            }
            

            var btn = (Button)sender;
            btn.IsEnabled = false;
            
            var pf = btn.CommandParameter as pfButtons;

            lblStatus.Text = "Calling " + pf.pFunc.ButtonName;

            if (pf != null)
            {
                try
                {
                    var task = rest.CallParticleFunction(pf.pFunc);
                    var ret = await task;

                    btn.IsEnabled = true;
                    if (!ret.Error)
                    {
                        lblStatus.Text = pf.pFunc.ButtonName +" returned " + ret.Return_value;
                    }
                    else {
                        lblStatus.Text = "Error calling " + pf.pFunc.ButtonName + "\n" + ret.ErrorDetail;
                    }
                    

                }
                catch (Exception exc)
                {
                    btn.IsEnabled = true;
                    lblStatus.Text = "Error:" + exc.ToString();
                    throw;
                }
            }

        }
    }
}
