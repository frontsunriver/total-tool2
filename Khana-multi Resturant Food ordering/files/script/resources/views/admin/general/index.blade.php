@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<h4 class="mb-15">{{ __('General Settings') }}</h4>
		<div class="alert alert-danger none">
			<ul id="errors">

			</ul>
		</div>
		<div class="alert alert-success none">
			<ul id="success">

			</ul>
		</div>
		<form  method="POST" id="basicform" action="{{ route('admin.general.store') }}">
			@csrf
			<div class="custom-form">
				<div class="form-group">
					<label for="logo">{{ __('logo') }}</label><br>
					<img src="{{ asset($info->logo) }}" class="mt-1 img-fluid" id="preview"><button  data-toggle="modal" data-target=".media-single" class="btn bg-btn ml-2" id="logoChange" type="button">{{ __('Change') }}</button>
					<input type="text" id="logo" name="logo" class="form-control none" value="{{ $info->logo }}">
				</div>
				<div class="form-group">
					<label for="Favicon">{{ __('Favicon') }}</label><br>
					<img src="{{ asset($info->favicon) }}"  class="mt-1 img-fluid" id="Favpreview"><button  data-toggle="modal" data-target=".media-single" class="btn bg-btn ml-2" id="FavChange" type="button">{{ __('Change') }}</button>
					<input type="text" id="favicon" name="favicon" class="form-control none" value="{{ $info->favicon }}">
				</div>
				
				<div class="form-group">
					<label for="copyright">{{ __('Copyright') }}</label>
					<input type="text" name="copyright" id="copyright" class="form-control"  placeholder="Copyright" value="{{ $info->copyright }}"> 
				</div>
				<div class="form-group">
					<label for="contact_address">{{ __('Contact Address') }}</label>
					<input type="text" name="contact_address" id="contact_address" class="form-control"  placeholder="contact_address" value="{{ $info->contact_address }}"> 
				</div>
				<div class="form-group">
					<label for="contact_email">{{ __('Contact Email') }}</label>
					<input type="text" name="contact_email" id="contact_email" class="form-control"  placeholder="contact_email" value="{{ $info->contact_email }}"> 
				</div>
				<div class="form-group">
					<label for="contact_phone">{{ __('Contact Phone') }}</label>
					<input type="text" name="contact_phone" id="contact_phone" class="form-control"  placeholder="contact_phone" value="{{ $info->contact_phone }}"> 
				</div>
				<div class="form-group">
					<label >{{ __('tawk.to Property ID for live chat') }}</label>
					<input type="text" name="propertyid"  class="form-control"  placeholder="Property ID" value="{{ $info->propertyid }}" > 
				</div>
				
				<div class="form-group">
					<label >{{ __('Social Profile') }}</label>
					
					<input type="text" name="facebook" class="form-control"  placeholder="Facebook" value="{{ $info->facebook }}"> 
					<input type="text" name="instagram" class="form-control mt-1"  placeholder="instagram" value="{{ $info->instagram }}"> 
					<input type="text" name="twitter" class="form-control mt-1"  placeholder="twitter" value="{{ $info->twitter }}"> 
					<input type="text" name="googleplus" class="form-control mt-1"  placeholder="googleplus" value="{{ $info->googleplus }}"> 
					<input type="text" name="youtube" class="form-control mt-1"  placeholder="youtube" value="{{ $info->youtube }}">
					 <input type="text" name="linkedin" class="form-control mt-1"  placeholder="linkedin" value="{{ $info->linkedin ?? '' }}"> 
				</div>
				<div class="form-group">
					<button type="submit" class="btn col-12">{{ __('Update') }}</button>
				</div>
			</div>
		</form>
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
		let logo=false;
		let favicon=false;	

		$('#logoChange').on('click',function(){
			logo = true;
			favicon=false;
		});
		$('#FavChange').on('click',function(){
			logo = false;
			favicon=true;
		});

		$('.use').on('click',function(){
			
			if (logo==true) {
				$('#preview').attr('src',myradiovalue);
				$('#logo').val(myradiovalue);
			}
			if (favicon==true) {
				$('#Favpreview').attr('src',myradiovalue);
				$('#favicon').val(myradiovalue);
			}
			
		});
})(jQuery);	

//success response will assign this function
function success(res){
	$('.alert-danger').hide();
	Sweet('success','Settings Updated')
}
function errosresponse(xhr){
	$('.alert-success').hide();
	$('.alert-danger').show();
	
}	
</script>
@endsection