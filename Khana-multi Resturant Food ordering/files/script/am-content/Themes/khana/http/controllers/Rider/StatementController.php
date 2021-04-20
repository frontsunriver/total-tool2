<?php 

namespace Amcoders\Theme\khana\http\controllers\Rider;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use Illuminate\Support\Str;
use App\Transactions;
use App\Order;
use Carbon\Carbon;
use App\Riderlog;
use App\Usermeta;

class StatementController extends controller
{

	public function earnings(Request $request)
	{
		$auth_id=Auth::id();
		$totals= Riderlog::where('user_id',$auth_id)->wherehas('completed')->where('status',1)->sum('commision');
		$currentMonthAmount=Riderlog::where('user_id',$auth_id)->wherehas('completed')->where('status',1)->whereYear('created_at', date('Y'))->where('created_at','>=',Carbon::now()->subdays(30))->sum('commision');

		$allorders= Riderlog::where('user_id',$auth_id)->wherehas('completed')->where('status',1)->latest()->paginate(30);

		$lastTransection= Transactions::where('user_id',$auth_id)->where('status',1)->sum('amount');
		if (!empty($request->date)) {

			$date = $request->date;
			$starting_date = substr($date, 0,10);
			$start = date( "Y-m-d h:i:s", strtotime($starting_date));
			$ending_date = substr($date, -10);
			$end = date( "Y-m-d h:i:s", strtotime($ending_date));


			$allorders=Riderlog::where('user_id',$auth_id)->wherehas('completed')->where('status',1)->whereBetween('created_at',[$start,$end])->paginate(100);

			return view('theme::rider.statement.earning',compact('totals','currentMonthAmount','allorders','date','lastTransection'));

		}


		return view('theme::rider.statement.earning',compact('totals','currentMonthAmount','allorders','lastTransection'));
	}


	public function payout()
	{
		$auth_id=Auth::id();
		$payouts=Transactions::where('user_id',$auth_id)->latest()->paginate(24);
		$totalWithdraw=Transactions::where('user_id',$auth_id)->where('status',1)->sum('amount');
		$currentMonthAmount=Riderlog::where('user_id',$auth_id)->wherehas('completed')->where('status',1)->sum('commision');
		$total= number_format($currentMonthAmount-$totalWithdraw,2);

		return view('theme::rider.statement.payout',compact('payouts','total','totalWithdraw'));    

	}




	public function setup()
	{
		$paypal=Usermeta::where('user_id',Auth::id())->where('type','paypal_info')->first();


		$bank=Usermeta::where('user_id',Auth::id())->where('type','bank_info')->first();
		$bank=json_decode($bank->content ?? '');
		return view('theme::rider.statement.edit',compact('paypal','bank'));    

	}

	public function paypalSetup(Request $request)
	{
		$validatedData = $request->validate([
			'email' => 'required|email|max:50',
		]);

		if ($request->email !== $request->confirm) {
			$validatedData = $request->validate([
				'email' => 'required|email|max:50|confirmed',
			]);
		}

		$paypal=Usermeta::where('user_id',Auth::id())->where('type','paypal_info')->first();
		if (empty($paypal)) {
			$paypal= new Usermeta;  
		}
		$paypal->user_id = Auth::id();
		$paypal->type='paypal_info';
		$paypal->content=$request->email;
		$paypal->save();

		return response()->json("Paypal Setup Complete");
	}

	public function bankSetup(Request $request)
	{
		$validatedData = $request->validate([
			'bank_name' => 'required|max:50',
			'branch_name' => 'required|max:50',
			'holder_name' => 'required|max:50',
			'account_number' => 'required|max:50',
		]);
		$bank=Usermeta::where('user_id',Auth::id())->where('type','bank_info')->first();
		if (empty($bank)) {
			$bank= new Usermeta;  
		}
		$data['bank_name']=$request->bank_name;
		$data['branch_name']=$request->branch_name;
		$data['holder_name']=$request->holder_name;
		$data['account_number']=$request->account_number;

		$bank->user_id = Auth::id();
		$bank->type='bank_info';
		$bank->content=json_encode($data);
		$bank->save();

		return response()->json("Bank Setup Complete");
	}


	public function withdraw(Request $request)
	{
		$auth_id=Auth::id();
		$check=Transactions::where('user_id',$auth_id)->whereDate('created_at', Carbon::today())->first();
		if (!empty($check)) {
			$returnData['errors']['name']=array(0=>"You Can Withdraw Only Per One Day");
			return response()->json($returnData, 401);
		}

		$withdraw=new Transactions;
		$withdraw->user_id = $auth_id;
		$withdraw->amount = $request->tk;
		$withdraw->payment_mode = $request->method;
		$withdraw->Save();

		return response()->json(['Withdrawal in process']);

	}
}
