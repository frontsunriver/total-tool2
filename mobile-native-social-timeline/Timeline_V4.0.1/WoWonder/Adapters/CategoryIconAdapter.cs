using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;

namespace WoWonder.Adapters
{
    public class CategoryIconAdapter : RecyclerView.Adapter 
    {
        public event EventHandler<CategoryIconAdapterClickEventArgs> ItemClick;
        public event EventHandler<CategoryIconAdapterClickEventArgs> ItemLongClick;
        private int Position;
        private readonly Activity ActivityContext;
        public ObservableCollection<Classes.Categories> CategoryList = new ObservableCollection<Classes.Categories>();

        public CategoryIconAdapter(Activity context)
        {
            HasStableIds = true;
            ActivityContext = context;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_CategoriesIconView, parent, false); 
                var vh = new CategoryIconAdapterViewHolder(itemView, OnClick, OnLongClick);
                return vh;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null;
            }
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                Position = position;

                switch (viewHolder)
                {
                    case CategoryIconAdapterViewHolder holder:
                    {
                        var item = CategoryList[Position];
                        if (item != null)
                        {
                            holder.TxtCategoryName.Text = item.CategoriesName;

                            FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, holder.CategoryIcon, item.CategoriesIcon);
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

        public override int ItemCount => CategoryList?.Count ?? 0;

        public Classes.Categories GetItem(int position)
        {
            return CategoryList[position];
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

        void OnClick(CategoryIconAdapterClickEventArgs args) => ItemClick?.Invoke(ActivityContext, args);
        void OnLongClick(CategoryIconAdapterClickEventArgs args) => ItemLongClick?.Invoke(ActivityContext, args); 
    }

    public class CategoryIconAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }
        public TextView CategoryIcon { get; private set; }
        public TextView TxtCategoryName { get; private set; }

        #endregion

        public CategoryIconAdapterViewHolder(View itemView, Action<CategoryIconAdapterClickEventArgs> clickListener, Action<CategoryIconAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                //Get values
                CategoryIcon = (TextView)MainView.FindViewById(Resource.Id.Icon);
                TxtCategoryName = MainView.FindViewById<TextView>(Resource.Id.name);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new CategoryIconAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new CategoryIconAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }

    public class CategoryIconAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}