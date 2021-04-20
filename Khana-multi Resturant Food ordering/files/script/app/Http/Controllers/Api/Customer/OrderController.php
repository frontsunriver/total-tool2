<?php

namespace App\Http\Controllers\Api\Customer;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\User;
use Illuminate\Support\Facades\Hash;
use Auth;
use Cart;
use Session;
use App\Order;
use App\Ordermeta;

class OrderController extends Controller
{
	public function index()
	{
		$orders = Order::where('user_id',Auth::User()->id)->orderBy('id','DESC')->get();
		return $orders;
	}

	public function details($id)
	{
		$info=Order::where('user_id',Auth::id())->with('orderlist','vendorinfo','riderinfo','coupon','orderlog','riderlog','liveorder')->find($id);
		return $info;
	}

	public function store(Request $request)
	{
	    
        
      
	    
		 $order = new Order();
		 $order->user_id = Auth::User()->id;
		 $order->vendor_id = $request->vendor_id;
		 $order->seen = 0;
		 $order->order_type = $request->order_type;
		 $order->payment_method = $request->payment_method;
		 $order->payment_status = $request->payment_status;
		 if($request->coupon_id)
		 {
		 	$order->coupon_id = $request->coupon_id;
		 }
		 $order->total = $request->total;
		 $order->shipping = $request->shipping;
		 $order->commission = $request->commission;
		 $order->discount = $request->discount;
		 $order->status = $request->status;
		 $data['name']=$request->name;
         $data['phone']=$request->phone;
         $data['address']=$request->delivery_address;
         $data['latitude']=$request->latitude;
         $data['longitude']=$request->longitude;
         $data['note']=$request->order_note;
         $order->data = json_encode($data);
         $order->save();
        
	    
     	foreach($request->datacart as $key => $value) {
         	$ordermeta = new Ordermeta;
            $ordermeta->order_id = $order->id;
            $ordermeta->qty  = $value['quantity'];
            $ordermeta->total  = $value['price'];
            $ordermeta->term_id  = $value['food']['id'];
            $ordermeta->save();
        }
        
        
        
         

         return $order;

	}
}

