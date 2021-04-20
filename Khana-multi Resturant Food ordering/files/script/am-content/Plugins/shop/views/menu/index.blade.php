@section('style')
@endsection
@extends('layouts.backend.app')

@section('content')
<div class="row" id="category_body">
	<div class="col-lg-4">      
		<div class="card">
			<div class="card-body">
				<form id="basicform" method="post" action="{{ route('store.menu.store') }}">
					@csrf
					<input type="hidden" name="type" value="{{ $type }}">
					<div class="custom-form">
						<div class="form-group">
							<label for="name">{{ __('Name') }}</label>
							<input type="text" name="name" class="form-control" id="name" placeholder="Menu Name">
						</div>
						
						<div class="form-group">
							<label for="p_id">{{ __('Parent Menu') }}</label>
							<select class="custom-select mr-sm-2" name="p_id" id="p_id inlineFormCustomSelect">
								<option value="">None</option>
								<?php echo ConfigCategory(1) ?>
								
							</select>
						</div>
						
						<div class="form-group mt-20">
							<button class="btn col-12 btn-primary" type="submit">{{ __('Add New Menu') }}</button>
						</div>
					</div>
				</form>
			</div>
		</div>
	</div>
	<div class="col-lg-8" >      
		<div class="card">
			<div class="card-body">
				@php
				if (!empty($req)) {
					$categeories=\App\Category::where('type',1)->where('user_id',Auth::id())->where('name','LIKE','%'.$req.'%' )->latest()->paginate(12);	
				}
				else{
					$categeories=\App\Category::where('type',1)->where('user_id',Auth::id())->latest()->paginate(12);
				}
				@endphp
				<div class="table-responsive">
					<div class="card-action-filter">
					
						<form id="basicform1" method="post" action="{{ route('store.menu.des') }}">
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
											<a href="{{ route('store.menu.edit',$category->id) }}">{{ __('Edit') }}</a>
										</div>
									</td>
									
									<td>{{ \App\PostCategory::where('category_id',$category->id)->count() }}</td>
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
								
								<th class="am-categories">{{ __('Count') }}</th>

							</tr>
						</tfoot>
					</table>
					<div class="f-right">{{ $categeories->links() }}</div>
				</div>
				
			</div>
		</div>
	</div>
</div>				


@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>

<script type="text/javascript">
	"use strict";

	function success(res){
		location.reload();
	}
</script>
@endsection