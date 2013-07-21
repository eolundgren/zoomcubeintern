ko.bindingHandlers.tooltip = {
    init: function (elm, valueAccessor, allBingings, viewModel, context) {
        var options = ko.utils.unwrapObservable(valueAccessor())
          , local = ko.utils.extend(ko.bindingHandlers.tooltip.defaults, options);

        $(elm).tooltip(local);

        // Also tell KO *not* to bind the descendants itself, otherwise they will be bound twice
        //return { controlsDescendantBindings: true };
    },
    defaults: {
        placement: 'top',
        html: false,
        title: '',
        trigger: 'hover'
    }
};