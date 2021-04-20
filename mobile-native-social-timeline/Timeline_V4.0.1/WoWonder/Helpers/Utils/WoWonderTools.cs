using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.UserProfile;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Group;
using WoWonderClient.Classes.Jobs;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Path = System.IO.Path;

namespace WoWonder.Helpers.Utils
{
    public static class WoWonderTools  
    {
        public static string GetNameFinal(UserDataObject dataUser)
        {
            try
            {
                return dataUser switch
                {
                    null => "",
                    _ => string.IsNullOrEmpty(dataUser.Name) switch
                    {
                        false when !string.IsNullOrWhiteSpace(dataUser.Name) => Methods.FunString.DecodeString(
                            dataUser.Name),
                        _ => string.IsNullOrEmpty(dataUser.Username) switch
                        {
                            false when !string.IsNullOrWhiteSpace(dataUser.Username) => Methods.FunString.DecodeString(
                                dataUser.Username),
                            _ => Methods.FunString.DecodeString(dataUser.Username)
                        }
                    }
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return Methods.FunString.DecodeString(dataUser?.Username);
            }
        }

        public static string GetAboutFinal(UserDataObject dataUser)
        {
            try
            {
                return dataUser switch
                {
                    null => Application.Context.Resources?.GetString(Resource.String.Lbl_DefaultAbout) + " " +
                            AppSettings.ApplicationName,
                    _ => string.IsNullOrEmpty(dataUser.About) switch
                    {
                        false when !string.IsNullOrWhiteSpace(dataUser.About) => Methods.FunString.DecodeString(
                            dataUser.About),
                        _ => Application.Context.Resources?.GetString(Resource.String.Lbl_DefaultAbout) + " " +
                             AppSettings.ApplicationName
                    }
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return Application.Context.Resources?.GetString(Resource.String.Lbl_DefaultAbout) + " " + AppSettings.ApplicationName;
            }
        }

        public static string GetIdCurrency(string currency)
        {
            try
            { 
                if (ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList != null)
                {
                    string currencyIcon = "0";
                    switch (currency?.ToUpper())
                    {
                        case "USD":
                        case "$":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("USD") || a.Contains("$")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("USD") || a.Value.Contains("$"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "Jpy":
                        case "¥":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("Jpy") || a.Contains("¥")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("Jpy") || a.Value.Contains("¥"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "EUR":
                        case "€":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("EUR") || a.Contains("€")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("EUR") || a.Value.Contains("€"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "TRY":
                        case "₺":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("TRY") || a.Contains("₺")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("TRY") || a.Value.Contains("₺"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "GBP":
                        case "£":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("GBP") || a.Contains("£")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("GBP") || a.Value.Contains("£"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "RUB":
                        case "₽":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("RUB") || a.Contains("₽")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("RUB") || a.Value.Contains("₽"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "PLN":
                        case "zł":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("PLN") || a.Contains("zł")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("PLN") || a.Value.Contains("zł"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "ILS":
                        case "₪":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("ILS") || a.Contains("₪")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("ILS") || a.Value.Contains("₪"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "BRL":
                        case "R$":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("BRL") || a.Contains("R$")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("BRL") || a.Value.Contains("R$"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        case "INR":
                        case "₹":
                            switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                            {
                                case > 0:
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList.FindIndex(a => a.Contains("INR") || a.Contains("₹")).ToString();
                                    break;
                                default:
                                {
                                    currencyIcon = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                    {
                                        > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                            ?.FirstOrDefault(a => a.Value.Contains("INR") || a.Value.Contains("₹"))
                                            .Key,
                                        _ => currencyIcon
                                    };

                                    break;
                                }
                            }
                            break;
                        default:
                            currencyIcon = "0";
                            break;
                    }

                    return currencyIcon;
                }

                return "0";
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return "0";
            }
        }
        
        public static (string, string) GetCurrency(string idCurrency)
        {
            try
            {
                switch (AppSettings.CurrencyStatic)
                {
                    case true:
                        return (AppSettings.CurrencyCodeStatic, AppSettings.CurrencyIconStatic);
                }

                string currency = AppSettings.CurrencyCodeStatic;
                bool success = int.TryParse(idCurrency, out var number);
                switch (success)
                {
                    case true:
                    {
                        Console.WriteLine("Converted '{0}' to {1}.", idCurrency, number);
                        switch (ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count)
                        {
                            case > 0 when ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList?.Count >= number:
                                currency = ListUtils.SettingsSiteList?.CurrencyArray.CurrencyList[number] ?? AppSettings.CurrencyCodeStatic;
                                break;
                            default:
                            {
                                currency = ListUtils.SettingsSiteList?.CurrencyArray.StringMap?.Count switch
                                {
                                    > 0 => ListUtils.SettingsSiteList?.CurrencyArray.StringMap
                                        ?.FirstOrDefault(a => a.Key == number.ToString())
                                        .Value ?? AppSettings.CurrencyCodeStatic,
                                    _ => currency
                                };

                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Console.WriteLine("Attempted conversion of '{0}' failed.", idCurrency ?? "<null>");
                        currency = idCurrency;
                        break;
                }

                if (ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList != null)
                {
                    string currencyIcon = currency?.ToUpper() switch
                    {
                        "USD" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Usd)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Usd ?? "$"
                            : "$",
                        "Jpy" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Jpy)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Jpy ?? "¥"
                            : "¥",
                        "EUR" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Eur)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Eur ?? "€"
                            : "€",
                        "TRY" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Try)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Try ?? "₺"
                            : "₺",
                        "GBP" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Gbp)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Gbp ?? "£"
                            : "£",
                        "RUB" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Rub)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Rub ?? "₽"
                            : "₽",
                        "PLN" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Pln)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Pln ?? "zł"
                            : "zł",
                        "ILS" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Ils)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Ils ?? "₪"
                            : "₪",
                        "BRL" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Brl)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Brl ?? "R$"
                            : "R$",
                        "INR" => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Inr)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Inr ?? "₹"
                            : "₹",
                        _ => !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Usd)
                            ? ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Usd ?? "$"
                            : "$"
                    };

                    return (currency, currencyIcon);
                }

                return (AppSettings.CurrencyCodeStatic, AppSettings.CurrencyIconStatic);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return (AppSettings.CurrencyCodeStatic, AppSettings.CurrencyIconStatic);
            }
        }

        public static List<string> GetCurrencySymbolList()
        {
            try
            {
                var arrayAdapter = new List<string>();

                switch (AppSettings.CurrencyStatic)
                {
                    case true:
                        arrayAdapter.Add(AppSettings.CurrencyIconStatic);
                        return arrayAdapter;
                }

                if (ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList != null)
                {
                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Usd))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Usd ?? "$");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Jpy))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Jpy ?? "¥");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Eur))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Eur ?? "€");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Try))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Try ?? "₺");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Gbp))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Gbp ?? "£");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Rub))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Rub ?? "₽");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Pln))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Pln ?? "zł");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Ils))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Ils ?? "₪");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Brl))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Brl ?? "R$");
                            break;
                    }

                    switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Inr))
                    {
                        case false:
                            arrayAdapter.Add(ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList.Inr ?? "₹");
                            break;
                    }
                }

                return arrayAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new List<string>(); 
            }
        }

        public static void OpenProfile(Activity activity, string userId, UserDataObject item , string namePage)
        {
            try
            {
                if (userId != UserDetails.UserId)
                {
                    var intent = new Intent(activity, typeof(UserProfileActivity));
                    if (item != null) intent.PutExtra("UserObject", JsonConvert.SerializeObject(item));
                    intent.PutExtra("UserId", userId);
                    intent.PutExtra("NamePage", namePage);
                    activity.StartActivity(intent);
                }
                else
                {
                    var intent = new Intent(activity, typeof(MyProfileActivity));
                    activity.StartActivity(intent);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static void OpenProfile(Activity activity, string userId, UserDataObject item)
        {
            try
            {
                if (userId != UserDetails.UserId)
                {
                    try
                    {
                        if (UserProfileActivity.SUserId != userId)
                        {
                            MainApplication.GetInstance()?.NavigateTo(activity, typeof(UserProfileActivity), item); 
                        } 
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                        var intent = new Intent(activity, typeof(UserProfileActivity));
                        if (item != null)
                            intent.PutExtra("UserObject", JsonConvert.SerializeObject(item));
                        intent.PutExtra("UserId", userId); 
                        activity.StartActivity(intent);
                    }
                }
                else
                {
                    switch (PostClickListener.OpenMyProfile)
                    {
                        case true:
                            return;
                        default:
                        {
                            var intent = new Intent(activity, typeof(MyProfileActivity));
                            activity.StartActivity(intent);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static bool GetStatusOnline(int lastSeen, string isShowOnline)
        {
            try
            {
                string time = Methods.Time.TimeAgo(lastSeen, false);
                bool status = isShowOnline == "on" && time == Methods.Time.LblJustNow;
                return status;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }

        public static JobInfoObject ListFilterJobs(JobInfoObject jobInfoObject)   
        {
            try
            {
                jobInfoObject.Image = jobInfoObject.Image.Contains(Client.WebsiteUrl) switch
                {
                    false => GetTheFinalLink(jobInfoObject.Image),
                    _ => jobInfoObject.Image
                };

                jobInfoObject.IsOwner = jobInfoObject.UserId == UserDetails.UserId;

                return jobInfoObject;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return jobInfoObject;
            }
        }

        public static Dictionary<string, string> GetSalaryDateList(Activity activity)
        {
            try
            {
                var arrayAdapter = new Dictionary<string, string>
                {
                    {"per_hour", activity.GetString(Resource.String.Lbl_per_hour)},
                    {"per_day", activity.GetString(Resource.String.Lbl_per_day)},
                    {"per_week", activity.GetString(Resource.String.Lbl_per_week)},
                    {"per_month", activity.GetString(Resource.String.Lbl_per_month)},
                    {"per_year", activity.GetString(Resource.String.Lbl_per_year)}
                };
                 
                return arrayAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new Dictionary<string, string>();
            }
        }
         
        public static Dictionary<string, string> GetJobTypeList(Activity activity)
        {
            try
            { 
                var arrayAdapter = new Dictionary<string, string>
                {
                    {"full_time", activity.GetString(Resource.String.Lbl_full_time)},
                    {"part_time", activity.GetString(Resource.String.Lbl_part_time)},
                    {"internship", activity.GetString(Resource.String.Lbl_internship)},
                    {"volunteer", activity.GetString(Resource.String.Lbl_volunteer)},
                    {"contract", activity.GetString(Resource.String.Lbl_contract)}
                };
                 
                return arrayAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new Dictionary<string, string>();
            }
        }
          
        public static Dictionary<string, string> GetAddQuestionList(Activity activity)
        {
            try
            { 
                var arrayAdapter = new Dictionary<string, string>
                {
                    {"free_text_question", activity.GetString(Resource.String.Lbl_FreeTextQuestion)},
                    {"yes_no_question", activity.GetString(Resource.String.Lbl_YesNoQuestion)},
                    {"multiple_choice_question", activity.GetString(Resource.String.Lbl_MultipleChoiceQuestion)},
                };
                 
                return arrayAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new Dictionary<string, string>();
            }
        }
        
        public static Dictionary<string, string> GetAddDiscountList(Activity activity)
        {
            try
            { 
                var arrayAdapter = new Dictionary<string, string>
                {
                    {"discount_percent", activity.GetString(Resource.String.Lbl_DiscountPercent)},
                    {"discount_amount", activity.GetString(Resource.String.Lbl_DiscountAmount)},
                    {"buy_get_discount", activity.GetString(Resource.String.Lbl_BuyGetDiscount)},
                    {"spend_get_off", activity.GetString(Resource.String.Lbl_SpendGetOff)},
                    {"free_shipping", activity.GetString(Resource.String.Lbl_FreeShipping)},
                };
                 
                return arrayAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new Dictionary<string, string>();
            }
        }
         
        public static Dictionary<string, string> GetCountryList(Activity activity)
        {
            try
            {
                var arrayAdapter = new Dictionary<string, string>
                {
                    {"1",  activity.GetString(Resource.String.Lbl_country1)}, 
                    {"2",  activity.GetString(Resource.String.Lbl_country2)}, 
                    {"3",  activity.GetString(Resource.String.Lbl_country3)}, 
                    {"4",  activity.GetString(Resource.String.Lbl_country4)},
                    {"5",  activity.GetString(Resource.String.Lbl_country5)},
                    {"6",  activity.GetString(Resource.String.Lbl_country6)},
                    {"7",  activity.GetString(Resource.String.Lbl_country7)},
                    {"8",  activity.GetString(Resource.String.Lbl_country8)},
                    {"9",  activity.GetString(Resource.String.Lbl_country9)},
                    {"10", activity.GetString(Resource.String.Lbl_country10)}, 
                    {"11", activity.GetString(Resource.String.Lbl_country11)}, 
                    {"12", activity.GetString(Resource.String.Lbl_country12)}, 
                    {"13", activity.GetString(Resource.String.Lbl_country13)}, 
                    {"14", activity.GetString(Resource.String.Lbl_country14)}, 
                    {"15", activity.GetString(Resource.String.Lbl_country15)}, 
                    {"16", activity.GetString(Resource.String.Lbl_country16)}, 
                    {"17", activity.GetString(Resource.String.Lbl_country17)}, 
                    {"18", activity.GetString(Resource.String.Lbl_country18)}, 
                    {"19", activity.GetString(Resource.String.Lbl_country19)}, 
                    {"20", activity.GetString(Resource.String.Lbl_country20)}, 
                    {"21", activity.GetString(Resource.String.Lbl_country21)}, 
                    {"22", activity.GetString(Resource.String.Lbl_country22)}, 
                    {"23", activity.GetString(Resource.String.Lbl_country23)}, 
                    {"24", activity.GetString(Resource.String.Lbl_country24)}, 
                    {"25", activity.GetString(Resource.String.Lbl_country25)}, 
                    {"26", activity.GetString(Resource.String.Lbl_country26)}, 
                    {"27", activity.GetString(Resource.String.Lbl_country27)}, 
                    {"28", activity.GetString(Resource.String.Lbl_country28)}, 
                    {"29", activity.GetString(Resource.String.Lbl_country29)}, 
                    {"30", activity.GetString(Resource.String.Lbl_country30)}, 
                    {"31", activity.GetString(Resource.String.Lbl_country31)}, 
                    {"32", activity.GetString(Resource.String.Lbl_country32)}, 
                    {"34", activity.GetString(Resource.String.Lbl_country34)}, 
                    {"35", activity.GetString(Resource.String.Lbl_country35)}, 
                    {"36", activity.GetString(Resource.String.Lbl_country36)}, 
                    {"37", activity.GetString(Resource.String.Lbl_country37)}, 
                    {"38", activity.GetString(Resource.String.Lbl_country38)}, 
                    {"39", activity.GetString(Resource.String.Lbl_country39)}, 
                    {"40", activity.GetString(Resource.String.Lbl_country40)}, 
                    {"41", activity.GetString(Resource.String.Lbl_country41)}, 
                    {"42", activity.GetString(Resource.String.Lbl_country42)}, 
                    {"43", activity.GetString(Resource.String.Lbl_country43)}, 
                    {"44", activity.GetString(Resource.String.Lbl_country44)}, 
                    {"45", activity.GetString(Resource.String.Lbl_country45)}, 
                    {"46", activity.GetString(Resource.String.Lbl_country46)}, 
                    {"47", activity.GetString(Resource.String.Lbl_country47)}, 
                    {"48", activity.GetString(Resource.String.Lbl_country48)}, 
                    {"49", activity.GetString(Resource.String.Lbl_country49)},
                    {"50", activity.GetString(Resource.String.Lbl_country50)},
                    {"51", activity.GetString(Resource.String.Lbl_country51)},
                    {"52", activity.GetString(Resource.String.Lbl_country52)},
                    {"53", activity.GetString(Resource.String.Lbl_country53)},
                    {"54", activity.GetString(Resource.String.Lbl_country54)},
                    {"55", activity.GetString(Resource.String.Lbl_country55)},
                    {"56", activity.GetString(Resource.String.Lbl_country56)},
                    {"57", activity.GetString(Resource.String.Lbl_country57)},
                    {"58", activity.GetString(Resource.String.Lbl_country58)},
                    {"59", activity.GetString(Resource.String.Lbl_country59)},
                    {"60", activity.GetString(Resource.String.Lbl_country60)},
                    {"61", activity.GetString(Resource.String.Lbl_country61)},
                    {"62", activity.GetString(Resource.String.Lbl_country62)},
                    {"63", activity.GetString(Resource.String.Lbl_country63)},
                    {"64", activity.GetString(Resource.String.Lbl_country64)},
                    {"65", activity.GetString(Resource.String.Lbl_country65)},
                    {"66", activity.GetString(Resource.String.Lbl_country66)},
                    {"67", activity.GetString(Resource.String.Lbl_country67)},
                    {"68", activity.GetString(Resource.String.Lbl_country68)},
                    {"69", activity.GetString(Resource.String.Lbl_country69)},
                    {"70", activity.GetString(Resource.String.Lbl_country70)},
                    {"71", activity.GetString(Resource.String.Lbl_country71)},
                    {"72", activity.GetString(Resource.String.Lbl_country72)},
                    {"73", activity.GetString(Resource.String.Lbl_country73)},
                    {"74", activity.GetString(Resource.String.Lbl_country74)},
                    {"75", activity.GetString(Resource.String.Lbl_country75)},
                    {"76", activity.GetString(Resource.String.Lbl_country76)},
                    {"77", activity.GetString(Resource.String.Lbl_country77)},
                    {"78", activity.GetString(Resource.String.Lbl_country78)},
                    {"79", activity.GetString(Resource.String.Lbl_country79)},
                    {"80", activity.GetString(Resource.String.Lbl_country80)},
                    {"81", activity.GetString(Resource.String.Lbl_country81)},
                    {"82", activity.GetString(Resource.String.Lbl_country82)},
                    {"83", activity.GetString(Resource.String.Lbl_country83)},
                    {"84", activity.GetString(Resource.String.Lbl_country84)},
                    {"85", activity.GetString(Resource.String.Lbl_country85)},
                    {"86", activity.GetString(Resource.String.Lbl_country86)},
                    {"87", activity.GetString(Resource.String.Lbl_country87)},
                    {"88", activity.GetString(Resource.String.Lbl_country88)},
                    {"89", activity.GetString(Resource.String.Lbl_country89)},
                    {"90", activity.GetString(Resource.String.Lbl_country90)},
                    {"91", activity.GetString(Resource.String.Lbl_country91)},
                    {"92", activity.GetString(Resource.String.Lbl_country92)},
                    {"93", activity.GetString(Resource.String.Lbl_country93)},
                    {"94", activity.GetString(Resource.String.Lbl_country94)},
                    {"95", activity.GetString(Resource.String.Lbl_country95)},
                    {"96", activity.GetString(Resource.String.Lbl_country96)},
                    {"97", activity.GetString(Resource.String.Lbl_country97)},
                    {"98", activity.GetString(Resource.String.Lbl_country98)},
                    {"99", activity.GetString(Resource.String.Lbl_country99)},
                    {"100",activity.GetString(Resource.String.Lbl_country100)},
                    {"101",activity.GetString(Resource.String.Lbl_country101)},
                    {"102",activity.GetString(Resource.String.Lbl_country102)},
                    {"103",activity.GetString(Resource.String.Lbl_country103)},
                    {"104",activity.GetString(Resource.String.Lbl_country104)},
                    {"105",activity.GetString(Resource.String.Lbl_country105)},
                    {"106",activity.GetString(Resource.String.Lbl_country106)},
                    {"107",activity.GetString(Resource.String.Lbl_country107)},
                    {"108",activity.GetString(Resource.String.Lbl_country108)},
                    {"109",activity.GetString(Resource.String.Lbl_country109)},
                    {"110",activity.GetString(Resource.String.Lbl_country110)},
                    {"111",activity.GetString(Resource.String.Lbl_country111)},
                    {"112",activity.GetString(Resource.String.Lbl_country112)},
                    {"113",activity.GetString(Resource.String.Lbl_country113)},
                    {"114",activity.GetString(Resource.String.Lbl_country114)},
                    {"115",activity.GetString(Resource.String.Lbl_country115)},
                    {"116",activity.GetString(Resource.String.Lbl_country116)},
                    {"117",activity.GetString(Resource.String.Lbl_country117)},
                    {"118",activity.GetString(Resource.String.Lbl_country118)},
                    {"119",activity.GetString(Resource.String.Lbl_country119)},
                    {"120",activity.GetString(Resource.String.Lbl_country120)},
                    {"121",activity.GetString(Resource.String.Lbl_country121)},
                    {"122",activity.GetString(Resource.String.Lbl_country122)},
                    {"123",activity.GetString(Resource.String.Lbl_country123)},
                    {"124",activity.GetString(Resource.String.Lbl_country124)},
                    {"125",activity.GetString(Resource.String.Lbl_country125)},
                    {"126",activity.GetString(Resource.String.Lbl_country126)},
                    {"127",activity.GetString(Resource.String.Lbl_country127)},
                    {"128",activity.GetString(Resource.String.Lbl_country128)},
                    {"129",activity.GetString(Resource.String.Lbl_country129)},
                    {"130",activity.GetString(Resource.String.Lbl_country130)},
                    {"131",activity.GetString(Resource.String.Lbl_country131)},
                    {"132",activity.GetString(Resource.String.Lbl_country132)},
                    {"133",activity.GetString(Resource.String.Lbl_country133)},
                    {"134",activity.GetString(Resource.String.Lbl_country134)},
                    {"135",activity.GetString(Resource.String.Lbl_country135)},
                    {"136",activity.GetString(Resource.String.Lbl_country136)},
                    {"137",activity.GetString(Resource.String.Lbl_country137)},
                    {"138",activity.GetString(Resource.String.Lbl_country138)},
                    {"139",activity.GetString(Resource.String.Lbl_country139)},
                    {"140",activity.GetString(Resource.String.Lbl_country140)},
                    {"141",activity.GetString(Resource.String.Lbl_country141)},
                    {"142",activity.GetString(Resource.String.Lbl_country142)},
                    {"143",activity.GetString(Resource.String.Lbl_country143)},
                    {"144",activity.GetString(Resource.String.Lbl_country144)},
                    {"145",activity.GetString(Resource.String.Lbl_country145)},
                    {"146",activity.GetString(Resource.String.Lbl_country146)},
                    {"147",activity.GetString(Resource.String.Lbl_country147)},
                    {"148",activity.GetString(Resource.String.Lbl_country148)},
                    {"149",activity.GetString(Resource.String.Lbl_country149)},
                    {"150",activity.GetString(Resource.String.Lbl_country150)},
                    {"151",activity.GetString(Resource.String.Lbl_country151)},
                    {"152",activity.GetString(Resource.String.Lbl_country152)},
                    {"153",activity.GetString(Resource.String.Lbl_country153)},
                    {"154",activity.GetString(Resource.String.Lbl_country154)},
                    {"155",activity.GetString(Resource.String.Lbl_country155)},
                    {"156",activity.GetString(Resource.String.Lbl_country156)},
                    {"157",activity.GetString(Resource.String.Lbl_country157)},
                    {"158",activity.GetString(Resource.String.Lbl_country158)},
                    {"159",activity.GetString(Resource.String.Lbl_country159)},
                    {"160",activity.GetString(Resource.String.Lbl_country160)},
                    {"161",activity.GetString(Resource.String.Lbl_country161)},
                    {"162",activity.GetString(Resource.String.Lbl_country162)},
                    {"163",activity.GetString(Resource.String.Lbl_country163)},
                    {"164",activity.GetString(Resource.String.Lbl_country164)},
                    {"165",activity.GetString(Resource.String.Lbl_country165)},
                    {"166",activity.GetString(Resource.String.Lbl_country166)},
                    {"167",activity.GetString(Resource.String.Lbl_country167)}, 
                    {"168",activity.GetString(Resource.String.Lbl_country168)}, 
                    {"169",activity.GetString(Resource.String.Lbl_country169)}, 
                    {"170",activity.GetString(Resource.String.Lbl_country170)}, 
                    {"171",activity.GetString(Resource.String.Lbl_country171)}, 
                    {"172",activity.GetString(Resource.String.Lbl_country172)}, 
                    {"173",activity.GetString(Resource.String.Lbl_country173)}, 
                    {"174",activity.GetString(Resource.String.Lbl_country174)}, 
                    {"175",activity.GetString(Resource.String.Lbl_country175)}, 
                    {"176",activity.GetString(Resource.String.Lbl_country176)}, 
                    {"177",activity.GetString(Resource.String.Lbl_country177)}, 
                    {"178",activity.GetString(Resource.String.Lbl_country178)}, 
                    {"179",activity.GetString(Resource.String.Lbl_country179)}, 
                    {"180",activity.GetString(Resource.String.Lbl_country180)}, 
                    {"181",activity.GetString(Resource.String.Lbl_country181)}, 
                    {"182",activity.GetString(Resource.String.Lbl_country182)}, 
                    {"183",activity.GetString(Resource.String.Lbl_country183)}, 
                    {"184",activity.GetString(Resource.String.Lbl_country184)}, 
                    {"185",activity.GetString(Resource.String.Lbl_country185)},
                    {"186",activity.GetString(Resource.String.Lbl_country186)},
                    {"187",activity.GetString(Resource.String.Lbl_country187)},
                    {"188",activity.GetString(Resource.String.Lbl_country188)}, 
                    {"189",activity.GetString(Resource.String.Lbl_country189)}, 
                    {"190",activity.GetString(Resource.String.Lbl_country190)}, 
                    {"191",activity.GetString(Resource.String.Lbl_country191)},
                    {"192",activity.GetString(Resource.String.Lbl_country192)}, 
                    {"193",activity.GetString(Resource.String.Lbl_country193)}, 
                    {"194",activity.GetString(Resource.String.Lbl_country194)}, 
                    {"195",activity.GetString(Resource.String.Lbl_country195)},
                    {"196",activity.GetString(Resource.String.Lbl_country196)}, 
                    {"197",activity.GetString(Resource.String.Lbl_country197)},
                    {"198",activity.GetString(Resource.String.Lbl_country198)}, 
                    {"199",activity.GetString(Resource.String.Lbl_country199)}, 
                    {"200",activity.GetString(Resource.String.Lbl_country200)},
                    {"201",activity.GetString(Resource.String.Lbl_country201)}, 
                    {"202",activity.GetString(Resource.String.Lbl_country202)}, 
                    {"203",activity.GetString(Resource.String.Lbl_country203)}, 
                    {"204",activity.GetString(Resource.String.Lbl_country204)}, 
                    {"205",activity.GetString(Resource.String.Lbl_country205)}, 
                    {"206",activity.GetString(Resource.String.Lbl_country206)}, 
                    {"207",activity.GetString(Resource.String.Lbl_country207)}, 
                    {"208",activity.GetString(Resource.String.Lbl_country208)}, 
                    {"209",activity.GetString(Resource.String.Lbl_country209)}, 
                    {"210",activity.GetString(Resource.String.Lbl_country210)}, 
                    {"211",activity.GetString(Resource.String.Lbl_country211)}, 
                    {"212",activity.GetString(Resource.String.Lbl_country212)}, 
                    {"213",activity.GetString(Resource.String.Lbl_country213)}, 
                    {"214",activity.GetString(Resource.String.Lbl_country214)}, 
                    {"215",activity.GetString(Resource.String.Lbl_country215)}, 
                    {"216",activity.GetString(Resource.String.Lbl_country216)}, 
                    {"217",activity.GetString(Resource.String.Lbl_country217)}, 
                    {"218",activity.GetString(Resource.String.Lbl_country218)}, 
                    {"219",activity.GetString(Resource.String.Lbl_country219)}, 
                    {"220",activity.GetString(Resource.String.Lbl_country220)}, 
                    {"221",activity.GetString(Resource.String.Lbl_country221)}, 
                    {"222",activity.GetString(Resource.String.Lbl_country222)}, 
                    {"223",activity.GetString(Resource.String.Lbl_country223)}, 
                    {"224",activity.GetString(Resource.String.Lbl_country224)}, 
                    {"225",activity.GetString(Resource.String.Lbl_country225)}, 
                    {"226",activity.GetString(Resource.String.Lbl_country226)}, 
                    {"227",activity.GetString(Resource.String.Lbl_country227)}, 
                    {"228",activity.GetString(Resource.String.Lbl_country228)}, 
                    {"229",activity.GetString(Resource.String.Lbl_country229)}, 
                    {"230",activity.GetString(Resource.String.Lbl_country230)}, 
                    {"231",activity.GetString(Resource.String.Lbl_country231)}, 
                    {"232",activity.GetString(Resource.String.Lbl_country232)}, 
                    {"233",activity.GetString(Resource.String.Lbl_country233)}, 
                    {"238",activity.GetString(Resource.String.Lbl_country238)}, 
                    {"239",activity.GetString(Resource.String.Lbl_country239)}, 
                    {"240",activity.GetString(Resource.String.Lbl_country240)}, 
                    {"241",activity.GetString(Resource.String.Lbl_country241)}, 
                    {"242",activity.GetString(Resource.String.Lbl_country242)},
                };

                return arrayAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new Dictionary<string, string>();
            }
        }
         
        public static bool CheckAllowedFileSharingInServer(string type)
        {
            try
            {
                switch (type)
                {
                    case "File" when !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.FileSharing) && ListUtils.SettingsSiteList?.FileSharing == "1":
                    // Allowed
                    case "Video" when !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.VideoUpload) && ListUtils.SettingsSiteList?.VideoUpload == "1":
                    // Allowed
                    case "Audio" when !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.AudioUpload) && ListUtils.SettingsSiteList?.AudioUpload == "1":
                    // Allowed
                    case "Image":
                        // Allowed
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }

        public static bool CheckMimeTypesWithServer(string path)
        {
            try
            {
                var allowedExtenstionStatic = "jpg,png,jpeg,gif,mp4,m4v,webm,flv,mov,mpeg,mp3,wav";
                switch (string.IsNullOrEmpty(path))
                {
                    case false:
                    {
                        var fileName = path.Split('/').Last();
                        var fileNameWithExtension = fileName.Split('.').Last();

                        switch (string.IsNullOrEmpty(ListUtils.SettingsSiteList?.MimeTypes))
                        {
                            case false:
                            {
                                var allowedExtenstion = ListUtils.SettingsSiteList?.AllowedExtenstion; //jpg,png,jpeg,gif,mkv,docx,zip,rar,pdf,doc,mp3,mp4,flv,wav,txt,mov,avi,webm,wav,mpeg
                                var mimeTypes = ListUtils.SettingsSiteList?.MimeTypes; //video/mp4,video/mov,video/mpeg,video/flv,video/avi,video/webm,audio/wav,audio/mpeg,video/quicktime,audio/mp3,image/png,image/jpeg,image/gif,application/pdf,application/msword,application/zip,application/x-rar-compressed,text/pdf,application/x-pointplus,text/css

                                var getMimeType = MimeTypeMap.GetMimeType(fileNameWithExtension);

                                if (allowedExtenstion.Contains(fileNameWithExtension) && mimeTypes.Contains(getMimeType))
                                {
                                    var type = Methods.AttachmentFiles.Check_FileExtension(path);

                                    var check = CheckAllowedFileSharingInServer(type);
                                    switch (check)
                                    {
                                        // Allowed
                                        case true:
                                            return true;
                                    }
                                }

                                break;
                            }
                        }

                        //just this Allowed : >> jpg,png,jpeg,gif,mp4,m4v,webm,flv,mov,mpeg,mp3,wav
                        if (allowedExtenstionStatic.Contains(fileNameWithExtension))
                            return true;
                        break;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }
         
        // Functions Save Images
        private static void SaveFile(string id, string folder, string fileName, string url)
        {
            try
            {
                if (url.Contains("http"))
                {
                    string folderDestination = folder + id + "/";

                    string filePath = Path.Combine(folderDestination);
                    string mediaFile = filePath + "/" + fileName;

                    if (File.Exists(mediaFile)) return; 
                    WebClient webClient = new WebClient();

                    webClient.DownloadDataAsync(new Uri(url));

                    webClient.DownloadDataCompleted += (s, e) =>
                    {
                        try
                        {
                            switch (e.Cancelled)
                            {
                                case true:
                                    //Downloading Cancelled
                                    return;
                            }

                            if (e.Error != null)
                            {
                                Console.WriteLine(e.Error);
                                return;
                            }

                            if (!File.Exists(mediaFile))
                            {
                                using FileStream fs = new FileStream(mediaFile, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
                                fs.Write(e.Result, 0, e.Result.Length);
                            }

                            //File.WriteAllBytes(mediaFile, e.Result);
                        }
                        catch (IOException exception)
                        {
                            Console.WriteLine(exception);
                            //Methods.DisplayReportResultTrack(exception);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    };
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        // Functions file from folder
        public static string GetFile(string id, string folder, string filename, string url)
        {
            try
            {
                string folderDestination = folder + id + "/";

                if (!Directory.Exists(folderDestination))
                {
                    if (Directory.Exists(Methods.Path.FolderDiskStory))
                        Directory.Delete(Methods.Path.FolderDiskStory, true);

                    Directory.CreateDirectory(folderDestination);
                }

                string imageFile = Methods.MultiMedia.GetMediaFrom_Gallery(folderDestination, filename);
                switch (imageFile)
                {
                    case "File Dont Exists":
                        //This code runs on a new thread, control is returned to the caller on the UI thread.
                        Task.Factory.StartNew(() => { SaveFile(id, folder, filename, url); });
                        return url;
                    default:
                        return imageFile;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return url;
            }
        }

        public static string GetDuration(string mediaFile)
        {
            try
            {
                string duration;
                MediaMetadataRetriever retriever;
                if (mediaFile.Contains("http"))
                {
                    retriever = new MediaMetadataRetriever();
                    switch ((int)Build.VERSION.SdkInt)
                    {
                        case >= 14:
                            retriever.SetDataSource(mediaFile, new Dictionary<string, string>());
                            break;
                        default:
                            retriever.SetDataSource(mediaFile);
                            break;
                    }

                    duration = retriever.ExtractMetadata(MetadataKey.Duration); //time In Millisec 
                    retriever.Release();
                }
                else
                {
                    var file = Android.Net.Uri.FromFile(new Java.IO.File(mediaFile));
                    retriever = new MediaMetadataRetriever();
                    //if ((int)Build.VERSION.SdkInt >= 14)
                    //    retriever.SetDataSource(file.Path, new Dictionary<string, string>());
                    //else
                    retriever.SetDataSource(file?.Path);

                    duration = retriever.ExtractMetadata(MetadataKey.Duration); //time In Millisec 
                    retriever.Release();
                }

                return duration;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return "0";
            }
        }

        private static readonly string[] RelationshipLocal = Application.Context.Resources?.GetStringArray(Resource.Array.RelationShipArray);
        public static string GetRelationship(int index)
        {
            try
            {
                switch (index)
                {
                    case > -1:
                    {
                        string name = RelationshipLocal[index];
                        return name;
                    }
                    default:
                        return RelationshipLocal?.First() ?? "";
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return "";
            }
        }

        public static bool IsValidPassword(string password)
        {
            try
            {
                bool flag;
                switch (password.Length)
                {
                    case >= 8 when password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsNumber) && password.Any(char.IsSymbol):
                        Console.WriteLine("valid");
                        flag = true;
                        break;
                    default:
                        Console.WriteLine("invalid");
                        flag = false;
                        break;
                }

                return flag;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }
          
        public static bool IsLikedPage(PageClass pageData)
        {
            try
            {
                switch (string.IsNullOrEmpty(pageData?.IsLiked.String))
                {
                    case false:
                        switch (pageData.IsLiked.String.ToLower())
                        {
                            case "no":
                                return false;
                            case "yes":
                                return true;
                        }

                        break;
                }

                return pageData?.IsLiked.Bool != null && pageData.IsLiked.Bool.Value;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }
        
        public static bool IsJoinedGroup(GroupClass pageData)
        {
            try
            {
                switch (string.IsNullOrEmpty(pageData?.IsJoined.String))
                {
                    case false:
                        switch (pageData.IsJoined.String.ToLower())
                        {
                            case "no":
                                return false;
                            case "yes":
                                return true;
                        }

                        break;
                }

                return pageData?.IsJoined.Bool != null && pageData.IsJoined.Bool.Value;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }
         
        /// <summary>
        /// ['amazone_s3'] == 1   >> $wo['config']['s3_site_url'] . '/' . $media;
        /// ['spaces'] == 1       >> 'https://' . $wo['config']['space_name'] . '.' . $wo['config']['space_region'] . '.digitaloceanspaces.com/' . $media;
        /// ['ftp_upload'] == 1   >> "http://".$wo['config']['ftp_endpoint'] . '/' . $media;
        /// ['cloud_upload'] == 1 >> "'https://storage.cloud.google.com/'. $wo['config']['cloud_bucket_name'] . '/' . $media;
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public static string GetTheFinalLink(string media)
        {
            try
            {
                if (media.Contains("http"))
                    return media;

                var path = media; 
                var config = ListUtils.SettingsSiteList;
                switch (string.IsNullOrEmpty(config?.AmazoneS3))
                {
                    case false when config?.AmazoneS3 == "1":
                        path = config.S3SiteUrl + "/" + media;
                        return path;
                }

                switch (string.IsNullOrEmpty(config?.Spaces))
                {
                    case false when config?.Spaces == "1":
                        path = "https://" + config.SpaceName + "." + config.SpaceRegion + ".digitaloceanspaces.com/" + media;
                        return path;
                }
               
                switch (string.IsNullOrEmpty(config?.FtpUpload))
                {
                    case false when config?.FtpUpload == "1":
                        path = "http://" + config.FtpEndpoint + "/" + media;
                        return path;
                }
                
                switch (string.IsNullOrEmpty(config?.CloudUpload))
                {
                    case false when config?.CloudUpload == "1":
                        path = "https://storage.cloud.google.com/" + config.BucketName + "/" + media;
                        return path;
                }

                path = media.Contains(Client.WebsiteUrl) switch
                {
                    false => Client.WebsiteUrl + "/" + media,
                    _ => path
                };

                return path;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return media;
            }
        }
         
        public static void SetAddFriend(Activity activity ,UserDataObject item , Button btnAddUser)
        {
            try
            {
                item.IsFollowing = item.IsFollowing switch
                {
                    null => "0",
                    _ => item.IsFollowing
                };

                var dbDatabase = new SqLiteDatabase();
                string isFollowing;
                switch (item.IsFollowing)
                {
                    case "0": // Add Or request friends
                    case "no":
                    case "No":
                        if (item.ConfirmFollowers == "1" || AppSettings.ConnectivitySystem == 0)
                        {
                            item.IsFollowing = isFollowing = "2";
                            btnAddUser.Tag = "request";

                            dbDatabase.Insert_Or_Replace_OR_Delete_UsersContact(item, "Update");
                        }
                        else
                        {
                            item.IsFollowing = isFollowing = "1";
                            btnAddUser.Tag = "friends";

                            dbDatabase.Insert_Or_Replace_OR_Delete_UsersContact(item, "Insert");
                        }

                        SetAddFriendCondition(isFollowing, btnAddUser);

                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Global.FollowUserAsync(item.UserId) });

                        break;
                    case "1": // Remove friends
                    case "yes": 
                    case "Yes":  
                        item.IsFollowing = isFollowing = "0";
                        btnAddUser.Tag = "Add";

                        dbDatabase.Insert_Or_Replace_OR_Delete_UsersContact(item, "Delete");

                        SetAddFriendCondition(isFollowing, btnAddUser);

                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Global.FollowUserAsync(item.UserId) });

                        break;
                    case "2": // Remove request friends

                        var dialog = new MaterialDialog.Builder(activity).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);
                        dialog.Content(activity.GetText(Resource.String.Lbl_confirmationUnFriend));
                        dialog.PositiveText(activity.GetText(Resource.String.Lbl_Confirm)).OnPositive((materialDialog, action) =>
                        {
                            try
                            {
                                item.IsFollowing = isFollowing = "0";
                                btnAddUser.Tag = "Add";

                                dbDatabase = new SqLiteDatabase();
                                dbDatabase.Insert_Or_Replace_OR_Delete_UsersContact(item, "Delete");

                                SetAddFriendCondition(isFollowing, btnAddUser);

                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Global.FollowUserAsync(item.UserId) }); 
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        });
                        dialog.NegativeText(activity.GetText(Resource.String.Lbl_Close)).OnNegative(new MyMaterialDialog());
                        dialog.AlwaysCallSingleChoiceCallback();
                        dialog.Build().Show();
                       
                        break;
                }
                 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static void SetAddFriendCondition(string isFollowing, Button btnAddUser)
        {
            try
            {
                switch (isFollowing)
                {
                    //>> Not Friend
                    case "0":
                        btnAddUser.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        btnAddUser.Text = Application.Context.GetText(AppSettings.ConnectivitySystem == 1 ? Resource.String.Lbl_Follow : Resource.String.Lbl_AddFriends);
                        btnAddUser.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        btnAddUser.Tag = "Add";
                        break;
                    //>> Friend
                    case "1":
                        btnAddUser.SetTextColor(Color.White);
                        btnAddUser.Text = Application.Context.GetText(AppSettings.ConnectivitySystem == 1 ? Resource.String.Lbl_Following : Resource.String.Lbl_Friends);
                        btnAddUser.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        btnAddUser.Tag = "friends";
                        break;
                    //>> Request
                    case "2":
                        btnAddUser.SetTextColor(Color.ParseColor("#444444"));
                        btnAddUser.Text = Application.Context.GetText(Resource.String.Lbl_Request);
                        btnAddUser.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        btnAddUser.Tag = "request";
                        break;
                    default:
                        btnAddUser.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        btnAddUser.Text = Application.Context.GetText(AppSettings.ConnectivitySystem == 1 ? Resource.String.Lbl_Follow : Resource.String.Lbl_AddFriends);
                        btnAddUser.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        btnAddUser.Tag = "Add";
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static async void SetJoinGroup(Activity activity, string groupId, Button button)
        {
            try 
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(activity, activity.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                var (apiStatus, respond) = await RequestsAsync.Group.JoinGroupAsync(groupId);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case JoinGroupObject result when result.JoinStatus == "requested":
                                button.SetTextColor(Color.ParseColor("#444444"));
                                button.Text = Application.Context.GetText(Resource.String.Lbl_Request);
                                button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                                break;
                            case JoinGroupObject result:
                            {
                                var isJoined = result.JoinStatus == "left" ? "false" : "true";
                                button.Text = activity.GetText(isJoined == "yes" || isJoined == "true" ? Resource.String.Btn_Joined : Resource.String.Btn_Join_Group);

                                switch (isJoined)
                                {
                                    case "yes":
                                    case "true":
                                        button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                                        button.SetTextColor(Color.White);
                                        break;
                                    default:
                                        button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                                        button.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                        break;
                                }

                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Methods.DisplayReportResult(activity, respond);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public static void SetLikePage(Activity activity, string pageId, Button button)
        {
            try
            { 
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(activity, activity.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                switch (button?.Tag?.ToString())
                {
                    case "false":
                        button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        button.SetTextColor(Color.ParseColor("#ffffff"));
                        button.Text = activity.GetText(Resource.String.Btn_Unlike);
                        button.Tag = "true";
                        break;
                    default:
                        button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        button.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        button.Text = activity.GetText(Resource.String.Btn_Like);
                        button.Tag = "false";
                        break;
                }

                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Page.LikePageAsync(pageId) });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            } 
        }
         

        public static void ChangeMenuIconColor(IMenu menu, Color color)
        {
            try
            {
                for (int i = 0; i < menu.Size(); i++)
                {
                    var drawable = menu.GetItem(i)?.Icon;
                    switch (drawable)
                    {
                        case null:
                            continue;
                        default:
                            drawable.Mutate();
                            drawable.SetColorFilter(new PorterDuffColorFilter(color, PorterDuff.Mode.SrcAtop));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }



        #region MaterialDialog

        public class MyMaterialDialog : Java.Lang.Object, MaterialDialog.ISingleButtonCallback
        {
            public void OnClick(MaterialDialog p0, DialogAction p1)
            {
                try
                {
                    if (p1 == DialogAction.Positive)
                    {
                    }
                    else if (p1 == DialogAction.Negative)
                    {
                        p0.Dismiss();
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }
         
        #endregion

    }
} 