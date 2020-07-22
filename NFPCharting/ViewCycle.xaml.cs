using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NFPCharting
{

    public interface IBaseUrl { string Get(); }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCycle : ContentPage
    {
        string[] Menstruals = { "N/A", "H", "M", "L", "VL", "B" };
        string[] Indicators1 = { "N/A", "0", "2", "2W", "4", "6", "8", "10", "10DL", "10SL", "10WL" };
        string[] Indicators2 = { "N/A", "B", "C", "CK", "K", "Y", "G", "P" };
        string[] Indicators3 = { "N/A", "L", "G", "P" };
        string[] Frequencies = { "N/A", "X1", "X2", "X3", "AD" };
        public int ndisp;

        public ViewCycle(int numDisp)
        {
            InitializeComponent();            
            ndisp = numDisp;

            web_view.Navigating += (s, er) =>
            {
                if (er.Url.Contains("?"))
                {                    
                    var data = er.Url.Split(new[] { "?" }, StringSplitOptions.None)[1];
                    var sdata = data.Split(new[] { "*" }, StringSplitOptions.None);                    
                    var tdy = Convert.ToInt32(sdata[0]);
                    var tid = Convert.ToInt32(sdata[1]);                    
                    var pg = new EditDayCV(tid, tdy);                    
                    Navigation.PushAsync(pg);
                    er.Cancel = true;
                }
            };            

        }

        protected override void OnAppearing()
        {
            var html = genHTML(ndisp);
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @html;
            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            web_view.Source = htmlSource;
        }



        string genHTML(int numDisp)
        {
            var numDispCycles = Int32.Parse(numDisp.ToString());
            var cycles = App.Database.GetNFPCyclesASC(numDispCycles);


            byte[] byteData;

            var assembly = typeof(ViewCycle).GetTypeInfo().Assembly;
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

            //var data = App.Database.GetAllNFPData(numDispCycles);

            int counter = 0;

            for (int i = 0; i < cycles.Count; i++)
            {
                html += "<div style=\"height:95px;width:2700px;border-style:solid;border-width:1px;white-space:nowrap\">";
                var data = App.Database.GetNFPData(cycles[i].CycleID);
                for (int j = 0; j < cycles[i].NumDays; j++)
                {
                    html += "<a style=\"text-decoration:none;color:black\" href =\"local.html?" + (j + 1).ToString() + "*" + cycles[i].CycleID + "\">";
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
                    html += "</div></a>";
                    counter++;
                }
                html += "</div>";                
            }

            html += "</body></html>";

            return html;
        }

    }
}