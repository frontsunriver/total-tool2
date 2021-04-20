using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime; 
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Java.Lang;
using Java.Lang.Reflect;
using Java.Util.Regex;
using System;
using System.Collections.Generic;
using Android.Widget;
using AndroidX.Core.Content;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;
using Pattern = Java.Util.Regex.Pattern;

namespace WoWonder.Library.Anjo.SuperTextLibrary
{
    public class SuperTextView : TextView
    {
        private static readonly int MinPhoneNumberLength = 8;
        private static readonly Color DefaultColor = Color.Blue;
        public StTools.IXAutoLinkOnClickListener AutoLinkOnClickListener;
        private Dictionary<string, string> UserId;
        private StTools.XAutoLinkMode[] AutoLinkModes;
        private List<StTools.XAutoLinkMode> MBoldAutoLinkModes;
        private string CustomRegex;
        private bool IsUnderLineEnabled;
        private static Color MentionModeColor { set; get; } 
        private static Color HashtagModeColor { set; get; }
        private static Color UrlModeColor { set; get; }
        private static Color PhoneModeColor { set; get; }
        private static Color EmailModeColor { set; get; }
        private static Color CustomModeColor { set; get; }
        private static Color DefaultSelectedColor = Color.LightGray;
         
        protected SuperTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SuperTextView(Context context) : base(context)
        {
            Init(context);
        }

        public SuperTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public SuperTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context);
        }

        public SuperTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            try
            {
                AddAutoLinkMode(new[] { StTools.XAutoLinkMode.ModePhone, StTools.XAutoLinkMode.ModeEmail, StTools.XAutoLinkMode.ModeHashTag, StTools.XAutoLinkMode.ModeUrl, StTools.XAutoLinkMode.ModeMention });
                SetPhoneModeColor(new Color(ContextCompat.GetColor(context, Resource.Color.AutoLinkText_ModePhone_color)));
                SetEmailModeColor(new Color(ContextCompat.GetColor(context, Resource.Color.AutoLinkText_ModeEmail_color)));
                SetHashtagModeColor(new Color(ContextCompat.GetColor(context, Resource.Color.AutoLinkText_ModeHashtag_color)));
                SetUrlModeColor(new Color(ContextCompat.GetColor(context, Resource.Color.AutoLinkText_ModeUrl_color)));
                SetMentionModeColor(Color.ParseColor(AppSettings.MainColor));
                //SetCustomModeColor(Color.ParseColor(AppSettings.MainColor));
                //EnableUnderLine();
                //SetAutoLinkOnClickListener(this); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetTextInfo(SuperTextView showMore)
        {
            try
            {
                if (showMore != null)
                {
                    showMore.AddAutoLinkMode(new[] { StTools.XAutoLinkMode.ModePhone, StTools.XAutoLinkMode.ModeEmail, StTools.XAutoLinkMode.ModeHashTag, StTools.XAutoLinkMode.ModeUrl, StTools.XAutoLinkMode.ModeMention });
                    showMore.SetPhoneModeColor(new Color(ContextCompat.GetColor(Context, Resource.Color.AutoLinkText_ModePhone_color)));
                    showMore.SetEmailModeColor(new Color(ContextCompat.GetColor(Context, Resource.Color.AutoLinkText_ModeEmail_color)));
                    showMore.SetHashtagModeColor(new Color(ContextCompat.GetColor(Context, Resource.Color.AutoLinkText_ModeHashtag_color)));
                    showMore.SetUrlModeColor(new Color(ContextCompat.GetColor(Context, Resource.Color.AutoLinkText_ModeUrl_color)));
                    showMore.SetMentionModeColor(Color.ParseColor(AppSettings.MainColor));
                    //showMore.SetCustomModeColor(Color.ParseColor(AppSettings.MainColor));
                    //showMore.EnableUnderLine();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void SetText(ICharSequence text, BufferType type)
        {
            try
            {
                if (TextUtils.IsEmpty(text))
                {
                    base.SetText(text, type);
                    return;
                }

                SpannableString spannableString = MakeSpannAbleString(text);
                MovementMethod = new XLinkTouchMovementMethod();

                base.SetText(spannableString, type);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                base.SetText(text, type);
            }
        }
        
        private SpannableString MakeSpannAbleString(ICharSequence text)
        {
            try
            {
                SpannableString spannableString = new SpannableString(text);

                List<StTools.XAutoLinkItem> autoLinkItems = MatchedRanges(text);

                foreach (var autoLinkItem in autoLinkItems)
                {
                    Console.WriteLine("Run foreach autoLinkItem => 135");
                    Color currentColor = GetColorByMode(autoLinkItem.GetAutoLinkMode());
                    XTouchableSpan clickAbleSpan = new XTouchableSpan(this, autoLinkItem, currentColor, DefaultSelectedColor, IsUnderLineEnabled);

                    spannableString.SetSpan(clickAbleSpan, autoLinkItem.GetStartPoint(), autoLinkItem.GetEndPoint(), SpanTypes.ExclusiveExclusive);

                    // check if we should make this auto link item bold
                    if (MBoldAutoLinkModes != null && MBoldAutoLinkModes.Contains(autoLinkItem.GetAutoLinkMode()))
                    {
                        // make the auto link item bold
                        spannableString.SetSpan(new StyleSpan(TypefaceStyle.Bold), autoLinkItem.GetStartPoint(), autoLinkItem.GetEndPoint(), SpanTypes.ExclusiveExclusive);
                    }
                }

                return spannableString;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                SpannableString spendableString = new SpannableString(text);
                return spendableString;
            }
        }

        private List<StTools.XAutoLinkItem> MatchedRanges(ICharSequence text)
        {
            try
            {
                List<StTools.XAutoLinkItem> autoLinkItems = new List<StTools.XAutoLinkItem>();

                switch (AutoLinkModes)
                {
                    case null:
                        Init(Context);
                        //throw new NullPointerException("Please add at least one mode");
                        break;
                }

                foreach (StTools.XAutoLinkMode anAutoLinkMode in AutoLinkModes)
                {
                    Console.WriteLine("Run foreach MatchedRanges => 175");
                    string regex = StTools.XUtils.GetRegexByAutoLinkMode(anAutoLinkMode, CustomRegex);
                   
                    switch (regex.Length)
                    {
                        case <= 0:
                            continue;
                    }
                    
                    Pattern pattern = Pattern.Compile(regex);
                    Matcher matcher = pattern.Matcher(text);
                   
                    switch (anAutoLinkMode)
                    {
                        case StTools.XAutoLinkMode.ModePhone:
                        {
                            while (matcher.Find())
                            {
                                Console.WriteLine("Run while MatchedRanges => 186");
                                StTools.XAutoLinkItem ss = new StTools.XAutoLinkItem(matcher.Start(), matcher.End(), matcher.Group(), anAutoLinkMode, UserId);

                                if (matcher.Group().Length > MinPhoneNumberLength)
                                {
                                    autoLinkItems.Add(ss);
                                }
                            }

                            break;
                        }
                        default:
                        {
                            while (matcher.Find())
                            {
                                Console.WriteLine("Run while MatchedRanges => 199");
                                autoLinkItems.Add(new StTools.XAutoLinkItem(matcher.Start(), matcher.End(), matcher.Group(), anAutoLinkMode, UserId));
                            }

                            break;
                        }
                    }
                }

                return autoLinkItems;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new List<StTools.XAutoLinkItem>();
            }
        }

        private Color GetColorByMode(StTools.XAutoLinkMode autoLinkMode)
        {
            try
            {
                return autoLinkMode switch
                {
                    StTools.XAutoLinkMode.ModeHashTag => HashtagModeColor,
                    StTools.XAutoLinkMode.ModeMention => MentionModeColor,
                    StTools.XAutoLinkMode.ModePhone => PhoneModeColor,
                    StTools.XAutoLinkMode.ModeEmail => EmailModeColor,
                    StTools.XAutoLinkMode.ModeUrl => UrlModeColor,
                    _ => DefaultColor
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return DefaultColor;
            }
        }

        public void SetMentionModeColor(Color mentionModeColor)
        {
            try
            {
                MentionModeColor = mentionModeColor;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetHashtagModeColor(Color hashtagModeColor)
        {
            try
            {
                HashtagModeColor = hashtagModeColor;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetUrlModeColor(Color urlModeColor)
        {
            try
            {
                UrlModeColor = urlModeColor;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetPhoneModeColor(Color phoneModeColor)
        {
            try
            {
                PhoneModeColor = phoneModeColor;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetEmailModeColor(Color emailModeColor)
        {
            try
            {
                EmailModeColor = emailModeColor;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetCustomModeColor(Color customModeColor)
        {
            try
            {
                CustomModeColor = customModeColor;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetSelectedStateColor(Color defaultSelectedColor)
        {
            try
            {
                DefaultSelectedColor = defaultSelectedColor;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void AddAutoLinkMode(StTools.XAutoLinkMode[] autoLinkModes)
        {
            try
            {
                AutoLinkModes = autoLinkModes;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetBoldAutoLinkModes(List<StTools.XAutoLinkMode> autoLinkModes)
        {
            try
            {
                MBoldAutoLinkModes = new List<StTools.XAutoLinkMode>();

                foreach (StTools.XAutoLinkMode autoLinkMode in autoLinkModes)
                {
                    MBoldAutoLinkModes.Add(autoLinkMode);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetCustomRegex(string regex)
        {
            try
            {
                CustomRegex = regex;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetAutoLinkOnClickListener(StTools.IXAutoLinkOnClickListener autoLinkOnClickListener, Dictionary<string, string> userId)
        {
            try
            {
                AutoLinkOnClickListener = autoLinkOnClickListener;
                UserId = userId;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void EnableUnderLine()
        {
            IsUnderLineEnabled = true;
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            try
            {
                switch (Build.VERSION.SdkInt)
                {
                    case >= (BuildVersionCodes)16:
                    {
                        StaticLayout layout = null!;
                        Field field = null!;

                        Class klass = Class.FromType(typeof(DynamicLayout));

                        try
                        {
                            Field insetsDirtyField = klass.GetDeclaredField("sStaticLayout");

                            insetsDirtyField.Accessible = true;
                            layout = (StaticLayout)insetsDirtyField.Get(klass);

                        }
                        catch (NoSuchFieldException ex)
                        {
                            Methods.DisplayReportResultTrack(ex);
                        }
                        catch (IllegalAccessException ex)
                        {
                            Methods.DisplayReportResultTrack(ex);
                        }

                        if (layout != null)
                        {
                            try
                            {
                                //Field insetsDirtyField = klass.GetDeclaredField("sStaticLayout");

                                field = layout.Class.GetDeclaredField("mMaximumVisibleLineCount");
                                field.Accessible = true;
                                field.SetInt(layout, MaxLines);
                            }
                            catch (NoSuchFieldException e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                            catch (IllegalAccessException e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        }

                        base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

                        if (layout != null && field != null)
                        {
                            try
                            {
                                field.SetInt(layout, Integer.MaxValue);
                            }
                            catch (IllegalAccessException e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        }

                        break;
                    }
                    default:
                        base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}