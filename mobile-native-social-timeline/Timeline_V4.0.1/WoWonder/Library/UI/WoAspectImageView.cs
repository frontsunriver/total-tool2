using System;
using Android.Content;
using Android.Runtime;

using Android.Util;
using AndroidX.AppCompat.Widget;

namespace WoWonder.Library.UI
{
    public class WoAspectImageView: AppCompatImageView
    {
        
        protected WoAspectImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public WoAspectImageView(Context context) : base(context)
        {
        }

        public WoAspectImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public WoAspectImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int width = MeasuredWidth;
            int height = width * 9 / 16;
            SetMeasuredDimension(width, height);
        }
    }
}