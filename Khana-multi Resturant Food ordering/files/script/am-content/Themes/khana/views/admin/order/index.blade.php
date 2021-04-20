@section('style')
<link rel="stylesheet" href="{{ asset('admin/assets/css/daterangepicker.css') }}">
@endsection
@extends('layouts.backend.app')
@section('content')

@include('layouts.backend.partials.headersection',['title'=>isset($type) ? ucfirst($type).' Orders' : 'Orders List'])
<div class="row">
	<div class="col-12 mt-2">
		<div class="card">
			<div class="card-body">
				
				<div class="float-right">
					<form action="{{ route('admin.order.index') }}">
						<div class="input-group mb-2 col-12">

							<input type="text" class="form-control" placeholder="Search..."  name="src" autocomplete="off" value="{{ $src ?? '' }}">
							<select class="form-control" name="type">
								<option value="id">{{ __('Search By Order Id') }}</option>
							</select>
							<div class="input-group-append">                                            
								<button class="btn btn-primary" type="submit"><i class="fas fa-search"></i></button>
							</div>
						</div>
					</form>
				</div>
				<div class="float-left">
					<form action="{{ route('admin.order.date.filter') }}" method="GET">
						<div class="d-flex">
							<div class="form-group">
								<div class="input-group">
									<div class="input-group-prepend">
										<div class="input-group-text">
											<i class="fas fa-calendar"></i>
										</div>
									</div>
									<input type="text" name="date" class="form-control daterange-cus">
									<input type="hidden" name="type" value="{{ isset($type) ? $type : '' }}">
								</div>
							</div>
							<div class="single-filter">
								<button type="submit" class="btn btn-primary btn-lg ml-2">{{ __('Filter') }}</button>
							</div>
						</div>
					</form>

				</div>
				<table class="table">
					<thead>
						<tr>
							<th class="am-title">{{ __('Order Id') }}</th>
							<th class="am-author">{{ __('Restaurant Name') }}</th>
							<th class="am-tags">{{ __('Order Type') }}</th>
							<th class="am-tags">{{ __('Payment Method') }}</th>
							<th class="am-tags">{{ __('Total Amount') }}</th>
							<th class="am-tags">{{ __('Status') }}</th>
							<th class="am-tags">{{ __('Payment Status') }}</th>
							<th class="am-date">{{ __('Action') }}</th>
						</tr>
					</thead>
					<tbody>
						@if($orders->count() > 0)
						@foreach($orders as $order)
						<tr>
							<td>#{{ $order->id }}</td>
							<td>{{ isset(App\User::find($order->vendor_id)->first()->name) ? App\User::find($order->vendor_id)->first()->name : '' }}</td>
							<td>@if($order->order_type == 1) {{ __('Home Delivery') }} @else {{ __('Pickup') }} @endif</td>
							<td>{{ strtoupper($order->payment_method) }}</td>
							<td>{{ number_format($order->total+$order->shipping,2) }}</td>
							<td>@if($order->status == 1) <span class="badge badge-success">{{ __('Completed') }}</span> @elseif($order->status == 2) <span class="badge badge-primary"> {{ __('Pending') }} </span> @elseif($order->status == 3) <span class="badge badge-warning"> {{ __('Accepted') }} </span> @elseif($order->status == 0)  <span class="badge badge-danger"> {{ __('Cancelled') }} </span> @endif</td>

							<td>@if($order->payment_status == 1) <span class="badge badge-success">{{ __('Completed') }}</span> @elseif($order->payment_status == 0)  <span class="badge badge-danger"> {{ __('Pending') }} </span> @endif</td>
							<td>
							<div class="btn-group">
                      <button class="btn btn-success dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                      Options
                      </button>
                      <div class="dropdown-menu" x-placement="top-start" >
                        <div class="dropdown-title">Order No #{{ $order->id }}</div>
                        <a class="dropdown-item" href="{{ route('admin.order.details',$order->id) }}">{{ __('View') }}</a>
                        <a class="dropdown-item cancel" href="{{ route('admin.order.delete',$order->id) }}">{{ __('Delete') }}</a>
                       
                      </div>
                    </div>
                    </td>
							
						</tr>
						@endforeach
						@else
						<tr>
							<td></td>
							<td></td>
							<td>{{ __('No Data Found') }}</td>
							<td></td>
							<td></td>
							<td></td>
						</tr>
						@endif
					</tbody>
					
				</table>
				{{ $orders->links() }}


			</div>
		</div>
	</div>
</div>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="{{ asset('admin/assets/js/moment.min.js') }}"></script>
<script src="{{ asset('admin/assets/js/daterangepicker.min.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//success response will assign this function
	function success(res){
		location.reload();
	}
	$('.daterange-cus').daterangepicker();
</script>
@endsection