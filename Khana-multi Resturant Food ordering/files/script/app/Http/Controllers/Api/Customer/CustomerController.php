<?php

namespace App\Http\Controllers\Api\Customer;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\User;
use Illuminate\Support\Facades\Hash;
use Auth;
class CustomerController extends Controller
{

    //customer login
    public function Login(Request $request)
    {
        $data=$request->validate([
            'email'=> 'required',
            'password'=> 'required',

        ]);

        $user = User::where('email', $request->email)->first();

        if (! $user || ! Hash::check($request->password, $user->password)) {
            return response()->json([
                'email' => ['The provided credentials are incorrect.'],
            ],401);
        }

        $token= $user->createToken('token')->plainTextToken;
        $token=explode('|', $token);
        return response()->json(['token'=>$token[1],'login_id'=>$token[0]]);
    }


    //customer register
    public function Register(Request $request)
    {
        
        $validator = \Validator::make($request->all(), [
            'name' => ['required', 'string', 'max:255'],
            'email' => ['required', 'string', 'email', 'max:255', 'unique:users'],
            'password' => ['required', 'string', 'min:8'],
        ]);

        if($validator->fails())
        {
            return response()->json(['errors'=>$validator->errors()->all()[0]]);
        }

        $user= User::create([
            'name' => $request->name,
            'role_id' => 2,
            'email' => $request->email,
            'password' => Hash::make($request->password),
        ]);

        $token= $user->createToken('token')->plainTextToken;
        $token=explode('|', $token);
        return response()->json(['token'=>$token[1],'login_id'=>$token[0]]);
    }


    //user logut
    public function Logout(Request $request)
    {
        $user = $request->user();         
        $user->tokens()->where('id', $user->currentAccessToken()->id)->delete();
        return response()->json('logout success');
    }


    //user basic info
    public function Info(Request $request)
    {
        $data = $request->user();
        $info['id']=$data->id;
        $info['name']=$data->name;
        $info['avatar']=asset($data->avatar);
        $info['email']=$data->email;
        $info['status']=$data->status;
        $info['email_verified_at']=$data->email_verified_at;
        return response()->json($info);
    }

    
}
