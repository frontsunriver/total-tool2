@extends('layouts.backend.app')

@section('content')
<!-- Main Content -->
<section class="section">
	<div class="section-header">
		<h1>{{ __('Select Payment Gateway') }}</h1>
		<div class="section-header-breadcrumb">
			<div class="breadcrumb-item active"><a href="{{ route('store.dashboard') }}">{{ __('Dashboard') }}</a></div>
			<div class="breadcrumb-item">{{ __('Select Payment Gateway') }}</div>
		</div>
	</div>
	<div class="section-body">
		<h2 class="section-title">{{ __('Select Payment gateway') }}</h2>
		<p class="section-lead">{{ __('Select Payment gateway for upgrade your plan.') }}</p>
		<div class="row">
			<div class="col-12">
				<div class="card">
					<div class="card-header">
						<h4>{{ __('Select Payment Gateway') }}</h4>
					</div>
					<div class="card-body">
						<div class="row mt-4">
							<div class="col-12 col-lg-8 offset-lg-2">
								<div class="wizard-steps">
									<ul class="nav nav-tabs">
										@if($credentials->paypal_status=='enabled')
										<li class="mb-5">
											<a href="#paypal" data-toggle="tab" class="active">
												<div class="wizard-step">
													<div class="wizard-step-icon">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/paypal.png') }}" alt="">
													</div>
													<div class="wizard-step-label">
														{{ __('Paypal') }}
													</div>
												</div>
											</a>
										</li>
										@endif

										@if($credentials->toyyibpay_status=='enabled')
										<li class="mb-5">
											<a href="#toyyibpay" data-toggle="tab">
												<div class="wizard-step">
													<div class="wizard-step-icon">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/toyyibpay.png') }}" alt="">
													</div>
													<div class="wizard-step-label">
														{{ __('Toyyibpay') }}
													</div>
												</div>
											</a>
										</li>
										@endif

										@if($credentials->razorpay_status=='enabled')
										<li class="mb-5">
											<a href="#razorpay" data-toggle="tab">
												<div class="wizard-step">
													<div class="wizard-step-icon">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/razorpay-logo.svg') }}" alt="">
													</div>
													<div class="wizard-step-label">
														{{ __('Razorpay') }}
													</div>
												</div>
											</a>
										</li>
										@endif
										@if(env('STRIPE_KEY') != null)
										<li class="mb-5">
											<a href="#stripe" data-toggle="tab">
												<div class="wizard-step">
													<div class="wizard-step-icon">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/stripe.png') }}" alt="">
													</div>
													<div class="wizard-step-label">
														{{ __('Stripe') }}
													</div>
												</div>
											</a>
										</li>
										@endif

										@if($credentials->instamojo_status=='enabled')
										<li class="mb-5">
											<a href="#instamojo" data-toggle="tab">
												<div class="wizard-step">
													<div class="wizard-step-icon">
														<img class="img-fluid" src="{{ theme_asset('khana/public/img/logo_instamojo.webp') }}" alt="">
														
													</div>
													<div class="wizard-step-label">
														{{ __('Instamojo') }}
													</div>
												</div>
											</a>
										</li>
										@endif
									</ul>
								</div>
							</div>
						</div>
						<div class="tab-content">
							@if($credentials->paypal_status=='enabled')
							<div class="wizard-content tab-pane fade active show" id="paypal">
								
								<div class="wizard-pane ">

									<div class="container">
										<div class="row">
											<div class="col-sm-3">	
											</div>
										<div class="col-sm-6">	
											<form action="{{ url('store/create-payment') }}" method="POST">
												@csrf

												<input type="hidden" name="id" value="{{ $plan->id }}">

												<input type="hidden" name="type" value="paypal">
												<label>{{ __('Phone Number') }}</label>
												<input type="number" name="phone" class="form-control">
												<button type="submit" class="btn btn-primary col-12 mt-4 btn-lg">{{ __('Send Payment With Paypal') }}</button>
											</form>
										</div>
									</div>
									</div>
								</div>
							</div>
							@endif

							@if($credentials->toyyibpay_status=='enabled')
							<div class="wizard-content tab-pane fade" id="toyyibpay">
								
								<div class="wizard-pane ">

									<div class="container">
										<div class="row">
											<div class="col-sm-3">	
											</div>
										<div class="col-sm-6">	
											<form action="{{ url('store/create-payment') }}" method="POST">
												@csrf

												<input type="hidden" name="id" value="{{ $plan->id }}">

												<input type="hidden" name="type" value="toyyibpay">
												<label>{{ __('Phone Number') }}</label>
												<input type="number" name="phone" class="form-control">
												<button type="submit" class="btn btn-primary col-12 mt-4 btn-lg">{{ __('Send Payment With Toyyibpay') }}</button>
											</form>
										</div>
									</div>
									</div>
								</div>
							</div>
							@endif


							@if($credentials->razorpay_status=='enabled')
							<div class="wizard-content tab-pane fade" id="razorpay">
								<div class="wizard-pane ">
									<div class="container">
										<div class="row">
											<div class="col-sm-3">	
											</div>
										<div class="col-sm-6">	
											<form action="{{ url('store/create-payment') }}" method="POST">
												@csrf
												<input type="hidden" name="id" value="{{ $plan->id }}">
												<input type="hidden" name="type" value="razorpay">
												<label>{{ __('Phone Number') }}</label>
												<input type="number" name="phone" class="form-control">
												<button type="submit" class="btn btn-primary col-12 mt-4 btn-lg">{{ __('Send Payment With Razorpay') }}</button>
											</form>
										</div>
									</div>
									</div>
								</div>
							</div>
							@endif
							@if($credentials->instamojo_status=='enabled')
							<div class="wizard-content tab-pane fade" id="instamojo">
								<div class="wizard-pane ">
									<div class="container">
										<div class="row">
											<div class="col-sm-3">	
											</div>
										<div class="col-sm-6">	
											<form action="{{ url('store/create-payment') }}" method="POST">
												@csrf
												<input type="hidden" name="id" value="{{ $plan->id }}">
												<input type="hidden" name="type" value="instamojo">
												<label>{{ __('Phone Number') }}</label>
												<input type="number" name="phone" class="form-control">
												<button type="submit" class="btn btn-primary col-12 mt-4 btn-lg">{{ __('Send Payment With Instamojo') }}</button>
											</form>
										</div>
									</div>
									</div>
								</div>
							</div>
							@endif
							@if(env('STRIPE_KEY') != null)
							<div class="wizard-content mt-2 tab-pane fade" id="stripe">
								<div class="wizard-pane">
									<div class="d-flex justify-content-center pb-5">
										<script src="https://js.stripe.com/v3/"></script>
										<form action="{{ url('store/create-payment') }}" method="POST" id="payment-form">
											@csrf
											
											<label for="card-element">
												{{ __('Credit or debit card') }}
											</label>
											<div id="card-element">
												<!-- A Stripe Element will be inserted here. -->
											</div>
											<input type="hidden" name="id" value="{{ $plan->id }}">
											<input type="hidden" name="type" value="stripe">
											<!-- Used to display form errors. -->
											<div id="card-errors" role="alert"></div>
											<button type="submit" class="btn btn-primary col-12 mt-4 btn-lg">{{ __('Send Payment') }}</button>
										</form>
									</div>	
								</div>
							</div>
							@endif
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</section>
@php 
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp
<input type="hidden" id="base_url" value="{{ url('/') }}">
<input type="hidden" id="plan_id" value="{{ $plan->id }}">
<input type="hidden" id="_token" value="{{ csrf_token() }}">
<input type="hidden" id="currency" value="{{ strtoupper($currency->value) }}">
<input type="hidden" id="stripe_api_key" value="{{ env('STRIPE_KEY') }}">
<input type="hidden" id="paypal_api_key" value="{{ env('PAYPAL_CLIENT_ID') }}">
@endsection
@if(env('STRIPE_KEY') != null)
@section('script')
<script src="{{ theme_asset('khana/public/js/plan/payment.js') }}"></script>
@endsection
@endif