(function (undefined) {

    var _interval = null
      , _duration = 20000
      , _queue = null
      , _aniRunning = false;

    function show(id) {
        _aniRunning = true;
        clearInterval(_interval);

        var $active = $('.featured .active:first')
          , $id = $(id)
          , $nav = $('.featured nav .active');

        $active.fadeOut(450, function () {
            $active.removeClass('active');
            $id.addClass('active').fadeIn(550);
            $('a[href="' + id + '"]').addClass('active');

            _aniRunning = false;
            _interval = setInterval(circle, _duration);

            if (_queue != null) {
                show(_queue);
                _queue = null;
            }
        });

        $nav.removeClass('active');
    }

    function circle() {
        var a = $('.featured nav .active')
          , next = a.next();

        if (next.length == 0)
            next = $('.featured nav a:first');
        
        show(next.attr('href'));
    }

    $().ready(function () {

        $('a[href^="#feature-"]').click(function (e) {

            var target = (e.srcElement | e.currentTarget).hash;

            if (_aniRunning)
                _queue = target;
            else
                show(target);

            return false;
        });

        _interval = setInterval(circle, _duration);

    });

})();