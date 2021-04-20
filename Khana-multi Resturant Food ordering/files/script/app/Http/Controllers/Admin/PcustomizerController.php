<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Carbon\Carbon;
use App\Customizer;
use App\Lpcustomize;
use App\Options;

class PcustomizerController extends Controller
{
	public function index(Request $request)
	{
		if (!empty($request->lang) && !empty($request->id) && !empty($request->id)) {
			return $request;
		}
		
		if ($request->page) {
			 $data= Lpcustomize::options($request->page);
			 $activePage = $data['data']['page_url'];
			 $currentpage = $request->page;
			 $pagecontent=Lpcustomize::$PageContent;

			 $options['pages']=$data['pages'];

		    return view('admin.pcustomizer.index',compact('options','pagecontent','activePage','currentpage'));

		}
		 $options=Lpcustomize::options();
		 $pagecontent=Lpcustomize::$PageContent;
		 $page_key=Lpcustomize::$key;

		 if ($options != false) {
		 	
			return view('admin.pcustomizer.index',compact('options','pagecontent','page_key'));
		 	
		 }		
	}



	public function section_option(Request $request)
	{

		if (!empty($request->id) && !empty($request->page_name)) {

			$options= Options::where('key',$request->id)->first();
			$json=json_decode($options->value ?? '',true);
			
			$options= Lpcustomize::options($request->page_name,$request->id);
			$key=$request->id;


			$languages=Options::where('key','langlist')->first();
			$languages=json_decode($languages->value ?? ['en']);

			$lang_key=$languages->key ?? 'en';

			return view('admin.pcustomizer.details',compact('options','json','key','languages','lang_key'));

		}
	}


	public function value_update(Request $request)
	{
		$language='en';//session language will append here
		$options= Options::where('key',$request->key)->where('lang',$language)->first();
		if (empty($options)) {
		$options = new 	Options;
		$options->key=$request->key;
		$options->lang=$language;
		}
		$options->value=json_encode($request->all());
		$options->save();
		return response()->json(['Save Success']);
	}


	
}
