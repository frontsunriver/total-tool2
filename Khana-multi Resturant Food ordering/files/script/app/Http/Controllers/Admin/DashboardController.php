<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Auth;
use App\Order;
use App\Riderlog;
use App\Userplan;
use App\Transactions;
use App\User;
use App\Options;
use Carbon\Carbon;

class DashboardController extends Controller
{
	/*
	return admin dashboard view
	*/
	public function Dashboard()
	{
		if (!Auth()->user()->can('dashboard')) {
			return abort(401);
		}
		//totalearnings
		$order_earn=Order::where('status',1)->where('payment_status',1)->sum('commission');
		$shipping=Order::where('status',1)->where('payment_status',1)->sum('shipping');
		$rider=Riderlog::wherehas('completed')->sum('commision');
		$subscribed=Userplan::where('status',1)->sum('amount');
		$totalearns=$order_earn+$subscribed+$shipping-$rider;

		//totalpayouts
		$totalPayouts=Transactions::where('status',1)->sum('amount');

		//totalresturents
		$resturents=User::where('role_id',3)->where('status','!=','pending')->count();

		//totalcustomers
		$customers=User::where('role_id',2)->count();

		//orders
		$totalOrders=Order::count();
		$totalPending=Order::where('status',2)->count();
		$totalProccessing=Order::where('status',3)->count();
		$totalCompleted=Order::where('status',1)->count();

		//today order
		 $totalTodaOrders = Order::whereDate('created_at', Carbon::today())->count();
		 $totalTodaComplete = Order::whereDate('created_at', Carbon::today())->where('status',1)->count();
		 $totalTodayPending = Order::whereDate('created_at', Carbon::today())->where('status',2)->count();
		 $totalTodayProccessing = Order::whereDate('created_at', Carbon::today())->where('status',3)->count();

		 //payout
		 $totalPayoutCount=Transactions::count();
		 $totalPayoutPending=Transactions::where('status',2)->count();
		 $totalPayoutComplete=Transactions::where('status',1)->count();


		 //resturent request
		 $requestResturent=User::where('role_id',3)->where('status','pending')->latest()->take(8)->get();
		 $requestRider=User::where('role_id',4)->where('status','pending')->latest()->take(8)->get();

		 $newOrders=Order::where('status',2)->with('vendor')->latest()->take(8)->get();

		 //option
		 $announcement=Options::where('key','announcement')->first();

		 if(Auth::User()->role_id == 1)
		 {
		 	return view('admin.dashboard',compact('totalearns','totalPayouts','resturents','customers','totalOrders','totalPending','totalProccessing','totalCompleted','totalTodaOrders','totalTodaComplete','totalTodayPending','totalTodayProccessing','totalPayoutComplete','totalPayoutPending','totalPayoutCount','requestResturent','requestRider','newOrders','announcement'));
		 }else{
		 	return redirect()->route('login');
		 }
	}


	public function announcement(Request $request)
	{
		$data['title']=$request->title;
		$data['message']=$request->message;

		$announcement=Options::where('key','announcement')->first();
		if (!$announcement) {
			$announcement = new Options;
			$announcement->key = 'announcement';
		}
		$announcement->lang = $request->status;
		$announcement->value = json_encode($data);
		$announcement->save();

		return response()->json('Announcement Updated');
	}
}
