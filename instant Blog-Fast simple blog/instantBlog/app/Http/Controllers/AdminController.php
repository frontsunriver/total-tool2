<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Post;
use App\Models\User;

class AdminController extends Controller
{
    /**
     * Create a new controller instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('can:admin-area');
    }

    /**
     * Show the application dashboard.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        $countUn = Post::wherePostLive('0')->count();
        $countPo = Post::wherePostLive('1')->count();
        $countUs = User::count();

        return view('admin', compact('countUn', 'countPo', 'countUs'));
    }
}
