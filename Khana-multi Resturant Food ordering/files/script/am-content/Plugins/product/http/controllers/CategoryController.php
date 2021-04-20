<?php 

namespace Amcoders\Plugin\product\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Category;
use Illuminate\Support\Str;
use Auth;
/**
 * 
 */
class CategoryController extends controller
{
	

	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
     public function index(Request $request)
     {

        if (!empty($request->query)) {
            $req=$request->qry;    
        }
        else{
            $req='';
        }
        $type=$request->type;
        if (!Auth()->user()->can('product.category')) {
            return abort(401);
        }
        return view('plugin::category.index',compact('req','type','request'));
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        //
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
            'name' => 'required|unique:categories|max:100',
            'slug' => 'unique:categories|max:100',
            
        ]);

        if ($request->slug) {
            $slug=$request->slug;
        }
        else{
            $slug=Str::slug($request->name);
        }

        if ($request->p_id) {
            $pid=$request->p_id;
        }
        else{
            $pid=null;
        }
        $category=new Category;
        $category->name=$request->name;
        $category->avatar=$request->preview;
        $category->slug=$slug;
        $category->p_id=$pid;
        $category->type=$request->type;
        $category->user_id=Auth::id();
        $category->save();
        return response()->json('category created');
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
       
        $info=Category::find($id);
        return view('plugin::category.edit',compact('info'));
        
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
            'name' => 'required|max:100',
            'slug' => 'required|max:100',            
        ]);

        $checktitle= Category::where('name',$request->name)->where('id','!=',$id)->first();
        $checkslug= Category::where('slug',$request->slug)->where('id','!=',$id)->first();
        if (!empty($checkslug)) {
            return response()->json(['Slug Must Be unique'],401);
        }

        if (!empty($checktitle)) {
            return response()->json(['Category Name Must Be unique'],401);
        }        
        if ($request->p_id==null) {
           $p_id=null;
        }
        else{
             $p_id=$request->p_id;
        }
        $category=Category::find($id);
        $category->name=$request->name;
        $category->avatar=$request->preview;
        $category->slug=$request->slug;
        $category->p_id=$p_id;
       
        $category->save();
        return response()->json('category update');

    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(Request $request)
    {
       if ($request->method=='delete') {
             if ($request->ids) {
                foreach ($request->ids as $id) {
                    Category::destroy($id);
                }
             }
        }
       
        
        return response()->json('Category Removed');
    }
}