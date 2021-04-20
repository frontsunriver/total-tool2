 $(function () {
    "use strict";
     //lazy load
      $(".lazy").unveil(200, function() {
          $(this).load(function() {
            this.style.opacity = 1;
        });
      }); 
    var origin, destination, map;
    if (localStorage.getItem('location') != '') {
        var location= localStorage.getItem('location');
        var city= localStorage.getItem('city');

        $('#location_input').val(location);
        $('#city').val(city);
        $('#lat').val(localStorage.getItem('lat'));
        $('#long').val(localStorage.getItem('long'));
    }

    LocationInput();
});

    // get current Position
    $('#locationIcon').on('click',function(){
        GetMyCurrentLocation();

    });

    function GetMyCurrentLocation(){
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(setCurrentPosition);
        } else {
            alert("Geolocation is not supported by this browser.")
        }
    }

    function LocationInput() {
     var to_places = new google.maps.places.Autocomplete(document.getElementById('location_input'));
     var place = to_places.getPlace();

     google.maps.event.addListener(to_places, 'place_changed', function () {

        var to_place = to_places.getPlace(); 
        var to_address = to_place.formatted_address;
        $('#location_input').val(to_address);
        $('#district').val(to_places.getPlace().address_components[2]['long_name'])
        localStorage.setItem('location',to_address);
        localStorage.setItem('district',to_places.getPlace().address_components[2]['long_name']);
        var place = to_places.getPlace();

        localStorage.setItem('lat',place.geometry.location.lat());
        localStorage.setItem('long',place.geometry.location.lng());

        $('#lat').val(place.geometry.location.lat());
        $('#long').val(place.geometry.location.lng());

        var geocoder = new google.maps.Geocoder();
        var latlng = {lat: parseFloat(place.geometry.location.lat()), lng: parseFloat(place.geometry.location.lng())};
        setcity(latlng);

    });
 }


    // get formatted address based on current position and set it to input
    function setCurrentPosition(pos) {
        var geocoder = new google.maps.Geocoder();
        var latlng = {lat: parseFloat(pos.coords.latitude), lng: parseFloat(pos.coords.longitude)};
        
        localStorage.setItem('lat',latlng.lat);
        localStorage.setItem('long',latlng.lng);

        geocoder.geocode({ 'location' :latlng  }, function (responses) {

            if (responses && responses.length > 0) {
               $('#location_input').val(responses[0].formatted_address)
               $('#district').val(responses[1].address_components[3]['long_name'])
               localStorage.setItem('location',responses[0].formatted_address);
               localStorage.setItem('district',responses[1].address_components[3]['long_name']);

               $('#lat').val(latlng.lat);
               $('#long').val(latlng.lng);
               setcity(latlng);

           } else {
            alert("Cannot determine address at this location.")
        }
    });
    }


    function setcity(latlng,lat=false) {
       new google.maps.Geocoder().geocode({'latLng' : latlng}, function(results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                var country = null, countryCode = null, city = null, cityAlt = null;
                var c, lc, component;
                for (var r = 0, rl = results.length; r < rl; r += 1) {
                    var result = results[r];

                    if (!city && result.types[0] === 'locality') {
                        for (c = 0, lc = result.address_components.length; c < lc; c += 1) {
                            component = result.address_components[c];

                            if (component.types[0] === 'locality') {
                                city = component.long_name;
                                break;
                            }
                        }
                    }
                    else if (!city && !cityAlt && result.types[0] === 'administrative_area_level_1') {
                        for (c = 0, lc = result.address_components.length; c < lc; c += 1) {
                            component = result.address_components[c];

                            if (component.types[0] === 'administrative_area_level_1') {
                                cityAlt = component.long_name;
                                break;
                            }
                        }
                    } else if (!country && result.types[0] === 'country') {
                        country = result.address_components[0].long_name;
                        countryCode = result.address_components[0].short_name;
                    }

                    if (city && country) {
                        break;
                    }
                }
                $('#city').val(city);
                localStorage.setItem('city',city);

            }
        }
    });
   }