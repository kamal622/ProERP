ProApp.controller("UtilityController", function ($scope, $http) {
    var DataFields = [];
    $scope.BreakdownColumns = [];
    $scope.BreakdownGroupColumns = [];
    $scope.ShowAggregates = false;
    $scope.SearchSource = [];

    $scope.SearchDateCreated = function (args) {
        var dateRange = searchDates[$(args.element).attr('data-id')];
        //var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = dateRange.from; //new Date(y, m, 1);
        var lastDay = dateRange.to; //new Date(y, m + 1, 0);
        var dateTimeInput = args.instance;
        dateTimeInput.setRange(firstDay, lastDay);
    }

    $scope.ReportsDataSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Utility/GetReportTemplates'
    };

    var searchDates = {};
    $scope.Report_Select = function (event) {
        var args = event.args;
        if (args) {
            //$scope.selectLog = 'Selected: ' + args.item.label;
            //alert(args.item.value);
            $.ajax({
                dataType: "json",
                type: 'POST',
                url: '/Utility/GetGridModel',
                data: { templateId: args.item.value },
                success: function (response) {
                    if (response.Status == 1) {
                        var gridColumns = ApplyColumnRules(response.Data.Columns);
                        $scope.$apply(function (e) {
                            $scope.BreakdownColumns = gridColumns; //response.Data.Columns;
                            $scope.BreakdownGroupColumns = response.Data.ColumnGropus;
                            $scope.ShowAggregates = true;
                            $scope.SearchSource = response.Data.SearchViewModel;
                        });

                        for (var i = 0; i < response.Data.SearchViewModel.length; i++) {
                            var searchModel = response.Data.SearchViewModel[i];
                            if (searchModel.DataType == "D") {
                                var dateRange = {};
                                dateRange.from = new Date(searchModel.Value.from);
                                dateRange.to = new Date(searchModel.Value.to);
                                searchDates['dtpSearchDate' + searchModel.Id] = dateRange;
                                //searchModel.Value.from = new Date(searchModel.Value.from);
                                //searchModel.Value.to = new Date(searchModel.Value.to);
                            }
                        }

                        $scope.GridSource = {
                            autoBind: false,
                            type: 'POST',
                            datatype: "json",
                            url: '/Utility/GetGridData',
                            data: { templateId: parseInt($scope.ddlReports.val()), searchViewModel: $scope.SearchSource },
                            datafields: response.Data.DataFields,
                            Id: 'Id'
                        }
                    }
                    else {
                        $scope.openMessageBox('Error', response.Message, 'auto', 'auto');
                    }
                },
                error: function (jqXHR, exception) {
                }
            });

        }
    }

    var OnChangeFormula = {};
    var ApplyColumnRules = function (gridColumns) {
        for (var i = 0; i < gridColumns.length; i++) {
            var column = gridColumns[i];
            if (column.DataType == 'F') {
                column.createeditor = function (row, cellvalue, editor, celltext, cellwidth, cellheight) {
                    editor.jqxNumberInput({ spinButtons: true, inputMode: 'simple', decimalDigits: 2 });
                }
                delete column.DataType;
            }

            if (column.OnChangeFormula != null) {
                OnChangeFormula[column.datafield] = column.OnChangeFormula;

                column.cellvaluechanging = function (rowindex, datafield, columntype, oldvalue, newvalue) {
                    if (newvalue == "" || rowindex < 0) {
                        return oldvalue;
                    }

                    //var splitLR = OnChangeFormula[datafield].split("=");
                    //var LHS = eval(splitLR[0].trim().replace('Prev', 'this.owner.getrowdata(rowindex - 1)'));
                    //var RHS = eval(splitLR[1].trim().replace('Prev', 'this.owner.getrowdata(rowindex - 1)').replace('Cur', 'this.owner.getrowdata(rowindex)'));
                    //LHS = RHS;

                    var formula = OnChangeFormula[datafield];
                    formula = OnChangeFormula[datafield].replaceAll("Cur['" + datafield + "']", newvalue);
                    eval(formula.replaceAll('Prev', 'this.owner.getrowdata(rowindex - 1)').replaceAll('Cur', 'this.owner.getrowdata(rowindex)').replaceAll('Next', 'this.owner.getrowdata(rowindex + 1)'));
                }
            }

            if (typeof (column.aggregates) != 'undefined') {
                if (column.aggregates[0] == 'avg') {
                    column.aggregatesrenderer = function (aggregates, column, element, summaryData) {
                        var renderstring = "";
                        var avgdata = 0;
                        var cnt = 0;
                        var rows = $scope.grdMain.getrows();
                        for (var i = 0; i < rows.length; i++) {
                            if (rows[i][column.datafield] != null) {
                                cnt++;
                                avgdata += (rows[i][column.datafield]);
                            }
                        }
                        var average = avgdata / cnt;
                        renderstring = "Avg:" + average.toFixed(2);
                        return '<div style="margin: 4px; overflow: hidden; position: relative;">' + renderstring + '</div>'
                    }
                }
            }
        }
        return gridColumns;
    }

    $scope.onSearchClick = function () {
        //var selection = $scope.dtpSearchDate.getRange();
        BindGrid();

        //$scope.selectLog = 'Selected: ' + args.item.label;
        //alert(args.item.value);
        //$.ajax({
        //    dataType: "json",
        //    type: 'POST',
        //    url: '/Utility/GetGridModel',
        //    data: { templateId: parseInt($scope.ddlReports.val()) },
        //    success: function (response) {
        //        if (response.Status == 1) {
        //            var gridColumns = ApplyColumnRules(response.Data.Columns);
        //            $scope.$apply(function (e) {
        //                $scope.BreakdownColumns = gridColumns; //response.Data.Columns;
        //                $scope.BreakdownGroupColumns = response.Data.ColumnGropus;
        //                $scope.ShowAggregates = true;
        //                $scope.SearchSource = response.Data.SearchViewModel;
        //            });

        //            for (var i = 0; i < response.Data.SearchViewModel.length; i++) {
        //                var searchModel = response.Data.SearchViewModel[i];
        //                if (searchModel.DataType == "D") {
        //                    var dateRange = {};
        //                    dateRange.from = new Date(searchModel.Value.from);
        //                    dateRange.to = new Date(searchModel.Value.to);
        //                    searchDates['dtpSearchDate' + searchModel.Id] = dateRange;
        //                    //searchModel.Value.from = new Date(searchModel.Value.from);
        //                    //searchModel.Value.to = new Date(searchModel.Value.to);
        //                }
        //            }

        //            $scope.GridSource = {
        //                autoBind: false,
        //                type: 'POST',
        //                datatype: "json",
        //                url: '/Utility/GetGridData',
        //                data: { templateId: parseInt($scope.ddlReports.val()), searchViewModel: $scope.SearchSource },
        //                datafields: response.Data.DataFields,
        //                Id: 'Id'
        //            }
        //        }
        //        else {
        //            $scope.openMessageBox('Error', response.Message, 'auto', 'auto');
        //        }
        //    },
        //    error: function (jqXHR, exception) {
        //    }
        //});
    }

    var BindGrid = function () {
        $scope.GridSource.data.templateId = parseInt($scope.ddlReports.val());

        for (var i = 0; i < $scope.SearchSource.length; i++) {
            var searchModel = $scope.SearchSource[i];
            if (searchModel.DataType == "D") {
                var dtp = $("div[data-id = 'dtpSearchDate" + searchModel.Id + "']");
                var range = dtp.jqxDateTimeInput('getRange');
                searchModel.Value.from = range.from.toISOString();
                searchModel.Value.to = range.to.toISOString();
            }
        }

        $scope.GridSource.data.searchViewModel = $scope.SearchSource;
        //$scope.GridSource.data.fromDate = selection.from.toISOString();
        //$scope.GridSource.data.toDate = selection.to.toISOString();

        $scope.grdMain.updatebounddata();
    };

    $scope.onExportClick = function () {
        //$scope.grdMain.exportdata('xls', 'UtilityData')
        var selectedItem = $scope.ddlReports.getSelectedItem();
        var templateId = parseInt($scope.ddlReports.val());
        for (var i = 0; i < $scope.SearchSource.length; i++) {
            var searchModel = $scope.SearchSource[i];
            if (searchModel.DataType == "D") {
                var dtp = $("div[data-id = 'dtpSearchDate" + searchModel.Id + "']");
                var range = dtp.jqxDateTimeInput('getRange');
                searchModel.Value.from = range.from.toISOString();
                searchModel.Value.to = range.to.toISOString();
            }
        }

        $.post('/Download/CreateUtilityDataFile', { templateId: templateId, searchViewModel: $scope.SearchSource, reportName: selectedItem.label }, function (retData) {
            window.location = '/Download/UtilityData?tempFileName=' + retData.TempFileName + '&downloadFileName=' + retData.DownloadFileName;
        });
    }

    $scope.onSaveClick = function () {
        var rows = $scope.grdMain.getrows();
        var templateId = parseInt($scope.ddlReports.val());

        var cloneRows = jQuery.extend(true, {}, rows);

        $.each(cloneRows, function (i, item) {
            $.each($scope.GridSource.datafields, function (j, dataField) {
                if (dataField.type == 'date')
                    item[dataField.name] = new Date(item[dataField.name]).toISOString();
            });
        });

        $.ajax({
            dataType: "json",
            type: 'POST',
            url: '/Utility/SaveData',
            data: { readingData: cloneRows, templateId: templateId },
            success: function (response) {
                if (response.Status == 1) {
                    BindGrid();
                    $scope.openMessageBox('Success', 'Data saved successfully.', 250, 85);
                }
                else {
                    $scope.openMessageBox('Error', response.Message, 'auto', 'auto');
                }
            },
            error: function (jqXHR, exception) {
            }
        });
    }

});