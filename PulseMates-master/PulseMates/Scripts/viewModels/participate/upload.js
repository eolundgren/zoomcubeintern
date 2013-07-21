/// <reference path="jquery-1.8.3.js" />
/// <reference path="jquery-ui-1.9.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.1.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
(function (ns, win, doc, undefined) {

    function formData(form)
    {
        var $form = $(form)
          , $inputs = $form.find(':input');




    }

    $().ready(function () {

        $.validator.unobtrusive.parse('#participate-upload');

        $('#participate-upload').submit(function (e) {
            var $form = $(this);

            if ($form.valid()) {
                return true;
            }
            
            return false;

        });

    });

})(PulseMates || (PulseMates.Participate = {}), window, document);