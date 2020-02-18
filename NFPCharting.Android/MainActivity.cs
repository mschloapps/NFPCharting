using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.Res;
using NFPCharting.Themes;

namespace NFPCharting.Droid
{
    [Activity(Label = "NFPCharting", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            SetAppTheme();
        }

        void SetAppTheme()
        {
            if (Resources.Configuration.UiMode.HasFlag(UiMode.NightYes))
            {
                if (App.AppTheme == "dark")
                    return;

                App.Current.Resources = new DarkTheme();

                App.AppTheme = "dark";
            }
            else
            {
                if (App.AppTheme != "dark")
                    return;
                App.Current.Resources = new LightTheme();
                App.AppTheme = "light";
            }
        }
    }
}