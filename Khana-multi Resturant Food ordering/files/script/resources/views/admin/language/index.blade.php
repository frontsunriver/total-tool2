@section('style')

@endsection
@extends('layouts.backend.app')

@section('content')
<div class="card shadow mb-4">
	<div class="card-header py-3">
		<h6 class="m-0 font-weight-bold text-primary">{{ __('Manage Language') }}</h6>
	</div>
	<div class="card-body">
		<form action="{{ route('admin.lang.set') }}" method="POST" id="basicform">
			@csrf
			<div class="row">
				<div class="col-lg-3">
					<div class="form-group">
						<select name="status" class="form-control">
							<option disabled selected>{{ __('Select Option') }}</option>
							<option value="active">{{ __('Active Language') }}</option>
						</select>
					</div>
				</div>
				<input type="hidden" value="{{ $theme_name }}" name="theme_name">
				<div class="col-lg-2 pl-0">
					<button type="submit" class="btn btn-primary btn-lg">{{ __('Submit') }}</button>
				</div>
				<div class="col-lg-7">
					<a href="{{ route('admin.language.create') }}" class="btn btn-info float-right btn-lg">{{ __('Add New Language') }}</a>
				</div>
			</div>
			<table class="table table-bordered">
				<thead>
				<tr>
					<th scope="col">
						<div class="custom-control custom-checkbox">
							<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
							<label class="custom-control-label checkAll" for="customCheck12"></label>
						</div>
					</th>
					<th scope="col">{{ __('Language Name') }}</th>
					<th scope="col">{{ __('Position') }}</th>
					<th scope="col">{{ __('Theme Name') }}</th>
					<th scope="col">{{ __('Action') }}</th>
				</tr>
				</thead>
				<tbody>
					@foreach($langs as $lang)
					<tr>
						<th scope="row">
							<div class="custom-control custom-checkbox">
								<input type="checkbox" name="lang[]" {{ $lang->status == 1 ? 'checked' : '' }} class="custom-control-input" id="customCheck{{ $lang->slug }}" value="{{ $lang->slug }}">
								<label class="custom-control-label" for="customCheck{{ $lang->slug }}"></label>
							</div>
						</th>
						@php
							$data = json_decode($lang->content,true);
						@endphp
						<td>{{ $data['lang_name'] }}</td>
						<td>{{ $data['lang_position'] }}</td>
						<td>{{ $lang->name }}</td>
						<td>
							<a href="{{ route('admin.lang.edit',['lang_code'=>$lang->slug,'theme_name'=>$lang->name]) }}" class="btn btn-info mr-2">{{ __('Customize') }}</a>
							<a href="{{ route('admin.lang.delete',['lang_code'=>$lang->slug,'theme_name'=>$lang->name]) }}" class="btn btn-danger cancel">{{ __('Delete') }}</a>
						</td>
					</tr>
					@endforeach
				</tbody>
			</table>
		</form>
	</div>
</div>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//success response will assign this function
	function success(res){
		location.reload();
	}
	function errosresponse(xhr){

		$("#errors").html("<li class='text-danger'>"+xhr.responseJSON[0]+"</li>")
	}
</script>
@endsection