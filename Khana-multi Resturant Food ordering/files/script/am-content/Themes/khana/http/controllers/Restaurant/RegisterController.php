<?php 

namespace Amcoders\Theme\khana\http\controllers\Restaurant;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Terms;
use App\User;
use App\Location;
use Auth;
use Hash;
use Illuminate\Support\Str;
use App\Usermeta;
use Carbon\Carbon;
use File;
use App\Media;
/**
 * 
 */
class RegisterController extends controller
{
	public function index()
	{
        if(Auth::check())
        {
            return redirect()->route('store.dashboard');
        }else{
            return view('theme::store.register_step_1');
        }
	} 

	public function store(Request $request)
	{
		$validator = \Validator::make($request->all(), [
            'name' => 'required',
            'email' => 'required|unique:users',
            'password' => 'required|confirmed',
            'delivery' => 'required',
            'pickup' => 'required',
            'phone_number_1' => 'required',
            'phone_number_2' => 'required',
            'email_address_1' => 'required',
            'email_address_2' => 'required',
            'description' => 'required'
        ]);

        if($validator->fails())
        {
            return back()->with('errors',$validator->errors()->all()[0]);
        }

        $user_slug=Str::slug($request->name);
        $user = User::where('slug',$user_slug)->first();

        if ($user) {
            $slug= Str::slug($request->name).Str::random(5);
        }
        else{
            $slug=Str::slug($request->name);
        }

        $badge = Terms::where('type',3)->where('status',1)->where('count',1)->first();

        $user = new User();
        $user->role_id = 3;
        $user->name = $request->name;
        $user->slug = $slug;
        $user->email = $request->email;
        $user->plan_id = 1;
        $user->password = Hash::make($request->password);
        if (!empty($badge)) {
           $user->badge_id = $badge->id; 
        }
        $user->save();

        Auth::login($user);

        $usermeta_1 = new Usermeta();
        $usermeta_1->user_id = $user->id;
        $usermeta_1->type = 'delivery';
        $usermeta_1->content = $request->delivery;
        $usermeta_1->save();

        $usermeta_2 = new Usermeta();
        $usermeta_2->user_id = $user->id;
        $usermeta_2->type = 'pickup';
        $usermeta_2->content = $request->pickup;
        $usermeta_2->save();

        $usermeta_3 = new Usermeta();
        $usermeta_3->user_id = $user->id;
        $usermeta_3->type = 'rattings';
        $usermeta_3->content = 0;
        $usermeta_3->save();

        $usermeta_4 = new Usermeta();
        $usermeta_4->user_id = $user->id;
        $usermeta_4->type = 'avg_rattings';
        $usermeta_4->content = 0;
        $usermeta_4->save();

        $usermeta_5 = new Usermeta();
        $usermeta_5->user_id = $user->id;
        $usermeta_5->type = 'info';
        $usermeta_5->content = '{"description":"'.$request->description.'","phone1":"'.$request->phone_number_1.'","phone2":"'.$request->phone_number_2.'","email1":"'.$request->email_address_1.'","email2":"'.$request->email_address_2.'","address_line":null,"full_address":null}';
        $usermeta_5->save();

        $usermeta_6 = new Usermeta();
        $usermeta_6->user_id = $user->id;
        $usermeta_6->type = 'gallery';
        $usermeta_6->content = null;
        $usermeta_6->save();

        $usermeta_7 = new Usermeta();
        $usermeta_7->user_id = $user->id;
        $usermeta_7->type = 'preview';
        $usermeta_7->content = null;
        $usermeta_7->save();


        return redirect()->route('restaurant.register_step_2');
	}

	public function step_2()
	{
        $location = Location::where('user_id',Auth::User()->id)->first();
        if($location)
        {
            return redirect()->route('store.dashboard');
        }
        $cities = Terms::where('type',2)->get();
		return view('theme::store.register_step_2',compact('cities'));
	}

    public function step_2_store(Request $request)
    {

        $user = Auth::User();
        $location = new Location();
        $location->user_id = $user->id;
        $location->term_id = $request->city;
        $location->latitude = $request->latitude;
        $location->longitude = $request->longitude;
        $location->role_id = 3;
        $location->save();

        $usermeta = Usermeta::where([
            ['user_id',$user->id],
            ['type','info']
        ])->first();
        $user_data = json_decode($usermeta->content);
        $user_data->address_line = $request->address_line;
        $user_data->full_address = $request->full_address;

        $usermeta->content = json_encode($user_data);
        $usermeta->save();

        return redirect()->route('restaurant.register_step_4');

        
    }

    public function step_3()
    {
        //return view('theme::store.register_step_3');
    }

    public function step_3_store(Request $request)
    {
        $this->validate($request,[
            'cover_img' => 'required|image',
            'logo_img' => 'required|image'
        ]);

        $logo_file = $request->file('logo_img');
        if (isset($logo_file)) {
            $curentdate = Carbon::now()->toDateString();
            $imagename =  $curentdate . '-' . uniqid() . '.' . $logo_file->getClientOriginalExtension();


            $path = 'uploads/';

            $logo_file->move($path, $imagename);

            $logo_file_name = $path.$imagename;


            $media = new Media();
            $media->user_id = Auth::User()->id;
            $media->name = $path.$imagename;
            $media->type = $logo_file->getClientOriginalExtension();
            $schemeurl=parse_url(url('/'));
            if ($schemeurl['scheme']=='https') {
               $url=substr(url('/'), 6);
            }
            else{
                 $url=substr(url('/'), 5);
            }
            $media->url = $url.'/'.$path.$imagename;
            $media->size = File::size('uploads/'.$imagename).'kib';
            $media->path = 'uploads/';
            $media->save();

            $user = Auth::User();
            $user->avatar = $url.'/'.$path.$imagename;
            $user->save();

        }else{
            $imagename = 'default.png';
        }

         $cover_file = $request->file('cover_img');
        if (isset($cover_file)) {
            $curentdate = Carbon::now()->toDateString();
            $imagename =  $curentdate . '-' . uniqid() . '.' . $cover_file->getClientOriginalExtension();


            $path = 'uploads/';

            $cover_file->move($path, $imagename);

            $cover_file_name = $path.$imagename;

            $main_cover_file_path = 'uploads/'. $imagename;

            $media = new Media();
            $media->user_id = Auth::User()->id;
            $media->name = $path.$imagename;
            $media->type = $cover_file->getClientOriginalExtension();
            $schemeurl=parse_url(url('/'));
            if ($schemeurl['scheme']=='https') {
               $url=substr(url('/'), 6);
            }
            else{
                 $url=substr(url('/'), 5);
            }
            $media->url = $url.'/'.$path.$imagename;
            $media->size = File::size('uploads/'.$imagename).'kib';
            $media->path = 'uploads/';
            $media->save();

            $usermeta = Usermeta::where([
                ['type','preview'],
                ['user_id',Auth::User()->id]
            ])->first();
            $usermeta->content = $url.'/'.$path.$imagename;
            $usermeta->save();

        }else{
            $imagename = 'default.png';
        }

        return redirect()->route('restaurant.register_step_4');
    }

    public function step_4()
    {
        return view('theme::store.register_step_4');
    }
}