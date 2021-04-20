<?php

namespace Amcoders\Plugin\table\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use App\Terms;
use App\Meta;
use App\Productmeta;
use Illuminate\Support\Str;

class TableController extends Controller
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
           $tables=Terms::with('excerpt','price')->where('type',7)->where('auth_id',$auth_id)->where('title','LIKE','%'.$request->src.'%')->latest()->paginate(20);
        }
        elseif($request->st) {
            if ($request->st=='trash') {
                 $tables=Terms::with('excerpt','price')->where('type',7)->where('auth_id',$auth_id)->where('status',0)->latest()->paginate(20);
                 $status=$request->st;
        return view('plugin::table.index',compact('tables','auth_id','status'));
        
            }
            else{
               $tables=Terms::with('excerpt','price')->where('type',7)->where('auth_id',$auth_id)->where('status',$request->st)->latest()->paginate(20);
               $status=$request->st;
        return view('plugin::table.index',compact('tables','auth_id','status'));
        
           }
           
       }
        else{
          $tables=Terms::with('excerpt','price')->where('type',7)->where('auth_id',$auth_id)->latest()->where('status','!=',0)->paginate(20);
        }
        $status=1;


        $tables=Terms::with('excerpt','price')->where('type',7)->where('auth_id',$auth_id)->latest()->paginate(20);
        return view('plugin::table.index',compact('auth_id','tables','status'));
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        return view('plugin::table.create');
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
            'chair' => 'required',
            'price' => 'required'
        ]);

        $slug=Str::slug($request->title);
        if ($slug=='') {
            $slug=str_replace(' ', '-', $request->title);
        }

        $post=new Terms;
        $post->title=$request->title;
        $post->slug=$slug;
        $post->type=7;
        $post->auth_id=Auth::id();
        $post->status=$request->status;
        $post->save();

        $post_meta = new Meta;
        $post_meta->term_id=$post->id;
        $post_meta->type='excerpt';
        $post_meta->content=$request->chair;
        $post_meta->save();

        $product=new Productmeta;
        $product->term_id = $post->id;
        $product->price = $request->price;
        $product->save();

        return response()->json(['Table Created']);
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
        $info = Terms::with('excerpt','price')->where('auth_id',Auth::id())->find($id);
        return view('plugin::table.edit',compact('info'));
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
            'chair' => 'required',
            'price' => 'required'
        ]);

        $slug=Str::slug($request->title);
        if ($slug=='') {
            $slug=str_replace(' ', '-', $request->title);
        }

        $post= Terms::where('id',$id)->first();
        $post->title=$request->title;
        $post->slug=$slug;
        $post->status=$request->status;
        $post->save();

        $post_meta = Meta::where('term_id',$id)->first();
        $post_meta->content=$request->chair;
        $post_meta->save();

        $product= Productmeta::where('term_id',$id)->first();
        $product->price = $request->price;
        $product->save();

        return response()->json(['Table Created']);
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
