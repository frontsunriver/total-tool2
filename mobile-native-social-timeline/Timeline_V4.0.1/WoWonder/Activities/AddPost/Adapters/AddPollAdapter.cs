using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Snackbar;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Posts;

namespace WoWonder.Activities.AddPost.Adapters
{ 
    public class AddPollAdapter : RecyclerView.Adapter
    {
        private readonly Activity ActivityContext;
        public readonly ObservableCollection<PollAnswers> AnswersList = new ObservableCollection<PollAnswers>();

        public AddPollAdapter(Activity context)
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
         
        public override int ItemCount => AnswersList?.Count ?? 0;

        public event EventHandler<AddPollAdapterClickEventArgs> ItemClick;
        public event EventHandler<AddPollAdapterClickEventArgs> ItemLongClick;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_AddPoll
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_AddPoll, parent, false);
                var vh = new AddPollAdapterViewHolder(itemView, Click, CloseClickListener);
                return vh;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder)
                {
                    case AddPollAdapterViewHolder holder:
                    {
                        var itemcount = position + 1;
                        holder.Number.Text = itemcount.ToString(); 
                        holder.Input.Hint = ActivityContext.GetText(Resource.String.Lbl2_Answer) + " " + itemcount;
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

 
        public PollAnswers GetItem(int position)
        {
            return AnswersList[position];
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

        private void Click(AddPollAdapterClickEventArgs args)
        {
            try
            {
                var item = AnswersList[args.Position];
                item.Answer = args.Text;
                args.Input.RequestFocus();
                ItemClick?.Invoke(this, args);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
            
        }

        private void CloseClickListener(AddPollAdapterClickEventArgs args)
        {
            switch (AnswersList.Count)
            {
                case > 2:
                {
                    var item = AnswersList[args.Position];
                    AnswersList.Remove(item);
                    NotifyDataSetChanged();
                    ItemLongClick?.Invoke(this, args);
                    break;
                }
                default:
                {
                    Snackbar mySnackbar = Snackbar.Make(args.View, ActivityContext.GetText(Resource.String.Lbl2_PollsLimitTwo), BaseTransientBottomBar.LengthShort);
                    mySnackbar.Show();
                    break;
                }
            }
        }
    }

    public class AddPollAdapterViewHolder : RecyclerView.ViewHolder
    {
       
        public AddPollAdapterViewHolder(View itemView, Action<AddPollAdapterClickEventArgs> clickListener,  Action<AddPollAdapterClickEventArgs> closeClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView; 
                Number = (TextView)MainView.FindViewById(Resource.Id.number);
                Input = (EditText)MainView.FindViewById(Resource.Id.text_input);
                CloseButton = (Button)MainView.FindViewById(Resource.Id.Close);
                 
                Methods.SetColorEditText(Input, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Typeface font = Typeface.CreateFromAsset(Application.Context.Resources?.Assets, "ionicons.ttf");
                if (CloseButton != null)
                {
                    CloseButton.SetTypeface(font, TypefaceStyle.Normal);
                    CloseButton.Text = IonIconsFonts.Close;

                    //Create an Event
                    if (Input != null)
                    {
                        Input.AfterTextChanged += (sender, e) => clickListener(new AddPollAdapterClickEventArgs {View = itemView, Position = AdapterPosition, Text = Input.Text, Input = Input});
                        CloseButton.Click += (sender, e) => closeClickListener(new AddPollAdapterClickEventArgs {View = itemView, Position = AdapterPosition, Text = Input.Text});
                    }
                }

                //itemView.Click += (sender, e) => clickListener(new AddPollAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        #region Variables Basic

        public View MainView { get; private set; }
        public TextView Number { get; private set; }
        public EditText Input { get; private set; }
        public Button CloseButton { get; private set; }

        #endregion
    }

    public class AddPollAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public string Text { get; set; }
        public EditText Input { get; set; }
    }
}