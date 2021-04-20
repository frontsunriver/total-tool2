<?php

namespace App\Http\Middleware;

use Closure;
use Auth;

class StoreMiddleware
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
        if (Auth::check() && Auth::User()->role->id == 3) {
           return $next($request);
        }else{
            return redirect()->route('login');
        } 
    }
}
