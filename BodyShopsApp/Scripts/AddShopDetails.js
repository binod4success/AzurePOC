$(document).ready(function (e) {
    //Remove this code in production release
    $('#txtLatitude').val('32.71533');
    $('#txtLongitude').val('-117.15726');

    function getPosition(position) {
        //$('#txtLatitude').val(position.coords.latitude);
        //$('#txtLongitude').val(position.coords.longitude);
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
        navigator.geolocation.getCurrentPosition(getPosition, showError);
    }
    else {
        alert("Geolocation is not supported by this browser.");
    }

    $('#txtLatitude, #txtLongitude').on('change', function (e) {
        ShowClickedLocation($('#txtLatitude').val(), $('#txtLongitude').val());
    });

    function ShowCurrentUserLocation(map) {
        var currLng = $('#txtLongitude').val();
        var currLat = $('#txtLatitude').val();

        // Adding a marker to the map
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(currLat, currLng),
            map: map,
            title: 'You are here!',
            animation: google.maps.Animation.BOUNCE,
            icon: 'http://gmaps-samples.googlecode.com/svn/trunk/markers/green/blank.png'
        });
    }

    function ShowClickedLocation(lat, lng) {
        document.getElementById("txtLatitude").value = lat;
        document.getElementById("txtLongitude").value = lng;
        marker.setPosition(new google.maps.LatLng(lat, lng, false));
    }

    var options = {
        zoom: 8,
        center: new google.maps.LatLng($('#txtLatitude').val(), $('#txtLongitude').val()),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    google.maps.visualRefresh = true;
    var map = new google.maps.Map(document.getElementById('map'), options);

    var marker = new google.maps.Marker({
        map: map,
        title: 'New Shop Location will be pinned here.',
        draggable: true,
        raiseOnDrag: false,
        animation: google.maps.Animation.DROP
    });
    google.maps.event.addListener(map, 'click', function (e) {
        ShowClickedLocation(e.latLng.lat(), e.latLng.lng());
        window.setTimeout(function () { map.panTo(marker.getPosition()); }, 3000);
    });

    google.maps.event.addListener(marker, 'drag', function (e) {
        ShowClickedLocation(e.latLng.lat(), e.latLng.lng());
    });

    google.maps.event.addListener(marker, 'dragend', function (e) {
        window.setTimeout(function () { map.panTo(marker.getPosition()); }, 3000);
    });

    //Current shop Location
    ShowCurrentUserLocation(map);
    // Creating a LatLngBounds object
    var bounds = new google.maps.LatLngBounds();
    //Extending the map for current loction [In case of no any location got to map, current location will be shown]
    bounds.extend(new google.maps.LatLng($('#txtLatitude').val(), $('#txtLongitude').val()));
});