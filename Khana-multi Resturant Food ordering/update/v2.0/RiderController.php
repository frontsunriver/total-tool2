<?php 

namespace Amcoders\Theme\khana\http\controllers\Rider;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use Illuminate\Support\Str;
use App\Transactions;
use App\User;
use App\Order;
use Carbon\Carbon;
use App\Riderlog;
use App\Usermeta;
use App\Onesignal;
use App\Location;

class RiderController extends controller
{
	public function subscribe(Request $request)
	{
		$check=Onesignal::where('user_id',Auth::id())->where('player_id',$request->player_id)->first();
		if (empty($check)) {
			$user = new Onesignal;
			$user->user_id= Auth::id();
			$user->player_id = $request->player_id;
			$user->save();
		}

	}

	 public function information()
    {
        $info=User::with('info','location')->find(Auth::id());
        return view('theme::rider.information',compact('info'));
    }

    public function informationupdate(Request $request)
    {
        $auth_id=Auth::id();
       
        //for location
        $location=Location::where('user_id',$auth_id)->first();
        if (empty($location)) {
           $location=new Location; 
           $location->user_id=$auth_id;
        }
        $location->term_id = $request->city;
        $location->latitude = $request->latitude;
        $location->longitude = $request->longitude;
        $location->role_id = 4;
        $location->save();

        
       

        //for information
        $info=Usermeta::where('user_id',$auth_id)->where('type','info')->first();
        if (empty($info)) {
            $info=new Usermeta;
            $info->user_id=$auth_id;
            $info->type='info';
        }
        $information['phone1']=$request->phone1;
        $information['phone2']=$request->phone2;
        $information['full_address']=$request->full_address;

        $info->content=json_encode($information);
        $info->save();
       

        return response()->json(['Information Updated']);


    }


    public function status(Request $request)
    {

        $user=User::find(Auth::id());
        $user->status=$request->status;
        $user->save();
        return back();
    }

}

