ProApp.controller("RMItemController", function ($scope, $http, $timeout) {

    $scope.ItemRules = [
          { input: '#txtItemName', message: 'Item name is required', action: 'blur', rule: 'required' },
           {
               input: '#ddlItemCategory', message: 'Please choose Category', action: 'change',
               rule: function (input, commit) {
                   var index = $scope.ddlItemCategory.getSelectedIndex();
                   return index != -1;
               }
           }
    ];

    $scope.onAddClick = function () {
        ClearData();
        $scope.ItemValidator.hide();
        $scope.WinAddOrUpdateItem.open();
    }

    $scope.ItemSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'ItemCode', type: 'string' },
           { name: 'CategoryId', type: 'int' },
           { name: 'CategoryName', type: 'string' },
        ],
        url: '/RMItem/GetItemList',
        data: { itemName: '' },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    $scope.ItemCategorySource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/RMItem/GetItemCategoryList',
    };

    $scope.UOMSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Measurement', type: 'string' }
        ],
        url: '/RMItem/GetMOUList',
    };

    $scope.onSearch = function () {
        var itemName = $scope.SearchItemName;
        $scope.ItemSource.data = { itemName: itemName };
    }

    $scope.onRefresh = function () {
        $scope.GrdItem.updatebounddata();
    }

    $scope.onSaveItem = function () {

        var isValidate = $scope.ItemValidator.validate();
        if (!isValidate)
            return;

        var itemData = $scope.model;
        itemData.CategoryId = $scope.ddlItemCategory.getSelectedItem().value;
        $http.post('/RMItem/SaveItems', { itemData: $scope.model }).then(function (result) {
            if (result.data.Type == "Success") {
                $scope.GrdItem.updatebounddata();
                //$scope.openMessageBox('Success', result.data.Message, 300, 90);
                ClearData();
                $scope.WinAddOrUpdateItem.close();
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    }

    $scope.UpdateItem = function (row, column, value) {
        return "<center><a ng-click='EditItem(" + row + ", event)' class='fa fa-edit fa-2' href='javascript:;'></a></center>";
    }

    $scope.EditItem = function (row, event) {
        var dataRecord = $scope.GrdItem.getrowdata(row);
        $.ajax({
            url: "/RMItem/GetItemById",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                $scope.$apply(function () {
                    $scope.model = data;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.ItemValidator.hide();
        $scope.WinAddOrUpdateItem.open();
    }

    $scope.DeleteItem = function (row, column, value) {
        return "<center><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></center>";
    }

    $scope.Delete = function (row, event) {
        var dataRecord = $scope.GrdItem.getrowdata(row);
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Item?', 350, 100, function (e) {
            if (e) {
                $http.post('/RMItem/DeleteItem', { Id: dataRecord.Id }).then(function (result) {
                    if (result.data.Message == "Success") {
                        $scope.GrdItem.updatebounddata();
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                    $scope.GrdMachine.updatebounddata();
                });
            }
            //else {
            //}
        });
    }

    var ClearData = function (event) {
        if ((typeof ($scope.model) != 'undefined') && $scope.model != null) {
            $scope.model = {};
            $scope.model.Name = null;
            $scope.model.ItemCode = null;
            $scope.model.Quantity = 0;
            $scope.model.PricePerUnit = 0;
            $scope.ddlItemCategory.clearSelection();
            $scope.ItemValidator.hide();
        }
    }

});