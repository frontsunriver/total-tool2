(function ($) {
	"use strict";

	function seed()
	{
		var url = $('#seed_url').val();
		var home_url = $('#home_url').val();
		$.ajaxSetup({
	 		headers: {
	 			'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
	 		}
	 	});
	 	$.ajax({
	 		type: 'GET',
	 		url: url,
	 		data: new FormData(this),
	 		dataType: 'html',
	 		contentType: false,
	 		cache: false,
	 		processData:false,
	 		success: function(response){ 
	 			$(".install-info").html(response);
	 			window.location.href = home_url;
	 		},
	 		error: function(xhr, status, error) 
	 		{
	 			

	 		}
	 	});
	}

	function migrate()
	{
		var url = $('#migrate_url').val();
		$.ajaxSetup({
	 		headers: {
	 			'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
	 		}
	 	});
	 	$.ajax({
	 		type: 'GET',
	 		url: url,
	 		data: new FormData(this),
	 		dataType: 'html',
	 		contentType: false,
	 		cache: false,
	 		processData:false,
	 		success: function(response){ 
	 			$(".install-info").html(response);
	 			seed();
	 		},
	 		error: function(xhr, status, error) 
	 		{
	 			

	 		}
	 	});
	}

	function check()
	{
		var url = $('#check_url').val();
		$.ajaxSetup({
	 		headers: {
	 			'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
	 		}
	 	});
	 	$.ajax({
	 		type: 'GET',
	 		url: url,
	 		data: new FormData(this),
	 		dataType: 'html',
	 		contentType: false,
	 		cache: false,
	 		processData:false,
	 		success: function(response){ 
	 			if (response == false) {
	 				$(".install-info").html("Could not find the database. Please check your configuration.");
	 				$(".back").removeClass('d-none');
	 			}else{	
	 				$(".install-info").html(response);
	 				console.log($('#type').val())
	 				if ($('#type').val() == 'install') {
	 					migrate();

	 				}
	 				else{
	 					var home_url = $('#home_url').val();
						window.location.href = home_url;
	 				}
	 				
	 			}
	 		},
	 		error: function(xhr, status, error) 
	 		{
	 			

	 		}
	 	});
	}

	 //install submit
	 $('#install').on('submit',function(e){
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
	 		dataType: 'html',
	 		contentType: false,
	 		cache: false,
	 		processData:false,
	 		beforeSend: function() {
	 			$(".loading_bar").fadeIn();
	 			$(".install-info").html("Sending Credentials");
		    },
	 		success: function(response){ 
	 			$(".install-info").html(response);

	 			check();
	 		},
	 		error: function(xhr, status, error) 
	 		{
	 			

	 		}
	 	});
	});

})(jQuery);	
