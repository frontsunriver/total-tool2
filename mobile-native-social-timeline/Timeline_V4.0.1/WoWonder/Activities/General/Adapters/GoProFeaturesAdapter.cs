using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.General.Adapters
{
    public class GoProFeaturesClass
    {
        public string HexColor { get; set; }
        public int StringResource { get; set; }

        public int ImageResource { get; set; }
    }

    public class GoProFeaturesAdapter : RecyclerView.Adapter
    { 
        #region Variables Basic

        public ObservableCollection<GoProFeaturesClass> FeaturesList = new ObservableCollection<GoProFeaturesClass>();
     
        #endregion

        public GoProFeaturesAdapter(Activity context)
        {
            try
            {
                HasStableIds = true;
                FeaturesList.Add(new GoProFeaturesClass { HexColor = "#7C99EC", StringResource = Resource.String.Lbl_go_pro_1,ImageResource = Resource.Drawable.gopro_medal });
                FeaturesList.Add(new GoProFeaturesClass { HexColor = "#2EC46A", StringResource = Resource.String.Lbl_go_pro_2, ImageResource = Resource.Drawable.gopro_notification });
                FeaturesList.Add(new GoProFeaturesClass { HexColor = "#F9B04F", StringResource = Resource.String.Lbl_go_pro_3, ImageResource = Resource.Drawable.gopro_flag });
                FeaturesList.Add(new GoProFeaturesClass { HexColor = "#FA375D", StringResource = Resource.String.Lbl_go_pro_4, ImageResource = Resource.Drawable.gopro_eye });
                FeaturesList.Add(new GoProFeaturesClass { HexColor = "#DC23EF", StringResource = Resource.String.Lbl_go_pro_5, ImageResource = Resource.Drawable.gopro_check });
                FeaturesList.Add(new GoProFeaturesClass { HexColor = "#4ACECA", StringResource = Resource.String.Lbl_go_pro_6, ImageResource = Resource.Drawable.gopro_check_loudspeaker });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => FeaturesList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_GoProFeatures_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_GoProFeatures_View, parent, false);
                var vh = new GoProViewHolder(itemView);
                return vh;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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
                    case GoProViewHolder holder:
                    {
                        var item = FeaturesList[position];
                        if (item != null)
                        {
                            holder.FeatureImg.SetImageResource(item.ImageResource);
                            holder.FeatureImg.SetColorFilter(Color.ParseColor(item.HexColor));
                            holder.FeatureText.Text = holder.MainView.Resources?.GetString(item.StringResource);

                            switch (AppSettings.SetTabDarkTheme)
                            {
                                case true:
                                    holder.MainLayout.SetBackgroundResource(Resource.Drawable.ShadowLinerLayoutDark);
                                    break;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public GoProFeaturesClass GetItem(int position)
        {
            return FeaturesList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        } 
    }

    public class GoProViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public LinearLayout MainLayout { get; private set; }
        public ImageView FeatureImg { get; private set; }
        public TextView FeatureText { get; private set; }
        public View MainView { get; }

        #endregion

        public GoProViewHolder(View itemView) : base(itemView)
        {
            try
            {
                MainView = itemView;

                MainLayout = MainView.FindViewById<LinearLayout>(Resource.Id.mainLayout);
                FeatureImg = MainView.FindViewById<ImageView>(Resource.Id.iv1);
                FeatureText = MainView.FindViewById<TextView>(Resource.Id.featureText);

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
    }

 
}