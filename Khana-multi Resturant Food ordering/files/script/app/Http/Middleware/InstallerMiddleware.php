<?php

namespace App\Http\Middleware;

use Closure;
use DB;

class InstallerMiddleware
{
    /**
     * Handle an incoming request.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @return mixed
     */
    public function handle($request, Closure $next)
    { 
         // Test database connection
        
        return $next($request);
        
    }
}
