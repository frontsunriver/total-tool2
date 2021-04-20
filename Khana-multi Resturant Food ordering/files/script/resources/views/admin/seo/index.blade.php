@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<h4 class="mb-20">{{ __('Seo Info') }}</h4>
		<div class="alert alert-success text-white none">
			<ul id="success" class="text-white">

			</ul>
		</div>		
		<form method="post"  id="basicform" action="{{ route('admin.seo.store') }}">
			@csrf
			<div class="custom-form">
				<div class="row">
					<div class="form-group col-lg-6">
						<label for="title">{{ __('Site Title') }}</label>
						<input type="text" name="title"  id="title" class="form-control" value="{{ $info->title }}" placeholder="Site Title">
					</div>
					<div class="form-group col-lg-6">
						<label for="twitterTitle">{{ __('Twitter Title') }}</label>
						<input type="text" name="twitterTitle"  id="twitterTitle" class="form-control" value="{{ $info->twitterTitle }}" placeholder="Twiiter Title">
					</div>
					<div class="form-group col-lg-6">
						<label for="canonical ">{{ __('Canonical URL') }}</label>
						<input type="text" name="canonical"  id="canonical" class="form-control" value="{{ $info->canonical }}" placeholder="Canonical URL">
					</div>
					<div class="form-group col-lg-6">
						<label for="tags">{{ __('Tags') }}</label>
						<input type="text" name="tags"  id="tags" class="form-control" value="{{ $info->tags }}" placeholder="Tags">
					</div>
					<div class="form-group col-lg-12">
						<label for="description">{{ __('Site description') }}</label>
						<textarea name="description" id="description" class="form-control" cols="30" rows="10">{{ $info->description }}</textarea>
					</div>
					<div class="form-group col-lg-12">
						<button class="btn btn-primary col-12" type="submit">{{ __('Update') }}</button>
					</div>
				</div>
			</div>
		</form>
		<form id="basicform1" action="{{ route('admin.seo.update',1) }}">
			@csrf
			@method('PUT')
			<div class="custom-form">
				<div class="form-group">
					<label for="sitemap">{{ __('Sitemap') }}</label>
					<input type="text" disabled="" class="form-control" value="{{ url('/').'/sitemap.xml' }}">
				</div>
				<div class="form-group">
					<button class="btn btn-primary col-12" type="submit">{{ __('Genarate New Sitemap') }}</button>
				</div>
			</div>
		</form>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
	"use strict";	
	// response will assign this function
	function success(res){
		$('.alert-danger').hide();
		$('.alert-success').show();
		$("#success").html("<li class='text-white'>"+res+"</li>");
	}
	function errosresponse(xhr){
		$('.alert-success').hide();
		$('.alert-danger').show();
		
	}
</script>
@endsection