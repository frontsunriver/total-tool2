<?php 

namespace Amcoders\Plugin\shop\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Illuminate\Support\Str;
use Auth;
use App\Terms;
use App\User;
use App\Order;
use App\Usermeta;
use App\Onesignal;
use App\Transactions;
use Hash;

class PayoutController extends controller
{
	public function request(Request $request)
	{
		if (!Auth()->user()->can('payout.request')) {
			return abort(401);
		}
		if ($request->src) {
			$src=$request->src;
			$requests=Transactions::where('status',2)->where($request->type,$request->src)->latest()->paginate(50);
			return view('plugin::admin.payouts.requests',compact('requests','src'));
		}
		
		$requests=Transactions::where('status',2)->latest()->paginate(50);
		return view('plugin::admin.payouts.requests',compact('requests'));
	}
	public function history(Request $request)
	{
		if (!Auth()->user()->can('payout.history')) {
			return abort(401);
		}
		if ($request->src) {
			$src=$request->src;
			$requests=Transactions::where('status','!=',2)->where($request->type,$request->src)->latest()->paginate(50);
			return view('plugin::admin.payouts.history',compact('requests','src'));
		}
		
		$requests=Transactions::where('status','!=',2)->latest()->paginate(50);
		return view('plugin::admin.payouts.history',compact('requests'));
	}

	public function show($id)
	{
		if (!Auth()->user()->can('payout.view')) {
			return abort(401);
		}
		 $info=Transactions::with('user','admin')->find($id);
		$transections=Transactions::where('user_id',$info->user_id)->latest()->paginate(30);
		$paypal=\App\Usermeta::where('user_id',$info->user_id)->where('type','paypal_info')->first();
		$bank=\App\Usermeta::where('user_id',$info->user_id)->where('type','bank_info')->first();
		$total_amount=Order::where('vendor_id',$info->user_id)->where('status',1)->where('payment_status',1)->sum('total');
		$total_commission=Order::where('vendor_id',$info->user_id)->where('status',1)->where('payment_status',1)->sum('commission');
		$transactions=Transactions::where('user_id',$info->user_id)->latest()->paginate(30);
		return view('plugin::admin.payouts.show',compact('info','transections','paypal','bank','total_amount','total_commission','transactions'));
	}

	public function payoutUpdate(Request $request,$id)
	{
		$info=Transactions::find($id);
		$info->amount=$request->amount;
		$info->admin_id=Auth::id();
		$info->payment_mode=$request->method;
		$info->status=$request->status;
		$info->save();
		return response()->json(['Payout Updated']);
	}



	public function accounts(Request $request)
	{
		if (!Auth()->user()->can('payout.account')) {
			return abort(401);
		}
		if (!empty($request->src)) {
		$accounts=Usermeta::where('user_id',$request->src)->where('type',$request->type)->latest()->paginate(50);
		}
		else{
		  $accounts=Usermeta::where('type','paypal_info')->orWhere('type','bank_info')->latest()->paginate(50);
		}
		$src=$request->src ?? '';
		return view('plugin::admin.payouts.accounts',compact('accounts','src'));
	}

	public function destroy($id)
	{
		Usermeta::destroy($id);
		return back();
	}
}	