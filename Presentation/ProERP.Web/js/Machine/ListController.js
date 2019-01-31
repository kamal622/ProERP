
ProApp.controller("MachineListController", function ($scope, $http) {
    //$("#ddlPlant").on('change', function (event) {
    //    alert('hi');
    //});
    $scope.SearchMachineName = '';
    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0 },
        Id: "Id"
     
    };
    
    $scope.MachineGrid = {};

    $scope.machines = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'MachineInCharge', type: 'string' },
           { name: 'Make', type: 'string' },
           { name: 'Model', type: 'string' },
           { name: 'InstallationDate', type: 'date' },
           { name: 'MachineInCharge', type: 'string' },
           { name: 'PlantName', type: 'string' },
           { name: 'MachineTypeName', type: 'string' },
           { name: 'LineId', type: 'int' },
           { name: 'LineName', type: 'string' },
           { name: 'IsActive', type: 'bool' },

        ],
        url: '/Machine/GetMachineList',
        data: { Name: $scope.SearchMachineName, SiteId: 0, PlantId: 0,LineId:0 },
        //data: { Name: "kal"},
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    $scope.plantSettings = {
        width : 300,
        height: 33
    };

    //$('#ddlPlant').on('select', onSelect);
    //jqx_width = 300, jqx_height = 33, jqx_theme = "theme", id = "ddlPlant", jqx_instance = "listBox", jqx_on_select = "onSelect(event)"

    $scope.UpdateMachine = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.MachineGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Machine/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }
    $scope.onSearch = function (e) {
      
       // alert($scope.SearchMachineName);

        var SiteId = ($scope.ddlSite.val() == "" ? 0 : $scope.ddlSite.val());
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
      //  var LineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        $scope.machines.data = { Name: $scope.SearchMachineName, SiteId: SiteId, PlantId: PlantId, LineId: 0 };
    };
    $scope.onRefreshClick = function (e) {
        $scope.MachineGrid.jqxGrid('updatebounddata');
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Machine(s)?', 350, 100, function (e) {
            if (e) {
                var rows = $scope.MachineGrid.jqxGrid('selectedrowindexes');
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.MachineGrid.jqxGrid('getrowdata', rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }

                $http.post('/Machine/DeleteMachine', { Ids: selectedIds }).success(function (retData) {
                    if (retData.Message == "Success") {

                        //  $('#lstBatch').jqxListBox('refresh');
                        $scope.MachineGrid.jqxGrid('updatebounddata');

                    } else {
                       
                        $scope.openMessageBox("Error", retData.Message, 350, 100);

                        $scope.MachineGrid.jqxGrid('updatebounddata');

                    }

                }).error(function (retData, status, headers, config) {

                });

            }
            else {

            }
        });
    };
    $scope.onSelect = function (event) {
        $scope.LineSource.data = { PlantId: parseInt($scope.ddlPlant.val()) };
    };
    $scope.selectItem = function (event) {
        alert("hi");
        debugger;
        if (event.args) {
            var item = event.args.item;
            if (item) {
                $scope.log = "Value: " + item.value + ", Label: " + item.label;
            }
        }
    };

    //$('#ddlSite').on('select', function (event) {
    //    debugger;
    //    alert("hi");
    //    var args = event.args;
    //    if (args) {
    //        // index represents the item's index.                
    //        var index = args.index;
    //        var item = args.item;
    //        // get item's label and value.
    //        var label = item.label;
    //        var value = item.value;
    //    }
    //});


    $scope.createWidget = true;
});