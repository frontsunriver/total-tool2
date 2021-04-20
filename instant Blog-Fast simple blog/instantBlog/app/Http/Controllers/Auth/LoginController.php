<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Laravel\Socialite\Facades\Socialite;
use Exception;
use App\Models\User;
use Illuminate\Support\Facades\Auth;



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

    public function redirectToProvider($driver)
    {
        return Socialite::driver($driver)->redirect();
    }

    public function handleProviderCallback($driver)
    {
        try {
            $user = Socialite::driver($driver)->user();
        } catch (Exception $e) {
            return redirect('auth/' . $driver);
        }
        
        $authUser = $this->findOrCreateUser($user);

        Auth::login($authUser, true);
        return redirect('/home');
    }

    public function findOrCreateUser($User)
    {
        $authUser = User::where('social_id', $User->id)->first();

        if ($authUser) {
            return $authUser;
        }

        $name = isset($User->name) ? $User->name : 'Your Name';
        $username = isset($User->nickname) ? $User->nickname  : str_slug($name);

        $usercheck = User::where('username', $username)->first();

        if ($usercheck) {
            $usernamechecked =  $username . rand(10, 100);
        } else {
            $usernamechecked = $username;
        }

        $password = bcrypt(str_random(10));
        $random_email = str_random(7) . '@writeyouremail.com';
        $email = isset($User->email) ? $User->email : $random_email;
        $social_id = isset($User->id) ? $User->id : '000000123';
        $avatar = isset($User->avatar) ? $User->avatar : url('/images/defaultuser.png');

        return User::create([
            'name' => $name,
            'username' => $usernamechecked,
            'password' => $password,
            'email' => $email,
            'social_id' => $social_id,
            'avatar' => $avatar,
        ]);
    }
}
