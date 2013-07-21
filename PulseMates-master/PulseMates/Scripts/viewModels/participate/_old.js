/// <reference path="jquery-1.8.3.js" />
/// <reference path="jquery-ui-1.9.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.1.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
var PulseMates;
var uuid = 0;

(function (ns, win, doc, undefined) {

    ns.ParticipateViewModel = function () {
        var self = this;

        this.datasetList = ko.observableArray();
        this.currentDataset = ko.observable();

        this.newDataset = function (viewModel, e) {

            var ds = new ns.DatasetViewModel({
                Id : 0,
                Name: '',
                Description: '',
                Fields: null
            });

            ds.editor.edit();
            this.datasetList.push(ds);
            this.currentDataset(ds);

        };
      
        $.ajax({
            type: 'get',
            url: '/api/dataset',
            success: function (e) {
                var map = $.map(e, function (i) { return new ns.DatasetViewModel(i); });
                self.datasetList(map);
                self.currentDataset(map[0]);
            },
            error: function (e) {
                alert(e);
            }
        });
    };

    ns.EditDatasetViewModel = function (root) {
        var self = this;

        self.editing = ko.observable(false);
        self.name = ko.observable();
        self.desc = ko.observable();
        self.fields = ko.observableArray();

        self.edit = function (viewModel, e) {
            self.name(root.name());
            self.desc(root.desc());
            self.fields(root.fields());

            self.editing(true);
        }; 

        self.save = function (viewModel, e) {

            var data = { Name: viewModel.name(), Description: viewModel.desc(), Definitions: viewModel.fields() };

            $.ajax({
                type: 'put',
                url: '/api/dataset/' + root.id,
                data: data,
                statusCode: {
                    404: function (e) {
                        alert(e);
                    },
                    202: function (e) {
                        root.name(e.Name);
                        root.desc(e.Description);
                        root.fields(mapFields(e.Definitions));

                        self.editing(false);
                    }
                },
                error: function (e) {
                    alert(e);
                }
                 
            });
        };

        this.cancel = function (viewModel, e) {
            self.editing(false);
        };

        self.addField = function (viewModel, e) {
            self.fields.push({ name: '', usage: '', type: 0, required: false });
        };

        self.delField = function (viewModel, e) {
            self.fields.remove(viewModel);
        };

    };

    ns.DatasetViewModel = function (obj) {
        var self = this;

        this.id = obj.Id;
        this.name = ko.observable(obj.Name);
        this.desc = ko.observable(obj.Description);
        this.fields = ko.observableArray();
        this.createField = ko.observable(false);
        this.editor = new ns.EditDatasetViewModel(this);

        this.fields(mapFields(obj.Definitions));

        this.addField = function (viewModel, e) {
            this.newField = {};
            this.createField(true);
        };

        this.addItem = function (viewModel, e) {

            var element = $(e.srcElement).parent('fieldset').find('ol')
              , url = '/api/dataset/' + viewModel.id + '/item';

            asyncUpload(url, element, function (e) {

                alert('fwfw');

            });

        };

        this.templateSelector = function (viewModel) {

            switch (viewModel.typeName.toLowerCase()) {
                case 'string': return 'field-string-template';
                case 'date': return 'field-date-template';
                case 'image': return 'field-image-template';
                case 'file': return 'field-file-template';
                case 'location': return 'field-geo-template';
                case 'address': return 'field-address-template';
                case 'number': return 'field-number-template';
                default: return 'field-string-template';
            }

        };

        ///
        /// 
        function asyncUpload(url, element, callback) {
            var iframeName = 'pulsemates_' + ++uuid
              , iframe = $('<iframe name="' + iframeName + '" style="position:absolute;top:-9999px" />')
                .appendTo('body')
              , form = '<form target="' + iframeName + '" action="' + url + '" method="post" enctype="multipart/form-data" />'
              , json = {};

            form = $(element).wrapAll(form).parent('form');
            form.submit(function () {

                iframe.load(function () {
                    
                    try{
                        var c = $(iframe.contents().get(0)).find('body').text();
                        json = JSON.parse(c);
                    }
                    catch (err) {
                        alert(err);
                    }

                    setTimeout(function () {
                        iframe.remove();

                        if (callback)
                            callback(json);
                    });
                });

            }).submit();
        }
    };

    function mapFields(fields) {
        if (fields != null) {
            return $.map(fields, function (i) {
                return { name: i.Name, usage: i.Usage, typeName: i.TypeName, required: i.Required }
            });
        }

        return [];
    }

    $().ready(function () {
        
        ko.applyBindings(new ns.ParticipateViewModel());

    });


})(PulseMates || (PulseMates = {}), window, document);