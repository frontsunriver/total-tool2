using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using AndroidX.ViewPager.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Sephiroth.ImageZoom;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Posts; 
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.MyPhoto.Adapters
{
    public class TouchMyPhotoAdapter : PagerAdapter
    {

        private readonly Activity ActivityContext;
        private readonly ObservableCollection<PostDataObject> ImagesList;
        private readonly LayoutInflater Inflater;
        private ImageViewTouch Image;

        public TouchMyPhotoAdapter(Activity context, ObservableCollection<PostDataObject> imagesList)
        {
            try
            {
                ActivityContext = context;
                ImagesList = new ObservableCollection<PostDataObject>(imagesList);
                Inflater = LayoutInflater.From(context);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public override Object InstantiateItem(ViewGroup view, int position)
        {
            try
            {
                //ImageView 
                var layout = Inflater.Inflate(Resource.Layout.Style_MultiImageCoursalVeiw, view, false);
                Image = layout.FindViewById<ImageViewTouch>(Resource.Id.image);

                var imageUrl = !string.IsNullOrEmpty(ImagesList[position].PostSticker) ? ImagesList[position].PostSticker : ImagesList[position].PostFileFull; 
                Glide.With(ActivityContext).Load(imageUrl).Apply(new RequestOptions()).Into(Image);

                view.AddView(layout);

                return layout;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        public override void DestroyItem(ViewGroup container, int position, Object @object)
        {
            try
            {
                View view = (View)@object;
                container.RemoveView(view);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);

            }
        }

        public override bool IsViewFromObject(View view, Object @object)
        {
            return view.Equals(@object);
        }

        public override int Count
        {
            get
            {
                if (ImagesList != null)
                {
                    return ImagesList.Count;
                }

                return 0;
            }
        } 
    }
}