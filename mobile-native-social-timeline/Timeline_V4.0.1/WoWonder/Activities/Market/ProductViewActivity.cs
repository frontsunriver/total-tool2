using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager.Widget;
using Java.Lang;
using Me.Relex.CircleIndicatorLib;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.Activities.Market.Adapters;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Tabbes;
using WoWonder.Activities.UsersPages;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo;
using WoWonder.Library.Anjo.SuperTextLibrary;
using WoWonderClient.Classes.Comments;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Message;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Product;
using WoWonderClient.Requests; 
using Exception = System.Exception;
using String = Java.Lang.String;

namespace WoWonder.Activities.Market
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class ProductViewActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtProductPrice, TxtProductNew, TxtProductInStock,  TxtProductLocation, TxtProductCardName, TxtProductTime;
        private SuperTextView TxtProductDescription;
        private ImageView ImageMore, UserImageAvatar, IconBack;
        private Button BtnContact;
        private ProductDataObject ProductData;
        private ViewPager ViewPagerView;
        private CircleIndicator CircleIndicatorView;
        private string TypeDialog, PostId;
        private RecyclerView CommentsRecyclerView;
        private CommentAdapter MAdapter;

        private ReactButton LikeButton;
        private LinearLayout BtnLike, BtnComment, BtnWonder;
        private ImageView ImgWonder;
        private TextView TxtWonder;
        private LinearLayout MainSectionButton;
        private PostClickListener ClickListener;
        private PostDataObject PostData;

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
                SetContentView(Resource.Layout.ProductView_Layout);

                PostId = Intent?.GetStringExtra("Id") ?? string.Empty;

                ClickListener = new PostClickListener(this, NativeFeedType.Global);

                //Get Value And Set Toolbar
                InitComponent();
                SetRecyclerViewAdapters();

                ProductData = JsonConvert.DeserializeObject<ProductDataObject>(Intent?.GetStringExtra("ProductView") ?? "");
                switch (ProductData)
                {
                    case null:
                        return;
                    default:
                        Get_Data_Product(ProductData);
                        break;
                }
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
                ViewPagerView = FindViewById<ViewPager>(Resource.Id.pager);
                CircleIndicatorView = FindViewById<CircleIndicator>(Resource.Id.indicator);
                 
                TxtProductPrice = (TextView)FindViewById(Resource.Id.tv_price);
                TxtProductNew = (TextView)FindViewById(Resource.Id.BoleanNew);
                TxtProductInStock = (TextView)FindViewById(Resource.Id.BoleanInStock);
                TxtProductDescription = (SuperTextView)FindViewById(Resource.Id.tv_description);
                TxtProductLocation = (TextView)FindViewById(Resource.Id.tv_Location);
                TxtProductCardName = (TextView)FindViewById(Resource.Id.card_name);
                TxtProductTime = (TextView)FindViewById(Resource.Id.card_dist);

                BtnContact = (Button)FindViewById(Resource.Id.cont);

                UserImageAvatar = (ImageView)FindViewById(Resource.Id.card_pro_pic);
                ImageMore = (ImageView)FindViewById(Resource.Id.Image_more);
                IconBack = (ImageView)FindViewById(Resource.Id.iv_back);


                BtnLike = FindViewById<LinearLayout>(Resource.Id.linerlike);
                BtnComment = FindViewById<LinearLayout>(Resource.Id.linercomment);

                MainSectionButton = FindViewById<LinearLayout>(Resource.Id.mainsection);
                BtnWonder = FindViewById<LinearLayout>(Resource.Id.linerSecondReaction);
                ImgWonder = FindViewById<ImageView>(Resource.Id.image_SecondReaction);
                TxtWonder = FindViewById<TextView>(Resource.Id.SecondReactionText);

                LikeButton = FindViewById<ReactButton>(Resource.Id.ReactButton);

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.ReactionDefault:
                    case PostButtonSystem.ReactionSubShine:
                    case PostButtonSystem.Like:
                        MainSectionButton.WeightSum = 2;
                        BtnWonder.Visibility = ViewStates.Gone;
                        break;
                    case PostButtonSystem.Wonder:
                        MainSectionButton.WeightSum = 3;
                        BtnWonder.Visibility = ViewStates.Visible;

                        ImgWonder.SetImageResource(Resource.Drawable.ic_action_wowonder);
                        TxtWonder.Text = Application.Context.GetText(Resource.String.Btn_Wonder);
                        break;
                    case PostButtonSystem.DisLike:
                        MainSectionButton.WeightSum = 3;
                        BtnWonder.Visibility = ViewStates.Visible;

                        ImgWonder.SetImageResource(Resource.Drawable.ic_action_dislike);
                        TxtWonder.Text = Application.Context.GetText(Resource.String.Btn_Dislike);
                        break;
                }

                switch (AppSettings.SetTabDarkTheme)
                {
                    case false:
                        ImageMore.SetColorFilter(Color.Black);
                        break;
                }

                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        IconBack.SetImageResource(Resource.Drawable.ic_action_ic_back_rtl);
                        break;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void ViewPagerViewOnClick()
        { 
            try
            {
                var intent = new Intent(this, typeof(MultiImagesPostViewerActivity));
                intent.PutExtra("indexImage", "0"); // Index Image Show
                intent.PutExtra("TypePost", "Product"); // Index Image Show
                intent.PutExtra("AlbumObject", JsonConvert.SerializeObject(PostData)); // PostDataObject
                OverridePendingTransition(Resource.Animation.abc_popup_enter, Resource.Animation.popup_exit);
                StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void SetRecyclerViewAdapters()
        {
            try
            { 
                CommentsRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);
                CommentsRecyclerView.NestedScrollingEnabled = false;
                MAdapter = new CommentAdapter(this)
                {
                    CommentList = new ObservableCollection<CommentObjectExtra>()
                };

                StartApiService();
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
                    {
                        BtnContact.Click += BtnContactOnClick;
                        UserImageAvatar.Click += UserImageAvatarOnClick;
                        TxtProductCardName.Click += UserImageAvatarOnClick; 
                        ImageMore.Click += ImageMoreOnClick;
                        IconBack.Click += IconBackOnClick;
                        TxtProductDescription.LongClick += TxtProductDescriptionOnLongClick;
                        BtnComment.Click += BtnCommentOnClick;

                        switch (AppSettings.PostButton)
                        {
                            case PostButtonSystem.Wonder:
                            case PostButtonSystem.DisLike:
                                BtnWonder.Click += BtnWonderOnClick;
                                break;
                        }

                        LikeButton.Click += (sender, args) => LikeButton.ClickLikeAndDisLike(new GlobalClickEventArgs
                        {
                            NewsFeedClass = PostData,
                        } , null, "ProductViewActivity");

                        switch (AppSettings.PostButton)
                        {
                            case PostButtonSystem.ReactionDefault:
                            case PostButtonSystem.ReactionSubShine:
                                LikeButton.LongClick += (sender, args) => LikeButton.LongClickDialog(new GlobalClickEventArgs
                                {
                                    NewsFeedClass = PostData,
                                }, null, "ProductViewActivity");
                                break;
                        }
                        break;
                    }
                    default:
                    {
                        BtnContact.Click -= BtnContactOnClick;
                        UserImageAvatar.Click -= UserImageAvatarOnClick;
                        TxtProductCardName.Click -= UserImageAvatarOnClick; 
                        ImageMore.Click -= ImageMoreOnClick;
                        IconBack.Click -= IconBackOnClick;
                        TxtProductDescription.LongClick -= TxtProductDescriptionOnLongClick;
                        BtnComment.Click -= BtnCommentOnClick;

                        switch (AppSettings.PostButton)
                        {
                            case PostButtonSystem.Wonder:
                            case PostButtonSystem.DisLike:
                                BtnWonder.Click -= BtnWonderOnClick;
                                break;
                        }

                        LikeButton.Click += null!;
                        switch (AppSettings.PostButton)
                        {
                            case PostButtonSystem.ReactionDefault:
                            case PostButtonSystem.ReactionSubShine:
                                LikeButton.LongClick -= null!;
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
        private void DestroyBasic()
        {
            try
            {
                ViewPagerView = null!;
                CircleIndicatorView = null!;
                TxtProductPrice = null!;
                TxtProductNew = null!;
                TxtProductInStock = null!;
                TxtProductDescription = null!;
                TxtProductLocation = null!;
                TxtProductCardName = null!;
                TxtProductTime = null!;
                BtnContact = null!;
                UserImageAvatar = null!;
                ImageMore = null!;
                IconBack  = null!;
                BtnLike = null!;
                BtnComment = null!;
                MainSectionButton = null!;
                BtnWonder = null!;
                ImgWonder = null!;
                TxtWonder = null!;
                LikeButton = null!;
                ClickListener = null!;
                PostData = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void TxtProductDescriptionOnLongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                if (Methods.FunString.StringNullRemover(ProductData.Description) != "Empty")
                {
                    Methods.CopyToClipboard(this, ProductData.Description); 
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Contact seller 
        private void BtnContactOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                switch (AppSettings.MessengerIntegration)
                {
                    case true when AppSettings.ShowDialogAskOpenMessenger:
                    {
                        var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        dialog.Title(Resource.String.Lbl_Warning).TitleColorRes(Resource.Color.primary);
                        dialog.Content(GetText(Resource.String.Lbl_ContentAskOPenAppMessenger));
                        dialog.PositiveText(GetText(Resource.String.Lbl_Yes)).OnPositive((materialDialog, action) =>
                        {
                            try
                            {
                                Methods.App.OpenAppByPackageName(this, AppSettings.MessengerPackageName, "SendMsgProduct", new ChatObject { UserId = ProductData.Seller.UserId, Avatar = ProductData.Seller.Avatar, Name = ProductData.Seller.Name, LastMessage = new LastMessageUnion { LastMessageClass = new MessageData { ProductId = ProductData.Id, Product = new ProductUnion { ProductClass = ProductData } } } });
                            }
                            catch (Exception exception)
                            {
                                Methods.DisplayReportResultTrack(exception);
                            }
                        });
                        dialog.NegativeText(GetText(Resource.String.Lbl_No)).OnNegative(this);
                        dialog.AlwaysCallSingleChoiceCallback();
                        dialog.Build().Show();
                        break;
                    }
                    case true:
                        Methods.App.OpenAppByPackageName(this, AppSettings.MessengerPackageName, "SendMsgProduct", new ChatObject { UserId = ProductData.Seller.UserId, Avatar = ProductData.Seller.Avatar, Name = ProductData.Seller.Name, LastMessage = new LastMessageUnion { LastMessageClass = new MessageData { ProductId = ProductData.Id, Product = new ProductUnion { ProductClass = ProductData } } } });
                        break;
                    default:
                    {
                        if (!Methods.CheckConnectivity())
                        {
                            Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                            return;
                        }

                        var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        var time = unixTimestamp.ToString();

                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Message.SendMessageAsync(ProductData.Seller.UserId, time, "", "", "", "", "", "", ProductData.Id) });
                        Toast.MakeText(this, GetString(Resource.String.Lbl_MessageSentSuccessfully), ToastLength.Short)?.Show();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        // Event Open User Profile
        private void UserImageAvatarOnClick(object sender, EventArgs e)
        {
            try
            {
                WoWonderTools.OpenProfile(this, ProductData.Seller.UserId, ProductData.Seller);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        //Event Back
        private void IconBackOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                Finish();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event More >> Show Menu (CopeLink , Share)
        private void ImageMoreOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                if (ProductData.Seller.UserId == UserDetails.UserId)
                {
                    arrayAdapter.Add(GetString(Resource.String.Lbl_EditProduct));
                    arrayAdapter.Add(GetString(Resource.String.Lbl_Delete));
                }
                
                arrayAdapter.Add(GetString(Resource.String.Lbl_CopeLink));
                arrayAdapter.Add(GetString(Resource.String.Lbl_Share));
                 
                dialogList.Title(GetString(Resource.String.Lbl_More)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show(); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Copy Link
        private void OnCopyLink_Button_Click()
        {
            try
            {
                Methods.CopyToClipboard(this, ProductData.Url); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Share
        private void OnShare_Button_Click()
        {
            try
            {
                ClickListener.SharePostClick(new GlobalClickEventArgs { NewsFeedClass = PostData, }, PostModelType.ProductPost);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        //Event Menu >> Edit Info Product if user == is_owner  
        private void EditInfoProduct_OnClick()
        {
            try
            {
                Intent intent = new Intent(this, typeof(EditProductActivity));
                intent.PutExtra("ProductView", JsonConvert.SerializeObject(ProductData));
                StartActivityForResult(intent, 3500);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        //Event Menu >> Edit Info Product if user == is_owner  
        private void DeleteProduct_OnClick()
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                   TypeDialog = "DeletePost";

                    var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                    dialog.Title(GetText(Resource.String.Lbl_DeletePost)).TitleColorRes(Resource.Color.primary);
                    dialog.Content(GetText(Resource.String.Lbl_AreYouSureDeletePost));
                    dialog.PositiveText(GetText(Resource.String.Lbl_Yes)).OnPositive(this);
                    dialog.NegativeText(GetText(Resource.String.Lbl_No)).OnNegative(this);
                    dialog.AlwaysCallSingleChoiceCallback();
                    dialog.ItemsCallback(this).Build().Show();
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
          
        //Event Add Comment
        private void BtnCommentOnClick(object sender, EventArgs e)
        {
            try
            {
                ClickListener.CommentPostClick(new GlobalClickEventArgs
                {
                    NewsFeedClass = PostData,
                });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Add Wonder / Disliked
        private void BtnWonderOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Application.Context, Application.Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }
                 

                if (PostData.IsWondered != null && (bool)PostData.IsWondered)
                {
                    var x = Convert.ToInt32(PostData.PostWonders);
                    switch (x)
                    {
                        case > 0:
                            x--;
                            break;
                        default:
                            x = 0;
                            break;
                    }

                    ImgWonder.SetColorFilter(Color.White);

                    PostData.IsWondered = false;
                    PostData.PostWonders = Convert.ToString(x, CultureInfo.InvariantCulture);

                    TxtWonder.Text = AppSettings.PostButton switch
                    {
                        PostButtonSystem.Wonder => GetText(Resource.String.Btn_Wonder),
                        PostButtonSystem.DisLike => GetText(Resource.String.Btn_Dislike),
                        _ => TxtWonder.Text
                    };

                    BtnWonder.Tag = "false";
                }
                else
                { 
                    var x = Convert.ToInt32(PostData.PostWonders);
                    x++;

                    PostData.IsWondered = true;
                    PostData.PostWonders = Convert.ToString(x, CultureInfo.InvariantCulture);

                    ImgWonder.SetColorFilter(Color.ParseColor("#f89823"));

                    TxtWonder.Text = AppSettings.PostButton switch
                    {
                        PostButtonSystem.Wonder => GetText(Resource.String.Lbl_wondered),
                        PostButtonSystem.DisLike => GetText(Resource.String.Lbl_disliked),
                        _ => TxtWonder.Text
                    };

                    BtnWonder.Tag = "true";
                }

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.Wonder:
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.PostId, "wonder") });
                        break;
                    case PostButtonSystem.DisLike:
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.PostId, "dislike") });
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
 
        #endregion

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                if (text == GetString(Resource.String.Lbl_CopeLink))
                {
                    OnCopyLink_Button_Click();
                }
                else if (text == GetString(Resource.String.Lbl_Share))
                {
                    OnShare_Button_Click();
                }
                else if (text == GetString(Resource.String.Lbl_EditProduct))
                {
                    EditInfoProduct_OnClick();
                }
                else if (text == GetString(Resource.String.Lbl_Delete))
                {
                     DeleteProduct_OnClick();
                }
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
                switch (TypeDialog)
                {
                    case "DeletePost":
                        try
                        {
                            if (!Methods.CheckConnectivity())
                            {
                                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                                return;
                            }

                            var adapterGlobal = WRecyclerView.GetInstance()?.NativeFeedAdapter;
                            var diff = adapterGlobal?.ListDiffer;
                            var dataGlobal = diff?.Where(a => a.PostData?.PostId == ProductData?.PostId).ToList();
                            if (dataGlobal != null)
                            {
                                foreach (var postData in dataGlobal)
                                {
                                    WRecyclerView.GetInstance()?.RemoveByRowIndex(postData);
                                }
                            }

                            var recycler = TabbedMainActivity.GetInstance()?.NewsFeedTab?.MainRecyclerView;
                            var dataGlobal2 = recycler?.NativeFeedAdapter.ListDiffer?.Where(a => a.PostData?.PostId == ProductData?.PostId).ToList();
                            if (dataGlobal2 != null)
                            {
                                foreach (var postData in dataGlobal2)
                                {
                                    recycler.RemoveByRowIndex(postData);
                                }
                            }

                            if (TabbedMarketActivity.GetInstance()?.MyProductsTab?.MAdapter?.MarketList != null)
                            {
                                TabbedMarketActivity.GetInstance().MyProductsTab.MAdapter.MarketList?.Remove(ProductData);
                                TabbedMarketActivity.GetInstance().MyProductsTab.MAdapter.NotifyDataSetChanged();
                            }

                            if (TabbedMarketActivity.GetInstance()?.MarketTab?.MAdapter?.MarketList != null)
                            {
                                TabbedMarketActivity.GetInstance().MarketTab.MAdapter.MarketList?.Remove(ProductData);
                                TabbedMarketActivity.GetInstance().MarketTab.MAdapter.NotifyDataSetChanged();
                            }
                         
                            Toast.MakeText(this, GetText(Resource.String.Lbl_postSuccessfullyDeleted), ToastLength.Short)?.Show();
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(ProductData.PostId, "delete") }); 

                            Finish();
                        }
                        catch (Exception e)
                        {
                            Methods.DisplayReportResultTrack(e);
                        }

                        break;
                    default:
                    {
                        if (p1 == DialogAction.Positive)
                        {
                        }
                        else if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
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

        #region Result

        //Result
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

                switch (requestCode)
                {
                    case 3500 when resultCode == Result.Ok:
                    {
                        if (string.IsNullOrEmpty(data.GetStringExtra("itemData"))) return;
                        var item = JsonConvert.DeserializeObject<ProductDataObject>(data.GetStringExtra("itemData"));
                        if (item != null)
                        {
                            ProductData = item;
                            Get_Data_Product(item);
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

        private void Get_Data_Product(ProductDataObject productData)
        {
            try
            {
                ProductData = productData;

                PostData = new PostDataObject
                {
                    PostId = productData.PostId,
                    Product = new ProductUnion
                    {
                        ProductClass = productData,
                    },
                    ProductId = productData.Id,
                    UserId = productData.UserId,
                    UserData = productData.Seller,
                    Url = productData.Url,
                    PostUrl = productData.Url,
                };

                List<string> listImageUser = new List<string>();
                switch (productData.Images?.Count)
                {
                    case > 0:
                        listImageUser.AddRange(productData.Images.Select(t => t.Image));
                        break;
                    default:
                        listImageUser.Add(productData.Images?[0]?.Image);
                        break;
                }

                switch (ViewPagerView.Adapter)
                {
                    case null:
                        ViewPagerView.Adapter = new MultiImagePagerAdapter(this, listImageUser);
                        ViewPagerView.CurrentItem = 0;
                        CircleIndicatorView.SetViewPager(ViewPagerView);
                        break;
                }
                ViewPagerView.Adapter.NotifyDataSetChanged();

                GlideImageLoader.LoadImage(this, productData.Seller.Avatar, UserImageAvatar, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                     
                var (currency, currencyIcon) = WoWonderTools.GetCurrency(productData.Currency); 
                TxtProductPrice.Text = productData.Price + " " + currencyIcon;
              
                Console.WriteLine(currency);
                var readMoreOption = new StReadMoreOption.Builder()
                    .TextLength(200, StReadMoreOption.TypeCharacter)
                    .MoreLabel(GetText(Resource.String.Lbl_ReadMore))
                    .LessLabel(GetText(Resource.String.Lbl_ReadLess))
                    .MoreLabelColor(Color.ParseColor(AppSettings.MainColor))
                    .LessLabelColor(Color.ParseColor(AppSettings.MainColor))
                    .LabelUnderLine(true)
                    .Build();

                if (Methods.FunString.StringNullRemover(productData.Description) != "Empty")
                {
                    var description =Methods.FunString.DecodeString(productData.Description); 
                     readMoreOption.AddReadMoreTo(TxtProductDescription, new String(description));
                }
                else
                {
                    TxtProductDescription.Text = GetText(Resource.String.Lbl_Empty);
                }

                TxtProductLocation.Text = Methods.FunString.DecodeString(productData.Location); 
                TxtProductCardName.Text = Methods.FunString.SubStringCutOf(WoWonderTools.GetNameFinal(productData.Seller), 14);
                TxtProductTime.Text = productData.TimeText;

                if (productData.Seller.UserId == UserDetails.UserId)
                    BtnContact.Visibility = ViewStates.Gone;
                 
                //Type == "0" >>  New // Type != "0"  Used
                TxtProductNew.Visibility = productData.Type == "0" ? ViewStates.Visible : ViewStates.Gone;

                // Status InStock
                TxtProductInStock.Visibility = productData.Status == "0" ? ViewStates.Visible : ViewStates.Gone;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else 
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { LoadPostDataAsync, () => LoadDataComment("0") });
        }

        private async Task LoadPostDataAsync()
        {
            if (Methods.CheckConnectivity())
            {
                var (apiStatus, respond) = await RequestsAsync.Posts.GetPostDataAsync(PostId, "post_data");
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetPostDataObject result:
                            {
                                PostData = result.PostData;

                                if (PostData.IsLiked != null && (bool)PostData.IsLiked)
                                {
                                    BtnLike.Tag = "true";
                                }
                                else
                                {
                                    BtnLike.Tag = "false";
                                }

                                if (PostData.IsWondered != null && (bool)PostData.IsWondered)
                                {
                                    BtnWonder.Tag = "true";
                                    ImgWonder.SetColorFilter(Color.ParseColor("#f89823"));

                                    TxtWonder.Text = GetText(Resource.String.Lbl_wondered);
                                }
                                else
                                {
                                    BtnWonder.Tag = "false";
                                    ImgWonder.SetColorFilter(Color.White);
                                    TxtWonder.Text = GetText(Resource.String.Btn_Wonder);

                                }

                                switch (AppSettings.PostButton)
                                {
                                    case PostButtonSystem.ReactionDefault:
                                    case PostButtonSystem.ReactionSubShine:
                                    {
                                        PostData.Reaction ??= new WoWonderClient.Classes.Posts.Reaction();

                                        if (PostData.Reaction.IsReacted != null && PostData.Reaction.IsReacted.Value)
                                        {
                                            switch (string.IsNullOrEmpty(PostData.Reaction.Type))
                                            {
                                                case false:
                                                {
                                                    var react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Id == PostData.Reaction.Type).Value?.Id ?? "";
                                                    switch (react)
                                                    {
                                                        case "1":
                                                            LikeButton.SetReactionPack(ReactConstants.Like);
                                                            break;
                                                        case "2":
                                                            LikeButton.SetReactionPack(ReactConstants.Love);
                                                            break;
                                                        case "3":
                                                            LikeButton.SetReactionPack(ReactConstants.HaHa);
                                                            break;
                                                        case "4":
                                                            LikeButton.SetReactionPack(ReactConstants.Wow);
                                                            break;
                                                        case "5":
                                                            LikeButton.SetReactionPack(ReactConstants.Sad);
                                                            break;
                                                        case "6":
                                                            LikeButton.SetReactionPack(ReactConstants.Angry);
                                                            break;
                                                        default:
                                                            LikeButton.SetReactionPack(ReactConstants.Default);
                                                            break; 
                                                    }

                                                    break;
                                                }
                                            }
                                        }

                                        break;
                                    }
                                    case PostButtonSystem.Wonder when PostData.IsWondered != null && (bool)PostData.IsWondered:
                                        ImgWonder.SetImageResource(Resource.Drawable.ic_action_wowonder);
                                        ImgWonder.SetColorFilter(Color.ParseColor(AppSettings.MainColor));

                                        TxtWonder.Text = GetString(Resource.String.Lbl_wondered);
                                        TxtWonder.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                        break;
                                    case PostButtonSystem.Wonder:
                                        ImgWonder.SetImageResource(Resource.Drawable.ic_action_wowonder);
                                        ImgWonder.SetColorFilter(Color.ParseColor("#666666"));

                                        TxtWonder.Text = GetString(Resource.String.Btn_Wonder);
                                        TxtWonder.SetTextColor(Color.ParseColor("#444444"));
                                        break;
                                    case PostButtonSystem.DisLike when PostData.IsWondered != null && (bool)PostData.IsWondered:
                                        ImgWonder.SetImageResource(Resource.Drawable.ic_action_dislike);
                                        ImgWonder.SetColorFilter(Color.ParseColor(AppSettings.MainColor));

                                        TxtWonder.Text = GetString(Resource.String.Lbl_disliked);
                                        TxtWonder.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                        break;
                                    case PostButtonSystem.DisLike:
                                        ImgWonder.SetImageResource(Resource.Drawable.ic_action_dislike);
                                        ImgWonder.SetColorFilter(Color.ParseColor("#666666"));

                                        TxtWonder.Text = GetString(Resource.String.Btn_Dislike);
                                        TxtWonder.SetTextColor(Color.ParseColor("#444444"));
                                        break;
                                }

                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, respond);
                        break;
                }
            }
            else
            { 
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            }
        }
         
        #region LoadDataComment

        private async Task LoadDataComment(string offset)
        {
            if (Methods.CheckConnectivity())
            {
                var countList = MAdapter.CommentList.Count;
                var (apiStatus, respond) = await RequestsAsync.Comment.GetPostCommentsAsync(PostId, "10", offset);
                if (apiStatus != 200 || respond is not CommentObject result || result.CommentList == null)
                {
                    Methods.DisplayReportResult(this, respond);
                }
                else
                {
                    var respondList = result.CommentList?.Count;
                    switch (respondList)
                    {
                        case > 0:
                        {
                            foreach (var item in from item in result.CommentList let check = MAdapter.CommentList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                            {
                                var db = ClassMapper.Mapper?.Map<CommentObjectExtra>(item);
                                if (db != null) MAdapter.CommentList.Add(db);
                            }

                            switch (countList)
                            {
                                case > 0:
                                    RunOnUiThread(() => { MAdapter.NotifyItemRangeInserted(countList, MAdapter.CommentList.Count - countList); });
                                    break;
                                default:
                                    RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });
                                    break;
                            }

                            break;
                        }
                    }
                }

                RunOnUiThread(ShowEmptyPage2);
            }
        }

        private void ShowEmptyPage2()
        {
            try
            {
                switch (MAdapter.CommentList.Count)
                {
                    case > 0:
                    {
                        CommentsRecyclerView.Visibility = ViewStates.Visible;

                        var emptyStateChecker = MAdapter.CommentList.FirstOrDefault(a => a.Text == MAdapter.EmptyState);
                        if (emptyStateChecker != null && MAdapter.CommentList.Count > 1)
                        {
                            MAdapter.CommentList.Remove(emptyStateChecker);
                            MAdapter.NotifyDataSetChanged();
                        }

                        break;
                    }
                    default:
                    {
                        CommentsRecyclerView.Visibility = ViewStates.Gone;

                        MAdapter.CommentList.Clear();
                        var d = new CommentObjectExtra { Text = MAdapter.EmptyState };
                        MAdapter.CommentList.Add(d);
                        MAdapter.NotifyDataSetChanged();
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
    
    }
}