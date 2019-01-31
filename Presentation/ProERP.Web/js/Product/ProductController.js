ProApp.controller("ProductController", function ($scope, $http, $timeout) {

    $scope.ProductRules = [
        { input: '#txtproductName', message: 'Product name is required', action: 'blur', rule: 'required' },
        {
            input: '#ddlProductCategory', message: 'Please choose Category', action: 'change',
            rule: function (input, commit) {
                var index = $scope.ddlProductCategory.getSelectedIndex();
                return index != -1;
            }
        }
    ];

    $scope.ProductSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'CategoryId', type: 'int' },
           { name: 'CategoryName', type: 'string' },
           { name: 'Name', type: 'string' },
           { name: 'Code', type: 'string' },
           { name: 'ShortName', type: 'string' }
        ],
        url: '/Product/GetProductList',
        data: { productName: '' },
        Id: "Id",
        sortcolumn: 'ProductName',
        sortdirection: 'asc'
    };

    $scope.ProductCategorySource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Product/GetProductCategoryList',
    };

    $scope.onSearch = function () {
        var productName = $scope.SearchProductName;
        $scope.ProductSource.data = { productName: productName };
    };

    $scope.onRefresh = function () {
        $scope.GrdProduct.updatebounddata();
    }

    $scope.onAddClick = function () {
        $scope.ProductValidator.hide();
        ClearData();
        $scope.WinAddOrUpdateProduct.open();
    };

    $scope.onSaveProduct = function () {
        var isValidate = $scope.ProductValidator.validate();
        if (!isValidate)
            return;
        var productData = $scope.model;
        productData.CategoryId = $scope.ddlProductCategory.getSelectedItem().value;
        $http.post('/Product/SaveProduct', { productData: productData }).then(function (result) {
            if (result.data.Status == 1) {
                $scope.GrdProduct.updatebounddata();
                //$scope.openMessageBox('Success', result.data.Message, 300, 90);
                $scope.WinAddOrUpdateProduct.close();
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    };

    $scope.UpdateProduct = function (row, column, value) {
        return "<center><a ng-click='EditProduct(" + row + ", event)' class='fa fa-edit fa-2' href='javascript:;'></a></center>";
    }
    $scope.EditProduct = function (row, event) {
        var dataRecord = $scope.GrdProduct.getrowdata(row);
        $.ajax({
            url: "/Product/GetProductById",
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
        $scope.ProductValidator.hide();
        $scope.WinAddOrUpdateProduct.open();
    }

    $scope.DeleteProduct = function (row, column, value) {
        return "<center><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></center>";
    }

    $scope.Delete = function (row, event) {
        var dataRecord = $scope.GrdProduct.getrowdata(row);
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Product?', 350, 100, function (e) {
            if (e) {
                $http.post('/Product/DeleteProduct', { Id: dataRecord.Id }).then(function (result) {
                    if (result.data.Message == "Success") {
                        $scope.GrdProduct.updatebounddata();
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                    $scope.GrdProduct.updatebounddata();
                });
            }
        });
    };

    var ClearData = function (event) {
        if ((typeof ($scope.model) != 'undefined') && $scope.model != null) {
            $scope.model = {};
            $scope.model.Name = null;
            $scope.model.Code = null;
            $scope.model.ShortName = null;
            $scope.ddlProductCategory.clearSelection();
            $scope.ProductValidator.hide();
        }
    }
});