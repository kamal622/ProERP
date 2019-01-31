ProApp.controller("IndentsController", function ($parse, $scope, $http) {

    $scope.model = { SiteId: 0, PlantId: 0, PreferredVendorId: 0, VendorCategoryId: 0, Priority:0 }

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
        data: { Id: 1 }
    };

    $scope.VcSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/PreventiveMaintenance/GetVCList',

    };

    $scope.VendorSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/PreventiveMaintenance/GetVendorList',
        data: { VendorCategoryId: 0 }
    };

    $scope.PrioritySource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetPriorityList'
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
        $scope.ddlPlant.val($scope.model.PlantId);
    }

    $scope.onBindingPriority = function (event) {
        $scope.PriorityInstance.val($scope.model.Priority);
    }

    $scope.onVcChange = function (event) {
        $scope.VendorSource.data = { VendorCategoryId: $scope.ddlVC.val() };
    };
    $scope.onBindingVc = function (e) {

        $scope.ddlVC.val($scope.model.VendorCategoryId);
    }

    $scope.onBindingVendor = function (e) {
        if ($scope.model.PreferredVendorId > 0)
            $scope.ddlVendor.val($scope.model.PreferredVendorId);
    }
    $scope.selectHandler = function (event) {
        if (event.args) {
            var item = event.args.item;
            if (item) {
                $scope.log = "Label: " + item.label + ", Value: " + item.value;
            }
        }
    };

    $scope.onSave = function (e) {
        //alert($scope.model.Id);
        $scope.model.Id = $('#txtId').val();

        var Indent = $scope.model;

        //Ajax call to save

        $http.post('/Indents/SaveIndent', { Indent: Indent }).success(function (retData) {
            debugger;

            if (retData.Message == "Success") {

                window.location.href = "/Indents/List/";
            }
            else {
            }

        }).error(function (retData, status, headers, config) {
            alert("status");
        });
      
    };

    $scope.model.Id = $('#txtId').val();
    //alert($scope.model.Id);
    $.ajax({
        url: "/Indents/GetIndentById",
        type: "GET",
        contentType: "application/json;",
        dataType: "json",
        data: { Id: $scope.model.Id },
        success: function (data) {

            $scope.$apply(function () {

                $scope.PlantSource.data = { SiteId: data.SiteId };
                $scope.VendorSource.data = { VendorCategoryId: data.VendorCategoryId };
                $scope.model = data;

            });

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert('oops, something bad happened');
        }
    });
  
















});