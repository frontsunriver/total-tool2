using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.SettingsPreferences.Adapters
{
    public class MyInformationAdapter : RecyclerView.Adapter
    {
        public event EventHandler<MyInformationAdapterClickEventArgs> ItemClick;
        public event EventHandler<MyInformationAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<Classes.MyInformation> MyInfoList = new ObservableCollection<Classes.MyInformation>();

        public MyInformationAdapter(Activity context)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = context;

                MyInfoList.Add(new Classes.MyInformation
                {
                    Id = 1,
                    Name = ActivityContext.GetText(Resource.String.Lbl_MyInformation),
                    Color = Color.ParseColor("#f44336"),
                    Icon = IonIconsFonts.Alert,
                    Type = "my_information",
                });

                MyInfoList.Add(new Classes.MyInformation
                {
                    Id = 2,
                    Name = ActivityContext.GetText(Resource.String.Lbl_Post),
                    Color = Color.ParseColor("#673AB7"),
                    Icon = IonIconsFonts.IosPaper,
                    Type = "posts",
                });

                MyInfoList.Add(new Classes.MyInformation
                {
                    Id = 3,
                    Name = ActivityContext.GetText(Resource.String.Lbl_Pages),
                    Color = Color.ParseColor("#2196F3"),
                    Icon = IonIconsFonts.Flag,
                    Type = "pages",
                });
          
                MyInfoList.Add(new Classes.MyInformation
                {
                    Id = 4,
                    Name = ActivityContext.GetText(Resource.String.Lbl_Groups),
                    Color = Color.ParseColor("#00BCD4"),
                    Icon = IonIconsFonts.Apps,
                    Type = "groups",
                });

                switch (AppSettings.ConnectivitySystem)
                {
                    case 1:
                        MyInfoList.Add(new Classes.MyInformation
                        {
                            Id = 5,
                            Name = ActivityContext.GetText(Resource.String.Lbl_Following),
                            Color = Color.ParseColor("#4CAF50"),
                            Icon = IonIconsFonts.PersonAdd,
                            Type = "following",
                        });

                        MyInfoList.Add(new Classes.MyInformation
                        {
                            Id = 6,
                            Name = ActivityContext.GetText(Resource.String.Lbl_Followers),
                            Color = Color.ParseColor("#FF9800"),
                            Icon = IonIconsFonts.IosPeople,
                            Type = "followers",
                        });
                        break;
                    default:
                        MyInfoList.Add(new Classes.MyInformation
                        {
                            Id = 5,
                            Name = ActivityContext.GetText(Resource.String.Lbl_Friends),
                            Color = Color.ParseColor("#FF9800"),
                            Icon = IonIconsFonts.IosPeople,
                            Type = "friends",
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => MyInfoList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_ItemInfoView
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_ItemInfoView, parent, false);
                var vh = new MyInformationAdapterViewHolder(itemView, Click, LongClick);
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
                    case MyInformationAdapterViewHolder holder:
                    {
                        var item = MyInfoList[position];
                        if (item != null)
                        {
                            holder.CardView.SetCardBackgroundColor(item.Color);
                            holder.TextInfo.Text = item.Name;

                            FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, holder.IconInfo, item.Icon);
                            holder.IconInfo.SetTextColor(item.Color);
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

        public Classes.MyInformation GetItem(int position)
        {
            return MyInfoList[position];
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

        private void Click(MyInformationAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(MyInformationAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
    }

    public class MyInformationAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MyInformationAdapterViewHolder(View itemView, Action<MyInformationAdapterClickEventArgs> clickListener, Action<MyInformationAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                CardView = MainView.FindViewById<CardView>(Resource.Id.cardView);
                IconInfo = MainView.FindViewById<TextView>(Resource.Id.iconInfo);
                TextInfo = MainView.FindViewById<TextView>(Resource.Id.textInfo); 

                //Event  
                itemView.Click += (sender, e) => clickListener(new MyInformationAdapterClickEventArgs {View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new MyInformationAdapterClickEventArgs {View = itemView, Position = AdapterPosition});

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        public CardView CardView { get; private set; }
        public TextView IconInfo { get; private set; }
        public TextView TextInfo { get; private set; } 

        #endregion
    }

    public class MyInformationAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}