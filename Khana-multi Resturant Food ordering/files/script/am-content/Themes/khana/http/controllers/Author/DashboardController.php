<?php 

namespace Amcoders\Theme\khana\http\controllers\Author;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Order;
use Auth;
/**
 * 
 */
class DashboardController extends controller
{
	public function dashboard()
	{
		 Order::where('user_id',Auth::User()->id)->orderBy('id','DESC')->with('orderlist')->get();
		return view('theme::author.dashboard');
	}
}