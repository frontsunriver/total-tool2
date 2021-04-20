@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('content')
<div class="container mt-5">
    <div class="row">
        <div class="col-md-12">     
            <div class="card">
                <div class="card-body">
                    <h1>{{ $page->page_title }}</h1>
                    {!! $page->page_content !!}
                </div>
            </div>
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