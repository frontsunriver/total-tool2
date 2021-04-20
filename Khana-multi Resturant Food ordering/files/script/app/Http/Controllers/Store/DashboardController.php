<?php

namespace App\Http\Controllers\Store;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Order;
use App\Transactions;
use Auth;
use Carbon\Carbon;
use App\User;
use App\Options;
class DashboardController extends Controller
{
    public function dashboard()
    {
    	$totalseles=Order::where('vendor_id',Auth::id())->where('payment_status',1)->where('status',1)->sum('total');
    	$totalcommsion=Order::where('vendor_id',Auth::id())->where('payment_status',1)->where('status',1)->sum('commission');
    	$totals=$totalseles-$totalcommsion;

    	$totalselesmonth=Order::where('vendor_id',Auth::id())
    	->where('payment_status',1)
    	->where('status',1)
    	->whereYear('created_at', Carbon::now()->year)
    	->whereMonth('created_at', Carbon::now()->month)
    	->sum('total');
    	$totalcommsionmonth=Order::where('vendor_id',Auth::id())
    	->where('payment_status',1)
    	->where('status',1)
    	->whereYear('created_at', Carbon::now()->year)
    	->whereMonth('created_at', Carbon::now()->month) 
    	->sum('commission');

    	$earningMonth=$totalselesmonth-$totalcommsionmonth;

    	$orders=Order::where('vendor_id',Auth::id())->latest()->paginate(20);

    	$notice=Options::where('key','announcement')->where('lang',1)->first();
    	return view('shop.dashboard',compact('totals','earningMonth','notice','orders'));
    }

    public function status(Request $request)
    {

    	$user=User::find(Auth::id());
    	$user->status=$request->status;
    	$user->save();
    	return back();
    }
}
