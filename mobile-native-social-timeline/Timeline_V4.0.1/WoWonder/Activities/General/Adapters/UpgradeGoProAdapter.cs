using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using Console = System.Console;

namespace WoWonder.Activities.General.Adapters
{
    public class UpgradeGoProClass
    {
        public string Id { get; set; }
        public string HexColor { get; set; }
        public string PlanText { get; set; }

        public string PlanTime { get; set; }

        public string PlanPrice { get; set; }

        public int ImageResource { get; set; }
        public Dictionary<string , string> PlanArray { get; set; } 
    }

    public class UpgradeGoProAdapter : RecyclerView.Adapter
    {
        #region Variables Basic

        public event EventHandler<UpgradeGoProAdapterClickEventArgs> UpgradeButtonItemClick;
        public event EventHandler<UpgradeGoProAdapterClickEventArgs> ItemClick;
        public event EventHandler<UpgradeGoProAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        private WoTextDecorator WoTextDecorator { get; set; } 
        public ObservableCollection<UpgradeGoProClass> PlansList = new ObservableCollection<UpgradeGoProClass>();

        #endregion

        public UpgradeGoProAdapter(Activity context)
        {
            try
            {
                ActivityContext = context;
                WoTextDecorator = new WoTextDecorator();
                 
                //PlansList.Add(new UpgradeGoProClass { Id = 1, HexColor = "#4c7737", PlanText = Resource.String.go_pro_plan_1,PlanTime="",PlanPrice="" ImageResource = Resource.Drawable.gopro_medal });
                //<item>STAR</item>
		        //<item>$3</item>
		        //<item>Per Week</item>
		        //<item>#4c7737</item>
                 
                switch (ListUtils.SettingsSiteList?.ProPackagesTypes?.Count)
                {
                    case > 0:
                    {
                        foreach (var type in ListUtils.SettingsSiteList?.ProPackagesTypes)//"1": "star"
                        {
                            var resourceId = ActivityContext.Resources?.GetIdentifier("ic_plan_" + type.Key, "drawable", ActivityContext.PackageName) ?? 0;
                            switch (resourceId)
                            {
                                case 0:
                                    continue;
                                default:
                                {
                                    var proClass = new UpgradeGoProClass
                                    {
                                        Id = type.Key,
                                        HexColor = GetColor(type.Key),
                                        PlanText = type.Value, 
                                        PlanPrice = GetPrice(type.Value),
                                        PlanTime = GetTime(type.Value),
                                        PlanArray = GetPlanArray(type.Value),
                                        ImageResource = resourceId,
                                    };

                                    PlansList.Add(proClass);
                                    break;
                                }
                            }
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

        private string GetColor(string id)
        {
            try
            {
                return id switch
                {
                    "1" => "#4c7737",
                    "2" => "#f9b340",
                    "3" => "#e13c4c",
                    "4" => "#3f4bb8",
                    _ => AppSettings.MainColor
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return AppSettings.MainColor;
            }
        }
        
        private string GetTime(string value)
        {
            try
            {
                string time = ListUtils.SettingsSiteList?.ProPackages?.FirstOrDefault(a => a.Key == value).Value?.Time?.ToString();
                switch (string.IsNullOrEmpty(time))
                {
                    case false:
                        return time switch
                        {
                            "7" => ActivityContext.GetText(Resource.String.Lbl_per_week),
                            "30" => ActivityContext.GetText(Resource.String.Lbl_per_month),
                            "365" => ActivityContext.GetText(Resource.String.Lbl_per_year),
                            "0" => ActivityContext.GetText(Resource.String.Lbl_LifeTime),
                            _ => time
                        };
                    default:
                        time = value switch
                        {
                            "star" => ActivityContext.GetText(Resource.String.Lbl_per_week),
                            "hot" => ActivityContext.GetText(Resource.String.Lbl_per_month),
                            "ultima" => ActivityContext.GetText(Resource.String.Lbl_per_year),
                            "vip" => ActivityContext.GetText(Resource.String.Lbl_LifeTime),
                            _ => ""
                        };
                        break;
                }

                return time; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return "";
            }
        }
        
        private string GetPrice(string value)
        {
            try
            {
                string price = ListUtils.SettingsSiteList?.ProPackages?.FirstOrDefault(a => a.Key == value).Value?.Price;
                switch (string.IsNullOrEmpty(price))
                {
                    case false:
                        return price;
                    default:
                        price = value switch
                        {
                            "star" => AppSettings.WeeklyPrice,
                            "hot" => AppSettings.MonthlyPrice,
                            "ultima" => AppSettings.YearlyPrice,
                            "vip" => AppSettings.LifetimePrice,
                            _ => ""
                        };
                        break;
                }
                
                return price;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return "";
            }
        }
         
        private Dictionary<string, string> GetPlanArray(string value)
        {
            try
            {
                /* 
                <item> \uf122 Featured member</item>
		        <item> \uf122 See profile visitors</item>
		        <item> \uf122 Show / Hide last seen</item>
		        <item> \uf122 Verified badge</item>
		        <item> \uf12a Posts promotion</item>
		        <item> \uf12a Pages promotion</item>
		        <item> \uf12a Discount</item> 
                 
                WoTextDecorator.SetTextColor(IonIconsFonts.Checkmark, "#43a735"); 
                WoTextDecorator.SetTextColor(IonIconsFonts.Close, "#e13c4c");

                */

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                DataProPackages packages = ListUtils.SettingsSiteList?.ProPackages?.FirstOrDefault(a => a.Key == value).Value;
                if (packages != null)
                { 
                    dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_go_pro_1), packages.FeaturedMember?.ToString() == "1" ? IonIconsFonts.Checkmark : IonIconsFonts.Close);
                    dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_go_pro_2), packages.ProfileVisitors?.ToString() == "1" ? IonIconsFonts.Checkmark : IonIconsFonts.Close);
                    dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_go_pro_4), packages.LastSeen?.ToString() == "1" ? IonIconsFonts.Checkmark : IonIconsFonts.Close);
                    dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_go_pro_5), packages.VerifiedBadge?.ToString() == "1" ? IonIconsFonts.Checkmark : IonIconsFonts.Close);

                    switch (packages.PostsPromotion?.ToString())
                    {
                        case "0":
                            dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_go_pro_6), IonIconsFonts.Close);
                            break;
                        default:
                            dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_BoostUpTo) + " " + packages.PostsPromotion?.ToString() + " " + ActivityContext.GetText(Resource.String.Lbl_Posts), IonIconsFonts.Checkmark);
                            break;
                    }
                  
                    switch (packages.PagesPromotion?.ToString())
                    {
                        case "0":
                            dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_go_pro_3), IonIconsFonts.Close);
                            break;
                        default:
                            dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_BoostUpTo) + " " + packages.PostsPromotion?.ToString() + " " + ActivityContext.GetText(Resource.String.Lbl_Pages), IonIconsFonts.Checkmark);
                            break;
                    }
                   
                    switch (packages.Discount?.ToString())
                    {
                        case "0":
                            dictionary.Add(ActivityContext.GetText(Resource.String.Lbl_Discount), IonIconsFonts.Close);
                            break;
                        default:
                            dictionary.Add(packages.Discount?.ToString() + "% " + ActivityContext.GetText(Resource.String.Lbl_Discount), IonIconsFonts.Checkmark);
                            break;
                    } 
                }
                  
                return dictionary;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new Dictionary<string, string>();
            }
        }
         
        public override int ItemCount => PlansList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_GoPro_Pricess_View
                View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_GoPro_Pricess_View, parent, false);
                UpgradePlansViewHolder vh = new UpgradePlansViewHolder(itemView, UpgradeButtonClick,Click, LongClick);
                return vh;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder)
                {
                    case UpgradePlansViewHolder holder:
                    {
                        UpgradeGoProClass item = PlansList[position];
                        if (item != null)
                        {
                            switch (AppSettings.SetTabDarkTheme)
                            {
                                case true:
                                    holder.MainLayout.SetBackgroundResource(Resource.Drawable.ShadowLinerLayoutDark);
                                    holder.RelativeLayout.SetBackgroundResource(Resource.Drawable.price_gopro_item_style_dark);
                                    break;
                            }

                            holder.PlanImg.SetImageResource(item.ImageResource);
                            holder.PlanImg.SetColorFilter(Color.ParseColor(item.HexColor));

                            var (currency, currencyIcon) = WoWonderTools.GetCurrency(ListUtils.SettingsSiteList?.Currency);
                            Console.WriteLine(currency);
                            if (ListUtils.SettingsSiteList != null)
                                holder.PriceText.Text = item.PlanPrice + currencyIcon;
                            else
                                holder.PriceText.Text = item.PlanPrice;
                         
                            holder.PlanText.Text = item.PlanText;
                            holder.PerText.Text = item.PlanTime;

                            holder.PlanText.SetTextColor(Color.ParseColor(item.HexColor));
                            holder.PriceText.SetTextColor(Color.ParseColor(item.HexColor));
                            holder.UpgradeButton.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(item.HexColor));
                         
                            switch (item.PlanArray?.Count)
                            {
                                case > 0:
                                {
                                    Typeface font = Typeface.CreateFromAsset(ActivityContext.Resources?.Assets, "ionicons.ttf");

                                    foreach (var options in item.PlanArray)
                                    {
                                        var content = options.Value + " " + options.Key.ToUpper();

                                        AppCompatTextView text = new AppCompatTextView(ActivityContext)
                                        {
                                            Text = content,
                                            TextSize = 13
                                        };
                                 
                                        text.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#444444"));
                                        text.Gravity = GravityFlags.CenterHorizontal;
                                        text.SetTypeface(font, TypefaceStyle.Normal);
                                        WoTextDecorator.Content = content;
                                        WoTextDecorator.DecoratedContent = new Android.Text.SpannableString(content);
                                        switch (options.Value)
                                        {
                                            case IonIconsFonts.Checkmark:
                                                WoTextDecorator.SetTextColor(IonIconsFonts.Checkmark, "#43a735");
                                                break;
                                            case IonIconsFonts.Close:
                                                WoTextDecorator.SetTextColor(IonIconsFonts.Close, "#e13c4c");
                                                break;
                                        }
                                 
                                        LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);//height and width are in pixel
                                        layoutParams.SetMargins(0, 30, 0, 5);

                                        text.LayoutParameters = layoutParams;
                                        holder.OptionLinerLayout.AddView(text);
                                        WoTextDecorator.Build(text, WoTextDecorator.DecoratedContent);
                                    }

                                    break;
                                }
                            } 
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

        public UpgradeGoProClass GetItem(int position)
        {
            return PlansList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        }

        private void UpgradeButtonClick(UpgradeGoProAdapterClickEventArgs args)
        {
            UpgradeButtonItemClick?.Invoke(this, args);
        }

        private void Click(UpgradeGoProAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(UpgradeGoProAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

    }

    public class UpgradePlansViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public LinearLayout MainLayout { get; private set; }
        public ImageView PlanImg { get; private set; }
        public TextView PriceText { get; private set; } 
        public TextView PerText { get; private set; }
        public Button UpgradeButton { get; private set; } 
        public TextView PlanText { get; private set; }
        public LinearLayout OptionLinerLayout { get; private set; } 
        public View MainView { get; private set; }
        public RelativeLayout RelativeLayout { get; private set; }

        #endregion

        public UpgradePlansViewHolder(View itemView ,Action<UpgradeGoProAdapterClickEventArgs> upgradeButtonClickListener, Action<UpgradeGoProAdapterClickEventArgs> clickListener, Action<UpgradeGoProAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                MainLayout = MainView.FindViewById<LinearLayout>(Resource.Id.mainLayout);
                PlanImg = MainView.FindViewById<ImageView>(Resource.Id.iv1);
                PriceText = MainView.FindViewById<TextView>(Resource.Id.priceTextView);
                PerText = MainView.FindViewById<TextView>(Resource.Id.PerText);
                PlanText = MainView.FindViewById<TextView>(Resource.Id.PlanText);
                OptionLinerLayout = MainView.FindViewById<LinearLayout>(Resource.Id.OptionLinerLayout);
                UpgradeButton = MainView.FindViewById<Button>(Resource.Id.UpgradeButton);
                RelativeLayout = MainView.FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);

                UpgradeButton.Click += (sender, e) => upgradeButtonClickListener(new UpgradeGoProAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.Click += (sender, e) => clickListener(new UpgradeGoProAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new UpgradeGoProAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class UpgradeGoProAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}