@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-12">
        <h1 class="display-4">Delete User</h1>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="box-white ml-3 mr-3">
        <div class="col-md-12"> 
            <h2 class="text-center"> Are you sure you want to delete? </h2>     
            <p class="lead text-center"><strong>User : </strong>{{ $user->username }}</p>
            <p class="lead text-center">Note: All contents belongs to this user will be deleted too!</p>
            <hr>
        </div>
        <div class="row">
            <div class="col-md-6">
                <form action="{{ url('/users/' . $user->id) }}" method="POST">
                    @csrf
                    {{ method_field('DELETE') }}
                    <button type="submit" class="btn btn-danger float-right">Delete</button>
                </form>
            </div>
            <div class="col-md-6">
                <a href="{{ url('/profile/') }}" class="btn btn-primary" role="button">Cancel</a>      
            </div>
        </div>
    </div>
</div>
@endsection
