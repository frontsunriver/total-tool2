<?php 

namespace Amcoders\Theme\khana\http\controllers\Admin;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use App\Order;
use Carbon\Carbon;
use App\Location;
use App\User;
use App\Terms;
use App\Orderlog;
use App\Riderlog;
use OneSignal;
use Amcoders\Plugin\Plugin;
/**
 * 
 */
class OrderController extends controller
{
	public function index(Request $request)
	{
		if (!Auth()->user()->can('order.list')) {
			return abort(401);
		}
		if (!empty($request->src)) {
			$src=$request->src;	
			$orders = Order::where('id',$request->src)->paginate(20);
			return view('theme::admin.order.index',compact('orders','src'));

		}
		$orders = Order::latest()->paginate(20);
		
		return view('theme::admin.order.index',compact('orders'));
	}

	public function details($id)
	{
		if (!Auth()->user()->can('order.control')) {
			return abort(401);
		}
		$info = Order::with('orderlist','vendorinfo','riderinfo','coupon','orderlog','riderlog')->find($id);
		if (empty($info)) {
			abort(404);
		}

		
		$riders= Location::where('role_id',4)
		->where('term_id',$info->vendorinfo->location->term_id)
		->whereHas('riders')
		->with('riders')
		->inRandomOrder()
		->take(20)
		->get();
		

		$riders = $riders ?? [];
		return view('theme::admin.order.details',compact('info','riders'));
	}

	public function date_filter(Request $request)
	{
		if (!Auth()->user()->can('order.control')) {
			return abort(401);
		}

		$date = $request->date;
		$starting_date = substr($date, 0,10);
		$start_date = date( "Y-m-d h:i:s", strtotime($starting_date));
		$ending_date = substr($date, -10);
		$end_date = date( "Y-m-d h:i:s", strtotime($ending_date));
		$cities = Terms::where('type',2)->get();
		if($request->type)
		{
			$type = $request->type;
			
			if($request->type == 'pending')
			{
				$status = 2;
			}elseif($request->type == 'accepted')
			{
				$status = 3;
			}elseif($request->type == 'complete')
			{
				$status = 1;
			}else
			{
				$status = 0;
			}
			$orders = Order::where('status',$status)->whereBetween('created_at', [$start_date, $end_date])
			->paginate(20);
			return view('theme::admin.order.index',compact('cities','orders','type'));
		}else{
			$orders = Order::whereBetween('created_at', [$start_date, $end_date])
			->paginate(20);
			return view('theme::admin.order.index',compact('cities','orders'));
		}
	}

	public function decline($id)
	{
		$order = Order::find($id);
		$order->rider_id = null;
		$order->save();

		return back();
	}

	public function pickup($id)
	{
		$order = Order::find($id);
		$order->status = 3;
		$order->save();

		return back();
	}

	public function delivery($id)
	{
		$order = Order::find($id);
		$order->status = 1;
		$order->save();

		return back();
	}

	public function delete($id)
	{
		Order::find($id)->delete();

		return back();
	}

	public function city(Request $request)
	{
		return $request->all();
	}

	public function type($type)
	{
		if (!Auth()->user()->can('order.control')) {
			return abort(401);
		}
		if($type == 'pending')
		{
			$status = 2;
		}elseif($type == 'accepted')
		{
			$status = 3;
		}elseif($type == 'complete')
		{
			$status = 1;
		}else
		{
			$status = 0;
		}

		$orders = Order::where('status',$status)->paginate(20);
		$cities = Terms::where('type',2)->get();
		return view('theme::admin.order.index',compact('orders','cities','type'));
	}


	/**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $id)
    {
      $validatedData = $request->validate([
        'status' => 'required',
      ]);

      
      $order=Order::find($id);
      $auth_id=$order->vendor_id;
       
      if (empty($order)) {
        abort(401);
      }
      if($order->order_type==1 && $request->status != 0){
        $validatedData = $request->validate([
          'rider' => 'required',
        ]);
      }
      if ($request->rider && $request->status != 0) {
        $order->rider_id=$request->rider;
      }
      
      $order->status=$request->status;
      $order->save();

     

    if($order->order_type==1 && $request->status != 0){
        $validatedData = $request->validate([
        'rider' => 'required',
        ]);

        $riderlog = new Riderlog;
        $riderlog->order_id = $order->id;
        $riderlog->user_id = $request->rider;
        $riderlog->status = 2;
        $riderlog->save(); 

        if (Plugin::is_active('WebNotification')) {
          $rider=\App\Onesignal::where('user_id',$request->rider)->latest()->first();
          if (!empty($rider)) {
            OneSignal::sendNotificationToUser("New Order",$request->rider,url('/rider/order/'.$order->id));
          }
        }

     }  

      $log = new Orderlog;
      $log->order_id = $order->id;
      $log->status = $request->status;
      $log->save();

        if ($request->status == 1) {
         $sum = Order::where('vendor_id',$auth_id)->where('status',1)->sum('total');
         $sellerbadges=Terms::where('type',3)->where('status',1)->where('slug', '>=', $sum)->orderBy('slug','ASC')->first();
         if (!empty($sellerbadges)) {
           $seller = User::find($auth_id);
           $seller->badge_id = $sellerbadges->id;
           $seller->save();
         }

         $commsion=User::with('usersaas')->find($auth_id);
         
        
         $or=Order::where('vendor_id',$auth_id)->find($id);
         if ($commsion->usersaas->commission != 0) {
         
           $com1=$commsion->usersaas->commission/100;
           $net_commision=$com1*$or->total;
           $or->commission=$net_commision;
           
         }
         else{
           $or->commission = 0;
         }
         $or->save();

       }     
      if ($request->status == 3) {
        return response()->json(['Order Processed']);
      }
      elseif($request->status == 1){
        return response()->json(['Order Completed']);
      }
      else{
        return response()->json(['Order Cancelled']);
      }
      
    }
}