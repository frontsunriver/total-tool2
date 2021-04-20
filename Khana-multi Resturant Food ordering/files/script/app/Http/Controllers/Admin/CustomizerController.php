<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Carbon\Carbon;
use App\Customizer;
use Illuminate\Support\Arr;
use App\Media;
use File;
use Auth;

class CustomizerController extends Controller
{
	public function index()
	{
		$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
		$themes = json_decode($file, true);
		foreach ($themes as $theme) {
			if ($theme['status'] == 'active') {
				$customizer_file = file_get_contents( base_path().'/am-content/Themes/'.$theme['Text Domain'].'/customizer.json');
				$active_theme = json_decode($customizer_file,true);
				foreach ($active_theme as $key => $value) {
					if ($value['status'] == 'active') {
						$main_file = file_get_contents( base_path().'/am-content/Themes/'.$theme['Text Domain'].'/views/'.$value['file_location']);
						$sidebar_options = json_decode($main_file,true);

					}
				}
			}
		}
		return view('admin.customizer.index',compact('active_theme','sidebar_options'));
	}

	public function page_change(Request $request)
	{
		$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
		$themes = json_decode($file, true);
		foreach ($themes as $theme) {
			if ($theme['status'] == 'active') {
				$customizer_file = file_get_contents( base_path().'/am-content/Themes/'.$theme['Text Domain'].'/customizer.json');
				$active_theme = json_decode($customizer_file,true);

				foreach ($active_theme as $key=> $value) {
					if ($value['status'] == 'active') {
						$active_theme[$key]['status'] = "deactive";
					}
					if ($value['page_name'] == $request->page_name) {
						$active_theme[$key]['status'] = "active";
					}
				}
				file_put_contents(base_path().'/am-content/Themes/'.$theme['Text Domain'].'/customizer.json', json_encode($active_theme,JSON_PRETTY_PRINT));
				return "ok";
			}
		}
		
	}

	public function section_option(Request $request)
	{
		$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
		$themes = json_decode($file, true);
		foreach ($themes as $theme) {
			if ($theme['status'] == 'active') {
				$customizer_file = file_get_contents( base_path().'/am-content/Themes/'.$theme['Text Domain'].'/customizer.json');
				$active_theme = json_decode($customizer_file,true);
				foreach ($active_theme as $key => $value) {
					if ($value['status'] == 'active') {
						$main_file = file_get_contents( base_path().'/am-content/Themes/'.$theme['Text Domain'].'/views/'.$value['file_location']);
						$sidebar_options = json_decode($main_file,true);

						foreach ($sidebar_options as $key => $option) {
							if ($option['id'] == $request->id) {
								return view('admin.customizer.details',compact('option'));
							}
						}

					}
				}
			}
		}
	}


	public function value_update(Request $request)
	{


		if ($request->option == 'key') {
			$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
			$themes = json_decode($file, true);
			foreach ($themes as $theme) {
				if ($theme['status'] == 'active') {
					$active_theme = $theme['Text Domain'];
				}
			}
			$info_key = Customizer::where([
				['key', $request->main_id],
				['theme_name', $active_theme],
			])->first();

			if ($info_key) {
				$old_value = json_decode($info_key->value,true);
				$main = $request->id;
				if (!isset($old_value['settings'][$main])) {
					$data = [
							'old_value' => null,
							'new_value' => $request->value
					];
					$result = data_fill($old_value, 'settings.'.$request->id, $data);
				}else{
					$man = $old_value['settings'][$request->id];
					foreach ($old_value['settings'] as $key => $value) {
						$old_value['settings'][$request->id]['old_value'] =  $man['new_value'];
						$old_value['settings'][$request->id]['new_value'] =  $request->value;
					}
					$result = $old_value;
				}

				$info_key->value = json_encode($result);
				$info_key->status = 0;
				$info_key->save();
				return $info_key;
			}else{
				
				

				$a = [
					$request->id => [
						'old_value' => null,
						'new_value' => $request->value
					]
				];


				$data = [];

				$result = data_fill($data, 'settings', $a);

				$customizer = new Customizer();
				$customizer->key = $request->main_id;
				$customizer->theme_name = $active_theme;
				$customizer->value = json_encode($result);
				$customizer->status = 0;
				$customizer->save();
				return $customizer;

			}
		}
	}


	public function multiple_settings_option(Request $request)
	{

		if ($request->option == 'content') {
			$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
			$themes = json_decode($file, true);
			foreach ($themes as $theme) {
				if ($theme['status'] == 'active') {
					$active_theme = $theme['Text Domain'];
				}
			}
			$info_key = Customizer::where([
				['key', $request->main_id],
				['theme_name', $active_theme],
			])->first();

			if ($info_key) {
				$old_value = json_decode($info_key->value,true);
				$main = $request->p_id;
				if (!isset($old_value['content'][$main][$request->id])) {
					$data = [
							'old_value' => null,
							'new_value' => $request->value
					
					];

					 $result = data_fill($old_value, 'content.'.$main.'.'.$request->id, $data);
				}else{
					 $man = $old_value['content'][$request->p_id][$request->id];
					foreach ($old_value as $key => $value) {
						$old_value['content'][$request->p_id][$request->id]['old_value'] =  $man['new_value'];
						$old_value['content'][$request->p_id][$request->id]['new_value'] =  $request->value;
					}
					 $result = $old_value;
				}

				$info_key->value = json_encode($result);
				$info_key->status = 0;
				$info_key->save();
				return $info_key;
			}else{


				$a = [
					$request->p_id => [
						$request->id => [
							'old_value' => null,
							'new_value' => $request->value
						]
					]
				];


				$data = [];

				$result = data_fill($data, 'content', $a);
				$customizer = new Customizer();
				$customizer->key = $request->main_id;
				$customizer->theme_name = $active_theme;
				$customizer->value = json_encode($result);
				$customizer->status = 0;
				$customizer->save();
				return $customizer;

			}
		}
	}


	public function image_upload(Request $request)
	{
		
		$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
		$themes = json_decode($file, true);
		foreach ($themes as $theme) {
			if ($theme['status'] == 'active') {
				$active_theme_name = $theme['Text Domain'];
			}
		}

		$file = $request->file('img'.$request->id);
		if (isset($file)) {
			$curentdate = Carbon::now()->toDateString();
			$imagename =  $curentdate . '-' . uniqid() . '.' . $file->getClientOriginalExtension();


			$path = 'uploads/';

			$file->move($path, $imagename);

			$file_name = $path.$imagename;

			$main_file_path = 'uploads/'. $imagename;

			$media = new Media();
			$media->user_id = Auth::User()->id;
			$media->name = $path.$imagename;
			$media->type = $file->getClientOriginalExtension();
			$schemeurl=parse_url(url('/'));
            if ($schemeurl['scheme']=='https') {
               $url=substr(url('/'), 6);
            }
            else{
                 $url=substr(url('/'), 5);
            }
			$media->url = $url.'/'.$path.$imagename;
			$media->size = File::size('uploads/'.$imagename).'kib';
			$media->path = 'uploads/';
			$media->save();

		}else{
			$imagename = 'default.png';
		}

		

		if ($request->option == 'key') {
			$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
			$themes = json_decode($file, true);
			foreach ($themes as $theme) {
				if ($theme['status'] == 'active') {
					$active_theme = $theme['Text Domain'];
				}
			}
			$info_key = Customizer::where([
				['key', $request->main_id],
				['theme_name', $active_theme],
			])->first();

			if ($info_key) {
				$old_value = json_decode($info_key->value,true);
				$main = $request->id;
				if (!isset($old_value['settings'][$main])) {
					$data = [
							'old_value' => null,
							'new_value' => $main_file_path
					];
					$result = data_fill($old_value, 'settings.'.$request->id, $data);
				}else{
					$man = $old_value['settings'][$request->id];
					foreach ($old_value['settings'] as $key => $value) {
						$old_value['settings'][$request->id]['old_value'] =  $man['new_value'];
						$old_value['settings'][$request->id]['new_value'] =  $main_file_path;
					}
					$result = $old_value;
				}

				$info_key->value = json_encode($result);
				$info_key->status = 0;
				$info_key->save();
				return $info_key;
			}else{
				
				

				$a = [
					$request->id => [
						'old_value' => null,
						'new_value' => $main_file_path
					]
				];


				$data = [];

				$result = data_fill($data, 'settings', $a);

				$customizer = new Customizer();
				$customizer->key = $request->main_id;
				$customizer->theme_name = $active_theme;
				$customizer->value = json_encode($result);
				$customizer->status = 0;
				$customizer->save();
				return $customizer;

			}
		}


		if ($request->option == 'content') {
			$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
			$themes = json_decode($file, true);
			foreach ($themes as $theme) {
				if ($theme['status'] == 'active') {
					$active_theme = $theme['Text Domain'];
				}
			}
			$info_key = Customizer::where([
				['key', $request->main_id],
				['theme_name', $active_theme],
			])->first();

			if ($info_key) {
				$old_value = json_decode($info_key->value,true);
				$main = $request->p_id;
				if (!isset($old_value['content'][$main][$request->id])) {
					$data = [
							'old_value' => null,
							'new_value' => $main_file_path
					
					];

					 $result = data_fill($old_value, 'content.'.$main.'.'.$request->id, $data);
				}else{
					 $man = $old_value['content'][$request->p_id][$request->id];
					foreach ($old_value as $key => $value) {
						$old_value['content'][$request->p_id][$request->id]['old_value'] =  $man['new_value'];
						$old_value['content'][$request->p_id][$request->id]['new_value'] =  $main_file_path;
					}
					 $result = $old_value;
				}

				$info_key->value = json_encode($result);
				$info_key->status = 0;
				$info_key->save();
				return $info_key;
			}else{


				$a = [
					$request->p_id => [
						$request->id => [
							'old_value' => null,
							'new_value' => $main_file_path
						]
					]
				];


				$data = [];

				$result = data_fill($data, 'content', $a);
				$customizer = new Customizer();
				$customizer->key = $request->main_id;
				$customizer->theme_name = $active_theme;
				$customizer->value = json_encode($result);
				$customizer->status = 0;
				$customizer->save();
				return $customizer;

			}
		}


	}


	public function save(Request $request)
	{
		
		$customizers = Customizer::all();

		foreach ($customizers as $key => $value) {
			$value->status = 1;
			$value->save();
		}

		return $customizers;
	}
}
