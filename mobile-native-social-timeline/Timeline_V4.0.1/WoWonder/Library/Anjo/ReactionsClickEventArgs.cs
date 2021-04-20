using System;
using Android.Views;
using Android.Widget;

namespace WoWonder.Library.Anjo
{
    public class ReactionsClickEventArgs : EventArgs
    {
        public ImageView ImgButton { get; set; }
        public int Position { get; set; } 
        public string React { get; set; }
         
    }

    public class ReactionsClickLongClickEventArgs : View.LongClickEventArgs
    {
        public ImageView ImgButton { get; set; }
        public int Position { get; set; } 
        public string React { get; set; }

        public ReactionsClickLongClickEventArgs(bool handled) : base(handled)
        {
        }
    }
    public class ReactionsTouchEventArgs : View.TouchEventArgs
    {
        public ImageView ImgButton { get; set; }
        public int Position { get; set; } 
        public string React { get; set; }
         
        public ReactionsTouchEventArgs(bool handled, MotionEvent? e) : base(handled, e)
        {
        }
    }
}