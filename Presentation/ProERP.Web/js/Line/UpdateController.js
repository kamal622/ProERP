
ProApp.controller("LineUpdateController", function ($parse, $scope, $http) {

    $scope.Rules = [
        { input: '#dropPlantName', message: 'Please select plant', action: 'blur',
            rule: function (input, commit) {
                var index = $scope.ddlPlantName.getSelectedIndex();
                return index != -1;
            }
        },
       { input: '#txtLineName', message: 'Line name is required', action: 'blur', rule: 'required' }
    ];
    $scope.model = { SiteId: 0, PlantId: 0 };

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
    $scope.onSiteChange = function (event) {
        $scope.PlantSource.data = { SiteId: $scope.ddlSite.val() };
    };
    $scope.onSiteBindingComplete = function (e) {
        $scope.model.Id = $('#txtId').val();
        $.ajax({
            url: "/Line/getModelById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Id: $scope.model.Id },
            success: function (data) {
                $scope.$apply(function () {
                    $scope.ddlSite.val($scope.model.SiteId);
                    $scope.PlantSource.data = { SiteId: data.SiteId };
                    $scope.model = data;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.onPlantBindingComplete = function (e) {
        $scope.ddlPlantName.val($scope.model.PlantId);
    }

    $scope.onUpdateLine = function (e) {
        //Set parameters
        var isValidate = $scope.LineValidator.validate();
        if (!isValidate)
            return;
        var line = $scope.model;
        
        $scope.model.PlantId = $scope.ddlPlantName.val();
        //Ajax call to save

        $http.post('/Line/UpdateLine', { line: line }).success(function (retData) {
            if (retData.Message == "Success") {
                window.location.href = "/Line/List/";
            }
            else {

            }

        }).error(function (retData, status, headers, config) {

        });

    };

    
    // now create the widget.

    $scope.createWidget = true;


});