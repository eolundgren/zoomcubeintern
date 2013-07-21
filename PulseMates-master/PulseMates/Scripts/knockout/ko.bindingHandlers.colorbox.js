ko.bindingHandlers.colorbox = {
    init: function (elm, valueAccessor, allBindings, viewModel, context) {
        var options = ko.utils.unwrapObservable(valueAccessor())
          , local = ko.utils.extend(ko.bindingHandlers.colorbox.defaults, options)
          , local = ko.utils.unwrapObservable(local);

        $(elm).colorbox(local);
    },
    defaults: {
        tilte: '',
        href: '',
        photo: true,
        rel: false,
        // there is an issue with photos now scaling, see: http://www.jacklmoore.com/colorbox/
        width: '80%',
        heigth: '80%',
        maxWidth: '80%',
        maxHeight: '80%',
        innerWidth: '75%',
        innerHeight: '75%'
    }
};