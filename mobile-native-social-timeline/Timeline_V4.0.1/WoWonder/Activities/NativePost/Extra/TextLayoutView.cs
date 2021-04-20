using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;

namespace WoWonder.Activities.NativePost.Extra
{
    public class TextLayoutView :View
    {
        private Layout MLayout;

        protected TextLayoutView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TextLayoutView(Context context) : base(context)
        {
        }

        public TextLayoutView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public TextLayoutView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public TextLayoutView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public void SetTextLayout(Layout layout)
        {
            MLayout = layout;
            RequestLayout();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            canvas.Save();

            if (MLayout != null)
            {
                canvas.Translate(PaddingLeft,PaddingTop);
                MLayout.Draw(canvas);
            }

            canvas.Restore();
        }
    }
}