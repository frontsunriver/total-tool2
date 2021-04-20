@section('style')

@endsection
@extends('layouts.backend.app')

@section('content')

<div class="row">
	<div class="col-md-12">
		<div class="card">
			<div class="card-body">
				<h4 class="mb-20">{{ __('Edit Menu') }}</h4>
				<div class="row">
					<div class="col-lg-12">
						
						<form method="post" id="basicform" class="custom-form" action="{{ route('store.menu.update',$info->id) }}">
							@csrf
							@method('PUT')
							<div class="custom-form">
								<div class="form-group">
									<label for="name">{{ __('Menu Name') }}</label>
									<input type="text" name="name" id="name" class="form-control input-rounded" placeholder="Enter Menu Name" required value="{{ $info->name }}">


								</div>
								
								<div class="form-group">
									<label for="p_id">{{ __('Parent Menu') }}</label>
									<select class="custom-select mr-sm-2" name="p_id" id="p_id">
										<option value="">None</option>
										<?php echo ConfigCategory(1,$info->p_id) ?>
										
									</select>
								</div>

								<button class="btn btn-primary col-12 mt-15">{{ __('Update') }}</button>
							</div>
						</div>
					</form>
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
	function errosresponse(xhr){
		$("#errors").html("<li class='text-danger'>"+xhr.responseJSON[0]+"</li>")
	}
</script>
@endsection