<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Providers\RouteServiceProvider;
use Illuminate\Foundation\Auth\AuthenticatesUsers;
use Auth;
use Socialite;
use App\User;
use Hash;
use Illuminate\Support\Str;
use Session;

class LoginController extends Controller
{
    /*
    |--------------------------------------------------------------------------
    | Login Controller
    |--------------------------------------------------------------------------
    |
    | This controller handles authenticating users for the application and
    | redirecting them to your home screen. The controller uses a trait
    | to conveniently provide its functionality to your applications.
    |
    */

    use AuthenticatesUsers;

    /**
     * Where to redirect users after login.
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
        $this->middleware('guest')->except('logout');
    }

    public function redirectToProvider($provider)
    {
        return Socialite::driver($provider)->redirect();
    }

    /**
     * Obtain the user information from GitHub.
     *
     * @return \Illuminate\Http\Response
     */
    public function handleProviderCallback($provider)
    {
        $users = Socialite::driver($provider)->user();

        $user = User::where('email',$users->getEmail())->first();

        if($user)
        {
            Auth::login($user,true);
            if(Session::has('user_login'))
            {
                if(Session::get('user_login')['status'] == true)
                {
                    Session::put('user_login',[
                        'status' => false
                    ]);
                    return redirect()->route('checkout.index');
                }
            }else{
                return redirect()->route('login');
            }
        }else{
            $user_data = User::firstOrCreate([
                'role_id' => 2,
                'name' => $users->name,
                'slug' => Str::slug($users->name),
                'email' => $users->getEmail(),
                'avatar' => $users->getAvatar(),
                'password' => Hash::make('rootadmin')
            ]);

            Auth::login($user_data,true);

            if(Session::has('user_login'))
            {
                if(Session::get('user_login')['status'] == true)
                {
                    Session::put('user_login',[
                        'status' => false
                    ]);
                    return redirect()->route('checkout.index');
                }
            }else{
                return redirect()->route('login');
            }
        }

    }
}
