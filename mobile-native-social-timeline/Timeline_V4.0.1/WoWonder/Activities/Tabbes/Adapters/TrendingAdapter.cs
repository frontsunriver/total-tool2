using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Util;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Bumptech.Glide.Util;
using Newtonsoft.Json;
using WoWonder.Activities.Articles;
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Communities.Pages;
using WoWonder.Activities.General; 
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using WoWonderClient.Classes.Global;
using ImageViews.Rounded;
using Android.Graphics.Drawables;
using WoWonder.Helpers.Controller;

namespace WoWonder.Activities.Tabbes.Adapters
{
    public class TrendingAdapter : RecyclerView.Adapter
    {
        public event EventHandler<TrendingAdapterClickEventArgs> ItemClick;
        public event EventHandler<TrendingAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<TrendingAdapterClickEventArgs> ItemUserClick;

        public readonly Activity ActivityContext; 
        
        public ObservableCollection<Classes.TrendingClass> TrendingList = new ObservableCollection<Classes.TrendingClass>();
        private RecyclerView.RecycledViewPool RecycledViewPool { get; set; }

        private ProUsersAdapter ProUsersAdapter;
        private ProPagesAdapter ProPagesAdapter; 
        private ShortcutsAdapter ShortcutsAdapter; 
         
        public TrendingAdapter(Activity context)
        {
            try
            {
                ActivityContext = context; 
                RecycledViewPool = new RecyclerView.RecycledViewPool();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                switch (viewType)
                {
                    case (int)Classes.ItemType.ProUser:
                    case (int)Classes.ItemType.ProPage:
                    case (int)Classes.ItemType.Shortcuts:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.TemplateRecyclerViewLayout, parent, false);
                        var vh = new TemplateRecyclerViewHolder(itemView, OnClick, OnLongClick);
                        RecycledViewPool = new RecyclerView.RecycledViewPool();
                        vh.MRecycler.SetRecycledViewPool(RecycledViewPool);
                        return vh;
                    }
                    case (int)Classes.ItemType.HashTag: 
                    {
                        var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_TrendingHashTagView, parent, false);
                        var vh = new TrendingSearchAdapterViewHolder(itemView, OnClick, OnLongClick);
                        return vh;
                    }  
                    case (int)Classes.ItemType.FriendRequest: 
                    {
                        var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_FriendRequest, parent, false);
                        var vh = new FriendRequestViewHolder(itemView, OnClick, OnLongClick);
                        return vh;
                    } 
                    case (int)Classes.ItemType.Weather: 
                    {
                        var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Weather, parent, false);
                        var vh = new WeatherViewHolder(itemView, OnClick, OnLongClick);
                        return vh;
                    }
                    case (int)Classes.ItemType.LastActivities:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_LastActivities_View, parent, false);
                        var vh = new ActivitiesAdapterViewHolder(itemView, OnClick, OnLongClick);
                        return vh;
                    }
                    case (int)Classes.ItemType.Section:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Section, parent, false);
                        var vh = new SectionViewHolder(itemView, OnClick, OnLongClick);
                        return vh;
                    }
                    case (int)Classes.ItemType.AdMob:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.PostType_AdMob3, parent, false);
                        var vh = new AdapterHolders.AdMob3AdapterViewHolder(itemView);
                        return vh;
                    }
                    case (int)Classes.ItemType.Divider:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Devider, parent, false); 
                        var vh = new AdapterHolders.PostDividerSectionViewHolder(itemView);
                        return vh;
                    }
                    case (int)Classes.ItemType.EmptyPage:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.EmptyStateLayout, parent, false);
                        var vh = new EmptyStateViewHolder(itemView);
                        return vh;
                    }
                    case (int)Classes.ItemType.LastBlogs:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Article_View, parent, false);
                        var vh = new ArticlesAdapterViewHolder(itemView, onUserClick, OnClick, OnLongClick);
                        return vh;
                    }
                    case (int)Classes.ItemType.ExchangeCurrency:
                    {
                        View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_ExchangeCurrency, parent, false);
                        var vh = new ExchangeCurrencyViewHolder(itemView);
                        return vh;
                    } 
                    case (int)Classes.ItemType.CoronaVirus:
                    {
                        View  itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_AlertCoronaVirus, parent, false);
                        var vh = new AlertCoronaVirusAdapterViewHolder(itemView, OnClick, OnLongClick);
                        return vh;
                    }

                    default:
                        return null!;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                var item = TrendingList[position];
                if (item != null)
                {
                    switch (item.Type)
                    {
                        case Classes.ItemType.ProUser:
                        {
                            switch (viewHolder)
                            {
                                case TemplateRecyclerViewHolder holder:
                                {
                                    switch (ProUsersAdapter)
                                    {
                                        case null:
                                        {
                                            ProUsersAdapter = new ProUsersAdapter(ActivityContext)
                                            {
                                                MProUsersList = new ObservableCollection<UserDataObject>()
                                            };

                                            LinearLayoutManager layoutManager = new LinearLayoutManager(ActivityContext, LinearLayoutManager.Horizontal, false);
                                            holder.MRecycler.SetLayoutManager(layoutManager);
                                            holder.MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;

                                            var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                                            var preLoader = new RecyclerViewPreloader<UserDataObject>(ActivityContext, ProUsersAdapter, sizeProvider, 10);
                                            holder.MRecycler.AddOnScrollListener(preLoader);
                                            holder.MRecycler.SetAdapter(ProUsersAdapter);
                                            holder.MRecycler.AddOnItemTouchListener(new RecyclerViewOnItemTouch(holder.MRecycler, TabbedMainActivity.GetInstance()?.ViewPager));
                                            ProUsersAdapter.ItemClick += ProUsersAdapterOnItemClick;
                                         
                                            holder.TitleText.Text = ActivityContext.GetText(Resource.String.Lbl_Pro_Users);
                                            holder.MoreText.Visibility = ViewStates.Invisible;

                                            var isPro = ListUtils.MyProfileList?.FirstOrDefault()?.IsPro ?? "0";
                                            switch (isPro)
                                            {
                                                case "0" when ListUtils.SettingsSiteList?.Pro == "1" && AppSettings.ShowGoPro:
                                                {
                                                    var dataOwner = ProUsersAdapter.MProUsersList.FirstOrDefault(a => a.Type == "Your");
                                                    switch (dataOwner)
                                                    {
                                                        case null:
                                                            ProUsersAdapter.MProUsersList.Insert(0, new UserDataObject
                                                            {
                                                                Avatar = UserDetails.Avatar,
                                                                Type = "Your",
                                                                Username = ActivityContext.GetText(Resource.String.Lbl_AddMe),
                                                            });

                                                            ProUsersAdapter.NotifyDataSetChanged();
                                                            break;
                                                    }

                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    var countList = item.UserList.Count;
                                    switch (item.UserList.Count)
                                    {
                                        case > 0 when countList > 0:
                                        {
                                            foreach (var user in from user in item.UserList let check = ProUsersAdapter.MProUsersList.FirstOrDefault(a => a.UserId == user.UserId) where check == null select user)
                                            {
                                                ProUsersAdapter.MProUsersList.Add(user);
                                            }

                                            ProUsersAdapter.NotifyItemRangeInserted(countList, ProUsersAdapter.MProUsersList.Count - countList);
                                            break;
                                        }
                                        case > 0:
                                            ProUsersAdapter.MProUsersList = new ObservableCollection<UserDataObject>(item.UserList);
                                            ProUsersAdapter.NotifyDataSetChanged();
                                            break;
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case Classes.ItemType.ProPage:
                        {
                            switch (viewHolder)
                            {
                                case TemplateRecyclerViewHolder holder:
                                {
                                    switch (ProPagesAdapter)
                                    {
                                        case null:
                                        {
                                            ProPagesAdapter = new ProPagesAdapter(ActivityContext)
                                            {
                                                MProPagesList = new ObservableCollection<PageClass>()
                                            };

                                            LinearLayoutManager layoutManager = new LinearLayoutManager(ActivityContext, LinearLayoutManager.Horizontal, false);
                                            holder.MRecycler.SetLayoutManager(layoutManager);
                                            holder.MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;

                                            var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                                            var preLoader = new RecyclerViewPreloader<PageClass>(ActivityContext, ProPagesAdapter, sizeProvider, 10);
                                            holder.MRecycler.AddOnScrollListener(preLoader);
                                            holder.MRecycler.SetAdapter(ProPagesAdapter);
                                            holder.MRecycler.AddOnItemTouchListener(new RecyclerViewOnItemTouch(holder.MRecycler, TabbedMainActivity.GetInstance()?.ViewPager));
                                            ProPagesAdapter.ItemClick += ProPagesAdapterOnItemClick;
                                         
                                            holder.TitleText.Text = ActivityContext.GetText(Resource.String.Lbl_Pro_Pages);
                                            holder.MoreText.Visibility = ViewStates.Invisible;
                                            break;
                                        }
                                    }

                                    var countList = item.PageList.Count;
                                    switch (item.PageList.Count)
                                    {
                                        case > 0 when countList > 0:
                                        {
                                            foreach (var page in from page in item.PageList let check = ProPagesAdapter.MProPagesList.FirstOrDefault(a => a.PageId == page.PageId) where check == null select page)
                                            {
                                                ProPagesAdapter.MProPagesList.Add(page);
                                            }

                                            ProPagesAdapter.NotifyItemRangeInserted(countList, ProPagesAdapter.MProPagesList.Count - countList);
                                            break;
                                        }
                                        case > 0:
                                            ProPagesAdapter.MProPagesList = new ObservableCollection<PageClass>(item.PageList);
                                            ProPagesAdapter.NotifyDataSetChanged();
                                            break;
                                    }

                                    break;
                                }
                            }


                            break;
                        }
                        case Classes.ItemType.LastActivities:
                        {
                            switch (viewHolder)
                            {
                                case ActivitiesAdapterViewHolder holder:
                                    InitializeLast(holder, item.LastActivities);
                                    break;
                            }

                            break;
                        }
                        case Classes.ItemType.Shortcuts:
                        {
                            switch (viewHolder)
                            {
                                case TemplateRecyclerViewHolder holder:
                                {
                                    switch (ShortcutsAdapter)
                                    {
                                        case null:
                                        {
                                            ShortcutsAdapter = new ShortcutsAdapter(ActivityContext)
                                            {
                                                ShortcutsList = new ObservableCollection<Classes.ShortCuts>()
                                            };

                                            LinearLayoutManager layoutManager = new LinearLayoutManager(ActivityContext, LinearLayoutManager.Horizontal, false);
                                            holder.MRecycler.SetLayoutManager(layoutManager);
                                            holder.MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;

                                            var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                                            var preLoader = new RecyclerViewPreloader<Classes.ShortCuts>(ActivityContext, ShortcutsAdapter, sizeProvider, 10);
                                            holder.MRecycler.AddOnScrollListener(preLoader);
                                            holder.MRecycler.SetAdapter(ShortcutsAdapter);
                                            holder.MRecycler.AddOnItemTouchListener(new RecyclerViewOnItemTouch(holder.MRecycler, TabbedMainActivity.GetInstance()?.ViewPager));
                                            ShortcutsAdapter.ItemClick += ShortcutsAdapterOnItemClick;
                                         
                                            holder.TitleText.Text = ActivityContext.GetText(Resource.String.Lbl_AllShortcuts);
                                            holder.MoreText.Visibility = ViewStates.Invisible;
                                            break;
                                        }
                                    }

                                    var countList = item.ShortcutsList.Count;
                                    switch (item.ShortcutsList.Count)
                                    {
                                        case > 0 when countList > 0:
                                        {
                                            foreach (var data in from data in item.ShortcutsList let check = ShortcutsAdapter.ShortcutsList.FirstOrDefault(a => a.Id == data.Id) where check == null select data)
                                            {
                                                ShortcutsAdapter.ShortcutsList.Add(data);
                                            }

                                            ShortcutsAdapter.NotifyItemRangeInserted(countList, ShortcutsAdapter.ShortcutsList.Count - countList);
                                            break;
                                        }
                                        case > 0:
                                            ShortcutsAdapter.ShortcutsList = new ObservableCollection<Classes.ShortCuts>(item.ShortcutsList);
                                            ShortcutsAdapter.NotifyDataSetChanged();
                                            break;
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case Classes.ItemType.LastBlogs:
                        {
                            switch (viewHolder)
                            {
                                case ArticlesAdapterViewHolder holder:
                                    var colorImage = Color.ParseColor(Methods.FunString.RandomColor());

                                    Glide.With(ActivityContext)
                                        .Load(item.LastBlogs.Thumbnail)
                                        .Apply(RequestOptions.CenterCropTransform().Placeholder(new ColorDrawable(colorImage)).Fallback(new ColorDrawable(colorImage)).SetPriority(Priority.High))
                                        .Into(holder.Image);

                                    Glide.With(ActivityContext)
                                        .Load(item.LastBlogs.Author.Avatar)
                                        .Apply(RequestOptions.CircleCropTransform())
                                        .Into(holder.UserImageProfile);

                                    holder.Category.Background.SetTint(colorImage);

                                    CategoriesController cat = new CategoriesController();
                                    string id = item.LastBlogs.CategoryLink.Split('/').Last();

                                    holder.Category.Text = cat.Get_Translate_Categories_Communities(id, item.LastBlogs.CategoryName, "Blog");

                                    holder.Title.Text = Methods.FunString.DecodeString(item.LastBlogs.Title);

                                    holder.Description.Text = Methods.FunString.DecodeString(item.LastBlogs.Description);

                                    holder.Username.Text = WoWonderTools.GetNameFinal(item.LastBlogs.Author);

                                    switch (item.LastBlogs.Author.Verified)
                                    {
                                        case "1":
                                            holder.Username.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.icon_checkmark_small_vector, 0);
                                            break;
                                    }
                                         
                                    holder.Time.Text = item.LastBlogs.Posted;

                                    break;
                            }

                            break;
                        }
                        case Classes.ItemType.HashTag:
                        {
                            switch (viewHolder)
                            {
                                case TrendingSearchAdapterViewHolder holder:
                                    holder.Text.Text = "#" + item.HashTags.Tag;
                                    holder.CountPosts.Text = item.HashTags.TrendUseNum + " " + ActivityContext.GetText(Resource.String.Lbl_Post);
                                    break;
                            }

                            break;
                        }
                        case Classes.ItemType.FriendRequest:
                        {
                            switch (viewHolder)
                            {
                                case FriendRequestViewHolder holder:
                                {
                                    holder.TxTFriendRequest.Text = ActivityContext.GetText(AppSettings.ConnectivitySystem == 1 ? Resource.String.Lbl_FollowRequest : Resource.String.Lbl_FriendRequest);
                                    holder.TxTFriendRequest.Text = ActivityContext.GetText(AppSettings.ConnectivitySystem == 1 ? Resource.String.Lbl_View_All_FollowRequest : Resource.String.Lbl_View_All_FriendRequest);

                                    switch (item.UserList.Count)
                                    {
                                        case > 0:
                                            holder.FriendRequestCount.Text = ListUtils.FriendRequestsList.Count.ToString();
                                            holder.FriendRequestCount.Visibility = ViewStates.Visible;
                                            break;
                                        default:
                                            holder.FriendRequestCount.Visibility = ViewStates.Gone;
                                            break;
                                    }

                                    for (var i = 0; i < 4; i++)
                                        switch (i)
                                        {
                                            case 0:
                                                switch (item.UserList.Count)
                                                {
                                                    case > 0:
                                                        GlideImageLoader.LoadImage(ActivityContext, item.UserList[i]?.Avatar, holder.FriendRequestImage3, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                                                        break;
                                                }
                                                break;
                                            case 1:
                                                switch (item.UserList.Count)
                                                {
                                                    case > 1:
                                                        GlideImageLoader.LoadImage(ActivityContext, item.UserList[i]?.Avatar, holder.FriendRequestImage2, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                                                        break;
                                                }
                                                break;
                                            case 2:
                                                switch (item.UserList.Count)
                                                {
                                                    case > 2:
                                                        GlideImageLoader.LoadImage(ActivityContext, item.UserList[i]?.Avatar, holder.FriendRequestImage1, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                                                        break;
                                                }
                                                break;
                                        }

                                    break;
                                }
                            }

                            break;
                        }
                        case Classes.ItemType.Weather:
                        {
                            switch (viewHolder)
                            {
                                case WeatherViewHolder holder:
                                {
                                    switch (item.Weather.Current.Condition.Text)
                                    {
                                        case "Cloudy":
                                            holder.Image.SetImageResource(Resource.Drawable.ic_weather_large_cloudy);
                                            break;
                                        case "Sunny":
                                            holder.Image.SetImageResource(Resource.Drawable.ic_weather_large_sunny);
                                            break;
                                        case "Partly cloudy":
                                            holder.Image.SetImageResource(Resource.Drawable.ic_weather_large_party_cloudy);
                                            break;
                                        case "Rain":
                                            holder.Image.SetImageResource(Resource.Drawable.ic_weather_large_rain);
                                            break;
                                        case "Snow":
                                            holder.Image.SetImageResource(Resource.Drawable.ic_weather_large_snow);
                                            break;
                                        default:
                                            item.Weather.Current.Condition.Icon =
                                            item.Weather.Current.Condition.Icon.Contains("http") switch
                                            {
                                                false => "http://" + item.Weather.Current.Condition.Icon,
                                                _ => item.Weather.Current.Condition.Icon
                                            };

                                            Glide.With(ActivityContext).Load(item.Weather.Current.Condition.Icon).Apply(new RequestOptions()).Into(holder.Image);
                                            break;
                                    }

                                    /*var current = Methods.Time.CurrentTimeMillis();
                                    string time = String.Format("%d:%d", Methods.Time.ConvertMillisecondsToHours(current),
                                        Methods.Time.ConvertMillisecondsToMinutes(current) - Methods.Time.ConvertMillisecondsToHours(current));*/

                                    holder.PlaceText.Text = string.IsNullOrEmpty(item.Weather.Location.Region) switch
                                    {
                                        false => item.Weather.Location.Region + ", " + item.Weather.Location.Country + "  ",
                                        _ => item.Weather.Location.Country
                                    };

                                    holder.HeadText.Text = item.Weather.Current.Condition.Text;
                                    holder.SubText.Text = item.Weather.Current.TempC + "°";

                                    holder.MphText.Text = item.Weather.Current.WindMph.ToString();
                                    holder.RainText.Text = item.Weather.Current.PrecipIn.ToString();
                                    holder.HumidityText.Text = item.Weather.Current.Humidity.ToString();

                                    List<HourObject> list = item.Weather.Forecast.ForecastDays.FirstOrDefault()?.Hour.Where(
                                        hourObject => hourObject.Time.Contains("03:00") ||
                                                      hourObject.Time.Contains("07:00") ||
                                                      hourObject.Time.Contains("11:00") ||
                                                      hourObject.Time.Contains("15:00") ||
                                                      hourObject.Time.Contains("19:00") ||
                                                      hourObject.Time.Contains("22:00")).ToList();
                                  
                                    holder.WeatherItems.RemoveAllViews();
                                    foreach (var ho in list)
                                    {
                                        View layout2 = LayoutInflater.From(ActivityContext)?.Inflate(Resource.Layout.Style_WeatherHourView, holder.WeatherItems, false);
                                        TextView temp = layout2.FindViewById<TextView>(Resource.Id.temp);
                                        ImageView icon = layout2.FindViewById<ImageView>(Resource.Id.Icon);
                                        TextView time = layout2.FindViewById<TextView>(Resource.Id.time);

                                        switch (ho.Condition.Text)
                                        {
                                            case "Cloudy":
                                                icon.SetImageResource(Resource.Drawable.ic_weather_small_cloudy);
                                                break;
                                            case "Sunny":
                                                icon.SetImageResource(Resource.Drawable.ic_weather_small_sunny);
                                                break;
                                            case "Partly cloudy":
                                                icon.SetImageResource(Resource.Drawable.ic_weather_small_party_cloudy);
                                                break;
                                            case "Rain":
                                                icon.SetImageResource(Resource.Drawable.ic_weather_small_rain);
                                                break;
                                            case "Snow":
                                                icon.SetImageResource(Resource.Drawable.ic_weather_small_snow);
                                                break;
                                            default:
                                                ho.Condition.Icon = ho.Condition.Icon.Contains("http") switch
                                                {
                                                    false => "http://" + ho.Condition.Icon,
                                                    _ => ho.Condition.Icon
                                                };
                                                Glide.With(ActivityContext).Load(ho.Condition.Icon).Apply(new RequestOptions()).Into(icon);
                                                break;
                                        }
                                        temp.Text = Methods.Time.TimeAgo(ho.TimeEpoch);
                                        time.Text = ho.TempC + "°";
                                        holder.WeatherItems.AddView(layout2);
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case Classes.ItemType.ExchangeCurrency:
                        {
                            switch (viewHolder)
                            {
                                case ExchangeCurrencyViewHolder holder:
                                {
                                    if (item.ExchangeCurrency.Timestamp != null)
                                    { 
                                        holder.CurrencyTime.Text = ActivityContext.GetText(Resource.String.Lbl_UpdatedAt) + " : " + Methods.Time.TimeAgo(item.ExchangeCurrency.Timestamp.Value); 
                                    }
                                  
                                    for (int i = 0; i < item.ExchangeCurrency.Rates?.Count; i++)
                                    {
                                        var (name, value) = item.ExchangeCurrency.Rates.ElementAt(i);
                                        if (name != null)
                                        {
                                            var buy = value.ToString("F");

                                            switch (i)
                                            {
                                                case 0:
                                                    holder.CurrencyText1.Text = name;
                                                    holder.CurrencyValue1.Text = buy + " " + AppSettings.ExCurrenciesIcons[0];
                                                    holder.CurrencyLayout1.Visibility = ViewStates.Visible; 
                                                    break;
                                                case 1:
                                                    holder.CurrencyText2.Text = name;
                                                    holder.CurrencyValue2.Text = buy + " " + AppSettings.ExCurrenciesIcons[1];
                                                    holder.CurrencyLayout2.Visibility = ViewStates.Visible;
                                                    break;
                                                case 2:
                                                    holder.CurrencyText3.Text = name;
                                                    holder.CurrencyValue3.Text = buy + " " + AppSettings.ExCurrenciesIcons[2];
                                                    holder.CurrencyLayout3.Visibility = ViewStates.Visible;
                                                    break;
                                            }
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case Classes.ItemType.Section:
                        {
                            switch (viewHolder)
                            {
                                case SectionViewHolder holder:
                                {
                                    holder.AboutHead.Text = item.Title; 

                                    switch (item.SectionType)
                                    {
                                        case Classes.ItemType.LastActivities:
                                        case Classes.ItemType.HashTag:
                                        case Classes.ItemType.LastBlogs:
                                            holder.AboutMore.Visibility = ViewStates.Visible;
                                            holder.AboutMore.Text = ActivityContext.GetText(Resource.String.Lbl_SeeAll);
                                            holder.AboutMore.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                            break;
                                        default:
                                            holder.AboutMore.Visibility = ViewStates.Gone;
                                            break;
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case Classes.ItemType.CoronaVirus:  
                        case Classes.ItemType.AdMob:  
                        case Classes.ItemType.EmptyPage: 
                        case Classes.ItemType.Divider:
                            break; 
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void InitializeLast(ActivitiesAdapterViewHolder holder, ActivityDataObject item)
        {
            try
            {
                GlideImageLoader.LoadImage(ActivityContext, item.Activator.Avatar, holder.ActivitiesImage, ImageStyle.RoundedCrop, ImagePlaceholders.Drawable);

                string replace = "";
                if (item.ActivityType.Contains("reaction"))
                {
                    if (item.ActivityType.Contains("Like"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_like);
                    }
                    else if (item.ActivityType.Contains("Love"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_love);
                    }
                    else if (item.ActivityType.Contains("HaHa"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_haha);
                    }
                    else if (item.ActivityType.Contains("Wow"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_wow);
                    }
                    else if (item.ActivityType.Contains("Sad"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_sad);
                    }
                    else if (item.ActivityType.Contains("Angry"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_angry);
                    }

                    if (UserDetails.LangName.Contains("fr"))
                    {
                        var split = item.ActivityText.Split("reacted to").Last().Replace("post", "");
                        replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_ReactedTo) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                    }
                    else
                        replace = item.ActivityText.Replace("reacted to", ActivityContext.GetString(Resource.String.Lbl_ReactedTo)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));

                }
                else switch (item.ActivityType)
                {
                    case "friend":
                    case "following":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_add);
                        //holder.Icon.SetColorFilter(Color.ParseColor("#333333"), PorterDuff.Mode.Multiply);

                        if (item.ActivityText.Contains("started following"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("started following").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_StartedFollowing) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("started following", ActivityContext.GetString(Resource.String.Lbl_StartedFollowing));
                        }
                        else if (item.ActivityText.Contains("become friends with"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("become friends with").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_BecomeFriendsWith) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("become friends with", ActivityContext.GetString(Resource.String.Lbl_BecomeFriendsWith));
                        }
                        else if (item.ActivityText.Contains("is following"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("is following").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_IsFollowing) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("is following", ActivityContext.GetString(Resource.String.Lbl_IsFollowing));
                        }

                        break;
                    }
                    case "liked_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_likes);

                        if (UserDetails.LangName.Contains("fr"))
                        {
                            var split = item.ActivityText.Split("liked").Last().Replace("post", "");
                            replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Btn_Liked) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                        }
                        else
                            replace = item.ActivityText.Replace("liked", ActivityContext.GetString(Resource.String.Btn_Liked)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));

                        break;
                    }
                    case "wondered_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_action_wowonder);
                        //holder.Icon.SetColorFilter(Color.ParseColor("#b71c1c"), PorterDuff.Mode.Multiply);

                        if (item.ActivityText.Contains("wondered"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("wondered").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_wondered) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("wondered", ActivityContext.GetString(Resource.String.Lbl_wondered)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));
                        }
                        else if (item.ActivityText.Contains("disliked"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("disliked").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_disliked) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("disliked", ActivityContext.GetString(Resource.String.Lbl_disliked)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));
                        }

                        break;
                    }
                    case "shared_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_action_share);
                        // holder.Icon.SetColorFilter(Color.ParseColor("#333333"), PorterDuff.Mode.Multiply);

                        if (UserDetails.LangName.Contains("fr"))
                        {
                            var split = item.ActivityText.Split("shared").Last().Replace("post", "");
                            replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_shared) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                        }
                        else
                            replace = item.ActivityText.Replace("shared", ActivityContext.GetString(Resource.String.Lbl_shared)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));

                        break;
                    }
                    case "commented_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_action_comments);
                        // holder.Icon.SetColorFilter(Color.ParseColor("#333333"), PorterDuff.Mode.Multiply);

                                if (UserDetails.LangName.Contains("fr"))
                        {
                            var split = item.ActivityText.Split("commented on").Last().Replace("post", "");
                            replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_CommentedOn) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                        }
                        else
                        {
                            replace = item.ActivityText.Replace("commented on", ActivityContext.GetString(Resource.String.Lbl_CommentedOn)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));
                        }

                        break;
                    }
                }

                holder.ActivitiesEvent.Text = !string.IsNullOrEmpty(replace) ? replace : item.ActivityText;

                holder.Time.Text = Methods.Time.TimeAgo(Convert.ToInt32(item.Time), false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Event
         
        private void ShortcutsAdapterOnItemClick(object sender, ShortcutsAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = ShortcutsAdapter.GetItem(position);
                        switch (item?.Type)
                        {
                            case "Page":
                            {
                                var intent = new Intent(ActivityContext, typeof(PageProfileActivity));
                                intent.PutExtra("PageObject", JsonConvert.SerializeObject(item.PageClass));
                                intent.PutExtra("PageId", item.PageClass.PageId);
                                ActivityContext.StartActivity(intent);
                                break;
                            }
                            case "Group":
                            {
                                var intent = new Intent(ActivityContext, typeof(GroupProfileActivity));
                                intent.PutExtra("GroupObject", JsonConvert.SerializeObject(item.GroupClass));
                                intent.PutExtra("GroupId", item.GroupClass.GroupId);
                                ActivityContext.StartActivity(intent);
                                break;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }
         
        private void ProPagesAdapterOnItemClick(object sender, ProPagesAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = ProPagesAdapter.GetItem(position);
                        if (item != null)
                        {
                            var intent = new Intent(ActivityContext, typeof(PageProfileActivity));
                            intent.PutExtra("PageObject", JsonConvert.SerializeObject(item));
                            intent.PutExtra("PageId", item.PageId);
                            ActivityContext.StartActivity(intent);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }
         
        private void ProUsersAdapterOnItemClick(object sender, ProUsersAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = ProUsersAdapter.GetItem(position);
                        if (item != null)
                        {
                            switch (item.Type)
                            {
                                case "Your":
                                {
                                    var intent = new Intent(ActivityContext, typeof(GoProActivity));
                                    ActivityContext.StartActivity(intent);
                                    break;
                                }
                                default:
                                    WoWonderTools.OpenProfile(ActivityContext, item.UserId, item);
                                    break;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }

        #endregion

        public override int ItemCount => TrendingList?.Count ?? 0;

        public Classes.TrendingClass GetItem(int position)
        {
            return TrendingList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                var item = TrendingList[position];
                if (item != null)
                {
                    return item.Type switch
                    {
                        Classes.ItemType.ProUser => (int)Classes.ItemType.ProUser, 
                        Classes.ItemType.ProPage => (int)Classes.ItemType.ProPage,
                        Classes.ItemType.HashTag => (int)Classes.ItemType.HashTag,
                        Classes.ItemType.FriendRequest => (int)Classes.ItemType.FriendRequest, 
                        Classes.ItemType.LastActivities => (int)Classes.ItemType.LastActivities, 
                        Classes.ItemType.Weather => (int)Classes.ItemType.Weather, 
                        Classes.ItemType.Shortcuts => (int)Classes.ItemType.Shortcuts, 
                        Classes.ItemType.AdMob => (int)Classes.ItemType.AdMob, 
                        Classes.ItemType.Section => (int)Classes.ItemType.Section, 
                        Classes.ItemType.EmptyPage => (int)Classes.ItemType.EmptyPage,
                        Classes.ItemType.Divider => (int)Classes.ItemType.Divider,
                        Classes.ItemType.LastBlogs => (int)Classes.ItemType.LastBlogs,
                        Classes.ItemType.ExchangeCurrency => (int)Classes.ItemType.ExchangeCurrency,
                        Classes.ItemType.CoronaVirus => (int)Classes.ItemType.CoronaVirus,
                        _ => position
                    };
                }

                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        void OnClick(TrendingAdapterClickEventArgs args) => ItemClick?.Invoke(ActivityContext, args);
        void OnLongClick(TrendingAdapterClickEventArgs args) => ItemLongClick?.Invoke(ActivityContext, args);
        void onUserClick(TrendingAdapterClickEventArgs args) => ItemUserClick?.Invoke(ActivityContext, args);
    }

    public class TemplateRecyclerViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }
        public LinearLayout MainLinear { get; private set; }
        public TextView TitleText { get; private set; }
        public TextView IconTitle { get; private set; }
        public TextView DescriptionText { get; private set; }
        public TextView MoreText { get; private set; }
        public RecyclerView MRecycler { get; private set; }

        #endregion

        public TemplateRecyclerViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                MainLinear = (LinearLayout)itemView.FindViewById(Resource.Id.mainLinear);
                TitleText = (TextView)itemView.FindViewById(Resource.Id.textTitle);
                IconTitle = (TextView)itemView.FindViewById(Resource.Id.iconTitle);
                DescriptionText = (TextView)itemView.FindViewById(Resource.Id.textSecondery);
                MoreText = (TextView)itemView.FindViewById(Resource.Id.textMore);
                MRecycler = (RecyclerView)itemView.FindViewById(Resource.Id.recyler);
              
                IconTitle.Visibility = ViewStates.Gone;
                DescriptionText.Visibility = ViewStates.Gone;
                 
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }
    
    public class TrendingAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        #region Variables Basic

        public View MainView { get; private set; }
        private readonly TrendingAdapter TrendingAdapter;

        public TextView TitleText { get; private set; }

        public LinearLayout UserItem { get; private set; }
        public ImageView UserImageProfile { get; private set; }
        public TextView Username { get; private set; }
        public TextView Time { get; private set; }

        public TextView Title { get; private set; }
        public ImageView Image { get; private set; } 


        public TextView MoreText { get; private set; }

        #endregion

        public TrendingAdapterViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener, TrendingAdapter trendingAdapter) : base(itemView)
        {
            try
            {
                MainView = itemView;
                TrendingAdapter = trendingAdapter;
                 
                TitleText = (TextView)itemView.FindViewById(Resource.Id.textTitle);
                 
                UserItem  = (LinearLayout)itemView.FindViewById(Resource.Id.UserItem_Layout); 
                UserImageProfile = (ImageView)itemView.FindViewById(Resource.Id.UserImageProfile);
                Username = (TextView)itemView.FindViewById(Resource.Id.Username);
                Time = (TextView)itemView.FindViewById(Resource.Id.time);

                Title = (TextView)itemView.FindViewById(Resource.Id.Title);
                Image = (ImageView)itemView.FindViewById(Resource.Id.Image); 
 
                MoreText = (TextView)itemView.FindViewById(Resource.Id.View_more);

                UserItem.SetOnClickListener(this);
                MoreText.SetOnClickListener(this);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnClick(View v)
        {
            try
            {
                if (AdapterPosition != RecyclerView.NoPosition)
                {
                    var item = TrendingAdapter.TrendingList[AdapterPosition]?.LastBlogs;

                    if (v.Id == UserItem.Id)
                        WoWonderTools.OpenProfile(TrendingAdapter.ActivityContext, item.Author.UserId, item.Author);
                    else if (v.Id == MoreText.Id)
                        MoreTextOnClick();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void MoreTextOnClick()
        {
            try
            {
                var intent = new Intent(TrendingAdapter.ActivityContext, typeof(ArticlesActivity));
                TrendingAdapter.ActivityContext.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }
    }
    
    public class TrendingSearchAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }
        public TextView Text { get; private set; }
        public TextView CountPosts { get; private set; } 

        #endregion

        public TrendingSearchAdapterViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Text = MainView.FindViewById<TextView>(Resource.Id.text);
                CountPosts = MainView.FindViewById<TextView>(Resource.Id.countPosts);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }
     
    public class ExchangeCurrencyViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }
        public RelativeLayout CurrencyLayout1 { get; private set; }
        public TextView CurrencyText1 { get; private set; }
        public TextView CurrencyValue1 { get; private set; }
        public RelativeLayout CurrencyLayout2 { get; private set; }
        public TextView CurrencyText2 { get; private set; }
        public TextView CurrencyValue2 { get; private set; }
        public RelativeLayout CurrencyLayout3 { get; private set; }
        public TextView CurrencyText3 { get; private set; }
        public TextView CurrencyValue3 { get; private set; } 
        public TextView CurrencyTime { get; private set; } 

        #endregion

        public ExchangeCurrencyViewHolder(View itemView) : base(itemView)
        {
            try
            {
                MainView = itemView;

                CurrencyLayout1 = MainView.FindViewById<RelativeLayout>(Resource.Id.CurrencyLayout1);
                CurrencyText1 = MainView.FindViewById<TextView>(Resource.Id.CurrencyText1);
                CurrencyValue1 = MainView.FindViewById<TextView>(Resource.Id.CurrencyValue1);
                CurrencyLayout2 = MainView.FindViewById<RelativeLayout>(Resource.Id.CurrencyLayout2);
                CurrencyText2 = MainView.FindViewById<TextView>(Resource.Id.CurrencyText2);
                CurrencyValue2 = MainView.FindViewById<TextView>(Resource.Id.CurrencyValue2);
                CurrencyLayout3 = MainView.FindViewById<RelativeLayout>(Resource.Id.CurrencyLayout3);
                CurrencyText3 = MainView.FindViewById<TextView>(Resource.Id.CurrencyText3);
                CurrencyValue3 = MainView.FindViewById<TextView>(Resource.Id.CurrencyValue3);
                CurrencyTime = MainView.FindViewById<TextView>(Resource.Id.CurrencyTime);

                CurrencyLayout1.Visibility = ViewStates.Gone;
                CurrencyLayout2.Visibility = ViewStates.Gone;
                CurrencyLayout3.Visibility = ViewStates.Gone;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }


    public class AlertCoronaVirusAdapterViewHolder : RecyclerView.ViewHolder
    {
        public View MainView { get; }

        public AlertCoronaVirusAdapterViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                Console.WriteLine(longClickListener);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        } 
    }


    public class FriendRequestViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }

        public RelativeLayout LayoutFriendRequest { get; private set; }
        public ImageView FriendRequestImage1 { get; private set; } 
        public ImageView FriendRequestImage2 { get; private set; }
        public ImageView FriendRequestImage3 { get; private set; }
        public TextView FriendRequestCount { get; private set; } 
        public TextView TxTFriendRequest { get; private set; }
        public TextView TxtAllFriendRequest { get; private set; }

        #endregion

        public FriendRequestViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                LayoutFriendRequest = (RelativeLayout)itemView.FindViewById(Resource.Id.layout_friend_Request);

                FriendRequestImage1 = (ImageView)itemView.FindViewById(Resource.Id.image_page_1);
                FriendRequestImage2 = (ImageView)itemView.FindViewById(Resource.Id.image_page_2);
                FriendRequestImage3 = (ImageView)itemView.FindViewById(Resource.Id.image_page_3);
                FriendRequestCount = (TextView)itemView.FindViewById(Resource.Id.count_view);

                TxTFriendRequest = (TextView)itemView.FindViewById(Resource.Id.tv_Friends_connection);
                TxtAllFriendRequest = (TextView)itemView.FindViewById(Resource.Id.tv_Friends);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }
    
    public class ActivitiesAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }

        public ImageView ActivitiesImage { get; private set; }
        public TextView ActivitiesEvent { get; private set; }
        public ImageView Icon { get; private set; }
        public TextView Time { get; private set; }

        #endregion

        public ActivitiesAdapterViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                ActivitiesImage = (ImageView)MainView.FindViewById(Resource.Id.Image);
                ActivitiesEvent = MainView.FindViewById<TextView>(Resource.Id.LastActivitiesText);
                Icon = MainView.FindViewById<ImageView>(Resource.Id.ImageIcon);
                Time = MainView.FindViewById<TextView>(Resource.Id.Time);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }

    public class SectionViewHolder : RecyclerView.ViewHolder
    {
        public View MainView { get; private set; }
        public TextView AboutHead { get; private set; }
        public TextView AboutMore { get; private set; }

        public SectionViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class WeatherViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }
         
        public ImageView Image { get; private set; }  
        public TextView HeadText { get; private set; }
        public TextView SubText { get; private set; }
        public TextView PlaceText { get; private set; }
        public RecyclerView MRecycler { get; private set; }
        public TextView MphText { get; private set; }
        public TextView RainText { get; private set; }
        public TextView HumidityText { get; private set; }
        public LinearLayout WeatherItems { get; private set; }
        #endregion 

        public WeatherViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;
                  
                Image = (ImageView)itemView.FindViewById(Resource.Id.Image);

                HeadText = (TextView)itemView.FindViewById(Resource.Id.HeadText);
                SubText = (TextView)itemView.FindViewById(Resource.Id.subText);
                PlaceText = (TextView)itemView.FindViewById(Resource.Id.PlaceText);
                MRecycler = (RecyclerView)itemView.FindViewById(Resource.Id.Recyler);
                MphText = itemView.FindViewById<TextView>(Resource.Id.tv_mph);
                RainText = itemView.FindViewById<TextView>(Resource.Id.tv_rain_percent);
                HumidityText = itemView.FindViewById<TextView>(Resource.Id.tv_humidity);
                WeatherItems = itemView.FindViewById<LinearLayout>(Resource.Id.LLWeatherItem);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }

    public class EmptyStateViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }
        public Button EmptyStateButton { get; private set; }
        public TextView EmptyStateIcon { get; private set; }
        public TextView DescriptionText { get; private set; }
        public TextView TitleText { get; private set; }

        #endregion

        public EmptyStateViewHolder(View itemView) : base(itemView)
        {
            try
            {
                MainView = itemView;

                EmptyStateIcon = (TextView)itemView.FindViewById(Resource.Id.emtyicon);
                TitleText = (TextView)itemView.FindViewById(Resource.Id.headText);
                DescriptionText = (TextView)itemView.FindViewById(Resource.Id.seconderyText);
                EmptyStateButton = (Button)itemView.FindViewById(Resource.Id.button);
                 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Analytics);
                EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoTrending_TitleText);
                DescriptionText.Text = " ";
                EmptyStateButton.Visibility = ViewStates.Gone;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }

    public class ArticlesAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ArticlesAdapterViewHolder(View itemView, Action<TrendingAdapterClickEventArgs> userClickListener, Action<TrendingAdapterClickEventArgs> clickListener, Action<TrendingAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                UserItem = MainView.FindViewById<RelativeLayout>(Resource.Id.UserItem_Layout);

                Image = MainView.FindViewById<RoundedImageView>(Resource.Id.Image);
                Category = MainView.FindViewById<TextView>(Resource.Id.Category);
                Title = MainView.FindViewById<TextView>(Resource.Id.Title);
                Description = MainView.FindViewById<TextView>(Resource.Id.description);
                UserImageProfile = MainView.FindViewById<ImageView>(Resource.Id.UserImageProfile);
                Username = MainView.FindViewById<TextView>(Resource.Id.Username);
                Time = MainView.FindViewById<TextView>(Resource.Id.card_dist);
                ViewMore = MainView.FindViewById<TextView>(Resource.Id.View_more);
                 
                //Event
                UserItem.Click += (sender, e) => userClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.Click += (sender, e) => clickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new TrendingAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #region Variables Basic

        private View MainView { get; }

        public RoundedImageView Image { get; private set; }
        public TextView Title { get; private set; }
        public TextView Description { get; private set; }
        public ImageView UserImageProfile { get; private set; }
        public TextView Category { get; private set; }
        public TextView Username { get; private set; }
        public TextView Time { get; private set; }
        public TextView ViewMore { get; private set; }
        public RelativeLayout UserItem { get; private set; } 

        #endregion
    }

    public class TrendingAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}