@extends('layouts.backend.app')
@section('style')
<link rel="stylesheet" href="{{ asset('admin/assets/css/daterangepicker.css') }}">
@endsection
@section('content')
@php
$currency=\App\Options::where('key','currency_icon')->select('value')->first();
@endphp
<div class="row">
	<div class="col-sm-4">
		<div class="card">
			<div class="card-body text-center">
				<p>
					{{ __('Earnings this month') }} ({{ date('F') }}) 
					<h3>{{ strtoupper($currency->value) }}{{ number_format($currentMonthAmount,2) }}</h3>
					
				</p>
			</div>
		</div>
	</div>
	<div class="col-sm-4">
		<div class="card">
			<div class="card-body text-center">
				<p>
					
					{{ __('Your balance') }}
					<h3>{{ strtoupper($currency->value) }}{{ number_format($currentMonthAmount-$lastTransection,2) }}</h3>
					
				</p>
			</div>
		</div>
	</div>
	<div class="col-sm-4">
		<div class="card">
			<div class="card-body text-center">
				<p>
					{{ __('Total value of your Earnings') }}
					<h3><b>{{ strtoupper($currency->value) }}{{ number_format($totals,2) }}</b></h3>
					
				</p>
			</div>
		</div>
	</div>
	<div class="col-sm-12">
		<div class="card card-primary">
			<div class="card-header">
				<h4>{{ __('History') }}</h4>
				<form class="card-header-form">
					<div class="input-group">
						<input type="text" name="date" class="form-control daterange-cus" value="{{ $date ?? '' }}" required="">
						<div class="input-group-btn">
							<button type="submit" class="btn btn-primary btn-icon"><i class="fas fa-search"></i></button>
						</div>
					</div>

				</form>
			</div>
			<div class="card-body">
				<div class="table-responsive">
					<table class="table">
						<tr>
							<th>{{ __('ORDER ID') }}</th>
							<th>{{ __('Commision') }}</th>
							<th>{{ __('Date') }}</th>
							<th>{{ __('View') }}</th>
						</tr>

						@foreach($allorders as $row)
						<tr>
							<td><a href="{{ route('rider.order.details',$row->order_id) }}">#{{ $row->order_id }}</a></td>
							<td>{{ strtoupper($currency->value) }}{{ $row->commision }}</td>
							
							<td>{{ $row->created_at->format('Y-F-d') }}</td>
							<td><a href="{{ route('rider.order.details',$row->order_id) }}" class="btn btn-primary btn-sm"><i class="far fa-eye"></i></a></td>
						</tr>
						@endforeach
					</table>
					{{ $allorders->links() }}
				</div>
			</div>
		</div>
	</div>
	
</div>

@endsection
@section('script')
<script src="{{ asset('admin/assets/js/moment.min.js') }}"></script>
<script src="{{ asset('admin/assets/js/daterangepicker.min.js') }}"></script>
<script>
	"use strict";
	$('.daterange-cus').daterangepicker();
</script>
@endsection