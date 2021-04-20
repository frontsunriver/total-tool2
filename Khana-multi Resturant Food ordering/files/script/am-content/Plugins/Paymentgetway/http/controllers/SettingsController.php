<?php 

namespace Amcoders\Plugin\Paymentgetway\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use App\Options;

class SettingsController extends controller
{
    public function index()
    {
        if (!Auth()->user()->can('payment.settings')) {
        return abort(401);
        }
        $payment = Options::where('key','payment_settings')->first();
        if(isset($payment))
        {
            $paymentmeta = json_decode($payment->value);
        }else{
            $paymentmeta = null;
        }
        return view('plugin::settings.index',compact('paymentmeta'));
    }

    public function update(Request $request)
    {
        $data = [
            'paypal_client_id' => $request->paypal_client_id,
            'paypal_secret' => $request->paypal_secret,
            'paypal_status' => $request->paypal_status,
            'instamojo_x_api_key' => $request->instamojo_x_api_key,
            'instamojo_x_auto_token' => $request->instamojo_x_auto_token,
            'instamojo_status' => $request->instamojo_status,
            'razorpay_key_id' => $request->razorpay_key_id,
            'razorpay_key_secret' => $request->razorpay_key_secret,
            'razorpay_status' => $request->razorpay_status,
            'toyyibpay_userSecretKey' => $request->toyyibpay_userSecretKey,
            'toyyibpay_categoryCode' => $request->toyyibpay_categoryCode,
            'toyyibpay_status' => $request->toyyibpay_status
        ];

        $payment_settings = Options::where('key','payment_settings')->first();

        if(isset($payment_settings))
        {
            $payment_settings->value = json_encode($data);
            $payment_settings->save();
        }else{
            $payment = new Options();
            $payment->key = 'payment_settings';
            $payment->value = json_encode($data);
            $payment->save();
        }

        return response()->json('Settings Updated');

    }
}