@extends('layouts.admin')
@section('jumbotron')
<div class="row align-items-center">
    <div class="col-9">
        <h1 class="display-4">Admin Panel</h1>
    </div>
    <div class="col-3">
        <div class="admin-item-img">
            <img src="{{ url('/images/' . Auth::user()->avatar) }}" class="admin-image rounded-circle">
        </div>
        <p class="admin-item-user">{{ Auth::user()->name }}</p>
        <p class="admin-item-text">{{ Auth::user()->username }}</p>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="row  ml-3 mr-3">
        <div class="col-md-4">
            <section class="admin admin-simple-sm">
                <div class="admin-simple-sm-icon">
                    <i class="icon-docs icons"></i>
                </div>
                <div class="admin-simple-sm-bottom">Total Posts :
                {{ $countPo }}
                </div>
                <div class="admin-simple-sm-bottom bottom-white">
                        <a href="{{ url('/contents/') }}" role="button" class="btn btn-secondary btn-sm">See Posts</a>
                </div>
            </section>
        </div>
        <div class="col-md-4">
            <section class="admin admin-simple-sm">
                <div class="admin-simple-sm-icon">
                    <i class="icon-doc icons"></i>
                </div>
                <div class="admin-simple-sm-bottom">Unpublished Posts :
                @if ($countUn > 0)                
                <span class="badge badge-pill badge-danger">{{ $countUn }}</span>
                @else
                {{ $countUn }}
                @endif
                </div>
                <div class="admin-simple-sm-bottom bottom-white">
                        <a href="{{ url('/unpublished/') }}" role="button" class="btn btn-secondary btn-sm">See Unpublished Posts</a>
                </div>
            </section>
        </div>
        <div class="col-md-4">
            <section class="admin admin-simple-sm">
                <div class="admin-simple-sm-icon">
                    <i class="icon-people icons"></i>
                </div>
                <div class="admin-simple-sm-bottom">Total Users :
                {{ $countUs }}
                </div>
                <div class="admin-simple-sm-bottom bottom-white">
                        <a href="{{ url('/users/') }}" role="button" class="btn btn-secondary btn-sm">See Users</a>
                </div>
            </section>
        </div>
    </div>
</div>
@endsection
