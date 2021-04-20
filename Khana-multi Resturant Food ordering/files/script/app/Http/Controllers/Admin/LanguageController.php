<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use File;
use App\Options;
use Auth;
use App\Category;

class LanguageController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        
        if (!Auth()->user()->can('language.control')) {
            return abort(401);
        }
        
        $theme_file_get = file_get_contents(base_path().'/am-content/Themes/theme.json');
        $themes = json_decode($theme_file_get,true);

        foreach($themes as $theme)
        {
            if($theme['status'] == 'active')
            {
                $theme_name = $theme['Text Domain'];
            }
        }
        if(isset($theme_name))
        {
            $langs = Category::where([
                ['name',$theme_name],
                ['type','lang']
            ])->latest()->paginate(20);
        }else{
            $langs = [];
            $theme_name = null;
        }
        return view('admin.language.index',compact('langs','theme_name'));

    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create(Request $request)
    {
        if (!Auth()->user()->can('language.control')) {
            return abort(401);
        }

        $countries=base_path('resources/lang/langlist.json');
        $countries= json_decode(file_get_contents($countries),true);

        $file = file_get_contents( base_path().'/am-content/Themes/theme.json');
        $themes = json_decode($file, true);
        return view('admin.language.create',compact('countries','themes'));
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {

        $validator = \Validator::make($request->all(), [
            'language_code' => 'required',
            'theme_name' => 'required',
            'theme_position' => 'required'
        ]);

        if ($validator->fails())
        {
            $data['errors']['lang']=$validator->errors()->all()[0];
            return response()->json($data,401);
        }

        $lang_file = file_get_contents(resource_path('lang/langlist.json'));
        $langs = json_decode($lang_file);

        foreach($langs as $lang)
        {
            if($lang->code == $request->language_code)
            {
                $lang_name = $lang->name;
            }
        }

        $lang_get = Category::where([
            ['slug',$request->language_code],
            ['name',$request->theme_name],
            ['type','lang']
        ])->first();

        if($lang_get)
        {
            $data['errors']['lang']="Language Already Exists";
            return response()->json($data,401);
        }else{
            if(!file_exists(resource_path('lang/theme/'.$request->theme_name)))
            {
                mkdir(resource_path('lang/theme/'.$request->theme_name,077,true));
            }

             $theme_lang_file = file_get_contents(base_path().'/am-content/Themes/'.$request->theme_name.'/translate.json');
             $theme_lang = json_decode($theme_lang_file);

            

            file_put_contents(resource_path('lang/theme/'.$request->theme_name.'/'.$request->language_code.'.json'),json_encode($theme_lang,JSON_PRETTY_PRINT));

            $theme_file_get = file_get_contents(base_path().'/am-content/Themes/theme.json');
            $themes = json_decode($theme_file_get,true);

            foreach($themes as $theme)
            {
                if($theme['Text Domain'] == $request->theme_name)
                {
                    if($theme['status'] == 'active')
                    {
                        file_put_contents(resource_path('lang/'.$request->language_code.'.json'),json_encode($theme_lang,JSON_PRETTY_PRINT));
                    }
                }
            }

            $data = [
                'lang_name' => $lang_name,
                'lang_position' => $request->theme_position
            ];

            $db_lang = new Category();
            $db_lang->name = $request->theme_name;
            $db_lang->slug = $request->language_code;
            $db_lang->content = json_encode($data);
            $db_lang->type = "lang";
            $db_lang->user_id = Auth::User()->id;
            $db_lang->status = 0;
            $db_lang->save();

            
            return response()->json('Language Created');
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
    public function edit($id,$theme_name)
    {
        $lang = Category::where([
            ['slug',$id],
            ['name',$theme_name],
            ['type','lang']
        ])->first();

        if(!$lang){
            return abort(404);
        }

        $lang_file = file_get_contents(resource_path('lang/'.$id.'.json'));
        $langs = json_decode($lang_file,true);

        return view('admin.language.edit',compact('langs','theme_name','id'));
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $lang_code,$theme_name)
    {
        $lang_file = file_get_contents(resource_path('lang/'.$lang_code.'.json'));
        $langs = json_decode($lang_file,true);

        foreach($langs as $key=>$lang)
        {
            if($key == $request->key)
            {
                $langs[$key] = $request->value;
            }
        }

        file_put_contents(resource_path('lang/'.$lang_code.'.json'),json_encode($langs,JSON_PRETTY_PRINT));
        file_put_contents(resource_path('lang/theme/'.$theme_name.'/'.$lang_code.'.json'),json_encode($langs,JSON_PRETTY_PRINT));

        return response()->json('Value Updated');
    }

    public function set(Request $request)
    {
        if($request->status == 'active')
        {
            if($request->lang)
            {
                foreach($request->lang as $language)
                {
                    $theme_file_get = file_get_contents(resource_path('lang/theme/'.$request->theme_name.'/'.$language.'.json'));
                    $theme_data = json_decode($theme_file_get,true);

                    file_put_contents(resource_path('lang/'.$language.'.json'),json_encode($theme_data,JSON_PRETTY_PRINT));

                    $lang_db = Category::where(['name'=>$request->theme_name,'type'=>'lang'])->get();
                    foreach($lang_db as $value)
                    {
                        $value->status = 0;
                        $value->save();
                    } 

                   
                }


                foreach($request->lang as $language)
                {
                    $lang_model = Category::where([
                        ['slug',$language],
                        ['name',$request->theme_name],
                        ['type','lang']
                    ])->get();

                    

                    foreach($lang_model as $value)
                    {
                        $value->status = 1;
                        $value->save();
                    } 
                }

                return response()->json('Language Set Successfully');
            }else{
                $data['errors']['lang']="Please Select your language!";
                return response()->json($data,401);
            }
        }
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function delete($lang_code,$theme_name)
    {
        if(file_exists(resource_path('lang/theme/'.$theme_name.'/'.$lang_code.'.json')))
        {
            unlink(resource_path('lang/theme/'.$theme_name.'/'.$lang_code.'.json'));
        }

        if(file_exists(resource_path('lang/'.$lang_code.'.json')))
        {
            unlink(resource_path('lang/'.$lang_code.'.json'));
        }

        $lang = Category::where([
            ['slug',$lang_code],
            ['name',$theme_name],
            ['type','lang']
        ])->first();

        $lang->delete();

        return back();

    }
}
