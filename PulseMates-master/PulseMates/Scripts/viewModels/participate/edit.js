/// <reference path="jquery-1.8.3.js" />
/// <reference path="jquery-ui-1.9.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.1.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
(function (ns, win, doc, undefined) {

    $().ready(function () {
        var id = $('#datasource-id').val()
          , $form = $('#participate-edit')
          , $body = $form.find('tbody');

        $.validator.unobtrusive.parse($form);

        $('a[href="#add-row"]').live('click', function (e) {
            e.preventDefault();

            var index = $body.find('tr').length
              , clone = $body.find('tr:last').clone(true);

            // update name and ids
            var inputs = clone.find(':input').each(function () {
                this.name = this.name.replace(/\d+/, index);
                this.id = this.id.replace(/\d+/, index);
            });

            $(inputs[2]).val(inputs[2].checked ? "true" : "false");
            $(inputs[3]).val(inputs[2].checked ? "false" : "true");

            $body.find('tr:last').after(clone);
            return false;
        });

        $('a[href="#del-row"]').live('click', function (e) {
            e.preventDefault();

            $(this).parent().parent().remove();
            return false;
        });

        $form.submit(function (e) {
            $form.removeData('validator');
            $form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse($form);

            if ($form.valid()) {

                var data = {

                    name: $('#Name').val(),
                    description: $('#Description').val(),
                    definitions: $.map($body.find('tr'), function(e) {

                        // the checkbox also has a hidden field that we will 
                        // use instead.
                        var input = $(e).find(':input');

                        return {
                            id: input[5].value,
                            name: input[0].value,
                            usage: input[1].value,
                            required: input[2].checked ? true : false,
                            typeName: input[4].value
                        };

                    })

                };

                ns.Dataset.Edit(id, data, function (e) {
                    alert(e);
                });

                return false;
            }

            return false;
        });

    });


})(PulseMates || (PulseMates.Participate = {}), window, document);