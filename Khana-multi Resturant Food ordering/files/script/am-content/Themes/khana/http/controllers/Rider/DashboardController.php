<?php 

namespace Amcoders\Theme\khana\http\controllers\Rider;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Order;
use App\Transactions;
use Auth;
use Carbon\Carbon;
use App\User;
use App\Options;
use App\Riderlog;
class DashboardController extends controller
{
	

	public function dashboard()
	{
		$totalseles=Riderlog::where('user_id',Auth::id())->wherehas('completed')->where('status',1)->sum('commision');
		
		$totals=$totalseles;

		$totalselesmonth=Riderlog::where('user_id',Auth::id())
		->wherehas('completed')
		->where('status',1)
		->whereYear('created_at', Carbon::now()->year)
		->whereMonth('created_at', Carbon::now()->month)
		->sum('commision');
		

		$earningMonth=$totalselesmonth;

		$orders=Order::where('rider_id',Auth::id())->latest()->paginate(20);

		$notice=Options::where('key','announcement')->where('lang',1)->first();
		return view('theme::rider.dashboard',compact('totals','earningMonth','notice','orders'));
	}


}