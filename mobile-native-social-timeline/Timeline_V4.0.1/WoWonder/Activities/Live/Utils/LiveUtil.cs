using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using Android;
using Android.Content.PM;
using WoWonder.Activities.Live.Page;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.Live.Utils
{
    public class LiveUtil
    {
        private readonly Activity Activity;
        public LiveUtil(Activity activity)
        {
            try
            {
                Activity = activity;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Live

        //Go Live
        public void GoLiveOnClick()
        {
            try
            {
                switch ((int)Build.VERSION.SdkInt)
                {
                    // Check if we're running on Android 5.0 or higher
                    case < 23:
                        OpenDialogLive();
                        break;
                    default:
                    {
                        if (Activity.CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted &&
                            Activity.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted &&
                            Activity.CheckSelfPermission(Manifest.Permission.AccessMediaLocation) == Permission.Granted &&
                            Activity.CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted &&
                            Activity.CheckSelfPermission(Manifest.Permission.RecordAudio) == Permission.Granted &&
                            Activity.CheckSelfPermission(Manifest.Permission.ModifyAudioSettings) == Permission.Granted)
                        {
                            OpenDialogLive();
                        }
                        else
                        {
                            Activity.RequestPermissions(new[]
                            {
                                Manifest.Permission.ReadExternalStorage,
                                Manifest.Permission.WriteExternalStorage,
                                Manifest.Permission.Camera,
                                Manifest.Permission.AccessMediaLocation,
                                Manifest.Permission.RecordAudio,
                                Manifest.Permission.ModifyAudioSettings,
                            }, 235);
                        }

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OpenDialogLive()
        {
            try
            {

                var streamName = "live" + Methods.Time.CurrentTimeMillis();
                if (string.IsNullOrEmpty(streamName) || string.IsNullOrWhiteSpace(streamName))
                {
                    Toast.MakeText(Activity, Activity.GetText(Resource.String.Lbl_PleaseEnterLiveStreamName), ToastLength.Short)?.Show();
                    return;
                }
                //Owner >> ClientRoleBroadcaster , Users >> ClientRoleAudience
                Intent intent = new Intent(Activity, typeof(LiveStreamingActivity));
                intent.PutExtra(Constants.KeyClientRole, DT.Xamarin.Agora.Constants.ClientRoleBroadcaster);
                intent.PutExtra("StreamName", streamName);
                Activity.StartActivity(intent);

                //var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                //dialog.Title(GetText(Resource.String.Lbl_CreateLiveVideo));
                //dialog.Input(Resource.String.Lbl_AddLiveVideoContext, 0, false, (materialDialog, s) =>
                //{
                //    try
                //    {

                //    }
                //    catch (Exception e)
                //    {
                //        Methods.DisplayReportResultTrack(e);
                //    }
                //});
                //dialog.InputType(InputTypes.TextFlagImeMultiLine);
                //dialog.PositiveText(GetText(Resource.String.Lbl_Go_Live)).OnPositive(new WoWonderTools.MyMaterialDialog());
                //dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(new WoWonderTools.MyMaterialDialog());
                //dialog.AlwaysCallSingleChoiceCallback();
                //dialog.Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion
    }
}