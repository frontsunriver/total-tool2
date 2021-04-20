using System;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;

namespace WoWonder.Helpers.Controller
{
    public class CategoriesController
    { 
        public static ObservableCollection<Classes.Categories> ListCategoriesPage = new ObservableCollection<Classes.Categories>();
        public static ObservableCollection<Classes.Categories> ListCategoriesGroup = new ObservableCollection<Classes.Categories>();
        public static ObservableCollection<Classes.Categories> ListCategoriesBlog = new ObservableCollection<Classes.Categories>();
        public static ObservableCollection<Classes.Categories> ListCategoriesProducts = new ObservableCollection<Classes.Categories>();
        public static ObservableCollection<Classes.Categories> ListCategoriesJob = new ObservableCollection<Classes.Categories>();
        public static ObservableCollection<Classes.Categories> ListCategoriesMovies = new ObservableCollection<Classes.Categories>();

        public string Get_Translate_Categories_Communities(string idCategory, string textCategory , string type)
        {
            try
            {
                string categoryName = textCategory;

                switch (type)
                {
                    case "Page":
                    {
                        categoryName = ListCategoriesPage?.Count switch
                        {
                            > 0 => ListCategoriesPage.FirstOrDefault(a => a.CategoriesId == idCategory)
                                ?.CategoriesName ?? textCategory,
                            _ => categoryName
                        };

                        break;
                    }
                    case "Group":
                    {
                        categoryName = ListCategoriesGroup?.Count switch
                        {
                            > 0 => ListCategoriesGroup.FirstOrDefault(a => a.CategoriesId == idCategory)
                                ?.CategoriesName ?? textCategory,
                            _ => categoryName
                        };

                        break;
                    }
                    case "Blog":
                    {
                        categoryName = ListCategoriesBlog?.Count switch
                        {
                            > 0 => ListCategoriesBlog.FirstOrDefault(a => a.CategoriesId == idCategory)
                                ?.CategoriesName ?? textCategory,
                            _ => categoryName
                        };

                        break;
                    }
                    case "Products":
                    {
                        categoryName = ListCategoriesProducts?.Count switch
                        {
                            > 0 => ListCategoriesProducts.FirstOrDefault(a => a.CategoriesId == idCategory)
                                ?.CategoriesName ?? textCategory,
                            _ => categoryName
                        };

                        break;
                    }
                    case "Job":
                    {
                        categoryName = ListCategoriesJob?.Count switch
                        {
                            > 0 => ListCategoriesJob.FirstOrDefault(a => a.CategoriesId == idCategory)
                                ?.CategoriesName ?? textCategory,
                            _ => categoryName
                        };

                        break;
                    }
                    case "Movies":
                    {
                        categoryName = ListCategoriesMovies?.Count switch
                        {
                            > 0 => ListCategoriesMovies.FirstOrDefault(a => a.CategoriesId == idCategory)
                                ?.CategoriesName ?? textCategory,
                            _ => categoryName
                        };

                        break;
                    }
                    default:
                        categoryName = Application.Context.GetText(Resource.String.Lbl_Unknown);
                        break;
                }

                if (string.IsNullOrEmpty(categoryName))
                    return Application.Context.GetText(Resource.String.Lbl_Unknown);
                 
                return categoryName; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);

                if (string.IsNullOrEmpty(textCategory))
                    return Application.Context.GetText(Resource.String.Lbl_Unknown);

                return textCategory;
            }
        } 
    }
}