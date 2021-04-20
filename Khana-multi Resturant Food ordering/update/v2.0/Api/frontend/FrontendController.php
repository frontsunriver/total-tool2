<?php

namespace App\Http\Controllers\Api\frontend;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Terms;
use App\Location;
use App\Category;
use App\User;
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
			->orderBy('locations.id',$request->order)
			->paginate(12);
		}
		else{
			return $posts=Location::where('role_id',3)
			->where('term_id',$id)
			->wherehas('restaurant')
			->with('restaurant')
			->orderBy('id',$request->order)
			->paginate(12);
		}

		$data['restaurants']=$posts;

		return $data;

	}

	//offerable restaurants
	public function offerAble(Request $request)
	{
	   $users=Location::where('term_id',$request->city_id)->select('id','user_id','term_id','latitude','longitude')->wherehas('Offerables')->with('Offerables')->latest()->paginate(10);
	   return response()->json(['restaurants'=>$users]);
		
	}


	public function restaurantView($id)
	{
		$store=User::with('info','gallery','preview','avg_ratting','delivery','pickup','shopcategory','location','shopday','ratting','badge','usersaas')->find($id);
		$data['info']['id']=$store->id;
		$data['info']['name']=$store->name;
		$data['info']['name']=$store->name;
		$data['info']['preview']=$store->preview->content;
		$data['info']['avg_ratting']=$store->avg_ratting->content;
		$data['info']['ratting']=$store->ratting->content;
		$data['info']['delivery']=$store->delivery->content;
		$data['info']['pickup']=$store->pickup->content;
		$data['info']['shopcategory']=$store->shopcategory;
		$data['info']['location']=$store->location;
		$data['info']['shopday']=$store->shopday;
		$data['info']['badge']=$store->badge;
		$data['info']['usersaas']=$store->usersaas;
		$data['info']['about']=json_decode($store->info->content);
		$data['info']['gallary']=explode(',', $store->gallery->content);
		return $data;
	}

	public function ResturantProductList($id)
	{
		$categories =Category::where('user_id',$id)->where('type',1)->select('id','name','user_id')->wherehas('products')->with('products')->get();
		return response()->json(['products'=>$categories]);
	}
}
