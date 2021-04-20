<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Terms;
use App\Meta;
use App\Post;
use Auth;
use Illuminate\Support\Str;
class PageController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {
        if (!Auth()->user()->can('page.list')) {
            return abort(401);
        }
         if($request->src) {
           $pages=Terms::where('type',1)->where('title','LIKE','%'.$request->src.'%')->latest()->paginate(20);
        }
        elseif($request->st) {
            if ($request->Terms=='trash') {
                 $pages=Terms::where('type',1)->where('status',0)->latest()->paginate(20);
                 $status=$request->st;
                 return view('admin.page.index',compact('pages','status'));
            }
            else{
               $pages=Terms::where('type',1)->where('status',$request->st)->latest()->paginate(20);
               $status=$request->st;
               return view('admin.page.index',compact('pages','status')); 
           }
           
       }
        else{
        $pages=Terms::where('type',1)->latest()->where('status','!=',0)->paginate(20);
        }
       
        return view('admin.page.index',compact('pages'));
      
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        if (!Auth()->user()->can('page.create')) {
            return abort(401);
        }
        return view('admin.page.create');
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
            
        ]);


        $creat_slug=Str::slug($request->title);
        $check=Terms::where('type',1)->where('slug',$creat_slug)->count();
        if ($check != 0) {
            $slug=$creat_slug.'-'.$check;
        }
        else{
            $slug=$creat_slug;
        }

        


        $post=new Terms;
        $post->title=$request->title;
        $post->slug=$slug;
        $post->type=$request->type;
        $post->auth_id=Auth::id();
        $post->status=$request->status;
        $post->save();

        $post_meta = new Meta;
        $post_meta->term_id=$post->id;
        $post_meta->type='excerpt';
        $post_meta->content=$request->excerpt;
        $post_meta->save();

        $post_meta = new Meta;
        $post_meta->term_id=$post->id;
        $post_meta->type='content';
        $post_meta->content=$request->content;
        $post_meta->save();
       
        return redirect()->route('admin.page.index');
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
         if (!Auth()->user()->can('page.edit')) {
            return abort(401);
        }
      $info=Terms::with('excerpt','content')->find($id);   

       return view('admin.page.edit',compact('info'));
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
            'title' => 'required|max:255',
            'slug' => 'required|max:255',            
        ]);

        $checktitle= Terms::where('type',1)->where('title',$request->name)->where('id','!=',$id)->first();
        $checkslug= Terms::where('type',1)->where('slug',$request->slug)->where('id','!=',$id)->first();
        if (!empty($checkslug)) {
            return response()->json(['Slug Must Be unique'],401);
        }

        if (!empty($checktitle)) {
            return response()->json(['Page Title Must Be unique'],401);
        }    
        
        $post= Terms::find($id);
        $post->title=$request->title;
        $post->slug=$request->slug;
        $post->type=$request->type;
        $post->auth_id=Auth::id();
        $post->status=$request->status;
        $post->save();

        $post_meta =  Meta::where('term_id',$id)->where('type','excerpt')->first();
        if (!empty($post_meta)) {
        $post_meta->term_id=$post->id;
        $post_meta->type='excerpt';
        $post_meta->content=$request->excerpt;
        $post_meta->save();
        }
       
        $postdetail= Meta::where('term_id',$id)->where('type','content')->first();
        if (!empty($postdetail)) {
        $postdetail->term_id=$post->id;
        $postdetail->type='content';
        $postdetail->content=$request->content;
        $postdetail->save(); 
        }
             
        return back();
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(Request $request)
    {
        if (!Auth()->user()->can('page.delete')) {
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
