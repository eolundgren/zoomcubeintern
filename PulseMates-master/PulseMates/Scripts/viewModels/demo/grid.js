(function (api, ns, undefined) {

    ns.DemoGridItemViewModel = function (obj) {

        this.id = obj.id;
        this.serverObj = obj;

        this.name = ko.observable(obj.name);
        this.desc = ko.observable(obj.description);
        this.tags = ko.observableArray(obj.tags);

        this.inputName = ko.observable();
        this.inputDesc = ko.observable();
        this.inputTags = ko.observableArray();

        this.tooltip = ko.computed(function () {
            return '<div>' + this.name() + '</br />Tags: ' + this.tags() + '</div>';
        }, this);

        this.thumbnailUrl = ko.computed(function () {
            return '/api/item/' + this.id + '?format=image&size=small';
        }, this);

        this.imageUrl = ko.computed(function () {
            return '/api/item/' + this.id + '?format=image';
        }, this);

        this.addTag = function (viewModel, e) {
            var value = (e.srcElement || e.currentTarget).value;

            if (value.length > 0)
                viewModel.inputTags.push(value);
        };

        this.removeTag = function (viewModel, e) {
            viewModel.inputTags.remove(e);
        };

        // ninja function is envoked whenever the modal is shown
        // part of the bindingHandler.
        this.shown = function (viewModel, e) {
            this.inputName(this.name());
            this.inputDesc(this.desc());
            this.inputTags(this.tags());
        };

    }

    ns.DemoGridViewModel = function () {

        this.itemsOnPage = ko.observableArray();

        this.currentItem = ko.observable();

        this.maxPageIndex = ko.observable();
        this.currentPageIndex = ko.observable(0);

        this.tags = ko.observableArray();

        this.selectedTags = ko.observableArray();

        this.selectedTags.subscribe(function (newValue) {
            loadPage.apply(this, [0, newValue]);
        }, this);

        this.currentPageIndex.subscribe(function (newIndex) {
            loadPage.apply(this, [newIndex, this.selectedTags()]);
        }, this);

        this.saveChanges = function (viewModel, e) {
            var newItem = $.extend({}, viewModel.serverObj, {
                tags: viewModel.inputTags(), name: viewModel.inputName(), description: viewModel.inputDesc()
            });

            api.items.save(viewModel.id, newItem, function (e) {

                viewModel.name(e.name);
                viewModel.desc(e.description);
                viewModel.tags(e.tags);

                // close modal...?
                this.currentItem(null);

            }, this);

        };

        this.deleteItem = function (viewModel, e) {

            api.items.remove(viewModel.id, function (e) {
                loadPage.apply(this, [this.currentPageIndex(), this.selectedTags()]);
            }, this);

        };

        function loadPage(index, tags) {
            api.items.all(index, tags, function (e) {
                var mappedItems = ko.utils.arrayMap(e.rows, function (i) { return new ns.DemoGridItemViewModel(i); });
                this.itemsOnPage(mappedItems);
                this.maxPageIndex(e.pages - 1);
            }, this);
        };

        api.tags.all(function (e) { this.tags(e); }, this);

        loadPage.apply(this, [this.currentPageIndex(), this.selectedTags()]);
    };



    $().ready(function () {

        ko.applyBindings(new ns.DemoGridViewModel());

    });


})(zoomcube.api, zoomcube);