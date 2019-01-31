ProApp.controller("SubAssemblyListController", function ($scope, $http) {


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

    $scope.MachineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachinesForLine',
        data: { LineId: 0 }
    };


    $scope.onSelectSite = function (event) {

        $scope.PlantSource.data = { SiteId: $scope.SiteInstance.val() };
    };
    $scope.onSelectPlant = function (event) {

        $scope.LineSource.data = { PlantId: $scope.PlantInstance.val() };
        $scope.MachineSource.data = { LineId: 0 };
    };
    $scope.onSelectLine = function (event) {

        $scope.MachineSource.data = { LineId: $scope.LineInstance.val() };
    };

    $scope.SubAssemblyGrid = {};

    $scope.SubAssemblies = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'Description', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'LineId', type: 'int' },
           { name: 'MachineId', type: 'int' }


        ],
        url: '/SubAssembly/GetSubAssemblyList',
        data: { Name: $scope.SearchName, SiteId: 0, PlantId: 0, LineId: 0, MachineId: 0 },
        //data: { Name: "kal"},
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    //onSearch
    $scope.onSearch = function (e) {

        var SiteId = ($scope.SiteInstance.val() == "" ? 0 : $scope.SiteInstance.val());
        var PlantId = ($scope.PlantInstance.val() == "" ? 0 : $scope.PlantInstance.val());
        var LineId = ($scope.LineInstance.val() == "" ? 0 : $scope.LineInstance.val());
        var MachineId = ($scope.MachineInstance.val() == "" ? 0 : $scope.MachineInstance.val());

        $scope.SubAssemblies.data = { Name: $scope.SearchMachineName, SiteId: SiteId, PlantId: PlantId, LineId: LineId, MachineId: MachineId };
    };

    $scope.UpdateSubAssembly = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.SubAssemblyGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/SubAssembly/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }


    $scope.createWidget = true;
});