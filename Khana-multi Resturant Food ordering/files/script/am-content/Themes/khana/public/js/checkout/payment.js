
(function($) {
 "use strict";

	$('#cod_payment_form').on('submit',function(e){
		e.preventDefault();
		$.ajaxSetup({
			headers: {
				'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
			}
		});
		let name = $('#first_name').val();
		let phone = $('#phone').val();
		let latitude = $('#latitude').val();
		let longitude = $('#longitude').val();
		let location_input = $('#location_input').val();
		let shipping = $('#shipping').val();
		let order_details = $('#order_details').val();
		let payment_type = $('#payment_type').val();
		let form = document.getElementById('cod_payment_form');
		let formdata = new FormData(form);
		formdata.append('name',name);
		formdata.append('phone',phone);
		formdata.append('latitude',latitude);
		formdata.append('longitude',longitude);
		formdata.append('delivery_address',location_input);
		formdata.append('shipping',shipping);
		formdata.append('order_note',order_details);
		formdata.append('payment_type',payment_type);
		$.ajax({
			type: 'POST',
			url: this.action,
			data: formdata,
			dataType: 'html',
			contentType: false,
			cache: false,
			processData:false,

			success: function(response){ 
				if(response == 'auth_error')
				{
					var base_url = $('#base_url').val();
					window.history.pushState("","",base_url+'/user/login');
    				location.reload();
				}else{
					var base_url = $('#base_url').val();
					window.history.pushState("","",base_url+'/order/confirmation');
    				location.reload();
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


	var stripe_api_key = $('#stripe_api_key').val();
    // Create a Stripe client.
    var stripe = Stripe(stripe_api_key);
    // Create an instance of Elements.
    var elements = stripe.elements();
    // Custom styling can be passed to options when creating an Element.
    // (Note that this demo uses a wider set of styles than the guide below.)
    var style = {
    	base: {
    		color: '#32325d',
    		fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
    		fontSmoothing: 'antialiased',
    		fontSize: '16px',
    		'::placeholder': {
    			color: '#aab7c4'
    		}
    	},
    	invalid: {
    		color: '#fa755a',
    		iconColor: '#fa755a'
    	}
    };
    // Create an instance of the card Element.
    var card = elements.create('card', {style: style});
    // Add an instance of the card Element into the `card-element` <div>.
    card.mount('#card-element');
    // Handle real-time validation errors from the card Element.
    card.addEventListener('change', function(event) {
    	var displayError = document.getElementById('card-errors');
    	if (event.error) {
    		displayError.textContent = event.error.message;
    	} else {
    		displayError.textContent = '';
    	}
    });
    // Handle form submission.
    var form = document.getElementById('payment-form');
    form.addEventListener('submit', function(event) {
    	event.preventDefault();
		    stripe.createToken(card).then(function(result) {
		    if (result.error) {
		        // Inform the user if there was an error.
		        var errorElement = document.getElementById('card-errors');
		        errorElement.textContent = result.error.message;
		    } else {
		        // Send the token to your server.
		        stripeTokenHandler(result.token);
		    }
		});
    });
    // Submit the form with the token ID.
    function stripeTokenHandler(token) {
      // Insert the token ID into the form so it gets submitted to the server
      var form = document.getElementById('payment-form');
      var name_val = $('#first_name').val();
      var name = document.createElement('input');
      name.setAttribute('type', 'hidden');
      name.setAttribute('name', 'name');
      name.setAttribute('value', name_val);
      form.appendChild(name);
      //create new
      var phone_val = $('#phone').val();
      var phone = document.createElement('input');
      phone.setAttribute('type', 'hidden');
      phone.setAttribute('name', 'phone');
      phone.setAttribute('value', phone_val);
      form.appendChild(phone);
      //create new
      var latitude_val = $('#latitude').val();
      var latitude = document.createElement('input');
      latitude.setAttribute('type', 'hidden');
      latitude.setAttribute('name', 'latitude');
      latitude.setAttribute('value', latitude_val);
      form.appendChild(latitude);
      //create new
      var longitude_val = $('#longitude').val();
      var longitude = document.createElement('input');
      longitude.setAttribute('type', 'hidden');
      longitude.setAttribute('name', 'longitude');
      longitude.setAttribute('value', longitude_val);
      form.appendChild(longitude);
      //create new
      var location_input = $('#location_input').val();
      var delivery_address = document.createElement('input');
      delivery_address.setAttribute('type', 'hidden');
      delivery_address.setAttribute('name', 'delivery_address');
      delivery_address.setAttribute('value', location_input);
      form.appendChild(delivery_address);
      //create new
      var shipping_val = $('#shipping').val();
      var shipping = document.createElement('input');
      shipping.setAttribute('type', 'hidden');
      shipping.setAttribute('name', 'shipping');
      shipping.setAttribute('value', shipping_val);
      form.appendChild(shipping);
      //create new
      var order_details = $('#order_details').val();
      var order_note = document.createElement('input');
      order_note.setAttribute('type', 'hidden');
      order_note.setAttribute('name', 'order_note');
      order_note.setAttribute('value', order_details);
      form.appendChild(order_note);
      //create new
      var payment_type_val = $('#payment_type').val();
      var payment_type = document.createElement('input');
      payment_type.setAttribute('type', 'hidden');
      payment_type.setAttribute('name', 'payment_type');
      payment_type.setAttribute('value', payment_type_val);
      form.appendChild(payment_type);
      //create new
      var hiddenInput = document.createElement('input');
      hiddenInput.setAttribute('type', 'hidden');
      hiddenInput.setAttribute('name', 'stripeToken');
      hiddenInput.setAttribute('value', token.id);
      form.appendChild(hiddenInput);
      // Submit the form
      form.submit();
  }


  	//paypal payment gateway
  	var paypal_api_key = $('#paypal_api_key').val();
	paypal.Button.render({
	    // Configure environment
	    env: 'production',
	    client: {
	    	sandbox: paypal_api_key,
	    	production: paypal_api_key
	    },
	    // Customize button (optional)
	    locale: 'en_US',
	    style: {
	    	size: 'large',
	    	color: 'gold',
	    	shape: 'pill',
	    },

	    // Enable Pay Now checkout flow (optional)
	    commit: true,

	    // Set up a payment
	    payment: function(data, actions) {
	    	var total_price = $('#total_price').val();
	    	var currency_value = $('#currency_value').val();
	    	return actions.payment.create({
	    		transactions: [{
	    			amount: {
	    				total: total_price,
	    				currency: currency_value
	    			}
	    		}]
	    	});
	    },
	    // Execute the payment
	    onAuthorize: function(data, actions) {
	    	var base_url = $('#base_url').val();
	    	var _token = $('#_token').val();
	    	var name = $('#first_name').val();
			var phone = $('#phone').val();
			var latitude = $('#latitude').val();
			var longitude = $('#longitude').val();
			var location_input = $('#location_input').val();
			var shipping = $('#shipping').val();
			var order_details = $('#order_details').val();
			var payment_type = $('#payment_type').val();
	    	return actions.payment.execute().then(function() {
	    		return actions.request.post(base_url+'/create-order', {
	    			name: name,
	    			phone: phone,
	    			latitude: latitude,
	    			longitude: longitude,
	    			delivery_address: location_input,
	    			shipping: shipping,
	    			order_note: order_details,
	    			payment_type: payment_type,
	    			_token: _token,
	    		})
	    		.then(function(res) {
	    			if(res == 'auth_error')
					{
						var base_url = $('#base_url').val();
						window.history.pushState("","",base_url+'/user/login');
	    				location.reload();
					}else{
						var base_url = $('#base_url').val();
						window.history.pushState("","",base_url+'/order/confirmation');
	    				location.reload();
					}
	    		});
	    	});
	    }
	}, '#paypal-button');


	//coupon form submit
	 $('#couponform').on('submit',function(e){
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
    			if(response.message)
    			{
    				$('#checkout_right').load(' #checkout_right');
    				$('.alert-message-area').fadeIn();
    				$('.ale').html(response.message);
    				$(".alert-message-area").delay( 2000 ).fadeOut( 2000 );
    				window.location.reload();
    			}

    			if(response.error)
    			{
    				$('.error-message-area').fadeIn();
    				$('.error-msg').html(response.error);
    				$(".error-message-area").delay( 2000 ).fadeOut( 2000 );
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

function select_payment(type)
{
	$('#payment_type').val(type);
}