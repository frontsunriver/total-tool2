<?php 

namespace Amcoders\Plugin\Paymentgetway\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use Omnipay\Omnipay;
use Session;
use Illuminate\Support\Facades\Http;
use Redirect;
use Illuminate\Http\RedirectResponse;
use App\Options;
class ToyyibpayController extends controller
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
		
		$option=\App\Options::where('key','payment_settings')->first();
		$credentials=json_decode($option->value);
		
		$phone=$array['phone'];
		$email=$array['email'];
		$amount=$array['amount'];
		$ref_id=$array['ref_id'];
		$name=$array['name'];
		$billName=$array['billName'];

		if (env('APP_DEBUG') == false) {
			$url='https://toyyibpay.com/';
		}
		else{
			$url='https://dev.toyyibpay.com/';
		}
		
		$data = array(
			'userSecretKey'=>$credentials->toyyibpay_userSecretKey,
			'categoryCode'=>$credentials->toyyibpay_categoryCode,
			'billName'=>$billName,
			'billDescription'=>"Thank you for your order",
			'billPriceSetting'=>1,
			'billPayorInfo'=>1,
			'billAmount'=>$amount*100,
			'billReturnUrl'=>route('toyyibpay.fallback'),
			'billCallbackUrl'=>route('toyyibpay.fallback'),
			'billExternalReferenceNo' => $ref_id,
			'billTo'=>$name,
			'billEmail'=>$email,
			'billPhone'=>$phone,
			'billSplitPayment'=>0,
			'billSplitPaymentArgs'=>'',
			'billPaymentChannel'=>'2',
			'billDisplayMerchant'=>1,
			'billContentEmail'=>"",
			'billChargeToCustomer'=>2
		);  

		 $f_url= $url.'index.php/api/createBill';
		
		$response= Http::asForm()->post($f_url,$data);
		$billcode=$response[0]['BillCode'];
		$url=$url.$billcode;
		return redirect($url);
		
	}


	public function status()
	{
		$response=Request()->all();
		$status=$response['status_id'];
		$payment_id=$response['billcode'];

		if ($status==1) {
			$data['payment_id'] = $payment_id;
			$data['payment_method'] = "toyyibpay";
			$order_info= Session::get('order_info');
			$data['ref_id'] =$order_info['ref_id'];
			$data['amount'] =$order_info['amount'];
			$data['vendor_id'] =$order_info['vendor_id'];
			Session::forget('order_info');
			Session::put('payment_info', $data);
			return redirect(ToyyibpayController::redirect_if_payment_success());
		}
		else{
			return redirect(ToyyibpayController::redirect_if_payment_faild());
		}

	}	

}	
