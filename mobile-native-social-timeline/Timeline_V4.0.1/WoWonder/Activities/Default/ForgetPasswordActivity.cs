using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.App;
using Com.EightbitLab.BlurViewBinding;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;

namespace WoWonder.Activities.Default
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class ForgetPasswordActivity : AppCompatActivity
    {
        #region Variables Basic

        private RelativeLayout EmailLayout;
        private EditText TxtEmail;
        private TextView TxtEmailRequired;
        private Button BtnSend;   
        private BlurView BlurView;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Window?.SetSoftInputMode(SoftInput.AdjustResize);

                Methods.App.FullScreenApp(this, true);

                // Create your application here
                SetContentView(Resource.Layout.ForgetPassword_Layout);

                //Get Value And Set Toolbar
                InitComponent();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                AddOrRemoveEvent(true);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
                AddOrRemoveEvent(false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            try
            {

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                base.OnTrimMemory(level);
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

        protected override void OnDestroy()
        {
            try
            {
                DestroyBasic();
                base.OnDestroy();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                EmailLayout = FindViewById<RelativeLayout>(Resource.Id.rl_email);
                TxtEmail = FindViewById<EditText>(Resource.Id.etEmail);
                TxtEmailRequired = FindViewById<TextView>(Resource.Id.tv_email_required);
                
                BtnSend = FindViewById<Button>(Resource.Id.btn_request_password);
                  
                BlurView = FindViewById<BlurView>(Resource.Id.bv_forgot_password);
                BlurBackground(BlurView, 10f); 
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
                // true +=  // false -=
                if (addEvent)
                {
                    BtnSend.Click += BtnSendOnClick; 
                    TxtEmail.TextChanged += TxtEmailOnTextChanged;
                }
                else
                {
                    BtnSend.Click -= BtnSendOnClick; 
                    TxtEmail.TextChanged -= TxtEmailOnTextChanged;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void DestroyBasic()
        {
            try
            {
                TxtEmail = null!;  
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events
          
        //send 
        private async void BtnSendOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_CheckYourInternetConnection), GetText(Resource.String.Lbl_Ok));
                    return;
                }

                if (string.IsNullOrEmpty(TxtEmail.Text.Replace(" ", "")))
                {
                    SetHighLight(true, EmailLayout, TxtEmail, TxtEmailRequired);
                    return;
                }
                else
                {
                    SetHighLight(false, EmailLayout, TxtEmail, TxtEmailRequired);
                }
                 
                HideKeyboard();

                ToggleVisibility(true);

                var (apiStatus, respond) = await RequestsAsync.Auth.ResetPasswordEmailAsync(TxtEmail.Text.Replace(" ", ""));
                if (apiStatus == 200 && respond is StatusObject auth)
                {
                    ToggleVisibility(false);
                    Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_Email_Has_Been_Send), GetText(Resource.String.Lbl_Ok));
                }
                else if (apiStatus == 400)
                {
                    if (respond is ErrorObject error)
                    {
                        ToggleVisibility(false);
                        var errorText = error.Error.ErrorText;
                        Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), errorText, GetText(Resource.String.Lbl_Ok));
                    }
                }
                else
                {
                    ToggleVisibility(false);
                    Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), respond.ToString(), GetText(Resource.String.Lbl_Ok));
                }
            }
            catch (Exception exception)
            {
                ToggleVisibility(false);
                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), exception.Message, GetText(Resource.String.Lbl_Ok));
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void TxtEmailOnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TxtEmail.Text.Length > 0)
                    SetHighLight(false, EmailLayout, TxtEmail, TxtEmailRequired);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion
  
        private void HideKeyboard()
        {
            try
            {
                var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
                inputManager?.HideSoftInputFromWindow(CurrentFocus?.WindowToken, HideSoftInputFlags.None);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void ToggleVisibility(bool isLoginProgress)
        {
            try
            {
                if (isLoginProgress)
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));
                else
                    AndHUD.Shared.Dismiss(this);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
           
        private void BlurBackground(BlurView blurView, float radius)
        {
            try
            {
                //float radius = 10f; 
                View decorView = Window.DecorView;
                //ViewGroup you want to start blur from. Choose root as close to BlurView in hierarchy as possible.
                ViewGroup rootView = decorView.FindViewById<ViewGroup>(Android.Resource.Id.Content);
                //Set drawable to draw in the beginning of each blurred frame (Optional). 
                //Can be used in case your layout has a lot of transparent space and your content
                //gets kinda lost after after blur is applied.
                Drawable windowBackground = decorView.Background;

                blurView.SetupWith(rootView)
                    .SetFrameClearDrawable(windowBackground)
                    .SetBlurAlgorithm(new RenderScriptBlur(this))
                    .SetBlurRadius(radius)
                    .SetBlurAutoUpdate(true)
                    .SetHasFixedTransformationMatrix(true);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void SetHighLight(bool state, RelativeLayout layout, EditText editText, TextView textView)
        {
            try
            {
                Color txtcolor, borderColor;
                if (state)
                {
                    textView.Visibility = ViewStates.Visible;
                    txtcolor = new Color(GetColor(Resource.Color.colorLoginHighlightText));
                    borderColor = new Color(GetColor(Resource.Color.colorLoginHighlightText));
                }
                else
                {
                    textView.Visibility = ViewStates.Gone;
                    txtcolor = new Color(GetColor(Resource.Color.gnt_white));
                    borderColor = new Color(GetColor(Resource.Color.transparent_border));
                }
                editText.SetHintTextColor(txtcolor);
                layout.Background.SetTint(borderColor);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}