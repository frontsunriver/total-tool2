<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use ZipArchive;
use Auth;

class PluginController extends Controller
{
	public function index()
	{
		if (!Auth()->user()->can('plugin.control')) {
			return abort(401);
		}
		$file = file_get_contents(base_path().'/am-content/Plugins/plugin.json');
		$plugins = json_decode($file, true);
		return view('admin.plugin.index',compact('plugins'));
	}

	public function active($plugin)
	{
    	// read file
		$data = file_get_contents(base_path().'/am-content/Plugins/plugin.json');

		// decode json to array
		$json_arr = json_decode($data, true);

		foreach ($json_arr as $key => $value) {
			if ($value['Text Domain'] == $plugin) {
				$json_arr[$key]['status'] = "active";
			}
		}

		// encode array to json and save to file
		file_put_contents(base_path().'/am-content/Plugins/plugin.json', json_encode($json_arr,JSON_PRETTY_PRINT));

		return back();
	}

	public function deactive($plugin)
	{
    	// read file
		$data = file_get_contents(base_path().'/am-content/Plugins/plugin.json');

		// decode json to array
		$json_arr = json_decode($data, true);

		foreach ($json_arr as $key => $value) {
			if ($value['Text Domain'] == $plugin) {
				$json_arr[$key]['status'] = "deactive";
			}
		}

		// encode array to json and save to file
		file_put_contents(base_path().'/am-content/Plugins/plugin.json', json_encode($json_arr,JSON_PRETTY_PRINT));

		return back();
	}


	public function upload(Request $request)
	{
		return back();
		$uploadedFile = $request->file('file');
		if (isset($uploadedFile)) {
			$filename = $uploadedFile->getClientOriginalName();

			$name = substr($filename, 0, -4);
			$path = base_path().'/am-content/Plugins/';

			$uploadedFile->move($path, $filename);
			$zip = new ZipArchive;
			$res = $zip->open(base_path().'/am-content/Plugins/'.$filename);
			if ($res === TRUE) {
				$zip->extractTo(base_path().'/am-content/Plugins/');
				$zip->close();
				unlink(base_path().'/am-content/Plugins/'.$filename);

				$inp = file_get_contents( base_path().'/am-content/Plugins/plugin.json');
				$tempArray = json_decode($inp,true);


				if (file_exists(base_path().'/am-content/Plugins/'.$name.'/single_plugin.json')) {
					$files = file_get_contents( base_path().'/am-content/Plugins/'.$name.'/single_plugin.json');
				}else{
					return back()->with('status','You plugin has many problem');
				}

				$data = json_decode($files, true);


				foreach ($tempArray as $key => $value) {
					if ($value['Text Domain'] == $data['Text Domain']) {
						return back()->with('status','This Plugin is already exists');
					}
				}

				$tempArray[] = $data;
				$final_data = json_encode($tempArray);


				file_put_contents(base_path().'/am-content/Plugins/plugin.json', $final_data);



				//helper function append
				$inps = file_get_contents( base_path().'/am-content/Plugins/Helper.json');
				$tempArrays = json_decode($inps,true);


				if (file_exists(base_path().'/am-content/Plugins/'.$name.'/single_helper.json')) {
					$get_file = file_get_contents( base_path().'/am-content/Plugins/'.$name.'/single_helper.json');
				}else{
					return back()->with('status','You plugin has many problem');
				}

				$datas = json_decode($get_file, true);

				$tempArrays[] = $datas;
				$final_datas = json_encode($tempArrays);


				file_put_contents(base_path().'/am-content/Plugins/helper.json', $final_datas);

				return back();

			}
		}
	}
}
