
ProApp.controller("SubAssemblyUpdateController", function ($parse, $scope, $http) {

    // now create the widget.

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

    $scope.MachineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachinesForLine',
        data: { LineId: 0 }
    };
    debugger;

    if ($('#hdnSubAssemblyId').val() > 0) {
        debugger;
        $.ajax({
            url: "/SubAssembly/GetSubAssemblyById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { SubAssemblyId: $('#hdnSubAssemblyId').val() },
            success: function (data) {
                debugger;
                $scope.$apply(function () {

                    // $scope.model = data;
                    debugger;
                    
                    $scope.PlantSource.data = { SiteId: data.SiteId };
                    $scope.LineSource.data = { PlantId: data.PlantId };
                    $scope.MachineSource.data = { LineId: data.LineId };
                    $scope.model = data;
                   
                  
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.onSelectPlant = function (event) {

        $scope.LineSource.data = { PlantId: $scope.PlantInstance.val() };
    };
    $scope.onSelectSite = function (event) {

        $scope.PlantSource.data = { SiteId: $scope.SiteInstance.val() };
    };
    $scope.onSelectLine = function (event) {

        $scope.MachineSource.data = { LineId: $scope.LineInstance.val() };
    };

    $scope.onBindingPlant = function (event) {
        $scope.SiteInstance.val($scope.model.
            SiteId);

        $scope.PlantInstance.val($scope.model.PlantId);
    }
    $scope.onBindingLine = function (event) {
        $scope.LineInstance.val($scope.model.LineId);
    }
    $scope.onBindingMachine = function (event) {
        $scope.MachineInstance.val($scope.model.MachineId);
    }

    $scope.createWidget = true;

});