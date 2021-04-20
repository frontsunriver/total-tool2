<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Options;
use Auth;
class FilesystemController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (!Auth()->user()->can('file.system')) {
            return abort(401);
        }
        $info=Options::where('key','lp_filesystem')->first();
        $info=json_decode($info->value);
        return view('admin.filesystem.index',compact('info'));
    }

    
    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        
       $system= Options::where('key','lp_filesystem')->first();
       $arr['compress']=$request->compress;
       $arr['system_type']=$request->method;

       if ($request->method=='do') {
         $request->validate([
           'url' => 'required',
            ]);
          $arr['system_url']=$request->url;
        }
        else{
            $json=json_decode($system->value);
            $arr['system_url']=$json->system_url;
        }

        $system->value=json_encode($arr);

     $system->save();
     return response()->json('File System Updated');

    }

    
}
