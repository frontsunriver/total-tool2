<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Providers\RouteServiceProvider;
use Illuminate\Foundation\Auth\VerifiesEmails;
use Auth;
use Session;

class VerificationController extends Controller
{
    /*
    |--------------------------------------------------------------------------
    | Email Verification Controller
    |--------------------------------------------------------------------------
    |
    | This controller is responsible for handling email verification for any
    | user that recently registered with the application. Emails may also
    | be re-sent if the user didn't receive the original email message.
    |
    */

    use VerifiesEmails;

    /**
     * Where to redirect users after verification.
     *
     * @var string
     */
    protected $redirectTo;

    /**
     * Create a new controller instance.
     *
     * @return void
     */
    public function __construct()
    {
        if (Auth::check() && Auth::User()->role->id == 1) {
            $this->redirectTo = route('admin.dashboard');
        }elseif(Auth::check() && Auth::User()->role->id == 3)
        {
            $this->redirectTo = route('store.dashboard');

        }elseif(Auth::check() && Auth::User()->role->id == 4)
        {
            $this->redirectTo = route('rider.dashboard');
        }else {
            if(Session::has('user_login'))
            {
                if(Session::get('user_login')['status'] == true)
                {
                    Session::put('user_login',[
                        'status' => false
                    ]);
                    $this->redirectTo = route('checkout.index');
                }
            }else{
                $this->redirectTo = route('login');
            }
        }
        
        $this->middleware('auth');
        $this->middleware('signed')->only('verify');
        $this->middleware('throttle:6,1')->only('verify', 'resend');
    }
}
