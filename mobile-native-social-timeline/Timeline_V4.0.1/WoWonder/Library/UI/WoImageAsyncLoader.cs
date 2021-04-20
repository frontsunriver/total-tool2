using System;
using Android.Content;
using Android.Runtime;

using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AsyncLayoutInflater.View;
using ImageViews.Rounded;
using WoWonder.Activities.NativePost.Post;

namespace WoWonder.Library.UI
{
    public class WoImageAsyncLoader : LinearLayout
    {
        protected WoImageAsyncLoader(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public WoImageAsyncLoader(Context context) : base(context)
        {
            Load();
        }

        public WoImageAsyncLoader(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Load();
        }

        public WoImageAsyncLoader(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Load();
        }

        public WoImageAsyncLoader(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Load();
        }

        private AdapterHolders.PostImageSectionViewHolder PostImageSectionViewHolder;
        public HeavyLayoutAsyncInflater AsyncHolderInflater;

        private void Load()
        {
            AsyncLayoutInflater inflater = new AsyncLayoutInflater(Context);
            AsyncHolderInflater = new HeavyLayoutAsyncInflater();
            inflater.Inflate(Resource.Layout.Post_Content_Image_Layout, this, AsyncHolderInflater);
        }

        public class HeavyLayoutAsyncInflater : Java.Lang.Object, AsyncLayoutInflater.IOnInflateFinishedListener
        {

            private View MainView;
            public HeavyLayoutAsyncInflater()
            {

            }
            public void OnInflateFinished(View view, int resId, ViewGroup parent)
            {
                MainView = view;
                parent.AddView(view);
                Console.WriteLine("TraceWO " + " ViewAdded " + view);
            }

            public void SetupViews(AdapterHolders.PostImageSectionViewHolder postImageSectionViewHolder)
            {
                postImageSectionViewHolder.Image = MainView.FindViewById<RoundedImageView>(Resource.Id.Image);
            }
        }


        public void ParseViews(AdapterHolders.PostImageSectionViewHolder postImageSectionViewHolder)
        {
            PostImageSectionViewHolder = postImageSectionViewHolder;
            Console.WriteLine("TraceWO " + " ParseViews " + postImageSectionViewHolder.Image);
            //Load();

        }
    }
}