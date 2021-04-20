using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Java.Lang;
using WoWonder.Helpers.Utils;
using Exception = Java.Lang.Exception;

namespace WoWonder.Library.Anjo.SuperTextLibrary
{
    public class XLinkTouchMovementMethod : LinkMovementMethod
    {
        private XTouchableSpan PressedSpan;

        public override bool OnTouchEvent(TextView textView, ISpannable spannable, MotionEvent e)
        {
            try
            {
                var action = e.Action;
                switch (action)
                {
                    case MotionEventActions.Down:
                    {
                        PressedSpan = GetPressedSpan(textView, spannable, e);
                        if (PressedSpan != null)
                        {
                            PressedSpan.SetPressed(true);
                            Selection.SetSelection(spannable, spannable.GetSpanStart(PressedSpan), spannable.GetSpanEnd(PressedSpan));
                        }

                        break;
                    }
                    case MotionEventActions.Move:
                    {
                        XTouchableSpan touchedSpan = GetPressedSpan(textView, spannable, e);
                        if (PressedSpan != null && touchedSpan != PressedSpan)
                        {
                            PressedSpan.SetPressed(false);
                            PressedSpan = null!;
                            Selection.RemoveSelection(spannable);
                        }

                        break;
                    }
                    default:
                    {
                        if (PressedSpan != null)
                        {
                            PressedSpan.SetPressed(false);
                            base.OnTouchEvent(textView, spannable, e);
                        }

                        PressedSpan = null!;
                        Selection.RemoveSelection(spannable);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }

            return true;

        }

        private XTouchableSpan GetPressedSpan(TextView textView, ISpannable spannable, MotionEvent e)
        { 
            try
            {
                int x = (int)e.GetX();
                int y = (int)e.GetY();

                x -= textView.TotalPaddingLeft;
                y -= textView.TotalPaddingTop;

                x += textView.ScrollX;
                y += textView.ScrollY;

                Layout layout = textView.Layout;
                int verticalLine = layout.GetLineForVertical(y);
                int horizontalOffset = layout.GetOffsetForHorizontal(verticalLine, x);

                var link = spannable.GetSpans(horizontalOffset, horizontalOffset, Class.FromType(typeof(XTouchableSpan)));

                switch (link?.Length)
                {
                    case > 0:
                    {
                        var sdfs = (XTouchableSpan)link[0];
                        return sdfs;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }

            return null!;
        }

    }
}