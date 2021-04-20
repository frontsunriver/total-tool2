@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-6">
				<h4>{{ __('Featured Seller') }}</h4>
				<div class="cart-filter mb-20">
					<a href="?st=1&type=seller&manage=featured_seller">{{ __('Featured Seller') }}</a>
				</div>
			</div>
		</div>
		<div class="cart-filter mb-20">
			<h5>{{ __('Membership') }}</h5>
			<a href="?st=1&type=seller">{{ __('All') }}</a>|
			@foreach($plans as $row)
			<a href="?st=1&type={{ $type }}&plan={{ $row->id }}">{{ $row->name }}</a> |
			@endforeach
		</div>
		<div class="card-action-filter">
			<form method="post" id="basicform" action="{{ route('admin.featured.store') }}">
				@csrf
				<div class="row">
					<div class="col-lg-6">
						<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control" name="status">
										<option value="featured_seller">{{ __('Featured Seller') }}</option>
									</select>
								</div>
							</div>
							<div class="single-filter">
								<button type="submit" class="btn btn-primary mt-1 ml-1">{{ __('Apply for featured') }}</button>
							</div>
						</div>
					</div>
					<div class="col-lg-6">
						<div class="single-filter f-right">
							<div class="form-group">
								<input type="text" id="data_search" class="form-control" placeholder="Enter Value">
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="table-responsive custom-table">
				<table class="table">
					<thead>
						<tr>
							<th class="am-select">
								<div class="custom-control custom-checkbox">
									<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
									<label class="custom-control-label checkAll" for="customCheck12"></label>
								</div>
							</th>
							<th class="am-title"><i class="far fa-image"></i></th>
							<th class="am-title">{{ __('Store Name') }}</th>
							<th class="am-title">{{ __('Email') }}</th>
							<th class="am-title">{{ __('Membership Status') }}</td>
							<th class="am-date">{{ __('Registered At') }}</th>
						</tr>
					</thead>
					<tbody>
						@foreach($posts as $post)
						<tr>
							<th>
								<div class="custom-control custom-checkbox">
									<input type="checkbox" name="ids[]" class="custom-control-input" id="customCheck{{ $post->id }}" value="{{ $post->id }}">
									<label class="custom-control-label" for="customCheck{{ $post->id }}"></label>
								</div>
							</th>
							<td>
								<img src="{{ asset($post->avatar) }}" height="50">
							</td>
							<td>
								{{ $post->name }}
								<div class="hover">
									
									<a href="{{ url('admin/user',$post->id) }}" class="last">{{ __('View') }}</a>
								</div>
							</td>
							<td>
								{{ $post->email }}
							</td>
							<td>
								{{ $post->plan->name }}
							</td>					
													
							
							<td>{{ __('Last Modified') }}
								<div class="date">
									{{ $post->updated_at->diffForHumans() }}
								</div>
							</td>
						</tr>
						@endforeach
					</tbody>
				</form>
				<tfoot>
					<tr>
						<th class="am-select">
							<div class="custom-control custom-checkbox">
								<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
								<label class="custom-control-label checkAll" for="customCheck12"></label>
							</div>
						</th>
						<th class="am-title"><i class="far fa-image"></i></th>
						<th class="am-title">{{ __('Store Name') }}</th>
						<th class="am-title">{{ __('Email') }}</th>
						<th class="am-title">{{ __('Membership Status') }}</td>
						<th class="am-date">{{ __('Registered At') }}</th>
					</tr>
				</tfoot>
			</table>
			{{ $posts->links() }}

		</div>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//response will assign this function
	function success(res){
		//location.reload();
	}
	
</script>
@endsection