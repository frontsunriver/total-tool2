<?php 

namespace Amcoders\Theme\khana\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\User;
use App\category;
use App\Terms;
use Cart;
use Session;
use Carbon\Carbon;
/**
 * 
 */
class CartController extends controller
{
	public function addon_add_to_cart(Request $request)
	{

		$single_product = Terms::with('price')->find($request->main_product);
		$cart = Cart::instance('cart_'.$request->store_slug)->add($single_product->id, $single_product->title, $request->qty_value, $single_product->price->price, 550,['type'=>'Regular']);
		Cart::instance('cart_'.$request->store_slug)->setGlobalTax(total_tax());

		if($request->addon)
		{
			foreach($request->addon as $value)
			{
				$product = Terms::with('price')->find($value);
				Cart::instance('cart_'.$request->store_slug)->add($product->id, $product->title, 1, $product->price->price, 550,['type'=>'Addon','note'=>$request->special_note]);
			}
		}


		Session::put('restaurant_cart',[
			'count' => Cart::instance('cart_'.$request->store_slug)->count(),
			'slug' => $request->store_slug
		]);
		

		return response()->json('ok');
	}

	public function add_to_cart(Request $request)
	{
		$product = Terms::with('price')->where('slug',$request->slug)->first();
		$cart = Cart::instance('cart_'.$request->store_slug)->add($product->id, $product->title, 1, $product->price->price, 550,['type'=>'Regular']);
		Cart::instance('cart_'.$request->store_slug)->setGlobalTax(total_tax());

		Session::put('restaurant_cart',[
			'count' => Cart::instance('cart_'.$request->store_slug)->count(),
			'slug' => $request->store_slug
		]);

		return "ok";
	}

	public function update(Request $request)
	{
		Cart::instance('cart_'.$request->store_slug)->update($request->id, $request->data_value);
		Session::put('restaurant_cart',[
			'count' => Cart::instance('cart_'.$request->store_slug)->count(),
			'slug' => $request->store_slug
		]);
		return "ok";
	}

	public function delete(Request $request)
	{
		Cart::instance('cart_'.$request->store_slug)->remove($request->id);
		Session::put('restaurant_cart',[
			'count' => Cart::instance('cart_'.$request->store_slug)->count(),
			'slug' => $request->store_slug
		]);
		return "ok";
	}

	public function discount(Request $request)
	{
		$check=Terms::where('title',$request->code)->where('auth_id',Session::get('restaurant_id')['id'])->where('type',10)->first();
		
		$mydate= Carbon::now()->toDateString();
		if (!empty($check)) {
			if ($check->slug >= $mydate) {
				$dicountPercent=$check->count;
				$total=Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->subtotal();

				 Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->setGlobalDiscount($check->count);

				 Session::put('coupon',[
				 	'id' => $check->id,
				 	'percent' => $check->count
				 ]);
				 $total=Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->subtotal();
				 return response()->json(['message'=>'Coupon Successfully Applied','amount'=>$total]);

			}else {
			   return response()->json(['error'=>'Coupon Expired',401]);
				
			}
		}
		else{
			return response()->json(['error'=>'Invalid Coupon',401]);
		}
	}
}