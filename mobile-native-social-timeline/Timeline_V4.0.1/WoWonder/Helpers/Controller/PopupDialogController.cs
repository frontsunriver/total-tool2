using System;
using Android.App;
using Android.OS;
using Android.Text.Format;
using Android.Widget;

namespace WoWonder.Helpers.Controller
{
    public class PopupDialogController
    {
        public class TimePickerFragment : AndroidX.Fragment.App.DialogFragment, TimePickerDialog.IOnTimeSetListener
        {
            public new static readonly string Tag = "MyTimePickerFragment";
            Action<DateTime> TimeSelectedHandler = delegate { };

            public static TimePickerFragment NewInstance(Action<DateTime> onTimeSelected)
            {
                TimePickerFragment frag = new TimePickerFragment {TimeSelectedHandler = onTimeSelected};
                return frag;
            }

            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                DateTime currentTime = DateTime.Now;
                bool is24HourFormat = DateFormat.Is24HourFormat(Activity);
                TimePickerDialog dialog = new TimePickerDialog(Activity, Resource.Style.MyTimePickerDialogTheme, this, currentTime.Hour, currentTime.Minute, is24HourFormat);
                return dialog;
            }

            public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
            {
                DateTime currentTime = DateTime.Now;
                DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);

                TimeSelectedHandler(selectedTime);
            }
        }

        public class DatePickerFragment : AndroidX.Fragment.App.DialogFragment, DatePickerDialog.IOnDateSetListener
        {
            // TAG can be any string of your choice.
            public new static readonly string Tag = "X:" + typeof(DatePickerFragment).Name.ToUpper();

            // Initialize this value to prevent NullReferenceExceptions.
            Action<DateTime> DateSelectedHandler = delegate { };

            public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
            {
                DatePickerFragment frag = new DatePickerFragment {DateSelectedHandler = onDateSelected};
                return frag;
            }

            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                DateTime currently = DateTime.Now;
                DatePickerDialog dialog = new DatePickerDialog(Activity, Resource.Style.MyTimePickerDialogTheme, this, currently.Year, currently.Month - 1, currently.Day);
                return dialog;
            }

            public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
                DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
                DateSelectedHandler(selectedDate);
            }
        }
    } 
}