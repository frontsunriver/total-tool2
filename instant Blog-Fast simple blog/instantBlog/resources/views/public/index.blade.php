@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('content')
<div class="se-pre-con"></div>
<div class="container-fluid mt-5">
    <div class="row">
        <div class="grid" data-columns>
            @forelse($posts as $key => $post)      
            @include('public.post')
            @if (!empty($setting->between_ads))
                @if( ($key + 1) % 9 == 0 )
                <div class="card border-one betads embed-responsive">
                    {!! $setting->between_ads !!}
                </div>
                @endif
            @endif
            @empty
            <div class="col-md-12">
                <h5 class="text-light">@lang('messages.nopost')</h5>
            </div>
            @endforelse    
        </div>
    </div>
    <hr>
    <div class="row">
        <div class="col-md-12">
            {{ $posts->links() }}
        </div>
    </div>  
</div>
@endsection
@section('extra')
<footer class="blog-footer">
@if (count($pages) > 0)
    <ul class="list-inline">
    @foreach ($pages as $page)    
       <li class="list-inline-item"><a class="text-light" href="{{url('/page/' . $page->page_slug)}}">{{ $page->page_title }}</a></li>
    @endforeach
    </ul>
@endif
@if (!empty($setting->footer))
<div class="text-muted foottxt">{!! clean($setting->footer) !!}</div>
@endif
</footer>
@endsection