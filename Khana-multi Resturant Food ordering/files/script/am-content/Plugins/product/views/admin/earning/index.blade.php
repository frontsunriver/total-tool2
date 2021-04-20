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
					{{ strtoupper($currency->value) }} {{ $total_earnings }}
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
					{{ strtoupper($currency->value) }} {{ $today_earnings }}
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
					{{ strtoupper($currency->value) }} {{ $monthly_earnings }}
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
					{{ strtoupper($currency->value) }} {{ $year_earnings }}
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
		<div class="float-left">
          <form method="get" action="{{ route('admin.earning.date') }}">
            <div class="row">
              <div class="form-group ml-3">
                <label>{{ __('Starting Date') }}</label>
                <input type="date" name="start" class="form-control" required>
              </div>
              <div class="form-group ml-2">
               <label>{{ __('End Date') }}</label>
               <input type="date" name="end" class="form-control" required>
             </div>
             <div class="form-group mt-4">
              <button class="btn btn-primary btn-lg  ml-2 mt-1" type="submit">{{ __('Filter') }}</button>
            </div>
          </div>
        </form>
      </div>
		<table class="table">
			<thead>
				<tr>
					<th class="am-title">{{ __('Id') }}</th>
					<th class="am-tags">{{ __('Date') }}</th>
					<th class="am-tags">{{ __('Earnings') }}</th>
					<th class="am-date">{{ __('Last Modified') }}</th>
				</tr>
			</thead>
			<tbody>
				@foreach($total_earnings_paginate as $key=>$earn)
				<tr>
					<td>{{ $key + 1 }}</td>
					<td>{{ $earn->created_at->toDateString() }}</td>
					<td>{{ strtoupper($currency->value) }} {{ $earn->commission }}</td>
					<td>{{ __('Last Modified') }}
						<div class="date">
							{{ $earn->updated_at->diffForHumans() }}
						</div>
					</td>
				</tr>
				@endforeach

			</tbody>

			<tfoot>
				<tr>
					<th class="am-title">{{ __('Id') }}</th>
					<th class="am-tags">{{ __('Date') }}</th>
					<th class="am-tags">{{ __('Earnings') }}</th>
					<th class="am-date">{{ __('Last Modified') }}</th>
				</tr>
			</tfoot>
		</table>
		{{ $total_earnings_paginate->links() }}
	</div>
</div>
@endsection
