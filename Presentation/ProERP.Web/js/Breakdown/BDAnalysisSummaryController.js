ProApp.controller("BDAnalysisSummaryController", function ($scope, $http) {

    var y = (new Date()).getFullYear().toString();
    var year = [];
    for (var i = 0; i <= 15; i++) {
        year.push(y.toString());
        y--;
    }
    $scope.YearSource = {
        localdata: year,
        datatype: "array"
    };
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

    $scope.onSiteChange = function (event) {
        var selectedId = parseInt($scope.ddlSite.val());
        $scope.PlantSource.data = { SiteId: selectedId };
    };
    $scope.onSiteBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlSite.val());
        $scope.PlantSource.data = { SiteId: selectedId };
    }

    $scope.onPlantBindingComplete = function (e) {
        $scope.ddlPlant.selectedIndex = 0;
    }
    $scope.onSearchClick = function () {
        LoadIframe();
    };
    var LoadIframe = function () {
        //siteId = $scope.ddlSite.val();
        plantId = $scope.ddlPlant.val();
        
            var y = $scope.ddlYear.val();
            var url = "/Reports/BDAnalysisSummary.aspx?SiteId=1&PlantId=" + plantId + "&year=" + y;
            $('#frmReport').attr('src', url);
        }
       
    
});