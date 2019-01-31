ProApp.controller("HistoryCardListController", function ($scope, $http) {
    $("#lblError").text("");
    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
         $scope.dtpSearchDate.setRange(firstDay, lastDay);
        
    }

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

    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0,}
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
        $scope.LineSource.data = { PlantId: selectedId };
    }
    $scope.onPlantBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId };
    }
    $scope.onLineChange = function (e) {
        $scope.MachineSource.data = { LineId: $scope.ddlLine.val() };
    }
    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.selectedIndex = 0;
        //$scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
        //LoadIframe();
    }
    $scope.onBindingMachine = function (e) {
        $scope.ddlMachine.val($scope.ddlMachine.val());
    }

    $scope.onSearchClick = function () {
        LoadIframe();
    };

    var LoadIframe = function () {
        //siteId = $scope.ddlSite.val();
        plantId = $scope.ddlPlant.val();
        lineId = $scope.ddlLine.val();
       
        if (lineId == "")
            $("#lblError").text("Please choose line.");
        else 
        {
            machineId = $scope.ddlMachine.val();
        var selection = $scope.dtpSearchDate.getRange();
        if (selection.from ==null || selection.to==null)
            $("#lblError").text("Please choose date.");
        else
        {
            $("#lblError").text("");
            var url = "/Reports/HistoryCard.aspx?SiteId=1&PlantId=" + plantId + "&LineId=" + lineId + "&MachineId=" + machineId + "&StartDate=" + selection.from.toISOString() + "&EndDate=" + selection.to.toISOString();
            $('#frmReport').attr('src', url);//"&MachineId=" + machineId  +
        }
        }
    }
});