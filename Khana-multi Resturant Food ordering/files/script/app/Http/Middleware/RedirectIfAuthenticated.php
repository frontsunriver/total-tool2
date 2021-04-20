<?php

namespace App\Http\Middleware;

use App\Providers\RouteServiceProvider;
use Closure;
use Illuminate\Support\Facades\Auth;

class RedirectIfAuthenticated
{
    /**
     * Handle an incoming request.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @param  string|null  $guard
     * @return mixed
     */
    public function handle($request, Closure $next, $guard = null)
    {
        if (Auth::guard($guard)->check() && Auth::User()->role_id == 1) {
            return redirect()->route('admin.dashboard');
        }elseif (Auth::guard($guard)->check() && Auth::User()->role_id == 2) {
            return redirect()->route('author.dashboard');
        }elseif(Auth::guard($guard)->check() && Auth::User()->role_id == 3)
        {
            return redirect()->route('store.dashboard');
        }elseif(Auth::guard($guard)->check() && Auth::User()->role_id == 4)
        {
            return redirect()->route('rider.dashboard');
        }
        else{
            return $next($request);
        }
    }
}
