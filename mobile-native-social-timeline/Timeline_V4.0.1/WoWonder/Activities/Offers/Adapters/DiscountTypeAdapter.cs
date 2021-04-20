using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.Offers.Adapters
{
    public class DiscountOffers
    {
        public string DiscountType { get; set; }
        public string DiscountFirst { get; set; }
        public string DiscountSec { get; set; }
        public string DiscountThr { get; set; }
    }

    public class DiscountTypeAdapter : RecyclerView.Adapter
    {
        public event EventHandler<DiscountTypeAdapterClickEventArgs> ItemClick;
        public event EventHandler<DiscountTypeAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<DiscountOffers> DiscountList = new ObservableCollection<DiscountOffers>();

        public DiscountTypeAdapter(Activity context)
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

        public override int ItemCount => DiscountList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HPage_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewSub_DiscountType, parent, false);
                var vh = new DiscountTypeAdapterViewHolder(itemView, Click, LongClick);
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
                    case DiscountTypeAdapterViewHolder holder:
                    {
                        var item = DiscountList[position];
                        if (item != null)
                        {
                            switch (item.DiscountType)
                            {
                                case "discount_percent":
                                {
                                    holder.EdtDiscountFirst.Visibility = ViewStates.Visible;
                                    holder.EdtDiscountSec.Visibility = ViewStates.Gone;
                                    holder.EdtDiscountThr.Visibility = ViewStates.Gone;
                                    holder.LayoutDiscount.Visibility = ViewStates.Gone;

                                    holder.EdtDiscountFirst.Hint = ActivityContext.GetText(Resource.String.Lbl_DiscountPercent);
                                    holder.EdtDiscountFirst.Text = item.DiscountFirst;
                                } break;
                                case "discount_amount":
                                {
                                    holder.EdtDiscountFirst.Visibility = ViewStates.Visible;
                                    holder.EdtDiscountSec.Visibility = ViewStates.Gone;
                                    holder.EdtDiscountThr.Visibility = ViewStates.Gone;
                                    holder.LayoutDiscount.Visibility = ViewStates.Gone;
                                     
                                    holder.EdtDiscountFirst.Hint = ActivityContext.GetText(Resource.String.Lbl_DiscountAmount);
                                    holder.EdtDiscountFirst.Text = item.DiscountFirst;
                                } break;
                                case "buy_get_discount":
                                {
                                    holder.EdtDiscountFirst.Visibility = ViewStates.Visible;
                                    holder.EdtDiscountSec.Visibility = ViewStates.Visible;
                                    holder.EdtDiscountThr.Visibility = ViewStates.Visible;
                                    holder.LayoutDiscount.Visibility = ViewStates.Visible;
                                     
                                    holder.EdtDiscountFirst.Hint = ActivityContext.GetText(Resource.String.Lbl_DiscountAmount);
                                    holder.EdtDiscountSec.Hint = ActivityContext.GetText(Resource.String.Lbl_Buy);
                                    holder.EdtDiscountThr.Hint = ActivityContext.GetText(Resource.String.Lbl_Get);

                                    holder.EdtDiscountFirst.Text = item.DiscountFirst;
                                    holder.EdtDiscountFirst.Text = item.DiscountSec;
                                    holder.EdtDiscountFirst.Text = item.DiscountThr;
                                } break;
                                case "spend_get_off":
                                {
                                    holder.EdtDiscountFirst.Visibility = ViewStates.Gone;
                                    holder.EdtDiscountSec.Visibility = ViewStates.Visible;
                                    holder.EdtDiscountThr.Visibility = ViewStates.Visible;
                                    holder.LayoutDiscount.Visibility = ViewStates.Visible;
                                     
                                    holder.EdtDiscountSec.Hint = ActivityContext.GetText(Resource.String.Lbl_Spend);
                                    holder.EdtDiscountThr.Hint = ActivityContext.GetText(Resource.String.Lbl_AmountOff);

                                    holder.EdtDiscountFirst.Text = item.DiscountFirst;
                                    holder.EdtDiscountFirst.Text = item.DiscountSec;
                                    holder.EdtDiscountFirst.Text = item.DiscountThr;
                                } break;
                                case "free_shipping":
                                {
                                    holder.EdtDiscountFirst.Visibility = ViewStates.Gone;
                                    holder.EdtDiscountSec.Visibility = ViewStates.Gone;
                                    holder.EdtDiscountThr.Visibility = ViewStates.Gone;
                                    holder.LayoutDiscount.Visibility = ViewStates.Gone; 
                                }break;
                            }
                         
                            switch (holder.EdtDiscountFirst.HasOnClickListeners)
                            {
                                case true:
                                    return;
                            }
                            holder.EdtDiscountFirst.TextChanged += (sender, args) =>
                            {
                                try
                                {
                                    item.DiscountFirst = args.Text.ToString();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            };

                            holder.EdtDiscountSec.TextChanged += (sender, args) =>
                            {
                                try
                                {
                                    item.DiscountSec = args.Text.ToString();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            };

                            holder.EdtDiscountThr.TextChanged += (sender, args) =>
                            {
                                try
                                {
                                    item.DiscountThr = args.Text.ToString();
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

        public DiscountOffers GetItem(int position)
        {
            return DiscountList[position];
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

        private void Click(DiscountTypeAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(DiscountTypeAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

    }

    public class DiscountTypeAdapterViewHolder : RecyclerView.ViewHolder
    {
        public DiscountTypeAdapterViewHolder(View itemView, Action<DiscountTypeAdapterClickEventArgs> clickListener, Action<DiscountTypeAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                EdtDiscountFirst = MainView.FindViewById<EditText>(Resource.Id.DiscountFirstEditText);
                EdtDiscountSec = MainView.FindViewById<EditText>(Resource.Id.DiscountSecEditText);
                EdtDiscountThr = MainView.FindViewById<EditText>(Resource.Id.DiscountThrEditText);
                LayoutDiscount = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutDiscount);

                Methods.SetColorEditText(EdtDiscountFirst, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(EdtDiscountSec, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(EdtDiscountThr, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                 
                //Event  
                itemView.Click += (sender, e) => clickListener(new DiscountTypeAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new DiscountTypeAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        public EditText EdtDiscountFirst { get; private set; }
        public EditText EdtDiscountSec { get; private set; }
        public EditText EdtDiscountThr { get; private set; } 
        public LinearLayout LayoutDiscount { get; private set; }

        #endregion
    }

    public class DiscountTypeAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}