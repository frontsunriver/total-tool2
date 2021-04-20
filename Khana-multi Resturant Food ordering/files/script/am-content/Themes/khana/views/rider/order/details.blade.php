@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'Order Details'])
@php
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp
<h2 class="section-title">{{ __('Order Details') }}</h2>
<p class="section-lead">
	{{ __('Live Order Tracting section. You can track our order with real time.') }}
</p>
<div class="row">
	<div class="col-md-6 col-lg-6">
		<div class="card">

			<div class="card-header mt-1">
				<div class="order d-flex align-items-center ">
					<h4>{{ __('Pickup Info') }}</h4>
					@if($order->status==3 && $riderlog->status != 0)
					<div class="btn-action-area ml-auto">
						<a href="{{ route('rider.order.decline',encrypt($order->id)) }}" class="btn btn-danger mr-2 cancel">{{ __('Cancel') }}</a>

						<a href="{{ route('rider.order.pickup',encrypt($order->id)) }}" class="btn btn-success mr-2">{{ __('Accept') }}</a>
					</div>
					@endif
				</div>
			</div>
			<div class="card-body">
				@php
				$json=json_decode($order->vendorinfo->info->content)
				@endphp
				<p><b>{{ __('Restaurant Name :') }}</b> {{ $order->vendorinfo->name }}</p>
				<p><b>{{ __('Phone 1:') }}</b> <a href="tel:{{ $json->phone1 }}">{{ $json->phone1 }}</a></p>
				<p><b>{{ __('Phone 2:') }}</b> <a href="tel:{{ $json->phone1 }}">{{ $json->phone2 }}</a></p>
				<p><b>{{ __('Address Line:') }}</b> {{ $json->address_line }}</p>
				<p><b>{{ __('Full Address:') }}</b> {{ $json->full_address }}</p>
			</div>

		</div>
	</div>
	<div class="col-md-6 col-lg-6">
		<div class="card">
			
			<div class="card-body">

				<div class="profile-widget">

					<div class="profile-widget-header">

						<div class="profile-widget-items">
							<div class="profile-widget-item">
								<div class="profile-widget-item-label">{{ __('Amount') }}</div>
								<div class="profile-widget-item-value">{{ strtoupper($currency->value) }} {{ $order->total+$order->shipping }}</div>
							</div>
							<div class="profile-widget-item">
								<div class="profile-widget-item-label">{{ __('Payment Mode') }}</div>
								<div class="profile-widget-item-value">{{ strtoupper($order->payment_method) }}</div>
							</div>
							<div class="profile-widget-item">
								<div class="profile-widget-item-label">{{ __('Order Status') }}</div>
								<div class="profile-widget-item-value"> 
									@if($order->status == 2)
									<span class="text-success">{{ __('Pending') }}</span>
									@elseif($order->status == 1)
									<span class="text-success">{{ __('Complete') }}</span>
									@elseif($order->status == 3)
									<span class="text-warning">{{ __('Seller Order Accepted') }}</span>
									@elseif($order->status == 0)
									<span class="text-danger">{{ __('Cancel') }}</span>
									@endif
								</div>

						</div>
						<div class="profile-widget-item">
								<div class="profile-widget-item-label">{{ __('Payment Status') }}</div>
								<div class="profile-widget-item-value"> 
									
									@if($order->payment_status == 1)
									<span class="text-success">{{ __('Complete') }}</span>
									
									@elseif($order->payment_status == 0)
									<span class="text-warning">{{ __('Pending') }}</span>
									@endif
								
							</div>

						</div>
					</div>
				</div>
				@php 
				$customerInfo = json_decode($order->data);
				@endphp
				<div class="profile-widget-description mt-4">
					<div class="profile-widget-name">{{ __('Customer Name') }}: <div class="text-muted d-inline font-weight-normal"> {{ $customerInfo->name }}</div></div>
					<div class="profile-widget-name">{{ __('Customer Phone') }}: <div class="text-muted d-inline font-weight-normal"><a href="tel:{{ $customerInfo->phone }}"> {{ $customerInfo->phone }}</a></div></div>
					<div class="profile-widget-name"> {{ __(
						'Delevery Address') }}: <div class="text-muted d-inline font-weight-normal"> {{ $customerInfo->address }}</div></div>
					</div>
				</div>
			</div>
		</div>
	</div>


	<div class="col-md-12" >
		<div class="card">
		<div class="card-body map-canvas-register" id="map-canvas">
				
		</div>
	</div>
</div>
</div>

@endsection
@section('script')
<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&sensor=false&callback=initialise"></script>
<script type="text/javascript">
	"use strict";
	var resturent_lat = {{ $order->vendorinfo->location->latitude }};
	var resturent_long  = {{ $order->vendorinfo->location->longitude }};

	var customer_lat = {{ $customerInfo->latitude }};
	var customer_long = {{ $customerInfo->longitude }};

	var resturent_icon= '{{ asset('uploads/resturent.png') }}';
	var user_icon= '{{ asset('uploads/userpin.png') }}';
	
	var customer_name= '{{ $customerInfo->name }}';
	var resturent_name= '{{ $order->vendorinfo->name }}';
	var mainUrl= "{{ url('/') }}";
</script>
<script src="{{ asset('admin/backend/orderdetailsrider.js') }}"></script>
@endsection
