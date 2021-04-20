using System;
using Android.Content;
using Android.Runtime;


using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.AsyncLayoutInflater.View;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Fonts;

namespace WoWonder.Library.UI
{
    public class WoLayoutAsyncLoader :LinearLayout
    {
        protected WoLayoutAsyncLoader(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public WoLayoutAsyncLoader(Context context) : base(context)
        {
            
        }

        public WoLayoutAsyncLoader(Context context, IAttributeSet attrs) : base(context, attrs)
        {
           
        }

        public WoLayoutAsyncLoader(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
           
        }

        public WoLayoutAsyncLoader(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
           
        }

        private AdapterHolders.PostTopSectionViewHolder PostTopSectionViewHolder;

        public void Load()
        {
            AsyncLayoutInflater inflater = new AsyncLayoutInflater(Context);
            inflater.Inflate(Resource.Layout.Post_TopSection_Layout, this, new HeavyLayoutAsyncInflater(PostTopSectionViewHolder));
        }

        public class HeavyLayoutAsyncInflater : Java.Lang.Object, AsyncLayoutInflater.IOnInflateFinishedListener
        {
            private readonly AdapterHolders.PostTopSectionViewHolder PostTopSectionViewHolder;

            public HeavyLayoutAsyncInflater(AdapterHolders.PostTopSectionViewHolder postTopSectionViewHolder)
            {
                PostTopSectionViewHolder = postTopSectionViewHolder;
            }
            public void OnInflateFinished(View view, int resId, ViewGroup parent)
            {
                PostTopSectionViewHolder.Username = view.FindViewById<TextViewWithImages>(Resource.Id.username);
                PostTopSectionViewHolder.TimeText = view.FindViewById<AppCompatTextView>(Resource.Id.time_text);
                PostTopSectionViewHolder.PrivacyPostIcon = view.FindViewById<ImageView>(Resource.Id.privacyPost);
                PostTopSectionViewHolder.UserAvatar = view.FindViewById<ImageView>(Resource.Id.userAvatar);
                 
                parent.AddView(view);
            }
        }

        
        public void ParseViews(AdapterHolders.PostTopSectionViewHolder postTopSectionViewHolder)
        {
            PostTopSectionViewHolder = postTopSectionViewHolder;
            Load();
        }
    }
}