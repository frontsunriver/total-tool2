<?php 

namespace Amcoders\Plugin\product\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Order;
use App\Userplan;
use App\Riderlog;
use Carbon\Carbon;
use Auth;
class EarningController extends controller
{
	public function index()
	{
		if (!Auth()->user()->can('earning.order.report')) {
			return abort(401);
		}
		$total_earnings = Order::where('status',1)->select('commission','created_at','updated_at')->sum('commission'); 
		$total_earnings_paginate = Order::where('status',1)->latest()->select('commission','created_at','updated_at')->paginate(20); 
		$monthly_earnings =  Order::where('status',1)->whereMonth('created_at', Carbon::now()->month)->select('commission')->sum('commission');
		$year_earnings =  Order::where('status',1)->whereYear('created_at', Carbon::now()->year)->select('commission')->sum('commission');
		$today_earnings = Order::where('status',1)->whereDate('created_at', Carbon::today())->select('commission')->sum('commission');
		
		return view('plugin::admin.earning.index',compact('total_earnings','monthly_earnings','year_earnings','today_earnings','total_earnings_paginate'));
	}

	public function date(Request $request)
	{
		if (!Auth()->user()->can('earning.order.report')) {
			return abort(401);
		}
		$start_date = Carbon::parse($request->start)->format('Y-m-d H:i:s');
		$end_date = Carbon::parse($request->end)->format('Y-m-d H:i:s');
		$total_earnings_paginate = Order::where('status',1)->whereBetween('created_at',[$start_date,$end_date])->latest()->select('commission','created_at','updated_at')->paginate(20); 
		$total_earnings = Order::where('status',1)->select('commission','created_at','updated_at')->sum('commission'); 
		$monthly_earnings =  Order::where('status',1)->whereMonth('created_at', Carbon::now()->month)->select('commission')->sum('commission');
		$year_earnings =  Order::where('status',1)->whereYear('created_at', Carbon::now()->year)->select('commission')->sum('commission');
		$today_earnings = Order::where('status',1)->whereDate('created_at', Carbon::today())->select('commission')->sum('commission');
		return view('plugin::admin.earning.index',compact('total_earnings','monthly_earnings','year_earnings','today_earnings','total_earnings_paginate'));
	}


	public function delivery(Request $request)
	{
		if (!Auth()->user()->can('earning.delivery.report')) {
			return abort(401);
		}
		
		$totalearnings = Order::where('status',1)->where('order_type',1)->sum('shipping'); 
		$totallogs = Riderlog::where('status',1)->sum('commision'); 
		$total_earnings=$totalearnings-$totallogs;


		$monthly_earnings =  Order::where('status',1)->where('order_type',1)->whereMonth('created_at', Carbon::now()->month)->sum('shipping');
		$monthly_logs = Riderlog::where('status',1)->whereMonth('created_at', Carbon::now()->month)->sum('commision');
		$monthly_earnings=$monthly_earnings-$monthly_logs;

		$year_earnings =  Order::where('status',1)->where('order_type',1)->whereYear('created_at', Carbon::now()->year)->sum('shipping');
		$year_logs =  Riderlog::where('status',1)->whereYear('created_at', Carbon::now()->year)->sum('commision');
		$year_earnings=$year_earnings-$year_logs;

		$today_earnings = Order::where('status',1)->where('order_type',1)->whereDate('created_at', Carbon::today())->sum('shipping');
		$today_logs = Riderlog::where('status',1)->whereDate('created_at', Carbon::today())->sum('commision');
		$today_earnings=$today_earnings-$today_logs;

		$orders=Order::where('status',1)->where('order_type',1)->latest()->paginate(30);

		return view('plugin::admin.earning.delivery',compact('total_earnings','monthly_earnings','year_earnings','today_earnings','orders'));
	}

	public function saas()
	{
		if (!Auth()->user()->can('earning.subscription.report')) {
			return abort(401);
		}
		$total_earnings = Userplan::where('status',1)->sum('amount'); 
		$orders = Userplan::where('status',1)->latest()->paginate(20); 
		$monthly_earnings =  Userplan::where('status',1)->whereMonth('created_at', Carbon::now()->month)->sum('amount');
		$year_earnings =  Userplan::where('status',1)->whereYear('created_at', Carbon::now()->year)->sum('amount');
		$today_earnings = Userplan::where('status',1)->whereDate('created_at', Carbon::today())->sum('amount');
		
		return view('plugin::admin.earning.saas',compact('total_earnings','monthly_earnings','year_earnings','today_earnings','orders'));
	}
}