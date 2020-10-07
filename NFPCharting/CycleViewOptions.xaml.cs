using System;
using System.IO;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace NFPCharting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CycleViewOptions : ContentPage
    {
        string[] Menstruals = { "N/A", "H", "M", "L", "VL", "B" };
        string[] Indicators1 = { "N/A", "0", "2", "2W", "4", "6", "8", "10", "10DL", "10SL", "10WL" };
        string[] Indicators2 = { "N/A", "B", "C", "CK", "K", "Y", "G", "P" };
        string[] Indicators3 = { "N/A", "L", "G", "P" };
        string[] Frequencies = { "N/A", "X1", "X2", "X3", "AD" };

        public CycleViewOptions()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                show_btn.TextColor = Color.FromRgb(35, 151, 243);
                show_btn.BackgroundColor = Color.Transparent;
                show_btn.BorderWidth = 0;
                send_btn.TextColor = Color.FromRgb(35, 151, 243);
                send_btn.BackgroundColor = Color.Transparent;
                send_btn.BorderWidth = 0;
            }

            numCyclesToDisplay.SelectedIndex = 0;
        }

        void Show_Clicked(object sender, System.EventArgs e)
        {            
            Navigation.PushAsync(new ViewCycle(Convert.ToInt32(numCyclesToDisplay.SelectedItem)));
        }

        string genHTML()
        {
            var numDispCycles = Int32.Parse(numCyclesToDisplay.SelectedItem.ToString());
            var cycles = App.Database.GetNFPCyclesASC(numDispCycles);


            byte[] byteData;

            var assembly = typeof(CycleViewOptions).GetTypeInfo().Assembly;
            //var assembly = typeof(EmbeddedImages).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames())
            //{
            //    System.Diagnostics.Debug.WriteLine("found resource: " + res);
            //}

            if (Device.RuntimePlatform == Device.Android)
            {
                using (Stream stream = assembly.GetManifestResourceStream("NFPCharting.baby.png"))
                {
                    long length = stream.Length;
                    byteData = new byte[length];
                    stream.Read(byteData, 0, (int)length);
                }
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                using (Stream stream = assembly.GetManifestResourceStream("NFPCharting.baby.png"))
                {
                    long length = stream.Length;
                    byteData = new byte[length];
                    stream.Read(byteData, 0, (int)length);
                }
            }
            else
            {
                byteData = File.ReadAllBytes("baby_small.png");
            }

            string img = Convert.ToBase64String(byteData, 0, byteData.Length);
            string imgURL = "data:image/png;base64," + img;

            string html = "<!DOCTYPE html><html><body>";
                        
            for (int i = 0; i < cycles.Count; i++)
            {
                html += "<div style=\"height:95px;width:2700px;border-style:solid;border-width:1px;white-space:nowrap\">";
                var data = App.Database.GetNFPData(cycles[i].CycleID);
                for (int j = 0; j < cycles[i].NumDays; j++)
                {
                    html += "<div style=\"width:65px;height:95px;border-style:solid;border-width:1px;float:left;background-color:" + data[j].Color;
                    if (data[j].Image == 1)
                    {
                        html += ";background-repeat:no-repeat;background-position:center;background-size:35px 55px;";
                        html += "background-image:url('" + imgURL + "')";
                    }
                    html += "\" title=\"" + data[j].Notes + "\">";                    
                    html += "<div style=\"text-align:center;font-size:9px;\">" + data[j].DayID + "</div>";

                    //var dateTime = DateTime.Parse(data[j].Date);
                    //var date_field = dateTime.ToString("d");

                    html += "<div style=\"text-align:center;font-size:9px;\">" + data[j].Date + "</div>";
                    html += "<div style=\"text-align:center;font-size:50px;height:60px;\">";
                    if (data[j].Peak == 1)
                    {
                        html += "P";
                    }
                    else if (data[j].DayCount > 0)
                    {
                        html += data[j].DayCount.ToString();
                    }
                    else
                    {
                        html += "";
                    }
                    html += "</div>";

                    html += "<div style=\"text-align:center;font-size:9px;\">";
                    if (data[j].Menstrual > 0)
                    {
                        html += Menstruals[data[j].Menstrual];
                    }
                    if (data[j].Indicator_1 > 0 && data[j].Menstrual > 0)
                    {
                        html += ";" + Indicators1[data[j].Indicator_1];
                    }
                    else if (data[j].Indicator_1 > 0)
                    {
                        html += Indicators1[data[j].Indicator_1];
                    }
                    if (data[j].Indicator_2 > 0)
                    {
                        html += Indicators2[data[j].Indicator_2];
                    }
                    if (data[j].Indicator_3 > 0)
                    {
                        html += Indicators3[data[j].Indicator_3];
                    }
                    if (data[j].Frequency > 0)
                    {
                        html += "-" + Frequencies[data[j].Frequency];
                    }

                    if (data[j].Intercourse == 1)
                    {
                        html += "<span style=\"font-size:12px;font-weight:bold;float:right;padding-right:10px;\">I</span>";
                    }
                    html += "</div>";
                    html += "</div>";                    

                }
                html += "</div>";
            }

            html += "</body></html>";

            return html;
        }

        /*
        void Send_Clicked(object sender, System.EventArgs e)
        {

            string html = genHTML();

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "chart.html");
            File.WriteAllText(fileName, html);

            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                var email = new EmailMessageBuilder()
                  //.To("to@service.com")
                  //.Cc("cc.plugins@xamarin.com")
                  //.Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
                  .Subject(AppResources.EmailSubjectLabel)
                    .Body(AppResources.EmailBodyLabel)
                  .WithAttachment(fileName, "text/html")
                  .Build();

                emailMessenger.SendEmail(email);
            }
            else
            {
                DisplayAlert(AppResources.AlertLabel, AppResources.SendErrorLabel, AppResources.OKLabel);
            }
        }
        */

        async void Share_Clicked(object sender, System.EventArgs e)
        {
            string html = genHTML();

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "chart.html");
            File.WriteAllText(fileName, html);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Title,
                File = new ShareFile(fileName, "text/html")                
            });

        }
    }
}