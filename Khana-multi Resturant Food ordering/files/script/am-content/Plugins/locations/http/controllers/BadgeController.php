<?php 

namespace Amcoders\Plugin\locations\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Terms;
use App\Meta;
use Auth;
use Illuminate\Support\Str;
/**
 * 
 */
class BadgeController extends controller
{
	

	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
	 public function index(Request $request)
	 {
        
        if (!Auth()->user()->can('badge.control')) {
            return abort(401);
        }
	 	if ($request->type) {
	 		if ($request->type=='seller') {
	 			if($request->st) {
	 				if ($request->st=='trash') {
	 					$posts=Terms::with('preview')->withCount('badgeusers')->where('type',3)->where('status',0)->latest()->paginate(20);
	 					$status=$request->st;

	 				}
	 				else{
	 					$posts=Terms::with('preview')->withCount('badgeusers')->where('type',3)->where('status',$request->st)->latest()->paginate(20);
	 					$status=$request->st;

	 				}

	 			}
	 			else{
	 				$posts=Terms::with('preview')->withCount('badgeusers')->where('type',3)->latest()->where('status','!=',0)->paginate(20);
	 				$status=1;
	 			}
	 			$type=$request->type;
	 			return view('plugin::badges.seller.index',compact('posts','status','type'));
	 		}
	 		if ($request->type=='rider') {
	 			if($request->st) {
	 				if ($request->st=='trash') {
	 					$posts=Terms::with('preview')->withCount('badgeusers')->where('type',4)->where('status',0)->latest()->paginate(20);
	 					$status=$request->st;

	 				}
	 				else{
	 					$posts=Terms::with('preview')->withCount('badgeusers')->where('type',4)->where('status',$request->st)->latest()->paginate(20);
	 					$status=$request->st;

	 				}

	 			}
	 			else{
	 			$posts=Terms::with('preview')->withCount('badgeusers')->where('type',4)->latest()->where('status','!=',0)->paginate(20);
	 				$status=1;
	 			}
	 			$type=$request->type;
	 			return view('plugin::badges.rider.index',compact('posts','status','type'));



	 		}
	 		if ($request->type=='customer') {
    			if($request->st) {
	 				if ($request->st=='trash') {
	 					$posts=Terms::with('preview')->withCount('badgeusers')->where('type',5)->where('status',0)->latest()->paginate(20);
	 					$status=$request->st;

	 				}
	 				else{
	 					$posts=Terms::with('preview')->withCount('badgeusers')->where('type',5)->where('status',$request->st)->latest()->paginate(20);
	 					$status=$request->st;

	 				}

	 			}
	 			else{
	 				$posts=Terms::with('preview')->withCount('badgeusers')->where('type',5)->latest()->where('status','!=',0)->paginate(20);
	 				$status=1;
	 			}
	 			$type=$request->type;
	 			return view('plugin::badges.customer.index',compact('posts','status','type'));

	 		}


	 	}

	 	

	 	
	 }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create(Request $request)
    {
        if (!Auth()->user()->can('badge.control')) {
            return abort(401);
        }
    	if ($request->type=='rider') {
    		return view('plugin::badges.rider.create');
    	}
    	elseif ($request->type=='customer') {
    		return view('plugin::badges.customer.create');
    		
    	}
    	elseif($request->type=='seller'){
    		return view('plugin::badges.seller.create');

    	}

    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
         if (!Auth()->user()->can('badge.control')) {
            return abort(401);
        }

    	if ($request->default == 1) {
          $default=Terms::where('type',$request->type)->where('count',1)->first();
          if (!empty($default)) {
            $default->count = 0;
            $default->save();
          }
        }

    	$post=new Terms;
    	$post->title=$request->title;
    	$post->slug=$request->number;
    	$post->type=$request->type;
    	$post->auth_id=Auth::id();
    	$post->status=$request->status;
        $post->count=$request->default;
    	$post->save();


    	$post_meta = new Meta;
    	$post_meta->term_id=$post->id;
        $post_meta->type='preview';
    	$post_meta->content=$request->preview;
    	$post_meta->save();

    	return response()->json(['Badge Created']);

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
         if (!Auth()->user()->can('badge.control')) {
            return abort(401);
        }
    	$info= Terms::with('meta')->find($id);
    	if ($info->type=='3') {
    		return view('plugin::badges.seller.edit',compact('info'));
    	}
    	if ($info->type=='4') {
    		return view('plugin::badges.rider.edit',compact('info'));
    	}
    	if ($info->type=='5') {
    		return view('plugin::badges.customer.edit',compact('info'));
    	}
    	// return view('plugin::location.edit',compact('info','json'));
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

    	if ($request->default == 1) {
          $default=Terms::where('type',$request->type)->where('count',1)->first();
          if (!empty($default)) {
            $default->count = 0;
            $default->save();
          }
        }
    	$post= Terms::find($id);
    	$post->title=$request->title;
    	$post->slug=$request->number;
    	$post->auth_id=Auth::id();
    	$post->status=$request->status;
        $post->count=$request->default;
    	$post->save();

    	$post_meta = Meta::where('term_id',$id)->where('type','preview')->first();
        $post_meta->content=$request->preview;
    	$post_meta->save();

    	return response()->json(['Badge updated']);
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(Request $request)
    {
         if (!Auth()->user()->can('badge.control')) {
            return abort(401);
        }
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