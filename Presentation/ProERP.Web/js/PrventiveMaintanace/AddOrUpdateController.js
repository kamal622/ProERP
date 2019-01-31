ProApp.controller("PMController", function ($parse, $scope, $http) {

    $scope.model = { SiteId: 0, PlantId: 0, LineId: 0, MachineId: 0, UserId: 0, WorkId: 0, VendorCategoryId: 0, PreferredVendorId: 0, ScheduleStartDate: new Date() }
    $scope.IsSite = false;
    $scope.checked = false;
    $scope.IsPeriod = true;
    $scope.enableRadioBtn = false;
    $scope.Rules = [
            { input: '#txtWorkDisc', message: 'Work discription is required!', action: 'blur', rule: 'required' },
            {   input: '#plantid', message: 'Please choose plant', action: 'change',
                rule: function (input, commit) {
                    var index = $scope.ddlPlant.getSelectedIndex();
                    return index != -1;          }
            },
            {   input: '#lineId', message: 'Please choose line', action: 'change',
                rule: function (input, commit) {
                     var index = $scope.ddlLine.getSelectedIndex();
                     return index != -1;
                 }
            },
            {   input: '#machineId', message: 'Please choose machine', action: 'change',
                rule: function (input, commit) {
                    var index = $scope.MachineInstance.getSelectedIndex();
                    return index != -1;
                }
            }
            //{
            //    input: '#assignto', message: 'please choose assign to user', action: 'change',
            //    rule: function (input, commit) {
            //        debugger;
            //        var index = $scope.UserInstance.getselectedindex();
            //        return index != -1;
            //    }
            //}
            
    ];

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

    $scope.UserSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'UserName', type: 'string' }
        ],
        url: '/PreventiveMaintenance/GetUserList'
    };

    $scope.PwdSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/PreventiveMaintenance/GetPWDList'
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

    $scope.onLineChange = function (e) {
        $scope.MachineSource.data = { LineId: $scope.ddlLine.val() };
    }
    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.val($scope.model.LineId);
    }

    $scope.onBindingMachine = function (e) {
        $scope.MachineInstance.val($scope.model.MachineId);
    }

    $scope.onSeverityBindingComplete = function (e) {
        if (typeof ($scope.model.Severity) != 'undefined')
            $scope.ddlSeverity.val($scope.model.Severity);
        else
            $scope.model.Severity = parseInt($scope.ddlSeverity.val());
    }

    $scope.onBindingWork = function (e) {
        if ($scope.model.WorkId > 0)
            $scope.PwdInstance.val($scope.model.WorkId);
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

    $scope.comboboxSettings = { selectedIndex: 0 };
    $scope.selectHandler = function (event) {
        if (event.args) {
            var item = event.args.item;
            if (item) {
                $scope.log = "Label: " + item.label + ", Value: " + item.value;
            }
        }
    };



    $scope.OnSavePM = function (e) {

        //Set parameters
        $scope.model.Id = $('#txtId').val();

        //alert($scope.model.Id)

        var isValidate = $scope.PMValidator.validate();
        if (!isValidate)
            return;

        var PreventiveMaintenance = $scope.model;

        var UserIds = [];

        var items = $scope.UserInstance.getCheckedItems();
        $.each(items, function (index) {
            UserIds.push(this.value);
        });
        //Ajax call to save

        $http.post('/PreventiveMaintenance/SavePM', { PreventiveMaintenance: PreventiveMaintenance, UserIds: UserIds }).success(function (retData) {
            if (retData.Message == "Success") {
                window.location.href = "/PreventiveMaintenance/List/";
            } else {
            }

        }).error(function (retData, status, headers, config) {
            alert("status");
        });
    };

   
    $scope.onBindingUser = function (e) {
        if ($scope.model.UserId != null) {
            for (i = 0; i < $scope.model.UserId.length; i++) {
                var userId = $scope.model.UserId[i];
                var item = $scope.UserInstance.getItemByValue(userId);
                $scope.UserInstance.checkItem(item);
            }
        }
      

    }

    $scope.model.Id = $('#txtId').val();
    if ($scope.model.Id != 0 && $scope.model.Id != null) {
        $scope.enableRadioBtn = true;
        $.ajax({
            cache: false,
            url: "/PreventiveMaintenance/GetPMById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Id: $scope.model.Id },
            success: function (data) {
                $scope.$apply(function () {
                    $scope.PlantSource.data = { SiteId: data.SiteId };
                    $scope.LineSource.data = { PlantId: data.PlantId };
                    $scope.MachineSource.data = { LineId: data.LineId };
                    $scope.VendorSource.data = { VendorCategoryId: data.VendorCategoryId };
                    $scope.model = data;

                    //alert($scope.model);
                    $scope.model.ScheduleStartDate = new Date($scope.ToJavaScriptDate(data.ScheduleStartDate));
                    if (typeof (data.ScheduleEndDate) != 'undefined' && data.ScheduleEndDate != null) {
                        $scope.model.ScheduleEndDate = new Date($scope.ToJavaScriptDate(data.ScheduleEndDate));
                        $scope.checked = true;
                    }
                });

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

                alert('oops, something bad happened');
            }
        });
    }
    else
        $scope.enableRadioBtn = false;
   

    $scope.OnScheduleTypeChanged = function (event) {
        if ($scope.model.ScheduleType == 1) {
            $scope.IsPeriod = true;
            $scope.Period = "Day(s)";
        }
        else if ($scope.model.ScheduleType == 2) {
            $scope.IsPeriod = true;
            $scope.Period = "Week(s)";
        }
        else if ($scope.model.ScheduleType == 3) {
            $scope.IsPeriod = true;
            $scope.Period = "Month(s)";
        }
        else if ($scope.model.ScheduleType == 4) {
            $scope.IsPeriod = true;
            $scope.Period = "Year(s)";
        }
        else if ($scope.model.ScheduleType == 5) {
            $scope.model.Interval = 0;
            $scope.IsPeriod = false;
        }
        else if ($scope.model.ScheduleType == 6) {
            $scope.IsPeriod = true;
            $scope.Period = "Hourly(s)";
        }
    }
});