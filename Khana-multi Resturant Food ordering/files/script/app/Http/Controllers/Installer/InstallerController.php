<?php

namespace App\Http\Controllers\Installer;
use Lpress\Verify\Everify;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Artisan;
use DB;
use Illuminate\Support\Str;
use File;

class InstallerController extends Controller
{

    public function install()
    {

        try {
          DB::connection()->getPdo();
          if(DB::connection()->getDatabaseName()){
            return redirect()->route(404);
          }else{
            $phpversion = phpversion();
            $mbstring = extension_loaded('mbstring');
            $bcmath = extension_loaded('bcmath');
            $ctype = extension_loaded('ctype');
            $json = extension_loaded('json');
            $openssl = extension_loaded('openssl');
            $pdo = extension_loaded('pdo');
            $tokenizer = extension_loaded('tokenizer');
            $xml = extension_loaded('xml');

            $info = [
                'phpversion' => $phpversion,
                'mbstring' => $mbstring,
                'bcmath' => $bcmath,
                'ctype' => $ctype,
                'json' => $json,
                'openssl' => $openssl,
                'pdo' => $pdo,
                'tokenizer' => $tokenizer,
                'xml' => $xml,
            ];
            return view('installer.requirments',compact('info'));
          }
        } catch (\Exception $e) {
            $phpversion = phpversion();
            $mbstring = extension_loaded('mbstring');
            $bcmath = extension_loaded('bcmath');
            $ctype = extension_loaded('ctype');
            $json = extension_loaded('json');
            $openssl = extension_loaded('openssl');
            $pdo = extension_loaded('pdo');
            $tokenizer = extension_loaded('tokenizer');
            $xml = extension_loaded('xml');

            $info = [
                'phpversion' => $phpversion,
                'mbstring' => $mbstring,
                'bcmath' => $bcmath,
                'ctype' => $ctype,
                'json' => $json,
                'openssl' => $openssl,
                'pdo' => $pdo,
                'tokenizer' => $tokenizer,
                'xml' => $xml,
            ];
            return view('installer.requirments',compact('info'));
        }
  
        
    }

    public function info()
    {

        try {
          DB::connection()->getPdo();
          if(DB::connection()->getDatabaseName()){
            return redirect()->route(404);
          }else{
            return view('installer.info'); 
          }
        } catch (\Exception $e) {
            return view('installer.info'); 
        }
           
    }

    public function send(Request $request)
    {

        $APP_NAME = Str::slug($request->app_name);
        $PUSHER_APP_KEY = $request->PUSHER_APP_KEY;
        $PUSHER_APP_CLUSTER = $request->PUSHER_APP_CLUSTER;
        $txt ="APP_NAME=".$APP_NAME."
APP_ENV=local
APP_KEY=base64:kZN2g9Tg6+mi1YNc+sSiZAO2ljlQBfLC3ByJLhLAUVc=
APP_DEBUG=true
APP_URL=".$request->app_url."
LOG_CHANNEL=stack\n
DB_CONNECTION=mysql
DB_HOST=".$request->db_host."
DB_PORT=3306
DB_DATABASE=".$request->db_name."
DB_USERNAME=".$request->db_user."
DB_PASSWORD=".$request->db_pass."\n
BROADCAST_DRIVER=log
CACHE_DRIVER=file
QUEUE_CONNECTION=sync
SESSION_DRIVER=file
SESSION_LIFETIME=120\n
REDIS_HOST=127.0.0.1
REDIS_PASSWORD=null
REDIS_PORT=6379\n
MAIL_DRIVER=smtp
MAIL_HOST=smtp.mailtrap.io
MAIL_PORT=2525
MAIL_USERNAME=
MAIL_PASSWORD=
MAIL_ENCRYPTION=tls
MAIL_FROM_ADDRESS=
MAIL_FROM_NAME=\n
DO_SPACES_KEY=
DO_SPACES_SECRET=
DO_SPACES_ENDPOINT=
DO_SPACES_REGION=
DO_SPACES_BUCKET=
PUSHER_APP_ID=
PUSHER_APP_KEY=
PUSHER_APP_SECRET=
PUSHER_APP_CLUSTER=mt1\n
MIX_PUSHER_APP_KEY=
MIX_PUSHER_APP_CLUSTER=\n
TIMEZONE=UTC
DEFAULT_LANG=en\n
STRIPE_KEY=
STRIPE_SECRET=\n
PAYPAL_CLIENT_ID=\n
PLACE_KEY=\n
FACEBOOK_CLIENT_ID=
FACEBOOK_CLIENT_SECRET=\n
GOOGLE_CLIENT_ID=
GOOGLE_CLIENT_SECRET=\n
ONESIGNAL_REST_API=
ONESIGNAL_AUTH_KEY=
ONESIGNAL_APP_ID=
ONESIGNAL_SUB_DOMAIN=
       ";
       File::put(base_path('.env'),$txt);
       return "Sending Credentials";
    }
    
    

    public function check()
    {
        try {
          DB::connection()->getPdo();
            if(DB::connection()->getDatabaseName()){
                return "Database Installing";
            }else{
                return false;
            }
        } catch (\Exception $e) {
            return false;
        }
        
    }

    public function migrate()
    {
        ini_set('max_execution_time', '0');
        \Artisan::call('migrate:fresh');
        return "Demo Importing";
    }

    public function seed()
    {
        ini_set('max_execution_time', '0');
        \Artisan::call('db:seed');
        return "Congratulations! Your site is ready";
    }


    public function verify($key)
    {
        $check= Everify::Check($key);
        if ($check==true) {
            echo "success";
         }
        else{
            echo  Everify::$massage;
         }
    }

    public function purchase()
    {
        return view('installer.purchase');
    }

    public function purchase_check(Request $request)
    {
        $this->validate($request,[
            'purchase_code' => 'required'
        ]);

        $check= Everify::Check($request->purchase_code);
        if ($check==true) {
            return redirect()->route('install.info');
        }
        else{
            return back()->with('alert-success',Everify::$massage);
         }

    }
}
