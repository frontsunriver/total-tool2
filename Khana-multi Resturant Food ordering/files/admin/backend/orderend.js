"use strict";

var mylat;
var mylng;
var myLatLng;
setInterval(function(){
	GetMyCurrentLocation();
	$('#basicform1').submit();
	initialise();
}, 60000);

GetMyCurrentLocation();
function GetMyCurrentLocation(){
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(setCurrentPosition);
	} else {
		alert("Geolocation is not supported by this browser.")
	}
}

function setCurrentPosition(pos) {
	mylat =parseFloat(pos.coords.latitude);
	mylng = parseFloat(pos.coords.longitude);
	localStorage.setItem("lat", mylat);	
	localStorage.setItem("long", mylng);	
	$('#lat').val(mylat);
	$('#long').val(mylng);


}

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
				if (localStorage.getItem("lat") != null) {
					mylat =parseFloat(localStorage.getItem("lat"));
					mylng =parseFloat(localStorage.getItem("long"));
					myLatLng={lat: mylat, lng: mylng}
				}
				else{
					myLatLng={lat: mylat, lng: mylng}
				}
				makeMarker( leg.start_location, resturent_icon, resturent_name );
				makeMarker( leg.end_location, user_icon, customer_name );
				makeMarker(myLatLng,my_icon,my_name);

			}
		});
	}
	function makeMarker( position, icon, title ) {
		new google.maps.Marker({
			position: position,
			map: map,
			icon: icon,
			title: title
		});
	}


	calculateroute();


}



var seconds=0;
if (localStorage.getItem("mytime") != null) {
	var seconds=localStorage.getItem("mytime");
}
else{
	var seconds=0;
}


setInterval(function(){
	seconds++;
	localStorage.setItem("mytime", seconds);	
	var timedata = secondsToHms(seconds);
	$('#time').html(timedata)

}, 1000);



function secondsToHms(d) {
	d = Number(d);
	var h = Math.floor(d / 3600);
	var m = Math.floor(d % 3600 / 60);
	var s = Math.floor(d % 3600 % 60);

	var hDisplay = h > 0 ? h + (h == 1 ? " hour, " : " hours, ") : "";
	var mDisplay = m > 0 ? m + (m == 1 ? " minute, " : " minutes, ") : "";
	var sDisplay = s > 0 ? s + (s == 1 ? " second" : " seconds") : "";
	return hDisplay + mDisplay + sDisplay; 
}

function success(param) {
	window.location.href = barurl;
	localStorage.setItem("mytime", 0);
}

