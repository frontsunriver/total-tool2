<?php 

namespace Amcoders\Theme\khana\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\User;
use App\Terms;
use App\Location;
use App\Category;
use App\Options;
use Cart;
use Str;
use Artesaos\SEOTools\Facades\SEOMeta;
use Artesaos\SEOTools\Facades\OpenGraph;
use Artesaos\SEOTools\Facades\TwitterCard;
use Artesaos\SEOTools\Facades\JsonLd;
class AreaController extends controller
{
	protected $categories;

	public function index($slug)
	{
		 $info=Terms::where('slug',$slug)->where('status',1)->where('type',2)->with('excerpt','preview')->first();

		if (empty($info)) {
			abort(404);
		}
		 $mapinfo=json_decode($info->excerpt->content ?? '');

		$lat = (double)$mapinfo->latitude;
		$long = (double)$mapinfo->longitude;
		$zoom = (double)$mapinfo->zoom;

		SEOMeta::setTitle($info->title);
        SEOMeta::setDescription($info->title);
        SEOMeta::setCanonical($info->title);

        OpenGraph::setDescription($info->title);
        OpenGraph::setTitle($info->title);
        OpenGraph::setUrl(url()->current());
        

        TwitterCard::setTitle($info->title);
        TwitterCard::setSite($info->title);

        JsonLd::setTitle($info->title);
        JsonLd::setDescription($info->title);
        JsonLd::addImage($info->preview->content);


	    return view('theme::area.index',compact('info','slug','lat','long','zoom'));
	}

	public function areainfo(Request $request,$id)
	{

		if (!$request->ajax()) {
			abort(404);
		}

		$info=Terms::where('status',1)->where('type',2)->with('excerpt')->find($id);
		$mapinfo=json_decode($info->excerpt->content);

		$lat = (double)$mapinfo->latitude;
		$long = (double)$mapinfo->longitude;
		$zoom = (double)$mapinfo->zoom;

		if (!empty($request->cats)) {
			
		 $posts=Location::join('user_category','user_category.user_id','locations.user_id')
		 	 ->where('locations.role_id',3)
		     ->where('locations.term_id',$id)
		     ->where('user_category.category_id',$request->cats)
		     ->wherehas('users')
		     ->with('users')
		     ->orderBy('locations.id',$request->order)
		     ->paginate(12);
		}
		else{
		$posts=Location::where('role_id',3)
		->where('term_id',$id)
		->wherehas('users')
		->with('users')
		->orderBy('id',$request->order)
		->paginate(12);
		}


	   $data['lat']=$lat;
	   $data['long']=$long;
	   $data['zoom']=$zoom;
	   $data['data']=$posts;

	   
	   return response()->json($data);
	}

	public function show(Request $request,$id)
	{
		if (!$request->ajax()) {
			abort(404);
		}
		
		if ($id==0) {
			
			return $posts=Location::join('user_category','user_category.user_id','locations.user_id')		
			->where('user_category.category_id',$request->cats)
			->wherehas('users')
			->with('users')
			->orderBy('locations.id',$request->order)
			->paginate(12);


			$info=Options::where('key','default_map')->first();
			$mapinfo=json_decode($info->value);
			$lat = (double)$mapinfo->default_lat;
			$long = (double)$mapinfo->default_long;
			$zoom = (double)$mapinfo->default_zoom;

			
			$data['lat']=$lat;
			$data['long']=$long;
			$data['zoom']=$zoom;
			$data['data']=$posts;


			return response()->json($data);
		}
		if (!empty($request->cats)) {
			
		 return $posts=Location::join('user_category','user_category.user_id','locations.user_id')
		 	 ->where('locations.role_id',3)
		     ->where('locations.term_id',$id)
		     ->where('user_category.category_id',$request->cats)
		     ->wherehas('users')
		     ->with('users')
		     ->orderBy('locations.id',$request->order)
		     ->paginate(12);
		}
		else{
		return $posts=Location::where('locations.role_id',3)

		->where('term_id',$id)
		->wherehas('users')
		->with('users')
		->orderBy('id',$request->order)
		->paginate(12);
		}
		
	}

	public function info($id)
	{
		 $info=Terms::where('id',$id)->where('status',1)->where('type',2)->with('excerpt','location')->first();
		 if(empty($info)){
		 	return [];
		 }
		 return $info;
		
	}

	public function resturents(Request $request)
	{
		if (!empty($request->lat) &&  !empty($request->long) &&  !empty($request->city)) {
			
			$slug = Str::slug($request->city);
			
			$info=Terms::where('slug',$slug)->where('status',1)->where('type',2)->with('excerpt','location')->first();

			
			$mapinfo=json_decode($info->excerpt->content ?? '');
			if (!empty($info)) {
				$lat = (double)$mapinfo->latitude;
				$long = (double)$mapinfo->longitude;
				$zoom = (double)$mapinfo->zoom;
			}
			else{
			$lat = $request->lat;
			$long = $request->long;
			$zoom = 10;	
			}
			
			return view('theme::area.index',compact('info','slug','lat','long','zoom'));
		}
		abort(404);
	}



	public function category($slug)
	{
		$category=Category::where('slug',$slug)->where('type',2)->first();
		if (empty($category)) {
			abort(404);
		}
		$info=Options::where('key','default_map')->first();
		$mapinfo=json_decode($info->value);
		
		$lat = (double)$mapinfo->default_lat;
		$long = (double)$mapinfo->default_long;
		$zoom = (double)$mapinfo->default_zoom;
		
		SEOMeta::setTitle($category->name);
        SEOMeta::setDescription($category->name);
        SEOMeta::setCanonical($category->name);

        OpenGraph::setDescription($category->name);
        OpenGraph::setTitle($category->name);
        OpenGraph::setUrl(url()->current());
        

        TwitterCard::setTitle($category->name);
        TwitterCard::setSite($category->name);

        JsonLd::setTitle($category->name);
        JsonLd::setDescription($category->name);
        JsonLd::addImage($category->avatar);

		return view('theme::area.index',compact('slug','lat','long','zoom','category'));
	}




}
