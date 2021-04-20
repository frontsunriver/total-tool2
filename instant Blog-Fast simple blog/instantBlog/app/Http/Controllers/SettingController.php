<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Image;
use App\Models\Setting;

class SettingController extends Controller
{
    public function __construct()
    {
        $this->middleware('can:admin-area');
    }

    public function index()
    {
        //Get user settings from database and show on needed places
        $setting = Setting::where('id', 1)->first();
        return view('posts.setting', compact('setting'));
    }

    public function update(Request $request, $id)
    {
        //Update user settings when filled form
        $setting = Setting::findOrFail($id);

        $attributes = request(['site_name', 'site_desc', 'site_title', 'allow_comments', 'allow_users', 'check_cont',
        'site_logo', 'site_extra', 'post_ads', 'page_ads', 'between_ads', 'fb_page_token','fb_publishing', 'fb_theme', 'fb_ads_code', 'amp_ad_server', 'amp_adscode', 'footer', 'site_analytic']);

        if ($request->hasFile('site_logo')) {
            $postimage = $request->file('site_logo');
            $filename = time() . '.' . $postimage->getClientOriginalExtension();
            Image::make($postimage)->resize(null, 35, function ($constraint) {
                $constraint->aspectRatio();
            })->save(public_path('/images/'. $filename));
            $attributes['site_logo'] = $filename;
        } else {
            $attributes['site_logo'] = $setting->site_logo ;
        }

        $setting->update($attributes);
        
        session()->flash('message', 'Settings Updated!');

        return redirect('/admin');
    }
}
