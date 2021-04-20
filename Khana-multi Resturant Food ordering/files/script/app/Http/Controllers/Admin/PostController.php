<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Str;
use App\Terms;
use App\Meta;
use App\Post;
use App\PostCategory;
use App\Postrelation;
use Auth;
use DB;
class PostController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {
       
       
        
        if($request->src) {
           $posts=Terms::with('user')->where('type',0)->where('title','LIKE','%'.$request->src.'%')->latest()->paginate(20);
        }
        elseif($request->st) {
            if ($request->st=='trash') {
                 $posts=Terms::with('user')->where('type',0)->where('status',0)->latest()->paginate(20);
                 $status=$request->st;
                 return view('admin.posts.index',compact('posts','status'));
            }
            else{
               $posts=Terms::with('user')->where('type',0)->where('status',$request->st)->latest()->paginate(20);
               $status=$request->st;
               return view('admin.posts.index',compact('posts','status')); 
           }
           
       }
        else{
          $posts=Terms::with('user')->where('type',0)->latest()->where('status','!=',0)->paginate(20);
        }
        $status=1;
        return view('admin.posts.index',compact('posts','status'));
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        return view('admin.posts.create');
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
        if ($creat_slug=='') {
            $creat_slug=str_replace(' ', '-', $request->title);
        }
        $check=Terms::where('slug',$creat_slug)->where('type',2)->count();
        if ($check != 0) {
            $slug=$creat_slug.'-'.$check;
        }
        else{
            $slug=$creat_slug;
        }


        


        $post=new Terms;
        $post->title=$request->title;
        $post->slug=$slug;
        $post->type=0;
        $post->auth_id=Auth::id();
        $post->lang=$request->lang;
        $post->status=$request->status;
        $post->lang=$request->lang;
        $post->save();

        $post_meta = new Meta;
        $post_meta->term_id=$post->id;
        $post_meta->type='excerpt';
        $post_meta->content=$request->excerpt;
        $post_meta->save();

        $post_meta = new Meta;
        $post_meta->term_id=$post->id;
        $post_meta->type='preview';
        $post_meta->content=$request->preview;
        $post_meta->save();

        $post_meta = new Meta;
        $post_meta->term_id=$post->id;
        $post_meta->type='content';
        $post_meta->content=$request->content;
        $post_meta->save();



        if ($request->category) {
            
         foreach ($request->category as $cat_row) {
           
                $cat= new PostCategory;
                $cat->term_id=$post->id;
                $cat->category_id=$cat_row;
                $cat->save();
          

         }
        }

        

        
        return response()->json('Post Created');

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
       $info=Terms::with('excerpt','content','preview','categories')->find($id);   
       return view('admin.posts.edit',compact('info'));
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request,$id)
    {
      
         $validatedData = $request->validate([
            'title' => 'required|max:100',
            'slug' => 'required|max:100',
            
        ]);
         $slug= Terms::where('slug',$request->slug)->where('id','!=',$id)->first();
        if (!empty($slug)) {
            return response()->json(['Slug Must Be unique'],401);
        }

      

        $post= Terms::find($id);
        $post->title=$request->title;
        $post->slug=$request->slug;
        $post->auth_id=Auth::id();
        $post->lang=$request->lang;
        $post->status=$request->status;
        $post->lang=$request->lang;
        $post->save();

        $post_meta =  Meta::where('term_id',$id)->where('type','excerpt')->first();
        if (!empty($post_meta)) {
          $post_meta->content=$request->excerpt;
          $post_meta->save();
      }
       
        $postdetail =  Meta::where('term_id',$id)->where('type','content')->first();
       
        if (!empty($postdetail)) {
            $post_meta->content=$request->content;
            $postdetail->save();
        }

        $pr =  Meta::where('term_id',$id)->where('type','preview')->first();
       
        if (!empty($pr)) {
            $pr->content=$request->preview;
            $pr->save();
        }
        


        if ($request->category) {
          PostCategory::where('term_id',$id)->delete();    
         foreach ($request->category as $cat_row) {
            if ($cat_row != 0) {
                $cat= new PostCategory;
                $cat->term_id=$id;
                $cat->category_id=$cat_row;
                $cat->save();
            }

         }
        }

      
       

        return response()->json('Post updated');


    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  \Illuminate\Http\Request  $request
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
