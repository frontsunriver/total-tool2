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
					<form action="{{ route('order.create') }}" method="POST" id="place_order_form">
						@csrf
						<div class="single-checkout mb-50">
							@if ($errors->any())
								<div class="alert alert-danger">
									<ul>
										@foreach ($errors->all() as $error)
											<li>{{ $error }}</li>
										@endforeach
									</ul>
								</div>
							@endif
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
									<input type="hidden" name="order_type" value="{{ $ordertype }}" >
									@if($ordertype == 1)
									<input type="hidden" name="latitude" id="latitude" value="{{ $resturent_info->resturentlocation->latitude }}">
									<input type="hidden" name="longitude" id="longitude" value="{{ $resturent_info->resturentlocation->longitude }}">



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
									<div class="col-lg-12">
										<div class="select-payment-area mt-50">
											<h3><span>2</span> {{ __('Select Payment Method') }}</h3>
											<div class="row justify-content-center select_payment">

												@if(env('STRIPE_KEY') != '')
												<div class="col-lg-3 payment_section">
													<label for="stripe" class="single-payment-section stripe text-center mb-30" onclick="select_payment('stripe')">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/stripe.png') }}" alt="">
													</label>
													<input type="radio" name="payment_method" value="stripe" id="stripe" class="d-none">
												</div>
												@endif
												@if ($credentials != null)
												@if($credentials->paypal_status == 'enabled')
												<div class="col-lg-3 payment_section">
													<label for="paypal" class="single-payment-section paypal text-center mb-30" onclick="select_payment('paypal')">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/paypal.png') }}" alt="">
													</label>
													<input id="paypal" type="radio" class="d-none" name="payment_method" value="paypal">
												</div>
												@endif

												@if($credentials->toyyibpay_status == 'enabled')
												<div class="col-lg-3 payment_section">
													<label for="toyyibpay" class="single-payment-section toyyibpay text-center mb-30" onclick="select_payment('toyyibpay')">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/toyyibpay.png') }}" alt="">
													</label>
													<input id="toyyibpay" type="radio" class="d-none" name="payment_method" value="toyyibpay">
												</div>
												@endif

												@if($credentials->razorpay_status == 'enabled')
												<div class="col-lg-3 payment_section">
													<label for="razorpay" class="single-payment-section razorpay text-center mb-30" onclick="select_payment('razorpay')">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/razorpay-logo.svg') }}" alt="">
													</label>
													<input id="razorpay" type="radio" class="d-none" name="payment_method" value="razorpay">
												</div>
												@endif

												@if($credentials->instamojo_status == 'enabled')
												<div class="col-lg-3 payment_section">
													<label for="instamojo" class="single-payment-section text-center instamojo" onclick="select_payment('instamojo')">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/logo_instamojo.webp') }}" alt="">
													</label>
													<input id="instamojo" type="radio" class="d-none" name="payment_method" value="instamojo">
												</div>
												@endif
												@endif
												<div class="col-lg-3 payment_section">
													<label for="cod" class="single-payment-section text-center cod" onclick="select_payment('cod')">
														<img class="img-fluid cod" src="{{ theme_asset('khana/public/img/cod.png') }}" alt="">
													</label>
													<input id="cod" type="radio" class="d-none" name="payment_method" value="cod">
												</div>


											</div>
										</div>
									</div>
									<input type="hidden" name="total_amount" id="total_amount" value="{{ number_format(str_replace(',', '', Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->total()),2) }}">
									<div class="col-lg-12">
										<div class="form-group">
											<div class="place-order mt-20">
												<button id="place_order_button">{{ __('Place Order') }}</button>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</form>
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
								<div class="single-product-another-info-show d-flex">
									<span class="product-another">{{ __('Tax fee') }}</span>
									<span class="product-price">{{ $currency->value }}: {{ Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->tax() }} <span id="delivery_fee"></span></span>
								</div>
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

<input type="hidden" id="stripe_api_key" value="{{ env('STRIPE_KEY') }}">
<input type="hidden" id="currency_value" value="{{ $currency->value }}">
@endsection
@push('js')
 <script>
	 $('#place_order_form').on('submit',function(){
		$('#place_order_button').attr('disabled','');
		$('#place_order_button').html('Please wait....');
	 });
	 	//coupon form submit
		 $('#couponform').on('submit',function(e){
    	e.preventDefault();
    	$.ajaxSetup({
    		headers: {
    			'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
    		}
    	});
    	$.ajax({
    		type: 'POST',
    		url: this.action,
    		data: new FormData(this),
    		dataType: 'json',
    		contentType: false,
    		cache: false,
    		processData:false,

    		success: function(response){ 
    			if(response.message)
    			{
    				$('#checkout_right').load(' #checkout_right');
    				$('.alert-message-area').fadeIn();
    				$('.ale').html(response.message);
    				$(".alert-message-area").delay( 2000 ).fadeOut( 2000 );
    				window.location.reload();
    			}

    			if(response.error)
    			{
    				$('.error-message-area').fadeIn();
    				$('.error-msg').html(response.error);
    				$(".error-message-area").delay( 2000 ).fadeOut( 2000 );
    			}

    		},
    		error: function(xhr, status, error) 
    		{
    			$('.errorarea').show();
    			$.each(xhr.responseJSON.errors, function (key, item) 
    			{
    				Sweet('error',item)
    				$("#errors").html("<li class='text-danger'>"+item+"</li>")
    			});
    			errosresponse(xhr, status, error);
    		}
    	})
    });

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

	function select_payment(type)
	{
		$('#payment_type').val(type);
	}
</script>
<script src="{{ theme_asset('khana/public/js/checkout/map.js') }}"></script>   
@endif

@endpush

