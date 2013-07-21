/// <reference path="jquery-1.8.3.js" />
/// <reference path="jquery-ui-1.9.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.1.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
var PulseMates = {};

(function (ns, win, doc, undefined) {

    var bingMapKey = 'AkEWCEOryJWJhxn6Pa5dJmDZrmSglTBtc5_-5YI2e3SQ045c5CHI4ASY0sJ65rOT';

    ns.Dataset = function() {
       
        var uuid = 0;
        ///
        /// 
        var _error = function(e) {
            alert(e.responseText);
        };
        ///
        /// 
        var _edit = function (id, data, callback) {
            $.ajax({
                url: '/api/datasource/' + id,
                type: 'put',
                data: data,
                success: callback,
                error: _error
            });
        };
        ///
        /// 
        var _getItems = function (id, callback) {
            $.ajax({
                url: '/api/datasource/' + id + '/value',
                type: 'get',
                success: function (e) {
                    var map = $.map(e, function (x) { return new ns.DatasetValue(x); });
                    callback(map);
                },
                error: _error
            });
        }

        var _async = function(url, element, callback) {
            var iframeName = 'pulsemates_' + ++uuid
              , iframe = $('<iframe name="' + iframeName + '" style="position:absolute;top:-9999px;" />')
                .appendTo('body')
              , form = '<form target="' + iframeName + '" action="' + url + '" method="post" enctype="multipart/form-data" />'
              , json = {};

            form = $(element).wrapAll(form).parent('form');
            form.submit(function () {

                iframe.load(function () {

                    try {
                        var c = $(iframe.contents().get(0)).find('body').text();
                        json = JSON.parse(c);
                    }
                    catch (err) {
                        alert(err);
                    }

                    setTimeout(function () {
                        iframe.remove();

                        if (callback)
                            callback(json);
                    });
                });

            }).submit();
        }
        var _create = function(data, callback) {
            $.ajax({
                url: '/api/datasource',
                type: 'post',
                data: data,
                success: callback,
                error: _error
            });
        };
        var _upload = function (id, input, callback) {
            _async('/api/datasource/' + id + '/value', input, callback);
        }

        return {
            Create: _create,
            Edit: _edit,
            Upload: _upload,
            Items: _getItems,
        };

    }();
    
    ns.DatasetValue = function (obj) {
        var self = this;

        this.id = obj.Id;
        $.each(obj.Values, function (i, e) { self[e.Name] = e; });
    };

    ///
    /// private functions
    ns.ParticipateViewModel = function() {
        var self = this,
            id = $('#datasource-id').val();

        this.items = ko.observableArray([]);

        ns.Dataset.Items(id, function (e) { self.items(e); });
    };

    ns.MapViewModel = function (i, elm) {
        var self = this
          , $input = $(elm)
          , $parent = $input.parent()
          , $address = $parent.find('.editor-for-address')
          , $coords = $parent.find('.editor-for-location');

        // Create the map div and insert it into the page.
        var $map = $('<div>', { css: { position: 'relative', width: '310px', height: '200px' } })
            .insertAfter($parent.find(':last-child'));

        // Attempt to parse the lat/long coordinates out of this input element.
        var latLong = parseLatLong(this.value);
        
        var map = new Microsoft.Maps.Map($map[0], {
            credentials: bingMapKey,
            showDashboard: false,
            center: new Microsoft.Maps.Location(latLong.latitude, latLong.longitude),
            zoom: 11,
            disableBirdseye: true,
            mapTypeId: Microsoft.Maps.MapTypeId.road,
        });

        map.entities.push(new Microsoft.Maps.Pushpin(latLong, { draggable: false }));

        var searchManager = null;

        Microsoft.Maps.loadModule('Microsoft.Maps.Search', {
            callback: function () {
                searchManager = new Microsoft.Maps.Search.SearchManager(map);
            }});
        
        $address.keydown(function(e) { 
            if (e.which == 13) { self.findByAddress(this.value); return false; }
        });

        this.findByAddress = function(where, callback)
        {
            function searchCallback(response, userData) {

                if (response.parseResults.length > 0) {
                    var result = response.parseResults[0]
                      , location = result.location.location;
                    
                    $address.val(result.location.name);
                    $coords.val(location.latitude + ',' + location.longitude);

                    setPin(location);
                }

            }

            function searchError(request) {
                alert("An error occurred.");
            }

            var request = { query: where, count: 1, callback: searchCallback, errorCallback: searchError };
            searchManager.search(request);
        };

        function setPin(latLong) {
            // Add a pin to the center of the map
            var pin = new Microsoft.Maps.Pushpin(latLong, { draggable: true });

            map.entities.clear();
            map.entities.push(pin);

            // Get the existing options.
            var options = map.getOptions();

            // Set the zoom level of the map
            options.zoom = 15;
            options.center = latLong;
            map.setView(options);

        }

        function parseLatLong(value) {
            var manhatten = { latitude: 40.716948, longitude: -74.003563 };

            if (!value) {
                return manhatten;
            }

            var latLong = value.match(/-?\d+\.\d+/g);

            if (latLong) {
                return { latitude: latLong[0], longitude: latLong[1] };
            }

            return manhatten;
        };

    };

    $().ready(function () {

        var index = doc.getElementById('participate');

        if (index != null)
            ko.applyBindings(new ns.ParticipateViewModel(), index);

        $('.editor-for-location, .display-for-location').each(ns.MapViewModel);
        //$('.editor-for-address, .display-for-address').each(ns.MapViewModel);

        $('.usage').hover(function (e) {

            var $this = $(this)
              , offset = $this.offset()
              , top = offset.top + 15
              , left = offset.left + $this.outerWidth() + 12;

            $this.next().css({ position: 'absolute', left: left, top: top }).show();

        }, function (e) {
            $('.usage-message').hide();            
        });
    });

})(PulseMates || (PulseMates.Participate = {}), window, document);