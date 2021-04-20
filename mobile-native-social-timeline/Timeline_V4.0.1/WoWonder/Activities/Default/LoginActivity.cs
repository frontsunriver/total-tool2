using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.App;
using Com.EightbitLab.BlurViewBinding;
using Newtonsoft.Json;
using Org.Json;
using WoWonder.Activities.General;
using WoWonder.Activities.Tabbes;
using WoWonder.Activities.WalkTroutPage;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.SocialLogins;
using WoWonder.Helpers.Utils;
using WoWonder.Library.OneSignal;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

namespace WoWonder.Activities.Default
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class LoginActivity : AppCompatActivity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
    {
        #region Variables Basic

        private RelativeLayout EmailLayout, PasswordLayout;
        private EditText TxtEmail, TxtPassword;
        private TextView TxtForgotPassword, TxtEmailRequired, TxtPasswordRequired;
        private Button BtnLogin, BtnGoogle, BtnFacebook, ContinueButton;
        private ImageView ImageShowPass;
        private ProgressBar ProgressBar;
        private LinearLayout LayoutCreateAccount;

        private string TimeZone = "";
        private bool IsActiveUser = true;

        private FbMyProfileTracker MprofileTracker;
        private ICallbackManager MFbCallManager;
        public static GoogleSignInClient MGoogleSignInClient;

        private BlurView BlurView;
        private DataTables.LoginTb LoginTb;

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
                SetContentView(Resource.Layout.Login_Layout);

                //Get Value And Set Toolbar
                InitComponent();
                InitSocialLogins(); 
                GetTimezone();

                if ((int) Build.VERSION.SdkInt is < 23)
                {
                    LoadConfigSettings();
                    CheckCrossAppAuthentication();
                }
                else
                {
                    if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted &&
                        CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                    {
                        LoadConfigSettings();
                        CheckCrossAppAuthentication();
                    }
                    else
                    {
                        RequestPermissions(new[]
                        {
                            Manifest.Permission.ReadExternalStorage,
                            Manifest.Permission.WriteExternalStorage
                        }, 101);
                    }
                }

                if (string.IsNullOrEmpty(UserDetails.DeviceId))
                    OneSignalNotification.RegisterNotificationDevice(); 
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
                EmailLayout = FindViewById<RelativeLayout>(Resource.Id.rl_login_email);
                TxtEmail = FindViewById<EditText>(Resource.Id.etEmail);
                TxtEmailRequired = FindViewById<TextView>(Resource.Id.tv_email_required);

                PasswordLayout = FindViewById<RelativeLayout>(Resource.Id.rl_login_password);
                TxtPassword = FindViewById<EditText>(Resource.Id.etPassword);
                TxtPasswordRequired = FindViewById<TextView>(Resource.Id.tv_password_required);

                BtnLogin = FindViewById<Button>(Resource.Id.btn_login);
                TxtForgotPassword = FindViewById<TextView>(Resource.Id.textForgotPassword);

                ImageShowPass = FindViewById<ImageView>(Resource.Id.imageShowPass);
                ProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

                ContinueButton = FindViewById<Button>(Resource.Id.btn_continue);
                ContinueButton.Visibility = ViewStates.Gone;

                LayoutCreateAccount = FindViewById<LinearLayout>(Resource.Id.layout_create_account);
                 
                BlurView = FindViewById<BlurView>(Resource.Id.bv_login);
                BlurBackground(BlurView, 10f);

                LayoutCreateAccount.Visibility = AppSettings.EnableRegisterSystem == false ? ViewStates.Gone : ViewStates.Visible;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InitSocialLogins()
        {
            try
            {
                //#Facebook
                if (AppSettings.ShowFacebookLogin)
                {
                    LoginButton loginButton = new LoginButton(this);
                    MprofileTracker = new FbMyProfileTracker();
                    MprofileTracker.StartTracking();

                    BtnFacebook = FindViewById<Button>(Resource.Id.btn_facebook); 
                    BtnFacebook.Visibility = ViewStates.Visible;
                    BtnFacebook.Click += BtnFacebookOnClick;

                    MprofileTracker.MOnProfileChanged += MprofileTrackerOnMOnProfileChanged;
                    loginButton.SetPermissions(new List<string>
                    {
                        "email",
                        "public_profile"
                    });

                    MFbCallManager = CallbackManagerFactory.Create();
                    loginButton.RegisterCallback(MFbCallManager, this);

                    //FB accessToken
                    var accessToken = AccessToken.CurrentAccessToken;
                    var isLoggedIn = accessToken != null && !accessToken.IsExpired;
                    if (isLoggedIn && Profile.CurrentProfile != null)
                    {
                        LoginManager.Instance.LogOut();
                    }

                    string hashId = Methods.App.GetKeyHashesConfigured(this);
                    Console.WriteLine(hashId);
                }
                else
                {
                    BtnFacebook = FindViewById<Button>(Resource.Id.btn_facebook); 
                    BtnFacebook.Visibility = ViewStates.Gone;
                }

                //#Google
                if (AppSettings.ShowGoogleLogin)
                {
                    // Configure sign-in to request the user's ID, email address, and basic profile. ID and basic profile are included in DEFAULT_SIGN_IN.
                    var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                        .RequestIdToken(AppSettings.ClientId)
                        .RequestScopes(new Scope(Scopes.Profile))
                        .RequestScopes(new Scope(Scopes.PlusMe))
                        .RequestScopes(new Scope(Scopes.DriveAppfolder))
                        .RequestServerAuthCode(AppSettings.ClientId)
                        .RequestProfile().RequestEmail().Build();

                    MGoogleSignInClient = GoogleSignIn.GetClient(this, gso);

                    BtnGoogle = FindViewById<Button>(Resource.Id.btn_google);
                    BtnGoogle.Click += GoogleSignInButtonOnClick;
                }
                else
                {
                    BtnGoogle = FindViewById<Button>(Resource.Id.btn_google);
                    BtnGoogle.Visibility = ViewStates.Gone;
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
                // true +=  // false -=
                if (addEvent)
                {
                    BtnLogin.Click += BtnLoginOnClick;
                    TxtForgotPassword.Click += TxtForgotPasswordOnClick;
                    ImageShowPass.Touch += ImageShowPassOnTouch;
                    LayoutCreateAccount.Click += LayoutCreateAccountOnClick;
                    TxtEmail.TextChanged += TxtEmailOnTextChanged;
                    TxtPassword.TextChanged += TxtPasswordOnTextChanged;
                    ContinueButton.Click += ContinueButtonOnClick;
                }
                else
                {
                    BtnLogin.Click -= BtnLoginOnClick;
                    TxtForgotPassword.Click -= TxtForgotPasswordOnClick;
                    ImageShowPass.Touch -= ImageShowPassOnTouch;
                    LayoutCreateAccount.Click -= LayoutCreateAccountOnClick;
                    TxtEmail.TextChanged -= TxtEmailOnTextChanged;
                    TxtPassword.TextChanged -= TxtPasswordOnTextChanged;
                    ContinueButton.Click -= ContinueButtonOnClick;
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
                TxtPassword = null!;
                TxtForgotPassword = null!;
                BtnLogin = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        //Continue as
        private void ContinueButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                CrossAppAuthentication();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Show Password 
        private void ImageShowPassOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                switch (e.Event?.Action)
                {
                    case MotionEventActions.Up: // hide password
                        TxtPassword.TransformationMethod = PasswordTransformationMethod.Instance;
                        ImageShowPass.SetImageResource(Resource.Drawable.ic_eye_hide);
                        break;
                    case MotionEventActions.Down: // show password
                        TxtPassword.TransformationMethod = HideReturnsTransformationMethod.Instance;
                        ImageShowPass.SetImageResource(Resource.Drawable.icon_eye);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Forgot Password
        private void TxtForgotPasswordOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(this, typeof(ForgetPasswordActivity)));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //start login 
        private async void BtnLoginOnClick(object sender, EventArgs e)
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
                
                if (string.IsNullOrEmpty(TxtPassword.Text))
                {
                    SetHighLight(true, PasswordLayout, TxtPassword, TxtPasswordRequired);
                    return;
                }
                else
                {
                    SetHighLight(false, PasswordLayout, TxtPassword, TxtPasswordRequired);
                }

                HideKeyboard();

                ToggleVisibility(true);

                var (apiStatus, respond) = await RequestsAsync.Auth.AuthAsync(TxtEmail.Text.Replace(" ", ""), TxtPassword.Text, TimeZone, UserDetails.DeviceId);
                if (apiStatus == 200 && respond is AuthObject auth)
                {
                    var emailValidation = ListUtils.SettingsSiteList?.EmailValidation ?? "0";
                    IsActiveUser = emailValidation switch
                    {
                        "1" => await CheckIsActiveUser(auth.UserId),
                        _ => IsActiveUser
                    };
                     
                    switch (IsActiveUser)
                    {
                        case true:
                            {
                                SetDataLogin(auth);

                                if (auth.Membership != null && auth.Membership.Value)
                                {
                                    var intent = new Intent(this, typeof(GoProActivity));
                                    intent.PutExtra("class", "login");
                                    StartActivity(intent);
                                }
                                else
                                {
                                    switch (AppSettings.ShowWalkTroutPage)
                                    {
                                        case true:
                                            {
                                                Intent newIntent = new Intent(this, typeof(AppIntroWalkTroutPage));
                                                newIntent?.PutExtra("class", "login");
                                                StartActivity(newIntent);
                                                break;
                                            }
                                        default:
                                            StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                                            break;
                                    }
                                }

                                ToggleVisibility(false);
                                FinishAffinity();
                                break;
                            }
                        default:
                            ToggleVisibility(false);
                            break;
                    } 
                }
                else if (apiStatus == 200 && respond is AuthMessageObject messageObject)
                {
                    ToggleVisibility(false);

                    UserDetails.Username = TxtEmail.Text;
                    UserDetails.FullName = TxtEmail.Text;
                    UserDetails.Password = TxtPassword.Text;
                    UserDetails.UserId = messageObject.UserId;
                    UserDetails.Status = "Pending";
                    UserDetails.Email = TxtEmail.Text;

                    //Insert user data to database
                    var user = new DataTables.LoginTb
                    {
                        UserId = UserDetails.UserId,
                        AccessToken = "",
                        Cookie = "",
                        Username = TxtEmail.Text,
                        Password = TxtPassword.Text,
                        Status = "Pending",
                        Lang = "",
                        DeviceId = UserDetails.DeviceId,
                    };
                    ListUtils.DataUserLoginList.Add(user);

                    var dbDatabase = new SqLiteDatabase();
                    dbDatabase.InsertOrUpdateLogin_Credentials(user);
                     
                    Intent newIntent = new Intent(this, typeof(VerificationCodeActivity));
                    newIntent?.PutExtra("TypeCode", "TwoFactor");
                    StartActivity(newIntent);

                }
                else if (apiStatus == 400)
                {
                    if (respond is ErrorObject error)
                    {
                        ToggleVisibility(false);

                        var errorText = error.Error.ErrorText;
                        var errorId = error.Error.ErrorId;
                        switch (errorId)
                        {
                            case "3":
                                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security),
                                    GetText(Resource.String.Lbl_ErrorLogin_3), GetText(Resource.String.Lbl_Ok));
                                break;
                            case "4":
                                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security),
                                    GetText(Resource.String.Lbl_ErrorLogin_4), GetText(Resource.String.Lbl_Ok));
                                break;
                            case "5":
                                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security),
                                    GetText(Resource.String.Lbl_ErrorLogin_5), GetText(Resource.String.Lbl_Ok));
                                break;
                            default:
                                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security),
                                    errorText, GetText(Resource.String.Lbl_Ok));
                                break;
                        }
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

        //CreateAccount
        private void LayoutCreateAccountOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(this, typeof(SelectRegisterActivity))); 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtPasswordOnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TxtPassword.Text.Length > 0)
                    SetHighLight(false, PasswordLayout, TxtPassword, TxtPasswordRequired);
            }
            catch (Exception exception)
            {
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

        #region Social Logins

        private string FbAccessToken, GAccessToken, GServerCode;

        #region Facebook

        private void BtnFacebookOnClick(object sender, EventArgs e)
        {
            try
            {
                LoginManager.Instance.LogInWithReadPermissions(this, new List<string>
                {
                    "email",
                    "public_profile"
                });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnCancel()
        {
            try
            {
                ToggleVisibility(false);

                SetResult(Result.Canceled);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnError(FacebookException error)
        {
            try
            {

                ToggleVisibility(false);

                // Handle exception
                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), error.Message, GetText(Resource.String.Lbl_Ok));

                SetResult(Result.Canceled);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            try
            {
                //var loginResult = result as LoginResult;
                //var id = AccessToken.CurrentAccessToken.UserId;

                ToggleVisibility(false);

                SetResult(Result.Ok);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public async void OnCompleted(JSONObject json, GraphResponse response)
        {
            try
            {
                //var data = json.ToString();
                //var result = JsonConvert.DeserializeObject<FacebookResult>(data);
                //FbEmail = result.Email;

                ToggleVisibility(true);

                var accessToken = AccessToken.CurrentAccessToken;
                if (accessToken != null)
                {
                    FbAccessToken = accessToken.Token;

                    //Login Api 
                    var (apiStatus, respond) = await RequestsAsync.Auth.SocialLoginAsync(FbAccessToken, "facebook", UserDetails.DeviceId);
                    if (apiStatus == 200)
                    {
                        if (respond is AuthObject auth)
                        {
                            SetDataLogin(auth);

                            switch (AppSettings.ShowWalkTroutPage)
                            {
                                case true:
                                {
                                    Intent newIntent = new Intent(this, typeof(AppIntroWalkTroutPage));
                                    newIntent?.PutExtra("class", "login");
                                    StartActivity(newIntent);
                                    break;
                                }
                                default:
                                    StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                                    break;
                            }

                            ToggleVisibility(false);
                            Finish();
                        }
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
                        Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security),
                            respond.ToString(), GetText(Resource.String.Lbl_Ok));
                    }
                }
                else
                {
                    ToggleVisibility(false);
                    Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_Please_enter_your_data), GetText(Resource.String.Lbl_Ok));
                }
            }
            catch (Exception exception)
            {
                ToggleVisibility(false);
                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), exception.Message, GetText(Resource.String.Lbl_Ok));
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void MprofileTrackerOnMOnProfileChanged(object sender, ProfileChangedEventArgs e)
        {
            try
            {
                if (e.MProfile != null)
                    try
                    {
                        //var FbFirstName = e.MProfile.FirstName;
                        //var FbLastName = e.MProfile.LastName;
                        //var FbName = e.MProfile.Name;
                        //var FbProfileId = e.MProfile.Id;

                        var request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);
                        var parameters = new Bundle();
                        parameters.PutString("fields", "id,name,age_range,email");
                        request.Parameters = parameters;
                        request.ExecuteAsync();
                    }
                    catch (Java.Lang.Exception ex)
                    {
                        Methods.DisplayReportResultTrack(ex);
                    }
                //else
                //    Toast.MakeText(this, GetString(Resource.String.Lbl_Null_Data_User), ToastLength.Short)?.Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
        #endregion

        //======================================================

        #region Google

        //Event Click login using google
        private void GoogleSignInButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                if (MGoogleSignInClient == null)
                {
                    // Configure sign-in to request the user's ID, email address, and basic profile. ID and basic profile are included in DEFAULT_SIGN_IN.
                    var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                        .RequestIdToken(AppSettings.ClientId)
                        .RequestScopes(new Scope(Scopes.Profile))
                        .RequestScopes(new Scope(Scopes.PlusMe))
                        .RequestScopes(new Scope(Scopes.DriveAppfolder))
                        .RequestServerAuthCode(AppSettings.ClientId)
                        .RequestProfile().RequestEmail().Build();

                    MGoogleSignInClient ??= GoogleSignIn.GetClient(this, gso);
                }

                var signInIntent = MGoogleSignInClient.SignInIntent;
                StartActivityForResult(signInIntent, 0);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private async void SetContentGoogle(GoogleSignInAccount acct)
        {
            try
            {
                //Successful log in hooray!!
                if (acct != null)
                {
                    ToggleVisibility(true);

                    //var GAccountName = acct.Account.Name;
                    //var GAccountType = acct.Account.Type;
                    //var GDisplayName = acct.DisplayName;
                    //var GFirstName = acct.GivenName;
                    //var GLastName = acct.FamilyName;
                    //var GProfileId = acct.Id;
                    //var GEmail = acct.Email;
                    //var GImg = acct.PhotoUrl.Path;
                    GAccessToken = acct.IdToken;
                    GServerCode = acct.ServerAuthCode;
                    Console.WriteLine(GServerCode);

                    if (!string.IsNullOrEmpty(GAccessToken))
                    {
                        var (apiStatus, respond) = await RequestsAsync.Auth.SocialLoginAsync(GAccessToken, "google", UserDetails.DeviceId);
                        if (apiStatus == 200)
                        {
                            if (respond is AuthObject auth)
                            {
                                SetDataLogin(auth);

                                switch (AppSettings.ShowWalkTroutPage)
                                {
                                    case true:
                                    {
                                        Intent newIntent = new Intent(this, typeof(AppIntroWalkTroutPage));
                                        newIntent?.PutExtra("class", "login");
                                        StartActivity(newIntent);
                                        break;
                                    }
                                    default:
                                        StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                                        break;
                                }

                                ToggleVisibility(false);
                                Finish();
                            }
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
                    else
                    {
                        ToggleVisibility(false);
                        Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_Please_enter_your_data), GetText(Resource.String.Lbl_Ok));
                    }
                }
            }
            catch (Exception exception)
            {
                ToggleVisibility(false);
                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), exception.Message, GetText(Resource.String.Lbl_Ok));
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #endregion

        #region Result && Permissions

        //Result
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if (requestCode == 0)
                {
                    var task = await GoogleSignIn.GetSignedInAccountFromIntentAsync(data);
                    SetContentGoogle(task);
                }
                else
                {
                    // Logins Facebook
                    MFbCallManager.OnActivityResult(requestCode, (int)resultCode, data);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Permissions
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                switch (requestCode)
                {
                    case 101 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        LoadConfigSettings();
                        CheckCrossAppAuthentication();
                        break;
                    case 101:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        Finish();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Cross App Authentication
         
        private void CheckCrossAppAuthentication()
        {
            try
            {
                LoginTb = JsonConvert.DeserializeObject<DataTables.LoginTb>(Methods.ReadNoteOnSD());
                if (LoginTb != null && !string.IsNullOrEmpty(LoginTb.AccessToken) && !string.IsNullOrEmpty(LoginTb.Username))
                {
                    ContinueButton.Text = GetString(Resource.String.Lbl_ContinueAs) + " " + LoginTb.Username;
                    ContinueButton.Visibility = ViewStates.Visible;
                }
                else
                    ContinueButton.Visibility = ViewStates.Gone;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void CrossAppAuthentication()
        {
            try
            {
                if (LoginTb != null && !string.IsNullOrEmpty(LoginTb.AccessToken) && !string.IsNullOrEmpty(LoginTb.Username))
                {
                    Current.AccessToken = LoginTb.AccessToken;

                    UserDetails.Username = LoginTb.Username;
                    UserDetails.FullName = LoginTb.Username;
                    UserDetails.Password = LoginTb.Password;
                    UserDetails.AccessToken = LoginTb.AccessToken;
                    UserDetails.UserId = LoginTb.UserId;
                    UserDetails.Status = LoginTb.Status;
                    UserDetails.Cookie = LoginTb.AccessToken;
                    UserDetails.Email = LoginTb.Email;

                    //Insert user data to database
                    var user = new DataTables.LoginTb
                    {
                        UserId = UserDetails.UserId,
                        AccessToken = UserDetails.AccessToken,
                        Cookie = UserDetails.Cookie,
                        Username = UserDetails.Username,
                        Password = UserDetails.Password,
                        Status = UserDetails.Status,
                        DeviceId = UserDetails.DeviceId,
                        Email = UserDetails.Email,
                    };
                    ListUtils.DataUserLoginList.Clear();
                    ListUtils.DataUserLoginList.Add(user);

                    var dbDatabase = new SqLiteDatabase();
                    dbDatabase.InsertOrUpdateLogin_Credentials(user);


                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => ApiRequest.Get_MyProfileData_Api(this) });

                    switch (AppSettings.ShowWalkTroutPage)
                    {
                        case true:
                            {
                                Intent newIntent = new Intent(this, typeof(AppIntroWalkTroutPage));
                                newIntent?.PutExtra("class", "login");
                                StartActivity(newIntent);
                                break;
                            }
                        default:
                            StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                            break;
                    }

                    Finish();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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
                ProgressBar.Visibility = isLoginProgress ? ViewStates.Visible : ViewStates.Gone;
                BtnLogin.Visibility = isLoginProgress ? ViewStates.Invisible : ViewStates.Visible;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void SetDataLogin(AuthObject auth)
        {
            try
            {
                Current.AccessToken = auth.AccessToken;

                UserDetails.Username = TxtEmail.Text;
                UserDetails.FullName = TxtEmail.Text;
                UserDetails.Password = TxtPassword.Text;
                UserDetails.AccessToken = auth.AccessToken;
                UserDetails.UserId = auth.UserId;
                UserDetails.Status = "Pending";
                UserDetails.Cookie = auth.AccessToken;
                UserDetails.Email = TxtEmail.Text;

                //Insert user data to database
                var user = new DataTables.LoginTb
                {
                    UserId = UserDetails.UserId,
                    AccessToken = UserDetails.AccessToken,
                    Cookie = UserDetails.Cookie,
                    Username = UserDetails.Email,
                    Password = UserDetails.Password,
                    Status = "Pending",
                    Lang = "",
                    DeviceId = UserDetails.DeviceId,
                    Email = UserDetails.Email,
                };

                ListUtils.DataUserLoginList.Clear();
                ListUtils.DataUserLoginList.Add(user);

                var dbDatabase = new SqLiteDatabase();
                dbDatabase.InsertOrUpdateLogin_Credentials(user);

                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => ApiRequest.Get_MyProfileData_Api(this) });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private async void GetTimezone()
        {
            try
            {
                if (Methods.CheckConnectivity())
                    TimeZone = await ApiRequest.GetTimeZoneAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private async Task<bool> CheckIsActiveUser(string userId)
        {
            try
            {
                var (apiStatus, respond) = await RequestsAsync.Auth.IsActiveUserAsync(userId);
                switch (apiStatus)
                {
                    case 200 when respond is MessageObject auth:
                        Console.WriteLine(auth);
                        return true;
                    case 400:
                        {
                            switch (respond)
                            {
                                case ErrorObject error:
                                    {
                                        var errorText = error.Error.ErrorText;
                                        var errorId = error.Error.ErrorId;
                                        switch (errorId)
                                        {
                                            case "5":
                                                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_ThisUserNotActive), GetText(Resource.String.Lbl_Ok));
                                                break;
                                            case "4":
                                                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_UserNotFound), GetText(Resource.String.Lbl_Ok));
                                                break;
                                            default:
                                                Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), errorText, GetText(Resource.String.Lbl_Ok));
                                                break;
                                        }

                                        break;
                                    }
                            }

                            break;
                        }
                    case 404:
                        Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), respond.ToString(), GetText(Resource.String.Lbl_Ok));
                        break;
                }

                return false;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return false;
            }
        }


        private void LoadConfigSettings()
        {
            try
            {
                var dbDatabase = new SqLiteDatabase();
                var settingsData = dbDatabase.GetSettings();
                if (settingsData != null)
                    ListUtils.SettingsSiteList = settingsData;

                if (Methods.CheckConnectivity())
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => ApiRequest.GetSettings_Api(this) });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void BlurBackground(BlurView view, float radius)
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

                view.SetupWith(rootView)
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

        private void SetHighLight(bool state , RelativeLayout layout , EditText editText , TextView textView)
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