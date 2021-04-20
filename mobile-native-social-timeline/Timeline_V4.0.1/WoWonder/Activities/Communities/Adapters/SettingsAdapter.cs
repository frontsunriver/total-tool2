using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Activities.Tabbes.Adapters;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;

namespace WoWonder.Activities.Communities.Adapters
{ 
    public class SettingsAdapter : RecyclerView.Adapter
    {
        private readonly Activity ActivityContext;
        public readonly ObservableCollection<SectionItem> SectionList = new ObservableCollection<SectionItem>();

        public SettingsAdapter(Activity context , string type , PageClass pageClass)
        {
            try
            {
                ActivityContext = context;
                switch (type)
                {
                    case "Page":
                        SetItemPage(pageClass);
                        break;
                    case "Group":
                        SetItemGroup();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => SectionList?.Count ?? 0;

        public event EventHandler<SettingsAdapterClickEventArgs> ItemClick;
        public event EventHandler<SettingsAdapterClickEventArgs> ItemLongClick;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> ChannelSubscribed_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_MoreSection_view, parent, false);
                var vh = new SettingsAdapterViewHolder(itemView, OnClick, OnLongClick);
                return vh;
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
                switch (viewHolder)
                {
                    case SettingsAdapterViewHolder holder:
                    {
                        var item = SectionList[position];
                        if (item != null)
                        {
                            switch (AppSettings.FlowDirectionRightToLeft)
                            {
                                case true:
                                    holder.LinearLayoutImage.LayoutDirection = LayoutDirection.Rtl;
                                    holder.LinearLayoutMain.LayoutDirection = LayoutDirection.Rtl;
                                    holder.Name.LayoutDirection = LayoutDirection.Rtl;
                                    break;
                            }

                            FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, holder.Icon, item.Icon);
                            holder.Icon.SetTextColor(item.IconColor);
                            holder.Name.Text = item.SectionName;
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
         
        public SectionItem GetItem(int position)
        {
            return SectionList[position];
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
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        private void OnClick(SettingsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void OnLongClick(SettingsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

        private void SetItemPage(PageClass pageData)
        {
            try
            {
                //Access to general settings >> general
                //Access to page information settings >> info
                //Access to social links settings >> social
                //Access to design settings >> design
                //Access to admins settings >> admins
                //Access to analytics settings >> analytics
                //Access to delete page settings >> delete_page
                 
                if (pageData?.AdminInfo?.AdminInfoClass != null && pageData.AdminInfo?.AdminInfoClass.General == "1" || pageData?.IsPageOnwer != null && pageData.IsPageOnwer.Value)
                {
                    SectionList.Add(new SectionItem
                    {
                        Id = 1,
                        SectionName = ActivityContext.GetText(Resource.String.Lbl_General),
                        BadgeCount = 0,
                        Badgevisibilty = false,
                        Icon = IonIconsFonts.Settings,
                        IconColor = Color.ParseColor(AppSettings.MainColor)
                    });
                }

                if (pageData?.AdminInfo?.AdminInfoClass != null &&  pageData.AdminInfo?.AdminInfoClass.Info == "1" || pageData?.IsPageOnwer != null &&  pageData.IsPageOnwer.Value) 
                {
                    SectionList.Add(new SectionItem
                    {
                        Id = 2,
                        SectionName = ActivityContext.GetText(Resource.String.Lbl_PageInformation),
                        BadgeCount = 0,
                        Badgevisibilty = false,
                        Icon = IonIconsFonts.InformationCircleOutline,
                        IconColor = Color.ParseColor(AppSettings.MainColor)
                    });
                }

                if (pageData?.AdminInfo?.AdminInfoClass != null && pageData.AdminInfo?.AdminInfoClass.General == "1" || pageData?.IsPageOnwer != null && pageData.IsPageOnwer.Value)
                {
                    SectionList.Add(new SectionItem
                    {
                        Id = 3,
                        SectionName = ActivityContext.GetText(Resource.String.Lbl_ActionButtons),
                        BadgeCount = 0,
                        Badgevisibilty = false,
                        Icon = IonIconsFonts.Bonfire,
                        IconColor = Color.ParseColor(AppSettings.MainColor)
                    });
                }

                if (pageData?.AdminInfo?.AdminInfoClass != null && pageData.AdminInfo?.AdminInfoClass.Social == "1" || pageData?.IsPageOnwer != null && pageData.IsPageOnwer.Value)
                {
                    SectionList.Add(new SectionItem
                    {
                        Id = 4,
                        SectionName = ActivityContext.GetText(Resource.String.Lbl_SocialLinks),
                        BadgeCount = 0,
                        Badgevisibilty = false,
                        Icon = IonIconsFonts.Heart,
                        IconColor = Color.ParseColor(AppSettings.MainColor)
                    });
                }

                switch (AppSettings.ShowJobs)
                {
                    case true:
                        SectionList.Add(new SectionItem
                        {
                            Id = 5,
                            SectionName = ActivityContext.GetText(Resource.String.Lbl_OfferAJob),
                            BadgeCount = 0,
                            Badgevisibilty = false,
                            Icon = IonIconsFonts.Briefcase,
                            IconColor = Color.ParseColor(AppSettings.MainColor)
                        });
                        break;
                }

                switch (AppSettings.ShowOffers)
                {
                    case true:
                        SectionList.Add(new SectionItem
                        {
                            Id = 6,
                            SectionName = ActivityContext.GetText(Resource.String.Lbl_Offer),
                            BadgeCount = 0,
                            Badgevisibilty = false,
                            Icon = IonIconsFonts.IosAttach,
                            IconColor = Color.ParseColor(AppSettings.MainColor)
                        });
                        break;
                }

                if (pageData?.AdminInfo?.AdminInfoClass != null && pageData.AdminInfo?.AdminInfoClass.Admins == "1" || pageData?.IsPageOnwer != null && pageData.IsPageOnwer.Value)
                {
                    SectionList.Add(new SectionItem
                    {
                        Id = 7,
                        SectionName = ActivityContext.GetText(Resource.String.Lbl_Admin),
                        BadgeCount = 0,
                        Badgevisibilty = false,
                        Icon = IonIconsFonts.Person,
                        IconColor = Color.ParseColor(AppSettings.MainColor)
                    });
                }

                if (pageData?.AdminInfo?.AdminInfoClass != null && pageData.AdminInfo?.AdminInfoClass.DeletePage == "1" || pageData?.IsPageOnwer != null && pageData.IsPageOnwer.Value)
                {
                    SectionList.Add(new SectionItem
                    {
                        Id = 8,
                        SectionName = ActivityContext.GetText(Resource.String.Lbl_DeletePage),
                        BadgeCount = 0,
                        Badgevisibilty = false,
                        Icon = IonIconsFonts.Trash,
                        IconColor = Color.ParseColor(AppSettings.MainColor)
                    });
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void SetItemGroup()
        {
            try
            {
                SectionList.Add(new SectionItem
                {
                    Id = 1,
                    SectionName = ActivityContext.GetText(Resource.String.Lbl_General),
                    BadgeCount = 0,
                    Badgevisibilty = false,
                    Icon = IonIconsFonts.Settings,
                    IconColor = Color.ParseColor(AppSettings.MainColor)
                });
                SectionList.Add(new SectionItem
                {
                    Id = 2,
                    SectionName = ActivityContext.GetText(Resource.String.Lbl_Privacy),
                    BadgeCount = 0,
                    Badgevisibilty = false,
                    Icon = IonIconsFonts.Lock,
                    IconColor = Color.ParseColor(AppSettings.MainColor)
                });
                SectionList.Add(new SectionItem
                {
                    Id = 3,
                    SectionName = ActivityContext.GetText(Resource.String.Lbl_Members),
                    BadgeCount = 0,
                    Badgevisibilty = false,
                    Icon = IonIconsFonts.Contacts,
                    IconColor = Color.ParseColor(AppSettings.MainColor)
                });
                SectionList.Add(new SectionItem
                {
                    Id = 4,
                    SectionName = ActivityContext.GetText(Resource.String.Lbl_DeleteGroup),
                    BadgeCount = 0,
                    Badgevisibilty = false,
                    Icon = IonIconsFonts.Trash,
                    IconColor = Color.ParseColor(AppSettings.MainColor)
                }); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class SettingsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public SettingsAdapterViewHolder(View itemView, Action<SettingsAdapterClickEventArgs> clickListener,Action<SettingsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                LinearLayoutMain = MainView.FindViewById<LinearLayout>(Resource.Id.main);
                LinearLayoutImage = MainView.FindViewById<RelativeLayout>(Resource.Id.imagecontainer);

                Icon = MainView.FindViewById<TextView>(Resource.Id.Icon);
                Name = MainView.FindViewById<TextView>(Resource.Id.section_name);
                Badge = MainView.FindViewById<ImageView>(Resource.Id.badge);
                Badge.Visibility = ViewStates.Gone;

                itemView.Click += (sender, e) => clickListener(new SettingsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new SettingsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public View MainView { get; }

        public LinearLayout LinearLayoutMain { get; private set; }
        public RelativeLayout LinearLayoutImage { get; private set; }
        public TextView Icon { get; private set; }
        public TextView Name { get; private set; }
        public ImageView Badge { get; private set; }


    }

    public class SettingsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}