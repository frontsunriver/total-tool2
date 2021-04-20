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
using static WoWonder.Activities.NativePost.Share.ShareGroupAdapter;

namespace WoWonder.Activities.NativePost.Share
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class ShareGroupActivity : BaseActivity, IOnClickListener
    {
        public RecyclerView rvShareGroup { get; private set; }
        private List<GroupClass> groups;
        private PostDataObject postData;
        private ShareGroupAdapter shareGroupAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.share_group_sheet);

            groups = JsonConvert.DeserializeObject<List<GroupClass>>(Intent.GetStringExtra("Groups"));
            postData = JsonConvert.DeserializeObject<PostDataObject>(Intent.GetStringExtra("PostObject"));

            rvShareGroup = FindViewById<RecyclerView>(Resource.Id.rv_share_group);
            shareGroupAdapter = new ShareGroupAdapter(groups, this);
            rvShareGroup.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));
            rvShareGroup.SetAdapter(shareGroupAdapter);

            //
            RelativeLayout rlClose = FindViewById<RelativeLayout>(Resource.Id.rl_close);
            rlClose.Click += RlClose_Click;
        }

        private void RlClose_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void OnItemClick(GroupClass item)
        {
            if (item != null)
            {
                Intent intent = new Intent(this, typeof(SharePostActivity));
                intent.PutExtra("ShareToType", "Group");
                intent.PutExtra("ShareToGroup", JsonConvert.SerializeObject(item)); //GroupClass
                intent.PutExtra("PostObject", JsonConvert.SerializeObject(postData)); //PostDataObject
                StartActivity(intent);
            }
        }
    }

    class ShareGroupAdapter : RecyclerView.Adapter
    {
        private List<GroupClass> groupClasses;
        private Context context;
        private IOnClickListener listener;

        public interface IOnClickListener
        {
            void OnItemClick(GroupClass item);
        }

        public ShareGroupAdapter(List<GroupClass> groupClasses, IOnClickListener listener)
        {
            this.groupClasses = groupClasses;
            this.listener = listener;
        }

        public override int ItemCount
        {
            get { return groupClasses.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ShareGroupHolder vh = holder as ShareGroupHolder;

            GroupClass item = groupClasses[position];
            GlideImageLoader.LoadImage((AppCompatActivity)context, item.Avatar, vh.ivGroup, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
            vh.tvGroupName.Text = item.GroupName;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            context = parent.Context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.share_group_row, parent, false);

            return new ShareGroupHolder(view, listener, groupClasses);
        }

        class ShareGroupHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public CircleImageView ivGroup;
            public TextView tvGroupName;
            private IOnClickListener listener;
            private List<GroupClass> groups;

            public ShareGroupHolder(View itemView, IOnClickListener listener, List<GroupClass> groups
                ) : base(itemView)
            {
                this.groups = groups;
                this.listener = listener;

                ivGroup = itemView.FindViewById<CircleImageView>(Resource.Id.civ_group);
                tvGroupName = itemView.FindViewById<TextView>(Resource.Id.tv_group_name);

                ItemView.SetOnClickListener(this);
            }


            public void OnClick(View v)
            {
                listener.OnItemClick(groups[this.LayoutPosition]);
            }
        }
    }

}