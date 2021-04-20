using System;
using System.Collections.Generic;
using Android.App;
using Android.Content; 
using Android.Widget;
using AndroidX.Core.App;
using Com.OneSignal.Abstractions;
using Com.OneSignal.Android;
using Org.Json;
using WoWonder.Activities.Tabbes;
using OSNotification = Com.OneSignal.Abstractions.OSNotification;
using OSNotificationPayload = Com.OneSignal.Abstractions.OSNotificationPayload;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;

namespace WoWonder.Library.OneSignal
{
    public static class OneSignalNotification
    {
        //Force your app to Register notification directly without loading it from server (For Best Result)

        private static string UserId, PostId, PageId, GroupId,EventId, Type;

        public static void RegisterNotificationDevice()
        {
            try
            {
                switch (UserDetails.NotificationPopup)
                {
                    case true:
                    {
                        if (!string.IsNullOrEmpty(AppSettings.OneSignalAppId) || !string.IsNullOrWhiteSpace(AppSettings.OneSignalAppId))
                        {
                            Com.OneSignal.OneSignal.Current.StartInit(AppSettings.OneSignalAppId)
                                .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                                .HandleNotificationReceived(HandleNotificationReceived)
                                .HandleNotificationOpened(HandleNotificationOpened)
                                .EndInit();
                            Com.OneSignal.OneSignal.Current.IdsAvailable(IdsAvailable);
                            Com.OneSignal.OneSignal.Current.RegisterForPushNotifications();
                            Com.OneSignal.OneSignal.Current.SetSubscription(true);
                            AppSettings.ShowNotification = true;
                        }

                        break;
                    }
                    default:
                        Un_RegisterNotificationDevice();
                        break;
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }

        public static void Un_RegisterNotificationDevice()
        {
            try
            {
                Com.OneSignal.OneSignal.Current.SetSubscription(false);
                Com.OneSignal.OneSignal.Current.ClearAndroidOneSignalNotifications();
                AppSettings.ShowNotification = false; 
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }

        private static void IdsAvailable(string userId, string pushToken)
        {
            try
            {
                UserDetails.DeviceId = userId;
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);  
            }
        }

        private static void HandleNotificationReceived(OSNotification notification)
        {
            try
            {
                //OSNotificationPayload payload = notification.payload;
                //Dictionary<string, object> additionalData = payload.additionalData; 
                //string message = payload.body; 
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long)?.Show(); //Allen
                Methods.DisplayReportResultTrack(ex);
            }
        }

        private static void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            try
            {
                OSNotificationPayload payload = result.notification.payload;
                Dictionary<string, object> additionalData = payload.additionalData;
                //string message = payload.body; "post_id" "post_id" "type" "url"
                string actionId = result.action.actionID;

                switch (additionalData?.Count)
                {
                    case > 0:
                    {
                        foreach (var item in additionalData)
                        {
                            switch (item.Key)
                            {
                                case "post_id":
                                    PostId = item.Value.ToString();
                                    break;
                                case "user_id":
                                    UserId = item.Value.ToString();
                                    break;
                                case "page_id":
                                    PageId = item.Value.ToString();
                                    break;
                                case "group_id":
                                    GroupId = item.Value.ToString();
                                    break;
                                case "event_id":
                                    EventId = item.Value.ToString();
                                    break;
                                case "type":
                                    Type = item.Value.ToString();
                                    break;
                            }
                        }

                        Intent intent = new Intent(Application.Context, typeof(TabbedMainActivity));
                        intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                        intent.AddFlags(ActivityFlags.SingleTop);
                        intent.SetAction(Intent.ActionView);
                        intent.PutExtra("userId", UserId);
                        intent.PutExtra("PostId", PostId);
                        intent.PutExtra("PageId", PageId);
                        intent.PutExtra("GroupId", GroupId);
                        intent.PutExtra("EventId", EventId);
                        intent.PutExtra("type", Type);
                        intent.PutExtra("Notifier", "Notifier");
                        Application.Context.StartActivity(intent);

                        if (additionalData.ContainsKey("discount"))
                        {
                            // Take user to your store..
                        }

                        break;
                    }
                }
                if (actionId != null)
                {
                    // actionSelected equals the id on the button the user pressed.
                    // actionSelected will equal "__DEFAULT__" when the notification itself was tapped when buttons were present. 
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }
    }

    public class NotificationExtenderServiceHandler : NotificationExtenderService, NotificationCompat.IExtender
    {
        protected override void OnHandleIntent(Intent intent)
        {

        }

        protected override bool OnNotificationProcessing(OSNotificationReceivedResult p0)
        {
            //OverrideSettings overrideSettings = new OverrideSettings();
            //overrideSettings.Extender = new NotificationCompat.CarExtender();

            Com.OneSignal.Android.OSNotificationPayload payload = p0.Payload;
            JSONObject additionalData = payload.AdditionalData;

            if (additionalData.Has("room_name"))
            {
                //string roomName = additionalData.Get("room_name").ToString();
                //string callType = additionalData.Get("call_type").ToString();
                //string callId = additionalData.Get("call_id").ToString();
                //string fromId = additionalData.Get("from_id").ToString();
                //string toId = additionalData.Get("to_id").ToString();

                return false;
            }

            return true;
        }

        public NotificationCompat.Builder Extend(NotificationCompat.Builder builder)
        {
            return builder;
        }
    }
}