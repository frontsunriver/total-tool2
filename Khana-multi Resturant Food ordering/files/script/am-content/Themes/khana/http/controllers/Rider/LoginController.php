<?php 

namespace Amcoders\Theme\khana\http\controllers\Rider;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Terms;
use App\User;
use App\Location;
use Auth;
use Hash;
use Illuminate\Support\Str;
/**
 * 
 */
class LoginController extends controller
{
	public function login()
	{
		if(Auth::check())
		{
			return redirect()->route('login');
		}
		return view('theme::rider.login');
	}
}