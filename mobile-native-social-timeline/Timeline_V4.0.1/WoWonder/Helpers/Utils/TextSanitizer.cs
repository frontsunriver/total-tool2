using System;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.Content; 
using Com.Luseen.Autolinklibrary; 
using WoWonder.Activities.AddPost;
using WoWonder.Activities.AddPost.Service;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.NativePost.Pages;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.UserProfile;
using WoWonder.Helpers.Controller;
using WoWonder.SQLite;

namespace WoWonder.Helpers.Utils
{
    public class TextSanitizer : Java.Lang.Object, MaterialDialog.ISingleButtonCallback
    {
        private readonly AutoLinkTextView AutoLinkTextView;
        private readonly Activity Activity;

        public TextSanitizer(AutoLinkTextView linkTextView, Activity activity, string typePage = "normal")
        {
            try
            {
                AutoLinkTextView = linkTextView;
                Activity = activity;
                switch (typePage)
                {
                    case "AddPost":
                    {
                        switch (AutoLinkTextView.HasOnClickListeners)
                        {
                            case false:
                                AutoLinkTextView.AutoLinkOnClick += AddPostAutoLinkTextViewOnAutoLinkOnClick;
                                break;
                        }

                        break;
                    }
                    default:
                        AutoLinkTextView.AutoLinkOnClick += AutoLinkTextViewOnAutoLinkOnClick;
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void Load(string text)
        {
            try
            {
                AutoLinkTextView.AddAutoLinkMode(AutoLinkMode.ModePhone, AutoLinkMode.ModeEmail, AutoLinkMode.ModeHashtag, AutoLinkMode.ModeUrl, AutoLinkMode.ModeMention, AutoLinkMode.ModeCustom);
                AutoLinkTextView.SetPhoneModeColor(ContextCompat.GetColor(Activity, Resource.Color.AutoLinkText_ModePhone_color));
                AutoLinkTextView.SetEmailModeColor(ContextCompat.GetColor(Activity, Resource.Color.AutoLinkText_ModeEmail_color));
                AutoLinkTextView.SetHashtagModeColor(ContextCompat.GetColor(Activity, Resource.Color.AutoLinkText_ModeHashtag_color));
                AutoLinkTextView.SetUrlModeColor(ContextCompat.GetColor(Activity, Resource.Color.AutoLinkText_ModeUrl_color));
                AutoLinkTextView.SetMentionModeColor(Color.ParseColor(AppSettings.MainColor));
                var textSplit = text.Split('/');
                switch (textSplit.Count())
                {
                    case > 1:
                        AutoLinkTextView.SetCustomModeColor(ContextCompat.GetColor(Activity, Resource.Color.AutoLinkText_ModeUrl_color));
                        AutoLinkTextView.SetCustomRegex(@"\b(" + textSplit.LastOrDefault() + @")\b");
                        break;
                }

                string lastString = text.Replace(" /", " ");
                switch (string.IsNullOrEmpty(lastString))
                {
                    case false:
                        AutoLinkTextView.SetAutoLinkText(lastString);
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void AutoLinkTextViewOnAutoLinkOnClick(object sender, AutoLinkOnClickEventArgs autoLinkOnClickEventArgs)
        {
            try
            {
                var typetext = Methods.FunString.Check_Regex(autoLinkOnClickEventArgs.P1.Replace(" ", ""));
                if (typetext == "Email" || autoLinkOnClickEventArgs.P0 == AutoLinkMode.ModeEmail)
                {
                    Methods.App.SendEmail(Activity, autoLinkOnClickEventArgs.P1.Replace(" ", ""));
                }
                else if (typetext == "Website" || autoLinkOnClickEventArgs.P0 == AutoLinkMode.ModeUrl)
                {
                    string url = autoLinkOnClickEventArgs.P1.Contains("http") switch
                    {
                        false => "http://" + autoLinkOnClickEventArgs.P1.Replace(" ", ""),
                        _ => autoLinkOnClickEventArgs.P1.Replace(" ", "")
                    };

                    //var intent = new Intent(Activity, typeof(LocalWebViewActivity));
                    //intent.PutExtra("URL", url);
                    //intent.PutExtra("Type", url);
                    //Activity.StartActivity(intent);
                    new IntentController(Activity).OpenBrowserFromApp(url);
                }
                else if (typetext == "Hashtag" || autoLinkOnClickEventArgs.P0 == AutoLinkMode.ModeHashtag)
                {
                    var intent = new Intent(Activity, typeof(HashTagPostsActivity));
                    intent.PutExtra("Id", autoLinkOnClickEventArgs.P1.Replace(" ", ""));
                    intent.PutExtra("Tag", autoLinkOnClickEventArgs.P1.Replace(" ", ""));
                    Activity.StartActivity(intent);
                }
                else if (typetext == "Mention" || autoLinkOnClickEventArgs.P0 == AutoLinkMode.ModeMention)
                {
                    var dataUSer = ListUtils.MyProfileList?.FirstOrDefault();
                    string name = autoLinkOnClickEventArgs.P1.Replace("@", "").Replace(" ", "");

                    var sqlEntity = new SqLiteDatabase();
                    var user = sqlEntity.Get_DataOneUser(name);
                    

                    if (user != null)
                    {
                        WoWonderTools.OpenProfile(Activity, user.UserId, user);
                    }
                    else
                    {
                        if (name == dataUSer?.Name || name == dataUSer?.Username)
                        {
                            switch (PostClickListener.OpenMyProfile)
                            {
                                case true:
                                    return;
                                default:
                                {
                                    var intent = new Intent(Activity, typeof(MyProfileActivity));
                                    Activity.StartActivity(intent);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            var intent = new Intent(Activity, typeof(UserProfileActivity));
                            //intent.PutExtra("UserObject", JsonConvert.SerializeObject(item));
                            intent.PutExtra("name", name);
                            Activity.StartActivity(intent);
                        }
                    }
                }
                else if (typetext == "Number" || autoLinkOnClickEventArgs.P0 == AutoLinkMode.ModePhone)
                {
                    Methods.App.SaveContacts(Activity, autoLinkOnClickEventArgs.P1.Replace(" ", ""), "", "2");
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void AddPostAutoLinkTextViewOnAutoLinkOnClick(object sender, AutoLinkOnClickEventArgs e)
        {
            try
            {
                if (e.P0 == AutoLinkMode.ModeEmail)
                {
                    Methods.App.SendEmail(Activity, e.P1.Replace(" ", ""));
                }
                else if (e.P0 == AutoLinkMode.ModeUrl)
                {
                    string url = e.P1.Contains("http") switch
                    {
                        false => "http://" + e.P1.Replace(" ", ""),
                        _ => e.P1.Replace(" ", "")
                    };

                    //var intent = new Intent(Activity, typeof(LocalWebViewActivity));
                    //intent.PutExtra("URL", url);
                    //intent.PutExtra("Type", url);
                    //Activity.StartActivity(intent);
                    new IntentController(Activity).OpenBrowserFromApp(url);
                }
                else if (e.P0 == AutoLinkMode.ModeHashtag)
                {
                    var intent = new Intent(Activity, typeof(HashTagPostsActivity));
                    intent.PutExtra("Id", e.P1.Replace(" ", ""));
                    intent.PutExtra("Tag", e.P1.Replace(" ", ""));
                    Activity.StartActivity(intent);
                }
                else if (e.P0 == AutoLinkMode.ModeMention)
                {
                    var dataUSer = ListUtils.MyProfileList?.FirstOrDefault();
                    string name = e.P1.Replace("@", "").Replace(" ", "");

                    var sqlEntity = new SqLiteDatabase();
                    var user = sqlEntity.Get_DataOneUser(name);
                    

                    if (user != null)
                    {
                        WoWonderTools.OpenProfile(Activity, user.UserId, user);
                    }
                    else
                    {
                        if (name == dataUSer?.Name || name == dataUSer?.Username)
                        {
                            switch (PostClickListener.OpenMyProfile)
                            {
                                case true:
                                    return;
                                default:
                                {
                                    var intent = new Intent(Activity, typeof(MyProfileActivity));
                                    Activity.StartActivity(intent);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            var intent = new Intent(Activity, typeof(UserProfileActivity));
                            //intent.PutExtra("UserObject", JsonConvert.SerializeObject(item));
                            intent.PutExtra("name", name);
                            Activity.StartActivity(intent);
                        }
                    }
                }
                else if (e.P0 == AutoLinkMode.ModePhone)
                {
                    Methods.App.SaveContacts(Activity, e.P1.Replace(" ", ""), "", "2");
                }
                else if (e.P0 == AutoLinkMode.ModeCustom)
                {
                    var dialog = new MaterialDialog.Builder(Activity).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                    dialog.Title(Activity.GetText(Resource.String.Lbl_Location)).TitleColorRes(Resource.Color.primary);
                    dialog.PositiveText(Activity.GetText(Resource.String.Lbl_RemoveLocation)).OnPositive(this);
                    dialog.NeutralText(Activity.GetText(Resource.String.Lbl_ChangeLocation)).OnNeutral(this);
                    dialog.NegativeText(Activity.GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                    //dialog.AlwaysCallSingleChoiceCallback();
                    dialog.Build().Show();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                if (p1 == DialogAction.Positive)
                {
                    ((AddPostActivity)Activity)?.RemoveLocation();
                    ((PostSharingActivity)Activity)?.RemoveLocation();
                }
                else if (p1 == DialogAction.Negative)
                {
                    p0.Dismiss();
                }
                else if (p1 == DialogAction.Neutral)
                {
                    //Open intent Location when the request code of result is 502
                    new IntentController(Activity).OpenIntentLocation();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}