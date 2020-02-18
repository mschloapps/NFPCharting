using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Globalization;
using System.Linq;
using Plugin.Multilingual;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NFPCharting
{
    public partial class App : Application
    {
        static SQLiteDB_1_1_0 database_1_1_0;
        public static List<NFPCycles> CurCycle { get; set; }
        //public static List<NFPData_1_1_0> CurData { get; set; }
        //public static int numDispDays = 4;
        //public static int lastCRPage = 0;
        //public static int nextCRPage = 2;
        //public static int curEditDay = 1;


        public App()
        {
            InitializeComponent();
            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;
            MainPage = new MainPage();
        }

        public static SQLiteDB_1_1_0 Database
        {
            get
            {
                if (database_1_1_0 == null)
                {
                    var olddbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NFPChart.db");

                    var importdbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "import.db");
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NFPChart_1_1_0.db");

                    if (File.Exists(olddbPath))
                    {
                        // migrate
                        var oldDB = new SQLiteDB(olddbPath);
                        database_1_1_0 = new SQLiteDB_1_1_0(dbPath);
                        var oldCycles = oldDB.GetNFPCyclesASC();
                        var oldData = oldDB.GetAllNFPData();
                        bool migrate_success;
                        migrate_success = database_1_1_0.MigrateData(oldCycles, oldData);
                        if (migrate_success)
                        {
                            File.Delete(olddbPath);
                        }
                    }
                    else if (File.Exists(importdbPath))
                    {
                        var delPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        var dir = new DirectoryInfo(delPath);
                        foreach (var file in dir.EnumerateFiles("NFPChart_1_1_0.*"))
                        {
                            file.Delete();
                        }
                        File.Move(importdbPath, dbPath);

                        database_1_1_0 = new SQLiteDB_1_1_0(dbPath);
                    }
                    else
                    {
                        database_1_1_0 = new SQLiteDB_1_1_0(dbPath);
                    }
                }
                return database_1_1_0;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            CurCycle = Database.GetCurrentCycle();
            if (CurCycle.Count > 0)
            {                
                var page = MainPage.FindByName<ContentPage>("crPage");                
                var tabbedPage = MainPage.FindByName<TabbedPage>("Tabs");
                tabbedPage.CurrentPage = page;
            }
            else
            {
                var page = MainPage.FindByName<NavigationPage>("cycleListViews");
                var tabbedPage = MainPage.FindByName<TabbedPage>("Tabs");
                tabbedPage.CurrentPage = page;
            }

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            /*
            CurCycle = Database.GetCurrentCycle();
            if (CurCycle.Count > 0)
            {
                CurData = Database.GetNFPData(CurCycle[0].CycleID);
            }
            */
        }

        public static string AppTheme
        {
            get; set;
        }

    }
}
