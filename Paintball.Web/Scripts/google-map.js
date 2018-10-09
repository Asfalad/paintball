var mapOptions = {
    center: new google.maps.LatLng(46.408257, 30.705634, 18),
    zoom: 18
}

var mapElement = document.getElementById('map');
var map = new google.maps.Map(mapElement, mapOptions);
var locations = [
    ['Мы на карте', 46.408257, 30.705634]
];

for (i = 0; i < locations.length; i++) {
    marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[i][1], locations[i][2]),
        map: map,
        title: locations[i][0]
    });
}