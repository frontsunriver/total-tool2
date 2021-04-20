<?php 

namespace Amcoders\Plugin\shop\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Illuminate\Support\Str;
use Auth;
use App\Terms;
use App\User;
use App\Productmeta;

/**
 * 
 */
class CouponController extends controller
{
 

	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
     public function index(Request $request)
     {   
        $auth_id=Auth::id();

        if($request->src) {
         $posts=Terms::where('type',10)->where('auth_id',$auth_id)->where('title','LIKE','%'.$request->src.'%')->withCount('coupon')->latest()->paginate(20);
     }
     elseif($request->st) {
        if ($request->st=='trash') {
           $posts=Terms::where('type',10)->where('auth_id',$auth_id)->where('status',0)->withCount('coupon')->latest()->paginate(20);
           $status=$request->st;
           return view('plugin::coupon.index',compact('posts','auth_id','status'));
           
       }
       else{
         $posts=Terms::where('type',10)->where('auth_id',$auth_id)->where('status',$request->st)->withCount('coupon')->latest()->paginate(20);
         $status=$request->st;
         return view('plugin::coupon.index',compact('posts','auth_id','status'));
         
     }
     
 }
 else{
  $posts=Terms::where('type',10)->where('auth_id',$auth_id)->withCount('coupon')->latest()->where('status','!=',0)->paginate(20);
}
$status=1;


$posts=Terms::where('type',10)->where('auth_id',$auth_id)->withCount('coupon')->latest()->paginate(20);
return view('plugin::coupon.index',compact('posts','auth_id','status'));

}

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        return view('plugin::coupon.create');
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        $validatedData = $request->validate([
            'title' => 'required|max:100',
            'percent' => 'required|max:2',
            'expired_date' => 'required',
            
        ]);


        $post=new Terms;
        $post->title=$request->title;
        $post->slug=$request->expired_date;
        $post->type=10;
        $post->auth_id=Auth::id();
        $post->count=$request->percent;
        $post->status=$request->status;
        $post->save();       

        return response()->json(['Coupon Created']);
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

        $info=Terms::where('type',10)->where('auth_id',Auth::id())->find($id);
        return view('plugin::coupon.edit',compact('info'));
        
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
        $user=User::with('usersaas')->find(Auth::id());
        if ($user->usersaas->offer == 0) {
         $returnData['errors']['name']=array(0=>"Please Update Your Plan");
         return response()->json($returnData, 401);
     }
     
     $validatedData = $request->validate([
        'title' => 'required|max:100',
        'percent' => 'required|max:2',
        'expired_date' => 'required', 
    ]);

     $post= Terms::find($id);
     $post->title=$request->title;
     $post->slug=$request->expired_date;
     $post->count=$request->percent;
     $post->status=$request->status;
     $post->save();     

     return response()->json(['Coupon Updated']);
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