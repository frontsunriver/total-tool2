using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime; 
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Java.Lang;
using Java.Util.Regex;
using Newtonsoft.Json;
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using Exception = System.Exception;

namespace WoWonder.Helpers.Fonts
{
    public class TextViewWithImages : AppCompatTextView
    {
        
        protected TextViewWithImages(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TextViewWithImages(Context context) : base(context)
        {
        }

        public TextViewWithImages(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public TextViewWithImages(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public override void SetText(ICharSequence text, BufferType type)
        {
            try
            {
                //SpannableString s = GetTextWithImages(Context, new Java.Lang.String(text.ToArray(), 0, text.Count()));
                base.SetText(text, BufferType.Spannable);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static UserDataObject Publisher;
        public virtual void SetText(ICharSequence text)
        {
            try
            {
                SetText(text, BufferType.Spannable);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static SpannableString GetTextWithImages(PostDataObject item, Context context, ICharSequence text)
        {
            try
            {
                SpannableString spendable = new SpannableString(text);
                spendable = AddImages(item, context, spendable);
                return spendable;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }
         
        private static SpannableString AddImages(PostDataObject item, Context context, SpannableString spendable)
        { 
            try
            {   //Regex pattern that looks for embedded images of the format: [img src=imageName/]
                //exp. This [img src=imageName/] is an icon.

                Pattern refImg = Pattern.Compile("\\Q[img src=\\E([a-zA-Z0-9_]+?)\\Q/]\\E");

                //bool hasChanges = false;

                Matcher matcher = refImg.Matcher(spendable);

                while (matcher.Find())
                {
                    bool set = true;
                    foreach (var span in spendable.GetSpans(matcher.Start(), matcher.End(), Class.FromType(typeof(ImageSpan))))
                    {
                        if (spendable.GetSpanStart(span) >= matcher.Start() && spendable.GetSpanEnd(span) <= matcher.End())
                        {
                            spendable.RemoveSpan(span);
                        }
                        else
                        {
                            set = false;
                            break;
                        }
                    }

                    switch (set)
                    {
                        case true:
                        {
                            string resName = spendable.SubSequence(matcher.Start(1), matcher.End(1))?.Trim();
                            int id = context.Resources.GetIdentifier(resName, "drawable", context.PackageName);

                            var d = ContextCompat.GetDrawable(context, id);
                            if (d != null)
                            {
                                d.SetBounds(0, 0, d.IntrinsicWidth, d.IntrinsicHeight);
                                spendable.SetSpan(new ImageSpan(d, SpanAlign.Baseline), matcher.Start(), matcher.End(), SpanTypes.ExclusiveExclusive);
                            }
                            else
                                spendable.SetSpan(new ImageSpan(context, id, SpanAlign.Baseline), matcher.Start(), matcher.End(), SpanTypes.ExclusiveExclusive);
                         
                            //hasChanges = true;
                            break;
                        }
                    }
                }

                var username = WoWonderTools.GetNameFinal(Publisher);
                SetTextStyle(spendable, username, TypefaceStyle.Bold);
                 
                if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_ChangedProfileCover)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_ChangedProfileCover), "#888888");
                } 
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_ChangedProfilePicture)))
                {
                    SetTextColor(spendable, context.GetText(Resource.String.Lbl_ChangedProfilePicture), "#888888"); 
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_WasLive))|| spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_IsLiveNow)))
                {
                    SetTextColor(spendable, context.GetText(Resource.String.Lbl_IsLiveNow), "#888888");
                    SetTextColor(spendable, context.GetText(Resource.String.Lbl_WasLive), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_IsListeningTo)))
                {
                    SetTextColor(spendable, context.GetText(Resource.String.Lbl_IsListeningTo) + " " + item.PostTraveling, "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_IsPlaying)))
                {
                    SetTextColor(spendable, context.GetText(Resource.String.Lbl_IsPlaying) + " " + item.PostPlaying, "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_IsTravelingTo)))
                {
                    SetTextColor(spendable, context.GetText(Resource.String.Lbl_IsTravelingTo) + " " + item.PostTraveling, "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_IsWatching)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_IsWatching) + " " + item.PostWatching, "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_AddedNewProductForSell)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_AddedNewProductForSell), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_CreatedNewArticle)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_CreatedNewArticle), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_CreatedNewEvent)) || item.Event?.EventClass != null && item.SharedInfo.SharedInfoClass == null)
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_CreatedNewEvent), "#888888");
                    SetTextColor(spendable , Methods.FunString.DecodeString(item.Event?.EventClass.Name), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_AddedNewPhotosTo)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_AddedNewPhotosTo) + " " + Methods.FunString.DecodeString(item.AlbumName), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_CreatedNewFund)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_CreatedNewFund), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_DonatedRequestFund)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_DonatedRequestFund), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_OfferPostAdded)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_OfferPostAdded), "#888888");
                }
                else if (spendable.ToString()!.Contains(context.GetText(Resource.String.Lbl_SharedPost)))
                {
                    SetTextColor(spendable ,context.GetText(Resource.String.Lbl_SharedPost), "#888888");
                }
                else switch (string.IsNullOrEmpty(item.PostMap))
                {
                    case false when spendable.ToString()!.Contains(item.PostMap.Replace("/", "")):
                        SetTextColor(spendable, item.PostMap.Replace("/", ""), "#888888");
                        break;
                }
                return spendable;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return spendable;
            }
        }

        private static void SetTextColor(SpannableString spendable , string texts, string color, float proportion = 0.9f)
        {
            try
            {
                string content = spendable.ToString(); 
                if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
                    return;
                 
                var indexFrom = content.IndexOf(texts, StringComparison.Ordinal);
                indexFrom = indexFrom switch
                {
                    <= -1 => 0,
                    _ => indexFrom
                };

                var indexLast = indexFrom + texts.Length;
                indexLast = indexLast switch
                {
                    <= -1 => 0,
                    _ => indexLast
                };

                spendable.SetSpan(new ForegroundColorSpan(Color.ParseColor(color)), indexFrom, indexLast, SpanTypes.ExclusiveExclusive);
                spendable.SetSpan(new RelativeSizeSpan(proportion), indexFrom, indexLast, SpanTypes.ExclusiveExclusive);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            } 
        }

        private static void SetTextStyle(SpannableString spendable, string texts, TypefaceStyle style)
        {
            try
            {
                string content = spendable.ToString();
                if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
                    return;

                var indexFrom = content.IndexOf(texts, StringComparison.Ordinal);
                indexFrom = indexFrom switch
                {
                    <= -1 => 0,
                    _ => indexFrom
                };

                var indexLast = indexFrom + texts.Length;
                indexLast = indexLast switch
                {
                    <= -1 => 0,
                    _ => indexLast
                };

                spendable.SetSpan(new StyleSpan(style), indexFrom, indexLast, SpanTypes.ExclusiveExclusive);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        public SpannableString SetClick(TextViewWithImages textViewWithImages , PostDataObject item, SpannableString spendable, AdapterHolders.PostTopSectionViewHolder viewHolder)
        {
            try
            {
                spendable = MakeTextClickAble(textViewWithImages, viewHolder, spendable, WoWonderTools.GetNameFinal(item.Publisher), "UserIdPost");
                 
                if (item.GroupRecipientExists != null && item.GroupRecipientExists.Value && !string.IsNullOrEmpty(item.GroupRecipient.Name) && spendable.ToString()!.Contains(item.GroupRecipient.Name))
                {
                    SetTextColor(spendable, item.GroupRecipient.Name, "#888888");
                    spendable = MakeTextClickAble(textViewWithImages, viewHolder, spendable, item.GroupRecipient.Name, "group");
                }
                else if (item.RecipientExists != null && item.RecipientExists.Value && item.Recipient.RecipientClass != null && spendable.ToString()!.Contains(WoWonderTools.GetNameFinal(item.Recipient.RecipientClass)))
                {
                    var name = WoWonderTools.GetNameFinal(item.Recipient.RecipientClass);
                    SetTextColor(spendable, name, "#888888");
                    spendable = MakeTextClickAble(textViewWithImages, viewHolder, spendable, name, "user");
                }
                return spendable;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return spendable;
            }
        }

        private static SpannableString MakeTextClickAble(TextViewWithImages textView, AdapterHolders.PostTopSectionViewHolder viewHolder, SpannableString spendable, string texts, string type)
        {
            try
            {
                string content = spendable.ToString();
                if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
                    return spendable;

                var indexFrom = content.IndexOf(texts, StringComparison.Ordinal);
                indexFrom = indexFrom switch
                {
                    <= -1 => 0,
                    _ => indexFrom
                };

                var indexLast = indexFrom + texts.Length;
                indexLast = indexLast switch
                {
                    <= -1 => 0,
                    _ => indexLast
                };

                Console.WriteLine(indexFrom);
                spendable.SetSpan(new ClickSpanClass(type, viewHolder), indexFrom, indexLast, SpanTypes.ExclusiveExclusive);

                textView.MovementMethod = LinkMovementMethod.Instance;

                return spendable;
            } 
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return spendable;
            }
        }
         
        private class ClickSpanClass : ClickableSpan
        {
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;
            private readonly AdapterHolders.PostTopSectionViewHolder ViewHolder;
            private readonly string Type;

            public ClickSpanClass(string type, AdapterHolders.PostTopSectionViewHolder viewHolder)
            {
                try
                {
                    Type = type;
                    ViewHolder = viewHolder;
                    PostAdapter = viewHolder.PostAdapter;
                    PostClickListener = viewHolder.PostClickListener;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnClick(View widget)
            {
                try
                {
                    var item = PostAdapter.ListDiffer[ViewHolder.AdapterPosition]?.PostData;
                    switch (Type)
                    {
                        case "user":
                            WoWonderTools.OpenProfile(PostAdapter.ActivityContext, item.RecipientId, null);
                            break;
                        case "group":
                        {
                            var intent = new Intent(PostAdapter.ActivityContext, typeof(GroupProfileActivity));
                            intent.PutExtra("GroupObject", JsonConvert.SerializeObject(item.GroupRecipient));
                            intent.PutExtra("GroupId", item.GroupId);
                            PostAdapter.ActivityContext.StartActivity(intent);
                            break;
                        }
                        default:
                            PostClickListener.ProfilePostClick(new ProfileClickEventArgs { NewsFeedClass = item, Position = ViewHolder.AdapterPosition, View = ViewHolder.MainView }, "NewsFeedClass", "Username");
                            break;
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
                    ds.Color = Color.ParseColor(AppSettings.SetTabDarkTheme ? "#ffffff" : "#444444");
                    ds.BgColor = Color.Transparent;
                    ds.UnderlineText = false;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }

            }
        } 
    }
}