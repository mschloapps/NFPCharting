using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NFPCharting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDayCV : ContentPage
    {
        public int stampSelect = 0;
        public string stampColor = "Red";
        public int img = 0;        

        public int tid;
        public int tdy;

        public EditDayCV(int cyid, int dy)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                update_btn.TextColor = Color.FromRgb(35, 151, 243);
                update_btn.BackgroundColor = Color.Transparent;
                update_btn.BorderWidth = 0;
            }

            tid = cyid;
            tdy = dy;

            var yes_no_list = new List<string>();
            yes_no_list.Add(AppResources.NoLabel);
            yes_no_list.Add(AppResources.YesLabel);
            pk_sel.ItemsSource = yes_no_list;
            int_sel.ItemsSource = yes_no_list;

            var indicator_list = new List<string>();
            indicator_list.Add(AppResources.NALabel);
            indicator_list.Add(AppResources.ZeroIndicator);
            indicator_list.Add(AppResources.TwoIndicator);
            indicator_list.Add(AppResources.TwoWIndicator);
            indicator_list.Add(AppResources.FourIndicator);
            indicator_list.Add(AppResources.SixIndicator);
            indicator_list.Add(AppResources.EightIndicator);
            indicator_list.Add(AppResources.TenIndicator);
            indicator_list.Add(AppResources.TenDLIndicator);
            indicator_list.Add(AppResources.TenSLIndicator);
            indicator_list.Add(AppResources.TenWLIndicator);
            ind1_sel.ItemsSource = indicator_list;

            var menstrual_list = new List<string>();
            menstrual_list.Add(AppResources.NALabel);
            menstrual_list.Add(AppResources.HMenstrual);
            menstrual_list.Add(AppResources.MMenstrual);
            menstrual_list.Add(AppResources.LMenstrual);
            menstrual_list.Add(AppResources.VLMenstrual);
            menstrual_list.Add(AppResources.BMenstrual);
            men_sel.ItemsSource = menstrual_list;

            var color_list = new List<string>();
            color_list.Add(AppResources.NALabel);
            color_list.Add(AppResources.BColor);
            color_list.Add(AppResources.CColor);
            color_list.Add(AppResources.CKColor);
            color_list.Add(AppResources.KColor);
            color_list.Add(AppResources.YColor);
            color_list.Add(AppResources.GColor);
            color_list.Add(AppResources.PColor);
            ind2_sel.ItemsSource = color_list;

            var sensation_list = new List<string>();
            sensation_list.Add(AppResources.NALabel);
            sensation_list.Add(AppResources.LSensation);
            sensation_list.Add(AppResources.GSensation);
            sensation_list.Add(AppResources.PSensation);
            ind3_sel.ItemsSource = sensation_list;

        }

        protected override void OnAppearing()
        {
            var converter = new ColorTypeConverter();
            try
            {                
                var dayData = App.Database.GetDayData(tid, tdy);
                RefreshDayData(dayData);
            }
            catch
            {                
                stampSelect = 0;
                stampColor = "White";
                stamp_view.BackgroundColor = (Color)converter.ConvertFromInvariantString(stampColor);
                stamp_grid.BackgroundColor = (Color)converter.ConvertFromInvariantString(stampColor);
                cycle_day.Text = "";
                int_sel.SelectedIndex = 0;
                freq_sel.SelectedIndex = 0;
                cycle_id.Text = "";
                date_field.Text = "";
                men_sel.SelectedIndex = 0;
                ind1_sel.SelectedIndex = 0;
                ind2_sel.SelectedIndex = 0;
                ind3_sel.SelectedIndex = 0;
                pk_sel.SelectedIndex = 0;
                dcnt_sel.SelectedIndex = 0;
                note_edit.Text = "";
            }
        }

        void RefreshDayData(List<NFPData_1_1_0> dt)
        {
            var converter = new ColorTypeConverter();
            cycle_day.Text = dt[0].DayID.ToString();
            int_sel.SelectedIndex = dt[0].Intercourse;
            freq_sel.SelectedIndex = dt[0].Frequency;
            cycle_id.Text = dt[0].CycleID.ToString();
            date_field.Text = dt[0].Date;
            men_sel.SelectedIndex = dt[0].Menstrual;
            ind1_sel.SelectedIndex = dt[0].Indicator_1;
            ind2_sel.SelectedIndex = dt[0].Indicator_2;
            ind3_sel.SelectedIndex = dt[0].Indicator_3;
            pk_sel.SelectedIndex = dt[0].Peak;
            dcnt_sel.SelectedIndex = dt[0].DayCount;
            note_edit.Text = dt[0].Notes;
            stampSelect = dt[0].StSelect;
            stampColor = dt[0].Color;
            img = dt[0].Image;
            stamp_view.BackgroundColor = (Color)converter.ConvertFromInvariantString(dt[0].Color);
            stamp_grid.BackgroundColor = (Color)converter.ConvertFromInvariantString(dt[0].Color);
            if (dt[0].Image == 1)
            {
                stamp_view.Source = "baby_small.png";
            }
            else
            {
                stamp_view.Source = null;
            }
        }

        void RedView_Clicked(object sender, System.EventArgs e)
        {
            stamp_view.BackgroundColor = Color.Red;
            stamp_grid.BackgroundColor = Color.Red;
            stamp_view.Source = null;
            stampSelect = 1;
            stampColor = "Red";
            img = 0;
        }

        void GreenView_Clicked(object sender, System.EventArgs e)
        {
            stamp_view.BackgroundColor = Color.Green;
            stamp_grid.BackgroundColor = Color.Green;
            stamp_view.Source = null;
            stampSelect = 1;
            stampColor = "Green";
            img = 0;
        }

        void LightGrayView_Clicked(object sender, System.EventArgs e)
        {
            stamp_view.BackgroundColor = Color.LightGray;
            stamp_grid.BackgroundColor = Color.LightGray;
            stamp_view.Source = "baby_small.png";
            stampSelect = 1;
            stampColor = "LightGray";
            img = 1;
        }

        void LightGreenView_Clicked(object sender, System.EventArgs e)
        {
            stamp_view.BackgroundColor = Color.LightGreen;
            stamp_grid.BackgroundColor = Color.LightGreen;
            stamp_view.Source = "baby_small.png";
            stampSelect = 1;
            stampColor = "LightGreen";
            img = 1;
        }

        void YellowView_Clicked(object sender, System.EventArgs e)
        {
            stamp_view.BackgroundColor = Color.Yellow;
            stamp_grid.BackgroundColor = Color.Yellow;
            stamp_view.Source = null;
            stampSelect = 1;
            stampColor = "Yellow";
            img = 0;
        }

        void BabyView_Clicked(object sender, System.EventArgs e)
        {
            stamp_view.Source = "baby_small.png";
            stampSelect = 1;
            img = 1;
        }

        void Menstrual_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GuessStamp();
        }

        void Indicator_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GuessStamp();
        }

        void Frequency_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GuessStamp();
        }

        void Peak_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (pk_sel.SelectedIndex > 0)
            {
                dcnt_sel.SelectedIndex = 0;
            }
            GuessStamp();
        }

        void DayCount_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (dcnt_sel.SelectedIndex > 0)
            {
                pk_sel.SelectedIndex = 0;

            }
            GuessStamp();
        }

        void Intercourse_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GuessStamp();
        }

        void Update_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var dateTime = DateTime.Parse(date_field.Text);

                bool res = App.Database.UpdateNFPData(dateTime.ToString("yyyy-MM-dd"), men_sel.SelectedIndex, ind1_sel.SelectedIndex, ind2_sel.SelectedIndex, ind3_sel.SelectedIndex, freq_sel.SelectedIndex, pk_sel.SelectedIndex, dcnt_sel.SelectedIndex, int_sel.SelectedIndex, note_edit.Text, stampColor, stampSelect, img, Int32.Parse(cycle_day.Text), Int32.Parse(cycle_id.Text));
                if (res == true)
                {
                    DisplayAlert(AppResources.DatabaseUpdateLabel, AppResources.UpdateSuccessLabel, AppResources.OKLabel);                    
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert(AppResources.AlertLabel, AppResources.UpdateErrorLabel, AppResources.OKLabel);
                }
            }
            catch
            {
                DisplayAlert(AppResources.AlertLabel, AppResources.UpdateErrorLabel, AppResources.OKLabel);
            }

        }

        void GuessStamp()
        {

            ind_txt.Text = "";

            try
            {
                if (ind1_sel.SelectedIndex > 0 && ind1_sel.SelectedIndex <= 4)
                {
                    if (men_sel.SelectedIndex > 0)
                    {
                        stamp_view.BackgroundColor = Color.Red;
                        stamp_grid.BackgroundColor = Color.Red;
                        stamp_txt.Text = "";
                        stamp_view.Source = null;
                        img = 0;
                        dcnt_sel.SelectedIndex = 0;
                        pk_sel.SelectedIndex = 0;
                        stampColor = "Red";
                    }
                    else if (dcnt_sel.SelectedIndex > 0)
                    {
                        stamp_view.BackgroundColor = Color.LightGreen;
                        stamp_grid.BackgroundColor = Color.LightGreen;
                        stamp_view.Source = "baby_small.png";
                        img = 1;
                        stamp_txt.Text = dcnt_sel.SelectedIndex.ToString();
                        pk_sel.SelectedIndex = 0;
                        stampColor = "LightGreen";
                    }
                    else
                    {
                        stamp_view.BackgroundColor = Color.Green;
                        stamp_grid.BackgroundColor = Color.Green;
                        stamp_txt.Text = "";
                        stamp_view.Source = null;
                        img = 0;
                        dcnt_sel.SelectedIndex = 0;
                        pk_sel.SelectedIndex = 0;
                        stampColor = "Green";
                    }
                }
                else if (ind1_sel.SelectedIndex > 0 && ind1_sel.SelectedIndex <= 31)
                {
                    if (men_sel.SelectedIndex > 0)
                    {
                        stamp_view.BackgroundColor = Color.Red;
                        stamp_grid.BackgroundColor = Color.Red;
                        stamp_txt.Text = "";
                        stamp_view.Source = null;
                        img = 0;
                        dcnt_sel.SelectedIndex = 0;
                        pk_sel.SelectedIndex = 0;
                        stampColor = "Red";
                    }
                    else
                    {
                        stamp_view.BackgroundColor = Color.LightGray;
                        stamp_grid.BackgroundColor = Color.LightGray;
                        stamp_view.Source = "baby_small.png";
                        stampColor = "LightGray";
                        img = 1;
                        if (pk_sel.SelectedIndex == 1)
                        {
                            stamp_txt.Text = "P";
                        }
                        else if (dcnt_sel.SelectedIndex > 0)
                        {
                            stamp_txt.Text = dcnt_sel.SelectedIndex.ToString();
                        }
                        else
                        {
                            stamp_txt.Text = "";
                        }
                    }
                }
                else
                {
                    if (men_sel.SelectedIndex > 0)
                    {
                        stamp_view.BackgroundColor = Color.Red;
                        stamp_grid.BackgroundColor = Color.Red;
                        stamp_txt.Text = "";
                        stamp_view.Source = null;
                        img = 0;
                        dcnt_sel.SelectedIndex = 0;
                        pk_sel.SelectedIndex = 0;
                        stampColor = "Red";
                    }
                    else
                    {
                        stamp_view.BackgroundColor = Color.White;
                        stamp_grid.BackgroundColor = Color.White;
                        stamp_txt.Text = "";
                        stamp_view.Source = null;
                        img = 0;
                        dcnt_sel.SelectedIndex = 0;
                        pk_sel.SelectedIndex = 0;
                        stampColor = "White";
                    }

                }
                if (men_sel.SelectedIndex > 0)
                {
                    ind_txt.Text += men_sel.SelectedItem.ToString().Split(' ')[0];
                }
                if (ind1_sel.SelectedIndex > 0 && men_sel.SelectedIndex > 0)
                {
                    ind_txt.Text += ";" + ind1_sel.SelectedItem.ToString().Split(' ')[0];
                }
                else if (ind1_sel.SelectedIndex > 0)
                {
                    ind_txt.Text += ind1_sel.SelectedItem.ToString().Split(' ')[0];
                }
                if (ind2_sel.SelectedIndex > 0)
                {
                    ind_txt.Text += ind2_sel.SelectedItem.ToString().Split(' ')[0];
                }
                if (ind3_sel.SelectedIndex > 0)
                {
                    ind_txt.Text += ind3_sel.SelectedItem.ToString().Split(' ')[0];
                }
                if (freq_sel.SelectedIndex > 0)
                {
                    ind_txt.Text += "-" + freq_sel.SelectedItem.ToString();
                }
                if (int_sel.SelectedIndex == 1)
                {
                    ind_txt.Text += "  I";
                }
            }
            catch
            {

            }            

        }
    }
}