@extends('layouts.backend.app')


@section('content')
<div class="row">
	<div class="col-lg-9">      
		<div class="card">
			<div class="card-body">
				<h4>{{ __('Add new Page') }}</h4>
				<form method="post" action="{{ route('admin.page.store') }}">
					@csrf
					<div class="custom-form pt-20">
						@php
						$arr['title']= 'Page Title';
						$arr['id']= 'name';
						$arr['type']= 'text';
						$arr['placeholder']= 'Page Title';
						$arr['name']= 'title';
						$arr['is_required'] = true;

						echo  input($arr);

						$arr['title']= 'Meta Description';
						$arr['id']= 'excerpt';
						$arr['placeholder']= 'short description';
						$arr['name']= 'excerpt';
						$arr['is_required'] = true;

						echo  textarea($arr);

						@endphp
						<div class="form-group">
							<label>Page Content</label>
							<textarea name="content" class="summernote"></textarea>
						</div>
					</div>
				</div>
			</div>

		</div>
		<div class="col-lg-3">
			<div class="single-area">
				<div class="card">
					<div class="card-body">
						<h5>{{ __('Publish') }}</h5>
						<hr>
						<div class="btn-publish">
							<button type="submit" class="btn btn-primary col-12"><i class="fa fa-save"></i> {{ __('Save') }}</button>
						</div>
					</div>
				</div>
			</div>
			<div class="single-area">
				<div class="card sub">
				<div class="card-body">
						<h5>{{ __('Status') }}</h5>
					<hr>
					<select class="custom-select mr-sm-2" id="inlineFormCustomSelect" name="status">
							<option selected value="1">{{ __('Published') }}</option>
							<option value="2">{{ __('Draft') }}</option>

					</select>
				</div>
			</div>
		</div>
	</div>

	<input type="hidden" name="type" value="1">
	<input type="hidden"  name="post_type" value="page">
</form>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="https://cdn.ckeditor.com/4.15.1/standard/ckeditor.js"></script>
<script>

	"use strict";	
	(function ($) {

		CKEDITOR.replace( 'content' );

		$('.use').on('click',function(){
			$('#preview').attr('src',myradiovalue);
			$('#image').val(myradiovalue);
		});

	})(jQuery);	
	//success response will assign here
	function success(res){
		location.reload()
	}	
</script>
@endsection