ProApp.controller("PMController", function ($parse, $scope, $http) {

    var MachineName = $scope.getCookie('MachineName');
    var PlantId = $scope.getCookie('PlantId');
    var LineId = $scope.getCookie('LineId');
    var ScheduleType = $scope.getCookie('ScheduleType');
    if (typeof (MachineName) == 'undefined')
        $scope.SearchMachineName = '';

    if (PlantId == '')
        PlantId = 0;
    if (LineId == '')
        LineId = 0;
    if (ScheduleType == '')
        ScheduleType = 0;

    $scope.SearchMachineName = MachineName;
    //$scope.ddlScheduleType.getSelectedIndex() = ScheduleType;
    $scope.PMGrid = {};
    $scope.model = { PlantId: 0, LineId: 0 }

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
    $scope.ScheduleSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/PreventiveMaintenance/GetScheduleTypeList'
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


    $scope.PM = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'MachineName', type: 'string' },
           { name: 'WorkName', type: 'string' },
           { name: 'Checkpoints', type: 'string' },
           { name: 'ScheduleTypeName', type: 'string' },
           { name: 'Interval', type: 'int' },
           { name: 'ShutdownRequired', type: 'int' },
           { name: 'ScheduleStartDate', type: 'date' },
           { name: 'ScheduleEndDate', type: 'date' },
           { name: 'Severity', type: 'string' },
           { name: 'IsObservation', type: 'bool' }
        ],
        url: '/PreventiveMaintenance/GetPMList',
        data: { Name: $scope.SearchMachineName, PlantId: PlantId, LineId: LineId, ScheduleType: ScheduleType },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc',
        updaterow: function (rowid, rowdata, commit) {
        $.ajax({
            url: "/PreventiveMaintenance/UpdateIsObservation",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { PMId: rowdata.Id, IsObservation: rowdata.IsObservation },
            success: function (data) {
                if (data.Status == 1 )
                {
                    $scope.openMessageBox('Success', 'Update successfully.', 200, 90);
                    $scope.GridPM.updatebounddata();
                }
                else
                {
                    $scope.openMessageBox('Error', data.Message, 500, 100);
                    $scope.GridPM.updatebounddata();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    },
    };

    $scope.onPlantChange = function (e) {
        $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    }
    $scope.onPlantBindingComplete = function (e) {
        if (PlantId > 0)
            $scope.ddlPlant.val(PlantId);
        else
            $scope.ddlPlant.val($scope.ddlPlant.val());
    }
    $scope.onLineBindingComplete = function (e) {
        if (LineId > 0)
            $scope.ddlLine.val(LineId);
        else
            $scope.ddlLine.val($scope.ddlLine.val());
    }
    $scope.onScheduleTypeBindingComplete = function (e) {
        if (ScheduleType > 0)
            $scope.ddlScheduleType.val(ScheduleType);
        else
            $scope.ddlScheduleType.val($scope.ddlScheduleType.val());
    }
    $scope.UpdatePM = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.PMGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/PreventiveMaintenance/Update/" + dataRecord.Id + "'> Edit/View </a>";

    }

    $scope.onSearch = function (e) {
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var LineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var ScheduleType = ($scope.ddlScheduleType.val() == "" ? 0 : $scope.ddlScheduleType.val());
        $scope.PM.data = { Name: $scope.SearchMachineName, PlantId: PlantId, LineId: LineId, ScheduleType: ScheduleType };
        if (typeof ($scope.SearchMachineName) == 'undefined')
            $scope.delete_cookie('MachineName');

        $scope.setCookie('MachineName', $scope.SearchMachineName, 1);
        $scope.setCookie('PlantId', PlantId, 1);
        $scope.setCookie('LineId', LineId, 1);
        $scope.setCookie('ScheduleType', ScheduleType, 1);
    };
    $scope.onRefreshClick = function (e) {
        $scope.PMGrid.jqxGrid('updatebounddata');
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete PM(s)?', 350, 100, function (e) {
            if (e) {

                var rows = $scope.PMGrid.jqxGrid('selectedrowindexes');
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.PMGrid.jqxGrid('getrowdata', rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }

                $http.post('/PreventiveMaintenance/DeletePM', { Ids: selectedIds }).success(function (retData) {
                    if (retData.Message == "Success") {

                        //  $('#lstBatch').jqxListBox('refresh');
                        $scope.PMGrid.jqxGrid('updatebounddata');

                    } else {

                        $scope.openMessageBox("Error", retData.Message, 350, 100);

                        $scope.PMGrid.jqxGrid('updatebounddata');

                    }

                }).error(function (retData, status, headers, config) {

                });

            }
            else {

            }
        });
    };

    $scope.OnExport = function (exportType) {
        if (exportType == 'excel') {
            //$scope.GridPM.exportdata('xls', 'PreventiveMaintenance')
            var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
            var LineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
            var ScheduleType = ($scope.ddlScheduleType.val() == "" ? 0 : $scope.ddlScheduleType.val());
            window.location = '/Download/PreventiveMaintenanceList?Name=' + $scope.SearchMachineName + '&PlantId=' + PlantId + '&LineId=' + LineId + '&ScheduleType=' + ScheduleType;
        }
    }
});