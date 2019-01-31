ProApp.controller("SummaryController", function ($scope) {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    //$scope.model = { FromDate: new Date(y, m, 1), ToDate: new Date(y, m + 1, 0), ScheduleType: 0 }
    $scope.model = { ScheduleType: 0 }
    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
        $scope.dtpSearchDate.setRange(firstDay, lastDay);
    }

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

    var fromDate = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        fromDate = $scope.dtpSearchDate.getRange().from;

    var toDate = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        toDate = $scope.dtpSearchDate.getRange().to;

    $scope.ReportSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LineName', type: 'string' },
           { name: 'TotalActivity', type: 'int' },
           { name: 'Moderate', type: 'int' },
           { name: 'Critical', type: 'int' },
           { name: 'Minor', type: 'int' },
           { name: 'ReviewedCount', type: 'int' },
           { name: 'ModerateReviewedCount', type: 'int' },
           { name: 'CriticalReviewedCount', type: 'int' },
           { name: 'MinorReviewedCount', type: 'int' },
           { name: 'LapseCount', type: 'int' },
           { name: 'ModerateLapseCount', type: 'int' },
           { name: 'CriticalLapseCount', type: 'int' },
           { name: 'MinorLapseCount', type: 'int' },
           { name: 'HoldCount', type: 'int' }
        ],
        url: '/PreventiveMaintenance/GetSummaryReportData',
        data: { FromDate: fromDate.toISOString(), ToDate: toDate.toISOString(), ScheduleType: $scope.model.ScheduleType, PlantId: 0, LineId: 0 },
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    //$scope.FromDateCreated = function (args) {
    //    $scope.frominstance.setDate(new Date(y, m, 1));
    //}
    //$scope.ToDateCreated = function (args) {
    //    $scope.toinstance.setDate(new Date(y, m + 1, 0));
    //}

    $scope.onPlantBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId };
    }

    $scope.onPlantChange = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId };
    }

    $scope.onSearch = function (e) {
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var LinetId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var selection = $scope.dtpSearchDate.getRange();
        $scope.ReportSource.data = { FromDate: selection.from.toISOString(), ToDate: selection.to.toISOString(), ScheduleType: $scope.model.ScheduleType, PlantId: PlantId, LineId: LinetId };
        $scope.PMSummaryGrid.updatebounddata();
    };

    $scope.onRefreshClick = function (e) {
        $scope.PMSummaryGrid.updatebounddata();
    }

    $scope.onExportClick = function (e) {
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var LinetId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var selection = $scope.dtpSearchDate.getRange();
        window.location = '/Download/SummaryReport?FromDate=' + selection.from.toISOString() + '&ToDate=' + selection.to.toISOString() + '&ScheduleType=' + $scope.model.ScheduleType + '&PlantId=' + PlantId + '&LineId=' + LinetId;
    }
});