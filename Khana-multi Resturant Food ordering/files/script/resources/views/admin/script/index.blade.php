@extends('layouts.backend.app')

@section('content')
<div class="card">
	<div class="card-body">
		<h4 class="text-success" id="success"></h4>
		<form id="basicform" action="{{ route('admin.script.store') }}">
			@csrf
			<div class="custom-form">
				<div class="row">
					<div class="col-lg-12">
						<div class="form-group">
							<label for="css">{{ __('Header Custom Script') }}</label>
							<textarea class="form-control" cols="30" rows="10" name="css" id="css">@include('script.headerscript')</textarea>
						</div>
					</div>
					<div class="col-lg-12">
						<div class="form-group">
							<label for="js">{{ __('Footer Custom Script') }}</label>
							<textarea class="form-control" cols="30" rows="10" name="js" id="js">@include('script.footerscript')</textarea>
						</div>
					</div>
					<div class="col-lg-12">
						<button type="submit" class="btn col-12">{{ __('Update') }}</button>
					</div>
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
	// response will assign this function
	function success(res){
		$('#success').html(res);
	}
</script>
@endsection