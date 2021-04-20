@extends('layouts.backend.app')

@section('content')
<div class="row">
	<div class="col-12 mt-2">
		<div class="card">
			<div class="card-body">
				<div class="float-right">
					<form>
						<div class="input-group mb-2 col-12">
							<input type="number" class="form-control" placeholder="Search..." required="" name="src" autocomplete="off" value="{{ $src ?? '' }}" >
							<select class="form-control" name="type">
								<option value="id">{{ __('Search Transection Id') }}</option>
								<option value="user_id">{{ __('Search By User Id') }}</option>
								
							</select>
							<div class="input-group-append">                                            
								<button class="btn btn-primary" type="submit"><i class="fas fa-search"></i></button>
							</div>
						</div>
					</form>
				</div>
				<div class="float-left">
					<div class="d-flex">
						<a href="{{ route('admin.plan.payment.create') }}" class="btn f-right btn btn-primary">{{ __('Create Payment') }}</a>
					</div>
				</div>
				<div class="table-responsive">
					<table class="table table-striped table-hover text-center table-borderless">
						<thead>
							<tr>
								<th class="am-title">{{ __('Id') }}</th>
								<th class="am-title">{{ __('User') }}</th>
								<th class="am-title">{{ __('Amount') }}</th>
								<th class="am-title">{{ __('Payment Method') }}</th>
								<th class="am-date">{{ __('Payment Status') }}</th>
								<th class="am-date">{{ __('Status') }}</th>
								<th class="am-date">{{ __('Payment Id') }}</th>
								<th class="am-date">{{ __('Subscribed At') }}</th>
								<th class="am-date">{{ __('Action') }}</th>
							</tr>
						</thead>
						<tbody>
							@foreach($plans as $key=>$plan)
							<tr>
								<td>{{ $plan->id }}</td>
								<td><a href="{{ url('/admin/user',$plan->user_id) }}">{{ $plan->user->name }}</a></td>
								<td>{{ $plan->amount }}</td>
								<td>{{ $plan->payment_method }}</td>
								<td>
									@if($plan->payment_status == 'pending')
									<div class="badge badge-danger">{{ __('pending') }}</div>
									@else
									<div class="badge badge-success">{{ __('Approved') }}</div>
									@endif
								</td>
								<td>{{ $plan->status }}</td>
								<td>@if($plan->image != 'default.png') {{ $plan->image }} @endif</td>
								<td>{{ $plan->created_at->diffforhumans() }}</td>
								<td>
									<div class="dropdown d-inline">
										<button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
											Action
										</button>
										<div class="dropdown-menu">
											<a class="dropdown-item has-icon" href="{{ route('admin.plan.payment.approved',encrypt($plan->id)) }}"><i class="fa fa-eye"></i> View</a>
											<a class="dropdown-item has-icon cancel" href="{{ route('admin.plan.payment.delete',encrypt($plan->id)) }}"><i class="fa fa-trash"></i> Delete</a>
											
										</div>
									</div>
									
								</td>
							</tr>
							@endforeach
						</tbody>
						<tfoot>
							<tr>
								<th class="am-title">{{ __('Id') }}</th>
								<th class="am-title">{{ __('User') }}</th>
								<th class="am-title">{{ __('Amount') }}</th>
								<th class="am-title">{{ __('Payment Method') }}</th>
								<th class="am-date">{{ __('Payment Status') }}</th>
								<th class="am-date">{{ __('Payment Id') }}</th>
								<th class="am-date">{{ __('Status') }}</th>
								<th class="am-date">{{ __('Subscribed At') }}</th>
								<th class="am-date">{{ __('Action') }}</th>
							</tr>
						</tfoot>
					</table>
					{{ $plans->links() }}
				</div>
			</div>
		</div>
	</div>
</div>
@endsection

@section('script')

@endsection