(function (ns, api, undefined) {

    ns.ItemViewModel = function (obj) {

    };

    ns.MapMarkerViewModel = function (obj) {
        this.title = ko.observable(obj.name);
        this.location = ko.observable([
            obj.location.latitude,
            obj.location.longitude
        ]);
        this.imageUrl = '/api/item/' + obj.id + '?format=image&size=tiny';
    };

    ns.IndexViewModel = function () {
        this.markers = ko.observableArray();
        this.items = ko.observableArray();

        api.items.all(0, [], function (e) {

            var mappedMarkers = ko.utils.arrayMap(e.rows, function (i) {
                return new ns.MapMarkerViewModel(i);
            });

            //var mappedItems = ko.utils.arrayMap(e.rows, function (i) {
            //    return new ns.ItemViewModel(i);
            //});

            this.items(e.rows);
            this.markers(mappedMarkers);

        }, this);

    };


    $().ready(function () {

        var viewModel = new ns.IndexViewModel();
        ko.applyBindings(viewModel);

        $('#map-toggle a').click(function () {

            var $map = $('#map-canvas')
              , $btn = $(this).parent()
              , $text = $(this).find('.text')
              , $main = $('#main')
              , height = $map.height();

            if ($map.css('visibility') === 'hidden') {
                $map.css({ visibility: 'visible' });
                $text.text('Hide map');
                $main.css({ bottom: height + 'px' });
                $btn.css({ bottom: height - $btn.outerHeight() + 'px' });
            }
            else {
                $map.css({ visibility: 'hidden' });
                $text.text('Show on map');
                $main.css({ bottom: '0px' });
                $btn.css({ bottom: '0px' });
            }

            return false;
        });

    });

})(zoomcube, zoomcube.api);