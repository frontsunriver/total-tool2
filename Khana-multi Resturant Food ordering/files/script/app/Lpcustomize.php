<?php 
namespace App;
use App\Options;
class Lpcustomize 
{
	protected static $found=false;
	public static $textdomain;
	public static $PageContent;
	public static $key;
	

	/**
	*
	* @return register pages 
	*/
	public static function options($key='',$page_name='')
	{

		$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
		$themes = json_decode($file, true);
		foreach ($themes as $theme) {
			if ($theme['status'] == 'active') {
				Lpcustomize::$textdomain=$theme['Text Domain'];

				if (file_exists(base_path().'/am-content/Themes/'.Lpcustomize::$textdomain.'/functions.php')) {
				include base_path().'/am-content/Themes/'.Lpcustomize::$textdomain.'/functions.php';
				Lpcustomize::$found=true;
				break;
				}
				else{
					Lpcustomize::$found=false;
				}
			}
		}

		if (!empty($key) && !empty($page_name)) {
			$page=RegisterPages();
			$data = $page[$key];
			if (file_exists(base_path().'/am-content/Themes/'.Lpcustomize::$textdomain.$data['file_location'])) {
				include base_path().'/am-content/Themes/'.Lpcustomize::$textdomain.$data['file_location'];
			}
			
			$value= customizeOptions();
			
			return $value[$page_name];
			
		}


		elseif (!empty($key)) {
			$page=RegisterPages();
			$data = $page[$key];
			if (file_exists(base_path().'/am-content/Themes/'.Lpcustomize::$textdomain.$data['file_location'])) {
				include base_path().'/am-content/Themes/'.Lpcustomize::$textdomain.$data['file_location'];
			}
			Lpcustomize::$PageContent = customizeOptions();

			$arr['data']=$data;
			$arr['pages']=$page;
			return $arr;
		}

			
		if (Lpcustomize::$found==true) {
			foreach (RegisterPages() as $key => $value) {
				
				if (file_exists(GetThemeRoot().$value['file_location'])) {
				
					Lpcustomize::$key = $key;
					include GetThemeRoot().$value['file_location'];
					break;
				}
			}
			
			Lpcustomize::$PageContent = customizeOptions();
			$options['pages']=RegisterPages();

			return $options;
		}
		else{
			return false;
		}
	}




	



	

	
}





		







