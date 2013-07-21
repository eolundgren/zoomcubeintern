(function (ns) {

    ns.api = function () {

        var pageSize = 30
          , itemUrl = '/api/item'
          , tagUrl = '/api/tags'
          , pageUrl = '/api/page'
          , eventUrl = '/api/event'
          , gridUrl = itemUrl + '/grid'
          , groupUrl = itemUrl + '/group';

        var ajax = {
            get: function (url, callback, context) {

                $.ajax({
                    type: 'get',
                    url: url,
                    success: function (e) { callback.apply(context, [e]); },
                    error: function (e) { alert(e.responseText); }
                });

            },
            del: function (url, callback, context) {

                $.ajax({
                    type: 'delete',
                    url: url,
                    success: function (e) { callback.apply(context, [e]); },
                    error: function (e) { alert(e.responseText); }
                });

            },
            put: function (url, data, callback, context) {

                $.ajax({
                    type: 'put',
                    url: url,
                    data: JSON.stringify(data),
                    contentType: 'application/json',
                    success: function (e) { callback.apply(context, [e]); },
                    error: function (e) { alert(e.responseText); }
                });

            },
            post: function (url, data, callback, context) {

                $.ajax({
                    type: 'post',
                    url: url,
                    data: JSON.stringify(data),
                    contentType: 'application/json',
                    success: function (e) { callback.apply(context, [e]); },
                    error: function (e) { alert(e.responseText); }
                });

            }
        };

        var itemApi = {
            group: function(callback, context) {
                ajax.get(groupUrl, callback, context);
            },
            area: function(index, tags, center, radius, callback, context) {
                var url = itemUrl + '?index=' + index + '?latitude=' + latitude + '&longitude=' + longitude + '&radius=' + radius;

                for (var tag in tags)
                    url += '&tag=' + tags[tag];

                ajax.get(url, callback, context);
            },
            all: function (index, tags, callback, context) {
                var url = gridUrl + '?index=' + index + '&size=' + pageSize;

                for (var tag in tags)
                    url += '&tag=' + tags[tag];

                ajax.get(url, callback, context);
            },
            save: function (id, obj, callback, context) {
                var url = itemUrl + '/' + id;

                ajax.put(url, obj, callback, context);
            },
            remove: function (id, callback, context) {
                var url = itemUrl + '/' + id;

                ajax.del(url, callback, context);
            }
        };

        var pageApi = {
            all: function (callback, context) {
                ajax.get(pageUrl, callback, context);
            },
            items: function (id, callback, context) {
                var url = pageUrl + '/' + id + '/item';
                ajax.get(url, callback, context);
            },
            save: function (id, obj, callback, context) {

                if (!context) {
                    // new page.
                    ajax.post(pageUrl, id, obj, callback);
                }
                else {
                    // update page.
                    var url = pageUrl + '/' + id;
                    ajax.put(url, data, callback, context);
                }



            }
        };

        var eventApi = {
            all: function (callback, context) {
                ajax.get(eventUrl, callback, context);
            }
        };

        var tagApi = {
            all: function (callback, context) {
                ajax.get(tagUrl, callback, context);
            },
            top: function (limit, callback, context) {
                var url = tagUrl + '?$take=' + limit;
                ajax.get(url, callback, context);
            },
        }

        return {
            items: itemApi,
            tags: tagApi,
            pages: pageApi
        };

    }();

    window.zoomcube = ns;

})(window.zoomcube || {});