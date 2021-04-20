@extends('layouts.backend.app')

@section('content')
<form method="post" id="basicform" action="{{ route('admin.plan.payment.create') }}">
	@csrf
	<div class="row">
		<div class="col-lg-9">      
			<div class="card">
				<div class="card-body">
					<h4>{{ __('Add new Payment') }}</h4>
					<div class="form-group">
						<label for="title">{{ __('Select Plan') }}<span class="text-danger"><b>*</b></span></label>
						<select name="plan_id" class="form-control">
							@foreach($plans as $plan)
							<option value="{{ $plan->id }}">{{ $plan->name }}</option>
							@endforeach
						</select>
					</div>
					<input type="hidden" name="user_id" id="user_id" required>
					<div class="form-group">
						<label>{{ __('User Email') }}</label>
						<input type="email" name="email" class="form-control" id="email" placeholder="Enter Restaurant User Email" required>
					</div>
					
					<div class="form-group">
						<label for="title">{{ __('Payment Status') }}</label>
						<select name="payment_status" class="form-control" >
							<option value="approved">{{ __('Complete') }}</option>
							<option value="pending">{{ __('Pending') }}</option>
						</select>
					</div>
				</div>
			</div>
		</div>
		<div class="col-lg-3">
			<div class="single-area">
				<div class="card">
					<div class="card-body">
						<h5>{{ __('Publish') }}</h5>
						<hr>
						<div class="btn-publish">
							<button type="submit" class="btn btn-primary col-12"><i class="fa fa-save"></i> {{ __('Save') }}</button>
						</div>
					</div>
				</div>
			</div>
			<div class="single-area">
				<div class="card sub">
					<div class="card-body">
						<h5>{{ __('Status') }}</h5>
						<hr>
						<select class="custom-select mr-sm-2" id="inlineFormCustomSelect" name="status">
							<option value="approved">{{ __('Approved') }}</option>
							<option value="pending">{{ __('Pending') }}</option>
						</select>
					</div>
				</div>
			</div>
		</div>
	</div>
</form>

<form id="basicform1" method="post" action="{{ route('admin.plan.user') }}">
	@csrf
	<input type="hidden" name="email" id="param">
</form>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";
	$('#email').on('focusout',function() {
		var email = $('#email').val();
		$('#param').val(email);
		$('#basicform1').submit();
	})

	function success(param) {
		$('#user_id').val(param.data);
	}
</script>
@endsection