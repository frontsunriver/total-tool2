<?php 

namespace Amcoders\Plugin\plan\http\controllers\Store;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Plan;
use App\User;
use App\Userplan;
use Auth;
use Carbon\Carbon;
use Cartalyst\Stripe\Laravel\Facades\Stripe;
use DateTime;
use Amcoders\Plugin\Paymentgetway\http\controllers\subscription\PaypalController;
use Amcoders\Plugin\Paymentgetway\http\controllers\subscription\ToyyibpayController;
use Amcoders\Plugin\Paymentgetway\http\controllers\subscription\InstamojoController;
use Amcoders\Plugin\Paymentgetway\http\controllers\subscription\RazorpayController;
use Session;
class PlanController extends controller
{
	public function index()
	{
		$plans = Plan::where('status',1)->latest()->get();
		return view('plugin::store.plan.index',compact('plans'));
	}

	public function checkout($id)
	{
		$plan = Plan::find(decrypt($id));
		 $option=\App\Options::where('key','payment_settings')->first();
		 $credentials=json_decode($option->value);
		return view('plugin::store.plan.checkout',compact('plan','credentials'));
	}

	public function payment(Request $request)
	{

		$plan = Plan::find($request->id);
		$currency=\App\Options::where('key','currency_name')->select('value')->first();
		$price=$plan->s_price;
		$currency=strtoupper($currency->value);
		$email=Auth::User()->email;
		$phone=$request->phone;

		if($request->type == 'stripe')
		{
			
			try {
	            $charge = Stripe::charges()->create([
	                'amount' => $price,
	                'currency' => $currency,
	                'source' => $request->stripeToken,
	                'description' => '',
	                'receipt_email' => Auth::User()->email,
	                
	            ]);

	            $userplan = new Userplan();
	            $userplan->user_id = Auth::User()->id;
	            $userplan->plan_id = $plan->id;
	            $userplan->payment_method = "stripe";
	            $userplan->payment_status = 'approved';
	            $userplan->status = 'approved';
	            $userplan->image = $charge['id'];

	            $userplan->amount = $plan->s_price;
	            $userplan->save();

	            $user = Auth::User();
	            $user->plan_id = $plan->id;
	            $user->save();


	        } catch (Exception $e) {
	        	return back();

	        }
		}
		else{		

			$data['ref_id']=$plan->id;
			$data['amount']=$price;
			$data['email']=Auth::user()->email;
			$data['name']=Auth::user()->name;
			$data['phone']=$request->phone;
			$data['billName']=$plan->name;
			$data['currency']=$currency;
			Session::put('order_info',$data);

			if ($request->type=='paypal') {
				return PaypalController::make_payment($data);
			}

			if ($request->type=='toyyibpay') {
				return ToyyibpayController::make_payment($data);

			}

			if ($request->type=='razorpay') {
				return redirect('/store/payment-with/razorpay');
			}

			if ($request->type=='instamojo') {

				return InstamojoController::make_payment($data);

			}
		}

		return redirect()->route('store.plan');
	}

	


	public function planCheck(Request  $request)
	{
		if (!$request->ajax()) {
			abort(404);
		}
		$latsEntrol= Userplan::where('user_id',Auth::id())->where('payment_status','approved')->with('usersaas')->latest()->first();
		if (!empty($latsEntrol)) {
		$start = Carbon::parse($latsEntrol->updated_at)->format('Y/m/d');
		$end =  date('Y/m/d');

		$datetime1 = new DateTime($start);
		$datetime2 = new DateTime($end);
		$interval = $datetime1->diff($datetime2);
	    $days = $interval->format('%a');
	    if (!empty($latsEntrol->usersaas)) {
	    	$type = $latsEntrol->usersaas->duration;
	    	if ($type=='month') {
	    		$time=30;
	    	}
	    	elseif($type=='year'){
	    		$time=365;
	    	}
	    }
	    else{
	    	$time=30;
	    }

	    if ($days >= $time) {
	    	$user = User::find(Auth::id());
	    	$user->plan_id=1;
	    	$user->save();

	    	$plan = new Userplan;
	    	$plan->user_id = Auth::id();
	    	$plan->plan_id  = 1;
	    	$plan->payment_method  = "default";
	    	$plan->payment_status  = "approved";
	    	$plan->status  = "approved";
	    	$plan->amount  = 0;
	    	$plan->save();
	    }
	    return "";
	  }
	  else{
	  	return "";
	  }

	   
	}

	public function success()
	{
		if (Session::has('payment_info')) {
			$data= Session::get('payment_info');
			$userplan = new Userplan();
			$userplan->user_id = Auth::User()->id;
			$userplan->plan_id = $data['ref_id'];
			$userplan->payment_method = $data['payment_method'];
			$userplan->payment_status = 'approved';
			$userplan->status = 'approved';
			$userplan->image = $data['payment_id'];
			$userplan->amount = $data['amount'];
			$userplan->save();

			$user = User::find(Auth::id());
			$user->plan_id = $data['ref_id'];
			$user->save();
			Session::forget('payment_info');
			Session::flash('success', "Thanks for subscribe");
			return redirect()->route('store.plan');
		}
		return abort(404);
		
	}

	public function fail()
	{
		Session::forget('order_info');
		Session::forget('payment_info');
		Session::flash('error', "Transaction Fail");
		return redirect()->route('store.plan');
	}
}