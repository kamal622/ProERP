ProApp.controller("BreakdownMonthlySummaryController", function ($scope, $http) {

    //$scope.SearchDateCreated = function (args) {
    //    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    //    var firstDay = new Date(y, m, 1);
    //    var lastDay = new Date(y, m + 1, 0);
    //    //dateTimeInput = args.instance;
    //    $scope.dtpSearchDate.setRange(firstDay, lastDay);
    //}
    $("#lblError").text("");
    var y = (new Date()).getFullYear().toString();
    var year = [];
    for (var i = 0; i <= 15; i++)
    {
        year.push(y.toString());
        y--;
    }
    $scope.YearSource = {
                            localdata: year,
                            datatype: "array"
    };
    //var month = [];
    //for (var i = 1; i <= 12; i++) {
    //    var dt = new Date(2001, i, 1).getMonth().toString();
    //    month.push(dt.toString());
    //}
    
    //$scope.MonthSource = {
    //    localdata: month,
    //    datatype: "array"
    //};

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
        data: { SiteId: 1 }
    };
    $scope.lineSettings = {};
    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0, isAll: true }
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
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId, isAll: true };
       
    }
    $scope.onPlantBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId, isAll: true };
    }
    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.val($scope.ddlLine.val());
        $scope.ddlLine.selectedIndex = 0;
        //LoadIframe();
    }
  

    $scope.onSearchClick = function () {
       
        LoadIframe();
    };

    var LoadIframe = function () {
        //siteId = $scope.ddlSite.val();
        plantId = $scope.ddlPlant.val();
        lineId = $scope.ddlLine.val();
        month = $scope.ddlMonth.val();
        if (lineId == "")
            $("#lblError").text("Please choose line.");
        else
        {
             $("#lblError").text("");
        var y = $scope.ddlYear.val();
        var url = "/Reports/MonthlyBreakdownReport.aspx?SiteId=1&PlantId=" + plantId + "&LineId=" + lineId + "&year=" + y + "&Month=" + month;
        //var selection = $scope.dtpSearchDate.getRange();
       // var url = "/Reports/MonthlyBreakdownReport.aspx?SiteId=" + siteId + "&PlantId=" + plantId + "&LineId=" + lineId + "&StartDate=" + selection.from.toISOString() + "&EndDate=" + selection.to.toISOString();
        $('#frmReport').attr('src', url);
            }
    }
});