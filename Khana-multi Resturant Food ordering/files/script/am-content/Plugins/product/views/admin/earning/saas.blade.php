@extends('layouts.backend.app')

@section('content')
@php
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp
<div class="row">
	<div class="col-lg-3 col-md-6 col-sm-6 col-12">
		<div class="card card-statistic-1">
			<div class="card-icon bg-primary">
				<i class="far fa-user"></i>
			</div>
			<div class="card-wrap">
				<div class="card-header">
					<h4>{{ __('Total Earnings') }}</h4>
				</div>
				<div class="card-body">
					{{ strtoupper($currency->value) }} {{ number_format($total_earnings,2) }}
				</div>
			</div>
		</div>
	</div>
	<div class="col-lg-3 col-md-6 col-sm-6 col-12">
		<div class="card card-statistic-1">
			<div class="card-icon bg-danger">
				<i class="far fa-newspaper"></i>
			</div>
			<div class="card-wrap">
				<div class="card-header">
					<h4>{{ __('Today Earnings') }}</h4>
				</div>
				<div class="card-body">
					{{ strtoupper($currency->value) }} {{ number_format($today_earnings,2) }}
				</div>
			</div>
		</div>
	</div>
	<div class="col-lg-3 col-md-6 col-sm-6 col-12">
		<div class="card card-statistic-1">
			<div class="card-icon bg-warning">
				<i class="far fa-file"></i>
			</div>
			<div class="card-wrap">
				<div class="card-header">
					<h4>{{ __('This Month') }}</h4>
				</div>
				<div class="card-body">
					{{ strtoupper($currency->value) }} {{ number_format($monthly_earnings,2) }}
				</div>
			</div>
		</div>
	</div>
	<div class="col-lg-3 col-md-6 col-sm-6 col-12">
		<div class="card card-statistic-1">
			<div class="card-icon bg-success">
				<i class="fas fa-circle"></i>
			</div>
			<div class="card-wrap">
				<div class="card-header">
					<h4>{{ __('This Year') }}</h4>
				</div>
				<div class="card-body">
					{{ strtoupper($currency->value) }} {{ number_format($year_earnings,2) }}
				</div>
			</div>
		</div>
	</div>
</div>
<div class="card">
	<div class="card-header">
		<h5 class="mb-0">{{ __('Total Earning Satements') }}</h5>
	</div>
	<div class="card-body">
	
		<table class="table text-center">
			<thead>
				<tr>
					<th class="am-title">{{ __('#') }}</th>
					<th class="am-tags">{{ __('Date') }}</th>
					<th class="am-tags">{{ __('Earnings') }}</th>
					<th class="am-date">{{ __('Created At') }}</th>
				</tr>
			</thead>
			<tbody>
				@foreach($orders as $key=>$earn)
				<tr>
					<td>{{ $key + 1 }}</td>
					<td>{{ $earn->created_at->toDateString() }}</td>
					<td>{{ strtoupper($currency->value) }} {{ $earn->amount }}</td>
					<td>{{ __('Created At') }}
						<div class="date">
							{{ $earn->updated_at->diffForHumans() }}
						</div>
					</td>
				</tr>
				@endforeach

			</tbody>

			<tfoot>
				<tr>
					<th class="am-title">{{ __('#') }}</th>
					<th class="am-tags">{{ __('Date') }}</th>
					<th class="am-tags">{{ __('Earnings') }}</th>
					<th class="am-date">{{ __('Last Modified') }}</th>
				</tr>
			</tfoot>
		</table>
		{{ $orders->links() }}
	</div>
</div>
@endsection
