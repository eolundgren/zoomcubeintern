
(function(undefined) {

    var unwrapMap = function (data, map) {

        if (map)
            return map(ko.utils.unwrapObservable(data));

        return ko.utils.unwrapObservable(data);
    };

    ko.observable.fn.link = function (data, map) {

        if (ko.isObservable(data)) {
            data.subscribe(function (dataHasChanged) {
                this(unwrapMap(data, map));
            }, this);
        }

        this(unwrapMap(data, map));
        return this;
    };

    ko.observableArray.fn.link = function (data, map) {

        if (ko.isObservable(data)) {
            data.subscribe(function (dataHasChanged) {
                this(unwrapMap(data, map));
            }, this);
        }

        this(unwrapMap(data, map));
        return this;
    };

})();