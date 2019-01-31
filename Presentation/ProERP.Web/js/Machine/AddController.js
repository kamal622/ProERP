ProApp.controller("MachineAddController", function ($parse, $scope, $http) {

    $scope.Rules = [
             { input: '#dropdownPlantName', message: 'Please select plant', action: 'blur',
                                                                        rule: function (input, commit) {
                                                                            var index = $scope.ddlPlant.getSelectedIndex();
                                                                        return index != -1;
                                                                     }
             },
             { input: '#dropdownLineName', message: 'Please select line', action: 'blur',
                  rule: function (input, commit) {
                      var index = $scope.LineInstance.getSelectedIndex();
                      return index != -1;
                  }
             },
             { input: '#dropdownMachineType', message: 'Please select machine type', action: 'blur',
                  rule: function (input, commit) {
                      var index = $scope.LineInstance.getSelectedIndex();
                      return index != -1;
                  }
              },
             { input: '#txtMachineName', message: 'Machine name is required!', action: 'blur', rule: 'required' }
    ];
    $scope.model = { SiteId: 0 , PlantId: 0, LineId: 0, MachineTypeId: 0, InstallationDate: new Date() };

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

    $scope.MachineTypeSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachineType'
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
        $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    }
    $scope.onPlantBindingComplete = function (e) {
        $scope.ddlPlant.val($scope.model.PlantId);
    }

    $scope.onLineBindingComplete = function (e) {
        $scope.LineInstance.val($scope.model.LineId);
    }

    $scope.onMachineTypeBindingComplete = function (e) {
        $scope.ddlMachineType.val($scope.model.MachineTypeId);
    }

    $scope.onAddMachine = function (e) {
        //Set parameters
        var machine = $scope.model;
        var isValidate = $scope.MachineValidator.validate();
        if (!isValidate)
            return;
        $scope.model.PlantId = $scope.ddlPlant.val();
        //Ajax call to save

        $http.post('/Machine/AddMachine', { machine: machine }).success(function (retData) {
           // debugger;
            if (retData.Message == "Success") {


                window.location.href = "/Machine/List/";
            }
            else {

            }

        }).error(function (retData, status, headers, config) {

        });

    };

    // now create the widget.
    $scope.createWidget = true;

    //$("#ddlPlant").on('change', function (event) { alert('hi'); });

   
});
