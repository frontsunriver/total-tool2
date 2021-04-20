<?php

namespace App\Http\Middleware;

use Closure;
use Auth;

class ApprovalMiddleware
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
        if(Auth::check() && Auth::User()->status == 'approved' || Auth::User()->status == 'offline')
        {
            return $next($request);
        }else{
            return redirect()->route('restaurant.register_step_4');
        }
    }
}
