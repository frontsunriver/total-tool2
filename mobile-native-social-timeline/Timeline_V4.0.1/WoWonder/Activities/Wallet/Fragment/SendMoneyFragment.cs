using System;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using WoWonder.Activities.Contacts;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Requests;
using Exception = System.Exception;

namespace WoWonder.Activities.Wallet.Fragment
{
    public class SendMoneyFragment : AndroidX.Fragment.App.Fragment 
    {
        #region  Variables Basic

        private TextView IconAmount, IconEmail;
        public EditText TxtAmount, TxtEmail;
        private TextView TxtMyBalance;
        private Button BtnContinue;
        public string UserId;
        private string Price;
        private TabbedWalletActivity GlobalContext;

        #endregion

        #region General

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            GlobalContext = (TabbedWalletActivity) Activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view = inflater.Inflate(Resource.Layout.SendMoneyLayout, container, false); 
                return view;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);

                InitComponent(view);
                AddOrRemoveEvent(true);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public override void OnLowMemory()
        {
            try
            {
                GC.Collect(GC.MaxGeneration);
                base.OnLowMemory();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                TxtMyBalance = view.FindViewById<TextView>(Resource.Id.myBalance);

                IconAmount = view.FindViewById<TextView>(Resource.Id.IconAmount);
                TxtAmount = view.FindViewById<EditText>(Resource.Id.AmountEditText);
                IconEmail = view.FindViewById<TextView>(Resource.Id.IconEmail);
                TxtEmail = view.FindViewById<EditText>(Resource.Id.EmailEditText);
                BtnContinue = view.FindViewById<Button>(Resource.Id.ContinueButton);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeRegular, IconAmount, FontAwesomeIcon.MoneyBillWave);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeRegular, IconEmail, FontAwesomeIcon.At);

                Methods.SetColorEditText(TxtEmail, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAmount, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtEmail);

                var userData = ListUtils.MyProfileList?.FirstOrDefault();
                if (userData != null)
                {
                    TxtMyBalance.Text = userData.Wallet;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        BtnContinue.Click += BtnContinueOnClick;
                        TxtEmail.Touch += TxtEmailOnTouch;
                        break;
                    default:
                        BtnContinue.Click -= BtnContinueOnClick;
                        TxtEmail.Touch -= TxtEmailOnTouch;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events
         
        //select user >> Get User id
        private void TxtEmailOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                Intent intent = new Intent(Activity, typeof(SelectContactsActivity));
                Activity.StartActivityForResult(intent, 1202);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Send Amount
        private async void BtnContinueOnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtAmount.Text) || string.IsNullOrWhiteSpace(TxtAmount.Text) || Convert.ToInt32(TxtAmount.Text) == 0)
                {
                    Toast.MakeText(Context, Context.GetText(Resource.String.Lbl_Please_enter_amount), ToastLength.Short)?.Show();
                    return;
                }

                if (string.IsNullOrEmpty(TxtEmail.Text) || string.IsNullOrWhiteSpace(TxtEmail.Text))
                {
                    Toast.MakeText(Context, Context.GetText(Resource.String.Lbl_Please_enter_name_email), ToastLength.Short)?.Show();
                    return;
                }

                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Context, Context.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                    return;
                }
                 
                GlobalContext.TypeOpenPayment = "SendMoneyFragment";
                Price = TxtAmount.Text;

                //Show a progress
                AndHUD.Shared.Show(Activity, GetText(Resource.String.Lbl_Loading));

                var (apiStatus, respond) = await RequestsAsync.Global.SendMoneyWalletAsync(UserId, TxtAmount.Text);
                switch (apiStatus)
                {
                    case 200:
                        TxtAmount.Text = string.Empty;
                        TxtEmail.Text = string.Empty;

                        AndHUD.Shared.Dismiss(Activity);
                        Toast.MakeText(Context, Context.GetText(Resource.String.Lbl_MoneySentSuccessfully), ToastLength.Short)?.Show();
                        break;
                    default:
                        Methods.DisplayAndHudErrorResult(Activity, respond);
                        break;
                }
            }
            catch (Exception exception)
            {
                AndHUD.Shared.Dismiss(Activity);
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion 
    }
}