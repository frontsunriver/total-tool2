@extends('layouts.backend.app')
@section('style')
<link rel="stylesheet" href="{{ asset('admin/assets/css/selectric.css') }}">
<link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/css/select2.min.css" rel="stylesheet" />
@endsection
@section('content')
<div class="card">
	<div class="card-header">
		<h5>Create New Language</h5>
	</div>
	<div class="card-body">
		<form action="{{ route('admin.lang.store') }}" method="POST" id="basicform">
			@csrf
			<div class="form-group">
				<label>Select Language</label>
				<select name="language_code" class="form-control select2">
					@foreach($countries as $row)
					<option value="{{ $row['code'] }}">{{ $row['name'] }}  -- {{ $row['nativeName'] }} -- ( {{ $row['code'] }})</option>
					@endforeach
				</select>
			</div>
			<div class="form-group">
				<label>Select Theme</label>
				<select name="theme_name" class="form-control selectric">
					@foreach ($themes as $theme)
					<option value="{{ $theme['Text Domain'] }}">{{ $theme['Theme Name'] }}</option>
					@endforeach
				</select>
			</div>
			<div class="form-group">
				<label>Select Position</label>
				<select name="theme_position" class="form-control selectric">
					<option value="LTR">LTR</option>
					<option value="RTL">RTL</option>
				</select>
			</div>
			<button type="submit" class="btn btn-primary btn-lg">Submit</button>
		</form>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="{{ asset('admin/assets/js/jquery.selectric.min.js') }}"></script>
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/js/select2.min.js"></script>

<script type="text/javascript">
	"use strict";	
	//success response will assign this function
	function success(res){
		window.history.pushState('', '', '{{ route('admin.language.index') }}');
		location.reload();
	}
	function errosresponse(xhr){

		$("#errors").html("<li class='text-danger'>"+xhr.responseJSON[0]+"</li>")
	}
</script>
@endsection


