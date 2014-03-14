$(document).ready(function (e) {
    $('#btnReset').on('click', function () {
        marker.setMap(null);
        window.setTimeout(function () { map.panTo(new google.maps.LatLng($('#txtLatitude').val(), $('#txtLongitude').val())); }, 1000);
    });
    $('#txtLatitude, #txtLongitude').on('change', function (e) {
        ShowClickedLocation($('#txtLatitude').val(), $('#txtLongitude').val());
    });

    function ShowCurrentShopLocation(map) {
        var currLng = $('#txtLongitude').val();
        var currLat = $('#txtLatitude').val();

        // Adding a marker to the map
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(currLat, currLng),
            map: map,
            title: 'Current Shop Location is here.',
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
        title: 'New Shop Location will be changed here.',
        draggable: true,
        raiseOnDrag: false,
        animation: google.maps.Animation.DROP
    });
    google.maps.event.addListener(map, 'click', function (e) {
        ShowClickedLocation(e.latLng.lat(), e.latLng.lng());
        marker.setMap(map);
        window.setTimeout(function () { map.panTo(marker.getPosition()); }, 3000);

    });
    google.maps.event.addListener(marker, 'drag', function (e) {
        ShowClickedLocation(e.latLng.lat(), e.latLng.lng());
    });
    google.maps.event.addListener(marker, 'dragend', function (e) {

        window.setTimeout(function () { map.panTo(marker.getPosition()); }, 3000);

    });
    //Current shop Location
    ShowCurrentShopLocation(map);
    // Creating a LatLngBounds object
    var bounds = new google.maps.LatLngBounds();
    //Extending the map for current loction [In case of no any location got to map, current location will be shown]
    bounds.extend(new google.maps.LatLng($('#txtLatitude').val(), $('#txtLongitude').val()));
});