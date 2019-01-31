ProApp.controller("BDController", function ($scope, $http) {

    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        //dateTimeInput = args.instance;
        $scope.dtpSearchDate.setRange(firstDay, lastDay);
    }
    $scope.model = { SiteId: 0, PlantId: 0, LineId: 0 };

    $scope.SiteSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Site/GetSites'
    };

    $scope.PlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 0 }
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

    $scope.onSiteChange = function (event) {
        var selectedId = parseInt($scope.ddlSite.val());
        $scope.PlantSource.data = { SiteId: selectedId };
    };
    $scope.onSiteBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlSite.val());
        $scope.PlantSource.data = { SiteId: selectedId };
    }

    $scope.onPlantChange = function (e) {
        $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    }
    $scope.onPlantBindingComplete = function (e) {
        $scope.ddlPlant.selectedIndex = 0;
    }

    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.selectedIndex = 0;
    }

    $scope.Gridcolumns = [
                            { text: 'Sub Assembly', datafield: 'MachineName', width: '150'},
                            { text: 'Description Of Failure', datafield: 'FailureDescription', width: '300'},
                            { text: 'Failure Date', datafield: 'Date',width:'80', cellsformat: 'dd/MM/yyyy' },
                            {
                                text: 'Time Taken(Hrs) ', datafield: 'TotalTime', width: '80', columngroup: 'Time',
                                renderer: function () {
                                    return '<div style="margin-top: 5px; margin-left: 5px;">Time Taken<br />(Hrs)</div>';
                                },
                            cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                               
                                    if (typeof (value) === 'undefined' || value == null || value == '')
                                        return null;
                                    var ms = Math.abs(value);
                                    // 1- Convert to seconds:
                                    var seconds = ms / 1000;
                                    // 2- Extract hours:
                                    var hours = parseInt(seconds / 3600); // 3,600 seconds in 1 hour
                                    seconds = seconds % 3600; // seconds remaining after extracting hours
                                    // 3- Extract minutes:
                                    var minutes = parseInt(seconds / 60); // 60 seconds in 1 minute
                                    // 4- Keep only seconds not extracted to minutes:
                                    seconds = seconds % 60;
                                    if (value >= 0)
                                        return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + '">' + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + '</span>';
                                    else
                                        return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + '">' + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + '</span>';
                                    //return hours + ":" + minutes;

                                }
                            },
                            { text: 'Spares Used', datafield: 'PartsUsed', width: '200' },
                            { text: 'Done By', datafield: 'MenPowerUsed', width: '200' },
                            { text: 'Service Used', datafield: 'ServiceUsed', width: '200' },
                            { text: 'Root Cause', datafield: 'RootCause', width: '200' },
                            { text: 'Correction', datafield: 'Correction', width: '200' },
                            { text: 'Corrective Action', datafield: 'CorrectiveAction', width: '200' },
                            { text: 'Preventive Action', datafield: 'PreventingAction', width: '200' }
    ];


    var siteId = 0;
    if (typeof ($scope.ddlSite) == 'object')
        siteId = $scope.ddlSite.val();
    
    var plantId = 0;
    if (typeof ($scope.ddlPlant) == 'object')
        plantId = $scope.ddlPlant.val();

    var lineId = 0;
    if (typeof ($scope.ddlLine) == 'object')
        lineId = $scope.ddlLine.val();

    var fromDate = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        fromDate = $scope.dtpSearchDate.getRange().from;

    var toDate = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        toDate = $scope.dtpSearchDate.getRange().to;

    $scope.HistoryGrid = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'MachineName', type: 'string' },
            { name: 'FailureDescription', type: 'string' },
            { name: 'Date', type: 'date', format: 'MM/dd/yyyy' },
            { name: 'TotalTime', type: 'int' },
            { name: 'PartsUsed', type: 'string' },
            { name: 'MenPowerUsed', type: 'string' },
            { name: 'ServiceUsed', type: 'string' },
            { name: 'RootCause', type: 'string' },
            { name: 'Correction', type: 'string' },
            { name: 'CorrectiveAction', type: 'string' },
            { name: 'PreventingAction', type: 'string' }
        ],
        url: '/Breakdown/GetListForHistroyReport',
        data: { siteId: parseInt(siteId), plantId: parseInt(plantId), lineId: parseInt(lineId), fromDate: fromDate.toISOString(), toDate: toDate.toISOString() },
        sortcolumn: 'MachineName',
        sortdirection: 'asc'
    };

    $scope.onSearchClick = function () {
        
        if (typeof ($scope.ddlSite) == 'object')
            siteId = $scope.ddlSite.val();

        if (typeof ($scope.ddlPlant) == 'object')
            plantId = $scope.ddlPlant.val();
       
        if (typeof ($scope.ddlLine) == 'object')
            lineId = $scope.ddlLine.val();
      
        var selection = $scope.dtpSearchDate.getRange();

        $scope.HistoryGrid.data = { siteId: parseInt(siteId), plantId: parseInt(plantId), lineId: parseInt(lineId), fromDate: selection.from.toISOString(), toDate: selection.to.toISOString() };
    }
    $scope.onExportClick = function (e) {
        $scope.HistoryReportGrid.exportdata('xls', 'Breakdown');
    }
    $scope.onRefreshClick = function (e) {
        $scope.HistoryReportGrid.updatebounddata();
    }


    $scope.rendertoolbar = function (toolbar) {
        
        var container ="<div>  <h4>hiii</h4> </div>";
					
        toolbar.append(container);
    }
});