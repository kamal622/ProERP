ProApp.controller("PartController", function ($parse, $scope, $http) {

    //$scope.model = { SiteId: 0, PlantId: 0, LineId: 0, MachineId: 0 }

    //$scope.SiteSource = {
    //    datatype: "json",
    //    datafields: [
    //       { name: 'Id', type: 'int' },
    //       { name: 'Name', type: 'string' }
    //    ],
    //    url: '/Site/GetSites'
    //};

    //$scope.PlantSource = {
    //    datatype: "json",
    //    datafields: [
    //       { name: 'Id', type: 'int' },
    //       { name: 'Name', type: 'string' }
    //    ],
    //    url: '/Plant/GetPlantsForSite',
    //    data: { SiteId: 0 }
    //};

    //$scope.LineSource = {
    //    datatype: "json",
    //    datafields: [
    //       { name: 'Id', type: 'int' },
    //       { name: 'Name', type: 'string' }
    //    ],
    //    url: '/Line/GetLinesForPlant',
    //    data: { PlantId: 0 }
    //};

    //$scope.MachineSource = {
    //    datatype: "json",
    //    datafields: [
    //        { name: 'Id', type: 'int' },
    //        { name: 'Name', type: 'string' }
    //    ],
    //    url: '/Machine/GetMachinesForLine',
    //    data: { LineId: 0 }
    //};

    //$scope.onSiteChange = function (event) {
    //     var selectedId = parseInt($scope.ddlSite.val());
    //     $scope.PlantSource.data = { SiteId: selectedId };
    //};
    //$scope.onSiteBindingComplete = function (e) {
    //    var selectedId = parseInt($scope.ddlSite.val());
    //    $scope.PlantSource.data = { SiteId: selectedId };
    //}

    //$scope.onPlantChange = function (e) {
    //    $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    //}
    //$scope.onPlantBindingComplete = function (e) {
    //    $scope.ddlPlant.val($scope.model.PlantId);
    //}

    //$scope.onLineChange = function (e) {
    //    $scope.MachineSource.data = { LineId: $scope.ddlLine.val() };
    //}
    //$scope.onLineBindingComplete = function (e) {
    //    $scope.ddlLine.val($scope.model.LineId);
    //}
    //$scope.selectHandler = function (event) {
    //    if (event.args) {
    //        var item = event.args.item;
    //        if (item) {
    //            $scope.log = "Label: " + item.label + ", Value: " + item.value;
    //        }
    //    }
    //};
    //$scope.onBindingMachine = function (e) {
    //    $scope.MachineInstance.val($scope.model.MachineId);
    //}

    $scope.OnSavePart = function (e) {

        $scope.model.Id = $('#txtId').val();

        var part = $scope.model;

        //Ajax call to save

        $http.post('/Part/SavePart', { Part: part }).success(function (retData) {

            if (retData.Message == "Success") {

                window.location.href = "/Part/List/";
            } else {

            }

        }).error(function (retData, status, headers, config) {
            alert("status");
        });

    };
    $scope.model.Id = $('#txtId').val();

    $.ajax({
        url: "/Part/GetPartById",
        type: "GET",
        contentType: "application/json;",
        dataType: "json",
        data: { Id: $scope.model.Id },
        success: function (data) {
            //$scope.PlantSource.data = { SiteId: data.SiteId };
            //$scope.LineSource.data = { PlantId: data.PlantId };
            //$scope.MachineSource.data = { LineId: data.LineId };
            $scope.$apply(function (e) {
                $scope.model = data;
            });

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('oops, something bad happened');
        }
    });
});