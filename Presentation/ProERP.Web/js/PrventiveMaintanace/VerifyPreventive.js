ProApp.controller("VPController", function ($parse, $scope, $http) {

    $scope.model = { PlantId: 0, LineId: 0, Verified: 2 }

    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
        dateTimeInput.setRange(firstDay, lastDay);
    }

    var from = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        from = $scope.dtpSearchDate.getRange().from;

    var to = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        to = $scope.dtpSearchDate.getRange().to;

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


    $scope.VPSource = {
        datatype: "json",
        autoBind: true,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'WorkName', type: 'string' },
           { name: 'Checkpoints', type: 'string' },
           { name: 'ScheduleTypeName', type: 'string' },
           { name: 'Severity', type: 'string' },
           { name: 'ReviewDate', type: 'date' },
           { name: 'ReviewBy', type: 'string' },
           { name: 'VerifyDate', type: 'date' },
           { name: 'VerifyBy', type: 'string' }
        ],
        url: '/PreventiveMaintenance/GetVPList',
        data: { PlantId: 0, LineId: 0, MachineId: 0, ScheduleType: 0, Verified: 2, FromDate: from.toISOString(), ToDate: to.toISOString() },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    $('#GridVP').on('rowselect', function (event) {
        var args = event.args;
        var rowBoundIndex = args.rowindex;
        var rowData = args.row;
        if (typeof rowBoundIndex == 'number') {
            if (rowData.VerifyDate != null) {
                $('#GridVP').jqxGrid('unselectrow', rowBoundIndex);
            }
        }
        else if (typeof rowBoundIndex == 'object') {
            var rows = $('#GridVP').jqxGrid('getrows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].VerifyDate != null) {
                    $('#GridVP').jqxGrid('unselectrow', i);
                }
            }
        }
    });


    $scope.onPlantChange = function (e) {
        $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    }
    $scope.onPlantBindingComplete = function (e) {
        $scope.ddlPlant.val($scope.ddlPlant.val());
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
    $scope.onScheduleTypeBindingComplete = function (e) {
        $scope.ddlScheduleType.val($scope.ddlScheduleType.val());
    }

    $scope.onSearch = function (e) {
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var LineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var MachineId = ($scope.ddlMachine.val() == "" ? 0 : $scope.ddlMachine.val());
        var ScheduleType = ($scope.ddlScheduleType.val() == "" ? 0 : $scope.ddlScheduleType.val());
        var verify = $scope.model.Verified;
        var selection = $scope.dtpSearchDate.getRange();
        $scope.VPSource.data = { PlantId: PlantId, LineId: LineId, MachineId: MachineId, ScheduleType: ScheduleType, Verified: verify, FromDate: selection.from.toISOString(), ToDate: selection.to.toISOString() };
    };

    $scope.onVerify = function (e) {
        if ($scope.GridVP.selectedrowindexes != 0)
        {
            var rows = $scope.GridVP.selectedrowindexes;
            if (rows == null)
                $scope.openMessageBox("Warning", "Please select row.", 200, 90);
            var selectedIds = [];
            for (var m = 0; m < rows.length; m++) {
                var row = $scope.GridVP.getrowdata(rows[m]);
                if (row != null)
                    selectedIds.push(row.Id);
            }
            $.ajax({
                dataType: "json",
                type: 'POST',
                url: '/PreventiveMaintenance/VerifyPreventiveData',
                data: {
                    Ids: selectedIds
                },
                success: function (response) {
                    $scope.openMessageBox('Success', response.Message, 200, 90);
                    $scope.GridVP.updatebounddata();
                    $scope.GridVP.clearselection();
                },
                error: function (jqXHR, exception) {
                }
            });
        }
        else
            $scope.openMessageBox("Warning", "Please select row.", 200, 90);
    };

    $scope.onExportClick = function (e) {
        var selection = $scope.dtpSearchDate.getRange();
        var plantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var lineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var MachineId = ($scope.ddlMachine.val() == "" ? 0 : $scope.ddlMachine.val());
        var ScheduleType = ($scope.ddlScheduleType.val() == "" ? 0 : $scope.ddlScheduleType.val());
        var verify = $scope.model.Verified;
        var fromDate = selection.from.toISOString();
        var toDate = selection.to.toISOString();
            //checkChangesEnabled = false;
        window.location = '/Download/VerifyPreventiveList?PlantId=' + plantId + '&LineId=' + lineId + '&MachineId=' + MachineId + '&ScheduleType=' + ScheduleType + '&Verified=' + verify + '&FromDate=' + fromDate + '&ToDate=' + toDate;
        

    }

});