ProApp.controller("SubAssemblyAddController", function ($parse, $scope, $http) {


    $scope.model = { SiteId: 0, PlantId: 0 ,LineId:0,MachineId:0};
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

    $scope.MachineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachinesForLine',
        data: { LineId: 0 }
    };

    $scope.onSelectPlant = function (event) {

        $scope.LineSource.data = { PlantId: $scope.PlantInstance.val() };
    };
    $scope.onSelectSite = function (event) {

        $scope.PlantSource.data = { SiteId: $scope.SiteInstance.val() };
    };
    $scope.onSelectLine = function (event) {

        $scope.MachineSource.data = { LineId: $scope.LineInstance.val() };
    };


    $scope.onAddSubAssembly = function (e) {
        //Set parameters
        debugger;
        var SubAssembly = $scope.model;

        //Ajax call to save

        $http.post('/SubAssembly/AddSubAssembly', { SubAssembly: SubAssembly }).success(function (retData) {

            if (retData.Message == "Success") {
                window.location.href = "/SubAssembly/List/";
            } else {

            }

        }).error(function (retData, status, headers, config) {

        });
    };

    // now create the widget.

    $scope.createWidget = true;


});