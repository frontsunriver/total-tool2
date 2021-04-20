using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;


namespace WoWonder.Library.Anjo.SuperTextLibrary
{
    public class XTouchableSpan : ClickableSpan
    {
        private bool IsPressed;
        private readonly Color NormalTextColor;
        private readonly Color PressedTextColor;
        private readonly bool IsUnderLineEnabled;
        private readonly SuperTextView MoreTextView;
        private readonly StTools.XAutoLinkItem AutoLinkItem;

        public XTouchableSpan(SuperTextView view, StTools.XAutoLinkItem item, Color normalTextColor, Color pressedTextColor, bool isUnderLineEnabled = false)
        {
            MoreTextView = view;
            AutoLinkItem = item;
            NormalTextColor = normalTextColor;
            PressedTextColor = pressedTextColor;
            IsUnderLineEnabled = isUnderLineEnabled;
        }

        public void SetPressed(bool isSelected)
        {
            IsPressed = isSelected;
        }

        public override void OnClick(View widget)
        {
            try
            {
                if (AutoLinkItem != null)
                {
                    MoreTextView.AutoLinkOnClickListener?.AutoLinkTextClick(AutoLinkItem.GetAutoLinkMode(), AutoLinkItem.GetMatchedText(), AutoLinkItem.GetUserIdText());
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            try
            {
                base.UpdateDrawState(ds);
                Color textColor = IsPressed ? PressedTextColor : NormalTextColor;
                ds.Color = textColor;
                ds.BgColor = Color.Transparent;
                ds.UnderlineText = IsUnderLineEnabled;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}