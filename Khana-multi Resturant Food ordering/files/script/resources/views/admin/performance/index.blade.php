@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="alert alert-danger none">
			<ul id="errors">

			</ul>
		</div>
		<form method="post" id="basicform" action="{{ route('admin.performance.store') }}">
			@csrf
			<div class="custom-form">
				<div class="row">
				
					<div class="form-group col-md-12">
						<label for="lazy">{{ __('Lazy loading for images') }}</label>

						<select class="custom-select mr-sm-2" id="lazy" name="lazy">
							<option value="1" @if($info->lazyload==1) selected="" @endif>{{ __('Enable') }}</option>
							<option value="0" @if($info->lazyload==0) selected="" @endif>{{ __('Disable') }}</option>
						</select>
						<small>{{ __('Enable this option to optimize your images loading on the website. They will be loaded only when user will scroll the page.') }}</small>
					</div>
					<div class="form-group col-md-12">
						 <a href="#" data-toggle="modal" data-target=".media-single" class="single-modal">
						<label>{{ __('Upload custom placeholder image') }}</label><br>
						
						@php
						if (!empty($info->image)) {
							$img=$info->image;
						}
						else{
							$img='admin/img/img/placeholder.png';
						}
						
						@endphp
					<img class="img-thumbnail h-100" id="preview"  src="{{ asset($img) }}" alt=""></a>
							

							<br>
							<small>{{ __('Add your custom image placeholder that will be used before the original image will be loaded') }}.</small>


						</div>
						<div class="form-group col-lg-12">
							<button type="submit" class="btn col-12">{{ __('Update') }}</button>
						</div>
					</div>
					<input type="hidden" name="image" id="image">
			</form>
		</div>
	</div>
</div>
@include('admin.media.mediamodal')
@endsection

@section('script')	
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="{{ asset('admin/js/media.js') }}"></script>
<script>	
(function ($) {
	"use strict";
	
	$('.use').on('click',function(){
		$('#preview').attr('src',myradiovalue);
		$('#image').val(myradiovalue);
	});
	
})(jQuery);	
</script>
@endsection
