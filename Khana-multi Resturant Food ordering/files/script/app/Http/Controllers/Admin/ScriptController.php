<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use File;
class ScriptController extends Controller
{
     /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
       return view('admin.script.index');
    }

    
    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {

       $header=base_path('resources/views/script/headerscript.blade.php');
       $footer=base_path('resources/views/script/footerscript.blade.php');
      
        File::put($header,$request->css);
        File::put($footer,$request->js);
               
       return response()->json('Script Updated');
    }

   
}
