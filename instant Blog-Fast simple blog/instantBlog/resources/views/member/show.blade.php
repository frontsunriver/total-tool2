@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('jumbotron')
<div class="jumbotron bg-none">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="text-white display-4">@lang('messages.delete')</h1>
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container">
    <div class="box-white ml-3 mr-3">
   @canany(['own-post', 'moderator-post'], $post)
        <div class="col-md-12"> 
            <h2 class="text-center">@lang('messages.askdelete')</h2>     
            <p class="lead text-center"><strong>@lang('messages.askcont') :</strong>{{ str_limit($post->post_title, 65) }}</p>
            <hr>
        </div>
        <div class="row">
            <div class="col-md-6">
                <form action="{{ url('/home/' . $post->id) }}" method="POST">
                    @csrf
                    {{ method_field('DELETE') }}
                    <button type="submit" class="btn btn-danger float-right">@lang('messages.delete')</button>
                </form>
            </div>
            <div class="col-md-6">
                <a href="{{ url('/home/') }}" class="btn btn-primary" role="button">@lang('messages.cancel')</a>      
            </div>
        </div>
    @else
    <div class="alert alert-warning" role="alert">
      <strong>@lang('messages.nopost')</strong> @lang('messages.backto') <a href="{{ url('/') }}">@lang('messages.hometxt')</a>.
    </div>
    @endcanany
    </div>
</div>
@endsection
