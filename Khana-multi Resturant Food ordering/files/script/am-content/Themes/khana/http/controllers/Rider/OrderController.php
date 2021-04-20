<?php 

namespace Amcoders\Theme\khana\http\controllers\Rider;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
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
class OrderController extends controller
{
	public function index(Request $request)
	{
		 if ($request->start) {
          $start = date("Y-m-d",strtotime($request->start));
          $end = date("Y-m-d",strtotime($request->end));
        
         
          $orders=Riderlog::with('orders')->whereBetween('created_at',[$start,$end])->paginate(30);
          $start=$request->start;
          $end=$request->end;
          return view('theme::rider.order.index',compact('orders','start','end'));

		}
		elseif($request->src){
			$orders=Riderlog::with('orders')->where('id',$request->src)->paginate(30);
		}
		elseif($request->status){
			if ($request->status=='complete') {
				$orders=Riderlog::with('orders')->wherehas('completed')->latest()->paginate(30);	
			}
			else{
			$orders=Riderlog::with('orders')->where('status',$request->status)->latest()->paginate(30);	
			}
			
		}
		else{
		  $orders=Riderlog::with('orders')->latest()->paginate(30);	
		}

		
		

		return view('theme::rider.order.index',compact('orders'));
	}

	public function jsonOrders(Request $request)
	{
		if ($request->ajax()) {
		 if ($request->id) {

		 		$row=Riderlog::where('user_id',Auth::id())->where('order_id',$request->id)->update(['seen'=>1]);
		 		
		 		return response('ok');
		 	}	
		
		 $newOrders=Riderlog::whereDate('created_at', Carbon::today())->where([
		 	['user_id',Auth::id()],
		 	['status',2],
		 	
		 ])->latest()->with('pendingorders')->get()->map(function($data){
		 	$qry['id']= $data->order_id;
		 	$qry['seen']= $data->seen;
		 	$qry['name']= $data->pendingorders->resturentinfo->name;
		 	$qry['total']= (float)$data->pendingorders->shipping + (float)$data->pendingorders->total;
		 	$qry['time']= $data->created_at->diffForHumans();
		 	return $qry;
		 });

			 

		 return response()->json(['newOrders'=>$newOrders]);
		}

		abort(404);
		
	}

	public function live_order()
	{
	 $accepteorders = Order::whereDate('created_at', Carbon::today())->where([
		 	['rider_id',Auth::User()->id],
		 	['status',3]
		 ])->whereHas('rideraccept')->select('id','seen','payment_method','total','updated_at','data','created_at','vendor_id')->latest()->get();
	

	$completeorders = Order::whereDate('created_at', Carbon::today())->where([
		 	['rider_id',Auth::User()->id],
		 	['status',1]
		 ])->select('id','seen','payment_method','total','updated_at','data','created_at','vendor_id')->latest()->get();	
	 return view('theme::rider.order.live',compact('accepteorders','completeorders'));
	}

	public function details($id)
	{

		$order = Order::where('rider_id',Auth::id())->with('vendorinfo')->find($id);
		if (empty($order)) {
			return back();
		}
		$riderlog=Riderlog::where('user_id',Auth::id())->where('order_id',$id)->first();
		return view('theme::rider.order.details',compact('order','riderlog'));
	}

	public function decline($id)
	{
		$id= decrypt($id);
		$auth_id = Auth::id();
		$order = Order::where('rider_id',Auth::id())->where('status',3)->find($id);
		if (empty($order)) {
			return back();
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

		return redirect()->route('rider.live.order');
	}

	public function pickup($id)
	{
		$id= decrypt($id);
		$log=Riderlog::where('order_id',$id)->where('user_id',Auth::id())->where('status',2)->first();
		$order = Order::where('rider_id',Auth::id())->where('status',3)->with('vendorinfo')->find($id);
		if (empty($order)) {
			return back();
		}
		if (!empty($log)) {
			$log->status = 1;
			$log->save(); 
		}
		
		$user=User::find(Auth::id());
		$user->status="offline";
		$user->save();

		

		$customerInfo = json_decode($order->data);
		$vandorInfo=json_decode($order->vendorinfo->info->content);
		return view('theme::rider.order.map',compact('order','customerInfo','vandorInfo'));
	}

	public function delivery(Request $request,$id)
	{
		$id= decrypt($id);
		$auth_id=Auth::id();
		
		$order = Order::where('rider_id',$auth_id)->where('status',3)->find($id);
		if (empty($order)) {
			return response()->json(['Unauthorized',401]);
		}
		if ($request->check == 1) {
			
			if (!empty($request->payment_status)) {
			  $order->payment_status=$request->payment_status;
			}
			$order->status = $request->status;
			$order->save();
		}
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

	public function map($id)
	{
		//$order = Order::find($id);
		//return view('theme::rider.order.map',compact('order')); 
	}

	public function type(Request $request,$type)
	{
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

		$orders = Order::where([
			['rider_id',Auth::User()->id],
			['status',$status]
		])->paginate(20);

		return view('theme::rider.order.index',compact('orders','type'));
	}


	public function live(Request $request)
	{
		
		if (!empty($request->lat) && !empty($request->long)) {
		
		$live = Live::where('order_id',$request->id)->first();
		if (empty($live)) {
			$live = new Live;
			$live->order_id = $request->id;	
		}
		$live->latitute = $request->lat;	
		$live->longlatitute = $request->long;	
		$live->save();

	  }

	}
}