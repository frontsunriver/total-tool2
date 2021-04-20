<?php

namespace App\Http\Controllers\Api\Customer;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\User;
use Illuminate\Support\Facades\Hash;
use Auth;
use Illuminate\Support\Facades\Mail;
use App\Mail\BoookMail;

class TableController extends Controller
{
	public function store(Request $request)
	{
		$validator = \Validator::make($request->all(), [
            'number_of_gutes' => 'required',
            'date' => 'required',
            'name' => 'required',
            'email' => 'required',
            'mobile' => 'required'
        ]);

        if($validator->fails())
        {
            return response()->json(['errors'=>$validator->errors()->all()[0]]);
        }

        $data = [
        	'number_of_gutes' => $request->number_of_gutes,
        	'date' => $request->date,
        	'name' => $request->name,
        	'email' => $request->email,
        	'mobile' => $request->mobile,
        	'message' => $request->message
        ];

		$user = User::find($request->vendor_id);
		Mail::to($user->email)->send(new BoookMail($user,$data));
		return response()->json('ok');
	}
}