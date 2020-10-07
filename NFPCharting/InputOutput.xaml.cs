using System;
using System.Collections.Generic;
using System.IO;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace NFPCharting
{
    public partial class InputOutput : ContentPage
    {
        public InputOutput()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                import_btn.TextColor = Color.FromRgb(35, 151, 243);
                import_btn.BackgroundColor = Color.Transparent;
                import_btn.BorderWidth = 0;
                export_btn.TextColor = Color.FromRgb(35, 151, 243);
                export_btn.BackgroundColor = Color.Transparent;
                export_btn.BorderWidth = 0;
            }

        }

        /*
        void Export_Clicked(object sender, System.EventArgs e)
        {


            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NFPChart_1_1_0.db");
            string expFile = "NFPChart_" + DateTime.Today.ToString("yyyy-MM-dd") + ".db";
            string expfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), expFile);

            var dbdata = File.ReadAllBytes(fileName);
            File.WriteAllBytes(expfileName, dbdata);

            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                var email = new EmailMessageBuilder()
                  //.To("to@service.com")
                  //.Cc("cc.plugins@xamarin.com")
                  //.Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
                  .Subject(AppResources.ExportSubjectLabel)
                    .Body(AppResources.ExportBodyLabel)
                    .WithAttachment(expfileName, "application/octet-stream")
                  .Build();

                emailMessenger.SendEmail(email);
            }
            else
            {
                DisplayAlert(AppResources.AlertLabel, AppResources.SendErrorLabel, AppResources.OKLabel);
            }
        }
        */


        async void Export_Clicked(object sender, System.EventArgs e)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NFPChart_1_1_0.db");
            string expFile = "NFPChart_" + DateTime.Today.ToString("yyyy-MM-dd") + ".db";
            string expfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), expFile);

            var dbdata = File.ReadAllBytes(fileName);
            File.WriteAllBytes(expfileName, dbdata);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Title,
                //File = new ShareFile(expfileName, "application/x-sqlite3")
                File = new ShareFile(expfileName, "application/vnd.sqlite3")
            });
        }

        async void Import_Clicked(object sender, System.EventArgs e)
        {
            //string[] ftypes = { "application/x-sqlite3" };
            try
            {
                var openfileData = await CrossFilePicker.Current.PickFile();
                if (openfileData == null) {
                    return; // user canceled file picking
                }

                //string openfileName = openfileData.FileName;
                //string openfilePath = openfileData.FilePath;
                string outputfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "import.db");
                byte[] dbdata = openfileData.DataArray;
                File.WriteAllBytes(outputfileName, dbdata);
                try
                {
                    var testDB = new SQLiteDB_1_1_0(outputfileName);
                    await DisplayAlert(AppResources.AlertLabel, AppResources.ImportNote, AppResources.OKLabel);
                }
                catch(Exception ex)
                {
                    File.Delete(outputfileName);
                    await DisplayAlert(AppResources.AlertLabel, AppResources.ImportErrorDBLabel, AppResources.OKLabel);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                if (ex.GetType() == typeof(System.InvalidOperationException))
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await DisplayAlert(AppResources.AlertLabel, AppResources.ImportErrorPermissionsAndroid, AppResources.OKLabel);
                    }
                    else if (Device.RuntimePlatform == Device.iOS)
                    {
                        await DisplayAlert(AppResources.AlertLabel, AppResources.ImportErrorPermissionsApple, AppResources.OKLabel);
                    }
                    else 
                    {
                        await DisplayAlert(AppResources.AlertLabel, AppResources.ImportErrorLabel, AppResources.OKLabel);
                    }
                }
                else 
                {
                    await DisplayAlert(AppResources.AlertLabel, AppResources.ImportErrorLabel, AppResources.OKLabel);
                }
            }

        }

    }
}
