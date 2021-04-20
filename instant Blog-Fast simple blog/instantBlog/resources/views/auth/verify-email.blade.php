@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('jumbotron')
<div class="jumbotron bg-none">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="display-4 text-white">@lang('messages.login.verify')</h1>
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container text-white">
    <div class="row">
        <div class="col-md-8 col-md-offset-2">
            <p class="text-white">@lang('messages.login.verifydesc')</p>
            <div class="panel panel-default">
                <div class="panel-body">
                    @if (session('status'))
                        <div class="alert alert-success">
                            {{ session('status') }}
                        </div>
                    @endif

                     @if (session('status') == 'verification-link-sent')
                       <div class="alert alert-success">
                            @lang('messages.login.verify')
                        </div>
                    @endif

                    <form class="form-horizontal" role="form" method="POST" action="{{ route('verification.send') }}">
                        @csrf

                        <div class="form-group">
                            <div class="col-md-6 col-md-offset-4">
                                <button type="submit" class="btn btn-primary">
                                   @lang('messages.login.verify')
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
@endsection
