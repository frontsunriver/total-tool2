<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use App\Menu;

class HomeController extends Controller
{
    /**
     * Create a new controller instance.
     *
     * @return void
     */
    public function __construct()
    {
       // $this->middleware('auth');
    }

    /**
     * Show the application dashboard.
     *
     * @return \Illuminate\Contracts\Support\Renderable
     */
    public function index()
    {
      $data = Menu::first();
      $menus=json_decode($data->link);
      return $menus;
      return view('welcome',compact('data'));
     
    }


    

}

