@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-6">
				<h4>{{ __('Current Featured Seller') }}</h4>
			</div>
		</div>
		<div class="card-action-filter">
			<form method="post" id="basicform" action="{{ route('admin.featured.update',1) }}">
				@csrf
				@method('PUT')
				<div class="row">
					<div class="col-lg-6">
						<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control" name="status">
										<option value="">Select Action</option>
										<option value="trash">{{ __('Remove From Featured') }}</option>
									</select>
								</div>
							</div>
							<div class="single-filter">
								<button type="submit" class="btn btn-primary mt-1 ml-1">{{ __('Apply') }}</button>
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
							
							<th class="am-date">{{ __('Featured At') }}</th>
						</tr>
					</thead>
					<tbody>
						@foreach($posts as $post)
						<tr>
							<th>
								<div class="custom-control custom-checkbox">
									<input type="checkbox" name="ids[]" class="custom-control-input" id="customCheck{{ $post->f_id }}" value="{{ $post->f_id }}">
									<label class="custom-control-label" for="customCheck{{ $post->f_id }}"></label>
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
							<td>{{ __('Last Modified') }}
								<div class="date">
									{{ $post->created_at->diffForHumans() }}
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
						
						<th class="am-date">{{ __('Featured At') }}</th>
					</tr>
				</tfoot>
			</table>
		</div>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
	"use strict";	
	//response will assign this function
	function success(res){
		location.reload();
	}
	
</script>
@endsection