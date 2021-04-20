"use strict";
var myLatLng;

function initialise(){
	var map;


	var resturent = new google.maps.LatLng(resturent_lat,resturent_long);
	var customer = new google.maps.LatLng(customer_lat,customer_long);

	var option ={
		zoom : 10,
		center : resturent, 
	};
	map = new google.maps.Map(document.getElementById('map-canvas'),option);
	var display = new google.maps.DirectionsRenderer({polylineOptions: {
		strokeColor: "rgba(255, 0, 0, 0.5)"
	}});
	var services = new google.maps.DirectionsService();
	display.setMap(map);


	function calculateroute(){
		var request ={
			origin : resturent,
			destination:customer,
			travelMode: 'DRIVING'
		};
		services.route(request,function(result,status){

			if(status =='OK'){
				display.setDirections(result);
				display.setOptions( { suppressMarkers: true } );
				var leg = result.routes[ 0 ].legs[ 0 ];
				
				myLatLng={lat: mylat, lng: mylng}
				
				makeMarker( leg.start_location, resturent_icon, resturent_name );
				makeMarker( leg.end_location, user_icon, customer_name );
				makeMarker(myLatLng,my_icon);

			}
		});
	}
	function makeMarker( position, icon, title='' ) {
		new google.maps.Marker({
			position: position,
			map: map,
			icon: icon,
			title: title
		});
	}


	calculateroute();


}




