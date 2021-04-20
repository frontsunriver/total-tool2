@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('jumbotron')
<div class="jumbotron bg-none">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="display-4 text-white">Activation</h1>
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container mt-5">
    <div class="row">
        <div class="col-md-12">     
            <div class="card">
                <div class="card-body">
                    <h3>Username : <code>admin</code> Password : <code>{{ $attribute }}</code></h3>
                    <p class="text-danger"> * If you have changed your admin username before, please use your username instead of "admin".</p>
                    <p> * Please login and change your password from admin panel > profile section.</p>
                    <h5><a href="{{ url('/login') }}">Go to login page</a></h5>           
                </div>
            </div>
        </div>
    </div>
</div>
@endsection