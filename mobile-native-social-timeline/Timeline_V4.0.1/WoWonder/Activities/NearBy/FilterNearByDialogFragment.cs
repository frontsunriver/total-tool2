using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.Content;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.BottomSheet;
using Java.Lang;
using WoWonder.Adapters;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using Exception = System.Exception;

namespace WoWonder.Activities.NearBy
{
    public class FilterNearByDialogFragment : BottomSheetDialogFragment, SeekBar.IOnSeekBarChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private PeopleNearByActivity ContextNearBy;
        private TextView IconBack, TxtDistanceCount, TxtRelationship, RelationshipMoreIcon;
        private TextView ResetTextView;
        private RelativeLayout LayoutRelationship;
        private SeekBar DistanceBar;
        private RecyclerView GenderRecycler;
        private GendersAdapter GenderAdapter;
        private Button ButtonOffline, ButtonOnline, BothStatusButton, BtnApply;
        private int DistanceCount, Status;
        private string Gender, RelationshipId;

        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            ContextNearBy = (PeopleNearByActivity)Activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                Context contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Dark_Base) : new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Base);
                // clone the inflater using the ContextThemeWrapper
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = localInflater?.Inflate(Resource.Layout.BottomSheetNearByFilter, container, false);
                // View view = inflater.Inflate(Resource.Layout.BottomSheetNearByFilter, container, false); 
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

                InitComponent(view);
                SetRecyclerViewAdapters();

                IconBack.Click += IconBackOnClick;

                ButtonOffline.Click += ButtonOfflineOnClick;
                ButtonOnline.Click += ButtonOnlineOnClick;
                BothStatusButton.Click += BothStatusButtonOnClick;
                BtnApply.Click += BtnApplyOnClick;
                ResetTextView.Click += ResetTextViewOnClick;
                LayoutRelationship.Click += LayoutRelationshipOnClick;

                GetFilter();
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

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                IconBack = view.FindViewById<TextView>(Resource.Id.IconBack);

                GenderRecycler = view.FindViewById<RecyclerView>(Resource.Id.GenderRecyler);


                TxtDistanceCount = view.FindViewById<TextView>(Resource.Id.Distancenumber);
                DistanceBar = view.FindViewById<SeekBar>(Resource.Id.distanceSeeker);

                ButtonOffline = view.FindViewById<Button>(Resource.Id.OfflineButton);
                ButtonOnline = view.FindViewById<Button>(Resource.Id.OnlineButton);
                BothStatusButton = view.FindViewById<Button>(Resource.Id.BothStatusButton);
                BtnApply = view.FindViewById<Button>(Resource.Id.ApplyButton);
                ResetTextView = view.FindViewById<TextView>(Resource.Id.Resetbutton);

                LayoutRelationship = view.FindViewById<RelativeLayout>(Resource.Id.LayoutRelationship);

                TxtRelationship = view.FindViewById<TextView>(Resource.Id.textRelationship);
                RelationshipMoreIcon = view.FindViewById<TextView>(Resource.Id.RelationshipMoreIcon);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconBack, AppSettings.FlowDirectionRightToLeft ? IonIconsFonts.IosArrowDropright : IonIconsFonts.IosArrowDropleft);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, RelationshipMoreIcon, AppSettings.FlowDirectionRightToLeft ? IonIconsFonts.IosArrowDropleft : IonIconsFonts.IosArrowDropright);

                DistanceBar.Max = 300;
                DistanceBar.SetOnSeekBarChangeListener(this);

                switch (Build.VERSION.SdkInt)
                {
                    case >= BuildVersionCodes.N:
                        DistanceBar.SetProgress(string.IsNullOrEmpty(UserDetails.NearByDistanceCount) ? 300 : Convert.ToInt32(UserDetails.NearByDistanceCount), true);
                        break;
                    // For API < 24 
                    default:
                        DistanceBar.Progress = string.IsNullOrEmpty(UserDetails.NearByDistanceCount) ? 300 : Convert.ToInt32(UserDetails.NearByDistanceCount);
                        break;
                }
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
                GenderRecycler.HasFixedSize = true;
                GenderRecycler.SetLayoutManager(new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false));
                GenderAdapter = new GendersAdapter(Activity)
                {
                    GenderList = new ObservableCollection<Classes.Gender>()
                };
                GenderRecycler.SetAdapter(GenderAdapter);
                GenderRecycler.NestedScrollingEnabled = false;
                GenderAdapter.NotifyDataSetChanged();
                GenderRecycler.Visibility = ViewStates.Visible;
                GenderAdapter.ItemClick += GenderAdapterOnItemClick;

                GenderAdapter.GenderList.Add(new Classes.Gender
                {
                    GenderId = "all",
                    GenderName = Activity.GetText(Resource.String.Lbl_All),
                    GenderColor = AppSettings.MainColor,
                    GenderSelect = false
                });

                switch (ListUtils.SettingsSiteList?.Genders?.Count)
                {
                    case > 0:
                    {
                        foreach (var (key, value) in ListUtils.SettingsSiteList?.Genders)
                        {
                            GenderAdapter.GenderList.Add(new Classes.Gender
                            {
                                GenderId = key,
                                GenderName = value,
                                GenderColor = AppSettings.SetTabDarkTheme ? "#ffffff" : "#444444",
                                GenderSelect = false
                            });
                        }

                        break;
                    }
                    default:
                        GenderAdapter.GenderList.Add(new Classes.Gender
                        {
                            GenderId = "male",
                            GenderName = Activity.GetText(Resource.String.Radio_Male),
                            GenderColor = AppSettings.SetTabDarkTheme ? "#ffffff" : "#444444",
                            GenderSelect = false
                        });
                        GenderAdapter.GenderList.Add(new Classes.Gender
                        {
                            GenderId = "female",
                            GenderName = Activity.GetText(Resource.String.Radio_Female),
                            GenderColor = AppSettings.SetTabDarkTheme ? "#ffffff" : "#444444",
                            GenderSelect = false
                        });
                        break;
                }

                GenderAdapter.NotifyDataSetChanged();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Event

        private void GenderAdapterOnItemClick(object sender, GendersAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = GenderAdapter.GetItem(position);
                        if (item != null)
                        {
                            var check = GenderAdapter.GenderList.Where(a => a.GenderSelect).ToList();
                            switch (check.Count)
                            {
                                case > 0:
                                {
                                    foreach (var all in check)
                                        all.GenderSelect = false;
                                    break;
                                }
                            }

                            item.GenderSelect = true;
                            GenderAdapter.NotifyDataSetChanged();

                            Gender = item.GenderId;
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

        //Back
        private void IconBackOnClick(object sender, EventArgs e)
        {
            try
            {
                Dismiss();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Save data 
        private void BtnApplyOnClick(object sender, EventArgs e)
        {
            try
            {
                UserDetails.NearByGender = Gender;
                UserDetails.NearByDistanceCount = DistanceCount.ToString();
                UserDetails.NearByStatus = Status.ToString();
                UserDetails.NearByRelationship = RelationshipId;

                var dbDatabase = new SqLiteDatabase();
                var newSettingsFilter = new DataTables.NearByFilterTb
                {
                    DistanceValue = DistanceCount,
                    Gender = Gender,
                    Status = Status,
                    Relationship = RelationshipId
                };
                dbDatabase.InsertOrUpdate_NearByFilter(newSettingsFilter);
                

                ContextNearBy.MAdapter.UserList.Clear();
                ContextNearBy.MAdapter.NotifyDataSetChanged();

                ContextNearBy.SwipeRefreshLayout.Refreshing = true;
                ContextNearBy.SwipeRefreshLayout.Enabled = true;

                ContextNearBy.MainScrollEvent.IsLoading = false;

                ContextNearBy.MRecycler.Visibility = ViewStates.Visible;
                ContextNearBy.EmptyStateLayout.Visibility = ViewStates.Gone;

                ContextNearBy.StartApiService();

                Dismiss();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Reset Value
        private void ResetTextViewOnClick(object sender, EventArgs e)
        {
            try
            {
                var dbDatabase = new SqLiteDatabase();
                var newSettingsFilter = new DataTables.NearByFilterTb
                {
                    DistanceValue = 0,
                    Gender = "all",
                    Status = 2,
                    Relationship = "5",
                };
                dbDatabase.InsertOrUpdate_NearByFilter(newSettingsFilter);
                

                Gender = "all";
                DistanceCount = 0;
                Status = 2;
                RelationshipId = "5";

                UserDetails.NearByGender = Gender;
                UserDetails.NearByDistanceCount = DistanceCount.ToString();
                UserDetails.NearByStatus = Status.ToString();
                UserDetails.NearByRelationship = RelationshipId;

                //////////////////////////// Gender //////////////////////////////

                var check1 = GenderAdapter.GenderList.Where(a => a.GenderSelect).ToList();
                switch (check1.Count)
                {
                    case > 0:
                    {
                        foreach (var all in check1)
                            all.GenderSelect = false;
                        break;
                    }
                }

                var check2 = GenderAdapter.GenderList.FirstOrDefault(a => a.GenderId == "all");
                if (check2 != null)
                {
                    check2.GenderSelect = true;
                    Gender = check2.GenderId;

                    GenderAdapter.NotifyDataSetChanged();
                }
                //////////////////////////// Status ////////////////////////////// 
                BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                BothStatusButton.SetTextColor(Color.ParseColor("#ffffff"));

                ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonOnline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonOffline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                switch (Build.VERSION.SdkInt)
                {
                    case >= BuildVersionCodes.N:
                        DistanceBar.SetProgress(300, true);
                        break;
                    // For API < 24 
                    default:
                        DistanceBar.Progress = 300;
                        break;
                }

                TxtDistanceCount.Text = DistanceCount + " " + GetText(Resource.String.Lbl_km);

                TxtRelationship.Text = RelationshipId switch
                {
                    "5" => GetText(Resource.String.Lbl_All),
                    "1" => GetText(Resource.String.Lbl_Single),
                    "2" => GetText(Resource.String.Lbl_InRelationship),
                    "3" => GetText(Resource.String.Lbl_Married),
                    "4" => GetText(Resource.String.Lbl_Engaged),
                    _ => GetText(Resource.String.Lbl_All)
                };
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Select Status >> Both (2)
        private void BothStatusButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                //follow_button_profile_friends >> Un click
                //follow_button_profile_friends_pressed >> click
                BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                BothStatusButton.SetTextColor(Color.ParseColor("#ffffff"));

                ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonOnline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonOffline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                Status = 2;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Select Status >> Online (1)
        private void ButtonOnlineOnClick(object sender, EventArgs e)
        {
            try
            {
                //follow_button_profile_friends >> Un click
                //follow_button_profile_friends_pressed >> click
                ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                ButtonOnline.SetTextColor(Color.ParseColor("#ffffff"));

                BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                BothStatusButton.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonOffline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                Status = 1;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Select Status >> Offline (0)
        private void ButtonOfflineOnClick(object sender, EventArgs e)
        {
            try
            {
                //follow_button_profile_friends >> Un click
                //follow_button_profile_friends_pressed >> click
                ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                ButtonOffline.SetTextColor(Color.ParseColor("#ffffff"));

                BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                BothStatusButton.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonOnline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                Status = 0;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Relationship 
        //1 >>  Single
        //2 >>  In a relationship
        //3 >>  Married
        //4 >>  Engaged
        //5 >>  All
        private void LayoutRelationshipOnClick(object sender, EventArgs e)
        {
            try
            {
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(Context).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetText(Resource.String.Lbl_All));
                arrayAdapter.Add(GetText(Resource.String.Lbl_Single));
                arrayAdapter.Add(GetText(Resource.String.Lbl_InRelationship));
                arrayAdapter.Add(GetText(Resource.String.Lbl_Married));
                arrayAdapter.Add(GetText(Resource.String.Lbl_Engaged));

                dialogList.Title(GetText(Resource.String.Lbl_Relationship)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
        #endregion

        #region SeekBar

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            try
            {
                TxtDistanceCount.Text = progress + " " + GetText(Resource.String.Lbl_km);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {

        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            try
            {
                DistanceCount = seekBar.Progress;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        private void GetFilter()
        {
            try
            {
                var dbDatabase = new SqLiteDatabase();

                var data = dbDatabase.GetNearByFilterById();
                if (data != null)
                {
                    Gender = data.Gender;
                    DistanceCount = data.DistanceValue;
                    Status = data.Status;
                    RelationshipId = data.Relationship;

                    //////////////////////////// Gender //////////////////////////////
                    Gender = data.Gender;

                    var check1 = GenderAdapter.GenderList.Where(a => a.GenderSelect).ToList();
                    switch (check1.Count)
                    {
                        case > 0:
                        {
                            foreach (var all in check1)
                                all.GenderSelect = false;
                            break;
                        }
                    }

                    var check2 = GenderAdapter.GenderList.FirstOrDefault(a => a.GenderId == data.Gender);
                    if (check2 != null)
                    {
                        check2.GenderSelect = true;
                        Gender = check2.GenderId;
                    }

                    GenderAdapter.NotifyDataSetChanged();

                    TxtDistanceCount.Text = DistanceCount + " " + GetText(Resource.String.Lbl_km);

                    switch (Build.VERSION.SdkInt)
                    {
                        case >= BuildVersionCodes.N:
                            DistanceBar.SetProgress(DistanceCount == 0 ? 300 : DistanceCount, true);
                            break;
                        // For API < 24 
                        default:
                            DistanceBar.Progress = DistanceCount == 0 ? 300 : DistanceCount;
                            break;
                    }

                    //////////////////////////// Status //////////////////////////////
                    //Select Status >> Both (2)
                    //Select Status >> Online (1)
                    //Select Status >> Offline (0)

                    switch (data.Status)
                    {
                        case 0:
                            ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                            ButtonOffline.SetTextColor(Color.ParseColor("#ffffff"));

                            BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                            BothStatusButton.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                            ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                            ButtonOnline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                            Status = 0;
                            break;
                        case 1:
                            ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                            ButtonOnline.SetTextColor(Color.ParseColor("#ffffff"));

                            BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                            BothStatusButton.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                            ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                            ButtonOffline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                            Status = 1;
                            break;
                        case 2:
                            BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                            BothStatusButton.SetTextColor(Color.ParseColor("#ffffff"));

                            ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                            ButtonOnline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                            ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                            ButtonOffline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                            Status = 2;
                            break;
                    }

                    TxtRelationship.Text = RelationshipId switch
                    {
                        "1" => GetText(Resource.String.Lbl_Single),
                        "2" => GetText(Resource.String.Lbl_InRelationship),
                        "3" => GetText(Resource.String.Lbl_Married),
                        "4" => GetText(Resource.String.Lbl_Engaged),
                        "5" => GetText(Resource.String.Lbl_All),
                        _ => GetText(Resource.String.Lbl_All)
                    };
                }
                else
                {
                    var newSettingsFilter = new DataTables.NearByFilterTb
                    {
                        DistanceValue = 0,
                        Gender = "all",
                        Status = 2,
                        Relationship = "5",
                    };
                    dbDatabase.InsertOrUpdate_NearByFilter(newSettingsFilter);

                    Gender = "all";
                    DistanceCount = 0;
                    Status = 2;
                    RelationshipId = "5";

                    //////////////////////////// Gender //////////////////////////////

                    var check1 = GenderAdapter.GenderList.Where(a => a.GenderSelect).ToList();
                    switch (check1.Count)
                    {
                        case > 0:
                        {
                            foreach (var all in check1)
                                all.GenderSelect = false;
                            break;
                        }
                    }

                    var check2 = GenderAdapter.GenderList.FirstOrDefault(a => a.GenderId == "all");
                    if (check2 != null)
                    {
                        check2.GenderSelect = true;
                        Gender = check2.GenderId;

                        GenderAdapter.NotifyDataSetChanged();
                    }

                    //////////////////////////// Status ////////////////////////////// 
                    BothStatusButton.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                    BothStatusButton.SetTextColor(Color.ParseColor("#ffffff"));

                    ButtonOnline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonOnline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                    ButtonOffline.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonOffline.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                    switch (Build.VERSION.SdkInt)
                    {
                        case >= BuildVersionCodes.N:
                            DistanceBar.SetProgress(300, true);
                            break;
                        // For API < 24 
                        default:
                            DistanceBar.Progress = 300;
                            break;
                    }

                    //TxtDistanceCount.Text = "300 " + GetText(Resource.String.Lbl_km);

                    TxtRelationship.Text = RelationshipId switch
                    {
                        "5" => GetText(Resource.String.Lbl_All),
                        "1" => GetText(Resource.String.Lbl_Single),
                        "2" => GetText(Resource.String.Lbl_InRelationship),
                        "3" => GetText(Resource.String.Lbl_Married),
                        "4" => GetText(Resource.String.Lbl_Engaged),
                        _ => GetText(Resource.String.Lbl_All)
                    };
                }

                
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                if (text == GetText(Resource.String.Lbl_Single))
                    RelationshipId = "1";
                else if (text == GetText(Resource.String.Lbl_InRelationship))
                    RelationshipId = "2";
                else if (text == GetText(Resource.String.Lbl_Married))
                    RelationshipId = "3";
                else if (text == GetText(Resource.String.Lbl_Engaged))
                    RelationshipId = "4";
                else if (text == GetText(Resource.String.Lbl_All))
                    RelationshipId = "5";

                TxtRelationship.Text = itemString.ToString();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                if (p1 == DialogAction.Positive)
                {
                }
                else if (p1 == DialogAction.Negative)
                {
                    p0.Dismiss();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        #endregion

    }
}