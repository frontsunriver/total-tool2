<?php

namespace App\Providers;

use Illuminate\Support\ServiceProvider;
use Illuminate\Pagination\Paginator;
use Illuminate\Support\Facades\Blade;

class AppServiceProvider extends ServiceProvider
{
    /**
     * Register any application services.
     *
     * @return void
     */
    public function register()
    {
        //
    }

    /**
     * Bootstrap any application services.
     *
     * @return void
     */
    public function boot()
    {
        view()->composer('public.archives', function ($view) {
            $view->with('archives', \App\Models\Post::archives());
        });

        view()->composer('public.tags', function ($view) {
            $view->with('tags', \App\Models\Tag::has('posts')->latest()->paginate(30));
        });

        view()->composer('posts.tagselect', function ($view) {
            $view->with('tags', \App\Models\Tag::all());
        });

        view()->composer('layouts.nav', function ($view) {
            $view->with('setting', \App\Models\Setting::where('id', 1)->first());
        });

        view()->composer('auth.login', function ($view) {
            $view->with('setting', \App\Models\Setting::where('id', 1)->first());
        });

        view()->composer('layouts.master', function ($view) {
            $view->with('setting', \App\Models\Setting::where('id', 1)->first());
        });

        view()->composer('public.*', function ($view) {
            $view->with('setting', \App\Models\Setting::where('id', 1)->first());
        });

        Blade::if('modorall', function () {
            $setting = \App\Models\Setting::where('id', 1)->first();
            return auth()->user()->can('moderator-post') OR $setting->allow_users == '0';
        });

        Paginator::useBootstrap();
    }
}
