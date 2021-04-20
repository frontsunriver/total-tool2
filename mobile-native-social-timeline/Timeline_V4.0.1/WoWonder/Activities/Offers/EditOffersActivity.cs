using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Offers.Adapters;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Offers;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using Console = System.Console;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Offers
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class EditOffersActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, View.IOnClickListener
    {
        #region Variables Basic

        private TextView TxtSave;
        private LinearLayout LayoutImage;
        private TextView IconDiscountType, IconDiscountItems, IconCurrency, IconDescription, IconDate, IconTime;
        private EditText TxtDiscountType, TxtDiscountItems, TxtCurrency, TxtDate, TxtTime, TxtDescription;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private DiscountTypeAdapter MAdapter;
        private string TypeDialog, CurrencyId, OfferId, AddDiscountId;
        private OfferObject OfferClass;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                Methods.App.FullScreenApp(this);

                // Create your application here
                SetContentView(Resource.Layout.CreateOffersLayout);

                OfferId = Intent?.GetStringExtra("OfferId") ?? "";

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                Get_DataOffer();
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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

        #region Menu

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                TxtSave = FindViewById<TextView>(Resource.Id.toolbar_title);
                TxtSave.Text = GetText(Resource.String.Lbl_Save);

                LayoutImage = FindViewById<LinearLayout>(Resource.Id.LayoutImage);
                LayoutImage.Visibility = ViewStates.Gone;

                IconDiscountType = FindViewById<TextView>(Resource.Id.IconDiscountType);
                TxtDiscountType = FindViewById<EditText>(Resource.Id.DiscountTypeEditText);

                MRecycler = FindViewById<RecyclerView>(Resource.Id.Recyler);
                MRecycler.Visibility = ViewStates.Gone;

                IconDiscountItems = FindViewById<TextView>(Resource.Id.IconDiscountItems);
                TxtDiscountItems = FindViewById<EditText>(Resource.Id.DiscountItemsEditText);

                IconCurrency = FindViewById<TextView>(Resource.Id.IconCurrency);
                TxtCurrency = FindViewById<EditText>(Resource.Id.CurrencyEditText);

                IconDescription = FindViewById<TextView>(Resource.Id.IconDescription);
                TxtDescription = FindViewById<EditText>(Resource.Id.DescriptionEditText);

                IconDate = FindViewById<TextView>(Resource.Id.IconDate);
                TxtDate = FindViewById<EditText>(Resource.Id.DateEditText);

                IconTime = FindViewById<TextView>(Resource.Id.IconTime);
                TxtTime = FindViewById<EditText>(Resource.Id.TimeEditText);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDiscountType, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDiscountItems, FontAwesomeIcon.MapMarkedAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCurrency, FontAwesomeIcon.DollarSign);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDescription, FontAwesomeIcon.Paragraph);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDate, FontAwesomeIcon.CalendarAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTime, FontAwesomeIcon.Clock);

                Methods.SetColorEditText(TxtDiscountType, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDiscountItems, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCurrency, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDescription, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtTime, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtDiscountType);
                Methods.SetFocusable(TxtCurrency);
                Methods.SetFocusable(TxtDate);
                Methods.SetFocusable(TxtTime);

                TxtDate.SetOnClickListener(this);
                TxtTime.SetOnClickListener(this);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InitToolbar()
        {
            try
            {
                var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = GetText(Resource.String.Lbl_EditOffers);
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                    SupportActionBar.SetHomeAsUpIndicator(AppCompatResources.GetDrawable(this, AppSettings.FlowDirectionRightToLeft ? Resource.Drawable.ic_action_right_arrow_color : Resource.Drawable.ic_action_left_arrow_color));

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
                LayoutManager = new LinearLayoutManager(this);
                MAdapter = new DiscountTypeAdapter(this) { DiscountList = new ObservableCollection<DiscountOffers>() };
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.SetAdapter(MAdapter);
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
                        TxtSave.Click += TxtSaveOnClick;
                        TxtCurrency.Touch += TxtCurrencyOnTouch;
                        TxtDiscountType.Touch += TxtDiscountTypeOnTouch;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtCurrency.Touch -= TxtCurrencyOnTouch;
                        TxtDiscountType.Touch -= TxtDiscountTypeOnTouch;
                        break;
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
                MAdapter = null!;
                TxtSave = null!;
                IconDiscountType = null!;
                TxtDiscountType = null!;
                MRecycler = null!;
                IconDiscountItems = null!;
                TxtDiscountItems = null!;
                IconCurrency = null!;
                TxtCurrency = null!;
                IconDescription = null!;
                TxtDescription = null!;
                IconDate = null!;
                TxtDate = null!;
                IconTime = null!;
                TxtTime = null!;
                OfferClass = null!;
                TypeDialog = null!;
                CurrencyId = null!;
                OfferId = null!; 
                AddDiscountId = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void TxtCurrencyOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                if (ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList != null)
                {
                    TypeDialog = "Currency";

                    var arrayAdapter = WoWonderTools.GetCurrencySymbolList();
                    switch (arrayAdapter?.Count)
                    {
                        case > 0:
                        {
                            var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                            dialogList.Title(GetText(Resource.String.Lbl_SelectCurrency)).TitleColorRes(Resource.Color.primary);
                            dialogList.Items(arrayAdapter);
                            dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                            dialogList.AlwaysCallSingleChoiceCallback();
                            dialogList.ItemsCallback(this).Build().Show();
                            break;
                        }
                    }
                }
                else
                {
                    Methods.DisplayReportResult(this, "Not have List Currency");
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtDiscountTypeOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "DiscountOffersAdapter";

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                var arrayAdapter = WoWonderTools.GetAddDiscountList(this).Select(pair => pair.Value).ToList();
                dialogList.Title(GetText(Resource.String.Lbl_DiscountType)).TitleColorRes(Resource.Color.primary);
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

        private async void TxtSaveOnClick(object sender, EventArgs e)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    if (string.IsNullOrEmpty(TxtDiscountType.Text) || string.IsNullOrEmpty(TxtDiscountItems.Text) || string.IsNullOrEmpty(TxtCurrency.Text)
                        || string.IsNullOrEmpty(TxtDescription.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_your_data), ToastLength.Short)?.Show();
                        return;
                    }

                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    OfferObject newInfoObject = new OfferObject
                    {
                        Id = OfferId,
                        DiscountType = AddDiscountId,
                        Currency = CurrencyId,
                        ExpireDate = TxtDate.Text,
                        Time = TxtTime.Text,
                        Description = TxtDescription.Text,
                        DiscountedItems = TxtDiscountItems.Text,
                    };

                    var dictionary = new Dictionary<string, string>
                    {
                        {"discount_type", AddDiscountId},
                        {"currency", CurrencyId},
                        {"offer_id", OfferId},
                        {"expire_date", TxtDate.Text},
                        {"expire_time", TxtTime.Text},
                        {"description", TxtDescription.Text},
                        {"discounted_items", TxtDiscountItems.Text},
                    };

                    switch (MAdapter.DiscountList.Count)
                    {
                        case > 0:
                        {
                            foreach (var discount in MAdapter.DiscountList)
                            {
                                switch (discount)
                                {
                                    case null:
                                        continue;
                                    default:
                                        switch (discount.DiscountType)
                                        {
                                            case "discount_percent":
                                                dictionary.Add("discount_percent", discount.DiscountFirst);

                                                newInfoObject.DiscountPercent = discount.DiscountFirst;
                                                break;
                                            case "discount_amount":
                                                dictionary.Add("discount_amount", discount.DiscountFirst);

                                                newInfoObject.DiscountAmount = discount.DiscountFirst;
                                                break;
                                            case "buy_get_discount":
                                                dictionary.Add("discount_percent", discount.DiscountFirst);
                                                dictionary.Add("buy", discount.DiscountSec);
                                                dictionary.Add("get", discount.DiscountThr);

                                                newInfoObject.DiscountPercent = discount.DiscountFirst;
                                                newInfoObject.Buy = discount.DiscountSec;
                                                newInfoObject.GetPrice = discount.DiscountThr;
                                                break;
                                            case "spend_get_off":
                                                dictionary.Add("spend", discount.DiscountSec);
                                                dictionary.Add("amount_off", discount.DiscountThr);

                                                newInfoObject.Spend = discount.DiscountSec;
                                                newInfoObject.AmountOff = discount.DiscountThr; 
                                                break;
                                            case "free_shipping": //Not have tag
                                                break;
                                        }

                                        break;
                                }
                            }

                            break;
                        }
                    }

                    var (apiStatus, respond) = await RequestsAsync.Offers.EditOfferAsync(dictionary);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageOfferObject result:
                                {
                                    Console.WriteLine(result.MessageData);
                                    Toast.MakeText(this, GetString(Resource.String.Lbl_OfferSuccessfullyAdded), ToastLength.Short)?.Show();

                                    AndHUD.Shared.Dismiss(this);

                                    var data = OffersActivity.GetInstance()?.MAdapter?.OffersList?.FirstOrDefault(a => a.Id == newInfoObject.Id);
                                    if (data != null)
                                    {
                                        data.DiscountType = AddDiscountId;
                                        data.Currency = CurrencyId;
                                        data.ExpireDate = TxtDate.Text;
                                        data.Time = TxtTime.Text;
                                        data.Description = TxtDescription.Text;
                                        data.DiscountedItems = TxtDiscountItems.Text;
                                        data.Description = TxtDescription.Text;
                                        data.DiscountPercent = newInfoObject.DiscountPercent;
                                        data.DiscountAmount = newInfoObject.DiscountAmount;
                                        data.DiscountPercent = newInfoObject.DiscountPercent;
                                        data.Buy = newInfoObject.Buy;
                                        data.GetPrice = newInfoObject.GetPrice;
                                        data.Spend = newInfoObject.Spend;
                                        data.AmountOff = newInfoObject.AmountOff;

                                        OffersActivity.GetInstance().MAdapter.NotifyItemChanged(OffersActivity.GetInstance().MAdapter.OffersList.IndexOf(data));
                                    }

                                    Intent intent = new Intent();
                                    intent.PutExtra("OffersItem", JsonConvert.SerializeObject(newInfoObject));
                                    SetResult(Result.Ok, intent);
                                    Finish();
                                    break;
                                }
                            }

                            break;
                        }
                        default:
                            Methods.DisplayAndHudErrorResult(this, respond);
                            break;
                    }
                }
                else
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }

            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                AndHUD.Shared.Dismiss(this);
            }
        }

        #endregion

        #region MaterialDialog

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

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "Currency":
                        TxtCurrency.Text = itemString.ToString();

                        var (currency, currencyIcon) = WoWonderTools.GetCurrency(itemId.ToString());
                        CurrencyId = currency;
                        Console.WriteLine(currencyIcon);
                        break;
                    case "DiscountOffersAdapter":
                    {
                        AddDiscountId = WoWonderTools.GetAddDiscountList(this)?.FirstOrDefault(a => a.Value == itemString.ToString()).Key.ToString();

                        TxtDiscountType.Text = itemString.ToString();

                        switch (AddDiscountId)
                        {
                            case "free_shipping":
                                MRecycler.Visibility = ViewStates.Gone;
                                MAdapter.DiscountList.Clear();
                                MAdapter.NotifyDataSetChanged();
                                break;
                            default:
                                MRecycler.Visibility = ViewStates.Visible;
                                MAdapter.DiscountList.Clear();

                                MAdapter.DiscountList.Add(new DiscountOffers
                                {
                                    DiscountType = AddDiscountId,
                                    DiscountFirst = "",
                                    DiscountSec = "",
                                    DiscountThr = "",
                                });
                                MAdapter.NotifyDataSetChanged();
                                break;
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
        public void OnClick(View v)
        {
            try
            {
                if (v.Id == TxtTime.Id)
                {
                    var frag = PopupDialogController.TimePickerFragment.NewInstance(delegate (DateTime time)
                    {
                        TxtTime.Text = time.ToShortTimeString();
                    });

                    frag.Show(SupportFragmentManager, PopupDialogController.TimePickerFragment.Tag);
                }
                else if (v.Id == TxtDate.Id)
                {
                    var frag = PopupDialogController.DatePickerFragment.NewInstance(delegate (DateTime time)
                    {
                        TxtDate.Text = time.Date.ToString("yyyy-MM-dd");
                    });

                    frag.Show(SupportFragmentManager, PopupDialogController.DatePickerFragment.Tag);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void Get_DataOffer()
        {
            try
            {
                OfferClass = JsonConvert.DeserializeObject<OfferObject>(Intent?.GetStringExtra("OfferItem"));
                if (OfferClass != null)
                {
                    AddDiscountId = OfferClass.DiscountType;
                    switch (OfferClass.DiscountType)
                    {
                        case "discount_percent":
                        {
                            TxtDiscountType.Text = GetString(Resource.String.Lbl_DiscountPercent);
                            MRecycler.Visibility = ViewStates.Visible;
                            MAdapter.DiscountList.Clear();

                            MAdapter.DiscountList.Add(new DiscountOffers
                            {
                                DiscountType = OfferClass.DiscountType,
                                DiscountFirst = OfferClass.DiscountPercent,
                                DiscountSec = "",
                                DiscountThr = "",
                            });
                            MAdapter.NotifyDataSetChanged();
                        } break;
                        case "discount_amount":
                        {
                            TxtDiscountType.Text = GetString(Resource.String.Lbl_DiscountAmount);

                            MRecycler.Visibility = ViewStates.Visible;
                            MAdapter.DiscountList.Clear();

                            MAdapter.DiscountList.Add(new DiscountOffers
                            {
                                DiscountType = OfferClass.DiscountType,
                                DiscountFirst = OfferClass.DiscountAmount,
                                DiscountSec = "",
                                DiscountThr = "",
                            });
                            MAdapter.NotifyDataSetChanged();
                        } break;
                        case "buy_get_discount":
                        {
                            TxtDiscountType.Text = GetString(Resource.String.Lbl_BuyGetDiscount);

                            MRecycler.Visibility = ViewStates.Visible;
                            MAdapter.DiscountList.Clear();

                            MAdapter.DiscountList.Add(new DiscountOffers
                            {
                                DiscountType = OfferClass.DiscountType,
                                DiscountFirst = OfferClass.DiscountPercent,
                                DiscountSec = OfferClass.Buy,
                                DiscountThr = OfferClass.GetPrice,
                            });
                            MAdapter.NotifyDataSetChanged();
                        } break;
                        case "spend_get_off":
                        {
                            TxtDiscountType.Text = GetString(Resource.String.Lbl_SpendGetOff);

                            MRecycler.Visibility = ViewStates.Visible;
                            MAdapter.DiscountList.Clear();

                            MAdapter.DiscountList.Add(new DiscountOffers
                            {
                                DiscountType = OfferClass.DiscountType,
                                DiscountFirst = "",
                                DiscountSec = OfferClass.Spend,
                                DiscountThr = OfferClass.AmountOff,
                            });
                            MAdapter.NotifyDataSetChanged();
                        } break;
                        case "free_shipping": //Not have tag
                        {
                            TxtDiscountType.Text = GetString(Resource.String.Lbl_FreeShipping);

                            MRecycler.Visibility = ViewStates.Gone;
                            MAdapter.DiscountList.Clear();
                            MAdapter.NotifyDataSetChanged();
                        } break;
                    }
                     
                    TxtDiscountItems.Text = Methods.FunString.DecodeString(OfferClass.DiscountedItems);
                    TxtCurrency.Text = OfferClass.Currency;
                    TxtDate.Text = OfferClass.ExpireDate; 
                    TxtTime.Text = OfferClass.ExpireTime;
                    TxtDescription.Text = Methods.FunString.DecodeString(OfferClass.Description);

                    CurrencyId = OfferClass.Currency;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}