@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('content')
<div class="se-pre-con"></div>
<div class="container-fluid mt-5">
    @php
    $enmonth = Request::get('month');
    $month = Carbon\Carbon::parse($enmonth);
    @endphp
    <h5 class="text-light">{{ $month->monthName }} {{ $year = Request::get('year') }}</h5>
    <hr>
    <div class="row">
        <div class="grid" data-columns>
            @forelse($posts as $key => $post)      
            @include('public.post')
            @if (!empty($setting->between_ads))
                @if( ($key + 1) % 9 == 0 )
                <div class="card betads embed-responsive">
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
            {{ $posts->appends(['month' => $enmonth, 'year' => $year])->links() }}
        </div>
    </div>  
</div>
@endsection