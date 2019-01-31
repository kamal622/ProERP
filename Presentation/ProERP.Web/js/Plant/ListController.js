
ProApp.controller("PlantListController", function ($scope, $http) {
    $scope.SearchPlantName = '';
    $scope.PlantGrid = {};

    $scope.plants = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'PlantInCharge', type: 'string' },
           { name: 'Location', type: 'string' },
           { name: 'SiteId', type: 'int' },
           { name: 'SiteName', type: 'string' }
           
        ],
        url: '/Plant/GetPlantList',
        data: { Name: $scope.SearchPlantName, SiteId:0 },
        //data: { Name: "kal"},
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };
    // select or unselect rows when the checkbox is clicked.


    //$scope.gridDataAdapter = new $.jqx.dataAdapter($scope.sites);

    $scope.UpdatePlant = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.PlantGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Plant/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }
    $scope.onSearch = function (e) {
       
        var SiteId = ($("#ddlSite").val()=="" ? 0 : $("#ddlSite").val());
        $scope.plants.data = { Name: $scope.SearchPlantName, SiteId: SiteId };
    };

    $scope.onRefreshClick = function (e) {
        
        $scope.PlantGrid.jqxGrid('updatebounddata');
    }

    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Plant(s)?', 350, 100, function (e) {
            if (e) {
                var rows = $scope.PlantGrid.jqxGrid('selectedrowindexes');
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.PlantGrid.jqxGrid('getrowdata', rows[m]);
                    if(row!=null)
                    selectedIds.push( row.Id);
                }

                $http.post('/Plant/DeletePlant', { Ids: selectedIds }).success(function (retData) {
                    if (retData.Message == "Success") {

                        //  $('#lstBatch').jqxListBox('refresh');
                        $scope.PlantGrid.jqxGrid('updatebounddata');

                    } else {
                        debugger;
                        $scope.openMessageBox("Error", retData.Message, 350, 100);

                        $scope.PlantGrid.jqxGrid('updatebounddata');

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
