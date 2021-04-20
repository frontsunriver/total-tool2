<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use File;
use Illuminate\Support\Str;
use Auth;
class EnvController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        if (!Auth()->user()->can('system.settings')) {
            return abort(401);
        }
        $countries= base_path('resources/lang/langlist.json');
        $countries= json_decode(file_get_contents($countries),true);

        return view('admin.settings.env',compact('countries'));
    }

   

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
     
       $APP_NAME = Str::slug($request->APP_NAME);
       $PUSHER_APP_KEY = $request->PUSHER_APP_KEY;
       $PUSHER_APP_CLUSTER = $request->PUSHER_APP_CLUSTER;
$txt ="APP_NAME=".$APP_NAME."
APP_ENV=".$request->APP_ENV."
APP_KEY=base64:kZN2g9Tg6+mi1YNc+sSiZAO2ljlQBfLC3ByJLhLAUVc=
APP_DEBUG=".$request->APP_DEBUG."
APP_URL=".$request->APP_URL."
LOG_CHANNEL=".$request->LOG_CHANNEL."\n
DB_CONNECTION=".env("DB_CONNECTION")."
DB_HOST=".env("DB_HOST")."
DB_PORT=".env("DB_PORT")."
DB_DATABASE=".env("DB_DATABASE")."
DB_USERNAME=".env("DB_USERNAME")."
DB_PASSWORD=".env("DB_PASSWORD")."\n
BROADCAST_DRIVER=".$request->BROADCAST_DRIVER."
CACHE_DRIVER=".$request->CACHE_DRIVER."
QUEUE_CONNECTION=".$request->QUEUE_CONNECTION."
SESSION_DRIVER=".$request->SESSION_DRIVER."
SESSION_LIFETIME=".$request->SESSION_LIFETIME."\n
REDIS_HOST=".$request->REDIS_HOST."
REDIS_PASSWORD=".$request->REDIS_PASSWORD."
REDIS_PORT=".$request->REDIS_PORT."\n
MAIL_DRIVER=".$request->MAIL_DRIVER."
MAIL_HOST=".$request->MAIL_HOST."
MAIL_PORT=".$request->MAIL_PORT."
MAIL_USERNAME=".$request->MAIL_USERNAME."
MAIL_PASSWORD=".$request->MAIL_PASSWORD."
MAIL_ENCRYPTION=".$request->MAIL_ENCRYPTION."
MAIL_FROM_ADDRESS=".$request->MAIL_FROM_ADDRESS."
MAIL_TO=".$request->MAIL_TO."
MAIL_FROM_NAME=".Str::slug($request->MAIL_FROM_NAME)."\n
DO_SPACES_KEY=".$request->DO_SPACES_KEY."
DO_SPACES_SECRET=".$request->DO_SPACES_SECRET."
DO_SPACES_ENDPOINT=".$request->DO_SPACES_ENDPOINT."
DO_SPACES_REGION=".$request->DO_SPACES_REGION."
DO_SPACES_BUCKET=".$request->DO_SPACES_BUCKET."\n

TIMEZONE=".$request->TIMEZONE.""."
DEFAULT_LANG=".$request->DEFAULT_LANG."\n

STRIPE_KEY=".$request->STRIPE_KEY."
STRIPE_SECRET=".$request->STRIPE_SECRET."\n

PAYPAL_CLIENT_ID=".$request->PAYPAL_CLIENT_ID."

PLACE_KEY=".$request->PLACE_KEY."

FACEBOOK_CLIENT_ID=".$request->FACEBOOK_CLIENT_ID."
FACEBOOK_CLIENT_SECRET=".$request->FACEBOOK_CLIENT_SECRET."

GOOGLE_CLIENT_ID=".$request->GOOGLE_CLIENT_ID."
GOOGLE_CLIENT_SECRET=".$request->GOOGLE_CLIENT_SECRET."

ONESIGNAL_REST_API=".$request->ONESIGNAL_REST_API."
ONESIGNAL_AUTH_KEY=".$request->ONESIGNAL_AUTH_KEY."
ONESIGNAL_APP_ID=".$request->ONESIGNAL_APP_ID."
ONESIGNAL_SUB_DOMAIN=".Str::slug($request->ONESIGNAL_SUB_DOMAIN)."
       ";
       File::put(base_path('.env'),$txt);
       return response()->json("System Updated");
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
