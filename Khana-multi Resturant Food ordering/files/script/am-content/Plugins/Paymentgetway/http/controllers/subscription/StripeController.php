<?php 

namespace Amcoders\Plugin\Paymentgetway\http\controllers\subscription;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use Omnipay\Omnipay;
use Cartalyst\Stripe\Laravel\Facades\Stripe;
use Session;
class StripeController extends controller
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
         
           try {
            $charge = Stripe::charges()->create([
                'amount' => $array['amount'],
                'currency' => $array['currency'],
                'source' => $array['stripeToken'],
                'description' => "",
                'receipt_email' => $array['email'],
            ]);
           
            $data['payment_id'] = $charge['id'];
            $data['payment_method'] = "stripe";
            $order_info= Session::get('order_info');
            $data['ref_id'] =$order_info['ref_id'];
            $data['amount'] =$order_info['amount'];
            $data['vendor_id'] =$order_info['vendor_id'];
            Session::forget('order_info');
            Session::put('payment_info', $data);
            
            return redirect(StripeController::redirect_if_payment_success());


        } catch (Exception $e) {
            return redirect(StripeController::redirect_if_payment_faild());;
        }


       
        
    } 
}