using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Essentials;

[assembly: ExportRenderer(typeof(DatePicker), typeof(NFPCharting.iOS.Renderers.DatePickerRenderer))]
namespace NFPCharting.iOS.Renderers
{
	class DatePickerRenderer : Xamarin.Forms.Platform.iOS.DatePickerRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<DatePicker> e)
		{
			
			base.OnElementChanged(e);
			//Console.WriteLine(Xamarin.Essentials.DeviceInfo.Version);

			if (Control != null)
			{
				UITextField entry = Control;
				UIDatePicker picker = (UIDatePicker)entry.InputView;
				if (DeviceInfo.Version.Major >= 14)
                {
					picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
				}
					
			}			
				
		}
	}
}