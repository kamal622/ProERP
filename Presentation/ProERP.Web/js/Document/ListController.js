ProApp.controller("DocumentController", function ($parse, $scope, $http) {

    $scope.txtSearch = $('#txtSearch');

    $scope.ShowUpload = true;
    $scope.ShowFileUpload = true;
    $scope.PlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
    };
    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0 }
    };
    $scope.MachineSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachinesForLine',
        data: { LineId: 0 }
    };
    $scope.PopUpPlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
    };
    $scope.PopUpLineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0 }
    };
    $scope.PopUpMachineSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachinesForLine',
        data: { LineId: 0 }
    };

    $scope.onPlantChange = function (e) {
        $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    }
    $scope.onPlantBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId };
    }
    $scope.onLineChange = function (e) {
        $scope.MachineSource.data = { LineId: $scope.ddlLine.val() };
    }
    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.val($scope.ddlLine.val());
    }
    $scope.onBindingMachine = function (e) {
        $scope.ddlMachine.val($scope.ddlMachine.val());
    }

    $scope.onPopUpPlantChange = function (e) {
        $scope.PopUpLineSource.data = { PlantId: $scope.Plantinstance.val() };
    }
    $scope.onPopUpPlantBindingComplete = function (e) {
        var selectedId = parseInt($scope.Plantinstance.val());
        $scope.PopUpLineSource.data = { PlantId: selectedId };

    }
    $scope.onPopUpLineChange = function (e) {
        $scope.PopUpMachineSource.data = { LineId: $scope.Lineinstance.val() };
    }
    $scope.onPopUpLineBindingComplete = function (e) {
        $scope.Lineinstance.val($scope.Lineinstance.val());
    }
    $scope.onPopUpBindingMachine = function (e) {
        $scope.Machineinstance.val($scope.Machineinstance.val());
    }


    $('#btnUpload').jqxTooltip({ position: 'bottom', content: "Add" });

    $scope.onSearchClick = function () {
        var selectedItem = $scope.treeSettings.jqxTree('getSelectedItem');
        if (selectedItem == null) {
            alert('Please select document category.');
            return;
        }

        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var LineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var MachineId = ($scope.ddlMachine.val() == "" ? 0 : $scope.ddlMachine.val());
        $scope.GridSource.data = { categoryId: selectedItem.value, PlantId: PlantId, LineId: LineId, MachineId: MachineId, searchKeyword: $scope.txtSearch.val() };
        $scope.gridMain.clearselection();
        $scope.gridDetails.clearselection();
        $scope.Tags = '';
        documentId = 0;
        $scope.HistoryGridSource.data = { documentId: documentId };
    }

    $scope.treeSettings = {
        width: '100%',
        allowDrag: false,
        theme: $scope.theme,
        select: function (event) {
            $scope.onSearchClick();
        },
        initialized: function (event) {
            //$scope.treeSettings.jqxTree('expandAll');
        }
    }

    var categoryIds = [];
    $scope.popupTreeSettings = {
        width: 200,
        theme: $scope.theme,
        checkboxes: true,
        hasThreeStates: true,
        checkChange: function (event) {
            var args = event.args;
            var element = args.element;
            var item = $scope.popupTreeSettings.jqxTree('getItem', element);

            if (args.checked && item != null && jQuery.inArray(item.value, categoryIds) === -1)
                categoryIds.push(item.value);
            else if (!args.checked && item != null && jQuery.inArray(JSON.stringify(item.value), categoryIds) !== -1)
                categoryIds = jQuery.grep(categoryIds, function (value) {
                    return value != item.value;
                });
        }
    }

    $scope.DDLCategoryCreated = function (args) {
        var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 7px;"><b>Add/Remove Category</b></div>';
        $scope.ddlCategory.setContent(dropDownContent);
        $scope.popupTreeSettings.jqxTree('expandAll');
        $scope.treeSettings.jqxTree('expandAll');
    }

    var treeSource = {
        datatype: "json",
        datafields: [
            { name: 'Id' },
            { name: 'ParentId' },
            { name: 'Name' },
            { name: 'Value' }
        ],
        id: 'Id',
        url: "/Documents/GetAllCategories"
    }
    var treeDataAdapter = new $.jqx.dataAdapter(treeSource, {
        loadComplete: function () {
            var records = treeDataAdapter.getRecordsHierarchy('Id', 'ParentId', 'items', [{ name: 'Name', map: 'label' }, { name: 'Value', map: 'value' }]);
            $scope.treeSettings.source = records;
            $scope.popupTreeSettings.source = records;
        }
    });

    $scope.GridSource = {

        datatype: "json",
        type: "POST",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'OriginalFileName', type: 'string' },
             { name: 'CategoryName', type: 'string' },
            { name: 'CreatedBy', type: 'string' },
            { name: 'CreatedOn', type: 'date' },
        ],
        url: '/Documents/GetDocuments',
        data: { categoryId: 0, PlantId: 0, LineId: 0, MachineId: 0, searchKeyword: $scope.txtSearch.val() },
        Id: "Id"
    };
    $scope.HistoryGridSource = {

        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'ActionName', type: 'string' },
            { name: 'ActionBy', type: 'string' },
            { name: 'ActionDate', type: 'date' },
        ],
        url: '/Documents/GetHistoryDocuments',
        data: { documentId: 0 }
    };

    $('#txtSearch').jqxInput({ placeHolder: "Search Keyword", width: 250, height: 28, theme: $scope.theme });

    $scope.toolBarButtonClick = function (event) {

        var clickedButton = event.args.button;
        if ($('#' + clickedButton[0].id).jqxButton('disabled'))
            return;
        if (clickedButton[0].id == 'btnUpload') {
            $scope.Uploadwindow.open();
        }
        else if (clickedButton[0].id == 'btndownload') {
            var rows = $scope.gridMain.getboundrows();
            var selectedIds = [];
            for (var m = 0; m < rows.length; m++) {
                if (rows[m].Select)
                    selectedIds.push(rows[m].Id);
            }
            window.location.href = "/Download/Documents/?Ids=" + selectedIds.join(",");
        }
        else if (clickedButton[0].id == 'btnExportToExcel') {
            $scope.gridMain.exportdata('xls', 'jqxGrid');

        } else if (clickedButton[0].id == 'btnExportToPDF') {
            $scope.gridMain.exportdata('pdf', 'jqxGrid');

        } else if (clickedButton[0].id == 'btnPrint') {
            var gridContent = $scope.gridMain.exportdata('html');
            var newWindow = window.open('', '', 'width=800, height=500'),
                document = newWindow.document.open(),
                pageContent =
                    '<!DOCTYPE html>\n' +
                        '<html>\n' +
                        '<head>\n' +
                        '<meta charset="utf-8" />\n' +
                        '<title>jQWidgets Grid</title>\n' +
                        '</head>\n' +
                        '<body>\n' + gridContent + '\n</body>\n</html>';
            document.write(pageContent);
            document.close();
            newWindow.print();
        }
    }
    $scope.btnSearchClick = function (event) {
        $scope.onSearchClick();
    }

    var UploadFiles = [];
    $scope.ZipFile_UploadEnd = function (event) {
        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;
        var jsonResponse = JSON.parse(serverResponce);
        $.merge(UploadFiles, jsonResponse);
        $scope.ShowUpload = false;
    }

    $scope.onZipClose = function () {
        $scope.ShowUpload = true;
        var UploadFile = $.grep(UploadFiles, function (item, i) {
            return item.ZipFileName != '';
        });
    }

    $scope.onFilesClose = function () {
        $scope.ShowFileUpload = true;
        var UploadFile = $.grep(UploadFiles, function (item, i) {
            return item.ZipFileName == '';
        });
    }

    $scope.SingleFile_UploadEnd = function (event) {
        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;
        var jsonResponse = JSON.parse(serverResponce);
        $.merge(UploadFiles, jsonResponse);
        $scope.ShowFileUpload = false;
    }

    $scope.onSaveClick = function () {
        $.ajax({

            url: "/Documents/SaveDocuments",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: {
                PlantId: parseInt($scope.Plantinstance.val()), LineId: $scope.Lineinstance.val(), MachineId: $scope.Machineinstance.val(), CategoryIds: categoryIds, UploadFiles: UploadFiles
            },
            success: function (response) {
                if (response.Status == 1) {
                    // Success
                    $scope.$apply(function () {
                        ClearData();
                        $scope.Uploadwindow.close();
                        // bindGrid();
                        $scope.openMessageBox('Success', 'Data upload successfully.', 200, 90);
                    });
                }
                else if (response.Status = 2) {
                    // Warning
                    $scope.openMessageBox('Warning', response.Message, 700, 100);
                }
                else {
                    // Error
                    $scope.openMessageBox('Error', response.Message, 700, 600);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }
    $scope.onCancelClick = function () {
        ClearData();
    }

    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete selected documents?', 350, 100, function (e) {
            if (e) {
                var rows = $scope.gridMain.getboundrows();
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    if (rows[m].Select)
                        selectedIds.push(rows[m].Id);
                }

                $http.post('/Documents/DeleteDocument', { Ids: selectedIds }).success(function (response) {
                    if (response.Status == 1) {
                        $scope.openMessageBox("Success", response.Message, 350, 100);
                        $scope.gridMain.updatebounddata();
                    }

                }).error(function (response, status, headers, config) {

                });

            }
            else {

            }
        });
    };

    var ClearData = function (event) {
        $scope.Plantinstance.selectIndex(0);
        $scope.Lineinstance.clearSelection();
        $scope.Machineinstance.clearSelection();
        $scope.ddljqxTree.uncheckAll();
        UploadFiles = [];
        categoryIds = [];
        $scope.ShowUpload = true;
        $scope.ShowFileUpload = true;
    }

    var documentId = 0;
    $('#grdMain').on('rowselect', function (event) {
        if (typeof (event) == 'undefined')
            return;
        $scope.Tags = '';
        documentId = event.args.row.Id;
        $scope.$apply(function (e) {
            $scope.HistoryGridSource.data = { documentId: documentId };
            $.ajax({
                url: "/Documents/GetTagsById",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: {
                    DocumentId: documentId
                },
                success: function (data) {
                    $scope.$apply(function (e) {
                        $scope.Tags = data.Tags;
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                    alert('oops, something bad happened');
                }
            });
        });
    });

    $scope.onUpdateClick = function (event) {
        //Ajax call to save
        if (documentId == 0 || documentId == null) {
            $scope.openMessageBox('warning', 'Please select row.', 200, 90);
        }
        else
            $.ajax({
                url: "/Documents/SaveTags",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { DocumentId: documentId, Tags: $scope.Tags },
                success: function (response) {
                    if (response.Status == 1) {
                        // Success
                        $scope.$apply(function () {
                            $scope.openMessageBox('Success', 'Update successfully.', 200, 90);
                        });
                    }
                    else {
                        // Error
                        $scope.openMessageBox('Error', response.Message, 700, 600);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
    }
    $scope.TagCancelClick = function () {
        $scope.onSearchClick();
    }

    treeDataAdapter.dataBind();
});