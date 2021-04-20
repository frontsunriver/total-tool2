<?php

namespace App\Http\Middleware;

use Closure;
use Auth;

class AdminMiddleware
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
        if (Auth::check() && Auth::User()->role_id == 1) {
            if(Auth::User()->status == 'approved' || Auth::id() == 1){
                 return $next($request);
            }
            return redirect()->route('login');

        }else{
            return redirect()->route('login');
        } 
    }
}
