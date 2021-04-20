<?php 

namespace Amcoders\Plugin\qrmenu\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Usermeta;
use Auth;
use SimpleSoftwareIO\QrCode\Facades\QrCode;
/**
 * 
 */
class QrmenuController extends controller
{
	

	 /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        $qrcode = Usermeta::where([
            ['user_id',Auth::User()->id],
            ['type','qrmenu']
        ])->first();
        
        if(isset($qrcode))
        {
            $qrmeta = json_decode($qrcode->content);
            
            $explode_color = explode(',',$qrmeta->color);
            $explode_bgcolor = explode(',',$qrmeta->bgcolor);

            $bgcolor = $qrmeta->bgcolor;
            $color = $qrmeta->color;
            $margin = $qrmeta->margin;
            $size = $qrmeta->size;

            $store_profile = route('store.show',Auth::User()->slug);
             
            $qrcodegenerate = QrCode::style($qrmeta->style)->eye($qrmeta->eye)->size($qrmeta->size)->color($explode_color[0],$explode_color[1],$explode_color[2])->backgroundColor($explode_bgcolor[0],$explode_bgcolor[1],$explode_bgcolor[2])->margin($qrmeta->margin)->generate($store_profile);
        }else{
            $store_profile = route('store.show',Auth::User()->slug);
            $qrcodegenerate = QrCode::size(275)->generate($store_profile);
            $bgcolor = null;
            $color = null;
            $margin = null;
            $size = null;
        }



        return view('plugin::qrmenu.index',compact('qrcodegenerate','bgcolor','color','margin','size'));
    }

    public function style(Request $request)
    {
        $qrmenu = Usermeta::where([
            ['user_id',Auth::User()->id],
            ['type','qrmenu']
        ])->first();

        if(isset($qrmenu))
        { 

            $qrmeta = json_decode($qrmenu->content);

            if($request->style)
            {
                $style = $request->style;
            }else{
                $style = $qrmeta->style;
            }

            if($request->eye)
            {
                $eye = $request->eye;
            }else{
                $eye = $qrmeta->eye;
            }

            if($request->bgcolor)
            {
                $bgcolor = $request->bgcolor;
            }else{
                $bgcolor = $qrmeta->bgcolor;
            }

            if($request->color)
            {
                $color = $request->color;
            }else{
                $color = $qrmeta->color;
            }

            
            if($request->margin)
            {
                $margin = $request->margin;
            }else{
                $margin = $qrmeta->margin;
            }

            if($request->size)
            {
                $size = $request->size;
            }else{
                $size = $qrmeta->size;
            }

           


            $qrmeta->style = $style;
            $qrmeta->eye = $eye;
            $qrmeta->bgcolor = $bgcolor;
            $qrmeta->color = $color;
            $qrmeta->margin = $margin;
            $qrmeta->size = $size;
            
            $qrmenu->content = json_encode($qrmeta);
            $qrmenu->save();

            return response()->json($qrmenu);

        }else{

            if($request->style)
            {
                $style = $request->style;
            }else{
                $style = 'square';
            }

            if($request->eye)
            {
                $eye = $request->eye;
            }else{
                $eye = 'square';
            }

            if($request->bgcolor)
            {
                $bgcolor = $request->bgcolor;
            }else{
                $bgcolor = '255,255,255';
            }

            if($request->color)
            {
                $color = $request->color;
            }else{
                $color = '0,0,0';
            }

            
            if($request->margin)
            {
                $margin = $request->margin;
            }else{
                $margin = 0;
            }

            if($request->size)
            {
                $size = $request->size;
            }else{
                $size = 275;
            }

            $data = [
                'style' => $style,
                'eye' => $eye,
                'bgcolor' => $bgcolor,
                'color' => $color,
                'margin' => $margin,
                'size' => $size
            ];

            $qrmenu = new Usermeta();
            $qrmenu->user_id = Auth::User()->id;
            $qrmenu->type = 'qrmenu';
            $qrmenu->content = json_encode($data);
            $qrmenu->save();

            return response()->json($qrmenu);
        }
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
        //
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