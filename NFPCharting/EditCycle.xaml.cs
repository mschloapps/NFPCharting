using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NFPCharting
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditCycle : ContentPage
	{
        public int cycleid;
        public int cndays;
        
        public EditCycle(int cid)
		{
			InitializeComponent ();

            if (Device.RuntimePlatform == Device.Android)
            {
                modifyCycleBtn.TextColor = Color.FromRgb(35, 151, 243);
                modifyCycleBtn.BackgroundColor = Color.Transparent;
                modifyCycleBtn.BorderWidth = 0;                
            }

            cycleid = cid;
            cndays = App.Database.GetNumDays(cycleid);
            cur_numdays.Text = cndays.ToString();
            numdaysEntry.Text = cndays.ToString();
        }

        async private void modifyCycleBtn_Clicked(object sender, EventArgs e)
        {            
            var ndys = Convert.ToInt32(Math.Round(Double.TryParse(numdaysEntry.Text, out double dndys) ? dndys : 0));
            ;
            if (ndys > 365)
            {
                await DisplayAlert(AppResources.AlertLabel, AppResources.TooManyDaysLabel, AppResources.OKLabel);
            } else if (ndys < 1)
            {
                await DisplayAlert(AppResources.AlertLabel, AppResources.TooFewDaysLabel, AppResources.OKLabel);
            } else
            {
                bool res = App.Database.ModifyCycle(cycleid, ndys);
                if (res == true)
                {
                    await DisplayAlert(AppResources.DatabaseUpdateLabel, AppResources.CycleModifiedLabel, AppResources.OKLabel);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert(AppResources.AlertLabel, AppResources.CycleModErrorLabel, AppResources.OKLabel);
                }
            }            

        }
    }
}