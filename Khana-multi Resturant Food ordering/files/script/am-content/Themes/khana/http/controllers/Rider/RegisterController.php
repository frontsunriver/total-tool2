<?php 

namespace Amcoders\Theme\khana\http\controllers\Rider;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Terms;
use App\User;
use App\Usermeta;
use App\Location;
use Auth;
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
		$cities = Terms::where('type',2)->get();
		return view('theme::rider.register',compact('cities'));
	}

	public function store(Request $request)
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

        $user_slug=Str::slug($request->name);
        $user = User::where('slug',$user_slug)->first();

        if ($user) {
            $slug= $request->name.Str::random(5);
        }
        else{
            $slug=Str::slug($request->name);
        }

        $badge = Terms::where('type',4)->where('status',1)->where('count',1)->first();

        $user = new User();
        $user->role_id = 4;
        $user->name = $request->name;
        $user->slug = $slug;
        $user->email = $request->email;
        $user->password = Hash::make($request->password);
        if (!empty($badge)) {
           $user->badge_id = $badge->id; 
        }
        $user->save();

        $location = new Location;
        $location->user_id = $user->id;
        $location->term_id = $request->city;
        $location->latitude = $request->latitude;
        $location->longitude = $request->longitude;
        $location->role_id = 4;
        $location->save();

        $phone = new Usermeta;
        $phone->user_id = $user->id;
        $phone->type = 'info';
        $data['phone1']=$request->phone1;
        $data['phone2']=$request->phone2;
        $data['full_address']=$request->full_address;
        $phone->content = json_encode($data);
        $phone->save();

        Auth::login($user,true);

        return redirect()->route('rider.dashboard');
	}

    
}