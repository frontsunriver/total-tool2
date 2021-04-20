<?php 

namespace Amcoders\Plugin\shop\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Illuminate\Support\Str;
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
class OrderController extends controller
{
	
	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {      
        $auth_id=Auth::id();
        
        if ($request->type=='live') {
           return view('plugin::order.index');
        }
        if ($request->start) {
          $start = date("Y-m-d",strtotime($request->start));
          $end = date("Y-m-d",strtotime($request->end));
          $status = $request->status;

          if (!empty($request->status)) {
             $allorders=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->count();
            $accepted=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',3)->count();
            $pendings=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',2)->count();
            $declineds=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',0)->count();
            $completed=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',0)->count();
            $orders= Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',$status)->paginate(100);
          }
          else{
            $allorders=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->count();
            $accepted=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',3)->count();
            $pendings=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',2)->count();
            $declineds=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',0)->count();
            $completed=Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->where('status',0)->count();
            $orders= Order::where('vendor_id',$auth_id)->whereBetween('created_at',[$start,$end])->paginate(100);
          }

          $start = $request->start;
          $end = $request->end;
          $st = $request->status;
         return view('plugin::order.report',compact('orders','start','end','st','allorders','accepted','pendings','declineds','completed'));

        }

        if ($request->src) {
          $orders= Order::where('vendor_id',$auth_id)->where('id',$request->src)->paginate(20);
          $src = $request->src;
          return view('plugin::order.report',compact('orders','src'));
        }

        if ($request->status) {
          $orders =  Order::where('vendor_id',$auth_id)->where('status',$request->status)->latest()->paginate(20);
          return view('plugin::order.report',compact('orders'));
        }

        $orders =  Order::where('vendor_id',$auth_id)->latest()->paginate(20);
        return view('plugin::order.report',compact('orders'));
    }

    /**
     * Get Order response
     *
     * @return \Illuminate\Http\Response
     */
    public function create(Request $request)
    {
       $auth_id=Auth::id();
       if ($request->id) {
        $find_row= Order::where('vendor_id',$auth_id)->find($request->id); 
        if (!empty($find_row)) {
            $find_row->seen=1;
            $find_row->save();
        }
        return "ok"; 
       }


       $newOrders=Order::whereDate('created_at', Carbon::today())->where('vendor_id',$auth_id)->where('status',2)->select('id','seen','payment_method','total','updated_at')->latest()->get()->map(function($data){
        $qry['id']= $data->id;
        $qry['seen']= $data->seen;
        $qry['payment_method']= strtoupper($data->payment_method);
        $qry['total']= (float)$data->total;
        $qry['time']= $data->updated_at->diffForHumans();
        return $qry;
      });

       $OrderAccepts=Order::whereDate('created_at', Carbon::today())->where('vendor_id',$auth_id)->where('status',3)->select('id','seen','payment_method','total','updated_at')->latest()->get()->map(function($data){
        $qry['id']= $data->id;
        $qry['seen']= $data->seen;
        $qry['payment_method']= strtoupper($data->payment_method);
        $qry['total']= (float)$data->total;
        $qry['time']= $data->updated_at->diffForHumans();
        return $qry;
      });

       $CompleteOrders=Order::whereDate('created_at', Carbon::today())->where('vendor_id',$auth_id)->where('status',1)->select('id','seen','payment_method','total','updated_at')->latest()->get()->map(function($data){
        $qry['id']= $data->id;
        $qry['seen']= $data->seen;
        $qry['payment_method']= strtoupper($data->payment_method);
        $qry['total']= (float)$data->total;
        $qry['time']= $data->updated_at->diffForHumans();
        return $qry;
      });
       return response()->json(['newOrders'=>$newOrders,'OrderAccepts'=>$OrderAccepts,'CompleteOrders'=>$CompleteOrders]);
    }

  
    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
      $auth_id=Auth::id();
      $info = Order::with('orderlist','vendorinfo','riderinfo','coupon','orderlog','riderlog')->where('vendor_id',$auth_id)->find($id);
      if (empty($info)) {
          abort(404);
      }
     
      if (empty($info->rider_id)) {
        $riders= Location::where('role_id',4)
        ->where('term_id',$info->vendorinfo->location->term_id)
        ->whereHas('riders')
        ->with('riders')
        ->inRandomOrder()
        ->take(20)
        ->get();
      }


      $riders = $riders ?? [];

      return view('plugin::order.show',compact('info','riders'));

    }

    public function invoice($id)
    {
      $auth_id=Auth::id();
      $info = Order::with('orderlist','vendorinfo','coupon')->where('vendor_id',$auth_id)->findorFail($id);
      $customer_info=json_decode($info->data);
      $vendor_info=json_decode($info->vendorinfo->info->content);
      $pdf = \PDF::loadView('plugin::order.invoice',compact('info','customer_info','vendor_info'));
      return $pdf->download('invoice.pdf');
     

    }

    public function invoice_print($id)
    {
      $auth_id=Auth::id();
      $info = Order::with('orderlist','vendorinfo','coupon')->where('vendor_id',$auth_id)->findorFail($id);
      $customer_info=json_decode($info->data);
      $vendor_info=json_decode($info->vendorinfo->info->content);
     return view('plugin::order.invoice_print',compact('info','customer_info','vendor_info'));
     

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

      $auth_id=Auth::id();
      $order=Order::where('vendor_id',$auth_id)->find($id);

       
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
            OneSignal::sendNotificationToUser("New Order",$rider->player_id,url('/rider/order/'.$order->id));
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

         $commsion=User::with('usersaas')->find(Auth::id());
         
        
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