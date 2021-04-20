using System;
using Android.App;
using Com.Razorpay;
using Org.Json;
using WoWonder.Helpers.Utils;

namespace WoWonder.Payment
{
    public class InitRazorPayPayment
    {
        private readonly Activity ActivityContext;
        private Checkout CheckOut;

        public InitRazorPayPayment(Activity activity)
        {
            try
            {
                ActivityContext = activity;

                // To ensure faster loading of the Checkout form,call this method as early as possible in your checkout flow.
                Checkout.Preload(activity);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //RazorPay
        public void BtnRazorPayOnClick(string price, string payType, string id)
        {
            try
            {
                var init = InitRazorPay();
                switch (init)
                {
                    case false:
                        return;
                }

                Activity activity = ActivityContext;
                JSONObject options = new JSONObject();

                switch (payType)
                {
                    case "Funding":
                        options.Put("name", "Donate to funding");
                        break;
                    case "membership":
                        options.Put("name", "Account upgrade request for type" + id);
                        break;
                    case "AddFunds":
                        options.Put("name", "Add to balance");
                        break;
                }
             
                options.Put("description", "");

                //options.Put("image", "https://demo.wowonder.com/themes/default/img/logo.png");
                //options.Put("order_id", "order_DBJOWzybf0sJbb");//from response of step 3.

                options.Put("theme.color", AppSettings.MainColor);
                options.Put("currency", AppSettings.RazorPayCurrency);

                var priceInt = Convert.ToInt32(price) * 100; 
                options.Put("amount", priceInt.ToString());//pass amount in currency subunits

                options.Put("prefill.email", "");
                options.Put("prefill.contact", "");

                CheckOut?.Open(activity, options);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private bool InitRazorPay()
        {
            try
            {
                //PayerID 
                string keyId = ActivityContext.GetText(Resource.String.razorpay_api_Key);
                //var option = ListUtils.SettingsSiteList;
                //if (option != null)
                //{
                //    keyId = option.RazorpayKeyId;
                //}

                if (string.IsNullOrEmpty(keyId))
                    return false;

                CheckOut = new Checkout();
                CheckOut.SetKeyID(keyId);
                CheckOut.SetImage(Resource.Mipmap.icon);

                return true;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }

        public void StopRazorPay()
        {
            try
            {
                if (CheckOut != null)
                {
                    #pragma warning disable 618
                    CheckOut.OnDestroy();
                    #pragma warning restore 618
                    CheckOut = null!;
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}