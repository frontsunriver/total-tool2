<?php 

namespace Amcoders\Theme\khana\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use App\Terms;
use App\User;
use Hash;
use Illuminate\Support\Str;
/**
 * 
 */
class RegisterController extends controller
{

    public function register()
    {
        if(Auth::check())
        {
            return redirect()->route('login');
        }
        return view('theme::register.index');
    }

    public function user_store(Request $request)
    {
        $validator = \Validator::make($request->all(), [
            'name' => 'required',
            'email' => 'required|unique:users',
            'password' => 'required|confirmed',
            'agree' => 'required'
        ]);

        if($validator->fails())
        {
            return back()->with('errors',$validator->errors()->all()[0]);
        }

        $user_slug = Str::slug($request->name);
        $user = User::where('slug',$user_slug)->first();

        if ($user) {
            $slug= $request->name.Str::random(5);
        }
        else{
            $slug = Str::slug($request->name);
        }
        $badge = Terms::where('type',5)->where('status',1)->where('count',1)->first();

        $user = new User();
        $user->role_id = 2;
        $user->name = $request->name;
        $user->slug = $slug;
        $user->email = $request->email;
        $user->password = Hash::make($request->password);
        if (!empty($badge)) {
           $user->badge_id = $badge->id; 
        }
        $user->save();

        Auth::login($user,true);

        return redirect()->route('login');

    }
}






