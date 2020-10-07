using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DatePicker), typeof(NFPCharting.iOS.Renderers.DatePickerRenderer))]
namespace NFPCharting.iOS.Renderers
{
	class DatePickerRenderer : Xamarin.Forms.Platform.iOS.DatePickerRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				UITextField entry = Control;
				UIDatePicker picker = (UIDatePicker)entry.InputView;
				picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
			}
		}
	}
}