<?php 
namespace Amcoders\Plugin\locations\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Illuminate\Support\Str;
use Auth;
use App\Plan;
use App\Featured;
use App\User;

/**
 * 
 */
class FeaturedController extends controller
{
	
	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {   
        if (!Auth()->user()->can('featured.control')) {
            return abort(401);
        }
        $auth_id=Auth::id();

        if($request->src) {
           $posts=Terms::with('price')->where('type',8)->where('auth_id',$auth_id)->where('title','LIKE','%'.$request->src.'%')->latest()->paginate(20);
        }
        elseif($request->st) {
            if ($request->st=='trash') {
                 $posts=Terms::with('price')->where('type',8)->where('auth_id',$auth_id)->where('status',0)->latest()->paginate(20);
                 $status=$request->st;
        return view('plugin::addon-product.index',compact('posts','auth_id','status'));
        
            }
            else{
               $posts=Terms::with('price')->where('type',8)->where('auth_id',$auth_id)->where('status',$request->st)->latest()->paginate(20);
               $status=$request->st;
        return view('plugin::addon-product.index',compact('posts','auth_id','status'));
        
           }
           
       }
        else{
          $posts=Terms::with('price')->where('type',8)->where('auth_id',$auth_id)->latest()->where('status','!=',0)->paginate(20);
        }
        $status=1;


        $posts=Terms::with('price')->where('type',8)->where('auth_id',$auth_id)->latest()->paginate(20);
        return view('plugin::addon-product.index',compact('posts','auth_id','status'));
        
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create(Request $request)
    {
        if (!Auth()->user()->can('featured.control')) {
            return abort(401);
        }
       $type=$request->type;
       if ($type=='seller') {
       	if ($request->manage) {
       	   $posts=User::join('featured_user','featured_user.user_id','users.id')
       		->where('featured_user.type',$request->manage)
       		->select('users.id as id','users.avatar as avatar','users.name as name','users.email as email','featured_user.created_at as created_at','featured_user.id as f_id')
       		->get();
       		return view('plugin::featured.seller.manage',compact('posts'));	
       	}
       	else{

       	if ($request->plan) {
       		$posts=User::where('role_id',3)->where('plan_id',$request->plan)->with('plan')->latest()->paginate(100);
       	}
       	else{
       		 $posts=User::where('role_id',3)->with('plan')->latest()->paginate(100);
       	}
       }
         $plans=Plan::where('status',1)->get();
         return view('plugin::featured.seller.create',compact('type','plans','posts'));				
       }

       return view('plugin::addon-product.create');
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
       
       foreach ($request->ids as $key => $row) {
            Featured::where('type',$request->status)->where('user_id',$row)->delete();
        	$user=new Featured;
        	$user->user_id = $row;
        	$user->type = $request->status;
        	$user->save();
        } 

        return response()->json(['Update Success']);
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        //
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit($id)
    {

       
        return view('plugin::addon-product.edit',compact('info'));
        
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $id)
    {
    	//dd($request);
       if ($request->status=='trash') {
            if ($request->ids) {
                foreach ($request->ids as $id) {
                   Featured::destroy($id);
                    
                }
                    
            }
        }
        

        return response()->json(['Updated Success']);
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(Request $request)
    {
        
        if ($request->status=='publish') {
            if ($request->ids) {

                foreach ($request->ids as $id) {
                    $post=Terms::find($id);
                    $post->status=1;
                    $post->save();   
                }
                    
            }
        }
        elseif ($request->status=='trash') {
            if ($request->ids) {
                foreach ($request->ids as $id) {
                    $post=Terms::find($id);
                    $post->status=0;
                    $post->save();   
                }
                    
            }
        }
        elseif ($request->status=='delete') {
            if ($request->ids) {
                foreach ($request->ids as $id) {
                   Terms::destroy($id);
                   
                }
            }
        }
        return response()->json('Success');

    }
}