using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Widget;
using WoWonder.Helpers.Utils;
using WoWonderClient.InAppBilling;

namespace WoWonder.PaymentGoogle
{
    public class InitInAppBillingPayment : BillingProcessor.IBillingHandler
    {
        private readonly Activity ActivityContext;
        private string PayType, Id;
        public BillingProcessor Handler;
        private List<SkuDetails> Products;
        private bool ReadyToPurchase = false;

        public InitInAppBillingPayment(Activity activity)
        {
            try
            {
                ActivityContext = activity;

                if (!BillingProcessor.IsIabServiceAvailable(activity))
                {
                    Console.WriteLine("In-app billing service is unavailable, please upgrade Android Market/Play to version >= 3.9.16");
                    return;
                }

                SetConnInAppBilling();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region In-App Billing Google

        public void SetConnInAppBilling()
        {
            try
            {
                switch (Handler)
                {
                    case null:
                        Handler = new BillingProcessor(ActivityContext, InAppBillingGoogle.ProductId, this);
                        Handler.Initialize();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void DisconnectInAppBilling()
        {
            try
            {
                Handler?.Release();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void InitInAppBilling(string price, string payType, string id)
        {
            PayType = payType;
            Id = id;

            if (Methods.CheckConnectivity())
            {
                if (!ReadyToPurchase || !Handler.IsInitialized())
                {
                    return;
                }

                try
                {
                    Products = Handler.GetPurchaseListingDetails(InAppBillingGoogle.ListProductSku);
                    switch (Products.Count)
                    {
                        case > 0:
                        {
                            var membershipLifeTime = Products.FirstOrDefault(a => a.ProductId == "membershiplifetime");
                            var membershipMonthly = Products.FirstOrDefault(a => a.ProductId == "membershipmonthly");
                            var membershipWeekly = Products.FirstOrDefault(a => a.ProductId == "membershipweekly");
                            var membershipYearly = Products.FirstOrDefault(a => a.ProductId == "membershipyearly");

                            var donationDefault = Products.FirstOrDefault(a => a.ProductId == "donationdefulte");
                            var donation5 = Products.FirstOrDefault(a => a.ProductId == "donation5") ?? donationDefault;
                            var donation10 = Products.FirstOrDefault(a => a.ProductId == "donation10") ?? donationDefault;
                            var donation15 = Products.FirstOrDefault(a => a.ProductId == "donation15") ?? donationDefault;
                            var donation20 = Products.FirstOrDefault(a => a.ProductId == "donation20") ?? donationDefault;
                            var donation25 = Products.FirstOrDefault(a => a.ProductId == "donation25") ?? donationDefault;
                            var donation30 = Products.FirstOrDefault(a => a.ProductId == "donation30") ?? donationDefault;
                            var donation35 = Products.FirstOrDefault(a => a.ProductId == "donation35") ?? donationDefault;
                            var donation40 = Products.FirstOrDefault(a => a.ProductId == "donation40") ?? donationDefault;
                            var donation45 = Products.FirstOrDefault(a => a.ProductId == "donation45") ?? donationDefault;
                            var donation50 = Products.FirstOrDefault(a => a.ProductId == "donation50") ?? donationDefault;
                            var donation55 = Products.FirstOrDefault(a => a.ProductId == "donation55") ?? donationDefault;
                            var donation60 = Products.FirstOrDefault(a => a.ProductId == "donation60") ?? donationDefault;
                            var donation65 = Products.FirstOrDefault(a => a.ProductId == "donation65") ?? donationDefault;
                            var donation70 = Products.FirstOrDefault(a => a.ProductId == "donation70") ?? donationDefault;
                            var donation75 = Products.FirstOrDefault(a => a.ProductId == "donation75") ?? donationDefault;
                            //var donation80 = Products.FirstOrDefault(a => a.ProductId == "donation80") ?? donationDefault;
                            //var donation85 = Products.FirstOrDefault(a => a.ProductId == "donation85") ?? donationDefault;
                            //var donation90 = Products.FirstOrDefault(a => a.ProductId == "donation90") ?? donationDefault;
                            //var donation95 = Products.FirstOrDefault(a => a.ProductId == "donation95") ?? donationDefault;
                            //var donation100 = Products.FirstOrDefault(a => a.ProductId == "donation100") ?? donationDefault;

                            switch (PayType)
                            {
                                //Weekly
                                case "membership" when Id == "1": // Per Week 
                                    Handler.Purchase(ActivityContext, membershipWeekly?.ProductId);
                                    break;
                                //Monthly
                                case "membership" when Id == "2": // Per Month 
                                    Handler.Purchase(ActivityContext, membershipMonthly?.ProductId);
                                    break;
                                //Yearly
                                case "membership" when Id == "3": // Per Year 
                                    Handler.Purchase(ActivityContext, membershipYearly?.ProductId);
                                    break;
                                case "membership" when Id == "4": // life time 
                                    Handler.Purchase(ActivityContext, membershipLifeTime?.ProductId);
                                    break;
                                case "Funding" when price == "5": // Donation with Amount 5
                                    Handler.Purchase(ActivityContext, donation5?.ProductId);
                                    break;
                                case "Funding" when price == "10":  // Donation with Amount 10
                                    Handler.Purchase(ActivityContext, donation10?.ProductId);
                                    break;
                                case "Funding" when price == "15": // Donation with Amount 15
                                    Handler.Purchase(ActivityContext, donation15?.ProductId);
                                    break;
                                case "Funding" when price == "20": // Donation with Amount 20
                                    Handler.Purchase(ActivityContext, donation20?.ProductId);
                                    break;
                                case "Funding" when price == "25": // Donation with Amount 25
                                    Handler.Purchase(ActivityContext, donation25?.ProductId);
                                    break;
                                case "Funding" when price == "30": // Donation with Amount 30
                                    Handler.Purchase(ActivityContext, donation30?.ProductId);
                                    break;
                                case "Funding" when price == "35": // Donation with Amount 35
                                    Handler.Purchase(ActivityContext, donation35?.ProductId);
                                    break;
                                case "Funding" when price == "40": // Donation with Amount 40
                                    Handler.Purchase(ActivityContext, donation40?.ProductId);
                                    break;
                                case "Funding" when price == "45": // Donation with Amount 45
                                    Handler.Purchase(ActivityContext, donation45?.ProductId);
                                    break;
                                case "Funding" when price == "50": // Donation with Amount 50
                                    Handler.Purchase(ActivityContext, donation50?.ProductId);
                                    break;
                                case "Funding" when price == "55": // Donation with Amount 55
                                    Handler.Purchase(ActivityContext, donation55?.ProductId);
                                    break;
                                case "Funding" when price == "60": // Donation with Amount 60
                                    Handler.Purchase(ActivityContext, donation60?.ProductId);
                                    break;
                                case "Funding" when price == "65": // Donation with Amount 65
                                    Handler.Purchase(ActivityContext, donation65?.ProductId);
                                    break;
                                case "Funding" when price == "70": // Donation with Amount 70
                                    Handler.Purchase(ActivityContext, donation70?.ProductId);
                                    break;
                                case "Funding" when price == "75": // Donation with Amount 75
                                    Handler.Purchase(ActivityContext, donation75?.ProductId);
                                    break;
                                case "Funding" when price == "80": // Donation with Amount 80
                                case "Funding" when price == "85": // Donation with Amount 85
                                case "Funding" when price == "90": // Donation with Amount 90
                                case "Funding" when price == "95": // Donation with Amount 95
                                case "Funding" when price == "100": // Donation with Amount 100
                                case "Funding": // Donation with Amount long 100
                                    Handler.Purchase(ActivityContext, donationDefault?.ProductId);
                                    break;
                            }

                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Something else has gone wrong, log it
                    Console.WriteLine("Issue connecting: " + ex);
                    Toast.MakeText(ActivityContext, "Issue connecting: " + ex, ToastLength.Long)?.Show();
                }
            }
            else
            {
                Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
            }
        }

        #endregion

        public void OnProductPurchased(string productId, TransactionDetails details)
        {
            Console.WriteLine("onProductPurchased: " + productId);
        }

        public void OnPurchaseHistoryRestored()
        {
            Console.WriteLine("onPurchaseHistoryRestored");

            foreach (var sku in Handler.ListOwnedProducts())
                Console.WriteLine("Owned Managed Product: " + sku);

            //foreach (var sku in Handler.ListOwnedSubscriptions())
            //    Console.WriteLine("Owned Subscription: " + sku); 
        }

        public void OnBillingError(int errorCode, Exception error)
        {
            Console.WriteLine("onBillingError: " + errorCode + " " + error.Message);
        }

        public void OnBillingInitialized()
        {
            Console.WriteLine("onBillingInitialized");
            ReadyToPurchase = true;
        }
    }
}