using System;
using System.IO;
using Xamarin.Forms;
using ParticleButtons.Models;
using System.Runtime.Serialization.Json;
using System.Linq;

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
            var btns = (pfButtons)BindingContext;

            if (File.Exists(btns.Filename))
            {
                File.Delete(btns.Filename);
            }

            await Navigation.PopAsync();
        }

        async void OnCopyButtonClicked(object sender, EventArgs e)
        {

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
