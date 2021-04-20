<?php 

namespace Amcoders\Theme\khana\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
/**
 * 
 */
class LoginController extends controller
{
	public function login()
	{
		if (Auth::check()) {
			return redirect()->route('login');
		}else{
			return view('theme::login.index');
		}
		
	}
}