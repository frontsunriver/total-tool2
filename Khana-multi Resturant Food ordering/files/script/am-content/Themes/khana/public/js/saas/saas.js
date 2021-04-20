(function ($) {
"use strict";

 $.ajax({
 	type: 'GET',
 	url: $('#saasurls').val(),
 	contentType: false,
 	cache: false,
 	processData:false,
 })

})(jQuery); 