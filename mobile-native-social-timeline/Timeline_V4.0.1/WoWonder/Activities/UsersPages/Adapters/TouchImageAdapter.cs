using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using AndroidX.ViewPager.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Sephiroth.ImageZoom;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.UsersPages.Adapters
{
    public class TouchImageAdapter : PagerAdapter 
    { 
        private readonly Activity ActivityContext;
        private readonly ObservableCollection<string> ImagesList;
        private readonly LayoutInflater Inflater;
        private ImageViewTouch Image;

        public TouchImageAdapter(Activity context, ObservableCollection<string> imagesList)
        {
            try
            {
                ActivityContext = context;
                ImagesList = new ObservableCollection<string>(imagesList);
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

                // set the default image display type
                //Image.SetDisplayType(ImageViewTouchBase.DisplayType.FitIfBigger);

                //var bit = drawable_from_url(new System.Uri(ImagesList[position].Image));
                //image.SetImageBitmap(bit);
                Glide.With(ActivityContext).Load(ImagesList[position]).Apply(new RequestOptions()).Into(Image);

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

        public override int Count => ImagesList?.Count ?? 0;

        //private Bitmap drawable_from_url(Uri url)
        //{
        //    try
        //    {
        //        if (Methods.CheckConnectivity())
        //            using (var webClient = new WebClient())
        //            {
        //                var imageBytes = webClient.DownloadData(url);
        //                if (imageBytes != null && imageBytes.Length > 0)
        //                {
        //                    var canvasBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        //                    //return new BitmapDrawable(ActivityContext.Resources, canvasBitmap);
        //                    return canvasBitmap;
        //                }

        //            }
        //        return null!;
        //    }
        //    catch (Exception e)
        //    {
        //        Methods.DisplayReportResultTrack(e);
        //        return null!;
        //    }
        //}
    }
}