  "use strict";
  function initialize() {

    var infoWindow = '',
    addressEl = document.querySelector('#location_input'),
    latEl = document.querySelector( '#latitude' ),
    longEl = document.querySelector( '#longitude' ),
    element = document.getElementById( 'map-canvas' );
    city = document.querySelector( '#city' );

    mapOptions = {
    // How far the maps zooms in.
    zoom: 12,
    
    // Current Lat and Long position of the pin/
    center: new google.maps.LatLng( lati, longlat),
    
    disableDefaultUI: false, // Disables the controls like zoom control on the map if set to true
    scrollWheel: true, // If set to false disables the scrolling on the map.
    draggable: true, // If set to false , you cannot move the map around.
    // mapTypeId: google.maps.MapTypeId.HYBRID, // If set to HYBRID its between sat and ROADMAP, Can be set to SATELLITE as well.
     maxZoom: 21, // Wont allow you to zoom more than this


 };

  /**
   * Creates the map using google function google.maps.Map() by passing the id of canvas and
   * mapOptions object that we just created above as its parameters.
   *
   */
  // Create an object map with the constructor function Map()
  map = new google.maps.Map( element, mapOptions ); // Till this like of code it loads up the map.

  /**
   * Creates the marker on the map
   *
   */
   marker = new google.maps.Marker({
    position: mapOptions.center,
    map: map,
    draggable: true
   });

  /**
   * Creates a search box
   */
   searchBox = new google.maps.places.SearchBox( addressEl );

  /**
   * When the place is changed on search box, it takes the marker to the searched location.
   */
   google.maps.event.addListener( searchBox, 'places_changed', function () {
    var places = searchBox.getPlaces(),
    bounds = new google.maps.LatLngBounds(),
    i, place, lat, long, resultArray,
    addresss = places[0].formatted_address;

    for( i = 0; place = places[i]; i++ ) {
      bounds.extend( place.geometry.location );
        marker.setPosition( place.geometry.location );  // Set marker position new.
    }

    map.fitBounds( bounds );  // Fit to the bound
    map.setZoom( 15 ); // This function sets the zoom to 15, meaning zooms to level 15.
    

    lat = marker.getPosition().lat();
    long = marker.getPosition().lng();
    latEl.value = lat;
    longEl.value = long;
    localStorage.setItem('lat',lat);
    localStorage.setItem('long',long);
    localStorage.setItem('location',$('#location_input').val());

    resultArray =  places[0].address_components;

    // Get the city and set the city input value to the one selected
//     for( var i = 0; i < resultArray.length; i++ ) {
//       if ( resultArray[ i ].types[0] && 'administrative_area_level_2' === resultArray[ i ].types[0] ) {
//         citi = resultArray[ i ].long_name;
//         //city.value = citi;
        
//     }
// }

    // Closes the previous info window if it already exists
    if ( infoWindow ) {
      infoWindow.close();
    }
    /**
     * Creates the info Window at the top of the marker
     */
     infoWindow = new google.maps.InfoWindow({
      content: addresss
     });

     infoWindow.open( map, marker );

     calculatearea();
 } );


  /**
   * Finds the new position of the marker when the marker is dragged.
   */
   google.maps.event.addListener( marker, "dragend", function ( event ) {
    var lat, long, address, resultArray, citi;


    lat = marker.getPosition().lat();
    long = marker.getPosition().lng();

    var geocoder = new google.maps.Geocoder();
    geocoder.geocode( { latLng: marker.getPosition() }, function ( result, status ) {
      if ( 'OK' === status ) {
        address = result[0].formatted_address;
        resultArray =  result[0].address_components;


        // Get the city and set the city input value to the one selected
        for( var i = 0; i < resultArray.length; i++ ) {
          if ( resultArray[ i ].types[0] && 'administrative_area_level_2' === resultArray[ i ].types[0] ) {
            citi = resultArray[ i ].long_name;

          }
        }
        addressEl.value = address;
        latEl.value = lat;
        longEl.value = long;

        localStorage.setItem('lat',lat);
        localStorage.setItem('long',long);
        localStorage.setItem('location',address);

    } else {
      alert( 'Geocode was not successful for the following reason: ' + status );
    }

      // Closes the previous info window if it already exists
      if ( infoWindow ) {
        infoWindow.close();
      }

      /**
       * Creates the info Window at the top of the marker
       */
       infoWindow = new google.maps.InfoWindow({
        content: address
       });

       infoWindow.open( map, marker );

       calculatearea()

   } );

   });

   calculatearea()

   $('#location_input').on('focusout',()=>{
     calculatearea();
         //alert('changeds')
       });

   var origin, destination;

   function calculatearea() {
    var origin = $('#location_input').val();
    var destination = resturentlocation;
    var travel_mode = "DRIVING";
    var directionsDisplay = new google.maps.DirectionsRenderer({'draggable': false});
    var directionsService = new google.maps.DirectionsService();
    displayRoute(travel_mode, origin, destination, directionsService, directionsDisplay);
    calculateDistance(travel_mode, origin, destination);
   }


   function displayRoute(travel_mode, origin, destination, directionsService, directionsDisplay) {
    directionsService.route({
      origin: origin,
      destination: destination,
      travelMode: travel_mode,
      avoidTolls: true
    }, function (response, status) {
      if (status === 'OK') {
                  
                  directionsDisplay.setDirections(response);
                } else {
                  directionsDisplay.setMap(null);
                  directionsDisplay.setDirections(null);
                  $('#msg').show();
                  var msg=$('#msg').html();
                    //alert(msg);
                }
            });
   }


// calculate distance , after finish send result to callback function
function calculateDistance(travel_mode, origin, destination) {

  var DistanceMatrixService = new google.maps.DistanceMatrixService();
    DistanceMatrixService.getDistanceMatrix(
    {
      origins: [origin],
      destinations: [destination],
      travelMode: google.maps.TravelMode[travel_mode],
      unitSystem: google.maps.UnitSystem.IMPERIAL,                     
      avoidHighways: false,
      avoidTolls: false
    }, save_results);
  }

     // save distance results
     function save_results(response, status) {

      if (status != google.maps.DistanceMatrixStatus.OK) {
        $('#result').html(err);
      } else {
        var origin = response.originAddresses[0];
        var destination = response.destinationAddresses[0];
        if (response.rows[0].elements[0].status === "ZERO_RESULTS") {
          $('#result').html("Sorry , not available to use this travel mode between " + origin + " and " + destination);
        } else {
          var distance = response.rows[0].elements[0].distance;
          var duration = response.rows[0].elements[0].duration;
                    var distance_in_kilo = distance.value / 1000; // the kilo meter
                    var distance_in_mile = distance.value / 1609.34; // the mile
                    var duration_text = duration.text;
                    

                    var totalfee=feePerkilo*distance_in_kilo;
                    var amount=$('#total_amount').val();
                    var amount=amount.replace(/,/g, ""); 
                    var total_amount = parseFloat(amount);
                    //alert(total_amount)
                    $('#delivery_fee').html(parseFloat(Math.round(totalfee)));
                    $('#shipping').val(parseFloat(Math.round(totalfee)));
                    $('#total_price').val(parseFloat(Math.round(total_amount + totalfee)));
                    $('#last_total').html(parseFloat(Math.round(total_amount + totalfee)));
                }
            }
        }


    }