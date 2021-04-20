<?php 

namespace Amcoders\Plugin\plan\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Plan;
use App\Userplan;
use App\User;
/**
 * 
 */
class PlanController extends controller
{
	public function index(Request $request)
	{
		if (!Auth()->user()->can('plan.list')) {
			return abort(401);
		}
		if($request->st) {
            if ($request->st=='trash') {
                 $posts=Plan::where('status',0)->withCount('countusers')->latest()->paginate(20);
                 $status=$request->st;
                 return view('plugin::plan.index',compact('posts','status'));

             }
             else{
             	$posts=Plan::where('status',$request->st)->withCount('countusers')->latest()->paginate(20);
             	$status=$request->st;
             	return view('plugin::plan.index',compact('posts','status'));

             }

         }
        $status=1;
		$posts = Plan::where('status',1)->withCount('countusers')->paginate(20);
		return view('plugin::plan.index',compact('posts','status'));
	}

	public function create()
	{
		if (!Auth()->user()->can('plan.create')) {
			return abort(401);
		}
		return view('plugin::plan.create');	
	}
	public function store(Request $request)
	{
		$validatedData = $request->validate([
			'name' => 'required|unique:plan_meta|max:30',
			
		]);
		
		 $post = new Plan;
		 $post->name = $request->name;
		 $post->s_price = $request->s_price;
		 $post->img_limit = $request->img_limit;
		 $post->duration = $request->duration;
		 $post->commission = $request->commission;
		 $post->f_resturent = $request->f_resturent;
		 $post->table_book = $request->table_book;
		 $post->status = $request->status;
		 $post->save();
		

		return response()->json(['Plan Created']);
	}
	public function edit($id)
	{
		if (!Auth()->user()->can('plan.edit')) {
			return abort(401);
		}
		$info=Plan::find($id);

		return view('plugin::plan.edit',compact('info'));

	}
	public function show(Request $request,$id)
	{
		if (!Auth()->user()->can('plan.view')) {
			return abort(401);
		}
		if (!empty($request->src) && !empty($request->type)) {
			$users=User::where('plan_id',$id)->where($request->type,$request->src)->with(['saasmeta'=>function($q){
				return $q->where('status',1)->orderBy('id','DESC')->first();
			}])->paginate(30);	
		}
		else{
			$users=User::where('plan_id',$id)->with(['saasmeta'=>function($q){
			return $q->where('status',1)->orderBy('id','DESC')->first();
		   }])->paginate(30);
		}
		$src=$request->src ?? '';
		return view('plugin::plan.show',compact('users','id','src'));
	}

	public function UserPlan($id)
	{
		$info=User::with('info')->find($id);
		$transections=Userplan::with('usersaas')->where('user_id',$id)->latest()->paginate(30);
		$plans = Plan::all();
		return view('plugin::plan.userplan',compact('info','plans','transections'));;
	}


	public function update(Request $request, $id)
	{
		$validatedData = $request->validate([
			'name' => 'required|max:30',
		]);

		$post =  Plan::find($id);
		$post->name = $request->name;
		$post->s_price = $request->s_price;
		$post->img_limit = $request->img_limit;
		$post->duration = $request->duration;
		$post->commission = $request->commission;
		$post->f_resturent = $request->f_resturent;
		$post->table_book = $request->table_book;
		$post->status = $request->status;
		$post->save();
		

		return response()->json(['Plan Updated']);
		
	}

	public function destroy(Request $request)
	{
		if (!Auth()->user()->can('plan.delete')) {
			return abort(401);
		}
		if ($request->status=='publish') {
            if ($request->ids) {

                foreach ($request->ids as $id) {
                    $post=Plan::find($id);
                    $post->status=1;
                    $post->save();   
                }
                    
            }
        }
        elseif ($request->status=='trash') {
            if ($request->ids) {
                foreach ($request->ids as $id) {
                    $post=Plan::find($id);
                    $post->status=0;
                    $post->save();   
                }
                    
            }
        }
        elseif ($request->status=='delete') {
            if ($request->ids) {
                foreach ($request->ids as $id) {
                   Plan::destroy($id);
                   
                }
            }
          return response()->json('Plan Removed');
        }
      
        
        return response()->json('Status Changed');
	}


	public function payment(Request $request)
	{
		if (!Auth()->user()->can('payment.request')) {
			return abort(401);
		}
		if ($request->src && $request->type) {

			$plans = Userplan::where($request->type,$request->src)->with('user')->latest()->paginate(20);
		}
		else{
			$plans = Userplan::with('user')->latest()->paginate(20);
		}
		
		$src=$request->src ?? '';

		return view('plugin::plan.payment',compact('plans','src'));
	}

	public function approved($id)
	{
		$main_id = decrypt($id);
		$info = Userplan::find($main_id);
		$plans = Plan::latest()->get();
		return view('plugin::plan.editpayment',compact('info','plans'));
	}

	public function payment_update(Request $request,$id)
	{
		if (!empty($request->user_id)) {
			$plan=Userplan::find($id);
			$plan->user_id =  $request->user_id;
			$plan->plan_id  = $request->plan_id;
			$plan->payment_status  = $request->payment_status;
			$plan->status  = $request->status;
			$plan->save();
		}
		if ($request->payment_status=='approved' && !empty($request->user_id)) {
			$user=User::find($request->user_id);
			$user->plan_id =  $request->plan_id;
			$user->save();
		}

		return response()->json(['Plan Updated']);
	}


	public function delete($id)
	{
		$main_id = decrypt($id);
		$userplan = Userplan::find($main_id)->delete();

		return back();
	}

	public function payment_create()
	{
		if (!Auth()->user()->can('payment.make')) {
			return abort(401);
		}
		$userplans = Userplan::latest()->get();
		$plans = Plan::latest()->get();
		$users = User::where('role_id',3)->get();
		return view('plugin::plan.payment_create',compact('plans','users','userplans'));
	}

	public function payment_store(Request $request)
	{
		
		if (!empty($request->user_id)) {
			$plan=new Userplan;
			$plan->user_id =  $request->user_id;
			$plan->plan_id  = $request->plan_id;
			$plan->payment_method  = "custom";
			$plan->payment_status  = $request->payment_status;
			$plan->status  = $request->status;
			$plan->amount  = Plan::find($request->plan_id)->s_price;
			$plan->save();
		}
		if ($request->payment_status=='approved' && !empty($request->user_id)) {
			$user=User::find($request->user_id);
			$user->plan_id =  $request->plan_id;
			$user->save();
		}
		
		

		return response()->json(['Plan Created']);
	}

	public function user(Request $request)
	{
		$user=User::where('email',$request->email)->where('role_id',3)->first();
		if (!empty($user)) {
			return response()->json(['data'=>$user->id]);
		}
		$returnData['errors']['name']=array(0=>"User Not Found");
        return response()->json($returnData, 401);
	}
}

