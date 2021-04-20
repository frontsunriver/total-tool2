<?php 

namespace Amcoders\Plugin\shop\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Category;
use Illuminate\Support\Str;
use Auth;

class MenuController extends controller
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
        return view('plugin::menu.index',compact('req','type'));
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
            'name' => 'required|max:100',
           
            
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
        $category->p_id=$pid;
        $category->type=1;
        $category->user_id=Auth::id();
        $category->save();
        return response()->json('Menu Ceated');
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
        $info=Category::where('user_id',Auth::id())->find($id);
        if (empty($info)) {
            abort(404);
        }
        return view('plugin::menu.edit',compact('info'));
        
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
            
        ]);

        

               
        if ($request->p_id==null) {
           $p_id=null;
        }
        else{
             $p_id=$request->p_id;
        }
        $category=Category::find($id);
        $category->name=$request->name;
        $category->user_id=Auth::id();
        
        $category->p_id=$p_id;
       
        $category->save();
        return response()->json('Menu update');

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
                    Category::where('user_id',Auth::id())->where('id',$id)->delete();
                }
             }
        }
       
        
        return response()->json('Category Removed');
    }
}