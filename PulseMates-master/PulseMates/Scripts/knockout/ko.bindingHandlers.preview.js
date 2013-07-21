(function (doc, win, undefined) {

    var findTopModalPosition = function (index) {
        var top = 0;

        $('.mosaic-column').each(function (i, e) {

            var li = $(e).find('li:eq(' + index + ')');

            if (li.length === 0)
                return;

            var pos = li.position()
              , bottom = pos.top + li.outerHeight();

            if (top < bottom)
                top = bottom;
        });

        top += $('#main').scrollTop() + 10;
        return top;
    };

    var insertBlankLi = function (index) {
        $('.mosaic-column').each(function () {

            $(this).find('li:eq(' + index + ')')
                .after($('<li class="blank">Pink Panther</li>'));

        });
    }

    // Templates used to render the grid
    var templateEngine = new ko.nativeTemplateEngine();

    ko.bindingHandlers.preview = {
        init: function (elm, valueAccessor, allBingins, viewModel, context) {
            
            if ($('#preview-modal').length === 0) {
                $('.mosaic-container').append('<div id="preview-modal" style="display: none">\
                    <button type="button" class="close" aria-hidden="true">&times;</button>\
                    <div class="arrow">&nbsp;</div>\
                    <div class="content"></div>\
                    <div class="next"></div>\
                    <div class="prev"></div>\
                </div>');

                $('#preview-modal .close').unbind('click').bind('click', function () {
                    $(this).parent().hide();
                    $('.mosaic-container .blank').remove();

                    return false;
                });

                $(window).keydown(function (e) {

                });
            }

        },
        update: function(elm, valueAccessor, allBindings, viewModel, context) {

            var options = ko.utils.unwrapObservable(valueAccessor())
              , local = ko.utils.extend(ko.bindingHandlers.preview.defaults, options)
              , $elm = $(elm);

            $elm.unbind('click').bind('click', function () {

                var modal = $('#preview-modal')
                  , content = modal.find('.content')
                  , arrow = modal.find('.arrow')
                  , mosaic = $('.mosaic-container');

                // cleaning up the modal and the container.
                content.empty();
                mosaic.find('.blank').remove();

                var index = $elm.parent().index()
                  , top = findTopModalPosition(index);

                modal.css({ top: top + 'px', left: mosaic.position().left + 'px', width: mosaic.width() + 'px' });

                insertBlankLi(index);

                // position arrow.
                var left = ($elm.offset().left - modal.position().left) + ($elm.width() / 2);
                arrow.css({ left: left + 'px' });

                modal.show();

                // ensure the modal is in view
                var scrollTo = top - (($('#main').height() - modal.height()) / 2) - 50;            
                $('#main').scrollTop(scrollTo);

                // Render the Preview
                var previewContainer = content[0].appendChild(doc.createElement("DIV"));
                ko.renderTemplate(local.template, viewModel, { templateEngine: templateEngine }, previewContainer, 'replaceNode');

                return false;
            });

        },
        defaults: {

        }
    };

})(document, window);