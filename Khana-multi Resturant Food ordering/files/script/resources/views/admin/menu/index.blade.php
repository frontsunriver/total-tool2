@extends('layouts.backend.app')
@section('style')

<link rel="stylesheet" href="{{ asset('admin/bootstrap-iconpicker/css/bootstrap-iconpicker.min.css') }}"/>
@endsection
@section('content')

<div class="row">
	<div class="col-md-4">
		<div class="card">
			<div class="card-body">
				<h4 class="mb-20">{{ __('Menu List') }}</h4>
				<div class="row">
					<div class="col-lg-12">
						<div class="alert alert-danger none">
							<ul id="errors"></ul>
						</div>	
						<form id="frmEdit" class="form-horizontal">
							<div class="custom-form">
								<div class="form-group">
									<label for="text">{{ __('Text') }}</label>
									<div class="input-group">
										<input type="text" class="form-control item-menu" name="text" id="text" placeholder="Text" autocomplete="off">
										<div class="input-group-append">
											<button type="button" id="myEditor_icon" class="btn btn-primary btn-sm"></button>
										</div>
									</div>
									<input type="hidden" name="icon" class="item-menu">
								</div>
								<div class="form-group">
									<label for="href">{{ __('URL') }}</label>
									<input type="text" class="form-control item-menu" id="href" name="href" placeholder="URL" required autocomplete="off">
								</div>
								<div class="form-group">
									<label for="target">{{ __('Target') }}</label>
									<select name="target" id="target" class="custom-select mr-sm-2 item-menu">
										<option value="_self">{{ __('Self') }}</option>
										<option value="_blank">{{ __('Blank') }}</option>
										<option value="_top">{{ __('Top') }}</option>
									</select>
								</div>
								<div class="form-group none">
									<label for="title">{{ __('Tooltip') }}</label>
									<input type="text" name="title" class="form-control item-menu" id="title" placeholder="Tooltip">
								</div>
							</div>
						</form>
						<div class="menu-add-update d-flex">
							<button type="button" id="btnUpdate" class="btn btn-update  btn-warning text-white col-6 mr-2" disabled><i class="fas fa-sync-alt"></i> {{ __('Update') }}</button>
							<button type="button" id="btnAdd" class="btn btn-success col-6 "><i class="fas fa-plus"></i> {{ __('Add') }}</button>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="col-lg-8">
		<div class="card mb-3">
			<div class="card-body">
				<div class="row mb-10">
					<div class="col-lg-6">
						<h4>{{ __('Menu structure') }}</h4>
					</div>
					<div class="col-lg-6">
						<div class="save-menu f-right" >
							<form id="basicform" class="f-right" method="post" action="{{ route('admin.menus.MenuNodeStore') }}"> @csrf <input type="hidden" name="data" id="data"> <input type="hidden" name="menu_id" value="{{ $info->id }}"> <button id="form-button" class="btn btn-primary " type="submit">{{ __('Save Changes') }}</button></form>
						</div>
					</div>
				</div>
				<ul id="myEditor" class="sortableLists list-group">
				</ul>	
				
			</div>
		</div>
	</div>
</div>
@endsection

@section('script')
<script  src="{{ asset('admin/js/bootstrap.bundle.min.js') }}"></script>
<script  src="{{ asset('admin/js/jquery-menu-editor.min.js') }}"></script>
<script  src="{{ asset('admin/bootstrap-iconpicker/js/iconset/fontawesome5-3-1.min.js') }}"></script>
<script  src="{{ asset('admin/bootstrap-iconpicker/js/bootstrap-iconpicker.min.js') }}"></script>
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
"use strict";
(function ($) {
    // menu items
    var arrayjson = {!! $info->data !!};
    // icon picker options
    var iconPickerOptions = {searchText: "Search...", labelHeader: "{0}/{1}"};
    // sortable list options
    var sortableListOptions = {
    	placeholderCss: {'background-color': "#cccccc"}
    };

    var editor = new MenuEditor('myEditor', {listOptions: sortableListOptions, iconPicker: iconPickerOptions});
    editor.setForm($('#frmEdit'));
    editor.setUpdateButton($('#btnUpdate'));
    $('#btnReload').on('click', function () {
    	editor.setData(arrayjson);
    });

    $('#btnOutput').on('click', function () {
    	var str = editor.getString();
    	$("#out").text(str);
    });

    $("#btnUpdate").on('click',function(){
    	if ($('#text').val() != '' && $('#href').val() != '') {
    		editor.update();
    	}	
    });

    $('#btnAdd').on('click',function(){
    	if ($('#text').val() != '' && $('#href').val() != '') {
    		editor.add();
    	}
    	
    });

    $('#form-button').on('click',function(){
    	$("#data").val(editor.getString());
    })
    editor.setData(arrayjson);
})(jQuery);	
	function success(res){
		$('.alert-danger').hide();
		$('.alert-success').show();
		$("#success").html("<li class='text-success'>"+res+"</li>");
	}
	function errosresponse(xhr){
		$('.alert-success').hide();
		$('.alert-danger').show();
		
	}
</script>
@endsection