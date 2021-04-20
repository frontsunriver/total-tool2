@extends('layouts.backend.app')

@section('content')
<form method="post" id="basicform" action="{{ route('admin.plan.payment.update',$info->id) }}">
	@csrf
	<div class="row">
		<div class="col-lg-9">      
			<div class="card">
				<div class="card-body">
					<h4>{{ __('Edit Payment') }}</h4>
					<h6>{{ __('Transaction Id') }} #{{ $info->id }}</h6>
					<div class="form-group">
						<label for="title">{{ __('Select Plan') }}<span class="text-danger"><b>*</b></span></label>
						<select name="plan_id" class="form-control">
							@foreach($plans as $plan)
							<option value="{{ $plan->id }}" @if($info->plan_id==$plan->id) selected="" @endif>{{ $plan->name }}</option>
							@endforeach
						</select>
					</div>
					<input type="hidden" name="user_id" id="user_id" required value="{{ $info->user_id }}">
					<div class="form-group">
						<label for="title">{{ __('Payment Status') }}</label>
						<select name="payment_status" class="form-control" >
							<option value="approved" @if($info->payment_status=='approved') selected="" @endif>Complete</option>
							<option value="pending" @if($info->payment_status=='pending') selected="" @endif>Pending</option>
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
							<option value="approved" @if($info->status=='approved') @endif>{{ __('Approved') }}</option>
							<option value="pending" @if($info->status=='pending') @endif>{{ __('Pending') }}</option>
						</select>
					</div>
				</div>
			</div>
		</div>
	</div>
</form>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@endsection