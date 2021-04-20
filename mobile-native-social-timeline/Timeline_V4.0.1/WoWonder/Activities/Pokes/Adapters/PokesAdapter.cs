using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Util;
using Refractored.Controls;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Pocks;
using Exception = System.Exception;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Pokes.Adapters
{
    public interface IOnPokeItemClickListener
    {
        void PokeButtonClick(PokesClickEventArgs pokesClickEventArgs);
    }

   
    public class PokesAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {

        public event EventHandler<PokesAdapterClickEventArgs> ItemClick;
        public event EventHandler<PokesAdapterClickEventArgs> ItemLongClick;

        private readonly PokesActivity ActivityContext;
        public ObservableCollection<PokeObject.Datum> PokeList = new ObservableCollection<PokeObject.Datum>();
   
       public IOnPokeItemClickListener PokeItemClickListener { get; set; }
        public PokesAdapter(PokesActivity activity, IOnPokeItemClickListener clickListener)
        {
            try
            {
                PokeItemClickListener = clickListener;
                ActivityContext = activity;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => PokeList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HContact_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_HContact_view, parent, false);
                var vh = new PokesAdapterViewHolder(itemView, Click, LongClick);
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
                    case PokesAdapterViewHolder holder:
                    {
                        var item = PokeList[position];
                        if (item != null)
                        { 
                            if (item.UserData?.UserDataClass != null)
                            {
                                GlideImageLoader.LoadImage(ActivityContext, item.UserData?.UserDataClass.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable, true);
                                holder.Name.Text = Methods.FunString.SubStringCutOf(WoWonderTools.GetNameFinal(item.UserData?.UserDataClass), 20);

                                switch (item.UserData?.UserDataClass.Verified)
                                {
                                    case "1":
                                        holder.Name.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.icon_checkmark_small_vector, 0);
                                        break;
                                }

                                holder.About.Text = ActivityContext.GetString(Resource.String.Lbl_Last_seen) + " " + Methods.Time.TimeAgo(Convert.ToInt32(item.UserData?.UserDataClass.LastseenUnixTime), false);

                                //Online Or offline
                                var online = WoWonderTools.GetStatusOnline(Convert.ToInt32(item.UserData?.UserDataClass.LastseenUnixTime), item.UserData?.UserDataClass.LastseenStatus);
                                holder.ImageLastSeen.SetImageResource(online ? Resource.Drawable.Green_Color : Resource.Drawable.Grey_Offline);
                            }

                            holder.Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                            holder.Button.SetTextColor(Color.ParseColor("#ffffff"));
                            holder.Button.Text = ActivityContext.GetText(Resource.String.Lbl_PokeBack);
                            holder.BindEvents(position, item, holder.Button, PokeItemClickListener);
                      
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

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            try
            {
                if (ActivityContext?.IsDestroyed != false)
                        return;

                switch (holder)
                {
                    case PokesAdapterViewHolder viewHolder:
                        Glide.With(ActivityContext).Clear(viewHolder.Image);
                        break;
                }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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

        public PokeObject.Datum GetItem(int position)
        {
            return PokeList[position];
        }


        private void Click(PokesAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(PokesAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
         
        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = PokeList[p0];
                switch (item)
                {
                    case null:
                        return Collections.SingletonList(p0);
                }

                if (item.UserData?.UserDataClass != null && item.UserData.Value.UserDataClass.Avatar != "")
                {
                    d.Add(item.UserData.Value.UserDataClass.Avatar);
                    return d;
                }

                return d;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return Collections.SingletonList(p0);
            }
        }

        public RequestBuilder GetPreloadRequestBuilder(Java.Lang.Object p0)
        {
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CircleCrop);
        }

       
    }

    public class PokesAdapterViewHolder : RecyclerView.ViewHolder
    {
       

        public PokesAdapterViewHolder(View itemView, Action<PokesAdapterClickEventArgs> clickListener, Action<PokesAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.card_pro_pic);
                Name = MainView.FindViewById<TextView>(Resource.Id.card_name);
                About = MainView.FindViewById<TextView>(Resource.Id.card_dist);
                Button = MainView.FindViewById<Button>(Resource.Id.cont);
                ImageLastSeen = (CircleImageView)MainView.FindViewById(Resource.Id.ImageLastseen);

                //Event
                itemView.Click += (sender, e) => clickListener(new PokesAdapterClickEventArgs {View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new PokesAdapterClickEventArgs {View = itemView, Position = AdapterPosition});

                
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public void BindEvents(int position, PokeObject.Datum data, Button button, IOnPokeItemClickListener pokeItemClickListener)
        {
            Clicker.SetClickEvents(position, data, button, pokeItemClickListener);
            Button.SetOnClickListener(Clicker);
        }

        public class PokeClickEvents : Java.Lang.Object, View.IOnClickListener
        {
          
            public int Position;
            public PokeObject.Datum UserData;
            public Button PokeButton;
            public IOnPokeItemClickListener PokeItemClickListener;
            public void SetClickEvents(int position, PokeObject.Datum data, Button button, IOnPokeItemClickListener pokeItemClickListener)
            {
                PokeButton = button;
                   UserData = data;
                Position = position;
                PokeItemClickListener = pokeItemClickListener;
            }
            public void OnClick(View v)
            {
                if (PokeButton.Id == v.Id)
                {
                    PokeItemClickListener.PokeButtonClick(new PokesClickEventArgs { UserClass = UserData, Position = Position, ButtonFollow = PokeButton });
                }
            }

        }

        #region Variables Basic

        public View MainView { get; }
         
        public ImageView Image { get; private set; }
        public PokeClickEvents Clicker = new PokeClickEvents();
        public TextView Name { get; private set; }
        public TextView About { get; private set; }
        public Button Button { get; private set; }
        public CircleImageView ImageLastSeen { get; private set; }

        #endregion
    }

    public class PokesAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
     
    public class PokesClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public PokeObject.Datum UserClass { get; set; }
        public Button ButtonFollow { get; set; }
    }
}