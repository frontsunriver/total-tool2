@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="alert alert-danger none">
			<ul id="errors">

			</ul>
		</div>
		<div class="alert alert-success none">
			<ul id="success">

			</ul>
		</div>
		<form id="basicform" method="post" action="{{ route('admin.filesystem.store') }}">
			@csrf
				<div class="form-group ">
						<label for="compress">{{ __('Auto Compress Lavel For Upload Image') }} </label>

						<select class="custom-select mr-sm-2" id="compress" name="compress">
							<option value="1" @if($info->compress==1) selected="" @endif>{{ __('10%') }}</option>
							<option value="2" @if($info->compress==2) selected="" @endif>{{ __('20%') }}</option>
							<option value="3" @if($info->compress==3) selected="" @endif>{{ __('30%') }}</option>
							<option value="4" @if($info->compress==4) selected="" @endif>{{ __('40%') }}</option>
							<option value="5" @if($info->compress==5) selected="" @endif>{{ __('50%') }}</option>
							<option value="6" @if($info->compress==6) selected="" @endif>{{ __('60%') }}</option>
							<option value="7" @if($info->compress==7) selected="" @endif>{{ __('70%') }}</option>
							<option value="8" @if($info->compress==8) selected="" @endif>{{ __('80%') }}</option>
							<option value="9" @if($info->compress==9) selected="" @endif>{{ __('90%') }}</option>

						</select>
						
					</div>
			<div class="custom-form">

				<div class="form-group">
					<label for="method">{{ __('Select File System') }}</label>
					<select class="custom-select mr-sm-2" id="method" name="method">
						<option value="local" @if($info->system_type=='local') selected="" @endif>{{ __('System Directory') }}</option>
						<option value="do" @if($info->system_type=='do') selected="" @endif>{{ __('Digital Ocean Droplet') }}</option>
					</select>


				</div>
				<div id="cdn" class="form-group @if($info->system_type =='local') none @endif">
					<label for="url">{{ __('CDN Link') }}</label>
					<input type="text" name="url" id="url" placeholder="https://cdn.example.com" class="form-control" value="{{ $info->system_url }}">
				</div>
				<div class="form-group">

					<button type="submit" class="btn btn-primary col-12">{{ __('Update') }}</button>
				</div>
			</div>
		</form>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//response will assign this function
	function success(res){
		$('.alert-danger').hide();
		Sweet('success','File System Updated')
	}
	function errosresponse(xhr){
		
		$('.alert-success').hide();
		$('.alert-danger').show();
		$("#errors").html("<li class='text-danger'>"+xhr.responseJSON.errors.url+"</li>")
	}

	$(document).ready(function(){
		$('#method').on('change',function(){
			let val= $('#method').val();
			if (val=='do') {
				$('#cdn').show();
			}
			else{
				$('#cdn').hide();
			}
		})
	})
</script>

@endsection