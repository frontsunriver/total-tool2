@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'Payout account'])
<div class="row">
	<div class="col-sm-6">
		<div class="card">
			<div class="card-header">
				<h5>{{ __('Paypal') }}</h5>

			</div>
			<div class="card-body">
				<p>{{ __('Get paid by credit or debit card, PayPal transfer or even via bank account in just a few clicks. All you need is your email address') }}</p>
				@if(empty($paypal))
				<form  method="post" action="{{ route('rider.payout.paypal') }}" id="basicform">
					@endif	
					@csrf
					<input type="hidden" name="paypal" value="0">
					<div class="form-group">
						<label>{{ __('Email') }}</label>
						<input type="email" name="email" class="form-control" required placeholder="Enter Paypal Email Address" value="{{ $paypal->content ?? '' }}" @if(!empty($paypal)) disabled=""  @endif>
					</div>
					@if(empty($paypal))
					<div class="form-group">
						<label>{{ __('Confirm email address') }}</label>
						<input type="email" name="confirm" class="form-control" required placeholder="Confirm email address">
					</div>
					@endif

				</div>
				@if(empty($paypal))
				<div class="card-footer mt-5">
					<small>{{ __('Note:') }} <span class="text-danger">{{ __('Once You Submit Your Payout Information You Cannot Edit It.') }}</span></small>
					<button class="btn btn-primary col-12" type="submit">{{ __('Set Payout Account') }}</button>
				</div>
				
			</form>
			@endif
		</div>
	</div>
	<div class="col-sm-6">
		<div class="card">
			<div class="card-header">
				<h5>{{ __('Bank Details') }}</h5>
			</div>
			<div class="card-body">
				@if(empty($bank))
				<form  method="post" action="{{ route('rider.payout.bank') }}" id="basicform1">
					@csrf
				@endif	
					<input type="hidden" name="bank" value="0">
					<div class="form-group">
						<label>{{ __('Bank Name') }}</label>
						<input type="text" name="bank_name" class="form-control" required placeholder="Enter Bank Name"  value="{{ $bank->bank_name ?? "" }}" @if(!empty($bank)) disabled @endif>
					</div>
					<div class="form-group">
						<label>{{ __('Bank Branch Name') }}</label>
						<input type="text" name="branch_name" class="form-control" required placeholder="Enter Bank Branch Name" value="{{ $bank->branch_name ?? "" }}" @if(!empty($bank)) disabled @endif>
					</div>

					<div class="form-group">
						<label>{{ __('Account Holder Name') }}</label>
						<input type="text" name="holder_name" class="form-control" required placeholder="Enter Name" value="{{ $bank->holder_name ?? "" }}" @if(!empty($bank)) disabled @endif>
					</div>
					<div class="form-group">
						<label>{{ __('Account Number') }}</label>
						<input type="text" name="account_number" class="form-control" required placeholder="Enter Number" value="{{ $bank->account_number ?? "" }}" @if(!empty($bank)) disabled @endif>
					</div>
					
					
				</div>
				@if(empty($bank))
				<div class="card-footer">
					<small>{{ __('Note:') }} <span class="text-danger">{{ __('Once You Submit Your Payout Information You Cannot Edit It.') }}</span></small>
					<button class="btn btn-primary col-12" type="submit">{{ __('Set Payout Account') }}</button>
				</div>
			</form>
			@endif
		</div>
	</div>
</div>

@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>

<script type="text/javascript">
	"use strict";
	function success(param) {
		window.location.reload();
	}
</script>
@endsection
