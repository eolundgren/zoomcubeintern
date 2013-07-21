(function (ns, api, undefined) {

    ns.DemoEditEventViewModel = function (obj) {

        if (!obj)
            obj = { name: '', description: '', tags: [] };

        this.name = ko.observable(obj.name);
        this.description = ko.observable(obj.description);
        this.tags = ko.observableArray(obj.tags);

        this.addTag = function (viewModel, e) {
            var elm = (e.srcElement || e.currentTarget)
              , tag = elm.value;

            if (tag.length > 0)
                viewModel.tags.push(tag);

            elm.value = '';
        };

        this.removeTag = function (viewModel, e) {
            viewModel.tags.remove(e);
        };

        this.toObj = function () {
            return {
                name: this.name(),
                description: this.description(),
                tags: this.tags()
            };
        }

    };

    ns.DemoEventViewModel = function () {

        this.editingEvent = ko.observable();

        this.newEvent = function (viewModel, e) {
            this.editingEvent(new ns.DemoEditEventViewModel());
        };

        this.saveChanges = function (viewModel, e) {
            alert('saving...');
        };

    };

    $().ready(function () {

        ko.applyBindings(new ns.DemoEventViewModel());

    });

})(zoomcube, zoomcube.api);