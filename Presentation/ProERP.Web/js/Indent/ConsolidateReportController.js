ProApp.controller("IndentBudgetController", function ($scope, $http) {

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
    $scope.PlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
    };
    $scope.onPlantBindingComplete = function (e) {
         $scope.ddlPlant.selectedIndex = 0;
    }
    $scope.onSearchClick = function () {
        LoadIframe();
    };
    var LoadIframe = function () {
        var y = $scope.ddlYear.val();
        var url = "/Reports/ConsolidateReport.aspx?year=" + y;
            $('#frmReport').attr('src', url);
        }

});