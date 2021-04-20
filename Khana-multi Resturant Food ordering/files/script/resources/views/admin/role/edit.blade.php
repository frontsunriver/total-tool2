@extends('layouts.backend.app')
@section('content')
<div class="row">
	<div class="col-lg-9">      
		<div class="card">
			<div class="card-body">
				<h4>{{ __('Edit Role') }}</h4>
				<form method="post" action="{{ route('admin.role.update',$role->id) }}" id="basicform">
                    @csrf
                    @method('PUT')
					<div class="pt-20">
						@php
						$arr['title']= 'Role Name';
						$arr['id']= 'name';
						$arr['type']= 'text';
						$arr['placeholder']= 'Enter Role Name';
						$arr['name']= 'name';
                        $arr['is_required'] = true;
                        $arr['value']=$role->name;
						echo  input($arr);
						@endphp

						<div class="row">
							<div class="col-sm-12">
								<div class="custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
										<label class="custom-control-label checkAll" for="customCheck12">Permissions</label>
									</div>
									<hr>
                                    @php $i = 1; @endphp
                                    @foreach ($permission_groups as $group)
                                        <div class="row">
                                            @php
                                                $permissions = App\User::getpermissionsByGroupName($group->name);
                                                $j = 1;
                                            @endphp
                                            
                                            <div class="col-3">
                                                <div class="form-check">
                                                    <input type="checkbox" class="form-check-input" id="{{ $i }}Management" value="{{ $group->name }}" onclick="checkPermissionByGroup('role-{{ $i }}-management-checkbox', this)" {{ App\User::roleHasPermissions($role, $permissions) ? 'checked' : '' }}>
                                                    <label class="form-check-label" for="checkPermission">{{ $group->name }}</label>
                                                </div>
                                            </div>
        
                                            <div class="col-9 role-{{ $i }}-management-checkbox">
                                               
                                                @foreach ($permissions as $permission)
                                                    <div class="form-check">
                                                        <input type="checkbox" class="form-check-input" onclick="checkSinglePermission('role-{{ $i }}-management-checkbox', '{{ $i }}Management', {{ count($permissions) }})" name="permissions[]" {{ $role->hasPermissionTo($permission->name) ? 'checked' : '' }} id="checkPermission{{ $permission->id }}" value="{{ $permission->name }}">
                                                        <label class="form-check-label" for="checkPermission{{ $permission->id }}">{{ $permission->name }}</label>
                                                    </div>
                                                    @php  $j++; @endphp
                                                @endforeach
                                                <br>
                                            </div>
        
                                        </div>
                                        @php  $i++; @endphp
                                    @endforeach
									
									

									
							</div>
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
			
	</div>


</form>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>

<script>
	"use strict";	
	
	
	function checkPermissionByGroup(className, checkThis){
            const groupIdName = $("#"+checkThis.id);
            const classCheckBox = $('.'+className+' input');
            if(groupIdName.is(':checked')){
                 classCheckBox.prop('checked', true);
             }else{
                 classCheckBox.prop('checked', false);
             }
         }	
</script>
@endsection