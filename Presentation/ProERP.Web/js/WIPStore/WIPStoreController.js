ProApp.controller("WIPStoreController", function ($scope, $http, $timeout) {

    $scope.WIpStoreRules = [
          { input: '#txtName', message: 'Item name is required', action: 'blur', rule: 'required' },
          { input: '#txtLocation', message: 'Item name is required', action: 'blur', rule: 'required' }
    ];



    $scope.WIPStoreSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'Location', type: 'string' }
        ],
        url: '/WIPStore/GetWIPStoreList',
        data: { Name: '' },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    $scope.onAddClick = function () {
        ClearData();
        $scope.WIPStoreValidator.hide();
        $scope.WinAddOrUpdateWIPStore.open();
    }

    $scope.onSearch = function () {
        var Name = $scope.SearchName;
        $scope.WIPStoreSource.data = { Name: Name };
    }

    $scope.onRefresh = function () {
        $scope.GrdWIPStore.updatebounddata();
    }

    $scope.onSaveWIP = function () {
        var isValidate = $scope.WIPStoreValidator.validate();
        if (!isValidate)
            return;
        $http.post('/WIPStore/SaveWIPStore', { wipData: $scope.model }).then(function (result) {
            if (result.data.Status == 1) {
                $scope.GrdWIPStore.updatebounddata();
                $scope.openMessageBox('Success', result.data.Message, 300, 90);
                ClearData();
                $scope.WinAddOrUpdateWIPStore.close();
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    }

    $scope.UpdateWIP = function (row, column, value) {
        return "<center><a ng-click='EditWIPStore(" + row + ", event)' class='fa fa-edit fa-2' href='javascript:;'></a></center>";
    }

    $scope.EditWIPStore = function (row, event) {
        var dataRecord = $scope.GrdWIPStore.getrowdata(row);
        $.ajax({
            url: "/WIPStore/GetWIPStoreById",
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
        $scope.WIPStoreValidator.hide();
        $scope.WinAddOrUpdateWIPStore.open();
    }

    $scope.DeleteWIP = function (row, column, value) {
        return "<center><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></center>";
    }

    $scope.Delete = function (row, event) {
        var dataRecord = $scope.GrdWIPStore.getrowdata(row);
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete WIP?', 350, 100, function (e) {
            if (e) {
                $http.post('/WIPStore/DeleteWipById', { Id: dataRecord.Id }).then(function (result) {
                    if (result.data.Status == 1) {
                        $scope.GrdWIPStore.updatebounddata();
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                    $scope.GrdWIPStore.updatebounddata();
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
            $scope.model.Location = null;
            $scope.model.Description = "";
            $scope.WIPStoreValidator.hide();
        }
    }

});