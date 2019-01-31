ProApp.controller("NotesController", function ($scope, $http, $timeout) {

    $scope.model = {};
    $scope.enableVersiontxtBox = false;
    var NotesData = [];
    var DeletedNotesId = [];
    var versionId = 0;
    var noteId = 0;

    $scope.Rules = [
        { input: '#txtReleaseVersion', message: 'Version no is required!', action: 'blur,change', rule: 'required' },
        { input: '#txtNotes', message: 'Notes is required!', action: 'blur,change', rule: 'required' }
    ];

    $scope.VersionSource = {
        datatype: "json",
        cache: false,
        autoBind: true,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'ReleaseVersion', type: 'string' },
           { name: 'ReleaseDate', type: 'date' },
        ],
        url: '/Home/GetAllVersion',
    };
    var initrowdetails = function (index, parentElement, gridElement, record) {
        var id = record.uid.toString();
        var grid = $($(parentElement).children()[0]);
        var rowData = $scope.GridSettings.apply('getrowdata', index);
        var nestedSource = {
            datafields: [
                       { name: 'Id', type: 'int' },
                       { name: 'VersionId', type: 'int' },
                       { name: 'Notes', type: 'string' },
            ],
            datatype: 'json',
            cache: false,
            autoBind: true,
            url: '/Home/GetNestedGridData',
            Id: "VersionId",
            data: { VersionId: rowData.Id }
        };

        var nestedAdapter = new $.jqx.dataAdapter(nestedSource);
        if (grid != null) {
            grid.jqxGrid({
                source: nestedAdapter,
                width: '98%',
                height: 200,
                columnsresize: true,
                theme: $scope.theme,
                columns: [
                            { text: 'Id', datafield: 'Id', width: '70', hidden: 'true' },
                            { text: 'Notes', datafield: 'Notes' },
                ]
            });
        }
    }

    $scope.GridSettings = {
        width: '100%',
        columnsresize: true,
        rowdetails: true,
        initrowdetails: initrowdetails,
        rowdetailstemplate: {
            rowdetails: "<div id='grid' style='margin: 10px;'></div>",
            rowdetailsheight: 200,
            rowdetailshidden: true
        },
    };

    $scope.NotesSource = {
        datatype: "json",
        localdata: NotesData,
        autoBind: true,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'VersionId', type: 'int' },
           { name: 'Notes', type: 'string' }
        ]
    };

    $scope.onAddClick = function (event) {
        $scope.enableVersiontxtBox = false;
        $scope.model.ReleaseVersion = null;
        $scope.NotesSource.localdata = [];
        NotesData = [];
        versionId = 0;
        ClearData();
        $scope.WinNotes.open();
    };
    
    $scope.onSaveNotes = function (event) {
        var IsValidate = $scope.NotesValidator.validate();
        if (!IsValidate)
            return;

        var minId = Math.min.apply(Math, NotesData.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;

        if (noteId != 0) {
            for (var i = 0; i < NotesData.length; i++) {
                if (NotesData[i].Id === noteId) {
                    NotesData[i].Notes = $scope.Notes;
                }
            }
        }
        else {
            Id = minId - 1;
            NotesData.push({ Id: Id, VersionId: versionId, Notes: $scope.Notes });
        }
        $scope.NotesSource.localdata = NotesData;
        $scope.GridNotes.updatebounddata();
        ClearData();
    };

    $scope.onSaveAllNotes = function (event) {
        $scope.model.Id = versionId;
        $scope.model.Notes = NotesData;
        if (typeof ($scope.model.Notes) == 'undefined' || $scope.model.Notes !='')
        {
            $.ajax({
            dataType: "json",
            type: 'POST',
            cache: false,
            url: '/Home/SaveNotes',
            data: { NotesData: $scope.model, DeletedNotesIds: DeletedNotesId },
            success: function (response) {
                $scope.openMessageBox('Success', response.Message, 'auto', 'auto');
                $scope.GridVersion.updatebounddata();
                NotesData = [];
                DeletedNotesIds = [];
                versionId = 0;
                $scope.WinNotes.close();
            },
            error: function (jqXHR, exception) {
                }
            });
        }
        else
        {
            var IsValidate = $scope.NotesValidator.validate();
            if (!IsValidate)
                return;
        }
    }

    $scope.EditCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Edit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    }
    $scope.model.ReleaseVersion = [];
    $scope.Edit = function (row, event) {
        var dataRecord = $scope.GridVersion.getrowdata(row);
        versionId = dataRecord.Id;
        $.ajax({
            url: "/Home/GetNotesByVersionId",
            type: "GET",
            contentType: "application/json;",
            cache: false,
            dataType: "json",
            data: { VersionId: dataRecord.Id },
            success: function (data) {
                $scope.$apply(function () {
                    NotesData = data;
                    $scope.model.ReleaseVersion = dataRecord.ReleaseVersion;
                    $scope.NotesSource.localdata = $.grep(NotesData, function (item, i) {
                        return item;
                    });
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.enableVersiontxtBox = true;
        ClearData();
        $scope.WinNotes.open();
    }
    $scope.DeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='VersionDelete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }
    $scope.VersionDelete = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete Notes?', 350, 100, function (e) {
            if (e) {
                var dataRecord = $scope.GridVersion.getrowdata(row);
                var versionId = dataRecord.Id;
                $.ajax({
                    dataType: "json",
                    type: 'POST',
                    cache: false,
                    url: '/Home/DeleteVersion',
                    data: {
                        Id: versionId
                    },
                    success: function (response) {
                        $scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        $scope.GridVersion.updatebounddata();
                    },
                    error: function (jqXHR, exception) {
                    }
                });
                $scope.GridVersion.updatebounddata();
            }
        });
    }

    $scope.EditCell = function (row, column, value) {
        return "Edit";
    }
    $scope.EditNote = function (row, event) {
        var dataRecord = $scope.GridNotes.getrowdata(row);
        noteId = dataRecord.Id;
        for(var i=0; i<NotesData.length ;i++)
        {
            if(noteId==NotesData[i].Id)
            {
                $scope.$apply(function () {
                    $scope.Notes = NotesData[i].Notes;
                    $scope.GridNotes.updatebounddata();
                });
            }
        }
    }
    $scope.DeleteCell = function (row, column, value) {
        return "Delete";
    }
    $scope.DeleteNote = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete Version?', 350, 100, function (e) {
            if (e) {
                var dataRecord = $scope.GridNotes.getrowdata(row);
                DeletedNotesId.push(dataRecord.Id);

                NotesData = $.grep(NotesData, function (item, i) {
                    return item.Id != dataRecord.Id;
                });
                $scope.NotesSource.localdata = $.grep(NotesData, function (item, i) {
                    return item;
                });
                $scope.GridNotes.updatebounddata();
            }
        });
    }
    var ClearData = function (e) {
        if (typeof (NotesData) == 'undefined') {
            NotesData = [];
        }
        noteId = 0;
        $scope.Notes = null;
        $scope.NotesValidator.hide();
    }
});