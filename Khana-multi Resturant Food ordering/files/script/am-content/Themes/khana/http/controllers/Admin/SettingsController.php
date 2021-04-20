<?php 

namespace Amcoders\Theme\khana\http\controllers\Admin;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Options;
use Auth;
class SettingsController extends controller
{
	public function index()
	{
		if (!Auth()->user()->can('site.settings')) {
			return abort(401);
		}
		$icon=Options::where('key','currency_icon')->first();
		$name=Options::where('key','currency_name')->first();
		$km_rate=Options::where('key','km_rate')->first();
		$com=Options::where('key','rider_commission')->first();
		$info=Options::where('key','system_basic_info')->first();
		$json=Options::where('key','default_map')->first();
		$json=json_decode($json->value);
		$info=json_decode($info->value);
		$tax = Options::where('key','tax')->first();
		if($tax)
		{
			$total_tax = $tax->value; 
		}else{
			$total_tax = 0; 
		}

		$color = Options::where('key','color')->first();
		if($color)
		{
			$theme_color = $color->value;
		}else{
			$theme_color = '#FF3252';
		}
		
		return view('theme::admin.settings.index',compact('icon','name','com','info','km_rate','json','total_tax','theme_color'));
	}

	public function update(Request $request)
	{

		if ($request->favicon) {
			$validatedData = $request->validate([
				'favicon' => 'max:20|mimes:ico',
			]);
			$favicon = 'favicon.'.$request->favicon->extension();  
			$request->favicon->move('uploads',$favicon);

		}
		if ($request->lazyload_40x40) {
			$validatedData = $request->validate([
				'lazyload_40x40' => 'max:20|mimes:png',
			]);
			$favicon = 'lazyload-40x40.'.$request->lazyload_40x40->extension();  
			$request->lazyload_40x40->move('uploads',$favicon);
		}
		if ($request->lazyload_138x135) {
			$validatedData = $request->validate([
				'lazyload_138x135' => 'max:20|mimes:png',
			]);
			$favicon = 'lazyload-138x135.'.$request->lazyload_138x135->extension();  
			$request->lazyload_138x135->move('uploads',$favicon);
		}

		if ($request->lazyload_250x186) {
			$validatedData = $request->validate([
				'lazyload_250x186' => 'max:20|mimes:png',
			]);
			$favicon = 'lazyload-250x186.'.$request->lazyload_250x186->extension();  
			$request->lazyload_250x186->move('uploads',$favicon);
		}

		if ($request->store) {
			$validatedData = $request->validate([
				'store' => 'max:100|image',
			]);
			$favicon = 'store.jpg';  
			$request->store->move('uploads',$favicon);
		}

		if ($request->login_bg) {
			$validatedData = $request->validate([
				'login_bg' => 'max:500|image',
			]);
			$login = 'login-bg.jpg';  
			$request->login_bg->move('admin',$login);
		}


		$currencyicon=Options::where('key','currency_icon')->first();
		$currencyicon->value=$request->currency_icon;
		$currencyicon->save();

		$name=Options::where('key','currency_name')->first();
		$name->value=strtoupper($request->currency_name);
		$name->save();

		$km_rate=Options::where('key','km_rate')->first();
		$km_rate->value=strtoupper($request->km_rate);
		$km_rate->save();

		$commision=Options::where('key','rider_commission')->first();
		$commision->value=$request->rider_commission;
		$commision->save();

		$json=Options::where('key','default_map')->first();
		$data['default_lat']=$request->default_lat;
		$data['default_long']=$request->default_long;
		$data['default_zoom']=$request->default_zoom;
		$json->value=json_encode($data);
		$json->save();

		$info=Options::where('key','system_basic_info')->first();
		$basic['number']=$request->number;
		$basic['logo']=$request->preview;
		$info->value=json_encode($basic);
		$info->save();

		if($request->tax)
		{
			$tax = Options::where('key','tax')->first();
			if($tax)
			{
				$tax->value = $request->tax;
				$tax->save();
			}else{
				$tax = new Options();
				$tax->key = 'tax';
				$tax->value = $request->tax;
				$tax->save();
			}
		}

		if($request->color)
		{
			$color = Options::where('key','color')->first();
			if($color)
			{
				$color->value = $request->color;
				$color->save();
			}else{
				$color = new Options();
				$color->key = 'color';
				$color->value = $request->color;
				$color->save();
			}
		}

		return response()->json(['Settings Updated']);
	}

}	