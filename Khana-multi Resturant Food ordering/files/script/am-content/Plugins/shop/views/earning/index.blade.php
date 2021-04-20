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
					{{ __('Sales earnings in last 30 days') }} {{ __('before taxes') }}
					<h3>{{ strtoupper($currency->value) }} {{ number_format($currentMonthAmount) }}</h3>
					
				</p>
			</div>
		</div>
	</div>
	<div class="col-sm-4">
		<div class="card">
			<div class="card-body text-center">
				<p>
					
					{{ __('Your balance') }}:
					<h3>{{ strtoupper($currency->value) }} {{ number_format($currentMonthAmount-$currentMonthTax-$totalWithdraw) }}</h3>
					
				</p>
			</div>
		</div>
	</div>
	<div class="col-sm-4">
		<div class="card">
			<div class="card-body text-center">
				<p>
					{{ __('Total value of your sales, before taxes') }}:
					<h3><b>{{ strtoupper($currency->value) }} {{ number_format($totals) }}</b></h3>
					
				</p>
			</div>
		</div>
	</div>
	<div class="col-sm-9">
		<div class="card card-primary">
			<div class="card-header">
				<h4>{{ __('Orders') }}</h4>
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
							<th>{{ __('Amount') }}</th>
							<th>{{ __('Commssion') }}</th>
							<th>{{ __('Order Typ') }}e</th>
							<th>{{ __('Date') }}</th>
						</tr>

						@foreach($allorders as $row)
						<tr>
							<td><a href="{{ route('store.order.show',$row->id) }}">#{{ $row->id }}</a></td>
							<td>{{ strtoupper($currency->value) }} {{ $row->total }}</td>
							<td>-{{ strtoupper($currency->value) }} {{ $row->commission }}</td>
							<td>@if($row->order_type == 1) <span class="badge badge-success">{{ __('Home Delivery') }}</span> @else <span class="badge badge-primary">{{ __('Pickup Delivery') }}</span> @endif</td>
							<td>{{ $row->created_at->format('Y-F-d') }}</td>
							
						</tr>
						@endforeach
					</table>
					{{ $allorders->links() }}
				</div>
			</div>
		</div>
	</div>
	<div class="col-sm-3">
		<div class="card card-primary">
			<div class="card-header">
				<h4>{{ __('Overview Of Amount') }}</h4>
			</div>
			<div class="card-body">
				<ul class="list-group">
					<li class="list-group-item d-flex justify-content-between align-items-center">
						{{ __('Total') }}
						<span class="badge badge-primary badge-pill">{{ $total ??  number_format($totals) }}</span>
					</li>
					<li class="list-group-item d-flex justify-content-between align-items-center">
						{{ __('Commisions') }}
						<span class="badge badge-primary badge-pill">{{  $tax ?? number_format($currentMonthAmount-$currentMonthTax) }}</span>
					</li>
					<li class="list-group-item d-flex justify-content-between align-items-center">
						{{ __('Balance') }}
						<span class="badge badge-primary badge-pill">{{ $net_total ??  number_format($currentMonthAmount-$currentMonthTax) }}</span>
					</li>
				</ul>
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