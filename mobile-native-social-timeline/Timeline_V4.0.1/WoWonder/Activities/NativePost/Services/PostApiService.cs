using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Java.Lang;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient;
using Exception = Java.Lang.Exception;

namespace WoWonder.Activities.NativePost.Services
{
    [Service(Exported = false)]
    public class PostApiService : JobIntentService
    { 
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        protected override void OnHandleWork(Intent p0)
        {
            try
            {
                //Toast.MakeText(Application.Context, "OnHandleWork", ToastLength.Short)?.Show();
                new Handler(Looper.MainLooper).PostDelayed(new PostUpdaterHelper(new Handler(Looper.MainLooper)), AppSettings.RefreshPostSeconds);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnCreate()
        {
            try
            {
                base.OnCreate();
                //Toast.MakeText(Application.Context, "OnCreate", ToastLength.Short)?.Show();
                new Handler(Looper.MainLooper).PostDelayed(new PostUpdaterHelper(new Handler(Looper.MainLooper)), AppSettings.RefreshPostSeconds);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            } 
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            try
            {
                base.OnStartCommand(intent, flags, startId);

                new Handler(Looper.MainLooper).PostDelayed(new PostUpdaterHelper(new Handler(Looper.MainLooper)), AppSettings.RefreshPostSeconds);

                //Toast.MakeText(Application.Context, "OnStartCommand", ToastLength.Short)?.Show();

                return StartCommandResult.Sticky;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return StartCommandResult.NotSticky;
            } 
        }
    }

    public class PostUpdaterHelper : Java.Lang.Object, IRunnable
    {
        private static Handler MainHandler;

        public PostUpdaterHelper(Handler mainHandler)
        {
            MainHandler = mainHandler;
        }

        public void Run()
        {
            try
            {
                //Toast.MakeText(Application.Context, "AppState " + Methods.AppLifecycleObserver.AppState, ToastLength.Short).Show();

                if (string.IsNullOrEmpty(Methods.AppLifecycleObserver.AppState))
                    Methods.AppLifecycleObserver.AppState = "Background";

                switch (Methods.AppLifecycleObserver.AppState)
                {
                    case "Background":
                    {
                        if (string.IsNullOrEmpty(Client.WebsiteUrl))
                        {
                            Client a = new Client(AppSettings.TripleDesAppServiceProvider);
                            Console.WriteLine(a);
                        }

                        SqLiteDatabase dbDatabase = new SqLiteDatabase();

                        if (string.IsNullOrEmpty(Current.AccessToken))
                        {
                            var login = dbDatabase.Get_data_Login_Credentials();
                            Console.WriteLine(login);
                        }

                        if (string.IsNullOrEmpty(Current.AccessToken))
                            return;

                        if (Methods.CheckConnectivity())
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { ApiPostAsync.FetchFirstNewsFeedApiPosts });

                        //Toast.MakeText(Application.Context, "ResultSender wael", ToastLength.Short).Show();
                        MainHandler ??= new Handler(Looper.MainLooper);
                        MainHandler?.PostDelayed(new PostUpdaterHelper(new Handler(Looper.MainLooper)), AppSettings.RefreshPostSeconds);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                //Toast.MakeText(Application.Context, "ResultSender failed",ToastLength.Short)?.Show();
                MainHandler ??= new Handler(Looper.MainLooper);
                MainHandler?.PostDelayed(new PostUpdaterHelper(new Handler(Looper.MainLooper)), AppSettings.RefreshPostSeconds);
                Methods.DisplayReportResultTrack(e);
            } 
        }
    }
     
}