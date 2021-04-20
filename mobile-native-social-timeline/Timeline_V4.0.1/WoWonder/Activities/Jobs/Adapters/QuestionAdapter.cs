using System;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;

namespace WoWonder.Activities.Jobs.Adapters
{
    public class QuestionJob 
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string QuestionAnswer { get; set; }
    }

    public class QuestionAdapter : RecyclerView.Adapter 
    {
        public event EventHandler<QuestionAdapterClickEventArgs> ItemClick;
        public event EventHandler<QuestionAdapterClickEventArgs> ItemLongClick;

        private readonly CreateJobActivity ActivityContext;

        public ObservableCollection<QuestionJob> QuestionList = new ObservableCollection<QuestionJob>();

        public QuestionAdapter(CreateJobActivity context)
        {
            try
            {
                ActivityContext = context;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => QuestionList?.Count ?? 0;
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HPage_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewSub_Question_Add, parent, false);
                var vh = new QuestionAdapterViewHolder(itemView, Click, LongClick);
                return vh;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder)
                {
                    case QuestionAdapterViewHolder holder:
                    {
                        var item = QuestionList[position];
                        if (item != null)
                        {
                            holder.TxtQuestion.Text = ActivityContext.GetText(Resource.String.Lbl_Question);
                          
                            switch (item.QuestionType)
                            {
                                case "free_text_question":
                                {
                                    holder.EdtQuestionType.Text = ActivityContext.GetString(Resource.String.Lbl_FreeTextQuestion);
                                    holder.EdtQuestionAnswer.Visibility = ViewStates.Gone;
                                    holder.DescriptionOfAnswer.Visibility = ViewStates.Gone;
                                    item.QuestionAnswer = "";
                                    break;
                                }
                                case "yes_no_question":
                                {
                                    holder.EdtQuestionType.Text = ActivityContext.GetString(Resource.String.Lbl_YesNoQuestion);
                                    holder.EdtQuestionAnswer.Visibility = ViewStates.Gone;
                                    holder.DescriptionOfAnswer.Visibility = ViewStates.Gone;
                                    item.QuestionAnswer = "";
                                    break;
                                }
                                case "multiple_choice_question":
                                {
                                    holder.EdtQuestionType.Text = ActivityContext.GetString(Resource.String.Lbl_MultipleChoiceQuestion);
                                    holder.EdtQuestionAnswer.Visibility = ViewStates.Visible;
                                    holder.DescriptionOfAnswer.Visibility = ViewStates.Visible;
                                    break;
                                }
                            }
                         
                            switch (holder.CloseQuestion.HasOnClickListeners)
                            {
                                case true:
                                    return;
                            }
                            holder.CloseQuestion.Click += (sender, args) =>
                            {
                                try
                                {
                                    //QuestionOneLayout.Visibility = ViewStates.Gone;
                                    //CountQuestion++;
                                    //InflatedQuestionOne = null!;
                                    item.Question = "";
                                    item.QuestionType = "";
                                    item.QuestionAnswer = "";

                                    var index = QuestionList.IndexOf(QuestionList.FirstOrDefault(a => a.Id == item.Id));
                                    if (index != -1)
                                    {
                                        QuestionList.Remove(item);
                                        NotifyItemRemoved(index);
                                    }

                                    ActivityContext.TxtAddQuestion.Text = ActivityContext.GetText(Resource.String.Lbl_AddQuestion) + "(" + ItemCount + ")";
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            };

                            holder.EdtQuestionType.Touch += (sender, args) =>
                            {
                                try
                                {
                                    if (args.Event.Action != MotionEventActions.Down) return;

                                    ActivityContext.OpenDialogSetQuestion(item);
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            };

                            holder.EdtQuestion.TextChanged += (sender, args) =>
                            {
                                try
                                {
                                    item.Question  = args.Text.ToString();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            };

                            holder.EdtQuestionAnswer.TextChanged += (sender, args) =>
                            {
                                try
                                {
                                    item.QuestionAnswer = args.Text.ToString();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            }; 
                        }

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public QuestionJob GetItem(int position)
        {
            return QuestionList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        private void Click(QuestionAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(QuestionAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
         
    }

    public class QuestionAdapterViewHolder : RecyclerView.ViewHolder
    {
        public QuestionAdapterViewHolder(View itemView, Action<QuestionAdapterClickEventArgs> clickListener, Action<QuestionAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;


                TxtQuestion = MainView.FindViewById<TextView>(Resource.Id.QuestionTextView);
                 
                DescriptionOfAnswer = MainView.FindViewById<TextView>(Resource.Id.DescriptionOfAnswerTextView);
                CloseQuestion = MainView.FindViewById<TextView>(Resource.Id.closeIcon); 
                EdtQuestionType = MainView.FindViewById<EditText>(Resource.Id.QuestionTypeEditText); 
                EdtQuestion = MainView.FindViewById<EditText>(Resource.Id.QuestionEditText);
                EdtQuestionAnswer = MainView.FindViewById<EditText>(Resource.Id.QuestionAnswerEditText);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, CloseQuestion, FontAwesomeIcon.TimesCircle);

                Methods.SetColorEditText(EdtQuestionType, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(EdtQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(EdtQuestionAnswer, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(EdtQuestionType);

                //Event  
                itemView.Click += (sender, e) => clickListener(new QuestionAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new QuestionAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

              
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }
         
        public TextView TxtQuestion { get; private set; }
        public TextView DescriptionOfAnswer { get; private set; }
        public TextView CloseQuestion { get; private set; }
        public EditText EdtQuestionType { get; private set; }
        public EditText EdtQuestion { get; private set; } 
        public EditText EdtQuestionAnswer { get; private set; }

        #endregion
    }

    public class QuestionAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}