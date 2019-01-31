ProApp.controller("ItemsController", function ($parse, $scope, $http) {

    $scope.SearchItemName = '';
    $scope.ItemGridSource = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'ItemCode', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'Price', type: 'decimal' },
           { name: 'Make', type: 'string' },
           { name: 'MOC', type: 'string' },
           { name: 'Model', type: 'string' },
           { name: 'UnitOfMeasure', type: 'string' },
           { name: 'AvailableQty', type: 'int' }
          
        ],
        url: '/Item/GetItemGridData',
        data: { Name: $scope.SearchItemName },
    };
    $scope.onRefreshClick = function (e) {
        $scope.ItemGrid.updatebounddata();
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete item(s)?', 350, 100, function (e) {
            if (e) {
                var rows = $scope.ItemGrid.selectedrowindexes;
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.ItemGrid.getrowdata(rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }
                 $.ajax({
                    dataType: "json",
                    type: 'POST',
                    url: '/Item/DeleteItem',
                    data: {
                        Ids: selectedIds
                    },
                    success: function (response) {
                        $scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        $scope.ItemGrid.updatebounddata();
                    },
                    error: function (jqXHR, exception) {
                    }
                });
            }
        });
    };
    $scope.onSearch = function (event) {
        $scope.ItemGridSource.data = { Name: $scope.SearchItemName };
    };
    $scope.UpdateItem = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.ItemGrid.getrowdata(row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Item/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }
}); 