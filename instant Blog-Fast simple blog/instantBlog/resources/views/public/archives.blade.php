@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('content')
<div class="container-fluid mt-5">
    <div class="row">
        @foreach($archives as $stats)
        @php
        $month = Carbon\Carbon::parse($stats['month']);
        @endphp   
        <div class="col-md-2 mb-4">
            <a href="{{url('/archiveposts/?month=' . $stats['month'] . '&year=' . $stats['year']) }}" class="btn btn-secondary" role="button">{{ $month->monthName . ' ' . $stats['year'] }}</a>
        </div>
        @endforeach
    </div>
</div>
@endsection