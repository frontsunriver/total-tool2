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
class LocationController extends controller
{
	

	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {
        if (!Auth()->user()->can('location.list')) {
            return abort(401);
        }

        if($request->st) {
            if ($request->st=='trash') {
                 $posts=Terms::with('preview')->withCount('userslocation')->where('type',2)->where('status',0)->latest()->paginate(20);
                 $status=$request->st;
                
            }
            else{
               $posts=Terms::with('preview')->withCount('userslocation')->where('type',2)->where('status',$request->st)->latest()->paginate(20);
               $status=$request->st;
              
           }
           
       }
        else{
         $posts=Terms::with('preview')->withCount('userslocation')->where('type',2)->latest()->where('status','!=',0)->paginate(20);
        }
        $status=1;
        
        return view('plugin::location.index',compact('posts','status'));
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        if (!Auth()->user()->can('location.create')) {
            return abort(401);
        }
       return view('plugin::location.create');
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        $creat_slug=Str::slug($request->title);
        if ($creat_slug=='') {
            $slug=str_replace(' ', '-', $request->title);
        }
        else{
            $slug = $creat_slug;
        }
              

        
        $post=new Terms;
        $post->title=$request->title;
        $post->slug=$slug;
        $post->type=2;
        $post->auth_id=Auth::id();
        $post->status=$request->status;
        $post->save();

        $data['latitude']=$request->latitude;
        $data['longitude']=$request->longitude;
        $data['zoom']=$request->zoom;

        $post_meta = new Meta;
        $post_meta->term_id=$post->id;
        $post_meta->content=json_encode($data);
        $post_meta->save();

        $preview=new Meta;
        $preview->term_id=$post->id;
        $preview->content=$request->preview;
        $preview->type='preview';
        $preview->save();
        return response()->json(['Location Created']);

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
        if (!Auth()->user()->can('location.edit')) {
            return abort(401);
        }
         $info= Terms::with('preview','excerpt')->find($id);
         $json=json_decode($info->excerpt->content);

         return view('plugin::location.edit',compact('info','json'));
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
              
        $validatedData = $request->validate([
            'title' => 'required|max:100',
                      
        ]);
        $data['latitude']=$request->latitude;
        $data['longitude']=$request->longitude;
        $data['zoom']=$request->zoom;

        $creat_slug=Str::slug($request->title);
       
            

        $post= Terms::find($id);
        $post->title=$request->title;
        $post->slug=$request->slug;
        $post->lang=$request->longitude;
        $post->type=2;
        $post->auth_id=Auth::id();
        $post->status=$request->status;
        $post->save();

        $post_meta = Meta::where('term_id',$id)->where('type','excerpt')->first();
        $post_meta->content=json_encode($data);
        $post_meta->save();
        
        $post_meta = Meta::where('term_id',$id)->where('type','preview')->first();
        $post_meta->content=$request->preview;
        $post_meta->save();



        return response()->json(['Location updated']);
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(Request $request)
    {
        if (!Auth()->user()->can('location.delete')) {
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