
ProApp.controller("SiteListController", function ($scope, $http) {
    $scope.SearchSiteName = '';
    $scope.SiteGrid = {};

    $scope.sites = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'InCharge', type: 'string' },
           { name: 'Address', type: 'string' },
           { name: 'Description', type: 'string' }
        ],
        url: '/Site/GetSiteList',
        data: { Name: $scope.SearchSiteName },
        //data: { Name: "kal"},
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };
    // select or unselect rows when the checkbox is clicked.
  
   
    //$scope.gridDataAdapter = new $.jqx.dataAdapter($scope.sites);

    $scope.UpdateSite = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.SiteGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Site/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }
    $scope.onSearch = function (e) {
        $scope.sites.data = { Name: $scope.SearchSiteName };

    };
    $scope.onRefreshClick = function (e) {
        $scope.SiteGrid.jqxGrid('updatebounddata');
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Site(s)?', 350, 100, function (e) {
            if (e) {
               
                var rows = $scope.SiteGrid.jqxGrid('selectedrowindexes');
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.SiteGrid.jqxGrid('getrowdata', rows[m]);
                   
                    if (row != null)
                        selectedIds.push(row.Id);
                }

                $http.post('/Site/DeleteSites', { Ids: selectedIds }).success(function (retData) {
                    if (retData.Message == "Success") {
                       
                        //  $('#lstBatch').jqxListBox('refresh');
                        $scope.SiteGrid.jqxGrid('updatebounddata');

                    } else {
                        $scope.openMessageBox("Error", retData.Message, 350, 100);

                        $scope.SiteGrid.jqxGrid('updatebounddata');

                    }

                }).error(function (retData, status, headers, config) {

                });

            }
            else {

            }
        });
    };
    //bindGrid();
    $scope.createWidget = true;
});
