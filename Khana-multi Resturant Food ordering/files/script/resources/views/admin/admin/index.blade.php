@extends('layouts.backend.app')
@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-6">
				<h4>{{ __('Admins') }}</h4>
			</div>
			<div class="col-lg-6">
                @can('admin.create')
				<div class="add-new-btn">
					<a href="{{ route('admin.users.create') }}" class="btn btn-primary float-right">{{ __('Add New') }}</a>
                </div>
                @endcan
			</div>
		</div>
		<br>
		<div class="card-action-filter">
			<form method="post" id="basicform" action="{{ route('admin.users.destroy') }}">
				@csrf
				<div class="row">
					<div class="col-lg-6">
						<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control selectric" name="status">
                                        <option disabled selected>{{ __('Select Action') }}</option>
										<option value="approved">{{ __('Active') }}</option>
										<option value="deactive">{{ __('Deactivate') }}</option>
									</select>
								</div>
                            </div>
                            @can('admin.edit')
							<div class="single-filter">
								<button type="submit" class="btn btn-primary btn-lg ml-2">{{ __('Apply') }}</button>
                            </div>
                            @endcan
						</div>
					</div>
					<div class="col-lg-6">
						
					</div>
				</div>
			</div>
			<div class="table-responsive custom-table">
				<table class="table">
					<thead>
						<tr>
							<th>
								<div class="custom-control custom-checkbox">
									<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
									<label class="custom-control-label checkAll" for="customCheck12"></label>
								</div>
							</th>
                            <th>Name</th>
                            <th>Email</th>
                            <th>Status</th>
                            <th>Role</th>
						</tr>
					</thead>
					<tbody>
						@foreach($users as $row)
						<tr>
							<td>
								<div class="custom-control custom-checkbox">
									<input type="checkbox" name="ids[]" class="custom-control-input" id="customCheck{{ $row->id }}" value="{{ $row->id }}">
									<label class="custom-control-label" for="customCheck{{ $row->id }}"></label>
								</div>
							</td>
							<td>
                                {{ $row->name }}
                                @can('admin.edit')
								<div class="hover">
									<a href="{{ route('admin.users.edit',$row->id) }}">{{ __('Edit') }}</a>

									
                                </div>
                                @endcan
                            </td>
                            <td>
                               {{ $row->email }}
                               
                            </td>
                        <td>@if($row->status==1)
                            <span class="badge badge-success">Active</span>
                            @else
                            <span class="badge badge-danger">Deactive</span>

                        @endif</td>
                        <td>@foreach($row->roles as $r) <span class="badge badge-primary">{{ $r->name }}</span> @endforeach</td>
							
						</tr>
						@endforeach
					</tbody>
				</form>
			
			</table>
			

		</div>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//response will assign this function
	function success(res){
		location.reload();
	}
	
</script>
@endsection