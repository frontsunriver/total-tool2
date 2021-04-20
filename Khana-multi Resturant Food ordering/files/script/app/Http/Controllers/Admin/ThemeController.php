<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use ZipArchive;
use Auth;
class ThemeController extends Controller
{
	public function index(Request $request)
	{
		 if (!Auth()->user()->can('theme')) {
		 	return abort(401);
		 }	

		$file = file_get_contents(base_path().'/am-content/Themes/theme.json');
		$themes = json_decode($file, true);
		return view('admin.theme.index',compact('themes'));
	}

	public function active($name)
	{


		// read file
		$data = file_get_contents(base_path().'/am-content/Themes/theme.json');

		// decode json to array
		$json_arr = json_decode($data, true);

		foreach ($json_arr as $key => $value) {
			if ($value['status'] == 'active') {
				$json_arr[$key]['status'] = "deactive";
			}
			if ($value['Text Domain'] == $name) {
				$json_arr[$key]['status'] = "active";
			}
		}

		// encode array to json and save to file
		file_put_contents(base_path().'/am-content/Themes/theme.json', json_encode($json_arr,JSON_PRETTY_PRINT));


		if (file_exists(base_path().'/am-content/Themes/'.$name.'/single_sidebar.json')) {
			$get_files = file_get_contents( base_path().'/am-content/Themes/'.$name.'/single_sidebar.json');
		}else{
			return back()->with('status','You theme has many problem');
		}

		$final_dataes = json_decode($get_files, true);


		file_put_contents(base_path().'/am-content/Themes/Sidebar.json', json_encode($final_dataes,JSON_PRETTY_PRINT));

		return back();
	}


	public function upload(Request $request)
	{

		return back();
		
		$uploadedFile = $request->file('file');
		if (isset($uploadedFile)) {
			$filename = $uploadedFile->getClientOriginalName();

			$name = substr($filename, 0, -4);
			$path = base_path().'/am-content/Themes/';

			$uploadedFile->move($path, $filename);
			$zip = new ZipArchive;
			$res = $zip->open(base_path().'/am-content/Themes/'.$filename);
			if ($res === TRUE) {
				$zip->extractTo(base_path().'/am-content/Themes/');
				$zip->close();
				unlink(base_path().'/am-content/Themes/'.$filename);

				$inp = file_get_contents( base_path().'/am-content/Themes/theme.json');
				$tempArray = json_decode($inp,true);


				if (file_exists(base_path().'/am-content/Themes/'.$name.'/single_theme.json')) {
					$files = file_get_contents( base_path().'/am-content/Themes/'.$name.'/single_theme.json');
				}else{
					return back()->with('status','You theme has many problem');
				}

				$data = json_decode($files, true);


				foreach ($tempArray as $key => $value) {
					if ($value['Text Domain'] == $data['Text Domain']) {
						return back()->with('status','This theme is already exists');
					}
				}

				$tempArray[] = $data;
				$final_data = json_encode($tempArray,JSON_PRETTY_PRINT);


				file_put_contents(base_path().'/am-content/Themes/theme.json', $final_data);



				//helper function append
				$inps = file_get_contents( base_path().'/am-content/Themes/Helper.json');
				$tempArrays = json_decode($inps,true);


				if (file_exists(base_path().'/am-content/Themes/'.$name.'/single_helper.json')) {
					$get_file = file_get_contents( base_path().'/am-content/Themes/'.$name.'/single_helper.json');
				}else{
					return back()->with('status','You theme has many problem');
				}

				$datas = json_decode($get_file, true);

				$tempArrays[] = $datas;
				return $final_datas = json_encode($tempArrays,JSON_PRETTY_PRINT);


				file_put_contents(base_path().'/am-content/Themes/helper.json', $final_datas);


				//sidebar add
				 

				if (file_exists(base_path().'/am-content/Themes/'.$name.'/single_sidebar.json')) {
					$get_files = file_get_contents( base_path().'/am-content/Themes/'.$name.'/single_sidebar.json');
				}else{
					return back()->with('status','You theme has many problem');
				}

				$final_dataes = json_decode($get_files, true);


				file_put_contents(base_path().'/am-content/Themes/Sidebar.json', json_encode($final_dataes,JSON_PRETTY_PRINT));


				// File Active
				$data = file_get_contents(base_path().'/am-content/Themes/theme.json');

				// decode json to array
				$json_arr = json_decode($data, true);

				foreach ($json_arr as $key => $value) {
					if ($value['status'] == 'active') {
						$json_arr[$key]['status'] = "deactive";
					}
					if ($value['Text Domain'] == $name) {
						$json_arr[$key]['status'] = "active";
					}
				}

				// encode array to json and save to file
				file_put_contents(base_path().'/am-content/Themes/theme.json', json_encode($json_arr,JSON_PRETTY_PRINT));

				return back();

			}
		}
	}
}
