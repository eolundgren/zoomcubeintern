(function (api, ns, undefined) {

    ns.DemoEditPageViewModel = function (obj) {

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

    ns.DemoPageViewModel = function () {
        
        this.pages = ko.observableArray();
        this.currentPage = ko.observable();

        this.items = ko.observableArray();

        this.editingPage = ko.observable();

        this.newPage = function (viewModel, e) {
            this.editingPage(new ns.DemoEditPageViewModel({
                name: '', description: '', tags: []
            }));
        };

        this.saveChanges = function (viewModel, e) {
            var obj = viewModel.toObj();

            api.pages.save(obj, function (e) {
                this.pages.push(e);
                this.editingPage(null);
            }, this);
        };

        this.currentPage.subscribe(function (newPage) {

            this.items([]);

            if (newPage)
                api.pages.items(newPage.id, function (e) { this.items(e); }, this);

        }, this);

        api.pages.all(function (e) { this.pages(e); }, this);
    };

    $().ready(function () {

        ko.applyBindings(new ns.DemoPageViewModel());

    });


})(zoomcube.api, zoomcube);