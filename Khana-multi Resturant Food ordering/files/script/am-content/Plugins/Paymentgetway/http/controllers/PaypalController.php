<?php 

namespace Amcoders\Plugin\Paymentgetway\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use Omnipay\Omnipay;
use Session;

class PaypalController extends controller
{
	    
    public static function redirect_if_payment_success()
    {
       return route('payment.success');
    }

    public static function redirect_if_payment_faild()
    {
        return route('payment.fail');
    }

    public static function make_payment($array)
    {   
        $phone=$array['phone'];
        $email=$array['email'];
        $amount=$array['amount'];
        $ref_id=$array['ref_id'];
        $name=$array['name'];
        $billName=$array['billName'];
        $currency=$array['currency'];

        $option=\App\Options::where('key','payment_settings')->first();
        $credentials=json_decode($option->value);
                
        $gateway = Omnipay::create('PayPal_Rest');
        $gateway->setClientId($credentials->paypal_client_id);
        $gateway->setSecret($credentials->paypal_secret);
        $gateway->setTestMode(env('APP_DEBUG')); 

        $response = $gateway->purchase(array(
            'amount' => $amount/100,
            'currency' => strtoupper($currency),
            'returnUrl' => route('paypal_fallback'),
            'cancelUrl' => PaypalController::redirect_if_payment_faild(),
        ))->send();
        if ($response->isRedirect()) {
            $response->redirect(); // this will automatically forward the customer
        } else {
            // not successful
            return redirect(PaypalController::redirect_if_payment_faild());
        }
    }

    public function paypal_success_payment(Request $request)
    {
        $option=\App\Options::where('key','payment_settings')->first();
        $credentials=json_decode($option->value);

        $gateway = Omnipay::create('PayPal_Rest');
        $gateway->setClientId($credentials->paypal_client_id);
        $gateway->setSecret($credentials->paypal_secret);
        $gateway->setTestMode(env('APP_DEBUG')); 

        $request= $request->all();

        $transaction = $gateway->completePurchase(array(
            'payer_id'             => $request['PayerID'],
            'transactionReference' => $request['paymentId'],
        ));
        $response = $transaction->send();
        if ($response->isSuccessful()) {
            $arr_body = $response->getData();
            $data['payment_id'] = $arr_body['id'];
            $data['payment_method'] = "paypal";

            $order_info= Session::get('order_info');
            $data['ref_id'] =$order_info['ref_id'];
            $data['amount'] =$order_info['amount'];
            $data['vendor_id'] =$order_info['vendor_id'];
            Session::forget('order_info');
            Session::put('payment_info', $data);
            return redirect(PaypalController::redirect_if_payment_success());
        }
        else{
           return redirect(PaypalController::redirect_if_payment_faild());
        }
    }


}