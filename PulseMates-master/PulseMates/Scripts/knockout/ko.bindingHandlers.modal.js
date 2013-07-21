ko.bindingHandlers.modal = {
    init: function (element, valueAccessor, allBindings, vm, context) {
        var options = ko.utils.unwrapObservable(valueAccessor())
          , local = ko.utils.extend(ko.bindingHandlers.modal.defaults, options);

        //init the modal and make sure that we clear the observable no matter how the modal is closed
        $(element).modal({ show: false }).on("hidden", function () {
            if (ko.isWriteableObservable(local.context)) {
                local.context(null);
            }
        });

        var templateName = function () {
            return local.template; // 'edit-item-template';
        };

        //a computed to wrap the current modal data
        var templateViewModel = ko.computed(function () {
            return local.viewModel();
        });

        //apply the template binding to this element
        return ko.applyBindingsToNode(element, { template: { 'if': local.context, name: templateName, data: templateViewModel } }, context);
    },
    update: function (element, valueAccessor, allBindings, viewModel) {
        var data = ko.utils.unwrapObservable(valueAccessor())
          , context = data.context();

        //show or hide the modal depending on whether the associated data is populated
        $(element).modal(context ? "show" : "hide");

        if (context && context.shown) {
            context.shown.apply(context, [context, element]);
        }

    },
    defaults: {
        viewModel: null,
        template: 'modal-template'
    }
};