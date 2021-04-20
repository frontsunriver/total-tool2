(function ($) {
	"use strict";

	$("#multiple_page").on('change',function(){
		const element = document.querySelector("#save_btn");
		if (element.classList.contains("disabled")) {
			var url = $("#page_change_url").val();
			var page_name = $("#multiple_page").val();
			$.ajax({
				url: url,
				data: { page_name: page_name },
				type: "GET",
				dataType: "HTML",
				success: function(response) {
					if (response == 'ok') {
						location.reload();
					}
				}
			});
		}else{
			Swal.fire({
				title: 'Save Changes?',
				icon: 'warning',
				showCancelButton: true,
				confirmButtonColor: '#43467f',
				cancelButtonColor: '#d33',
				confirmButtonText: 'Yes, Save it!'
			}).then((result) => {
				if (result.value) {
					var url = $("#save_btn_url").val();
					var id = 1;
					$.ajax({
						url: url,
						data: {id:id},
						type: "GET",
						dataType: "HTML",
						success: function(response) {
							$("#save_btn").addClass('disabled');
						}
					});
					Swal.fire({
						title: 'Saved!',
						text: "Your update has been saved.",
						icon: 'success',
						showCancelButton: true,
						confirmButtonColor: '#43467f',
						cancelButtonColor: '#d33',
						confirmButtonText: 'Ok! Move Forward'
					}).then((result) => {
						if (result.value) {
							var url1 = $("#page_change_url").val();
							var page_name = $("#multiple_page").val();
							$.ajax({
								url: url1,
								data: { page_name: page_name },
								type: "GET",
								dataType: "HTML",
								success: function(response) {
									if (response == 'ok') {
										location.reload();
									}
								}
							});
						}
					})
				}
			})
		}
	});


	$("#save_btn").on('click',function(){
		Swal.fire({
			title: 'Are you sure?',
			text: "You won't be able to revert this!",
			icon: 'warning',
			showCancelButton: true,
			confirmButtonColor: '#43467f',
			cancelButtonColor: '#d33',
			confirmButtonText: 'Yes, Save it!'
		}).then((result) => {
			if (result.value) {
				var url = $("#save_btn_url").val();
				var id = 1;
				$.ajax({
					url: url,
					data: {id:id},
					type: "GET",
					dataType: "HTML",
					success: function(response) {
						$("#save_btn").addClass('disabled');
					}
				});
				Swal.fire(
					'Saved!',
					'Your update has been Saved.',
					'success'
					)
			}
		})
		
	});

	$('.display-view').on('click','a',function(){
		$(this).addClass('active').siblings().removeClass('active');
	});

	$("#mobile_device").on('click',function(){
		$('.website-append').addClass('mobile');
		$('.website-append').removeClass('labtop');
		$('.website-append').removeClass('tablet');
	});

	$("#labtop_device").on('click',function(){
		$('.website-append').addClass('labtop');
		$('.website-append').removeClass('mobile');
		$('.website-append').removeClass('tablet');
	});

	$("#tablet_device").on('click',function(){
		$('.website-append').addClass('tablet');
		$('.website-append').removeClass('labtop');
		$('.website-append').removeClass('mobile');
	});

	$(window).on('load',function(){
		$('.loader').fadeOut();
	});

})(jQuery);	


function arrow()
{
	$('.loading').fadeIn();
	$(".sidebar-area-start").load(' .sidebar-area-start');
	
}


function loadFile(event,id,type,option,main_id,p_id=null) {
	var url = $('#image_upload_url').val();
	$.ajaxSetup({
		headers: {
			'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
		}
	});
	let form = document.getElementById('img_form');
	let formdata = new FormData(form);
	formdata.append('id',id);
	formdata.append('type',type);
	formdata.append('option',option);
	formdata.append('main_id',main_id);
	formdata.append('p_id',p_id);
	$.ajax({
		url: url,
		data: formdata,
		type: "POST",
		dataType: "JSON",
		processData: false,
		contentType: false,
		beforeSend: function() {
			if (type == 'image') {
				var reader = new FileReader();
				reader.onload = function (e) {
					$("#myFrame").contents().find('#'+id).attr('src',e.target.result);
				}
				reader.readAsDataURL(event.target.files[0]);
			}

			if (type == 'bg_image') {
				var reader = new FileReader();
				reader.onload = function (e) {
					$("#myFrame").contents().find('#'+id).css("background-image", "url("+e.target.result+")");
					console.log(e.target.result);
				}
				reader.readAsDataURL(event.target.files[0]);

			}
			
		},
		success: function(response) {

		}
	});
};


function section(id)
{
	var url = $("#section_url").val();
	$.ajax({
		url: url,
		data: { id: id },
		type: "GET",
		dataType: "HTML",
		beforeSend: function() {
			$('.loading').fadeIn();
		},
		success: function(response) {
			var iframe = document.getElementById("myFrame");
			var elmnt = iframe.contentWindow.document.getElementById(id).scrollIntoView({ behavior: 'smooth'});
			$('.sidebar-area-start').html(response);
			$('.loading').fadeOut();
		}
	});
}

function settings_option(id,type,option,main_id)
{
	var value = $('#'+id).val();
	var url = $('#value_update').val();
	$.ajax({
		url: url,
		data: { id: id, value: value, option: option, main_id: main_id },
		type: "GET",
		dataType: "HTML",
		success: function(response) {
			console.log(response);
		}
	});
}

function settings_option1(id,type,option,main_id)
{
	$("#save_btn").removeClass('disabled');
	var value = $('#'+id).val();
	if (type == 'text' || type == 'textarea') {
		var iframe = document.getElementById("myFrame");
		var elmnt = iframe.contentWindow.document.getElementById(id);
		elmnt.textContent = value;
	}
	if (type == 'link') {
		$("#myFrame").contents().find('#'+id).attr('href',value);
	}

	if (type == 'icon') {
		$("#myFrame").contents().find('#'+id).attr('class',value);
	}
}

function section_multi_options(id,type,option,main_id)
{
	$("#save_btn").removeClass('disabled');
	var value = $('#'+id).val();
	if (type == 'text' || type == 'textarea') {
		var iframe = document.getElementById("myFrame");
		var elmnt = iframe.contentWindow.document.getElementById(id);
		elmnt.textContent = value;
	}
	if (type == 'link') {
		$("#myFrame").contents().find('#'+id).attr('href',value);
	}
	if (type == 'icon') {
		$("#myFrame").contents().find('#'+id).attr('class',value);
	}
}

function section_multi_options1(id,type,option,main_id,p_id)
{
	var value = $('#'+id).val();
	var url = $('#multiple_settings_option').val();
	$.ajax({
		url: url,
		data: { id: id, value: value, option: option, type: type, main_id: main_id, p_id: p_id },
		type: "GET",
		dataType: "HTML",
		success: function(response) {
			console.log(response)
		}
	});
}

$(function() {
	$(document).on("change",".uploadFile", function()
	{
		var uploadFile = $(this);
		var files = !!this.files ? this.files : [];
        if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support

        if (/^image/.test( files[0].type)){ // only image file
            var reader = new FileReader(); // instance of the FileReader
            reader.readAsDataURL(files[0]); // read the local file

            reader.onloadend = function(){ // set image data as background of div
                uploadFile.closest(".imgUp").find('.imagePreview').css("background-image", "url("+this.result+")");
                $("#save_btn").removeClass('disabled');
            }
        }

    });
});