@section('style')
@endsection
@extends('layouts.backend.app')

@section('content')
<div class="row" id="category_body">
	@if($type==2)
	<div class="col-lg-4">      
		<div class="card">
			<div class="card-body">
				<form id="basicform" method="post" action="{{ route('admin.shop.category.store') }}">
					@csrf
					<input type="hidden" name="type" value="{{ $type }}">
					<div class="custom-form">
						<div class="form-group">
							<label for="name">{{ __('Name') }}</label>
							<input type="text" name="name" class="form-control" id="name" placeholder="Category Name">
						</div>
						<div class="form-group">
							<label for="slug">{{ __('Slug') }}</label>
							<input type="text" name="slug" class="form-control" id="slug" placeholder="Category slug">
						</div>
						<div class="form-group">
							<label for="p_id">{{ __('Parent Category') }}</label>
							<select class="custom-select mr-sm-2" name="p_id" id="p_id inlineFormCustomSelect">
								<option value="">None</option>
								<?php echo ConfigCategory($type) ?>
								
							</select>
						</div>
						@if($type==2)
						{{ mediasection() }}
						@endif
						<div class="form-group mt-20">
							<button class="btn col-12 btn-primary" type="submit">{{ __('Add New Category') }}</button>
						</div>
					</div>
				</form>
			</div>
		</div>
	</div>
	@endif
	@if($type==2)
	<div class="col-lg-8" >
	@else
	<div class="col-lg-12" >
	@endif
	      
		<div class="card">
			<div class="card-body">
				@php
				if (!empty($req)) {
					$categeories=\App\Category::where('type',$type)->where('name','LIKE','%'.$req.'%' )->whereHas('user')->with('user')->latest()->paginate(12);	
				}
				else{
					$categeories=\App\Category::where('type',$type)->whereHas('user')->with('user')->latest()->paginate(12);
				}
				@endphp
				<div class="table-responsive">
					<div class="card-action-filter">
						
						<form id="basicform1" method="post" action="{{ route('admin.shop.productcategory') }}">
							@csrf
							
								<div class="row ml-1 mt-1">
									
									<div class="form-group">
										<select class="form-control" name="method">
											<option>Select Action</option>
											<option value="delete">{{ __('Delete Permanently') }}</option>
										</select>
									</div>
								
								<div class="single-filter">
									<button type="submit" class="btn btn-danger ml-1 mt-1">{{ __('Apply') }}</button>
								</div>
							</div>
						</div>
						<table class="table category">
							<thead>
								<tr>
									<th class="am-select">
										<div class="custom-control custom-checkbox">
											<input type="checkbox" class="custom-control-input checkAll" id="checkAll">
											<label class="custom-control-label" for="checkAll"></label>
										</div>
									</th>
									<th class="am-title">{{ __('Title') }}</th>
									
									<th class="am-title">{{ __('Created By') }}</th>

									
									
									<th class="am-title">{{ __('count') }}</th>

								</tr>
							</thead>
							<tbody>
								@foreach($categeories as $category)
								<tr>
									<th>
										<div class="custom-control custom-checkbox">
											<input type="checkbox" name="ids[]" class="custom-control-input" id="customCheck{{ $category->id }}" value="{{ $category->id }}">
											<label class="custom-control-label" for="customCheck{{ $category->id }}"></label>
										</div>
									</th>
									<td>
										{{ $category->name }}
										<div class="hover">
											<a href="{{ route('admin.shop.category.edit',$category->id) }}">{{ __('Edit') }}</a>
										</div>
									</td>
									<td><a href="{{ url('/admin/user',$category->user->id) }}">{{ $category->user->name }}</a></td>
									
									@if($category->type==1)
									<td>{{ \App\PostCategory::where('category_id',$category->id)->count() }}</td>
									@else
									<td>{{ \App\Usercategory::where('category_id',$category->id)->count() }}</td>
									@endif
								</tr>
								@endforeach

							</tbody>
						</form>	
						<tfoot>
							<tr>
								<th class="am-select">
									<div class="custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input checkAll" id="checkAll">
										<label class="custom-control-label" for="checkAll"></label>
									</div>
								</th>
								<th class="am-title">{{ __('Title') }}</th>
								
									<th class="am-title">{{ __('Created By') }}</th>

								
								
								<th class="am-categories">{{ __('Count') }}</th>

							</tr>
						</tfoot>
					</table>
					<div class="f-right">{{ $categeories->appends($request->all())->links() }}</div>
				</div>
				
			</div>
		</div>
	</div>
</div>				
@if($type==2)
{{ mediasingle() }}
@endif
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@if($type==2)
<script src="{{ asset('admin/js/media.js') }}"></script>
@endif
<script type="text/javascript">
	"use strict";
	(function ($) {

		$('.use').on('click',function(){

			$('#preview').attr('src',myradiovalue);
			$('#preview_input').val(myradiovalue);

		});

	})(jQuery);
	function success(res){
		location.reload();
	}
</script>
@endsection