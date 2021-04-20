"use strict";
$('#addon_form').on('submit',function(e){
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

		success: function(response){ 
			if(response == 'ok')
			{
				$('.main_cart').load(' .main_cart');
				$('.main_cart_ok').load(' .main_cart_ok');
				$('.modal-area').addClass('d-none');
				$('.count_load').load(' .count_load');

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





function close_model()
{
	$('.modal-area').addClass('d-none');
}


function product_addon(slug,store_slug)
{
	var url = $('#addon_url').val();
	$.ajax({
		url: url,
		data: {slug:slug,store_slug:store_slug},
		type: "GET",
		dataType: "HTML",
		beforeSend: function() {

		},
		success: function(response) {
			$('.modal-area').removeClass('d-none');
			$('.modal-main-content').html(response);
		}
	});
}


function product_add_to_cart(slug,store_slug) {
	var url = $('#add_to_cart_url').val();
	$.ajaxSetup({
		headers: {
			'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
		}
	});
	$.ajax({
		type: 'GET',
		url: url,
		data: {slug:slug,store_slug:store_slug},
		dataType: 'HTML',
		success: function(response){ 
			if(response == 'ok')
			{
				$('.main_cart').load(' .main_cart');
				$('.main_cart_ok').load(' .main_cart_ok');
				$('.count_load').load(' .count_load');
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
}


function limit_plus(id,store_slug) {
	var l = $('#total_limit'+id).val();
	l++;
	document.getElementById('total_limit'+id).value = l;
	var url = $('#cart_update').val();
	var data_value = $('#total_limit'+id).val();
	$.ajaxSetup({
		headers: {
			'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
		}
	});
	$.ajax({
		type: 'GET',
		url: url,
		data: {id:id,data_value:data_value,store_slug:store_slug},
		dataType: 'HTML',
		success: function(response){ 
			if(response == 'ok')
			{
				$('.main_cart').load(' .main_cart');
				$('.main_cart_ok').load(' .main_cart_ok');
				$('.count_load').load(' .count_load');
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
}

function limit_minus(id,store_slug) {
	var l = $('#total_limit'+id).val();
	if($('#total_limit'+id).val() > 1)
	{
		l--;
		document.getElementById('total_limit'+id).value = l;
		var url = $('#cart_update').val();
		var data_value = $('#total_limit'+id).val();
		$.ajaxSetup({
			headers: {
				'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
			}
		});
		$.ajax({
			type: 'GET',
			url: url,
			data: {id:id,data_value:data_value,store_slug:store_slug},
			dataType: 'HTML',
			success: function(response){ 
				if(response == 'ok')
				{
					$('.main_cart').load(' .main_cart');
					$('.main_cart_ok').load(' .main_cart_ok');
					$('.count_load').load(' .count_load');
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
	}
}


function delete_cart(id,store_slug)
{
	if(confirm('Are you want to delete this product from cart?'))
	{
		var url = $('#cart_delete').val();
		$.ajaxSetup({
			headers: {
				'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
			}
		});
		$.ajax({
			type: 'GET',
			url: url,
			data: {id:id,store_slug:store_slug},
			dataType: 'HTML',
			success: function(response){ 
				if(response == 'ok')
				{
					$('.main_cart').load(' .main_cart');
					$('.main_cart_ok').load(' .main_cart_ok');
					$('.count_load').load(' .count_load');
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
	}
}

function delivery_pickup(slug)
{
	var pickup_price = $('#pickup_price').val();
	var delivery_price = $('#delivery_price').val();
	var url = $('#checkout_type').val();
	var p_id = $('#d_p').val();
	if(p_id == 1)
	{	
		$.ajax({
			type: 'GET',
			url: url,
			data: {id:1,slug:slug},
			dataType: 'HTML',
			success: function(response){ 
				$('#d_p').val(0);
				$('#dummy').html('<i class="fas fa-truck"></i> '+delivery_price+' Min');
			}
		})
		
	}else{
		$.ajax({
			type: 'GET',
			url: url,
			data: {id:0,slug:slug},
			dataType: 'HTML',
			success: function(response){ 
				$('#d_p').val(1);
				$('#dummy').html('<i class="fas fa-truck"></i> '+pickup_price+' Min');
			}
		})
	}
}
