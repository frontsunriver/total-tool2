<?php

namespace App\Http\Controllers;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Category;

class LocalizationController extends Controller
{
    public function store(Request $request)
    {
      $theme_file = file_get_contents(base_path().'/am-content/Themes/theme.json');
      $themes = json_decode($theme_file,true);

      foreach($themes as $theme)
      {
        if($theme['status'] == 'active')
        {
          $theme_name = $theme['Text Domain'];
        }
      }
      
      $locale = $request->locale;
      $languages = Category::where([
        ['slug',$locale],
        ['name',$theme_name],
        ['type','lang']
      ])->first();
      
      $info = json_decode($languages->content);

      \Session::put('lang_position',$info->lang_position);

    	$locale = $request->locale;
    	\Session::put('locale',$locale);
		  return "ok";
    }
}
