using System;
using System.IO;
using Xamarin.Forms;
using ParticleButtons.Models;
using System.Linq;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace ParticleButtons
{
    public partial class SettingsPage : ContentPage
    {
        

        public SettingsPage()
        {
            InitializeComponent();
             
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var pfb = (pfButtons)BindingContext;
            if (!pfb.pFunc.Saved) { btnDelete.IsEnabled = false;btnCopy.IsEnabled = false; }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("OnSaveButtonClicked");
            string filename;
            
            var pfbut = (pfButtons)BindingContext;

            if (string.IsNullOrWhiteSpace(pfbut.Filename))
            {
                filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.pfbuttons.txt");   
            } 
            else
            {
                filename = pfbut.Filename;
            }

            pfbut.pFunc.Saved = true;
            var json = pfbut.pFunc.ToJson();
            
            File.WriteAllText(filename, json);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("OnDeleteButtonClicked");
            var btns = (pfButtons)BindingContext;

            if (File.Exists(btns.Filename))
            {
                File.Delete(btns.Filename);
            }

            await Navigation.PopAsync();
        }

        async void OnCopyButtonClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("OnCopyButtonClicked");

            var pfbut = (pfButtons)BindingContext;
            pfbut.Filename = String.Empty;
            pfbut.Text = string.Empty;
            pfbut.pFunc.ButtonName = pfbut.pFunc.ButtonName + " Copy";
            pfbut.pFunc.Saved = false;
            pfbut.pFunc.Order = pfbut.pFunc.Order+1;

            Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault());
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = pfbut
            });
        }
    }
}
