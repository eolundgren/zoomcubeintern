ko.bindingHandlers.enter = {
    init: function (element, valueAccessor, allBindings, viewModel, context) {

        var callback = ko.utils.unwrapObservable(valueAccessor())
            , elm = $(element);

        elm.unbind('keydown').bind('keydown', function (e) {

            if (e.keyCode === 13) {
                callback.apply(viewModel, [viewModel, e]);
                elm.val('');

                return false;
            }

        });
    }
};