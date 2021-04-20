@extends('layouts.master')
@section('css')
<style type="text/css">
    .bg-nav {
        margin-top: 0px!important;
        padding-top: 1.35rem;
        background: linear-gradient(to right bottom, #3f4756, #4a4a69, #634874, #834074, #a23567);
    }
</style>
@endsection
@section('bodyclass')
    <body>
@endsection
@section('jumbotron')
<div class="jumbotron jblight">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="display-4">@lang('messages.user.edit_profile')</h1>
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="row">
        <div class="col-lg-3 col-lg-pull-6 col-md-6">
            <div class="profile-card">
                <div>
                    @if (substr( $user->avatar, 0, 4 ) === "http")
                    <img class="profile-card-photo" src=" {{ $user->avatar }} ">
                    @else
                    <img class="profile-card-photo" src="{{ url('/images/' . $user->avatar) }}">
                    @endif
                </div>
                <div class="profile-card-name">{{ $user->name }}</div>
                <div class="profile-card-username">{{ $user->username }}</div>
            </div>
        </div>
        <div class="col-lg-9 col-lg-push-3 col-md-6">
            <section class="box-white">
                @include('layouts.errors')
                <form method="POST" action="{{url('/profile/' . $user->id)}}" enctype="multipart/form-data">
                    {{ method_field('PUT') }}
                    @csrf
                    <div class="form-group row">
                        <label for="name" class="col-sm-4 col-form-label">@lang('messages.sign.name') :</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="name" name="name" value="{{ $user->name }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="username" class="col-sm-4 col-form-label">@lang('messages.sign.username') :</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="username" name="username" value="{{ $user->username }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="email"  class="col-sm-4 col-form-label">@lang('messages.sign.email') :</label>
                        <div class="col-sm-7">
                            <input type="email" class="form-control" id="email" name="email" value="{{ $user->email }}" required>
                        </div>
                    </div>
                    <div class="form-group row">
                    <label class="col-sm-4 col-form-label" >@lang('messages.sign.user_image')</label>
                        <div class="col-sm-6">     
                            <input type="file" class="form-control-file" id="avatar" name="avatar" aria-describedby="fileHelp">
                            <small id="fileHelp" class="form-text text-muted">@lang('messages.sign.img_help')</small>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="website" class="col-sm-4 col-form-label">@lang('messages.sign.website') :</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="website" name="website" value="{{ $user->website }}">
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="facebook" class="col-sm-4 col-form-label">Facebook :</label>
                        <div class="col-sm-7">
                            <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                                <div class="input-group-addon">facebook.com/</div>
                                <input type="text" class="form-control" id="facebook" name="facebook" value="{{ $user->facebook }}">
                            </div>                            
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="twitter" class="col-sm-4 col-form-label">Twitter :</label>
                        <div class="col-sm-7">
                            <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                                <div class="input-group-addon">twitter.com/</div>
                                <input type="text" class="form-control" id="twitter" name="twitter" value="{{ $user->twitter }}">
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="instagram" class="col-sm-4 col-form-label">Instagram :</label>
                        <div class="col-sm-7">
                            <div class="input-group mb-2 mr-sm-2 mb-sm-0">
                                <div class="input-group-addon">instagram.com/</div>
                                <input type="text" class="form-control" id="instagram" name="instagram" value="{{ $user->instagram }}">
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="linkedin" class="col-sm-4 col-form-label">LinkedIn :</label>
                        <div class="col-sm-7">
                                <input type="text" class="form-control" id="linkedin" name="linkedin" value="{{ $user->linkedin }}">
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="offset-sm-4 col-sm-7">
                            <button type="submit" class="btn btn-primary">@lang('messages.save')</button>
                            <a href="{{ url('/home/') }}" class="btn btn-danger" role="button">@lang('messages.cancel')</a> 
                        </div>
                    </div>
                </form>
            </section>
            <section class="box-white">
                <form method="POST" action="{{url('/profile/' . $user->id)}}" enctype="multipart/form-data">
                    {{ method_field('PUT') }}
                    @csrf
                    <div class="form-group row">
                        <label for="password"  class="col-sm-4 col-form-label">@lang('messages.sign.password') :</label>
                        <div class="col-sm-7">
                            <input type="password" class="form-control" id="password" name="password" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="password_confirmation"  class="col-sm-4 col-form-label">@lang('messages.sign.cpassword') :</label>
                        <div class="col-sm-7">
                            <input type="password" class="form-control" id="password_confirmation" name="password_confirmation" required>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="offset-sm-4 col-sm-7">
                            <button type="submit" class="btn btn-primary">@lang('messages.changepass')</button>
                        </div>
                    </div>
                </form>
            </section>
        </div>        
    </div>
</div>
@endsection