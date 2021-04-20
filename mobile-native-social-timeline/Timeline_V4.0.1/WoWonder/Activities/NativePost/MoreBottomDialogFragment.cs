using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using Newtonsoft.Json;
using System;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Posts;

namespace WoWonder.Activities.NativePost
{
    public class MoreBottomDialogFragment : BottomSheetDialogFragment, View.IOnClickListener
    {
        
        private LinearLayout LlSavePost, LlCopyText, LlCopyLink, LlReportPost, LlEditPost, LlBoostPost, LlDisableComments, LlDeletePost;
        private TextView TvSavePost, TvCopyText, TvCopyLink, TvReportPost, TvEditPost, TvBoostPost, TvDisableComments, TvDeletePost;
        private string Savepost, Copytext, Reportpost, Editpost, Boostpost, Disablecomments;
        private PostDataObject DataPost;
        private PostModelType TypePost;
        private Activity MainContext;

        public string TypeDialog { get; private set; }

        private ITemClickListener Listener;

        public MoreBottomDialogFragment(ITemClickListener listener)
        {
            this.Listener = listener;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                Context contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Dark_Base) : new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Base);
                MainContext = Activity;
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);
                View view = localInflater?.Inflate(Resource.Layout.post_more_bottom, container, false);
                return view;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnLowMemory()
        {
            try
            {
                GC.Collect(GC.MaxGeneration);
                base.OnLowMemory();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);

                DataPost = JsonConvert.DeserializeObject<PostDataObject>(Arguments?.GetString("ItemData") ?? "");
                TypePost = JsonConvert.DeserializeObject<PostModelType>(Arguments?.GetString("TypePost") ?? "");

                Savepost = JsonConvert.DeserializeObject<string>(Arguments?.GetString("savePost") ?? "");
                Copytext = JsonConvert.DeserializeObject<string>(Arguments?.GetString("copyText") ?? "");
                Reportpost = JsonConvert.DeserializeObject<string>(Arguments?.GetString("copyLink") ?? "");
                Editpost = JsonConvert.DeserializeObject<string>(Arguments?.GetString("postType") ?? "");
                Boostpost = JsonConvert.DeserializeObject<string>(Arguments?.GetString("boostPost") ?? "");
                Disablecomments = JsonConvert.DeserializeObject<string>(Arguments?.GetString("commentStatus") ?? "");

                InitComponent(view);
                AddOrRemoveEvent(true);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void InitComponent(View view)
        {
            try
            {
                LlSavePost = view.FindViewById<LinearLayout>(Resource.Id.more_save_post);
                LlCopyText = view.FindViewById<LinearLayout>(Resource.Id.more_copy_text);
                LlCopyLink = view.FindViewById<LinearLayout>(Resource.Id.more_copy_link);
                LlReportPost = view.FindViewById<LinearLayout>(Resource.Id.more_report_post);
                LlEditPost = view.FindViewById<LinearLayout>(Resource.Id.more_edit_post);
                LlBoostPost = view.FindViewById<LinearLayout>(Resource.Id.more_boost_post);
                LlDisableComments = view.FindViewById<LinearLayout>(Resource.Id.more_disable_comment);
                LlDeletePost = view.FindViewById<LinearLayout>(Resource.Id.more_delete_post);

                //
                if (Editpost == null)
                    LlEditPost.Visibility = ViewStates.Gone;
                else
                    LlEditPost.SetOnClickListener(this);

                //
                if (Boostpost == null)
                    LlBoostPost.Visibility = ViewStates.Gone;
                else
                    LlBoostPost.SetOnClickListener(this);

                //
                if (Disablecomments == null)
                    LlDisableComments.Visibility = ViewStates.Gone;
                else
                    LlDisableComments.SetOnClickListener(this);

                // 
                if (DataPost.Publisher.UserId == UserDetails.UserId)
                    LlDeletePost.SetOnClickListener(this);
                else
                    LlDeletePost.Visibility = ViewStates.Gone;

                LlSavePost.SetOnClickListener(this);
                LlCopyText.SetOnClickListener(this);
                LlCopyLink.SetOnClickListener(this);
                LlReportPost.SetOnClickListener(this);

                TvSavePost = view.FindViewById<TextView>(Resource.Id.tv_save_post);
                TvCopyText = view.FindViewById<TextView>(Resource.Id.tv_copy_text);
                TvCopyLink = view.FindViewById<TextView>(Resource.Id.tv_copy_link);
                TvReportPost = view.FindViewById<TextView>(Resource.Id.tv_report_post);
                TvEditPost = view.FindViewById<TextView>(Resource.Id.tv_edit_post);
                TvBoostPost = view.FindViewById<TextView>(Resource.Id.tv_boost_post);
                TvDisableComments = view.FindViewById<TextView>(Resource.Id.tv_disable_comment);
                TvDeletePost = view.FindViewById<TextView>(Resource.Id.tv_delete_post);

                SetNames();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void SetNames()
        {
            if (DataPost.IsPostReported == true)
                LlReportPost.Visibility = ViewStates.Gone;

            TvSavePost.Text = Savepost;

            if (Copytext.Length == 0)
                LlCopyText.Visibility = ViewStates.Gone;
            else
                TvCopyText.Text = Copytext;

            TvReportPost.Text = Reportpost;
            TvEditPost.Text = Editpost;
            TvBoostPost.Text = Boostpost;
            TvDisableComments.Text = Disablecomments;
        }

        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnClick(View v)
        {
            string item;
            switch (v.Id)
            {
                case Resource.Id.more_save_post:
                    item = TvSavePost.Text;
                    Listener.OnItemClick(item);
                    break;
                case Resource.Id.more_copy_text:
                    item = TvCopyText.Text;
                    Listener.OnItemClick(item);
                    break;
                case Resource.Id.more_copy_link:
                    item = TvCopyLink.Text;
                    Listener.OnItemClick(item);
                    break;
                case Resource.Id.more_report_post:
                    item = TvReportPost.Text;
                    Listener.OnItemClick(item);
                    break;
                case Resource.Id.more_edit_post:
                    item = TvEditPost.Text;
                    Listener.OnItemClick(item);
                    break;
                case Resource.Id.more_boost_post:
                    item = TvBoostPost.Text;
                    Listener.OnItemClick(item);
                    break;
                case Resource.Id.more_disable_comment:
                    item = TvDisableComments.Text;
                    Listener.OnItemClick(item);
                    break;
                case Resource.Id.more_delete_post:
                    item = TvDeletePost.Text;
                    Listener.OnItemClick(item);
                    break;
                default:
                    break;
            }
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);

            if( context.GetType() == typeof(ITemClickListener) )
            {
                Listener = context as ITemClickListener;
            }

        }

        public override void OnDetach()
        {
            base.OnDetach();
            Listener = null;
        }

        public interface ITemClickListener
        {
            void OnItemClick(string item);
        }


    }
}