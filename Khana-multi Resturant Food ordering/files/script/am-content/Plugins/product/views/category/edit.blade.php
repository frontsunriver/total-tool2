@section('style')

@endsection
@extends('layouts.backend.app')

@section('content')

<div class="row">
	<div class="col-md-12">
		<div class="card">
			<div class="card-body">
				<h4 class="mb-20">{{ __('Edit Category') }}</h4>
				<div class="row">
					<div class="col-lg-8">
						
						<form method="post" id="basicform" class="custom-form" action="{{ route('admin.shop.category.update',$info->id) }}">
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
										<?php echo ConfigCategory($info->type,$info->p_id) ?>
										
									</select>
								</div>

								<button class="btn btn-primary col-12 mt-15">{{ __('Update') }}</button>
							</div>

						</div>
						@if($info->type==2)
						<div class="col-lg-4">
							<?php
						
							if(!empty($info->avatar)){

								$media['preview'] = $info->avatar;
								$media['value'] = $info->avatar;
								echo  mediasection($media);
								
							}
							else{
								echo mediasection();
							}

							?>
						</div>
						@endif
					</form>
				</div>
			</div>
		</div>
	</div>
</div>
{{ mediasingle() }}
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="{{ asset('admin/js/media.js') }}"></script>
<script>
	"use strict";
	(function ($) {

		$('.use').on('click',function(){

			$('#preview').attr('src',myradiovalue);
			$('#preview_input').val(myradiovalue);

		});

	})(jQuery);	
	
	function errosresponse(xhr){
		$("#errors").html("<li class='text-danger'>"+xhr.responseJSON[0]+"</li>")
	}
</script>
@endsection