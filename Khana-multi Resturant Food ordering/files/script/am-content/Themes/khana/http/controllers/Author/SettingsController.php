<?php 

namespace Amcoders\Theme\khana\http\controllers\Author;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use Hash;
/**
 * 
 */
class SettingsController extends controller
{
	public function update(Request $request)
	{

		$user = Auth::User();
		$validator = \Validator::make($request->all(), [
            'name' => 'required',
            'email' => 'unique:users,email,' . $user->id,
        ]);

        if($validator->fails())
        {
            return response()->json(['error'=>$validator->errors()->all()[0]]);
        }

        
        $user->email = $request->email;
        $user->name = $request->name;
        $user->save();

        if($request->current_password)
        {
        	$validator = \Validator::make($request->all(), [
	            'current_password' => 'required|password',
            	'password' => 'required|confirmed'
	        ]);

	        if($validator->fails())
	        {
	            return response()->json(['error'=>$validator->errors()->all()[0]]);
	        }

	        $user->password = Hash::make($request->password);
	        $user->save();

	        return response()->json('ok');
            
        }
        return response()->json('ok');
	}
}