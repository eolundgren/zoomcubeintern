(function (doc, win, undefined) {

    // Templates used to render the grid
    var templateEngine = new ko.nativeTemplateEngine();

    templateEngine.addTemplate = function (templateName, templateMarkup) {
        doc.write('<script id="' + templateName + '" type="text/html">' + templateMarkup + '</script>');
    };

    templateEngine.render = function (templateName, viewModel, container) {
        ko.renderTemplate(templateName, viewModel, { templateEngine: templateEngine }, container, 'replaceNode');
    };

    templateEngine.addTemplate('mosaic-master-template', '\
        <!-- ko foreach: columns -->\
        <ul class="mosaic-column thumbnails" data-bind="template: { name: $parent.template, foreach: $data }, style: { width: $parent.columnWidth }"></ul>\
        <!-- /ko -->');

    var MosaicViewModel = function (dimension, options) {

        this.template = ko.utils.unwrapObservable(options.template);
        this.columnWidth = ko.observable(dimension.columnWidth + 'px');
        this.columns = ko.observableArray()
            .toColumn(options.tiles, dimension.columns);

        this.maxRowCount = ko.computed(function () {
            // the first columns is always the biggest.
            var c = this.columns();
            return c[0] ? c[0].length : 0;
            
        }, this);

        this.itemsInRow = function (index) {
            if (index > 0 && index < this.maxRowCount())
                return [];

            return options.tiles.slice(index * dimension.columns, dimension.columns);
        };

    };

    var calculateColumnDimenation = function (elm, options) {

        var maxColumnWidth = ko.utils.unwrapObservable(options.maxColumnWidth)
          , maxColumns = ko.utils.unwrapObservable(options.maxColumns);

        var innerWidth = $(elm).innerWidth()
          , columns = Math.ceil(innerWidth / maxColumnWidth);

        if (maxColumns > 0 && columns > maxColumns)
            columns = maxColumns;

        // minus 20? can seems to get the resize working properly
        // something with padding and margin.
        var columnWidth = Math.floor((innerWidth - 20) / columns);

        return {
            columns: columns,
            columnWidth: columnWidth
        };
    };

    ko.bindingHandlers.mosaic = {

        init: function (elm, valueAccessor, allBindings, viewModel, context) {
            return { controlsDescendantBindings: true };
        },
        update: function (elm, valueAccessor, allBindings, viewModel, context) {

            var options = ko.utils.unwrapObservable(valueAccessor())
              , local = ko.utils.extend(ko.bindingHandlers.mosaic.defaults, options)
              , $elm = $(elm);

            $elm.empty();

            if (!$elm.hasClass('mosaic-container'))
                $elm.addClass('mosaic-container');

            if (!viewModel.mosaic) {
                
                var dim = calculateColumnDimenation(elm, local);
                viewModel.mosaic = new MosaicViewModel(dim, local);

            }

            // Render the Mosaic
            var mosaicContainer = elm.appendChild(doc.createElement("DIV"));
            ko.renderTemplate(local.masterTemplate, viewModel.mosaic, { templateEngine: templateEngine }, mosaicContainer, 'replaceNode');

            return { controlsDescendantBindings: true };
        },
        defaults: {
            maxColumns: 0,
            maxColumnWidth: 256,
            template: 'mosaic-item-template',
            masterTemplate: 'mosaic-master-template',
            tiles: []
        }

    };

})(document, window);