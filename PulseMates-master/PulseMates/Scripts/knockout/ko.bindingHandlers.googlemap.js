

ko.bindingHandlers.googleMap = {
    _map: null,
    _markerArray: [],
    _infoArray: [],
    init: function (elm, valueAccessor, allBindings, viewModel, context) {
        var options = ko.utils.unwrapObservable(valueAccessor())
          , local = ko.utils.extend(ko.bindingHandlers.googleMap.defaults, options)
          , local = ko.utils.unwrapObservable(local);

        local.center = new google.maps.LatLng(local.center[0], local.center[1]);

        ko.bindingHandlers.googleMap._map = new google.maps.Map(elm, local);
    },
    update: function (elm, valueAccessor, allBindings, viewModel, context) {
        var options = ko.utils.unwrapObservable(valueAccessor())
          , local = ko.utils.extend(ko.bindingHandlers.googleMap.defaults, options);

        var clearMarkers = function (arr) {
            /* Remove All Markers */
            while (arr.length) {
                var marker = arr.pop();
                google.maps.event.removeListener(marker);
                marker.setMap(null);
            }
        };

        var createMarker = function (obj) {
            var pos = obj.location();
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(pos[0], pos[1]),
                map: ko.bindingHandlers.googleMap._map,
                title: obj.title(),
                info: new google.maps.InfoWindow({
                    content: '<strong>Hello, InfoWindow</strong><img src="' + obj.imageUrl + '" />'
                })
            });

            google.maps.event.addListener(marker, "click", function () {
                if (marker.map.info)
                    marker.map.info.close();
                
                marker.info.open(marker.map, marker);
                marker.map.info = marker.info;
            });

            return marker;
        };

        var setMarkers = function (arr) {

            // map: an instance of GMap3
            // latlng: an array of instances of GLatLng
            var bounds = new google.maps.LatLngBounds()
              , result = [];

            for (var i in arr) {
                var marker = createMarker(arr[i]);
                bounds.extend(marker.position);

                result.push(marker);
            }

            if (result.length > 0) {
                ko.bindingHandlers.googleMap._map.setCenter(bounds.getCenter());
                ko.bindingHandlers.googleMap._map.fitBounds(bounds);
            }

            return result;
        };

        clearMarkers(ko.bindingHandlers.googleMap._markerArray);
        ko.bindingHandlers.googleMap._markerArray = setMarkers(local.markers());
    },
    defaults: {
        key: 'AIzaSyBvU8_f5tVD5iSwk6H5S8RzUisUbEcJfGI',
        zoom: 8,
        center: [0, 0],
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        disableDefaultUI: true,
        panControl: true,
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL
        },
        map: null
    }
};
