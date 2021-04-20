@section('style')

@endsection
@extends('layouts.backend.app')

@section('content')

<div class="card">
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-6">
				<h4>{{ __('Addon Product List') }}</h4>
			</div>
			<div class="col-lg-6">
				<div class="add-new-btn">
					<a href="{{ route('store.addon-product.create') }}" class="btn btn-primary f-right">{{ __('Add New') }}</a>
				</div>
			</div>
		</div>
		<div class="cart-filter mb-20">

			<a href="{{ route('store.addon-product.index') }}">{{ __('All') }} <span>({{ App\Terms::where('type',8)->where('auth_id',$auth_id)->where('status','!=',0)->count() }})</span></a>
			<a href="?st=1">{{ __('Published') }} <span>({{ App\Terms::where('type',8)->where('auth_id',$auth_id)->where('status',1)->count() }})</span></a>
			<a href="?st=2">{{ __('Drafts') }} <span>({{ App\Terms::where('type',8)->where('auth_id',$auth_id)->where('status',2)->count() }})</span></a>
			<a href="?st=trash" class="trash">{{ __('Trash') }} <span>({{ App\Terms::where('type',8)->where('auth_id',$auth_id)->where('status',0)->count() }})</span></a>
		</div>
		<div class="card-action-filter">
			<div class="row mb-10">
				<div class="col-lg-6">
					<form id="basicform" method="post" action="{{ route('store.products.destroy') }}">
						@csrf
						<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control" name="status">
										<option>{{ __('Bulk Actions') }}</option>
										<option value="publish">{{ __('Publish') }}</option>
										<option value="trash">{{ __('Move to Trash') }}</option>
										<option value="delete">{{ __('Delete Permanently') }}</option>
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
									<input type="checkbox" class="custom-control-input checkAll" id="checkAll">
									<label class="custom-control-label" for="checkAll"></label>
								</div>
							</th>
							
							<th class="am-title">{{ __('Title') }}</th>
							
							<th class="am-tags">{{ __('Price') }}</th>
							<th class="am-tags">{{ __('Sales') }}</th>
							<th class="am-tags">{{ __('Status') }}</th>

							<th class="am-date">{{ __('Last Modified') }}</th>
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
								{{ $post->title }}
								<div class="hover">
									<a class="last" href="{{ route('store.addon-product.edit',$post->id) }}">{{ __('Edit') }}</a>

									
								</div>
							</td>
							
							<td>{{ $post->price->price }}</td>
							<td>{{ $post->price->total_sales }}</td>
							
							
							
							
							<td>@if($post->status==1)  Published @elseif($post->status==2)  {{ __('Draft') }} @else {{ __('Trash') }} @endif</td>
							<td>{{ __('Last Modified') }}
								<div class="date">
									{{ $post->updated_at->diffForHumans() }}
								</div>
							</td>
						</tr>
						@endforeach

					</tbody>

					<tfoot>
						<tr>
							<th class="am-select">
								<div class="custom-control custom-checkbox">
									<input type="checkbox" class="custom-control-input checkAll" id="checkAll">
									<label class="custom-control-label" for="checkAll"></label>
								</div>
							</th>
							
							<th class="am-title">{{ __('Title') }}</th>
							
							<th class="am-tags">{{ __('Price') }}</th>
							<th class="am-tags">{{ __('Sales') }}</th>
							<th class="am-tags">{{ __('Status') }}</th>

							<th class="am-date">{{ __('Last Modified') }}</th>
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
	//success response will assign this function
 function success(res){
 	location.reload();
 }
 function errosresponse(xhr){

 	$("#errors").html("<li class='text-white'>"+xhr.responseJSON[0]+"</li>")
 }
</script>
@endsection