using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Util;
using Google.Android.Material.BottomSheet;
using WoWonder.Activities.Gift.Adapters;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;

namespace WoWonder.Activities.Gift
{
    public class GiftDialogFragment : BottomSheetDialogFragment 
    {
        #region Variables Basic

        private GiftAdapter MAdapter; 
        private RecyclerView MRecycler;
        private GridLayoutManager LayoutManager;
        public View Inflated;
        private string UserId;

        #endregion

        #region General
         
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                Context contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Dark_Base) : new ContextThemeWrapper(Activity, Resource.Style.MyTheme);
                // clone the inflater using the ContextThemeWrapper
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = localInflater?.Inflate(Resource.Layout.GiftDialogtLayout, container, false); 
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
                base.OnViewCreated(view, savedInstanceState);

                UserId = Arguments.GetString("UserId");

                InitComponent(view);
                SetRecyclerViewAdapters();
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnDestroy()
        {
            try
            {
                
                base.OnDestroy();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                MRecycler = (RecyclerView)view.FindViewById(Resource.Id.recyler);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
  
        private void SetRecyclerViewAdapters()
        {
            try
            {
                MRecycler.NestedScrollingEnabled = false;
                MAdapter = new GiftAdapter(Activity);
                MAdapter.OnItemClick += GiftAdapterOnItemClick; 
                LayoutManager = new GridLayoutManager(Activity, 3);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.SetItemViewCacheSize(20);
                MRecycler.HasFixedSize = true;
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<GiftObject.DataGiftObject>(Activity, MAdapter, sizeProvider, 10);
                MRecycler.AddOnScrollListener(preLoader);
                MRecycler.SetAdapter(MAdapter);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events
         
        private void GiftAdapterOnItemClick(object sender, GiftAdapterClickEventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Context, Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                int position = e.Position;
                switch (position)
                {
                    case > -1:
                    {
                        var item = MAdapter.GetItem(position);
                        if (item != null)
                        { 
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Global.SendGiftAsync(UserId, item.Id) });

                            Toast.MakeText(Context, Context.GetText(Resource.String.Lbl_Sent_successfully), ToastLength.Short)?.Show();
                            //Close Fragment 
                            Dismiss();
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

        #endregion
         
    }
}