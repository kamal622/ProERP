ProApp.controller("MRController", function ($parse, $scope, $http) {

    $scope.IsBreakdown = false;
    $scope.enableProperty = false;
    $scope.IsAssigned = false;
    $scope.InProgress = false;
    $scope.IsBreakdownType = false;
    $scope.IsRemarks = false;
    $scope.IsDownload = false;
    $scope.IsSave = false;
    $scope.model = { SiteId: 0, PlantId: 0, LineId: 0, MachineId: 0 }

    $scope.MaintanceRules = [
           { input: '#txtSerialNo', message: 'SerialNo is required!', action: 'blur', rule: 'required' },
           { input: '#txtProblem', message: 'Problem is required!', action: 'blur', rule: 'required' },
           { input: '#ddlPlant', message: 'Please choose plant', action: 'change',
           rule: function (input, commit) {
               if ($scope.chkBreakdown.checked) {  
                   var index = $scope.ddlWinPlant.getSelectedIndex();
                   return index != -1;
               }
               else
               {
                   $scope.MRValidator.hide();
                   return true;
               }
             }
           },
           { input: '#ddlLine', message: 'Please choose line', action: 'change',
           rule: function (input, commit) {
                    if ($scope.chkBreakdown.checked)
                    {
                        var index = $scope.ddlWinLine.getSelectedIndex();
                        return index != -1;
                    }
                    else
                    {
                        $scope.MRValidator.hide();
                        return true;
                    }
               }
           }
    ];

    $scope.WinPlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
    };
    $scope.WinLineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0 }
    };
    $scope.WinMachineSource = {
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
            { name: 'StatusName', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetStatusList'
    };
    
    $scope.onWinPlantChange = function (e) {
        if ($scope.ddlWinPlant.val() == "")
        {
            $scope.WinLineSource.data = { PlantId: $scope.ddlWinPlant.val($scope.model.PlantId) };
        }
        else
        {
            $scope.WinLineSource.data = { PlantId: $scope.ddlWinPlant.val() };
        }
        
    }
    $scope.onPlantWinBindingComplete = function (e) {
        $scope.ddlWinPlant.val($scope.model.PlantId);
    }
    $scope.onWinLineChange = function (e) {
        if ($scope.ddlWinLine.val() == "")
        {
            $scope.WinMachineSource.data = { LineId: $scope.ddlWinLine.val($scope.model.LineId) };//$scope.model.LineId
        }
        else
        {
            $scope.WinMachineSource.data = { LineId: $scope.ddlWinLine.val() };//$scope.model.LineId
        }
    }
    $scope.onWinLineBindingComplete = function (e) {
        $scope.ddlWinLine.val($scope.model.LineId);
    }
    $scope.onWinBindingMachine = function (e) {
        if ($scope.ddlWinMachine.val() == "")
        {
            $scope.ddlWinMachine.val($scope.model.MachineId);
        }
        else
        {
            $scope.ddlWinMachine.val();
        }
    }

    //below drop down event for Search Box
    $scope.SearchPlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
    };
    $scope.SearchLineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0 }
    };
    $scope.SearchMachineSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachinesForLine',
        data: { LineId: 0 }
    };
    $scope.SearchPrioritySource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetPriorityList'
    };
    $scope.SearchStatusSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'StatusName', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetStatusList'
    };
    $scope.onPlantSearchChange = function (e) {
        $scope.SearchLineSource.data = { PlantId: $scope.ddlSearchPlant.val() };
    }
    $scope.onPlantSearchBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlSearchPlant.val());
        $scope.SearchLineSource.data = { PlantId: selectedId };
    }
    $scope.onLineSearchChange = function (e) {
        $scope.SearchMachineSource.data = { LineId: $scope.ddlSearchLine.val() };
    }
    $scope.onLineSearchBindingComplete = function (e) {
        $scope.ddlSearchLine.val($scope.ddlSearchLine.val());
    }
    $scope.onBindingSearchMachine = function (e) {
        $scope.ddlSearchMachine.val($scope.ddlSearchMachine.val());
    }

    $scope.MRGrid = {};

    $scope.MR = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'SerialNo', type: 'string' },
           { name: 'Problem', type: 'string' },
           { name: 'RequestBy', type: 'int' },
           { name: 'Status', type: 'string' },
           { name: 'Priority', type: 'string' },
           { name: 'RequestUserName', type: 'string' },
           { name: 'AssignUserName', type: 'string' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'IsBreakdown', type: 'bool' }
        ],
        url: '/MaintenanceRequest/GetMRList',
        data: { PlantId: 0,LineId:0,MachineId:0, PriorityId: 0, StatusId: 0 },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'

    };

  

    $scope.onSearch = function (e) {
       
        var PlantId = ($scope.ddlSearchPlant.val() == "" ? 0 : $scope.ddlSearchPlant.val());
        var LineId = ($scope.ddlSearchLine.val() == "" ? 0 : $scope.ddlSearchLine.val());
        var MachineId = ($scope.ddlSearchMachine.val() == "" ? 0 : $scope.ddlSearchMachine.val());
        var PriorityId = ($scope.ddlPrioritySearch.val() == "" ? 0 : $scope.ddlPrioritySearch.val());
        var StatusId = ($scope.ddlStatusSearch.val() == "" ? 0 : $scope.ddlStatusSearch.val());

        $scope.MR.data = { PlantId: PlantId, LineId:LineId, MachineId:MachineId, PriorityId: PriorityId, StatusId: StatusId };
    };
    $scope.onRefreshClick = function (e) {
        $scope.MRGrid.jqxGrid('updatebounddata');
    }
    
    $scope.ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    };

    $scope.onAddClick = function () {
        ClearData();
        $scope.IsSave = true;
        $scope.IsDownload = false;
        $scope.IsAssigned = false;
        $scope.InProgress = false;
        $scope.IsBreakdown = false;
        $scope.IsRemarks = false;
        $scope.dtRequestTime.setDate(new Date());
        $scope.WinAddRequest.open();
    }
    $scope.onBreakdownCheck = function (event) {
        if ($scope.chkBreakdown.checked == true)
        {
            $scope.IsBreakdown = true;
        }
        else
            $scope.IsBreakdown = false;
        
    }
    
    $scope.OnSaveMaitanceRequest = function (event) {

        var isValidate = $scope.MRValidator.validate();
            if (!isValidate)
                return;

        $scope.model.RequestDate = $scope.dtRequestDate.getDate().toISOString();
        $scope.model.RequestTime = $scope.dtRequestTime.getDate().toISOString();//toTimeString("HH/mm/ss");
        $scope.model.PriorityId = $scope.PriorityInstance.getSelectedItem().value;
        $scope.model.BreakdownType = $("input[name='btnBreakdwonType']:checked").val();

        $http.post('/MaintenanceRequest/SaveMR', { MaintenanceRequest: $scope.model, requestTime: $scope.model.RequestTime }).success(function (result) {
            if (result.Status == 1) {
                $scope.grdMR.updatebounddata();
                $scope.WinAddRequest.close();
                $scope.openMessageBox('Success', result.Message, 300, 90);
            } else if (result.Status == 3) {
                $scope.openMessageBox('Error', result.Message, 'auto', 'auto');
            }

        }).error(function (result, status, headers, config) {
            alert("status");
        });
    }
    $scope.model = {};


    $scope.UpdateMaitanceRequest = function (row, column, value) {
        return "<center><a ng-click='EditMR(" + row + ", event)' href='javascript:;'>View </a></center>";
    }
    var MRId = 0;
    $scope.EditMR = function (row, event) {
        var dataRecord = $scope.grdMR.getrowdata(row);
        $.ajax({
            url: "/MaintenanceRequest/GetMRById",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                $scope.$apply(function () {
                    $scope.model = data;
                    MRId = data.Id;
                    $scope.IsAssigned = false;
                     $scope.IsDownload = true;
                    if (data.IsBreakdown == true)
                    {
                        $scope.IsBreakdown = true;
                        $scope.IsBreakdownType = false;
                        $scope.IsRemarks = true;
                        $scope.ddlWinPlant.val($scope.model.PlantId);
                        $scope.ddlWinLine.val($scope.model.LineId);
                        $scope.ddlWinMachine.val($scope.model.MachineId);
                        $scope.WinPlantSource.data = { SiteId: 1 };
                        $scope.WinLineSource.data = { PlantId: $scope.model.PlantId };
                        $scope.WinMachineSource.data = { LineId: $scope.model.LineId };
                        if (data.StatusId == 2 || data.StatusId == 3 || data.StatusId == 4 || data.StatusId == 5) {
                            $scope.enableProperty = true;
                        }
                        else
                            $scope.enableProperty = false;
                    }
                    else {
                        $scope.IsBreakdown = false;
                        $scope.IsBreakdownType = false;
                    }
                    if (typeof (data.RequestDate) != 'undefined' && data.RequestDate != null) {
                        data.RequestDate = new Date($scope.ToJavaScriptDate(data.RequestDate));
                        $scope.dtRequestTime.setDate(data.RequestDate);
                    }
                    if (typeof (data.RequestTime) != 'undefined' && data.RequestTime != null) {
                        data.RequestTime = new Date($scope.ToJavaScriptDate(data.RequestTime));
                        $scope.dtRequestTime.setDate(data.RequestTime);
                    }
                    
                    
                    if (data.BreakdownType != null)
                    {
                        $scope.model.BreakdownType = data.BreakdownType;
                        if (data.BreakdownType == 1)
                            $scope.btnElectrical = true;
                        else if (data.BreakdownType == 2)
                            $scope.btnMechanical = true;
                        else if (data.BreakdownType == 3)
                            $scope.btnInstrument = true;
                    }
                    if (data.StatusId == 2 || data.StatusId == 3 || data.StatusId == 4 || data.StatusId == 5) {
                        $scope.enableProperty = true;
                        $scope.IsRemarks = true;
                        $scope.IsSave = false;
                    }
                    else
                    {
                        $scope.IsSave = true;
                        $scope.IsRemarks = false;
                        $scope.enableProperty = false;
                    }
                    $scope.InProgress = false;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.MRValidator.hide();
        $scope.WinAddRequest.open();
    }

    $scope.ViewAllMRRemarks = function (event) {
        $scope.MRRemarks.data = { mrId: MRId };
        $scope.WinMRRemarks.open();
    }
    $scope.MRRemarks = {
        datatype: "json",
        cache: false,
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'StatusName', type: 'string' },
            { name: 'Remarks', type: 'string' },
            { name: 'RemarksDate', type: 'date' },
            { name: 'RemarksBy', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetMRRemarks',
        data: { mrId: 0 }
    };

    $scope.OnDownloadMaitanceRequest = function (event) {
        window.location.href = "/Download/MaintenanceRequest/?Id=" + MRId;
    }

    var ClearData = function (event) {
        if ((typeof ($scope.model) != 'undefined') && $scope.model != null) {
            $scope.model = {};
            $scope.IsDownload = false;
            $scope.IsRemarks = true;
            $scope.IsBreakdown = false;
            $scope.InProgress = false;
            $scope.enableProperty = false;
            $scope.model.Id = null;
            $scope.model.SerialNo = null;
            $scope.model.Problem = null;
            $scope.model.Description = null;
            $scope.model.RequestDate = new Date();
            $scope.model.RequestTime = new Date();
            $scope.model.IsBreakdown = null;
            $scope.ddlWinPlant.clearSelection();
            $scope.ddlWinLine.clearSelection();  
            $scope.ddlWinMachine.clearSelection();
            $scope.PriorityInstance.selectIndex(1);
            $scope.model.IsCritical = null;
            $scope.MRValidator.hide();
        }
       
    }
});