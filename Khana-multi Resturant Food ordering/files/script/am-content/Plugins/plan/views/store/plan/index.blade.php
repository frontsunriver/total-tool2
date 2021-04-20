@extends('layouts.backend.app')

@section('content')
@include('layouts.backend.partials.headersection',['title'=>'Pricing'])
@php
$currency=\App\Options::where('key','currency_icon')->select('value')->first();
@endphp
<!-- Main Content -->
<section class="section">
	<div class="section-body">
		<h2 class="section-title">{{ __('Pricing') }}</h2>
		<p class="section-lead">{{ __('Price Table. You can also update your plan.') }}</p>
		<div class="row">
			<div class="col-sm-12">
				@if (\Session::has('success'))
				<div class="alert alert-success">
					<ul>
						<li>{!! \Session::get('success') !!}</li>
					</ul>
				</div>
				@endif
				@if (\Session::has('error'))
				<div class="alert alert-danger">
					<ul>
						<li>{!! \Session::get('error') !!}</li>
					</ul>
				</div>
				@endif
			</div>
			@foreach($plans as $plan)
			<div class="col-12 col-md-4 col-lg-4">
				<div class="pricing {{ $plan->id == Auth::User()->plan_id ? 'pricing-highlight' : '' }}">
					<div class="pricing-title">
						{{ $plan->name }}
					</div>
					<div class="pricing-padding">
						<div class="pricing-price">
							<div>{{ strtoupper($currency->value) }}{{ $plan->s_price }}</div>
							<div>{{ __('Per') }} {{ ucfirst($plan->duration) }}</div>
						</div>
						<div class="pricing-details">
							<div class="pricing-item">
								<div class="pricing-item-icon"><i class="fas fa-check"></i></div>
								<div class="pricing-item-label">{{ $plan->commission }}{{ __('% Commission Fee') }}</div>
							</div>
							@if($plan->f_resturent == 1)
							<div class="pricing-item">
								<div class="pricing-item-icon"><i class="fas fa-check"></i></div>
								<div class="pricing-item-label">{{ __('Featured Restutant') }}</div>
							</div>
							@else
							<div class="pricing-item">
								<div class="pricing-item-icon bg-danger text-white"><i class="fas fa-times"></i></div>
								<div class="pricing-item-label">{{ __('Featured Restutant') }}</div>
							</div>
							@endif
							@if($plan->table_book == 1)
							<div class="pricing-item">
								<div class="pricing-item-icon"><i class="fas fa-check"></i></div>
								<div class="pricing-item-label">{{ __('Table Booking') }}</div>
							</div>
							@else
							<div class="pricing-item">
								<div class="pricing-item-icon bg-danger text-white"><i class="fas fa-times"></i></div>
								<div class="pricing-item-label">{{ __('Table Booking') }}</div>
							</div>
							@endif
							<div class="pricing-item">
								<div class="pricing-item-icon"><i class="fas fa-check"></i></div>
								<div class="pricing-item-label">{{ $plan->img_limit }} {{ __('Image Upload Limit') }}</div>
							</div>

						</div>
					</div>
					@if($plan->id == Auth::User()->plan_id)
					<div class="pricing-cta">
						<a href="{{ route('store.plan.checkout',encrypt($plan->id)) }}" >{{ __('Subscribed') }} <i class="fas fa-arrow-right"></i></a>
					</div>
					@else
					<div class="pricing-cta">
						@if($plan->s_price > 0 )
						<a href="{{ route('store.plan.checkout',encrypt($plan->id)) }}">{{ __('Buy Now') }} <i class="fas fa-arrow-right"></i></a>
						@endif
					</div>
					@endif
				</div>
			</div>
			@endforeach
		</div>
	</div>

	<div class="section-body">
		<h2 class="section-title">{{ __('Subscription History') }}</h2>
		<dvi class="card">
			<div class="card-body">
				

				<div class="row">
					<div class="col-md-12">
						<div class="table-responsive">
							@php
							$orders=App\Userplan::where('user_id',Auth::id())->with('usersaas')->latest()->paginate(20);
							@endphp
							<table class="table table-striped table-hover">
								<tr>
									<th>{{ __('ID') }}</th>
									<th>{{ __('Plan Name') }}</th>
									<th>{{ __('Payment Method') }}</th>
									<th>{{ __('Payment Status') }}</th>
									<th>{{ __('Status') }}</th>
									<th>{{ __('Amount') }}</th>
									<th>{{ __('Transaction id') }}</th>
									<th>{{ __('Subscribed At') }}</th>
								</tr>
								@foreach($orders as $row)
								<tr>
									<td>#{{ $row->id }}</td>
									<td>{{ $row->usersaas->name }}</td>
									<td>{{ strtoupper($row->payment_method) }}</td>
									<td>{{ $row->payment_status }}</td>
									<td>{{ $row->status }}</td>
									<td>{{ number_format($row->amount,2) }}</td>
									<td>@if($row->image != 'default.png') {{ $row->image }} @endif</td>
									<td>{{ $row->created_at->diffforHumans() }}</td>
								</tr>
								@endforeach
							</table>
							{{ $orders->links() }}
						</div>
					</div>
				</div>
			</div>
		</dvi>	
	</div>	
</section>
@endsection