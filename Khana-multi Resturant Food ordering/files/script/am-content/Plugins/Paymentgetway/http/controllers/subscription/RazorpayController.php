<?php 

namespace Amcoders\Plugin\Paymentgetway\http\controllers\subscription;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use Session;
use Illuminate\Support\Facades\Http;
use Razorpay\Api\Api;
class RazorpayController extends controller
{
    protected static $key_id;
    protected static $key_secret;
    protected static $payment_id;

    
    public static function redirect_if_payment_success()
    {
       return route('store.payment.success');
    }

    public static function redirect_if_payment_faild()
    {
        return route('store.payment.fail');
    }

    public function index()
    {
        $option=\App\Options::where('key','payment_settings')->first();
        $credentials=json_decode($option->value);

        RazorpayController::$key_id=$credentials->razorpay_key_id;
        RazorpayController::$key_secret=$credentials->razorpay_key_secret;

        $data=Session::get('order_info');
        $response=RazorpayController::make_payment($data);
        return view('plugin::razorpay',compact('response'));
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

        $api = new Api(RazorpayController::$key_id, RazorpayController::$key_secret);
        $referance_id=$ref_id;
        $order = $api->order->create(array(
            'receipt' => $referance_id,
            'amount' => $amount*100,
            'currency' => $currency
        )
        );

         // Return response on payment page
        $response = [
            'orderId' => $order['id'],
            'razorpayId' => RazorpayController::$key_id,
            'amount' => $amount*100,
            'name' => $name,
            'currency' => $currency,
            'email' => $email,
            'contactNumber' => $phone,
            'address' => "",
            'description' => $billName,
        ];

        // Let's checkout payment page is it working
        return $response;
    }


    public function status(Request $request)
    {
        $option=\App\Options::where('key','payment_settings')->first();
        $credentials=json_decode($option->value);

        RazorpayController::$key_id=$credentials->razorpay_key_id;
        RazorpayController::$key_secret=$credentials->razorpay_key_secret;
        
    // Now verify the signature is correct . We create the private function for verify the signature
        $signatureStatus = RazorpayController::SignatureVerify(
            $request->all()['rzp_signature'],
            $request->all()['rzp_paymentid'],
            $request->all()['rzp_orderid']
        );

      // If Signature status is true We will save the payment response in our database
      // In this tutorial we send the response to Success page if payment successfully made
        if($signatureStatus == true)
        {
      
            //for success
            $data['payment_id'] = RazorpayController::$payment_id;
            $data['payment_method'] = "razorpay";

            $order_info= Session::get('order_info');
            $data['ref_id'] =$order_info['ref_id'];
            $data['amount'] =$order_info['amount'];
            Session::forget('order_info');
            Session::put('payment_info', $data);
            return redirect(RazorpayController::redirect_if_payment_success());
        }
        else{
            return redirect(RazorpayController::redirect_if_payment_faild());
        }
    }

    // In this function we return boolean if signature is correct
    private static function SignatureVerify($_signature,$_paymentId,$_orderId)
    {
        try
        {
        // Create an object of razorpay class
            $api = new Api(RazorpayController::$key_id, RazorpayController::$key_secret);
            $attributes  = array('razorpay_signature'  => $_signature,  'razorpay_payment_id'  => $_paymentId ,  'razorpay_order_id' => $_orderId);
            $order  = $api->utility->verifyPaymentSignature($attributes);
            RazorpayController::$payment_id=$_paymentId;
            return true;
        }
        catch(\Exception $e)
        {
        // If Signature is not correct its give a excetption so we use try catch
            return false;
        }
    }	

}