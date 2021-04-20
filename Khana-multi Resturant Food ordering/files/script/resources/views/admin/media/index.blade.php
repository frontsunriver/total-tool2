@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'All Media Files'])
<div class="row">
	<div class="col-12 mt-2">
		<div class="card">
			<div class="card-body">
				<div class="float-right">
					<form>
						<div class="input-group mb-2 col-12">

							<input type="text" class="form-control" placeholder="Search..." required="" name="src" autocomplete="off" value="{{ $src ?? '' }}" >
							<select class="form-control" name="type">

								<option value="user_id">{{ __('Search By User ID') }}</option>
								<option value="name">{{ __('Search By Name') }}</option>
							</select>
							<div class="input-group-append">                                            
								<button class="btn btn-primary" type="submit"><i class="fas fa-search"></i></button>
							</div>
						</div>
					</form>
				</div>
				<form action="{{ route('admin.medias.destroy') }}" id="basicform">
				<div class="float-left">
					<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control" name="status">
										<option>{{ __('Select Action') }}</option>
										<option value="delete">{{ __('Delete Permanently') }}</option>
									</select>
								</div>
							</div>
							<div class="single-filter">
								<button type="submit" class="btn btn-primary mt-1 ml-1">{{ __('Apply') }}</button>
							</div>
						</div>
				</div>
				<div class="table-responsive">
					<table class="table table-striped table-hover text-center table-borderless">
						<thead>
							<tr>
								<th class="am-select">
									<div class="custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input checkAll" id="checkAll">
										<label class="custom-control-label" for="checkAll"></label>
									</div>
								</th>
								<th>{{ __('File') }}</th>
								
								<th>{{ __('User Id') }}</th>
								
								<th>{{ __('Url') }}</th>
								<th>{{ __('Directory') }}</th>

								<th>{{ __('Uploaded At') }}</th>
							</tr>
						</thead>
						<tbody>
							@foreach($medias as $row)
							<tr>
								<th>
									<div class="custom-control custom-checkbox">
										<input type="checkbox" name="id[]" class="custom-control-input" id="customCheck{{ $row->id }}" value="{{ $row->id }}">
										<label class="custom-control-label" for="customCheck{{ $row->id }}"></label>
									</div>
								</th>
								<td><img src="{{ asset($row->url) }}" height="50"></td>

								<td>
									@if (Amcoders\Plugin\Plugin::is_active('shop'))
									<a href="{{ route('admin.vendor.show',$row->user_id) }}">#{{ $row->user_id }}</a>
									@else 
									#{{ $row->user_id }}
									@endif
								</td>
								<td><a href="{{ url($row->url) }}">{{ Str::limit($row->url, 30) }}</a></td>
								<td>{{ $row->path }}</td>
								<td>{{ $row->created_at->diffforHumans() }}</td>
							</tr>
							@endforeach
						</tbody>
						<tfoot>
							<tr>
								<th class="am-select">
									<div class="custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input checkAll" id="checkAll">
										<label class="custom-control-label" for="checkAll"></label>
									</div>
								</th>
								<th>{{ __('File') }}</th>
								
								<th>{{ __('User Id') }}</th>
								
								<th>{{ __('Url') }}</th>
								<th>{{ __('Directory') }}</th>

								<th>{{ __('Uploaded At') }}</th>
								
							</tr>
						</tfoot>
					</table>
					{{ $medias->links() }}
				</div>
				</form>
			</div>
		</div>
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