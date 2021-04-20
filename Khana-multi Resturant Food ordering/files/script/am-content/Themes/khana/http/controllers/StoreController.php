<?php 

namespace Amcoders\Theme\khana\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\User;
use App\Category;
use App\Terms;
use Auth;
use Session;
use Cart;
use Artesaos\SEOTools\Facades\SEOMeta;
use Artesaos\SEOTools\Facades\OpenGraph;
use Artesaos\SEOTools\Facades\TwitterCard;
use Artesaos\SEOTools\Facades\JsonLd;
use App\Options;


class StoreController extends controller
{
	protected $vendor_id;

	public function show($slug)
	{
		

		$store=User::with('info','gallery','preview','avg_ratting','delivery','pickup','shopcategory','location','shopday','role','vendor_reviews','ratting','badge','usersaas')->where('slug',$slug)->first();
		if (empty($store)) {
			abort(404);
		}
		$des=json_decode($store->info->content);

		$rattings = User::where('slug',$slug)->with('five_ratting','four_ratting','three_ratting','two_ratting','one_ratting')->first();
		

		SEOMeta::setTitle($store->name);
        SEOMeta::setDescription($des->description);
       
      

        OpenGraph::setDescription($des->description);
        OpenGraph::setTitle($store->name);
        OpenGraph::setUrl(url()->current());
       
        OpenGraph::addImage($store->preview->content ?? $store->avatar);
        OpenGraph::addImage($store->avatar);
        OpenGraph::addImage(['url' => $store->preview->content ?? $store->avatar, 'size' => 300]);
        OpenGraph::addImage($store->preview->content ?? $store->avatar, ['height' => 300, 'width' => 300]);
        
        JsonLd::setTitle($store->name);
        JsonLd::setDescription($des->description);
     
        JsonLd::addImage($store->preview->content ?? $store->avatar);

		$five_calculate = (100 * $rattings->five_ratting()->count());
		if($store->vendor_reviews()->count() != 0)
		{
			$five_rattings = $five_calculate / $store->vendor_reviews()->count();
		}else{
			$five_rattings = 0;
		}
		

		$four_calculate = (100 * $rattings->four_ratting()->count());
		if($store->vendor_reviews()->count() != 0)
		{
			$four_rattings = $four_calculate / $store->vendor_reviews()->count();
		}else{
			$four_rattings = 0;
		}

		$three_calculate = (100 * $rattings->three_ratting()->count());
		if($store->vendor_reviews()->count() != 0)
		{
			$three_rattings = $three_calculate / $store->vendor_reviews()->count();
		}else{
			$three_rattings = 0;
		}
		

		$two_calculate = (100 * $rattings->two_ratting()->count());
		if($store->vendor_reviews()->count() != 0)
		{
			$two_rattings = $two_calculate / $store->vendor_reviews()->count();
		}else{
			$two_rattings = 0;
		}
			

		$one_calculate = (100 * $rattings->one_ratting()->count());
		if($store->vendor_reviews()->count() != 0)
		{
			$one_rattings = $one_calculate / $store->vendor_reviews()->count();
		}else{
			$one_rattings = 0;
		}
			

		Session::put('restaurant_cart',[
			'count' => Cart::instance('cart_'.$store->slug)->count(),
			'slug' => $store->slug
		]);

		Session::put('restaurant_id',[
			'id' => $store->id,
			'name' => $store->name
		]);
		
		if($store && $store->role_id == 3 && $store->status == 'approved' || $store->status == 'offline') {
			$galleries = explode(",", $store->gallery->content);
			return view('theme::store.index',compact('store','galleries','five_rattings','four_rattings','three_rattings','two_rattings','one_rattings'));
		}else{
			return abort(404);
		}
	}

	public function store_data(Request $request,$slug)
	{
		$store = User::where('slug',$slug)->first();
		$this->vendor_id = $store->id;
		 $categories =Category::where('user_id',$store->id)->where('type',1)->with('products')->wherehas('products')->get();
		return view('theme::layouts.section.storeproduct',compact('categories','store'));

	}

	public function addon_product(Request $request)
	{
		$store = User::where('slug',$request->store_slug)->first();
		$product = Terms::with('price','addons','excerpt')->where('slug',$request->slug)->first();
		return view('theme::layouts.section.addonproduct',compact('product','store'));
	}

	public function resturantinfo(Request $request)
	{
		$store=User::with('info','shopcategory','location','shopday')->where('slug',$request->slug)->first();
		return view('theme::layouts.section.resturantinfo',compact('store'));
	}

	
}

