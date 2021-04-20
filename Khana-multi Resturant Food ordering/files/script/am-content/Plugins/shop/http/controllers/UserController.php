<?php 

namespace Amcoders\Plugin\shop\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Illuminate\Support\Str;
use Auth;
use App\Terms;
use App\User;
use App\Order;
use App\Onesignal;
use App\Riderlog;
use App\Transactions;
use Hash;

class UserController extends controller
{


	//return all customers
	public function customers(Request $request)
	{
		if (!Auth()->user()->can('customer.list')) {
			return abort(401);
		}
		if ($request->src) {
			$users=User::where('role_id',2)->where($request->type,$request->src)->latest()->paginate(40);
			$src=$request->src;
			return view('plugin::admin.customer.index',compact('users','src'));
		}
		$users=User::where('role_id',2)->latest()->paginate(40);
		return view('plugin::admin.customer.index',compact('users'));
	}
	//return all resturents
	public function vendors(Request $request)
	{
		if (!Auth()->user()->can('all.resturents')) {
			return abort(401);
		}
		$type="approved";	
		if ($request->src) {
			$users=User::where('role_id',3)->where('status','!=','pending')->where($request->type,$request->src)->latest()->paginate(40);
			$src=$request->src;
			return view('plugin::admin.vendors.resturents',compact('users','src','type'));
		}
		$users=User::where('role_id',3)->where('status','!=','pending')->latest()->paginate(40);
		return view('plugin::admin.vendors.resturents',compact('users','type'));
	}

	//return all riders
	public function riders(Request $request)
	{
		if (!Auth()->user()->can('all.rider')) {
			return abort(401);
		}
		$type="approved";	
		if ($request->src) {
			$users=User::where('role_id',4)->where('status','!=','pending')->where($request->type,$request->src)->latest()->paginate(40);
			$src=$request->src;
			return view('plugin::admin.rider.request',compact('users','src','type'));
		}
		$users=User::where('role_id',4)->where('status','!=','pending')->latest()->paginate(40);
		return view('plugin::admin.rider.request',compact('users','type'));
	}

	//return all vendor request
	public function requests(Request $request)
	{	
		if (!Auth()->user()->can('resturents.requests')) {
			return abort(401);
		}
		$type="pending";	
		if ($request->src) {
			$users=User::where('role_id',3)->where('status','=','pending')->where($request->type,$request->src)->latest()->paginate(40);
			$src=$request->src;
			return view('plugin::admin.vendors.resturents',compact('users','src','type'));
		}
	 	$users=User::where('role_id',3)->where('status','=','pending')->latest()->paginate(40);
		return view('plugin::admin.vendors.resturents',compact('users','type'));
	}

	//return all rider request
	public function riderrequests(Request $request)
	{	
		if (!Auth()->user()->can('rider.request')) {
			return abort(401);
		}
		$type="pending";	
		if ($request->src) {
			$users=User::where('role_id',4)->where('status','=','pending')->where($request->type,$request->src)->latest()->paginate(40);
			$src=$request->src;
			return view('plugin::admin.rider.request',compact('users','src','type'));
		}
	 	$users=User::where('role_id',4)->where('status','=','pending')->latest()->paginate(40);
		return view('plugin::admin.rider.request',compact('users','type'));
	}


	//return user profile
	public function show($id){
		 $info=User::with('resturentlocationwithcity','usersaas','info','Onesignal','delivery','pickup')->find($id);

		  if ($info->role_id==3) {
		  	if (!Auth()->user()->can('resturents.view')) {
		  		return abort(401);
		  	}
		 $orders=Order::where('vendor_id',$id)->select('id','vendor_id','order_type','total','created_at','status')->latest()->paginate(50);
		 $total_amount=Order::where('vendor_id',$id)->where('status',1)->where('payment_status',1)->sum('total');
		 $total_commission=Order::where('vendor_id',$id)->where('status',1)->where('payment_status',1)->sum('commission');
		 

			$transactions=Transactions::where('user_id',$id)->latest()->paginate(30);
		 	return view('plugin::admin.vendors.show',compact('info','orders','total_amount','total_commission','transactions'));	
		 }
		 elseif($info->role_id==4){
		 	$orders=Order::where('rider_id',$id)->select('id','order_type','total','created_at','shipping','status')->with('rideraccept')->latest()->paginate(50);
		 	$total_orders=Order::where('rider_id',$id)->where('status',1)->count();
		 	$total_earning= Riderlog::where('user_id',$id)->wherehas('completed')->where('status',1)->sum('commision');
		 	$total_withdraw= Transactions::where('user_id',$id)->where('status',1)->sum('amount');
		 	$transactions=Transactions::where('user_id',$id)->latest()->paginate(30);


		 	return view('plugin::admin.rider.show',compact('info','orders','transactions','total_orders','total_earning','total_withdraw'));
		 }
		 elseif($info->role_id==2){
		 	if (!Auth()->user()->can('customer.edit')) {
		 		return abort(401);
		 	}
		   $orders=Order::where('user_id',$id)->latest()->paginate(30);
		   return view('plugin::admin.customer.edit',compact('orders','orders','info'));

		 } 
				
	}


	//update seller info
	public function sellerUpdate(Request $request,$id)
	{
	   $user=User::find($id);
	   $user->name=$request->name;
	   $user->email=$request->email;
	   if (!empty($request->password_change)) {
	   	 if (!empty($request->password)) {
	   	 	 $user->password = Hash::make($request->password);
	   	 }
	   }
	   if (!empty($request->plan_id)) {
	   	$user->plan_id = $request->plan_id;
	   }
	   $user->status = $request->status;
	   $user->save();
	   if ($user->role_id == 3) {
	   	return response()->json(['Seller Updated']);
	   }
	   elseif($user->role_id == 4){
	   	return response()->json(['Rider Updated']);
	   }
	   elseif($user->role_id == 2){
	   	return response()->json(['Customer Info Updated']);
	   }
	   
	}


	//remove onesignal userid
	public function signalremove($id)
	{
		$remove=Onesignal::destroy($id);
		return back();
	}

	public function UsersDelete(Request $request)
	{
		if ($request->status=='delete') {
			foreach ($request->ids as $key => $row) {
				User::destroy($row);
			}
		}

		return back();
	}
	

}	