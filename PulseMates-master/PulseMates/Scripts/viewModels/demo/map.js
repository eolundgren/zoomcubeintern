(function(api, ns, undefined) {

    ns.DemoMapMarkerViewModel = function (obj) {
        this.title = ko.observable(obj.name);
        this.location = ko.observable([
            obj.location.latitude,
            obj.location.longitude
        ]);
        this.imageUrl = '/api/item/' + obj.id + '?format=image&size=tiny';
    };

    ns.DemoMapViewModel = function () {
        this.markers = ko.observableArray();

        api.items.all(0, [], function (e) {

            var mappedItems = ko.utils.arrayMap(e.rows, function (i) {
                return new ns.DemoMapMarkerViewModel(i);
            });

            this.markers(mappedItems);

        }, this);
    };

    $().ready(function () {

        ko.applyBindings(new ns.DemoMapViewModel());

    });


})(zoomcube.api, zoomcube);