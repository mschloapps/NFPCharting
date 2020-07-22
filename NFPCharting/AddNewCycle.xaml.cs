using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NFPCharting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewCycle : ContentPage
    {
        public AddNewCycle()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {                
                addCycleBtn.TextColor = Color.FromRgb(35, 151, 243);
                addCycleBtn.BackgroundColor = Color.Transparent;
                addCycleBtn.BorderWidth = 0;
            }

            cydatePicker.Date = DateTime.Today;
            numdaysEntry.Text = "32";
        }

        async private void addCycleBtn_Clicked(object sender, EventArgs e)
        {
            var ndys = Convert.ToInt32(Math.Round(Double.TryParse(numdaysEntry.Text, out double dndys) ? dndys : 0));
            if (ndys > 365)
            {
                await DisplayAlert(AppResources.AlertLabel, AppResources.TooManyDaysLabel, AppResources.OKLabel);
            }
            else if (ndys < 1)
            {
                await DisplayAlert(AppResources.AlertLabel, AppResources.TooFewDaysLabel, AppResources.OKLabel);
            }
            else
            {
                bool res = App.Database.InsertCycle(cydatePicker.Date.ToString("yyyy-MM-dd"), ndys);
                if (res == true)
                {
                    await DisplayAlert(AppResources.DatabaseUpdateLabel, AppResources.AddCycleSuccessLabel, AppResources.OKLabel);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert(AppResources.AlertLabel, AppResources.AddCycleErrorLabel, AppResources.OKLabel);
                }
            }

        }
    }
}