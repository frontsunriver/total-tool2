using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS; 
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.Lang;
using WoWonder.Activities.Default;
using WoWonder.Activities.NativePost.Pages;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;

namespace WoWonder.Activities
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/SplashScreenTheme", NoHistory = true, MainLauncher = true, ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataSchemes = new[] { "http", "https" }, DataHost = "@string/ApplicationUrlWeb", AutoVerify = false)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataSchemes = new[] { "http", "https" }, DataHost = "@string/ApplicationUrlWeb", DataPathPrefixes = new[] { "/register/", "/post/" }, AutoVerify = false)]
    public class SplashScreenActivity : AppCompatActivity
    { 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                new Handler(Looper.MainLooper).Post(new Runnable(FirstRunExcite));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void FirstRunExcite()
        {
            try
            {
                switch (string.IsNullOrEmpty(AppSettings.Lang))
                {
                    case false:
                        LangController.SetApplicationLang(this, AppSettings.Lang);
                        break;
                    default:
                        #pragma warning disable 618 
                        UserDetails.LangName = (int)Build.VERSION.SdkInt < 25 ? Resources?.Configuration?.Locale?.Language.ToLower() : Resources?.Configuration?.Locales.Get(0)?.Language.ToLower() ?? Resources?.Configuration?.Locale?.Language.ToLower();
                        #pragma warning restore 618
                        LangController.SetApplicationLang(this, UserDetails.LangName);
                        break;
                }
                  
                switch (string.IsNullOrEmpty(UserDetails.AccessToken))
                {
                    case false when Intent?.Data?.Path != null:
                    {
                        if (Intent.Data.Path.Contains("register") && UserDetails.Status != "Active" && UserDetails.Status != "Pending")
                        {
                            StartActivity(new Intent(Application.Context, typeof(RegisterActivity)));
                        }
                        else if (Intent.Data.Path.Contains("post") && (UserDetails.Status == "Active" || UserDetails.Status == "Pending"))
                        {
                            var postId = Intent.Data.Path.Split("/").Last().Replace("/", "").Split("_").First();

                            var intent = new Intent(Application.Context, typeof(ViewFullPostActivity));
                            intent.PutExtra("Id", postId);
                            StartActivity(intent);
                        }
                        else
                        {
                            switch (UserDetails.Status)
                            {
                                case "Active":
                                case "Pending":
                                    StartActivity(new Intent(Application.Context, typeof(TabbedMainActivity)));
                                    break;
                                default: 
                                    StartActivity(new Intent(Application.Context, typeof(LoginActivity))); 
                                    break;
                            }
                        }

                        break;
                    }
                    case false:
                        switch (UserDetails.Status)
                        {
                            case "Active":
                            case "Pending":
                                StartActivity(new Intent(Application.Context, typeof(TabbedMainActivity)));
                                break;
                            default: 
                                StartActivity(new Intent(Application.Context, typeof(LoginActivity))); 
                                break;
                        } 
                        break;
                    default: 
                        StartActivity(new Intent(Application.Context, typeof(LoginActivity))); 
                        break;
                }

                OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                Finish();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                Toast.MakeText(this, exception.Message, ToastLength.Short)?.Show();
            }
        }
    }
}