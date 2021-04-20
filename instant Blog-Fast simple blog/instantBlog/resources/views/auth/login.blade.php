@extends('layouts.master')

@section('bodyclass')
    <body class="bg-instant">
@endsection
@if ($setting->site_instant == 1)
@section('jumbotron')
<div class="jumbotron bg-none">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="display-4 text-white">@lang('messages.login.title')</h1>
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container text-white">
    <div class="row">
        <div class="col-md-4">
            <form class="form-horizontal" role="form" method="POST" action="{{ route('login') }}">
                @csrf

                <div class="form-group{{ $errors->has('username') ? ' has-error' : '' }}">
                    <label for="username">@lang('messages.login.username')</label>
                    <input id="username" type="text" class="form-control" name="username" value="{{ old('username') }}" required autofocus>
                        @if ($errors->has('username'))
                        <span class="help-block">
                            <strong>{{ $errors->first('username') }}</strong>
                        </span>
                        @endif
                </div>

                <div class="form-group{{ $errors->has('password') ? ' has-error' : '' }}">
                    <label for="password">@lang('messages.login.password')</label>
                    <input id="password" type="password" class="form-control" name="password" required>
                    @if ($errors->has('password'))
                    <span class="help-block">
                        <strong>{{ $errors->first('password') }}</strong>
                    </span>
                    @endif
                </div>

                <div class="form-group">                    
                <div class="checkbox">
                    <label>
                    @lang('messages.login.dont')
                    <a href="{{ route('register') }}"><strong> @lang('messages.login.sign')</strong></a>
                    </label>
                </div>
                <div class="checkbox">
                    <label>
                    @lang('messages.login.forgot')
                    <a href="{{ route('password.request') }}"><strong> @lang('messages.login.click')</strong></a>
                    </label>
                </div>
                </div>
                <div class="form-group">                    
                    <button type="submit" class="btn btn-primary btnpoint">
                        @lang('messages.login.button')
                    </button>
                </div>
            </form>
        </div>
        <div class="col-md-1">
            <div class="ordiv">                
            </div>
        </div>
        <div class="col-md-4 pt-4">
             <div class="sociallogin mt-4 mb-3">
                <a role="button" class="btn btn-face social" href="{{ url('/auth/facebook') }}" ><i class="icon-social-facebook icons"></i>@lang('messages.login.facebook')</a>
            </div>
             <div class="sociallogin">
                <a role="button" class="btn btn-google social" href="{{ url('/auth/google') }}" ><i class="icon-social-google icons"></i>@lang('messages.login.google')</a>
            </div>
        </div>
    </div>
</div>
@endsection
@else
@include('member.information')
@endif
