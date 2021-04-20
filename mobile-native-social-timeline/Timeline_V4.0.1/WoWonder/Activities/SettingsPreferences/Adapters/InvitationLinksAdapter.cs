using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;

namespace WoWonder.Activities.SettingsPreferences.Adapters
{
    public class InvitationLinksAdapter : RecyclerView.Adapter
    {
        public event EventHandler<InvitationLinksAdapterClickEventArgs> CopyItemClick;
        public event EventHandler<InvitationLinksAdapterClickEventArgs> ItemClick;
        public event EventHandler<InvitationLinksAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<InvitationDataObject> LinksList = new ObservableCollection<InvitationDataObject>();

        public InvitationLinksAdapter(Activity context)
        {
            try
            {
                //HasStableIds = true;
                ActivityContext = context;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => LinksList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_GeneratedLinksView
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_GeneratedLinksView, parent, false);
                var vh = new InvitationLinksAdapterViewHolder(itemView, CopyClick, Click, LongClick);
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
                    case InvitationLinksAdapterViewHolder holder:
                    {
                        var item = LinksList[position];
                        if (item != null)
                        {
                            DateTime dateTimeSeen = Methods.Time.UnixTimeStampToDateTime(Convert.ToInt32(item.Time));
                            holder.Date.Text = dateTimeSeen.ToLongDateString();

                            if (item.UserData?.UserDataClass != null) 
                                holder.InvitedUser.Text = WoWonderTools.GetNameFinal(item.UserData?.UserDataClass);
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

        public InvitationDataObject GetItem(int position)
        {
            return LinksList[position];
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

        private void CopyClick(InvitationLinksAdapterClickEventArgs args)
        {
            CopyItemClick?.Invoke(this, args);
        }

        private void Click(InvitationLinksAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(InvitationLinksAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
    }

    public class InvitationLinksAdapterViewHolder : RecyclerView.ViewHolder
    {
        public InvitationLinksAdapterViewHolder(View itemView, Action<InvitationLinksAdapterClickEventArgs> copyClickListener, Action<InvitationLinksAdapterClickEventArgs> clickListener, Action<InvitationLinksAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Date = MainView.FindViewById<TextView>(Resource.Id.date);
                InvitedUser = MainView.FindViewById<TextView>(Resource.Id.invitedUser);
                Button = MainView.FindViewById<ImageView>(Resource.Id.copyLink);

                //Event  
                Button.Click += (sender, e) => copyClickListener(new InvitationLinksAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.Click += (sender, e) => clickListener(new InvitationLinksAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new InvitationLinksAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        public TextView InvitedUser { get; private set; }
        public TextView Date { get; private set; }
        public ImageView Button { get; private set; }

        #endregion
    }

    public class InvitationLinksAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}