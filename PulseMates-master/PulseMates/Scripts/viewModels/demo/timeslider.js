(function (ns, api, undefined) {

    // parse a date in yyyy-mm-dd format
    function parseDate(input) {
        var parts = input.split('-');
        // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
        return Date.parse(new Date(parts[0], parts[1] - 1, parts[2])); // months are 0-based
    }

    ns.DemoTimeslider = function () {

        this.currentTime = ko.observable(new Date());
        this.content = ko.observableArray();
        this.distinctGroups = ko.observableArray();

        this.upperBound = ko.computed(function () {

            var c = this.content();

            if (c.length)
                return parseDate(c[0].group);

        }, this);

        this.lowerBound = ko.computed(function () {

            var c = this.content();

            if (c.length)
                return parseDate(c[c.length - 1].group);

        }, this);

        this.increment = ko.observable();

        api.items.group(function (e) {

            var contentArr = []
              , distinctGroups = [];

            ko.utils.arrayForEach(e, function (g) {

                distinctGroups.push(g.name);

                ko.utils.arrayForEach(g.nodes, function (i) {
                    i.group = g.name;
                    contentArr.push(i);
                });

            });

            this.content(contentArr);
            this.distinctGroups(distinctGroups);

        }, this);

    };

    $().ready(function () {

        ko.applyBindings(new ns.DemoTimeslider());

    });

})(zoomcube, zoomcube.api);