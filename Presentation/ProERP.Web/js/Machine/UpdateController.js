
ProApp.controller("MachineUpdateController", function ($parse, $scope, $http) {
    // now create the widget.

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
             { input: '#txtMachineName', message: 'Line name is required!', action: 'blur', rule: 'required' }
    ];
    $scope.model = { SiteId: 0, PlantId: 0, LineId: 0, MachineTypeId: 0 };

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
        url: '/Plant/GetPlantList',
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

    $scope.ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes();
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

    $scope.onUpdateMachine = function (e) {
        //Set parameters
        var machine = $scope.model;
        var isValidate = $scope.MachineValidator.validate();
        if (!isValidate)
            return;

        //Ajax call to save

        $http.post('/Machine/UpdateMachine', { machine: machine }).success(function (retData) {

            if (retData.Message == "Success") {


                window.location.href = "/Machine/List/";
            } else {

            }

        }).error(function (retData, status, headers, config) {

        });
    };

    if ($('#hdnMachineId').val() > 0) {

        $.ajax({
            url: "/Machine/GetMachineById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { MachineId: $('#hdnMachineId').val() },
            cache: false,
            success: function (data) {
                $scope.$apply(function () {
                    $scope.PlantSource.data = { SiteId: data.SiteId };
                    $scope.LineSource.data = { PlantId: data.PlantId };
                    $scope.model = data;
                    $scope.model.InstallationDate = new Date($scope.ToJavaScriptDate(data.InstallationDate));
                });

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

                alert('oops, something bad happened');
            }
        });
    }

    $scope.createWidget = true;


});