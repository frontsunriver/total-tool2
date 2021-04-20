using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WoWonder.Activities.Articles
{
    [Activity(Label = "FilterCategoriesActivity")]
    public class FilterCategoriesActivity : AppCompatActivity, FilterCategoryAdapter.IOnClickListener
    {
        private RecyclerView rvCategory;
        private FilterCategoryAdapter categoryAdapter;
        private List<string> categories;
        private RelativeLayout rlClose;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.filter_articles_activity);

            //
            rlClose = FindViewById<RelativeLayout>(Resource.Id.rl_close);
            rlClose.Click += RlClose_Click;

            categories = JsonConvert.DeserializeObject<List<string>>(Intent.GetStringExtra("filter_category"));
            // 
            rvCategory = FindViewById<RecyclerView>(Resource.Id.rv_filter_category);
            categoryAdapter = new FilterCategoryAdapter(categories, this);
            rvCategory.SetLayoutManager(new LinearLayoutManager(this, RecyclerView.Vertical, false));
            rvCategory.SetAdapter(categoryAdapter);
        }

        private void RlClose_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void OnItemClick(string item)
        {
            Intent intent = new Intent();
            intent.PutExtra("category_item", item);
            SetResult(Result.Ok, intent);
            Finish();
        }

    }

    class FilterCategoryAdapter : RecyclerView.Adapter
    {
        private List<string> categories;
        private Context context;
        private IOnClickListener listener;

        public interface IOnClickListener
        {
            void OnItemClick(string item);
        }

        public FilterCategoryAdapter(List<string> categories, IOnClickListener listener)
        {
            this.categories = categories;
            this.listener = listener;
        }
        public override int ItemCount
        {
            get { return categories.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CategoryHolder vh = holder as CategoryHolder;
            vh.Bind(categories[position], listener);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            context = parent.Context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.filter_category_row, parent, false);
            CategoryHolder holder = new CategoryHolder(view);
            return holder;
        }

        class CategoryHolder : RecyclerView.ViewHolder
        {
            private TextView tvCategory;
            private IOnClickListener listener;

            public CategoryHolder(View itemView) : base(itemView)
            {
                tvCategory = itemView.FindViewById<TextView>(Resource.Id.tv_filter_category);
            }

            public void Bind(string category, IOnClickListener listener)
            {
                tvCategory.Text = category;

                this.listener = listener;
                ItemView.Click += ItemView_Click;
            }

            private void ItemView_Click(object sender, EventArgs e)
            {
                listener.OnItemClick(tvCategory.Text);
            }
        }
    }
}