ProApp.controller("PMController", function ($parse, $scope, $http) {
    
    //$scope.Report1Grid = {
    //    bindingcomplete: function () {
    //        $scope.Report1Grid.jqxGrid('expandallgroups');
    //    },
    //};
    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
        $scope.dtpSearchDate.setRange(firstDay, lastDay);
        $scope.PMSummaryGrid.updatebounddata();
    }
    $scope.model = { ScheduleType: 0, Activity: 1 }
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
    $scope.onPlantChange = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId };
    }
    $scope.onPlantBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId };
    }
    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.val($scope.ddlLine.val());
    }
    $scope.FromDateCreated = function (args) {
        $scope.frominstance.setDate(new Date(y, m, 1));
    }
    $scope.ToDateCreated = function (args) {
        $scope.toinstance.setDate(new Date(y, m + 1, 0));
    }

    var OnGridBindingComplete = function () {
        $scope.PMSummaryGrid.expandallgroups();
    };
    //var selection = $scope.dtpSearchDate.getRange();

    var fromDate = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        fromDate = $scope.dtpSearchDate.getRange().from;

    var toDate = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        toDate = $scope.dtpSearchDate.getRange().to;

    $scope.Report1 = {
        datatype: "json",
        autoBind: false,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'WorkName', type: 'string' },
           { name: 'Notes', type: 'string' },
           { name: 'ScheduleTypeName', type: 'string' },
           { name: 'UserName', type: 'string' },
           { name: 'AssignedTo', type: 'string' },
           { name: 'ReviewDate', type: 'date' },
           { name: 'NextReviewDate', type: 'date' },
           { name: 'ScheduledReviewDate', type: 'date' }
        ],
        url: '/PreventiveMaintenance/GetListForReport1',
        data: { FromDate: fromDate.toISOString(), ToDate: toDate.toISOString(), ScheduleType: $scope.model.ScheduleType, PlantId: 0, LinetId: 0, Activity: $scope.model.Activity, WorkDescription : '' },//selection.from.toISOString(),selection.to.toISOString()
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    $scope.GridColumns = [{ text: 'Line Name', datafield: 'LineName' },
                            { text: 'Machine Name', datafield: 'MachineName' },
                            { text: 'Work Description', datafield: 'WorkName' },
                            { text: 'Schedule', datafield: 'ScheduleTypeName', width: 80 },
                            { text: 'Assign To', datafield: 'AssignedTo', width: 100, hidden: true },
                            { text: 'Scheduled Date', datafield: 'ScheduledReviewDate', width: 110, cellsformat: 'dd/MM/yyyy' },
                            { text: 'Review By', datafield: 'UserName', width: 140 },
                            { text: 'Review Date', datafield: 'ReviewDate', width: 130, cellsformat: 'dd/MM/yyyy HH:mm:ss' },
                            { text: 'Note', datafield: 'Notes', width: 200 }
    ];

    $scope.onRefreshClick = function (e) {
        $scope.PMSummaryGrid.updatebounddata();
    }

    $scope.onExportClick = function (e) {
        var selection = $scope.dtpSearchDate.getRange();
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var LineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var workdescription = $scope.model.WorkDescription;
        //$.ajax({
        //    url: "/Download/PreventiveHistory",
        //    async: false,
        //    type: "GET",
        //    dataType: "json",
        //    data: { FromDate: $scope.model.FromDate.toISOString(), ToDate: $scope.model.ToDate.toISOString(), ScheduleType: $scope.model.ScheduleType, PlantId: 0, LinetId: 0 },
        //    success: function (response) {
        //    },
        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //        alert('oops, something bad happened');
        //    }
        //});
        //'/Download/PreventiveHistory?FromDate=2017-04-30T18%3A30%3A00.000Z&ToDate=2017-05-30T18%3A30%3A00.000Z&ScheduleType=0&PlantId=0&LinetId=0'
        window.location = '/Download/PreventiveHistory?FromDate=' + selection.from.toISOString() + '&ToDate=' + selection.to.toISOString() + '&ScheduleType=' + $scope.model.ScheduleType + '&PlantId=' + PlantId + '&LinetId=' + LineId + '&Activity=' + $scope.model.Activity + '&WorkDescription=' + workdescription;
    }
    
    $scope.onSearch = function (e) {
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var LinetId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        var selection = $scope.dtpSearchDate.getRange();
        var workdescription = $scope.model.WorkDescription;
        $scope.Report1.data = { FromDate: selection.from.toISOString(), ToDate: selection.to.toISOString(), ScheduleType: $scope.model.ScheduleType, PlantId: PlantId, LinetId: LinetId, Activity: $scope.model.Activity, WorkDescription: workdescription };//
    };

});