// Knockout ObservableArray extension that will transform an array 
// into a multidimension array (or columns) while still maintaining 
// sort direction from left to right so that: [1, 2, 3, 4, 5, 6, 7] 
// becomes [ [1, 4, 7], [2, 5], [3, 6] ] with 3 columns. 
// 
// fiddler demo:
// http://jsfiddle.net/harboe/RGBdq/16/
ko.observableArray.fn.toColumn = function (data, numberOfColumns) {

    var generateColumns = function (data, numberOfColumns) {

        var arr = ko.utils.unwrapObservable(data)
          , size = ko.utils.unwrapObservable(numberOfColumns)
          , result = [];

        for (var i = 0; i < size; i++) {
            for (var n = i; n < arr.length; n += size) {
                if (!result[i])
                    result[i] = [];

                result[i].push(arr[n]);
            }
        }

        return result;
    }

    if (ko.isObservable(data)) {
        data.subscribe(function (dataHasChanged) {
            this(generateColumns(dataHasChanged, numberOfColumns));
        }, this);
    }

    if (ko.isObservable(numberOfColumns)) {
        numberOfColumns.subscribe(function (numberHasChanged) {
            this(generateColumns(data, numberHasChanged));
        }, this);
    }

    this(generateColumns(data, numberOfColumns));
    return this;
};