ProApp.controller("VendorController", function ($parse, $scope, $http) {
    $scope.SearchVendorName = '';
    $scope.VendorGridSource = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'Address', type: 'string' },
           { name: 'PhoneNo', type: 'string' },
           { name: 'Email', type: 'string' },
        ],
        url: '/Vendor/GetVendorListData',
        data: { Name: $scope.SearchVendorName },
    };
    $scope.onRefreshClick = function (e) {
        $scope.VendorGrid.updatebounddata();
    }
    $scope.UpdateVendor = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.VendorGrid.getrowdata(row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Vendor/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete vendor(s)?', 350, 100, function (e) {
            if (e) {
                var rows = $scope.VendorGrid.selectedrowindexes;
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.VendorGrid.getrowdata(rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }
                $.ajax({
                    dataType: "json",
                    type: 'POST',
                    url: '/Vendor/DeleteVendor',
                    data: {
                        Ids: selectedIds
                    },
                    success: function (response) {
                        $scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        $scope.VendorGrid.updatebounddata();
                    },
                    error: function (jqXHR, exception) {
                    }
                });
            }
        });
    };
    $scope.onSearch = function (event) {
        $scope.VendorGridSource.data = { Name: $scope.SearchVendorName };
    };
});