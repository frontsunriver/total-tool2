using System;
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables; 
using Android.Widget;
using AndroidX.Core.Content;
using Bumptech.Glide;
using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Load.Engine.BitmapRecycle;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Security;
using WoWonder.Helpers.Utils;

namespace WoWonder.Helpers.CacheLoaders
{
    public enum ImageStyle
    {
        CenterCrop, CircleCrop, RoundedCrop, FitCenter , CircleCropWithBorder
    }

    public enum ImagePlaceholders
    {
        Color, Drawable
    }

    public static class GlideImageLoader
    { 
        public static void LoadImage(Activity activity, string imageUri, ImageView image, ImageStyle style, ImagePlaceholders imagePlaceholders, bool compress = false , RequestOptions options = null)
        {
            try
            { 
                if (string.IsNullOrEmpty(imageUri) || string.IsNullOrWhiteSpace(imageUri) || image == null || activity?.IsDestroyed != false)
                    return;

                imageUri = imageUri.Replace(" ", "");

                var newImage = Glide.With(activity);
                 
                options ??= GetOptions(style, imagePlaceholders);

                switch (compress)
                {
                    case true when style != ImageStyle.RoundedCrop:
                    {
                        if (imageUri.Contains("avatar") || imageUri.Contains("Avatar"))
                            options.Override(AppSettings.AvatarPostSize);
                        else if (imageUri.Contains("gif"))
                            options.Override(AppSettings.ImagePostSize);
                        else
                            options.Override(AppSettings.ImagePostSize);
                        break;
                    }
                }

                switch (compress)
                {
                    case true:
                        options.Override(AppSettings.ImagePostSize);
                        break;
                }

                if (imageUri.Contains("no_profile_image") || imageUri.Contains("blackdefault") || imageUri.Contains("no_profile_image_circle")
                    || imageUri.Contains("ImagePlacholder") || imageUri.Contains("ImagePlacholder_circle") || imageUri.Contains("Grey_Offline")
                    || imageUri.Contains("Image_File")|| imageUri.Contains("Audio_File") || imageUri.Contains("addImage") || imageUri.Contains("d-group")
                    || imageUri.Contains("d-cover") || imageUri.Contains("d-avatar")|| imageUri.Contains("user_anonymous"))
                {
                    if (imageUri.Contains("no_profile_image_circle"))
                        newImage.Load(Resource.Drawable.no_profile_image_circle).Apply(options).Into(image);
                    else if (imageUri.Contains("no_profile_image") || imageUri.Contains("d-avatar"))
                        newImage.Load(Resource.Drawable.no_profile_image).Apply(options).Into(image);
                    else if (imageUri.Contains("ImagePlacholder"))
                        newImage.Load(Resource.Drawable.ImagePlacholder).Apply(options).Into(image);
                    else if (imageUri.Contains("ImagePlacholder_circle"))
                        newImage.Load(Resource.Drawable.ImagePlacholder_circle).Apply(options).Into(image);
                    else if (imageUri.Contains("blackdefault"))
                        newImage.Load(Resource.Drawable.blackdefault).Apply(options).Into(image);
                    else if (imageUri.Contains("Grey_Offline"))
                        newImage.Load(Resource.Drawable.Grey_Offline).Apply(options).Into(image);
                    else if (imageUri.Contains("Image_File"))
                        newImage.Load(Resource.Drawable.Image_File).Apply(options).Into(image);
                    else if (imageUri.Contains("Audio_File"))
                        newImage.Load(Resource.Drawable.Audio_File).Apply(options).Into(image);
                    else if (imageUri.Contains("addImage"))
                        newImage.Load(Resource.Drawable.addImage).Apply(options).Into(image);
                    else if (imageUri.Contains("d-group"))
                        newImage.Load(Resource.Drawable.default_group).Apply(options).Into(image);
                    else if (imageUri.Contains("d-page"))
                        newImage.Load(Resource.Drawable.default_page).Apply(options).Into(image);
                    else if (imageUri.Contains("d-cover"))
                        newImage.Load(Resource.Drawable.Cover_image).Apply(options).Into(image);
                    else if (imageUri.Contains("user_anonymous"))
                        newImage.Load(Resource.Drawable.user_anonymous).Apply(options).Into(image);
                }
                else switch (string.IsNullOrEmpty(imageUri))
                {
                    case false when imageUri.Contains("http"):
                        newImage.Load(imageUri).Apply(options).Into(image);
                        break;
                    case false when imageUri.Contains("file://") || imageUri.Contains("content://") || imageUri.Contains("storage") || imageUri.Contains("/data/user/0/"):
                    {
                        File file2 = new File(imageUri);
                        var photoUri = FileProvider.GetUriForFile(activity, activity.PackageName + ".fileprovider", file2);
                        RequestOptions option = style == ImageStyle.CircleCrop ? new RequestOptions().CircleCrop() : new RequestOptions();
                        Glide.With(activity).Load(photoUri).Apply(option).Into(image);
                        break;
                    }
                    default:
                        newImage.Load(Resource.Drawable.no_profile_image).Apply(options).Into(image);
                        break;
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public static void LoadImage(Activity activity, int imageUri, ImageView image, ImageStyle style, ImagePlaceholders imagePlaceholders, bool compress = false , RequestOptions options = null)
        {
            try
            { 
                if (image == null || activity?.IsDestroyed != false)
                    return;
                 
                var newImage = Glide.With(activity);
                 
                options ??= GetOptions(style, imagePlaceholders);

                switch (compress)
                {
                    case true when style != ImageStyle.RoundedCrop:
                        options.Override(AppSettings.ImagePostSize);
                        break;
                }

                switch (compress)
                {
                    case true:
                        options.Override(AppSettings.ImagePostSize);
                        break;
                }

                newImage.Load(imageUri).Apply(options).Into(image);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static RequestOptions GetOptions(ImageStyle style, ImagePlaceholders imagePlaceholders)
        {
            try
            {
                RequestOptions options = new RequestOptions();

                switch (style)
                {
                    case ImageStyle.CenterCrop:
                        options = new RequestOptions().Apply(RequestOptions.CenterCropTransform()
                            .CenterCrop()
                            .SetPriority(Priority.High)
                            .SetUseAnimationPool(false).SetDiskCacheStrategy(DiskCacheStrategy.All).AutoClone() 
                            .Error(Resource.Drawable.ImagePlacholder)
                            .Placeholder(Resource.Drawable.ImagePlacholder));
                        break;
                    case ImageStyle.FitCenter:
                        options = new RequestOptions().Apply(RequestOptions.CenterCropTransform().AutoClone()
                            .FitCenter()
                            .SetPriority(Priority.High)
                            .SetUseAnimationPool(false).SetDiskCacheStrategy(DiskCacheStrategy.All)
                            .Error(Resource.Drawable.ImagePlacholder)
                            .Placeholder(Resource.Drawable.ImagePlacholder));
                        break;
                    case ImageStyle.CircleCrop:
                        options = new RequestOptions().Apply(RequestOptions.CircleCropTransform().AutoClone()
                            .CenterCrop().CircleCrop()
                            .SetPriority(Priority.High)
                            .SetUseAnimationPool(false).SetDiskCacheStrategy(DiskCacheStrategy.All)
                            .Error(Resource.Drawable.ImagePlacholder_circle)
                            .Placeholder(Resource.Drawable.ImagePlacholder_circle));
                        break;
                    case ImageStyle.CircleCropWithBorder:
                        options = new RequestOptions().Apply(RequestOptions.CircleCropTransform().AutoClone()
                            .CenterCrop().CircleCrop()
                            .Transform(new GlideCircleWithBorder(1, Color.Black))
                            .SetPriority(Priority.High)
                            .SetUseAnimationPool(false).SetDiskCacheStrategy(DiskCacheStrategy.All)
                            .Error(Resource.Drawable.ImagePlacholder_circle)
                            .Placeholder(Resource.Drawable.ImagePlacholder_circle));
                        break;
                    case ImageStyle.RoundedCrop:
                        options = new RequestOptions().Apply(RequestOptions.CircleCropTransform().AutoClone()
                            .CenterCrop()
                            .Transform(new MultiTransformation(new CenterCrop(), new RoundedCorners(20)))
                            .SetPriority(Priority.High)
                            .SetUseAnimationPool(false).SetDiskCacheStrategy(DiskCacheStrategy.All)
                            .Error(Resource.Drawable.ImagePlacholder_circle)
                            .Placeholder(Resource.Drawable.ImagePlacholder_circle));
                        break;

                    default:
                        options.CenterCrop();
                        break;
                }

                switch (imagePlaceholders)
                {
                    case ImagePlaceholders.Color:
                        var color = Methods.FunString.RandomColor();
                        options.Placeholder(new ColorDrawable(Color.ParseColor(color))).Fallback(new ColorDrawable(Color.ParseColor(color)));
                        break;
                    case ImagePlaceholders.Drawable:
                        switch (style)
                        {
                            case ImageStyle.CircleCrop:
                            case ImageStyle.CircleCropWithBorder:
                                options.Placeholder(Resource.Drawable.ImagePlacholder_circle).Fallback(Resource.Drawable.ImagePlacholder_circle);
                                break;
                            default:
                                options.Placeholder(Resource.Drawable.ImagePlacholder).Fallback(Resource.Drawable.ImagePlacholder);
                                break;
                        }
                        break;
                }
                 
                return options;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new RequestOptions().CenterCrop(); 
            }
        }

        public static RequestBuilder GetPreLoadRequestBuilder(Activity activityContext, string url, ImageStyle style)
        {
            try
            {
                if (url == null || string.IsNullOrEmpty(url))
                    return null!;
                 
                var options = GetOptions(style, ImagePlaceholders.Drawable);

                if (url.Contains("avatar"))
                    options.CircleCrop();

                if (url.Contains("avatar"))
                {
                    options.Override(AppSettings.AvatarPostSize);
                }
                else if (url.Contains("gif"))
                {
                    options.Override(AppSettings.ImagePostSize);
                }
                else
                {
                    options.Override(AppSettings.ImagePostSize);
                }
                
                options.SetDiskCacheStrategy(DiskCacheStrategy.All);
                 
                return Glide.With(activityContext)
                    .Load(url) 
                    .Apply(options);

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }

        }

        public static RequestOptions GetRequestOptions(ImageStyle style, ImagePlaceholders imagePlaceholders)
        {
            try
            {
                var options = new RequestOptions();


                switch (style)
                {
                    case ImageStyle.CenterCrop:
                        options.CenterCrop();
                        break;
                    case ImageStyle.FitCenter:
                        options.FitCenter();
                        break;
                    case ImageStyle.CircleCrop:
                        options.CircleCrop();
                        break;
                    case ImageStyle.CircleCropWithBorder:
                        options.CircleCrop();
                        options.Transform(new GlideCircleWithBorder(2, Color.White));
                        break;
                    case ImageStyle.RoundedCrop:
                        options.Transform(new MultiTransformation(new CenterCrop(), new RoundedCorners(20)));
                        break;

                    default:
                        options.CenterCrop();
                        break;
                }


                switch (imagePlaceholders)
                {
                    case ImagePlaceholders.Color:
                        var color = Methods.FunString.RandomColor();
                        options.Placeholder(new ColorDrawable(Color.ParseColor(color))).Fallback(new ColorDrawable(Color.ParseColor(color)));
                        break;
                    case ImagePlaceholders.Drawable:
                        options.Placeholder(Resource.Drawable.ImagePlacholder).Fallback(Resource.Drawable.ImagePlacholder);
                        break;
                }

                return options;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new RequestOptions(); 
            }

        }
    }

    public class GlideCircleWithBorder : BitmapTransformation
    {
        private readonly Paint MBorderPaint;
        private readonly float MBorderWidth;
         
        public GlideCircleWithBorder(int borderWidth, Color borderColor)
        {
            try
            {
                if (Resources.System?.DisplayMetrics != null)
                    MBorderWidth = Resources.System.DisplayMetrics.Density * borderWidth;

                MBorderPaint = new Paint {Dither = true, AntiAlias = true, Color = borderColor};
                MBorderPaint.SetStyle(Paint.Style.Stroke);
                MBorderPaint.StrokeWidth=MBorderWidth;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        protected override Bitmap Transform(IBitmapPool pool, Bitmap toTransform, int outWidth, int outHeight)
        {
            return CircleCrop(pool, toTransform);
        }

        public override void UpdateDiskCacheKey(MessageDigest p0)
        {
            
        }

        private Bitmap CircleCrop(IBitmapPool pool, Bitmap source)
        {
            try
            {
                switch (source)
                {
                    case null:
                        return null;
                }
                int size = (int)(Math.Min(source.Width, source.Height) - MBorderWidth / 2);
                int x = (source.Width - size) / 2;
                int y = (source.Height- size) / 2;
                Bitmap squared = Bitmap.CreateBitmap(source, x, y, size, size);
                Bitmap result = pool.Get(size, size, Bitmap.Config.Argb8888);
                switch (result)
                {
                    case null:
                        result = Bitmap.CreateBitmap(size, size, Bitmap.Config.Argb8888);
                        break;
                }
                //Create a brush Canvas Manually draw a border
                Canvas canvas = new Canvas(result);
                Paint paint = new Paint();
                paint.SetShader(new BitmapShader(squared, Shader.TileMode.Clamp, Shader.TileMode.Clamp));
                paint.AntiAlias=true;
                float r = size / 2f;
                canvas.DrawCircle(r, r, r, paint);
                if (MBorderPaint != null)
                {
                    float borderRadius = r - MBorderWidth / 2;
                    canvas.DrawCircle(r, r, borderRadius, MBorderPaint);
                }
                return result;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }
    }
}