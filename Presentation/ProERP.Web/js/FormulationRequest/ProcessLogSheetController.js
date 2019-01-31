ProApp.controller("LogSheetController", function ($scope, $http) {

    var allLotData = [];
    var categoryId = 0;
    var formulationID = 0;
    var allGradeData = [];
    var DeletedlogSheet1Id = [];
    var DeletedlogSheet2Id = [];
    var productId = 0;
    $scope.IsBatchChange = false;

    var ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    };

    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetFormulationLine',
    };

    $scope.BatchSource = {
        datatype: "json",
        datafields: [
           { name: 'BatchId', type: 'int' },
           { name: 'LotNo', type: 'string' }
        ],
        url: '/FormulationRequest/GetBatchByLineId',
        data: { LineId: 0 }
    };

    $scope.onLineChange = function (event) {
        $scope.BatchSource.data = { LineId: $scope.ddlLine.val() };
        $scope.txtGradeName.val("");
        productId = 0;
    }

    $scope.onBatchChange = function (event) {
        productId = 0;
        $.ajax({
            dataType: "json",
            type: 'GET',
            contentType: "application/json;",
            url: '/Product/GetProductByBatchId',
            data: { BatchId: $scope.ddlBatch.val() },
            success: function (response) {
                $scope.txtGradeName.val(response.GradeName);
                productId = response.ProductId;
            },
            error: function (response) {
            }
        });
        RebindGrid();
    }

    $scope.ProcessLogSheet1Source = {
        datatype: "json",
        autoBind: false,
        cache: false,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BatchId', type: 'int' },
           { name: 'GradeId', type: 'int' },
           { name: 'Date', type: 'date' },
           { name: 'Time', type: 'date', format: 'HH:mm' },
           { name: 'TZ1', type: 'decimal' },
           { name: 'TZ2', type: 'decimal' },
           { name: 'TZ3', type: 'decimal' },
           { name: 'TZ4', type: 'decimal' },
           { name: 'TZ5', type: 'decimal' },
           { name: 'TZ6', type: 'decimal' },
           { name: 'TZ7', type: 'decimal' },
           { name: 'TZ8', type: 'decimal' },
           { name: 'TZ9', type: 'decimal' },
           { name: 'TZ10', type: 'decimal' },
           { name: 'TZ11', type: 'decimal' },
           { name: 'TZ12Die', type: 'decimal' },
           { name: 'TM1', type: 'decimal' },
           { name: 'PM1', type: 'decimal' },
           { name: 'PM11', type: 'decimal' },
           { name: 'Vaccumembar', type: 'decimal' }
        ],
        url: '/FormulationRequest/GetProcessLogSheet1GridData',
        data: { LineId: 0, BatchId: 0, currentDate: (new Date()).toISOString() }
    };

    $scope.ProcessLogSheet2Source = {
        datatype: "json",
        autoBind: false,
        cache: false,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BatchId', type: 'int' },
           { name: 'GradeId', type: 'int' },
           { name: 'Date', type: 'date' },
           { name: 'Time', type: 'date', format: 'HH:mm' },
           { name: 'RPM', type: 'decimal' },
           { name: 'TORQ', type: 'decimal' },
           { name: 'AMPS', type: 'decimal' },
           { name: 'RPM1', type: 'decimal' },
           { name: 'RPM2', type: 'decimal' },
           { name: 'RPM3', type: 'decimal' },
           { name: 'F1KGHR', type: 'decimal' },
           { name: 'F1Perc', type: 'decimal' },
           { name: 'F2KGHR', type: 'decimal' },
           { name: 'F2Perc', type: 'decimal' },
           { name: 'F3KGHR', type: 'decimal' },
           { name: 'F3Perc', type: 'decimal' },
           { name: 'F4KGHR', type: 'decimal' },
           { name: 'F4Perc', type: 'decimal' },
           { name: 'F5KGHR', type: 'decimal' },
           { name: 'F5Perc', type: 'decimal' },
           { name: 'F6KGHR', type: 'decimal' },
           { name: 'F6Perc', type: 'decimal' },
           { name: 'Output', type: 'decimal' },
           { name: 'NoofDiesHoles', type: 'decimal' },
           { name: 'Remarks', type: 'string' },
        ],
        url: '/FormulationRequest/GetProcessLogSheet2GridData',
        data: { LineId: 0, BatchId: 0 , currentDate: (new Date()).toISOString()}
    };

    $scope.ProcessLogSheet1Columns = [
         {
             text: '#', sortable: false, filterable: false, editable: true,
             groupable: false, draggable: false, resizable: false, pinned: true,
             datafield: 'SRNo', columntype: 'number', width: 50,
             cellsrenderer: function (row, column, value) {
                 return "<div style='margin:4px;'>" + (value + 1) + "</div>";
             }
         },
        {
            text: 'Time', datafield: 'Time', width: 50, align: 'center', columntype: 'datetimeinput', cellsformat: "HH:mm", sortable: true,
            createeditor: function (row, value, editor) {
                editor.jqxDateTimeInput({ showTimeButton: false, showCalendarButton: false, formatString: 'HH:mm' });
            }
        },
       { text: 'TZ1', datafield: 'TZ1', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ2', datafield: 'TZ2', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ3', datafield: 'TZ3', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ4', datafield: 'TZ4', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ5', datafield: 'TZ5', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ6', datafield: 'TZ6', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ7', datafield: 'TZ7', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ8', datafield: 'TZ8', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ9', datafield: 'TZ9', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ10', datafield: 'TZ10', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ11', datafield: 'TZ11', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TZ12Die', datafield: 'TZ12Die', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'TM1', datafield: 'TM1', editable: true, width: 70, columngroup: 'BarrelTemperature' },
       { text: 'PM1', datafield: 'PM1', editable: true, width: 70, columngroup: 'Pressure' },
       { text: 'PM11', datafield: 'PM11', editable: true, width: 70, columngroup: 'Pressure' },
       { text: 'Vaccumembar', datafield: 'Vaccumembar', editable: true, columngroup: 'Vaccume' }
    ];

    $scope.ProcessLogSheet1GroupColumns = [
        { text: 'Barrel Temperature Parameters', align: 'center', name: 'BarrelTemperature' },
        { text: 'Pressure', align: 'center', name: 'Pressure' },
        { text: 'Vaccume', align: 'center', name: 'Vaccume' }
    ];

    $scope.ProcessLogSheet2Columns = [
         {
             text: '#', sortable: false, filterable: false, editable: true,
             groupable: false, draggable: false, resizable: false, pinned: true,
             datafield: 'SRNo', columntype: 'number', width: 50,
             cellsrenderer: function (row, column, value) {
                 return "<div style='margin:4px;'>" + (value + 1) + "</div>";
             }
         },
       {
           text: 'Time', datafield: 'Time', width: 50, align: 'center', columntype: 'datetimeinput', cellsformat: "HH:mm", sortable: true,
           createeditor: function (row, value, editor) {
               editor.jqxDateTimeInput({ showTimeButton: false, showCalendarButton: false, formatString: 'HH:mm' });
           }
       },
       { text: 'RPM', datafield: 'RPM', editable: true, width: 70, columngroup: 'EtruderParameter' },
       { text: 'TORQ', datafield: 'TORQ', editable: true, width: 70, columngroup: 'EtruderParameter' },
       { text: 'AMPS', datafield: 'AMPS', editable: true, width: 70, columngroup: 'EtruderParameter' },
       { text: 'RPM', datafield: 'RPM1', editable: true, width: 70, columngroup: 'SideFeeder' },
       { text: 'RPM', datafield: 'RPM2', editable: true, width: 70, columngroup: 'SideFeeder' },
       { text: 'RPM', datafield: 'RPM3', editable: true, width: 70, columngroup: 'Peletiser' },
       { text: 'F1 KG/HR', datafield: 'F1KGHR', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F1 %', datafield: 'F1Perc', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F2 KG/HR', datafield: 'F2KGHR', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F2 %', datafield: 'F2Perc', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F3 KG/HR', datafield: 'F3KGHR', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F3 %', datafield: 'F3Perc', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F4 KG/HR', datafield: 'F4KGHR', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F4 %', datafield: 'F4Perc', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F5 KG/HR', datafield: 'F5KGHR', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F5 %', datafield: 'F5Perc', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F6 KG/HR', datafield: 'F6KGHR', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'F6 %', datafield: 'F6Perc', editable: true, width: 70, columngroup: 'FeedRate' },
       { text: 'Output', datafield: 'Output', editable: true, width: 70, columngroup: 'Total' },
       { text: 'NoofDiesHoles', datafield: 'NoofDiesHoles', editable: true, width: 100 },
       { text: 'Remarks', datafield: 'Remarks', editable: true }
    ];

    $scope.ProcessLogSheet2GroupColumns = [
       { text: 'Etruder Parameter ', align: 'center', name: 'EtruderParameter' },
       { text: 'Side Feeder', align: 'center', name: 'SideFeeder' },
       { text: 'Peletiser ', align: 'center', name: 'Peletiser' },
       { text: 'Feed Rate ', align: 'center', name: 'FeedRate' },
       { text: 'Total', align: 'center', name: 'Total' }
    ];

    var RebindGrid = function () {
        //productId = 0;
        $scope.IsBatchChange = true;
        var newDate = $scope.dtPackingDate.getDate().toISOString();
        var LineId = $scope.ddlLine.val();
        var BatchId = $scope.ddlBatch.val();
        $scope.ProcessLogSheet1Source.data = { LineId: LineId, BatchId: BatchId, currentDate: newDate };
        $scope.ProcessLogSheet2Source.data = { LineId: LineId, BatchId: BatchId, currentDate: newDate };
        $scope.gridProcessLogSheet1.updatebounddata();
        $scope.gridProcessLogSheet2.updatebounddata();
    }

    $scope.onSearchClick = function () {
        RebindGrid();
    }

    $scope.onRefreshSheet1Click = function () {
        $scope.gridProcessLogSheet1.updatebounddata();
    }

    $scope.onRefreshSheet2Click = function () {
        $scope.gridProcessLogSheet2.updatebounddata();
    }
    
    $scope.onSaveSheet1Click = function (event) {
        var rows = $scope.gridProcessLogSheet1.getrows();
        logsheet1Data = [];
        var logsheet1Data = $.grep(rows, function (a) {
            return a.Time != null && a.TZ1 > 0 && a.TZ2 > 0 && a.TZ3 > 0 && a.TZ4 > 0 && a.TZ5 > 0 && a.TZ6 > 0
            && a.TZ7 > 0 && a.TZ8 > 0 && a.TZ9 > 0 && a.TZ10 > 0 && a.TZ11 > 0 && a.TZ12Die > 0 && a.TM1 > 0
            && a.PM1 && a.PM11 && a.Vaccumembar;
        })

        var lineId=$scope.ddlLine.val();
        var batchId=$scope.ddlBatch.val();
        var gradeId=productId;
        var todayDate=$scope.dtPackingDate.getDate().toISOString();

        if (lineId > 0 && batchId > 0 && gradeId > 0) {
            var postsheet1Data = [];
            for (var i = 0; i < logsheet1Data.length; i++) {
                var item = logsheet1Data[i];
                var postItem = {
                    Id: item.Id, LineId: lineId, BatchId: batchId, GradeId: gradeId, Date: todayDate, Time: item.Time,
                    TZ1: item.TZ1, TZ2: item.TZ2, TZ3: item.TZ3, TZ4: item.TZ4, TZ5: item.TZ5, TZ6: item.TZ6, TZ7: item.TZ7,
                    TZ8: item.TZ8, TZ9: item.TZ9, TZ10: item.TZ10, TZ11: item.TZ11, TZ12Die: item.TZ12Die, TM1: item.TM1,
                    TM1: item.TM1, PM1: item.PM1, PM11: item.PM11, Vaccumembar: item.Vaccumembar
                };
                postsheet1Data.push(postItem);
            }

            $http.post('/FormulationRequest/SaveProcessLogSheet1GridData', {
                processLog1Data: postsheet1Data
            }).then(function (result) {
                if (result.data.Status == 1) {
                    $scope.openMessageBox('Success', result.data.Message, 250, 90);
                    $scope.gridProcessLogSheet1.updatebounddata();
                    postsheet1Data=[];
                }
                else {
                    $scope.gridProcessLogSheet1.updatebounddata();
                    $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                }
            }, function (result, status, headers, config) {
                alert("status");
            });
        }
        else{
            return;
        }
    }

    $scope.onSaveSheet2Click = function (event) {
        var rows = $scope.gridProcessLogSheet2.getrows();
        logsheet2Data = [];
        var logsheet2Data = $.grep(rows, function (a) {
            return a.Time != null && a.RPM > 0 && a.TORQ > 0 && a.AMPS > 0 && a.RPM1 > 0 && a.RPM2 > 0 && a.RPM3 > 0
            && a.F1KGHR > 0 && a.F1Perc > 0 && a.F2KGHR > 0 && a.F2Perc > 0 && a.F3KGHR > 0 && a.F3Perc > 0 && a.F4KGHR > 0
            && a.F4Perc > 0 && a.F5KGHR > 0 && a.F5Perc > 0 && a.F6KGHR > 0 && a.F6Perc > 0 && a.Output > 0
            && a.NoofDiesHoles > 0;
        })

        var lineId=$scope.ddlLine.val();
        var batchId=$scope.ddlBatch.val();
        var gradeId = productId;
        var todayDate = $scope.dtPackingDate.getDate().toISOString();

        if (lineId > 0 && batchId > 0 && gradeId > 0) {
            var postsheet2Data = [];
            for (var i = 0; i < logsheet2Data.length; i++) {
                var item = logsheet2Data[i];
                var postItem = {
                    Id: item.Id, LineId: lineId, BatchId: batchId, GradeId: gradeId, Date: todayDate, Time: item.Time,
                    RPM: item.RPM, TORQ: item.TORQ, AMPS: item.AMPS, RPM1: item.RPM1, RPM2: item.RPM2, RPM3 : item.RPM3, F1KGHR: item.F1KGHR,
                    F1Perc: item.F1Perc, F2KGHR: item.F2KGHR, F2Perc: item.F2Perc, F3KGHR: item.F3KGHR, F3Perc: item.F3Perc,
                    F4KGHR: item.F4KGHR, F4Perc: item.F4Perc, F5KGHR: item.F5KGHR, F5Perc: item.F5Perc, F6KGHR: item.F6KGHR, F6Perc: item.F6Perc,
                    Output: item.Output, NoofDiesHoles: item.NoofDiesHoles, Remarks: item.Remarks
                };
                postsheet2Data.push(postItem);
            }

            $http.post('/FormulationRequest/SaveProcessLogSheet2GridData', {
                processLogSheet2Data: postsheet2Data
            }).then(function (result) {
                if (result.data.Status == 1) {
                    $scope.openMessageBox('Success', result.data.Message, 250, 90);
                    $scope.gridProcessLogSheet1.updatebounddata();
                    $scope.gridProcessLogSheet2.updatebounddata();
                    postsheet2Data = [];
                }
                else {
                    $scope.gridProcessLogSheet2.updatebounddata();
                    $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                }
            }, function (result, status, headers, config) {
                alert("status");
            });
        }
        else{
            return;
        }
    }

    $scope.onDeleteSheet1Click = function (event) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete selected item(s)?', 350, 100, function (isYes) {
            if (isYes) {
                var rows = $scope.gridProcessLogSheet1.selectedrowindexes;
                var DeletedlogSheet1Id = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.gridProcessLogSheet1.getrowdata(rows[m]);
                    if (row != null && row.Id > 0)
                        DeletedlogSheet1Id.push(row.Id);
                }
                if (DeletedlogSheet1Id.length == 0) {
                    $scope.gridProcessLogSheet1.clearselection();
                    return;
                }

                $http.post('/FormulationRequest/DeleteProcessLogSheet1GridData', { DeletedId: DeletedlogSheet1Id }).then(function (result) {
                    if (result.data.Status == 1) {
                        $scope.openMessageBox('Success', result.data.Message, 200, 80);
                        $scope.gridProcessLogSheet1.updatebounddata();
                        $scope.gridProcessLogSheet1.clearselection();
                        DeletedlogSheet1Id = [];
                    }
                    else {
                        $scope.gridProcessLogSheet1.updatebounddata();
                        $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                });

            }
        });
    }

    $scope.onDeleteSheet2Click = function (event) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete selected item(s)?', 350, 100, function (isYes) {
            if (isYes) {
                var rows = $scope.gridProcessLogSheet2.selectedrowindexes;
                var DeletedlogSheet2Id = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.gridProcessLogSheet2.getrowdata(rows[m]);
                    if (row != null && row.Id > 0)
                        DeletedlogSheet2Id.push(row.Id);
                }
                if (DeletedlogSheet2Id.length == 0) {
                    $scope.gridProcessLogSheet2.clearselection();
                    return;
                }

                $http.post('/FormulationRequest/DeleteProcessLogSheet2GridData', { DeletedId: DeletedlogSheet2Id }).then(function (result) {
                    if (result.data.Status == 1) {
                        $scope.openMessageBox('Success', result.data.Message, 200, 80);
                        $scope.gridProcessLogSheet2.updatebounddata();
                        $scope.gridProcessLogSheet2.clearselection();
                        DeletedlogSheet2Id = [];
                    }
                    else {
                        $scope.gridProcessLogSheet2.updatebounddata();
                        $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                });

            }
        });
    }

    $scope.OnExportlogSheet1 = function (exportType) {
        if (exportType == 'excel') {
            var newDate = $scope.dtPackingDate.getDate().toISOString();
            var LineId = $scope.ddlLine.val();
            var BatchId = $scope.ddlBatch.val();
            if (LineId > 0 && BatchId > 0) {
                window.location = '/Download/GetProcessLogSheet1Excel?LineId=' + LineId + '&BatchId=' + BatchId + '&currentDate=' + newDate;
            }
            else {
                $scope.openMessageBox('Warning', 'Please select Line and batch', 200, 100);
            }
        }
    }

    $scope.OnExportlogSheet2 = function (exportType) {
        if (exportType == 'excel') {
            var newDate = $scope.dtPackingDate.getDate().toISOString();
            var LineId = $scope.ddlLine.val();
            var BatchId = $scope.ddlBatch.val();
            if (LineId > 0 && BatchId > 0) {
                window.location = '/Download/GetProcessLogSheet2Excel?LineId=' + LineId + '&BatchId=' + BatchId + '&currentDate=' + newDate;
            }
            else {
                $scope.openMessageBox('Warning', 'Please select Line and batch', 200, 100);
            }
        }
    }

    $scope.onAddNewRowSheet1Click = function (event) {
        var lineId = $scope.ddlLine.val();
        var batchId = $scope.ddlBatch.val();
        var gradeId = productId;
        var todayDate = $scope.dtPackingDate.getDate().toISOString();
        var rows = new Array();
        for (var i = 0; i < 2; i++) {
            var Datarow = {
                Id: 0, LineId: lineId, BatchId: batchId, GradeId: gradeId, Date: todayDate, Time: "00:00",
                TZ1: 0, TZ2: 0, TZ3: 0, TZ4: 0, TZ5: 0, TZ6: 0, TZ7: 0,
                TZ8: 0, TZ9: 0, TZ10: 0, TZ11: 0, TZ12Die: 0, TM1: 0,
                TM1: 0, PM1: 0, PM11: 0, Vaccumembar: 0
            };
            rows.push(Datarow);
        }
        $scope.gridProcessLogSheet1.addrow(null, rows);
    }

    $scope.onAddNewRowSheet2Click = function (event) {
        var lineId = $scope.ddlLine.val();
        var batchId = $scope.ddlBatch.val();
        var gradeId = productId;
        var todayDate = $scope.dtPackingDate.getDate().toISOString();
        var rows = new Array();
        for (var i = 0; i < 2; i++) {
            var Datarow = {
                Id: item.Id, LineId: lineId, BatchId: batchId, GradeId: gradeId, Date: todayDate, Time : "00:00",
                RPM: 0, TORQ: 0, AMPS: 0, RPM1: 0, RPM2: 0, RPM3: 0, F1KGHR: 0,
                F1Perc: 0, F2KGHR: 0, F2Perc: 0, F3KGHR: 0, F3Perc: 0,
                F4KGHR: 0, F4Perc: 0, F5KGHR: 0, F5Perc: 0, F6KGHR: 0,
                Output: 0, NoofDiesHoles: 0, Remarks: ""
            };
            rows.push(Datarow);
        }
        $scope.gridProcessLogSheet2.addrow(null, rows);
    }

});