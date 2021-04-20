@extends('theme::layouts.app')

@section('content')

<!-- success-alert start -->
<div class="alert-message-area">
	<div class="alert-content">
		<h4 class="ale">{{ __('Your Settings Successfully Updated') }}</h4>
	</div>
</div>
<!-- success-alert end -->

<!-- error-alert start -->
<div class="error-message-area">
	<div class="error-content">
		<h4 class="error-msg"></h4>
	</div>
</div>
<!-- error-alert end -->


<!-- checkout area start -->
<section>
	<div class="checkout-area">
		<div class="container">
			<div class="row mt-50">
				<div class="col-lg-8">
					<div class="single-checkout mb-50">
						<h3><span>1</span> {{ __('Delivery Details') }}</h3>
						<h6 class="text-danger none" id="msg">{{ __('Service Not Available On Your Area') }}</h6>
						<div class="delivery-form">
							<div class="row">
								<div class="col-lg-6">
									<div class="form-group">
										<label for="first_name">{{ __('Name') }}</label>
										<input autocomplete="off" type="text" class="form-control" name="name" id="first_name" placeholder="{{ __('Name') }}" value="{{ Auth::user()->name ?? '' }}">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label for="phone">{{ __('Phone Number') }}</label>
										<input autocomplete="off" type="number" class="form-control" name="phone" id="phone" placeholder="{{ __('Phone Number') }}" required autocomplete="off">
									</div>
								</div>
								@if($ordertype == 1)
								<input type="hidden" name="latitude" id="latitude">
								<input type="hidden" name="longitude" id="longitude">



								<div class="col-lg-12">
									<div class="form-group">
										<label for="billing">{{ __('Delivery Address') }}</label>
										<input  type="text" class="form-control location_input" autocomplete="off" id="location_input" placeholder="{{ __('Delivery Address') }}" name="delivery_address" required>
									</div>
								</div>



								<div class="col-lg-12">
									<div class="form-group">
										<div class="map-canvas" id="map-canvas">

										</div>
										<input type="hidden" name="shipping" id="shipping">
									</div>
								</div>

								@endif
								<div class="col-lg-12">
									<div class="form-group">
										<label for="order_details">{{ __('Order Note') }}</label>
										<textarea name="note" class="form-control" name="order_note" id="order_details" cols="5" rows="5" maxlength="200" placeholder="{{ __('Order Note') }}"></textarea>
									</div>
								</div>
							</div>
						</div>
					</div>
					<input type="hidden" name="payment_type" value="stripe" id="payment_type">
					<div class="single-checkout">
						<h3 class="mb-40"><span>2</span> {{ __('Select Payment Method') }}</h3>
						<ul class="nav nav-tabs">
							<li>
								<a href="#stripe" onclick="select_payment('stripe')" data-toggle="tab" class="active">
									<div class="single-payment text-center">
										<div class="payment-img d-flex justify-content-center">
											<img class="mr-2" src="{{ theme_asset('khana/public/img/visa.svg') }}" alt="">
											<img src="{{ theme_asset('khana/public/img/mastercard.svg') }}" alt="">
										</div>
										<span>{{ __('Credit Card') }}</span>
									</div>
								</a>
							</li>
							<li>
								<a href="#paypal" onclick="select_payment('paypal')" data-toggle="tab">
									<div class="single-payment text-center">
										<div class="payment-img d-flex justify-content-center">
											<img src="{{ theme_asset('khana/public/img/paypal.png') }}" alt="">
										</div>
										<span>{{ __('Paypal') }}</span>
									</div>
								</a>
							</li>
							@if($ordertype == 1)
							<li>
								<a href="#cod" onclick="select_payment('cod')" data-toggle="tab">
									<div class="single-payment text-center">
										<div class="payment-img d-flex justify-content-center">
											<img src="{{ theme_asset('khana/public/img/cod.svg') }}" alt="">
										</div>
										<span>{{ __('Cash On Delivery') }}</span>
									</div>
								</a>
							</li>
							@endif
						</ul>
						<div class="tab-content">
							<div class="single-payment-details tab-pane fade active show" id="stripe" class="payment_form">
								<h4 class="mt-40">{{ __('Credit / Debit Card') }}</h4>
								<div class="row">
									<div class="col-lg-12">
										<div class="delivery-form">
											<form id="payment-form" method="post" action="{{ route('order.store') }}">
												@csrf
												<div class="row mt-10">
													<div class="col-lg-12">
														<div class="form-group">
															<label>{{ __('Card from name') }}</label>
															<input type="text" class="form-control" name="card_form_name">
														</div>
													</div>
													<div class="col-lg-12">
														<script src="https://js.stripe.com/v3/"></script>
														<label for="card-element">
															{{ __('Credit or debit card') }}
														</label>
														<div id="card-element">
															<!-- A Stripe Element will be inserted here. -->
														</div>

														<!-- Used to display form errors. -->
														<div id="card-errors" role="alert"></div>
													</div>
													<input type="hidden" name="coupon_url" value="{{ route('coupon') }}">
													<div class="col-lg-12">
														<div class="place-order mt-20">
															<button type="submit">{{ __('Place Order') }}</button>
														</div>
													</div>
												</div>
											</form>
										</div>
									</div>
								</div>
							</div>
							<div class="single-payment-details tab-pane fade" id="paypal">
								<div class="row">
									<div class="col-lg-12">
										<div class="delivery-form text-center mt-80 mb-40">
											<div id="paypal-button"></div>
										</div>
									</div>
								</div>
							</div>
							<div class="single-payment-details tab-pane fade" id="cod">
								<h4 class="mt-40">{{ __('PAY BY CASH ON DELIVERY') }}</h4>
								<form id="cod_payment_form" action="{{ route('order.store') }}" method="POST">
									@csrf
									<div class="row">
										<div class="col-lg-12">
											<div class="delivery-form">
												<p>{{ __('Consider payment upon ordering for contactless delivery') }}</p>
												<div class="place-order mt-20">
													<button type="submit">{{ __('Place Order') }}</button>
												</div>
											</div>
										</div>
									</div>
								</form>
							</div>
						</div>
					</div>
				</div>
				<div class="col-lg-4">
					<div id="checkout_right">
						<div class="checkout-right-section">
							<div class="order-store text-center">
								<h4>{{ __('Your Order') }} {{ Session::get('restaurant_id')['name'] }}</h4>
							</div>
							<div class="order-product-list">
								@foreach(Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->content() as $cart)
								<div class="single-order-product-info d-flex">
									<div class="product-qty-name">
										<span class="product-qty">{{ $cart->qty }}</span> <span class="symbol">x</span><span>{{ $cart->name }}</span>
									</div>
									<div class="product-price-info">
										<span>{{ $currency->value }}: {{ number_format($cart->price,2) }}</span>
									</div>
								</div>
								@endforeach
							</div>
							<div class="product-another-info-show">
								<div class="single-product-another-info-show d-flex">
									<span class="product-another">{{ __('Subtotal') }}</span>
									<span class="product-price">{{ $currency->value }}: {{ Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->priceTotal() }}</span>
								</div>
								@if(Session::has('coupon'))
								<div class="single-product-another-info-show d-flex">
									<span class="product-another">{{ __('Discount') }}</span>
									<span class="product-price">{{ Session::get('coupon')['percent'] }}%</span>
								</div>
								@endif
								@if($ordertype == 1)
								<div class="single-product-another-info-show d-flex">
									<span class="product-another">{{ __('Delivery fee') }}</span>
									<span class="product-price">{{ $currency->value }}: <span id="delivery_fee"></span></span>
								</div>
								@endif
								<div class="single-product-another-info-show total d-flex">
									<span class="product-another">{{ __('Total(Incl. VAT)') }}</span>
									<span class="product-price">{{ $currency->value }}: <span id="last_total">{{ Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->total() }}</span></span>
								</div>
							</div>
						</div>
						@if(!Session::has('coupon'))
						<div class="checkout-right-section mt-35">
							<form action="{{ route('coupon') }}" method="POST" id="couponform">
								@csrf
								<div class="apply-coupon">
									<div class="form-group">
										<label>{{ __('Enter Coupon Code') }}</label>
										<div class="d-flex">
											<input class="form-control" type="text" name="code">
											<button type="submit">{{ __('Apply') }}</button>
										</div>
									</div>
								</div>
							</form>
						</div>
						@endif
					</div>
				</div>
			</div>
		</div>
	</div>
</section>
<!-- checkout area end -->
<input type="hidden" name="base_url" id="base_url" value="{{ url('/') }}">
<input type="hidden" name="_token" id="_token" value="{{ csrf_token() }}">
<input type="hidden" name="total_amount" id="total_amount" value="{{ number_format(str_replace(',', '', Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->total()),2) }}">
<input type="hidden" name="total_price" id="total_price" value="{{ number_format(str_replace(',', '', Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->total()),2) }}">
<input type="hidden" id="stripe_api_key" value="{{ env('STRIPE_KEY') }}">
<input type="hidden" id="paypal_api_key" value="{{ env('PAYPAL_CLIENT_ID') }}">
<input type="hidden" id="currency_value" value="{{ $currency->value }}">
@endsection
@push('js')
 <script>
//
$("body").on("contextmenu",function(e){
return false;
});
$(document).keydown(function(e){
if (e.ctrlKey && (e.keyCode === 67 || e.keyCode === 86 || e.keyCode === 85 || e.keyCode === 117)){
return false;
}
if(e.which === 123){
return false;
}
if(e.metaKey){
return false;
}
//document.onkeydown = function(e) {
// "I" key
if (e.ctrlKey && e.shiftKey && e.keyCode == 73) {
return false;
}
// "J" key
if (e.ctrlKey && e.shiftKey && e.keyCode == 74) {
return false;
}
// "S" key + macOS
if (e.keyCode == 83 && (navigator.platform.match("Mac") ? e.metaKey : e.ctrlKey)) {
return false;
}
if (e.keyCode == 224 && (navigator.platform.match("Mac") ? e.metaKey : e.ctrlKey)) {
return false;
}
// "U" key
if (e.ctrlKey && e.keyCode == 85) {
return false;
}
// "F12" key
if (event.keyCode == 123) {
return false;
}
});
</script> 

<!-- stripe js -->
<script src="https://www.paypalobjects.com/api/checkout.js"></script>
<script src="{{ theme_asset('khana/public/js/checkout/payment.js') }}"></script>
@if($ordertype == 1)

<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&callback=initialize"></script>
<script>
	"use strict";
	if (localStorage.getItem('location') != null) {
		var locs= localStorage.getItem('location');
	}
	else{
		var locs = "{{ $json->full_address }}";
	}
	$('#location_input').val(locs);
	if (localStorage.getItem('lat') !== null) {
		var lati=localStorage.getItem('lat');
		$('#latitude').val(lati)
	}	
	else{
		var lati= {{ $resturent_info->resturentlocation->latitude }};
	}

	if (localStorage.getItem('long') !== null) {
		var longlat=localStorage.getItem('long');
		$('#longitude').val(longlat)
	}
	else{
		var longlat= {{ $resturent_info->resturentlocation->longitude }};

	}


	var resturentlocation="{{ $json->full_address }}";
	var feePerkilo= {{ $km_rate->value }};
	var mapOptions;
	var map;
	var marker;
	var searchBox;
	var city;
</script>
<script src="{{ theme_asset('khana/public/js/checkout/map.js') }}"></script>   
@endif


@endpush

