<?php
use App\Terms;
use App\Options;
use Amcoders\Lpress\Lphelper;

/*
replace image name via $name from $url
*/
function ImageSize($url,$name){
	$img_arr=explode('.', $url);
	$ext='.'.end($img_arr);
	$newName=str_replace($ext, $name.$ext, $url);
	return $newName;
}


 /**
 * genarate frontend menu.
 *
 * @param $position=menu position
 * @param $ul=ul class
 * @param $li=li class
 * @param $a=a class
 * @param $icon= position left/right
 * @param $lang= translate true or false
 */

function Menu($position,$ul='',$li='',$a='',$icon_position='top',$lang=false)
{
	return Lphelper::Menu($position,$ul,$li,$a,$icon_position,$lang);
}	

function ConfigCategory($type,$select = ''){
	return Lphelper::ConfigCategory($type,$select);  
	
}
/*
return total active language
*/
function adminLang($c='')
{
	return Lphelper::AdminLang($c);
}

function disquscomment()
{
	return Lphelper::Disqus();	 	
}

/*
return options value
*/
function LpOption($key,$array=false,$translate=false){
	if ($translate == true) {
		$data=Options::where('key',$key)->where('lang',Session::get('locale'))->select('value')->first();
		if (empty($data)) {
			$data=Options::where('key',$key)->select('value')->first();
			
		}
	}
	else{
		$data=Options::where('key',$key)->select('value')->first();
	}

	if ($array==true) {
		return json_decode($data->value,true);
	}
	return json_decode($data->value);
}

function Livechat($param)
{
	return Lphelper::TwkChat($param);  	
}


function total_tax()
{
	$tax = Options::where('key','tax')->first();
	if($tax)
	{
		Cart::setGlobalTax($tax->value);

		return true;
	}else{
		return 0;
	}
}

function mediasingle()
{
	 if (Auth::User()->role->id == 3) {
	 	 $medialimit= true; 
     
      }
     else{
          $medialimit= true; 

     }

	return view('admin.media.mediamodal',compact('medialimit'));
}

function input($array = [])
{
	$title = $array['title'] ?? 'title'; 
	$type = $array['type'] ?? 'text'; 
	$placeholder = $array['placeholder'] ?? ''; 
	$name = $array['name'] ?? 'name'; 
	$id = $array['id'] ?? ''; 
	$value = $array['value'] ?? ''; 
	if (isset($array['is_required'])) {
		$required = $array['is_required']; 
	}
	else{
		$required = false; 
	}
	return view('components.input',compact('title','type','placeholder','name','id','value','required'));
}

function textarea($array = [])
{
	$title=$array['title'] ?? '';
	$id=$array['id'] ?? '';
	$name=$array['name'] ?? '';
	$placeholder=$array['placeholder'] ?? '';
	$maxlength=$array['maxlength'] ?? '';
	$cols=$array['cols'] ?? 30;
	$rows=$array['rows'] ?? 3;
	$class=$array['class'] ?? '';
	$value=$array['value'] ?? '';
	$is_required=$array['is_required'] ?? false;
	return view('components.textarea',compact('title','placeholder','name','id','value','is_required','class','cols','rows','maxlength'));
}

function editor($array = [])
{
	$title=$array['title'] ?? '';
	$id=$array['id'] ?? 'content';
	$name=$array['name'] ?? '';
	$cols=$array['cols'] ?? 30;
	$rows=$array['rows'] ?? 10;
	$class=$array['class'] ?? '';
	$value=$array['value'] ?? '';
	
	return view('components.editor',compact('title','name','id','value','class','cols','rows'));
}

function mediasection($array = [])
{
	$title=$array['title'] ?? 'Image';
	$preview_id=$array['preview_id'] ?? 'preview';
	$preview=$array['preview'] ?? 'admin/img/img/placeholder.png';
	$input_id=$array['input_id'] ?? 'preview_input';
	$input_class=$array['input_class'] ?? 'input_image';
	$input_name=$array['input_name'] ?? 'preview';
	$value=$array['value'] ?? '';
	return view('admin.media.section',compact('title','preview_id','preview','input_id','input_class','input_name','value'));
}

function mediasectionmulti($array = [])
{
	$title=$array['title'] ?? 'Image';
	$preview_id=$array['preview_id'] ?? 'preview';
	$preview=$array['preview'] ?? '';
	$input_id=$array['input_id'] ?? 'preview_input';
	$input_class=$array['input_class'] ?? 'input_image';
	$input_name=$array['input_name'] ?? 'preview';
	$area_id=$array['area_id'] ?? 'gallary-img';
	$value=$array['value'] ?? '';
	return view('admin.media.multisection',compact('title','preview_id','preview','input_id','input_class','input_name','value','area_id'));
}



function mediamulti()
{
	return view('admin.media.multiplemediamodel');
}
/*
return posts array
*/
function LpPosts($arr){
	
	$type=$arr['type'];
	$relation=$arr['with'] ?? '';
	$order=$arr['order'] ?? 'DESC';
	$limit=$arr['limit'] ?? null;
	$lang=$arr['translate'] ?? true;

	if (!empty($relation)) {
		if (empty($limit)) {
			if ($lang==true) {
				$data=Terms::with($relation)->where('type',$type)->where('status',1)->orderBy('id',$order)->where('lang',Session::get('locale'))->get();
				
			}
			else{
				$data=Terms::with($relation)->where('type',$type)->where('status',1)->orderBy('id',$order)->where('lang','en')->get();
			}
			
		}
		else{
			if ($lang==true) {
				$data=Terms::with($relation)->where('type',$type)->where('status',1)->where('lang',Session::get('locale'))->orderBy('id',$order)->paginate($limit);
			}
			else{
				$data=Terms::with($relation)->where('type',$type)->where('status',1)->where('lang','en')->orderBy('id',$order)->paginate($limit);
			}
			
		}

	}
	else{
		if (empty($limit)) {
			if ($lang==true) {
				$data=Terms::where('type',$type)->where('status',1)->where('lang',Session::get('locale'))->orderBy('id',$order)->get();
			}		
			else {
				$data=Terms::where('type',$type)->where('status',1)->where('lang','en')->orderBy('id',$order)->get();

			}


		}
		else{
			if ($lang==true) {
				$data=Terms::where('type',$type)->where('status',1)->where('lang',Session::get('locale'))->orderBy('id',$order)->paginate($limit);
			}
			else {
				$data=Terms::where('type',$type)->where('status',1)->where('lang','en')->orderBy('id',$order)->paginate($limit);


			}

		}
	}

	return $data;
}


function currency_name()
{
	return Cache::remember('currency_name', 200,  function () {
		$info=Options::where('key','currency_name')->first();
		return $info->value;
	});
	
}

function currency_icon()
{
	return Cache::remember('currency_icon', 200,  function () {
		$info=Options::where('key','currency_icon')->first();
		return $info->value;
	});
	
}

/*
return admin category
*/

function  AdminCategory($type)
{
	 return Lphelper::LPAdminCategory($type);  
}

/*
return category selected
*/

function AdminCategoryUpdate($type,$arr = [],$role=false){
	 return Lphelper::LPAdminCategoryUpdate($type,$arr,$role);
}



function theme_asset($path)
{
	return asset('script/am-content/Themes/'.$path);
}

function plugin_asset($path)
{
	return asset('script/am-content/Plugins/'.$path);
}

function make_token($token)
{
	return base64_decode(base64_decode(base64_decode($token)));
}

function content($main_id,$id,$get_id=null)
{

	$theme_json_file_get = file_get_contents( base_path().'/am-content/Themes/theme.json');
	$themes = json_decode($theme_json_file_get, true);
	foreach ($themes as $theme) {
		if ($theme['status'] == 'active') {
			$active_theme = $theme['Text Domain'];
		}
	}

	$customizer = App\Customizer::where([
		['key', $main_id],
		['theme_name', $active_theme],
	])->first();

	if ($customizer) {
		$main_value = json_decode($customizer->value);

		if ($get_id == null) {
			$check_id = json_decode($customizer->value);
			if (isset($check_id->settings->$id)) {

				if (Session::has('locale')) {
					if(Session::get('locale') == 'en'){
						if ($customizer->status == 0) {
							$value = json_decode($customizer->value);
							return $value->settings->$id->old_value;
						}else{
							$value = json_decode($customizer->value);
							return $value->settings->$id->new_value;
						}
					}else{
						if (file_exists(base_path().'/resources/lang/'.Session::get('locale').'.json')) {
						$lang_file = file_get_contents( base_path().'/resources/lang/'.Session::get('locale').'.json');
							
						}
						else{
						$lang_file = file_get_contents( base_path().'/resources/lang/en.json');

						}
						$lang_data = json_decode($lang_file);

						if (isset($lang_data->$id)) {
							return $lang_data->$id;
						}
					}
				}

				if ($customizer->status == 0) {
					$value = json_decode($customizer->value);
					return $value->settings->$id->old_value;
				}else{
					$value = json_decode($customizer->value);
					return $value->settings->$id->new_value;
				}

			}
		}


		if ($get_id != null) {
			$check_get_id  = json_decode($customizer->value);
			if (isset($check_get_id->content->$get_id->$id)) {

				if (Session::has('locale')) {
					if(Session::get('locale') == 'en')
					{
						if ($customizer->status == 0) {
							$value = json_decode($customizer->value);
							return $value->content->$get_id->$id->old_value;
						}else{
							$value = json_decode($customizer->value);
							return $value->content->$get_id->$id->new_value;
						}
					}else{
						if (file_exists(base_path().'/resources/lang/'.Session::get('locale').'.json')) {
						$lang_file = file_get_contents( base_path().'/resources/lang/'.Session::get('locale').'.json');
						}
						else{
						$lang_file = file_get_contents( base_path().'/resources/lang/en.json');

						}
						$lang_data = json_decode($lang_file);

						if (isset($lang_data->$id)) {
							return $lang_data->$id;
						}
					}

				}

				if ($customizer->status == 0) {
					$value = json_decode($customizer->value);
					return $value->content->$get_id->$id->old_value;
				}else{
					$value = json_decode($customizer->value);
					return $value->content->$get_id->$id->new_value;
				}

			}

		}
	}

	
}



function GetThemeRoot()
{
	$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
	$themes = json_decode($file, true);
	foreach ($themes as $theme) {
		if ($theme['status'] == 'active') {
			$customizer_file = file_get_contents( base_path().'/am-content/Themes/'.$theme['Text Domain'].'/customizer.json');
			$active_theme = json_decode($customizer_file,true);
			foreach ($active_theme as $key => $value) {
				if ($value['status'] == 'active') {
					$root = base_path().'/am-content/Themes/'.$theme['Text Domain'].'/';
					break;

				}
			}
		}
	}

	$dir= $root ?? "lp";

	return $dir;
}


function put($content,$root)
{
	$content=file_get_contents($content);
	File::put($root,$content);
}


function AdminSidebar()
{
	if(file_exists(base_path().'/am-content/Plugins/menuregister.php')){
      include_once(base_path().'/am-content/Plugins/menuregister.php');
      if(function_exists('RegisterAdminMenuBar')){
      	$dyanmicMenu=RegisterAdminMenuBar();
      }
      
    }
    return $dyanmicMenu ?? [];


}
