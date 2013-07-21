(function (doc, win, undefined) {
    
    // Templates used to render the grid
    var templateEngine = new ko.nativeTemplateEngine();

    templateEngine.addTemplate = function (templateName, templateMarkup) {
        doc.write('<script id="' + templateName + '" type="text/html">' + templateMarkup + '</script>');
    };

    templateEngine.render = function (templateName, viewModel, container) {
        var containerWrapper = container.appendChild(doc.createElement('DIV'));
        ko.renderTemplate(templateName, viewModel, { templateEngine: templateEngine }, containerWrapper, 'replaceNode');
    };

    templateEngine.addTemplate('timeslider-template', '\
        <div class="timeslider">\
            <div class="upper" data-bind="text: upper"></div>\
            <div class="cursor"></div>\
            <!-- ko foreach: marks -->\
                <div class="section"></div>\
            <!-- /ko -->\
            <div class="lower" data-bind="text: lower"></div>\
            <div class="background"></div>\
        </div>');

    var initializeCursor = function (parent, newLocationCallback) {
        var $elm = $(parent)
          , cursor = $elm.find('.cursor')
          , offset = 0
          , start = 0
          , top = $elm.find('.upper').outerHeight(true)
          , bottom = $elm.height() - top - cursor.height()
          , prevY = 0;

        function goto(y) {

            if (y < top)
                y = top;

            if (y > bottom)
                y = bottom;

            cursor.css({ top: y + 'px' });

            if (newLocationCallback && y !== prevY) {
                var h = ($elm.height() - (top * 2));

                newLocationCallback({ height: h, pos: y - top });
                prevY = y;
            }
        }

        $(doc).unbind('mousemove').bind('mousemove', function (e) {

            if (offset > 0) {
                var y = offset + e.clientY - start;
                goto(y);
            }

            return false;
        });

        cursor.unbind('mousedown').bind('mousedown', function (e) {

            if (offset === 0) {
                start = e.clientY;
                offset = parseInt(cursor.css('top'), 10);
            }
            else {
                start = 0;
                offset = 0;
            }

            return false;
        });

        $(doc).unbind('mouseup').bind('mouseup', function (e) {
            offset = 0;
            start = 0;
        });

        $elm.unbind('click').bind('click', function (e) {

            var y = e.offsetY - (cursor.height() / 2);

            if (y > 0)
                goto(y);

            return false;
        });

        //$section.scroll(function () {

        //    var sectionTop = $section.scrollTop()
        //      , cursorTop = cursor.offset().top;

        //    if (screenTop > cursorTop) {
        //        cursor.css({ top: sectionTop + 10 + 'px' });
        //    }
        //});
    };

    var TimeSliderViewModel = function (options) {
        this.lower = ko.observable().link(options.lower);
        this.upper = ko.observable().link(options.upper);
        this.marks = ko.observable().link(options.marks);
        this.increment = ko.observable().link(options.increment);
    };

    ko.bindingHandlers.timeslider = {
        init: function (elm, valueAccessor, allBindings, viewModel, context) {
            return { controlsDescendantBindings: true };
        },
        update: function (elm, valueAccessor, allBindings, viewModel, context) {
            var options = ko.utils.unwrapObservable(valueAccessor())
              , local = ko.utils.extend(ko.bindingHandlers.timeslider.defaults, options);

            var markList = ko.utils.unwrapObservable(local.marks);

            if (markList.length === 0)
                return;

            var viewModel = new TimeSliderViewModel(local);
            templateEngine.render('timeslider-template', viewModel, elm);

            initializeCursor(elm, function (e) {
                var u = viewModel.upper()
                  , l = viewModel.lower()
                  , unit = (u - l) / e.height
                  , ticks = u - (unit * e.pos)
                  , date = new Date(ticks);

                console.log('cursor date: ' + date.getYear() + '-' + (date.getMonth() + 1) + '-' + date.getDay());
            });

            return { controlsDescendantBindings: true };
        },
        defaults: {

        }
    };

})(document, window);