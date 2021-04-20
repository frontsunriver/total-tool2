using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Request;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.AddPost;
using WoWonder.Activities.AddPost.Service;
using WoWonder.Activities.Default;
using WoWonder.Activities.NativePost.Services;
using WoWonder.Activities.SettingsPreferences;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.OneSignal;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Articles;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Group;
using WoWonderClient.Classes.Page;
using WoWonderClient.Classes.User;
using WoWonderClient.Requests;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Exception = System.Exception;
using File = Java.IO.File;
using HttpMethod = System.Net.Http.HttpMethod;
using Thread = System.Threading.Thread;

namespace WoWonder.Helpers.Controller
{
    internal static class ApiRequest
    {
        private static readonly string ApiGetSearchGif = "https://api.giphy.com/v1/gifs/search?api_key=b9427ca5441b4f599efa901f195c9f58&limit=45&rating=g&q=";
        private static readonly string ApiGeTrendingGif = "https://api.giphy.com/v1/gifs/trending?api_key=b9427ca5441b4f599efa901f195c9f58&limit=45&rating=g";
        private static readonly string ApiGetTimeZone = "http://ip-api.com/json/";
        private static readonly string ApiGetWeatherApi = "https://api.weatherapi.com/v1/forecast.json?key=";
        private static readonly string ApiGetExchangeCurrency = "https://openexchangerates.org/api/latest.json?app_id=";
        private static readonly string ApiGetInfoCovid19 = "https://covid-193.p.rapidapi.com/statistics?country=";

        public static async Task<GetSiteSettingsObject.ConfigObject> GetSettings_Api(Activity context)
        {
            if (Methods.CheckConnectivity())
            {
                await SetLangUserAsync().ConfigureAwait(false);

                (var apiStatus, dynamic respond) = await Current.GetSettingsAsync();

                if (apiStatus != 200 || respond is not GetSiteSettingsObject result || result.Config == null)
                    return Methods.DisplayReportResult(context, respond);

                ListUtils.SettingsSiteList = result.Config;
                 
                AppSettings.OneSignalAppId = result.Config.AndroidNPushId;
                OneSignalNotification.RegisterNotificationDevice();

                SqLiteDatabase dbDatabase = new SqLiteDatabase();
                dbDatabase.InsertOrUpdateSettings(result.Config);

                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        //Page Categories
                        var listPage = result.Config.PageCategories?.Select(cat => new Classes.Categories
                        {
                            CategoriesId = cat.Key,
                            CategoriesName = Methods.FunString.DecodeString(cat.Value),
                            CategoriesColor = "#ffffff",
                            SubList = new List<SubCategories>()
                        }).ToList();

                        CategoriesController.ListCategoriesPage.Clear();
                        CategoriesController.ListCategoriesPage = listPage?.Count switch
                        {
                            > 0 => new ObservableCollection<Classes.Categories>(listPage),
                            _ => CategoriesController.ListCategoriesPage
                        };

                        switch (result.Config.PageSubCategories?.SubCategoriesList?.Count)
                        {
                            case > 0:
                            {
                                //Sub Categories Page
                                foreach (var sub in result.Config.PageSubCategories?.SubCategoriesList)
                                {
                                    var subCategories = result.Config.PageSubCategories?.SubCategoriesList?.FirstOrDefault(a => a.Key == sub.Key).Value;
                                    switch (subCategories?.Count)
                                    {
                                        case > 0:
                                        {
                                            var cat = CategoriesController.ListCategoriesPage.FirstOrDefault(a => a.CategoriesId == sub.Key);
                                            if (cat != null)
                                            {
                                                foreach (var pairs in subCategories)
                                                {
                                                    cat.SubList.Add(pairs);
                                                }
                                            }

                                            break;
                                        }
                                    }
                                }

                                break;
                            }
                        }

                        //Group Categories
                        var listGroup = result.Config.GroupCategories?.Select(cat => new Classes.Categories
                        {
                            CategoriesId = cat.Key,
                            CategoriesName = Methods.FunString.DecodeString(cat.Value),
                            CategoriesColor = "#ffffff",
                            SubList = new List<SubCategories>()
                        }).ToList();

                        CategoriesController.ListCategoriesGroup.Clear();
                        CategoriesController.ListCategoriesGroup = listGroup?.Count switch
                        {
                            > 0 => new ObservableCollection<Classes.Categories>(listGroup),
                            _ => CategoriesController.ListCategoriesGroup
                        };

                        switch (result.Config.GroupSubCategories?.SubCategoriesList?.Count)
                        {
                            case > 0:
                            {
                                //Sub Categories Group
                                foreach (var sub in result.Config.GroupSubCategories?.SubCategoriesList)
                                {
                                    var subCategories = result.Config.GroupSubCategories?.SubCategoriesList?.FirstOrDefault(a => a.Key == sub.Key).Value;
                                    switch (subCategories?.Count)
                                    {
                                        case > 0:
                                        {
                                            var cat = CategoriesController.ListCategoriesGroup.FirstOrDefault(a => a.CategoriesId == sub.Key);
                                            if (cat != null)
                                            {
                                                foreach (var pairs in subCategories)
                                                {
                                                    cat.SubList.Add(pairs);
                                                }
                                            }

                                            break;
                                        }
                                    }
                                }

                                break;
                            }
                        }

                        CategoriesController.ListCategoriesGroup = CategoriesController.ListCategoriesGroup.Count switch
                        {
                            0 when CategoriesController.ListCategoriesPage.Count > 0 => new
                                ObservableCollection<Classes.Categories>(CategoriesController.ListCategoriesPage),
                            _ => CategoriesController.ListCategoriesGroup
                        };

                        //Blog Categories
                        var listBlog = result.Config.BlogCategories?.Select(cat => new Classes.Categories
                        {
                            CategoriesId = cat.Key,
                            CategoriesName = Methods.FunString.DecodeString(cat.Value),
                            CategoriesColor = "#ffffff",
                            SubList = new List<SubCategories>()
                        }).ToList();

                        CategoriesController.ListCategoriesBlog.Clear();
                        CategoriesController.ListCategoriesBlog = listBlog?.Count switch
                        {
                            > 0 => new ObservableCollection<Classes.Categories>(listBlog),
                            _ => CategoriesController.ListCategoriesBlog
                        };

                        //Products Categories
                        var listProducts = result.Config.ProductsCategories?.Select(cat => new Classes.Categories
                        {
                            CategoriesId = cat.Key,
                            CategoriesName = Methods.FunString.DecodeString(cat.Value),
                            CategoriesColor = "#ffffff",
                            SubList = new List<SubCategories>()
                        }).ToList();

                        CategoriesController.ListCategoriesProducts.Clear();
                        CategoriesController.ListCategoriesProducts = listProducts?.Count switch
                        {
                            > 0 => new ObservableCollection<Classes.Categories>(listProducts),
                            _ => CategoriesController.ListCategoriesProducts
                        };

                        switch (result.Config.ProductsSubCategories?.SubCategoriesList?.Count)
                        {
                            case > 0:
                            {
                                //Sub Categories Products
                                foreach (var sub in result.Config.ProductsSubCategories?.SubCategoriesList)
                                {
                                    var subCategories = result.Config.ProductsSubCategories?.SubCategoriesList?.FirstOrDefault(a => a.Key == sub.Key).Value;
                                    switch (subCategories?.Count)
                                    {
                                        case > 0:
                                        {
                                            var cat = CategoriesController.ListCategoriesProducts.FirstOrDefault(a => a.CategoriesId == sub.Key);
                                            if (cat != null)
                                            {
                                                foreach (var pairs in subCategories)
                                                {
                                                    cat.SubList.Add(pairs);
                                                }
                                            }

                                            break;
                                        }
                                    }
                                }

                                break;
                            }
                        }

                        //Job Categories
                        var listJob = result.Config.JobCategories?.Select(cat => new Classes.Categories
                        {
                            CategoriesId = cat.Key,
                            CategoriesName = Methods.FunString.DecodeString(cat.Value),
                            CategoriesColor = "#ffffff",
                            SubList = new List<SubCategories>()
                        }).ToList();

                        CategoriesController.ListCategoriesJob.Clear();
                        CategoriesController.ListCategoriesJob = listJob?.Count switch
                        {
                            > 0 => new ObservableCollection<Classes.Categories>(listJob),
                            _ => CategoriesController.ListCategoriesJob
                        };

                        //Family
                        var listFamily = result.Config.Family?.Select(cat => new Classes.Family
                        {
                            FamilyId = cat.Key,
                            FamilyName = Methods.FunString.DecodeString(cat.Value),
                        }).ToList();

                        ListUtils.FamilyList.Clear();
                        ListUtils.FamilyList = listFamily?.Count switch
                        {
                            > 0 => new ObservableCollection<Classes.Family>(listFamily),
                            _ => ListUtils.FamilyList
                        };

                        //Movie Category
                        var listMovie = result.Config.MovieCategory?.Select(cat => new Classes.Categories
                        {
                            CategoriesId = cat.Key,
                            CategoriesName = Methods.FunString.DecodeString(cat.Value),
                            CategoriesColor = "#ffffff",
                            SubList = new List<SubCategories>()
                        }).ToList();

                        CategoriesController.ListCategoriesMovies.Clear();
                        CategoriesController.ListCategoriesMovies = listMovie?.Count switch
                        {
                            > 0 => new ObservableCollection<Classes.Categories>(listMovie),
                            _ => CategoriesController.ListCategoriesMovies
                        };

                        switch (AppSettings.SetApisReportMode)
                        {
                            case true:
                            {
                                switch (CategoriesController.ListCategoriesPage.Count)
                                {
                                    case 0:
                                        Methods.DialogPopup.InvokeAndShowDialog(context, "ReportMode", "List Categories Page Not Found, Please check api get_site_settings ", "Close");
                                        break;
                                }

                                switch (CategoriesController.ListCategoriesGroup.Count)
                                {
                                    case 0:
                                        Methods.DialogPopup.InvokeAndShowDialog(context, "ReportMode", "List Categories Group Not Found, Please check api get_site_settings ", "Close");
                                        break;
                                }

                                switch (CategoriesController.ListCategoriesProducts.Count)
                                {
                                    case 0:
                                        Methods.DialogPopup.InvokeAndShowDialog(context, "ReportMode", "List Categories Products Not Found, Please check api get_site_settings ", "Close");
                                        break;
                                }

                                switch (ListUtils.FamilyList.Count)
                                {
                                    case 0:
                                        Methods.DialogPopup.InvokeAndShowDialog(context, "ReportMode", "Family List Not Found, Please check api get_site_settings ", "Close");
                                        break;
                                }

                                switch (AppSettings.SetApisReportMode)
                                {
                                    case true when AppSettings.ShowColor:
                                    {
                                        if (ListUtils.SettingsSiteList?.PostColors != null && ListUtils.SettingsSiteList?.PostColors.Value.PostColorsList != null && ListUtils.SettingsSiteList?.PostColors.Value.PostColorsList.Count == 0)
                                        {
                                            Methods.DialogPopup.InvokeAndShowDialog(context, "ReportMode", "PostColors Not Found, Please check api get_site_settings ", "Close");
                                        }

                                        break;
                                    }
                                }

                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }).ConfigureAwait(false);
                  
                return result.Config; 
            }
            else
            {
                Toast.MakeText(Application.Context, Application.Context.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                return null!;
            }
        }

        public static async Task GetGifts()
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    var (apiStatus, respond) = await RequestsAsync.Global.FetchGiftAsync();
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case GiftObject result:
                                {
                                    switch (result.Data.Count)
                                    {
                                        case > 0:
                                        {
                                            ListUtils.GiftsList = new ObservableCollection<GiftObject.DataGiftObject>(result.Data);

                                            SqLiteDatabase sqLiteDatabase = new SqLiteDatabase();
                                            sqLiteDatabase.InsertAllGifts(ListUtils.GiftsList);
                                

                                            await Task.Factory.StartNew(() =>
                                            {
                                                try
                                                {
                                                    foreach (var item in result.Data)
                                                    {
                                                        Methods.MultiMedia.DownloadMediaTo_DiskAsync(Methods.Path.FolderDiskGif, item.MediaFile);

                                                        Glide.With(Application.Context).Load(item.MediaFile).Apply(new RequestOptions().SetDiskCacheStrategy(DiskCacheStrategy.All).CenterCrop()).Preload();
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    Methods.DisplayReportResultTrack(e);
                                                }
                                            });
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
                else
                {
                    Toast.MakeText(Application.Context, Application.Context.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public static async Task<string> GetTimeZoneAsync()
        {
            try
            {
                switch (AppSettings.AutoCodeTimeZone)
                {
                    case true:
                    {
                        var client = new HttpClient();
                        var response = await client.GetAsync(ApiGetTimeZone);
                        string json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<TimeZoneObject>(json);

                        UserDetails.Country = data.Country;
                        UserDetails.City = data.City;

                        return data?.Timezone;
                    }
                    default:
                        return AppSettings.CodeTimeZone;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return AppSettings.CodeTimeZone;
            }
        }

        private static async Task SetLangUserAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Current.AccessToken) || !AppSettings.SetLangUser)
                    return;

                string lang = "english";
                if (UserDetails.LangName.Contains("en"))
                    lang = "english";
                else if (UserDetails.LangName.Contains("ar"))
                    lang = "arabic";
                else if (UserDetails.LangName.Contains("de"))
                    lang = "german";
                else if (UserDetails.LangName.Contains("el"))
                    lang = "greek";
                else if (UserDetails.LangName.Contains("es"))
                    lang = "spanish";
                else if (UserDetails.LangName.Contains("fr"))
                    lang = "french";
                else if (UserDetails.LangName.Contains("it"))
                    lang = "italian";
                else if (UserDetails.LangName.Contains("ja"))
                    lang = "japanese";
                else if (UserDetails.LangName.Contains("nl"))
                    lang = "dutch";
                else if (UserDetails.LangName.Contains("pt"))
                    lang = "portuguese";
                else if (UserDetails.LangName.Contains("ro"))
                    lang = "romanian";
                else if (UserDetails.LangName.Contains("ru"))
                    lang = "russian";
                else if (UserDetails.LangName.Contains("sq"))
                    lang = "albanian";
                else if (UserDetails.LangName.Contains("sr"))
                    lang = "serbian";
                else if (UserDetails.LangName.Contains("tr"))
                    lang = "turkish";
                //else
                //    lang = string.IsNullOrEmpty(UserDetails.LangName) ? AppSettings.Lang : "";

                await Task.Factory.StartNew(() =>
                {
                    if (lang != "")
                    {
                        var dataPrivacy = new Dictionary<string, string>
                        {
                            {"language", lang}
                        };

                        var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                        if (dataUser != null)
                        {
                            dataUser.Language = lang;

                            var sqLiteDatabase = new SqLiteDatabase();
                            sqLiteDatabase.Insert_Or_Update_To_MyProfileTable(dataUser);
                            
                        }

                        if (Methods.CheckConnectivity())
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> {() => RequestsAsync.Global.UpdateUserDataAsync(dataPrivacy)});
                        else
                            Toast.MakeText(Application.Context,Application.Context.GetText(Resource.String.Lbl_CheckYourInternetConnection),ToastLength.Short)?.Show();
                    }
                });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public static async Task<ObservableCollection<GifGiphyClass.Datum>> SearchGif(string searchKey , string offset)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Application.Context, Application.Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return null!;
                }
                else
                {
                    var response = await RestHttp.Client.GetAsync(ApiGetSearchGif + searchKey + "&offset=" + offset);
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<GifGiphyClass>(json);

                    return response.StatusCode switch
                    {
                        HttpStatusCode.OK => data.DataMeta.Status == 200
                            ? new ObservableCollection<GifGiphyClass.Datum>(data.Data)
                            : null,
                        _ => null!
                    };
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }
        
        public static async Task<ObservableCollection<GifGiphyClass.Datum>> TrendingGif(string offset)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Application.Context, Application.Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return null!;
                }
                else
                {
                    var response = await RestHttp.Client.GetAsync(ApiGeTrendingGif + "&offset=" + offset);
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<GifGiphyClass>(json);

                    return response.StatusCode switch
                    {
                        HttpStatusCode.OK => data.DataMeta.Status == 200
                            ? new ObservableCollection<GifGiphyClass.Datum>(data.Data)
                            : null,
                        _ => null!
                    };
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }
        
        public static async Task<GetWeatherObject> GetWeather()
        {
            try
            { 
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Application.Context, Application.Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return null!;
                }
                else
                {
                    if (string.IsNullOrEmpty(UserDetails.City))
                    {
                        await GetTimeZoneAsync();  
                    }
                     
                    var response = await RestHttp.Client.GetAsync(ApiGetWeatherApi + AppSettings.KeyWeatherApi +"&q=" + UserDetails.City + "&lang=" + UserDetails.LangName);
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<GetWeatherObject>(json);
                    return response.StatusCode switch
                    {
                        HttpStatusCode.OK => data,
                        _ => null!
                    };
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }
         
        public static async Task<(int, dynamic)> GetExchangeCurrencyAsync()
        {
            try
            {   
                var response = await RestHttp.Client.GetAsync(ApiGetExchangeCurrency + AppSettings.KeyCurrencyApi + "&base=" + AppSettings.ExCurrency  + "&symbols=" + AppSettings.ExCurrencies ); 
                string json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<Classes.ExchangeCurrencyObject>(json);
                if (data != null)
                {   
                    return (200, data);
                }

                var error = JsonConvert.DeserializeObject<Classes.ExErrorObject>(json); 
                return (400, error);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return (404, e.Message);
            }
        }
         
        public static async Task<(int, dynamic)> GetInfoCovid19Async(string country)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ApiGetInfoCovid19 + country),
                    Headers =
                    {
                        {"x-rapidapi-key", AppSettings.KeyCoronaVirus},
                        {"x-rapidapi-host", AppSettings.HostCoronaVirus},
                    }
                };
                var response = await client.SendAsync(request); 
                string json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<Covid19Object>(json);
                if (data != null)
                {   
                    return (200, data);
                }

                var error = JsonConvert.DeserializeObject<ErrorCovid19Object>(json); 
                return (400, error);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return (404, e.Message);
            }
        }
         
        public static async Task Get_MyProfileData_Api(Activity context)
        {
            if (Methods.CheckConnectivity())
            {
                var (apiStatus, respond) = await RequestsAsync.Global.GetUserDataAsync(UserDetails.UserId);

                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetUserDataObject result:
                            {
                                UserDetails.Avatar = result.UserData.Avatar;
                                UserDetails.Cover = result.UserData.Cover;
                                UserDetails.Username = result.UserData.Username;
                                UserDetails.FullName = result.UserData.Name;
                                UserDetails.Email = result.UserData.Email;

                                ListUtils.MyProfileList = new ObservableCollection<UserDataObject> {result.UserData};

                                ListUtils.MyFollowersList = result.Followers?.Count switch
                                {
                                    > 0 => new ObservableCollection<UserDataObject>(result.Followers),
                                    _ => ListUtils.MyFollowersList
                                };

                                context?.RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        Glide.With(Application.Context).Load(UserDetails.Avatar).Apply(new RequestOptions().SetDiskCacheStrategy(DiskCacheStrategy.All).CircleCrop()).Preload();
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e); 
                                    }
                                });
                         
                                await Task.Factory.StartNew(() =>
                                {
                                    SqLiteDatabase dbDatabase = new SqLiteDatabase();
                                    dbDatabase.Insert_Or_Update_To_MyProfileTable(result.UserData);

                                    switch (result.Following?.Count)
                                    {
                                        case > 0:
                                            dbDatabase.Insert_Or_Replace_MyContactTable(new ObservableCollection<UserDataObject>(result.Following));
                                            break;
                                    }

                            
                                });
                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Methods.DisplayReportResult(context, respond);
                        break;
                }
            }
        }

        public static async Task LoadSuggestedGroup()
        {
            if (Methods.CheckConnectivity())
            {
                //var countList = ListUtils.SuggestedGroupList.Count;
                var (respondCode, respondString) = await RequestsAsync.Group.GetRecommendedGroupsAsync("25", "0").ConfigureAwait(false);
                switch (respondCode)
                {
                    case 200:
                    {
                        switch (respondString)
                        {
                            case ListGroupsObject result:
                            {
                                var respondList = result.Data.Count;
                                ListUtils.SuggestedGroupList = respondList switch
                                {
                                    > 0 => new ObservableCollection<GroupClass>(result.Data),
                                    _ => ListUtils.SuggestedGroupList
                                };

                                break;
                            }
                        }

                        break;
                    }
                }

                //else Methods.DisplayReportResult(activity, respondString);
            }
        }

        public static async Task LoadSuggestedUser()
        {
            if (Methods.CheckConnectivity())
            {
                //var countList = ListUtils.SuggestedUserList.Count;
                var (respondCode, respondString) = await RequestsAsync.Global.GetRecommendedUsersAsync("25", "0").ConfigureAwait(false);
                switch (respondCode)
                {
                    case 200:
                    {
                        switch (respondString)
                        {
                            case ListUsersObject result:
                            {
                                var respondList = result.Data.Count;
                                ListUtils.SuggestedUserList = respondList switch
                                {
                                    > 0 => new ObservableCollection<UserDataObject>(result.Data),
                                    _ => ListUtils.SuggestedUserList
                                };

                                break;
                            }
                        }

                        break;
                    }
                }

                //else Methods.DisplayReportResult(activity, respondString);
            }
        }
         
        public static async Task GetMyGroups()
        {
            if (Methods.CheckConnectivity())
            {
                try
                {
                    var (apiStatus, respond) = await RequestsAsync.Group.GetMyGroupsAsync("0", "25");
                    if (apiStatus != 200 || respond is not ListGroupsObject result || result.Data == null)
                    {
                        //Methods.DisplayReportResult(this, respond);
                    }
                    else
                    {
                        var respondList = result.Data.Count;
                        switch (respondList)
                        {
                            case > 0:
                            {
                                result.Data.Reverse(); 
                                ListUtils.MyGroupList = new ObservableCollection<GroupClass>(result.Data);

                                foreach (var groupClass in result.Data)
                                {
                                    ListUtils.ShortCutsList.Add(new Classes.ShortCuts
                                    {
                                        Id = ListUtils.ShortCutsList.Count +1,
                                        SocialId = groupClass.GroupId,
                                        Type = "Group",
                                        Name = groupClass.GroupName,
                                        GroupClass = groupClass,
                                    }); 
                                }

                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }

                try
                {
                    var (apiStatus2, respond2) = await RequestsAsync.Group.GetJoinedGroupsAsync(UserDetails.UserId, "0", "25");
                    if (apiStatus2 != 200 || !(respond2 is ListGroupsObject result2) || result2.Data == null)
                    {
                        //Methods.DisplayReportResult(this, respond);
                    }
                    else
                    {
                        var respondList = result2.Data.Count;
                        switch (respondList)
                        {
                            case > 0:
                            {
                                foreach (var item in result2.Data)
                                {
                                    switch (ListUtils.MyGroupList.FirstOrDefault(a => a.GroupId == item.GroupId))
                                    {
                                        case null:
                                            ListUtils.MyGroupList.Add(item);
                                            break;
                                    }
                                }

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
        }

        public static async Task GetMyPages()
        {
            if (Methods.CheckConnectivity())
            {
                var (apiStatus, respond) = await RequestsAsync.Page.GetMyPagesAsync("0", "25");
                if (apiStatus != 200 || respond is not ListPagesObject result || result.Data == null)
                {
                    //Methods.DisplayReportResult(this, respond);
                }
                else
                {
                    var respondList = result.Data.Count;
                    switch (respondList)
                    {
                        case > 0:
                        {
                            result.Data.Reverse();

                            ListUtils.MyPageList = new ObservableCollection<PageClass>(result.Data);

                            foreach (var pageClass in result.Data)
                            {
                                ListUtils.ShortCutsList.Add(new Classes.ShortCuts
                                {
                                    Id = ListUtils.ShortCutsList.Count + 1,
                                    SocialId = pageClass.PageId,
                                    Type = "Page",
                                    Name = pageClass.PageName,
                                    PageClass = pageClass,
                                });
                            }

                            break;
                        }
                    }
                } 
            } 
        }
 
        public static async Task GetLastArticles()
        {
            if (Methods.CheckConnectivity())
            {
                var (apiStatus, respond) = await RequestsAsync.Article.GetArticlesAsync("5");
                if (apiStatus != 200 || respond is not GetUsersArticlesObject result || result.Articles == null)
                {
                    //Methods.DisplayReportResult(this, respond);
                }
                else
                {
                    var respondList = result.Articles.Count;
                    ListUtils.ListCachedDataArticle = respondList switch
                    {
                        > 0 => new ObservableCollection<ArticleDataObject>(result.Articles),
                        _ => ListUtils.ListCachedDataArticle
                    };
                } 
            } 
        }
 
        /////////////////////////////////////////////////////////////////
        private static bool RunLogout;

        public static async void Delete(Activity context)
        {
            try
            {
                switch (RunLogout)
                {
                    case false:
                        RunLogout = true;

                        await RemoveData("Delete");

                        context?.RunOnUiThread(() =>
                        {
                            try
                            {
                                Methods.Path.DeleteAll_MyFolderDisk();

                                SqLiteDatabase dbDatabase = new SqLiteDatabase();
                                dbDatabase.DropAll();

                                Runtime.GetRuntime()?.RunFinalization();
                                Runtime.GetRuntime()?.Gc();
                                TrimCache(context);
                                 
                                ListUtils.ClearAllList();

                                UserDetails.ClearAllValueUserDetails();

                                dbDatabase.CheckTablesStatus();

                                context.StopService(new Intent(context, typeof(PostService)));
                                context.StopService(new Intent(context, typeof(PostApiService)));

                                MainSettings.SharedData?.Edit()?.Clear()?.Commit();
                                MainSettings.InAppReview?.Edit()?.Clear()?.Commit();
                                 
                                Intent intent = new Intent(context, typeof(LoginActivity)); 
                                intent.AddCategory(Intent.CategoryHome);
                                intent.SetAction(Intent.ActionMain);
                                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.ClearTask);
                                context.StartActivity(intent);
                                context.FinishAffinity();
                                context.Finish();
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        });

                        RunLogout = false;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static async void Logout(Activity context)
        {
            try
            {
                switch (RunLogout)
                {
                    case false:
                        RunLogout = true;

                        await RemoveData("Logout");
                     
                        context?.RunOnUiThread(() =>
                        {
                            try
                            {
                                Methods.Path.DeleteAll_MyFolderDisk();

                                SqLiteDatabase dbDatabase = new SqLiteDatabase();
                                dbDatabase.DropAll();

                                Runtime.GetRuntime()?.RunFinalization();
                                Runtime.GetRuntime()?.Gc();
                                TrimCache(context);
                                 
                                ListUtils.ClearAllList();

                                UserDetails.ClearAllValueUserDetails();

                                dbDatabase.CheckTablesStatus();
                         
                                context.StopService(new Intent(context, typeof(PostService)));
                                context.StopService(new Intent(context, typeof(PostApiService)));

                                MainSettings.SharedData?.Edit()?.Clear()?.Commit();
                                MainSettings.InAppReview?.Edit()?.Clear()?.Commit();
                                 
                                Intent intent = new Intent(context, typeof(LoginActivity)); 
                                intent.AddCategory(Intent.CategoryHome);
                                intent.SetAction(Intent.ActionMain);
                                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.ClearTask);
                                context.StartActivity(intent);
                                context.FinishAffinity();
                                context.Finish();
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        });
                     
                        RunLogout = false;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private static void TrimCache(Activity context)
        {
            try
            {
                File dir = context?.CacheDir;
                if (dir != null && dir.IsDirectory)
                {
                    DeleteDir(dir);
                }

                context?.DeleteDatabase(AppSettings.DatabaseName + "_.db");
                context?.DeleteDatabase(SqLiteDatabase.PathCombine);

                if (context?.IsDestroyed != false)
                    return;

                Glide.Get(context)?.ClearMemory();
                new Thread(() =>
                {
                    try
                    {
                        Glide.Get(context)?.ClearDiskCache();
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }).Start();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private static bool DeleteDir(File dir)
        {
            try
            {
                if (dir == null || !dir.IsDirectory) return dir != null && dir.Delete();
                string[] children = dir.List();
                if (children.Select(child => DeleteDir(new File(dir, child))).Any(success => !success))
                {
                    return false;
                }

                // The directory is now empty so delete it
                return dir.Delete();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }
        
        private static void Reset()
        {
            try
            {
                MentionActivity.MAdapter = null!;
                 
                Current.AccessToken = string.Empty;

                TabbedMainActivity.GetInstance()?.NewsFeedTab?.RemoveHandler();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private static async Task RemoveData(string type)
        {
            try
            {
                switch (type)
                {
                    case "Logout":
                    {
                        if (Methods.CheckConnectivity())
                        {
                            await RequestsAsync.Auth.DeleteTokenAsync();
                        }

                        break;
                    }
                    case "Delete":
                    {
                        Methods.Path.DeleteAll_FolderUser();

                        if (Methods.CheckConnectivity())
                        {
                            await RequestsAsync.Auth.DeleteUserAsync(UserDetails.Password);
                        }

                        break;
                    }
                }

                switch (AppSettings.ShowGoogleLogin)
                {
                    case true when LoginActivity.MGoogleSignInClient != null:
                    {
                        if (Auth.GoogleSignInApi != null)
                        {
                            LoginActivity.MGoogleSignInClient.SignOut();
                            LoginActivity.MGoogleSignInClient = null!;
                        }

                        break;
                    }
                }
                 
                switch (AppSettings.ShowFacebookLogin)
                {
                    case true:
                    {
                        var accessToken = AccessToken.CurrentAccessToken;
                        var isLoggedIn = accessToken != null && !accessToken.IsExpired;
                        switch (isLoggedIn)
                        {
                            case true when Profile.CurrentProfile != null:
                                LoginManager.Instance.LogOut();
                                break;
                        }

                        break;
                    }
                }

                OneSignalNotification.Un_RegisterNotificationDevice();

                ListUtils.ClearAllList();
                Reset();

                UserDetails.ClearAllValueUserDetails();

                Methods.DeleteNoteOnSD(); 

                GC.Collect();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }
}