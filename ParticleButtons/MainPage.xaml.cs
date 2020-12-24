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
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

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
                pfb.Running = false;

                pfBtns.Add(pfb);
            }

            listView.ItemsSource = pfBtns
                .OrderBy(d => d.pFunc.Order)
                .ToList();

            // First time ever launched application
            var firstLaunch = VersionTracking.IsFirstLaunchEver;

            // First time launching current version
            var firstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion;

            // First time launching current build
            var firstLaunchBuild = VersionTracking.IsFirstLaunchForCurrentBuild;

            // Current app version (2.0.0)
            var currentVersion = VersionTracking.CurrentVersion;
        }


        /// <summary>
        /// Add a messagen when we receive a push notification
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            lblStatus.Text = "Push: " + message;

            Device.BeginInvokeOnMainThread(() =>
            {
                if (messages.Children.OfType<Label>().Where(c => c.Text == message).Any())
                {
                    // Do nothing, an identical message already exists
                }
                else
                {
                    Label label = new Label()
                    {
                        Text = message,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    };
                    messages.Children.Add(label);
                }
            });
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
                Crashes.TrackError(new Exception("IncAppCounter", ex));
            }
            return true;
        }

        async void OnAddedClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("OnAddedClicked");
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = new pfButtons()
            });
        }

        async void OnHelpClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("OnHelpClicked");
            try
            {
                await Xamarin.Essentials.Browser.OpenAsync("http://www.github.com", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(new Exception("OnHelpClicked",ex));
                // An unexpected error occured. No browser may be installed on the device.
                lblStatus.Text = "Error " + ex.ToString();
            }
        }

        public async void ButtonClickedSettings(object sender, System.EventArgs e)
        {
            Analytics.TrackEvent("ButtonClickedSettings");
            var btn = (ImageButton)sender;
            var pf = btn.CommandParameter as pfButtons;
            
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = pf
            }); ;
        }
       
        public void ButtonClickedClearLog(object sender, System.EventArgs e) 
        {
            Analytics.TrackEvent("ButtonClickedClearLog");
            lblStatus.Text = "";
        }
        public async void ButtonClickedCallFunction(object sender, System.EventArgs e)
        {
            
            Analytics.TrackEvent("ButtonClickedCallFunction");
            if (!IncAppCounter())
            {
                lblStatus.Text = "Exceeded uses. Please buy license";
            }
            

            var btn = (Button)sender;
            btn.IsEnabled = false;
            
            var actualColor = btn.BackgroundColor;

            var pf = btn.CommandParameter as pfButtons;
            pf.Running = true;

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
                        await btn.ChangeBackgroundColorTo(Color.Green, 550, Easing.CubicOut);
                        await btn.ChangeBackgroundColorTo(actualColor, 500, Easing.SinOut);
                        lblStatus.Text = pf.pFunc.ButtonName +" returned " + ret.Return_value;
                    }
                    else {
                        // Here is the effective use of the smooth background color change animation
                        await btn.ChangeBackgroundColorTo(Color.Red, 550, Easing.CubicOut);
                        await btn.ChangeBackgroundColorTo(actualColor, 500, Easing.SinOut);

                        lblStatus.Text = "Error calling " + pf.pFunc.ButtonName + "\n" + ret.ErrorDetail;
                    }

                }
                catch (Exception exc)
                {
                    Crashes.TrackError(exc);
                    btn.IsEnabled = true;
                    lblStatus.Text = "Error:" + exc.ToString();
                    throw;
                }
                pf.Running = false;
            }

        }
    }
}
