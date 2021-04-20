using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Uri = Android.Net.Uri;

namespace WoWonder.Helpers.Utils
{
    [Preserve(AllMembers = true)]
    public class StoreReviewApp : IStoreReview
    {
        /// <summary>
        /// Opens the store listing.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public void OpenStoreListing(string appId) => OpenStoreReviewPage(appId);
         
        private Intent GetRateIntent(string url)
        {
            try
            {
                var intent = new Intent(Intent.ActionView, Uri.Parse(url));

                intent.AddFlags(ActivityFlags.NoHistory);
                intent.AddFlags(ActivityFlags.MultipleTask);
                switch ((int)Build.VERSION.SdkInt)
                {
                    case >= 21:
                        intent.AddFlags(ActivityFlags.NewDocument);
                        break;
                    default:
                        intent.AddFlags(ActivityFlags.ClearWhenTaskReset);
                        break;
                }
                intent.SetFlags(ActivityFlags.ClearTop);
                intent.SetFlags(ActivityFlags.NewTask);
                return intent;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        /// <summary>
        /// Opens the store review page.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public void OpenStoreReviewPage(string appId)
        {
            var url = $"market://details?id={appId}";
            try
            {
                var intent = GetRateIntent(url);
                Application.Context.StartActivity(intent);
                return;
            }
            catch (Exception ex)
            {
                //Unable to launch app store
                Methods.DisplayReportResultTrack(ex);
            }

            url = $"https://play.google.com/store/apps/details?id={appId}";
            try
            {
                var intent = GetRateIntent(url);
                Application.Context.StartActivity(intent);
            }
            catch (Exception ex)
            {
                //Unable to launch app store:
                Methods.DisplayReportResultTrack(ex);
            }
        }
        /// <summary>
        /// Requests an app review.
        /// </summary>
        public void RequestReview()
        {
        } 
    }
     
    public interface IStoreReview
    {
        /// <summary>
        /// Opens the store listing.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        void OpenStoreListing(string appId);

        /// <summary>
        /// Opens the store review page.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        void OpenStoreReviewPage(string appId);

        /// <summary>
        /// Requests an app review.
        /// </summary>
        void RequestReview();
    }

}