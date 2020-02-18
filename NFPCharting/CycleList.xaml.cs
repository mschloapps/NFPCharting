using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NFPCharting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CycleList : ContentPage
    {
        public CycleList()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lst = App.Database.GetNFPCycles();
            if (lst.Count == 0)
            {
                var stk = new StackLayout();
                var lbl = new Label();
                lbl.Text = AppResources.AddCycleButtonLabel;
                lbl.HorizontalOptions = LayoutOptions.Center;
                lbl.VerticalOptions = LayoutOptions.CenterAndExpand;
                lbl.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                stk.Children.Add(lbl);
                this.Content = stk;
            }
            else
            {
                for (var i = 0; i < lst.Count; i++)
                {
                    //var dateTime = DateTime.Parse(lst[i].StartingDate);                    
                    //lst[i].StartingDate = dateTime.ToString("d");
                    if (lst[i].IsCurrentTxt == "Yes") 
                    {
                        lst[i].IsCurrentTxt = AppResources.YesLabel;
                    } else
                    {
                        lst[i].IsCurrentTxt = AppResources.NoLabel;
                    }
                }
                this.Content = listView;
                listView.ItemsSource = lst;
            }
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var cycle = (NFPCycles)e.SelectedItem;
            App.Database.SetCurrentID(cycle.CycleID);
            listView.ItemsSource = null;
            listView.ItemsSource = App.Database.GetNFPCycles();
            var lView = (ListView)sender;
            var lViewParent = (ContentPage)lView.Parent;
            var cpParent = (NavigationPage)lViewParent.Parent;
            var npParent = (TabbedPage)cpParent.Parent;
            var page = (EditDay)npParent.FindByName<ContentPage>("crPage");
            npParent.CurrentPage = page;
        }

        protected async void deleteItem(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert(AppResources.DeleteQLabel, AppResources.DeletePrompt, AppResources.YesLabel, AppResources.NoLabel);
            if (answer == true)
            {
                var menuItem = (MenuItem)sender;
                int del = Convert.ToInt32(menuItem.CommandParameter.ToString());
                bool res = App.Database.DeleteCycleID(del);
                if (res == true)
                {
                    listView.ItemsSource = null;
                    var lst = App.Database.GetNFPCycles();
                    listView.ItemsSource = lst;
                    await DisplayAlert(AppResources.DatabaseUpdateLabel, AppResources.DeleteSuccessLabel, AppResources.OKLabel);

                }
                else
                {
                    await DisplayAlert(AppResources.AlertLabel, AppResources.DeleteErrorLabel, AppResources.OKLabel);
                }
            } else
            {

            }
            
        }

        void AddNewCycle(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AddNewCycle());
        }

        void editItem(object sender, EventArgs args)
        {
            var menuItem = (MenuItem)sender;
            int cid = Convert.ToInt32(menuItem.CommandParameter.ToString());
            Navigation.PushAsync(new EditCycle(cid));
        }
    }
}