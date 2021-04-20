<?php

namespace App\Http\Middleware;

use Closure;
use Auth;

class AuthorMiddleware
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
        if (Auth::check() && Auth::User()->role_id == 2) {
           return $next($request);
        }else{
            return redirect()->route('login');
        } 
    }
}
