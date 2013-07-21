

ko.bindingHandlers.highlight = {

    init: function (elm, valueAccessor, allBindings, viewModel, context) {

        var group = ko.utils.unwrapObservable(valueAccessor())
          , $elm = $(elm)
          , className = $elm.prop('class').replace(/^(\S*).*/, '$1')
          , selector = '.' + className + ':not([rel="' + group + '"])'

        $elm.attr('rel', group);

        $elm.hover(
            function (e) {
                $(selector).css({ "opacity": 0.25 });
                //$(selector).css({ backgroundColor: '#0088cc' });
            },
            function (e) {
                $(selector).css({ "opacity": 1 });
                //$(selector).css({ backgroundColor: 'transparent' });
            }
        );

    }

};