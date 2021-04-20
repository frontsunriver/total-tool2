@extends('layouts.backend.app')
@section('content')
<div class="row">
	<div class="col-lg-12">
		<div class="card">
			<div class="card-header d-flex">
				<div class="order ">
					<h3>{{ __('Tracking') }}</h3>
					<span id="time" class="text-danger"></span>
				</div>
				<div class="ml-auto">
					@if($order->status == 3)
					<a href="#" class="btn btn-primary text-right" data-toggle="modal" data-target="#exampleModal">{{ __('Delivery Complete') }}</a>
					@endif
					
				</div>
			</div>
			<div class="card-header">
				<div class="order d-flex align-items-center">
					<div class="col-6">
						
						<p><b>{{ __('Delevery Address') }}:</b> {{  $customerInfo->address }}</p>
						<p><b>{{ __('Customer Name') }}:</b> {{  $customerInfo->name }}</p>
						<p><b>{{ __('Customer Phone') }}:</b> {{  $customerInfo->phone }}</p>

					</div>
					<div class="col-6">
						<p><b>{{ __('Restaurant Address') }}:</b> {{  $customerInfo->address }}</p>
						<p><b>{{ __('Restaurant Name') }}:</b> {{  $customerInfo->name }}</p>
						<p><b>{{ __('Restaurant Phone') }}:</b> {{  $customerInfo->name }}</p>
					</div>	
				</div>
			</div>
			<div class="card-body">
				
				<div id="map-canvas" class="map-canvas-register"></div>
				<small class="text-danger">{{ __('Note: if you are getting your wrong location just turn on your GPS from your device') }}</small>
			</div>
		</div>
	</div>
</div>


<form id="basicform1" method="post" action="{{ route('rider.order.live') }}">
	@csrf
	<input type="hidden" name="id" value="{{ $order->id }}">
	<input type="hidden" name="lat" id="lat" required>
	<input type="hidden" name="long" id="long" required>
</form>

@endsection
@if($order->status==3 )
@section('extra')
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">{{ __('Order Complete') }}</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <form id="basicform" method="post" action="{{ route('rider.order.delivery',encrypt($order->id)) }}">
      	@csrf
      <div class="modal-body">
      	@if($order->payment_status == 0)
      	<div class="from-group">
        	<label for="payment">{{ __('Total Amount') }}</label>
        	<input type="text" class="form-control" disabled="" value="{{ number_format($order->shipping+$order->total) }}" />
        </div>
        <div class="from-group">
        	<label for="payment">{{ __('Take Payment ?') }}</label>
        	<select class="form-control" name="payment_status">
        			<option value="1">{{ __('Yes') }}</option>
        			<option value="0">{{ __('No') }}</option>
        	</select>
        </div>
        @endif
        <div class="from-group">
        	<label for="payment">{{ __('Delivery Complete ?') }}</label>
        	<select class="form-control" name="status">
        			<option value="1">{{ __('Yes') }}</option>
        			<option value="0">{{ __('No') }}</option>
        	</select>
        </div>
        
        <div class="form-group">
      	<input type="checkbox" name="check" value="1" id="check" required>
        <label for="check">{{ __('Order Completed') }}</label>
        </div>
      </div>
      
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">{{ __('Close') }}</button>
        <button type="submit" class="btn btn-primary">{{ __('Save') }}</button>
      </div>
       </form>
    </div>
  </div>
</div>
@endsection
@endif
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&sensor=false&callback=initialise"></script>
<script type="text/javascript">
	"use strict";
	var resturent_lat = {{ $order->vendorinfo->location->latitude }};
	var resturent_long  = {{ $order->vendorinfo->location->longitude }};

	var customer_lat = {{ $customerInfo->latitude }};
	var customer_long = {{ $customerInfo->longitude }};

	var resturent_icon= '{{ asset('uploads/resturent.png') }}';
	var user_icon= '{{ asset('uploads/userpin.png') }}';
	
	var my_icon= '{{ asset('uploads/delivery.png') }}';
	var my_name= '{{ Auth::User()->name }}';

	var customer_name= '{{ $customerInfo->name }}';
	var resturent_name= '{{ $order->vendorinfo->name }}';
	var mainUrl= "{{ url('/') }}";

	var barurl= '{{ route('rider.live.order') }}';
	
</script>
<script src="{{ asset('admin/backend/orderend.js') }}"></script>
@endsection