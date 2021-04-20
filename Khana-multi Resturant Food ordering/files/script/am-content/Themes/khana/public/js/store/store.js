
(function($) {
	"use strict";
	$(window).on('load',function(){
		var url = $('#store_url').val();
		$.ajax({
			url: url,
			data: null,
			type: "GET",
			dataType: "HTML",
			beforeSend: function() {

			},
			success: function(response) {
				$('#online_order').html(response);
			}
		});
	});

	$('#book_form').on('submit',function(e){
		e.preventDefault();
		$.ajaxSetup({
			headers: {
				'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
			}
		});
		$.ajax({
			type: 'POST',
			url: this.action,
			data: new FormData(this),
			dataType: 'json',
			contentType: false,
			cache: false,
			processData:false,
			beforeSend: function()
			{
				$('#book_submit').html('Please Wait...');
			},
			success: function(response){ 

				if(response.errors)
				{
					$('.error-message-area').fadeIn();
					$('.error-msg').html(response.errors);
					$(".error-message-area").delay( 2000 ).fadeOut( 2000 );
					$('#book_submit').html('Submit');
				}

				if(response == 'ok')
				{
					$('.alert-message-area').fadeIn();
					$('.ale').html('Your booking request successfully sent');
					$(".alert-message-area").delay( 2000 ).fadeOut( 2000 );
					$('#book_submit').html('Submit');
					document.getElementById('book_form').reset();
				}
			},
			error: function(xhr, status, error) 
			{
				$('.errorarea').show();
				$.each(xhr.responseJSON.errors, function (key, item) 
				{
					Sweet('error',item)
					$("#errors").html("<li class='text-danger'>"+item+"</li>")
				});
				errosresponse(xhr, status, error);
			}
		})
	});



})(jQuery);

function restaurantsinfo(slug)
{
	if(!$('*').hasClass('restaurantsinfo_open')){
		var url = $('#resturantinfo_url').val();
		$.ajax({
			url: url,
			data: {slug:slug},
			type: "GET",
			dataType: "HTML",
			beforeSend: function() {
				$('#info').html('<div class="loader-main-area"><div class="loader-area"><div class="loader"></div></div></div>');
			},
			success: function(response) {
				$('#info').html(response);
			}
		});
	}
}





