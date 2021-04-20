<?php

namespace App\Http\Controllers\Api\Rider;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Terms;
use App\User;
use App\Live;
use App\Location;
use Auth;
use Hash;
use Illuminate\Support\Str;
use App\Order;
use Carbon\Carbon;
use App\Riderlog;
use App\Options;
use App\Orderlog;
class RiderController extends Controller
{
	public function TodaysPendingOrders()
	{
		$newOrders=Riderlog::whereDate('created_at', Carbon::today())->where([
			['user_id',Auth::id()],
			['status',2],

		])->latest()->with('pendingorders')->get()->map(function($data){
			$qry['id']= $data->order_id;
			$qry['seen']= $data->seen;
			$qry['restaurant_name']= $data->pendingorders->resturentinfo->name;
			$qry['total']= (float)$data->pendingorders->shipping+(float)$data->pendingorders->total;
			$qry['time']= $data->created_at->diffForHumans();
			return $qry;
		});



		return response()->json(['new_pending_orders'=>$newOrders]);
	}


	public function AllOrders()
	{
		$orders=Riderlog::with('orders')->latest()->paginate(30);			 
		return response()->json(['orders'=>$orders]);
	}

	public function OrderView($id)
	{
		$order = Order::where('rider_id',Auth::id())->with('vendorinfo')->find($id);
		if (empty($order)) {
			return response()->json(['error'=>'Unauthorized'],401);
		}
		$data['order_id']=$order->id;
		$data['customer_data']=json_decode($order->data);
		$data['created_at']=$order->created_at->diffForHumans();
		$data['created_date']=$order->created_at->toDate('d-F-Y');
		$data['payment_status']=$order->payment_status;
		$data['amount']=$order->total;
		$data['shipping']=$order->shipping;
		$data['total']=(float)$order->shipping+(float)$order->total;
		$data['payment_method']=strtoupper($order->payment_method);
		$data['vendorinfo']['name']=asset($order->vendorinfo->avatar);
		$data['vendorinfo']['location']=$order->vendorinfo->location;
		$data['vendorinfo']['info']=json_decode($order->vendorinfo->info->content);
		return response()->json(['order'=>$data]);
	}

	public function accept(Request $request)
	{
		$validatedData = $request->validate([
			'order_id' => 'required',

		]);

		$id= $request->order_id;
		$log=Riderlog::where('order_id',$id)->where('user_id',Auth::id())->where('status',2)->first();
		$order = Order::where('rider_id',Auth::id())->where('status',3)->with('vendorinfo')->find($id);
		if (empty($order)) {
			return response()->json(['error'=>'Unauthorized'],401);
		}
		if (!empty($log)) {
			$log->status = 1;
			$log->save(); 
		}
		
		$user=User::find(Auth::id());
		$user->status="offline";
		$user->save();

		

		
		$data['status']="Order Accepted";
		$data['order_id']=$order->id;
		$data['customer_data']=json_decode($order->data);
		$data['created_at']=$order->created_at->diffForHumans();
		$data['created_date']=$order->created_at->toDate('d-F-Y');
		$data['payment_status']=$order->payment_status;
		$data['amount']=(float)$order->total;
		$data['shipping']=(float)$order->shipping;
		$data['total']=(float)$order->shipping+(float)$order->total;
		$data['payment_method']=strtoupper($order->payment_method);
		$data['vendorinfo']['name']=asset($order->vendorinfo->avatar);
		$data['vendorinfo']['location']=$order->vendorinfo->location;
		$data['vendorinfo']['info']=json_decode($order->vendorinfo->info->content);

		return response()->json(['data'=>$data]);
	}

	public function decline(Request $request)
	{
		$validatedData = $request->validate([
			'order_id' => 'required',

		]);

		$id= $request->order_id;
		$auth_id = Auth::id();
		$order = Order::where('rider_id',$auth_id)->where('status',3)->find($id);
		if (empty($order)) {
			return response()->json(['error'=>'Unauthorized'],401);
		}
		$order->rider_id = null;
		$order->status = 2;
		$order->seen = 0;
		$order->save();

		$log=Riderlog::where('order_id',$id)->where('user_id',$auth_id)->first();
		if (!empty($log)) {
			$log->status = 0;
			$log->save(); 
		}

		return response()->json(["status"=>"Order Canceled Successfully"]);
	}

	public function completeOrder(Request $request)
	{
		$validatedData = $request->validate([
			'order_id' => 'required',
			'status' => 'required',

		]);

		$id= $request->order_id;
		$auth_id=Auth::id();
		
		$order = Order::where('rider_id',$auth_id)->where('status',3)->find($id);
		if (empty($order)) {
			return response()->json(['Unauthorized',401]);
		}
		

		if (!empty($request->payment_status)) {
			$order->payment_status=$request->payment_status;
		}
		$order->status = $request->status;
		$order->save();
		
		$order = Order::where('rider_id',$auth_id)->find($id);
		$riderlog=Riderlog::where('order_id',$id)->where('user_id',$auth_id)->where('status',1)->first();
		if ($order->payment_status == 1 && $order->status == 1) {
			$percent=Options::where('key','rider_commission')->first();
			if (!empty($percent)) {
				$com1=$percent->value/100;
				$net_commision=$com1*$order->shipping;
				$riderlog->commision=$net_commision;
			}

			

			
			$commsion=User::with('usersaas')->find($order->vendor_id);
			
			if ($commsion->usersaas->commission != 0) {

				$com1=$commsion->usersaas->commission/100;
				$net_commision=$com1*$order->total;
				$order->commission=$net_commision;

			}
			else{
				$order->commission = 0;
			}
			$order->save();


			$sum = Order::where('vendor_id',$order->vendor_id)->where('status',1)->sum('total');
			$sellerbadges=Terms::where('type',3)->where('status',1)->where('slug', '>=', $sum)->orderBy('slug','ASC')->first();
			if (!empty($sellerbadges)) {
				$seller = User::find($auth_id);
				$seller->badge_id = $sellerbadges->id;
				$seller->save();
			}

			

		}

		$riderlog->status=$order->status;
		$riderlog->save();

		$log = new Orderlog;
		$log->order_id = $id;
		$log->status = $request->status;
		$log->save();

		$user=User::find($auth_id);
		$user->status="approved";
		$user->save();
		return response()->json(['Order Complete Success']);
	}

	public function status(Request $request)
    {
    	$validatedData = $request->validate([
			'status' => 'required',
		]);
        $user=User::find(Auth::id());
        $user->status=$request->status;
        $user->save();
        return response()->json("Status Update");
    }
}
