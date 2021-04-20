@extends('layouts.backend.app')
@section('content')
<div class="row">
	
	<div class="col-sm-6">
		<div class="card">
			<div class="card-header ">

				<div class="text-center">
					<a href="{{ route('store.payout.edit') }}" class="text-left"><img src="{{ asset('uploads/paypal.png') }}" height="65"></a>
					<a href="{{ route('store.payout.edit') }}" class="text-right ml-5"><img src="{{ asset('uploads/bank.png') }}" height="40"></a>
				</div>
			</div>
			<div class="card-body text-left">
				
				<a href="{{ route('store.payout.edit') }}">{{ __('Edit Payout Information') }}</a>
			</div>
		</div>
	</div>
	<div class="col-sm-6">
		<div class="card">
			<div class="card-header ">

				<div class="text-center">
					<a href="#" data-toggle="modal" data-target="#exampleModal"><img src="{{ asset('uploads/cash.png') }}" height="65"></a>
					
				</div>
				<h2 class="text-right ml-5"><strong>${{ $total }}</strong></h2>
			</div>
			<div class="card-body text-left">
				
				<a href="#" data-toggle="modal" data-target="#exampleModal">{{ __('Withdraw') }}</a>
			</div>
		</div>
	</div>
	<div class="col-sm-12">
		<div class="card card-primary">
			<div class="card-header">
				<h4>{{ __('Payout History') }}</h4>
				
			</div>
			<div class="card-body">
				<div class="table-responsive">
					<table class="table">
						<tr>
							<th>{{ __('Transaction ID') }}</th>
							<th>{{ __('Amount') }}</th>
							<th>{{ __('Payout Method') }}</th>
							<th>{{ __('Date Processed') }}</th>
							<th>{{ __('Payout Status') }}</th>
							
						</tr>

						@foreach($payouts as $row)
						<tr>
							<td>#{{ $row->id }}</td>
							<td>{{ $row->amount }}</td>
							<td>{{ strtoupper($row->payment_mode) }}</td>
							<td>{{ $row->created_at->format('Y-F-d') }}</td>
							<td>@if($row->status==0)
								<span class="badge badge-danger">{{ __('Canceled') }}</span>
								@elseif($row->status==1)
								<span class="badge badge-success">{{ __('Completed') }}</span> @elseif($row->status==2)
								<span class="badge badge-primary">
									{{ __('Processing') }}

								@endif</td>
							</tr>
							@endforeach
						</table>
						{{ $payouts->links() }}
					</div>
				</div>
			</div>
		</div>

	</div>

	@endsection
	@if($total > 0)
	@section('extra')
	<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="exampleModalLabel">{{ __('Withdraw') }}</h5>
					<button type="button" class="close" data-dismiss="modal" aria-label="Close">
						<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<form method="post" action="{{ route('store.withdraw') }}" id="basicform">
					@csrf
					<input type="hidden" name="tk" value="{{ $total }}">
					<div class="modal-body">
						<div class="form-group">
							<label>{{ __('Amount') }}</label>
							<input type="number" name="amount" value="{{ $total }}" required class="form-control" disabled="">
						</div>
						<div class="form-group">
							<label>{{ __('Select Method') }}</label>
							<select class="form-control" name="method">
								<option value="paypal">Paypal</option>
								<option value="bank-transfer">Bank Transfer</option>
							</select>
						</div>
					</div>
					
					<div class="modal-footer">
						
						<button type="submit" class="btn btn-primary col-12">{{ __('Withdraw Now') }}</button>
					</div>

				</form>

			</div>
		</div>
	</div>

	@endsection
	@endif

	@section('script')
	<script src="{{ asset('admin/js/form.js') }}"></script>

	<script type="text/javascript">
		"use strict";
		function success(param) {
			window.location.reload();
		}
	</script>
	@endsection
