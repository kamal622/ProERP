ProApp.controller("MRController", function ($parse, $scope, $http) {

    $scope.model = { PlantId: 0, LineId: 0, MachineId: 0 }
    $scope.IsCd = true;
    $scope.PlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
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

    $scope.PrioritySource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetPriorityList'
    };
    $scope.StatusSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetStatusList'
    };

    $scope.onPlantChange = function (e) {
        $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    }
    $scope.onPlantBindingComplete = function (e) {
            $scope.ddlPlant.val($scope.model.PlantId);
    }

    $scope.onLineChange = function (e) {
        $scope.MachineSource.data = { LineId: $scope.ddlLine.val() };
    }
    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.val($scope.model.LineId);
    }

    $scope.onBindingMachine = function (e) {
        $scope.ddlMachine.val($scope.model.MachineId);
    }
    $scope.onBindingPriority = function (event) {
        $scope.PriorityInstance.val($scope.model.PriorityId);
    }

    $scope.onStatusChange = function (e) {
        var item = $scope.ddlStatus.getSelectedItem();
        var status = item.label;
        if (status == "Open") {
            $scope.IsCd = true;
        }
        else if (status == "Close") {
            $scope.IsCd = false;
        }
        else if (status == "InProcess") {
            $scope.IsCd = true;
        }
    }
    $scope.onBindingStatus = function (event) {
        if (typeof ($scope.model.StatusId) != 'undefined')
            $scope.ddlStatus.val($scope.model.StatusId);
        else
            $scope.model.StatusId = parseInt($scope.ddlStatus.val());
    }
    
    $scope.ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes();
    };

    $scope.OnSaveMR = function (e) {
        
        $scope.model.Id = $('#txtId').val();
        var item = $scope.ddlStatus.getSelectedItem();
        var status = item.label;
        $scope.model.OpenDateTime = new Date($scope.dtOpen.getDate()).toISOString();
        if (status == "Close")
            $scope.model.CloseDateTime = new Date($scope.dtClose.getDate()).toISOString();
        else
            $scope.model.CloseDateTime = null;
        
        var MaintenanceRequest = $scope.model;
        
        //Ajax call to save
        if (status == "Close" && (MaintenanceRequest.CloseDateTime == 'undefined' || MaintenanceRequest.CloseDateTime == null)) {
            $scope.openMessageBox('Warning', 'Please choose close date&time.', 350, 90);
        }
        else if (MaintenanceRequest.PlantId == null|| MaintenanceRequest.PlantId == 0) {
            $scope.openMessageBox('Warning', 'Please choose plant.', 350, 90);
        }
        else if (MaintenanceRequest.OpenDateTime == 'undefined' || MaintenanceRequest.OpenDateTime == null) {
            $scope.openMessageBox('Warning', 'Please choose open date&time.', 350, 90);
        }
        else if (MaintenanceRequest.TaskDescription == 'undefined' || MaintenanceRequest.TaskDescription == '') {
            $scope.openMessageBox('Warning', 'Please inset task description.', 350, 90);
        }
        else
        $.ajax({
            url: "/MaintenanceRequest/SaveMR",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { MaintenanceRequest: MaintenanceRequest },
            success: function (response) {
                if (response.Status == 1) {
                    // Success
                    window.location.href = "/MaintenanceRequest/List/";
                }
                else {
                    // Error
                    $scope.openMessageBox('Error', response.Message, 700, 500);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    };

    angular.element(document).ready(function () {
        $scope.model.Id = parseInt($('#txtId').val());
        if ($scope.model.Id > 0) {
            $.ajax({
                url: "/MaintenanceRequest/GetMRById",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { Id: $scope.model.Id },
                success: function (data) {
                    $scope.$apply(function () {
                        $scope.PlantSource.data = { SiteId: 1 };
                        $scope.LineSource.data = { PlantId: data.PlantId };
                        $scope.MachineSource.data = { LineId: data.LineId };
                        if (typeof (data.OpenDateTime) != 'undefined' && data.OpenDateTime != null) {
                            data.OpenDateTime = new Date($scope.ToJavaScriptDate(data.OpenDateTime));
                            $scope.dtOpen.setDate(data.OpenDateTime);
                        }
                        if (typeof (data.CloseDateTime) != 'undefined' && data.CloseDateTime != null) {
                            data.CloseDateTime = new Date($scope.ToJavaScriptDate(data.CloseDateTime));
                            $scope.dtClose.setDate(data.CloseDateTime);
                        }
                        $scope.model = data;

                    });

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
        }
    });
});