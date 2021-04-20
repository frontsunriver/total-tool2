@extends('layouts.backend.app')

@section('content')

<div class="row">
	<div class="col-md-12">
		<div class="card">
			<div class="card-body">
				<h4 class="mb-20">{{ __('Edit Category') }}</h4>
				<div class="row">
					<div class="col-lg-12">
						<div class="alert alert-danger none errorarea">
							<ul id="errors">

							</ul>
						</div>
						<form method="post" id="basicform" class="custom-form" action="{{ route('admin.category.update',$info->id) }}">
							@csrf
							@method('PUT')
							<div class="custom-form">
								<div class="form-group">
									<label for="name">{{ __('Category Name') }}</label>
									<input type="text" name="name" id="name" class="form-control input-rounded" placeholder="Enter Category Name" required value="{{ $info->name }}">


								</div>
								<div class="form-group">
									<label for="slug">{{ __('Slug') }}</label>
									<input type="text" name="slug" class="form-control" id="slug" value="{{ $info->slug }}">
								</div>
								<div class="form-group">
									<label for="p_id">{{ __('Parent Category') }}</label>
									<select class="custom-select mr-sm-2" name="p_id" id="p_id">
										<option value="">None</option>
										 <?php echo ConfigCategory(0,$info->p_id) ?>
										
									</select>
								</div>
								<button class="btn col-12 mt-15">{{ __('Update') }}</button>
							</div>
						</form>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
	"use strict";	
	function success(res){
		location.reload();
	}
	function errosresponse(xhr){
		$("#errors").html("<li class='text-danger'>"+xhr.responseJSON[0]+"</li>")
	}
</script>
@endsection