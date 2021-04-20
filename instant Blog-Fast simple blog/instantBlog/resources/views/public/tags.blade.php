@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('content')
<div class="se-pre-con"></div>
<div class="container-fluid mt-5">
    <div class="row">
        <div class="grid" data-columns>
            @forelse($tags as $tag)
            <div class="card bg-dark text-white">
                <img class="tag-img" src="{{ url('/uploads/' . $tag->tag_media) }}">
                <div class="card-img-overlay bg-over">
                    <a class="link-over" href="{{ url('/category/' . $tag->name) }}"></a>
                    <p class="card-text text-muted text-uppercase mb-0">{{ str_limit($tag->title, 70) }}</p>
                    <h2 class="text-uppercase"> {{ $tag->name }} </h2>   
                </div>
            </div>
            @empty
            <div class="col-md-12">
                <h5 class="text-light">@lang('messages.nocat')</h5>
            </div>
            @endforelse
        </div>
    </div>
    <hr>
    <div class="row">
        <div class="col-md-12">
            {{ $tags->links() }}
        </div>
    </div>
</div>
@endsection