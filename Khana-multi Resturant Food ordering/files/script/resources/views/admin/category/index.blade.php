@extends('layouts.backend.app')

@section('content')
<div class="row" id="category_body">
	<div class="col-lg-4">      
		<div class="card">
			<div class="card-body">
				<form id="basicform" method="post" action="{{ route('admin.category.store') }}">
					@csrf
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
								  <?php echo ConfigCategory(0) ?>
								
							</select>
						</div>
						<div class="form-group mt-20">
							<button class="btn btn-primary col-12" type="submit">{{ __('Add New Category') }}</button>
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
					$categeories=App\Category::where('name','LIKE','%'.$req.'%' )->latest()->paginate(12);	
				}
				else{
					$categeories=App\Category::latest()->paginate(12);
				}
				@endphp
				<div class="table-responsive">
					<div class="card-action-filter">
						<form>
						<input type="text" class="form-control" name="query" required>
						</form>
						<form id="basicform1" method="post" action="{{ route('admin.categorys.destroy') }}">
							@csrf
							<div class="card-filter-content">
								<div class="single-filter">
									<div class="form-group">
										<select class="form-control" name="method">
											<option value="delete">{{ __('Delete Permanently') }}</option>
										</select>
									</div>
								</div>
								<div class="single-filter">
									<button type="submit" class="btn">{{ __('Apply') }}</button>
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
									<th class="am-title">{{ __('Slug') }}</th>
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
											<a href="{{ route('admin.category.edit',$category->id) }}">{{ __('Edit') }}</a>
										</div>
									</td>
									<td>{{ $category->slug }}</td>
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
								<th class="am-author">{{ __('Slug') }}</th>
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