<?php

namespace App\Http\Controllers\Api\frontend;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Terms;
use App\Location;
use App\Category;
use App\User;
use Str;
use App\Options;

class FrontendController extends Controller
{
	//all city
	public function AllCity()
	{
		$locations=Terms::where('type',2)->withcount('Locationcount')->with('preview')->latest()->get()->map(function($data){
			$qry['id']=$data->id;
			$qry['title']=$data->title;
			$qry['count']=$data->locationcount_count;
			$qry['preview']=$data->preview->content;
			return $qry;
		});

		return response()->json(['allcity'=>$locations]);
	}

	public function GetCityId($slug)
	{
		$slug=Str::slug($slug);
		$info=Terms::where('slug',$slug)->where('status',1)->where('type',2)->first();
		if (empty($info)) {
			return response()->json('City Not Found',404);
		}
		return response()->json(['data'=>$info]);
	}

	
    //all category
	function category(Request $request)
	{
		if ($request->random=='off') {
			return $categories=Category::where('type',2)->select('id','name','avatar')->take($request->limit)->get();
		}
		else{
			return $categories=Category::where('type',2)->select('id','name','avatar')->inRandomOrder()->take($request->limit)->get();
		}
		
	}

	//city by resturants
	public function CityByUsers(Request $request,$id)
	{
		$info=Terms::where('status',1)->where('type',2)->with('excerpt')->find($id);
		$mapinfo=json_decode($info->excerpt->content ?? '');
		
		$data['info']['title']=$info->title;
		$data['info']['id']=$info->id;

		$data['info']['latitude']=(double)$mapinfo->latitude;
		$data['info']['longitude']=(double)$mapinfo->longitude;
		$data['info']['zoom']=(double)$mapinfo->zoom;

		if (!empty($request->cats)) {
			
			$posts=Location::join('user_category','user_category.user_id','locations.user_id')
			->where('locations.role_id',3)
			->where('locations.term_id',$id)
			->where('user_category.category_id',$request->cats)
			->wherehas('restaurant')
			->with('restaurant')
			->orderBy('locations.id',$request->order ?? 'DESC')
			->paginate(12);
		}
		else{
			return $posts=Location::where('role_id',3)
			->where('term_id',$id)
			->wherehas('restaurant')
			->with('restaurant')
			->orderBy('id',$request->order ?? 'DESC')
			->paginate(12);
		}

		$data['restaurants']=$posts;

		return $data;

	}

	//offerable restaurants
	public function offerAble($id)
	{
	   $users=Location::where('term_id',$id)->select('id','user_id','term_id','latitude','longitude')->wherehas('Offerables')->with('Offerables')->latest()->paginate(10);
	   if (empty($users)) {
	   	return response()->json('City Not Found',404); 
	   }
	   return response()->json(['restaurants'=>$users]);
		
	}

	public function home($id)
	{
		$Offerables=Location::where('term_id',$id)->select('id','user_id','term_id','latitude','longitude')->wherehas('Offerables')->with('Offerables')->latest()->paginate(10);
		 $categories=Category::where('type',2)->select('id','name','avatar')->inRandomOrder()->get()->map(function($q)
		{
			$data['id']=$q->id;
			$data['name']=$q->name;
			if (!empty($q->avatar)) {
				$data['avatar']=asset(imagesize($q->avatar,'medium'));
			}
			else{
				$data['avatar']=asset('uploads/store.jpg');
			}
			

			return $data;
		});
		
		$all_resturants=Location::where('role_id',3)
			->where('term_id',$id)
			->wherehas('restaurant_info')
			->with('restaurant_info')
			->latest()
			->paginate(12);

		return response()->json(['offerables'=>$Offerables,'categories'=>$categories,'all_resturants'=>$all_resturants]);

	}

	public function getResturents($id)
	{
		$users=Location::where('role_id',3)
			->where('term_id',$id)
			->wherehas('restaurant')
			->with('restaurant')
			->latest()
			->paginate(12);
		if (empty($users)) {
		return response()->json('User Not Found',404);		
		}

		return response()->json(['all_resturants'=>$users]);	
	}




	public function restaurantView($id)
	{
		$store=User::where('status','approved')->where('role_id',3)->with('info','gallery','preview','avg_ratting','delivery','pickup','shopcategory','location','shopday','ratting','vendor_reviews')->find($id);
		if (empty($store)) {
			return response()->json('Something Wrong',404);
		}
		$data['info']['id']=$store->id;
		$data['info']['name']=$store->name;
		$data['info']['slug']=$store->slug;
		if (!empty($store->preview->content)) {
			$data['info']['preview']=asset($store->preview->content);
		}
		else{
			$data['info']['preview']=asset($store->avatar);
		}
		
		$data['info']['avg_ratting']=$store->avg_ratting->content;
		$data['info']['ratting']=$store->ratting->content;
		$data['info']['delivery']=$store->delivery->content;
		$data['info']['pickup']=$store->pickup->content;
		$data['info']['shopcategory']=$store->shopcategory;
		$data['info']['location']=$store->location;
		$data['info']['shopday']=$store->shopday;
		$data['info']['about']=json_decode($store->info->content);
		$data['info']['reviews']=$store->vendor_reviews;

		$gallaries=[];
		$arrs=explode(',', $store->gallery->content);
		foreach ($arrs as $key => $value) {
			if (!empty($value)) {
				array_push($gallaries, asset($value));
			}
			
		}

		$data['info']['gallary']=$gallaries;
		$data['info']['avatar']=asset($store->avatar);

		$menus=Category::where('user_id',$id)->where('type',1)->inRandomOrder()->get();
		$products= Terms::where('auth_id',$id)->with('price','preview','postcategory')->where('status',1)->where('terms.type',6)->latest()->get();

		$info['info']=$data;
		$info['menus']=$menus;
		$info['products']=$products;

		return response()->json($info);
	}

	public function ResturantProductList($id)
	{
		$categories =Category::where('user_id',$id)->where('type',1)->select('id','name','user_id')->wherehas('products')->with('products')->get();
		return response()->json(['products'=>$categories]);
	}
	
	public function deliveryfee()
	{
	    return $delivery_fee = Options::where('key','km_rate')->first()->value;
	}
}
