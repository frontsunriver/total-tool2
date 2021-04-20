using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Fragment.App;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Page;
using WoWonderClient.Requests;
using Exception = System.Exception;

namespace WoWonder.Activities.Communities.Pages
{
    public class DialogRatingBarFragment : DialogFragment
    {
        #region Variables Basic

        private RatingBar RatingBar;
        private EditText TxtReview;
        private Button BtnSave;
        private TextView TxtRate;
        public event EventHandler<RatingBarUpEventArgs> OnUpComplete;
        private readonly string PageId = "";
        private readonly PageClass Item;
        private readonly PageProfileActivity ActivityContext;

        #endregion
         
        #region General
         
        public DialogRatingBarFragment(PageProfileActivity activity, string pageId, PageClass item)
        {
            try
            {
                ActivityContext = activity;
                PageId = pageId;
                Item = item;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                // Set our view from the "DialogRatePageLayout" layout resource
               // var view = inflater.Inflate(Resource.Layout.DialogRatePageLayout, container, false);

                Context contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Dark_Base) : new ContextThemeWrapper(Activity, Resource.Style.MyTheme);
                // clone the inflater using the ContextThemeWrapper
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = localInflater?.Inflate(Resource.Layout.DialogRatePageLayout, container, false); 
                return view;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                Dialog?.Window?.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
                base.OnViewCreated(view, savedInstanceState);
                if (Dialog?.Window?.Attributes != null)
                    Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation; //set the animation
                InitComponent(view);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public override void OnResume()
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

        public override void OnPause()
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
                RatingBar = view.FindViewById<RatingBar>(Resource.Id.ratingBar);
                TxtReview = view.FindViewById<EditText>(Resource.Id.ReviewEditText);
                TxtRate = view.FindViewById<TextView>(Resource.Id.rate);

                BtnSave = view.FindViewById<Button>(Resource.Id.ApplyButton);

                if (RatingBar != null) RatingBar.NumStars = 5;

                if (TxtRate != null) TxtRate.Text = GetString(Resource.String.Lbl_Rate) + " : @" + Item.PageName;

                Methods.SetColorEditText(TxtReview, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
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
                        RatingBar.RatingBarChange += RatingBarOnRatingBarChange;
                        BtnSave.Click += BtnSaveOnClick;
                        break;
                    default:
                        RatingBar.RatingBarChange -= RatingBarOnRatingBarChange;
                        BtnSave.Click -= BtnSaveOnClick;
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

        private void RatingBarOnRatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            try
            {
                RatingBar.Rating = e.Rating;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void BtnSaveOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(ActivityContext, ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
                {
                    switch (RatingBar.Rating)
                    {
                        case <= 0:
                            Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_Please_select_Rating), ToastLength.Short)?.Show(); 
                            return;
                    }

                    if (string.IsNullOrEmpty(TxtReview.Text) || string.IsNullOrWhiteSpace(TxtReview.Text))
                    {
                        Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_Please_enter_review), ToastLength.Short)?.Show();
                        return;
                    }

                    //Show a progress
                    AndHUD.Shared.Show(ActivityContext, ActivityContext.GetString(Resource.String.Lbl_Loading) + "...");

                    StartApiService(); 
                } 
            }
            catch (Exception ex)
            {
                AndHUD.Shared.Dismiss(ActivityContext);
                Methods.DisplayReportResultTrack(ex);
            }
        }

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(ActivityContext, ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { RatePageApi });
        }

        private async Task RatePageApi()
        {
            var (apiStatus, respond) = await RequestsAsync.Page.RatePageAsync(PageId, RatingBar.Rating.ToString(CultureInfo.InvariantCulture), TxtReview.Text);
            switch (apiStatus)
            {
                case 200:
                {
                    switch (respond)
                    {
                        case RatePageObject result:
                            ActivityContext?.RunOnUiThread(() =>
                            {
                                try
                                {
                                    AndHUD.Shared.Dismiss(ActivityContext);
                             
                                    Item.Rating = result.Val;

                                    var modelsClass = ActivityContext.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.InfoPageBox);
                                    if (modelsClass != null)
                                    {
                                        modelsClass.PageInfoModelClass = new PageInfoModelClass
                                        {
                                            PageClass = Item,
                                            PageId = Item.PageId
                                        };
                                        ActivityContext.PostFeedAdapter.NotifyItemChanged(ActivityContext.PostFeedAdapter.ListDiffer.IndexOf(modelsClass));
                                    } 
                              
                                    PageProfileActivity.PageData.IsRated = true;
                                    PageProfileActivity.PageData.Rating = result.Val;

                                    Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_Rated), ToastLength.Short)?.Show();

                                    Dismiss(); 
                                }
                                catch (Exception e)
                                {
                                    AndHUD.Shared.Dismiss(ActivityContext);
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                            break;
                    }

                    break;
                }
                default:
                    Methods.DisplayAndHudErrorResult(ActivityContext, respond);
                    break;
            }
        }
         
        #endregion

        public abstract class RatingBarUpEventArgs : EventArgs
        {
            public View View { get; set; }
            public int Position { get; set; }
        }

        protected virtual void OnRatingBarUpComplete(RatingBarUpEventArgs e)
        {
            OnUpComplete?.Invoke(this, e);
        }

    }
}