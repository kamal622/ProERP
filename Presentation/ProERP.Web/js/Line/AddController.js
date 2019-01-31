ProApp.controller("LineAddController", function ($parse, $scope, $http) {

    $scope.Rules = [
             { input: '#dropPlantName', message: 'Please select plant', action: 'blur', 
                                                                        rule: function (input, commit) {
                                                                            var index = $scope.ddlPlantName.getSelectedIndex();
                                                                        return index != -1;
                                                                     }
             },
             { input: '#txtLineName', message: 'Line name is required!', action: 'blur', rule: 'required' }
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
        data: { SiteId: 1 }
    };

    $('#chkIsActive').on('checked', function () {
        alert("You checked the box")
    });

    $scope.onSiteChange = function (event) {
        var selectedId = parseInt($scope.ddlSite.val());
        $scope.PlantSource.data = { SiteId: selectedId };
    };
    //$scope.onSiteBindingComplete = function (e) {
    //    var selectedId = parseInt($scope.ddlSite.val());
    //    $scope.PlantSource.data = { SiteId: selectedId };
    //}

    $scope.onPlantBindingComplete = function (e) {
        $scope.ddlPlantName.val($scope.model.PlantId);
    }

    $scope.onAddLine = function (e) {
       // debugger;
        //Set parameters
        var line = $scope.model;
        var isValidate = $scope.LineValidator.validate();
        if (!isValidate)
            return;
        $scope.model.PlantId = $scope.ddlPlantName.val();

        //Ajax call to save

        $http.post('/Line/AddLine', { line: line }).success(function (retData) {
            //debugger;
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