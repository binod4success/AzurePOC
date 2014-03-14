$(document).ready(function (e) {

    //show navigation pane
    $("#navigation").hide();
    $('#directions').addClass('ui-disabled');
    var directionsDisplay = null;

    function showPosition(position) {
        //$('#hfLatitude').val(position.coords.latitude);
        //$('#hfLongitude').val(position.coords.longitude);
        $('#hfLatitude').val('32.71533');
        $('#hfLongitude').val('-117.15726');
    }

    function showError(error) {
        switch (error.code) {
            case error.PERMISSION_DENIED:
                alert("User denied the request for Geolocation.");
                break;
            case error.POSITION_UNAVAILABLE:
                alert("Location information is unavailable.");
                break;
            case error.TIMEOUT:
                alert("The request to get user location timed out.");
                break;
            case error.UNKNOWN_ERROR:
                alert("An unknown error occurred.");
                break;
        }
    }

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    }
    else {
        alert("Geolocation is not supported by this browser.");
    }

    function ShowCurrentLocationOnMap(map) {
        var currLatd = $('#hfLatitude').val();
        var currLong = $('#hfLongitude').val();

        // Adding a marker to the map
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(currLatd, currLong),
            map: map,
            title: 'You are here..',
            animation: google.maps.Animation.BOUNCE,
            icon: 'http://gmaps-samples.googlecode.com/svn/trunk/markers/green/blank.png'
        });

        // Creating an InfoWindow with the content text"
        infowindow = new google.maps.InfoWindow({
            content: '<span>You are here..!</span>'
        });

        // Adding a click event to the marker
        google.maps.event.addListener(marker, 'click', function () {
            // Calling the open method of the infoWindow
            infowindow.open(map, marker);
        });
    }

    function panel() {
        $(document).on("pageshow", "#pagetwo", function () {
            directionsDisplay.setPanel(document.getElementById('directions-panel'));
        });
    }

    $('#btnGo').on('click', function (e) {
        if (!$('#frmShop').valid()) {
            return false;
        } else {
            $('div[data-valmsg-summary]').removeClass('validation-summary-errors').addClass('validation-summary-valid');
            $('#divMsg').hide();
        }
        var places = [];

        var infowindow = new google.maps.InfoWindow();
        $('#directions-panel').empty();
        $.ajax({
            url: $('#frmShop').attr('action'),
            data: $('#frmShop').serialize(),
            type: 'Get',
            cache: false
        }).done(function (data) {
            //show navigation pane
            $("#navigation").show();

            // Creating a MapOptions object with the required properties
            var options = {
                zoom: 5,
                center: new google.maps.LatLng($('#hfLatitude').val(), $('#hfLongitude').val()),
                provideRouteAlternatives: true,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            // Creating the map
            var map = new google.maps.Map(document.getElementById('map'), options);

            //specification for drawing circle
            var CircleOptions = {
                strokeColor: '#5858FA',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#D0F5A9',
                fillOpacity: 0.35,
                map: map,
                center: new google.maps.LatLng($('#hfLatitude').val(), $('#hfLongitude').val()),
                radius: $('#range').val() * 1000
            };

            var Circle = new google.maps.Circle(CircleOptions);

            //Current Location Marker                    
            ShowCurrentLocationOnMap(map);

            // Creating a LatLngBounds object
            var bounds = new google.maps.LatLngBounds();
            //Extending the map for current loction [In case of no any location got to map, current location will be shown]
            bounds.extend(new google.maps.LatLng($('#hfLatitude').val(), $('#hfLongitude').val()));

            //On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                places.push(new google.maps.LatLng(item.ShopGeoLocation.Latitude, item.ShopGeoLocation.Longitude));
            });
            var markerImage = {
                url: 'Images/mapLocationMarkerImage.png',
                // This marker is 20 pixels wide by 32 pixels tall.
                size: new google.maps.Size(50, 50),
            };
            for (var i = 0; i < places.length; i++) {
                // Adding the markers
                var marker = new google.maps.Marker({
                    position: places[i],
                    map: map,
                    //icon: markerImage,
                    title: data[i].Name
                });
                // Wrapping the event listener inside an anonymous function
                // that we immediately invoke and passes the variable i to.
                (function (i, marker) {
                    // Creating the event listener. It now has access to the values of
                    // i and marker as they were during its creation
                    google.maps.event.addListener(marker, 'mouseover', function () {
                        var contentString = '<div style="width:auto"><strong>' + data[i].Name + '<strong>' +
                         '<div>Address: ' + data[i].AddressLine1 + '</div>' +
                         '<div>' + data[i].AddressLine1 + '</div>' +
                         '<div>' + data[i].AddressLine2 + '</div>' +
                         '<div>' + data[i].City + ', ' + data[i].State + ' [ ' + data[i].Country + ' ]' + '</div>' +
                         '<div>Pin: ' + data[i].Pin + '</div>' +
                         '<div>Contact: ' + data[i].Contact + '</div><a href="tel:' + data[i].Contact + '"><image src="Images/call.png" style="height:14px; width:14px"></image> Make a call</a></div>';

                        // Check to see if we already have an InfoWindow
                        if (!infowindow) {
                            InfoWindow = new google.maps.InfoWindow();
                        }
                        // Setting the content of the InfoWindow
                        infowindow.setContent(contentString);
                        // Tying the InfoWindow to the marker
                        infowindow.open(map, marker);
                    });
                })(i, marker);

                // Extending the bounds object with each LatLng
                bounds.extend(places[i]);

                (function (i, marker) {
                    google.maps.event.addListener(marker, 'click', function () {
                        $('#directions').removeClass('ui-disabled');
                        var directionsService = new google.maps.DirectionsService();
                        var start = new google.maps.LatLng($('#hfLatitude').val(), $('#hfLongitude').val());
                        var end = new google.maps.LatLng(data[i].ShopGeoLocation.Latitude, data[i].ShopGeoLocation.Longitude);
                        if (directionsDisplay != null) {
                            directionsDisplay.setMap(null);
                            directionsDisplay.setPanel(null);
                            directionsDisplay = null;
                        }
                        var polylineOptionsActual = new google.maps.Polyline({
                            strokeColor: '#3a79c8',
                            strokeOpacity: 1.0,
                            strokeWeight: 3
                        });
                        var rendererOptions = {
                            polylineOptions: polylineOptionsActual,
                            suppressMarkers: true
                        }
                        directionsDisplay = new google.maps.DirectionsRenderer(rendererOptions);
                        directionsDisplay.setMap(map); // map should be already initialized.
                        directionsDisplay.setPanel(document.getElementById('directions-panel'));
                        //var selectedMode = document.getElementById("mode").value;

                        panel();

                        var request = {
                            origin: start,
                            destination: end,
                            //travelMode: google.maps.TravelMode[selectedMode]
                            travelMode: google.maps.TravelMode.DRIVING
                        };
                        directionsService.route(request, function (response, status) {
                            if (status == google.maps.DirectionsStatus.OK) {

                                //directionsDisplay.setDirections(null);
                                directionsDisplay.setDirections(response);
                            }
                        });
                    });

                })(i, marker);
            }
            // Adjusting the map to new bounding box
            map.fitBounds(bounds);
        }).error(function (jqXHR, textStatus, errorThrown) {
            $('#divMsg').html(jqXHR.getResponseHeader('errorMsg')).addClass('validation-summary-errors').show();

            // Creating a MapOptions object with the required properties
            var options = {
                zoom: 5,
                center: new google.maps.LatLng($('#hfLatitude').val(), $('#hfLongitude').val()),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            // Creating the map
            var map = new google.maps.Map(document.getElementById('map'), options);
            //Current Location Marker                    
            ShowCurrentLocationOnMap(map);

            // Creating a LatLngBounds object
            var bounds = new google.maps.LatLngBounds();
            //Extending the map for current loction [In case of no any location got to map, current location will be shown]
            bounds.extend(new google.maps.LatLng($('#hfLatitude').val(), $('#hfLongitude').val()));

            // Adjusting the map to new bounding box
            map.fitBounds(bounds);
        });
        return false;
    });
});