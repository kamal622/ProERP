ProApp.controller("PartController", function ($scope, $http) {

    //$scope.model = { SiteId: 0, PlantId: 0, LineId: 0, MachineId: 0 }
    $scope.SearchPartName = '';
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
    //    var selectedId = parseInt($scope.ddlSite.val());
    //    $scope.PlantSource.data = { SiteId: selectedId };
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

    $scope.PartGrid = {};

    $scope.Part = {
        datatype: "json",
        datafields: [
          { name: 'Id', type: 'int' },
          { name: 'Name', type: 'string' }
          //{ name: 'PlantName', type: 'string' },
          //{ name: 'LineName', type: 'string' },
          //{ name: 'MachineName', type: 'string' }
        ],
        data: { Name: $scope.SearchPartName},
        url: '/Part/GetPartList'
        //data: { SiteId:0, PlantId: 0,LineId:0, MachineId: 0 },
        //Id: "Id",
        //sortcolumn: 'Name',
        //sortdirection: 'asc'
    };

    $scope.onSearch = function (e) {
        //var SiteId = ($scope.ddlSite.val() == "" ? 0 : $scope.ddlSite.val());
        //var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        //var LineId = ($scope.ddlLine.val() == "" ? 0 : $scope.ddlLine.val());
        //var MachineId = ($scope.MachineInstance.val() == "" ? 0 : $scope.MachineInstance.val());
        $scope.Part.data = { Name: $scope.SearchPartName };
    };

    $scope.onRefreshClick = function (e) {
        $scope.PartGrid.jqxGrid('updatebounddata');
    }

    $scope.UpdatePart = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.PartGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Part/Update/" + dataRecord.Id + "'> Edit/View </a>";

    }

    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Part(s)?', 350, 100, function (e) {
            if (e) {
               
                var rows = $scope.PartGrid.jqxGrid('selectedrowindexes');
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.PartGrid.jqxGrid('getrowdata', rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }

                $http.post('/Part/DeletePart', { Ids: selectedIds }).success(function (retData) {
                    if (retData.Message == "Success") {

                        //  $('#lstBatch').jqxListBox('refresh');
                        $scope.PartGrid.jqxGrid('updatebounddata');

                    } else {
                        debugger;
                        $scope.openMessageBox("Error", retData.Message, 350, 100);

                        $scope.PartGrid.jqxGrid('updatebounddata');

                    }

                }).error(function (retData, status, headers, config) {

                });

            }
            else {

            }
        });
    };

    $scope.createWidget = true;
});