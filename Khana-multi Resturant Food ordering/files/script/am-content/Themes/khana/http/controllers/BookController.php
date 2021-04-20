<?php 

namespace Amcoders\Theme\khana\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\User;
use App\category;
use App\Terms;
use Cart;
use Illuminate\Support\Facades\Mail;
use App\Mail\BoookMail;
/**
 * 
 */
class BookController extends controller
{
	public function store(Request $request,$slug)
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

		$user = User::where('slug',$slug)->first();
		Mail::to($user->email)->send(new BoookMail($user,$data));
		return response()->json('ok');

	}
}