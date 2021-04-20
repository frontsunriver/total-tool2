
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using AndroidX.Core.Content;
using AndroidX.ViewPager.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Java.IO;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;

namespace WoWonder.Activities.Market.Adapters
{
    public class MultiImagePagerAdapter : PagerAdapter, View.IOnClickListener
    {
        private readonly List<string> Images;
        private readonly LayoutInflater Inflater;
        private readonly ProductViewActivity Context;

        public MultiImagePagerAdapter(ProductViewActivity context, List<string> images)
        {
            try
            {
                Context = context;
                Images = images;
                Inflater = LayoutInflater.From(context);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public override int Count => Images?.Count ?? 0;

        public override Java.Lang.Object InstantiateItem(ViewGroup view, int position)
        {
            try
            {
                View imageLayout = Inflater.Inflate(Resource.Layout.Style_ImageNormal, view, false);
                ImageView imageView = imageLayout?.FindViewById<ImageView>(Resource.Id.image);
                imageLayout?.SetOnClickListener(this);

                if (Images[position].Contains("http"))
                {
                    Glide.With(Context).Load(Images[position]).Apply(RequestOptions.CenterCropTransform().Placeholder(Resource.Drawable.ImagePlacholder)).Into(imageView);
                }
                else
                {
                    File file2 = new File(Images[position]);
                    var photoUri = FileProvider.GetUriForFile(Context, Context.PackageName + ".fileprovider", file2);
                    Glide.With(Context).Load(photoUri).Apply(RequestOptions.CenterCropTransform().Placeholder(Resource.Drawable.ImagePlacholder)).Into(imageView);
                }
                 
                view.AddView(imageLayout, 0);
                return imageLayout;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }  
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view.Equals(@object);
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
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

        public void OnClick(View v)
        {
            try
            {
                Context?.ViewPagerViewOnClick();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}