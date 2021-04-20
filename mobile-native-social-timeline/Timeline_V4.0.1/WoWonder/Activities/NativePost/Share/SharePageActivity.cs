using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using Refractored.Controls;
using System;
using System.Collections.Generic;
using Android.Content.PM;
using WoWonder.Activities.Base;
using WoWonder.Helpers.CacheLoaders;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using static WoWonder.Activities.NativePost.Share.SharePageAdapter;

namespace WoWonder.Activities.NativePost.Share
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class SharePageActivity : BaseActivity, IOnClickListener
    {
        public RecyclerView rvSharePage { get; private set; }
        private List<PageClass> pages;
        private PostDataObject postData;
        private SharePageAdapter sharePageAdapter;
        private TextView tvPageTitle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.share_group_sheet);

            pages = JsonConvert.DeserializeObject<List<PageClass>>(Intent.GetStringExtra("Pages"));
            postData = JsonConvert.DeserializeObject<PostDataObject>(Intent.GetStringExtra("PostObject"));

            rvSharePage = FindViewById<RecyclerView>(Resource.Id.rv_share_group);
            sharePageAdapter = new SharePageAdapter(pages, this);
            rvSharePage.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));
            rvSharePage.SetAdapter(sharePageAdapter);

            //
            RelativeLayout rlClose = FindViewById<RelativeLayout>(Resource.Id.rl_close);
            rlClose.Click += RlClose_Click;

            //
            tvPageTitle = FindViewById<TextView>(Resource.Id.tv_shareTo);
            tvPageTitle.Text = "Share to a Page";
        }

        private void RlClose_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void OnItemClick(PageClass item)
        {
            if (item != null)
            {
                Intent intent = new Intent(this, typeof(SharePostActivity));
                intent.PutExtra("ShareToType", "Page");
                intent.PutExtra("ShareToPage", JsonConvert.SerializeObject(item)); //PageClass
                intent.PutExtra("PostObject", JsonConvert.SerializeObject(postData)); //PostDataObject
                StartActivity(intent);
            }
        }
    }

    class SharePageAdapter : RecyclerView.Adapter
    {
        private List<PageClass> pageClasses;
        private Context context;
        private IOnClickListener listener;

        public interface IOnClickListener
        {
            void OnItemClick(PageClass item);
        }

        public SharePageAdapter(List<PageClass> pageClasses, IOnClickListener listener)
        {
            this.pageClasses = pageClasses;
            this.listener = listener;
        }

        public override int ItemCount
        {
            get { return pageClasses.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SharePageHolder vh = holder as SharePageHolder;

            PageClass item = pageClasses[position];
            GlideImageLoader.LoadImage((AppCompatActivity)context, item.Avatar, vh.ivGroup, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
            vh.tvGroupName.Text = item.PageName;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            context = parent.Context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.share_group_row, parent, false);

            return new SharePageHolder(view, listener, pageClasses);
        }

        class SharePageHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public CircleImageView ivGroup;
            public TextView tvGroupName;
            private IOnClickListener listener;
            private List<PageClass> pages;

            public SharePageHolder(View itemView, IOnClickListener listener, List<PageClass> pages
                ) : base(itemView)
            {
                this.pages = pages;
                this.listener = listener;

                ivGroup = itemView.FindViewById<CircleImageView>(Resource.Id.civ_group);
                tvGroupName = itemView.FindViewById<TextView>(Resource.Id.tv_group_name);

                ItemView.SetOnClickListener(this);
            }


            public void OnClick(View v)
            {
                listener.OnItemClick(pages[this.LayoutPosition]);
            }
        }
    }
}