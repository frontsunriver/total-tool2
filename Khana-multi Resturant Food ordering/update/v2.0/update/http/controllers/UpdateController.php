<?php 

namespace Amcoders\Plugin\update\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use ZipArchive;
use App\Options;
class UpdateController extends controller
{
	protected $extract=false;
    protected $version;

	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {

        return view('plugin::update.index');
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
     * Update Website
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {

        $validatedData = $request->validate([
        'file' => 'required|mimes:zip'
        ]);
        
         ini_set('max_execution_time', '0');
        $this->version=$name=basename($request->file('file')->getClientOriginalName(), '.'.$request->file('file')->getClientOriginalExtension());
       

       $zip = new ZipArchive;
       $res = $zip->open($request->file);
        if ($res === TRUE) {
        $zip->extractTo('uploads/update');
        $zip->close();
        $this->extract=true;
       

        } else {
          $this->extract=false;
        }


        if ($this->extract==true) {
           $file='uploads/update/'.$this->version.'/update.php';
           if (file_exists($file)) {
            include $file;
            if (function_exists('update')) {
                foreach (update() as $key => $row) {

                    if (file_exists($row['file'])) {
                        if ($key=='file') {
                            $content=file_get_contents($row['file']);
                            \File::put($row['root'],$content);
                        }
                        else{
                            \File::copyDirectory($row['file'],$row['root']);
                        }
                    }
                
                }
                $update=Options::where('key','version')->first();
                if (empty($update)) {
                    $update=new Options;
                    $update->key='version';
                }
                $update->value=$this->version;
                $update->save();
                \File::deleteDirectory('uploads/update/'.$this->version);
                return response()->json(['Site Update success']);
            }


        }
    }

      
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
        //
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
        //
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        //
    }
}