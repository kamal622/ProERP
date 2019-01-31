ProApp.controller("AddOrUpdateController", function ($scope) {
   
   
    if (parseInt($('#hdnItemId').val())>0) {
    $.ajax({
        url: "/Item/GetItemById",
        type: "GET",
        contentType: "application/json;",
        dataType: "json",
        cache: false,
        data: { Id: parseInt($('#hdnItemId').val()) },
        success: function (data) {
            $scope.$apply(function () {
                $scope.model = data;
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('oops, something bad happened');
        }
    });
    }
    $scope.VendorSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Item/GetVendorList',
    };
    var ClearItemData = function (e) {
        $scope.model.Name = null;
        $scope.model.ItemCode = null;
        $scope.model.Make = null;
        $scope.model.Model = null;
        $scope.model.MOC = null;
        $scope.model.Price = 0;
        $scope.model.UnitOfMeasure = null;
        $scope.model.AvailableQty = 0;
        $scope.model.Description = "";
    }


    $scope.SaveNewItem = function (e) {
        //Ajax call to save
        var item = $scope.model;
        if ($scope.model.Name == null || $scope.model.Name =="")
            $scope.openMessageBox('Warning', 'Please insert item name.', 200, 90);
        
        else if ($scope.model.UnitOfMeasure == null || $scope.model.UnitOfMeasure == "") {
            $scope.openMessageBox('Warning', 'Please insert unit of measure.', 200, 90);
        }
        else if ($scope.model.AvailableQty == null || $scope.model.AvailableQty == "") {
            $scope.openMessageBox('Warning', 'Available quantity must be greater than zero(0).', 350, 90);
        }
        else
            $.ajax({
                url: "/Item/SaveNewItem",
                type: "POST",
                //contentType: "application/json;",
                dataType: "json",
                data: { item: item },
                success: function (response) {
                    if (e == 2) {
                        if (response.Type == 'Success') {
                            $scope.$parent.Itemwindow.close();
                            $scope.$parent.ddlItemName.addItem(response.Data);
                            $scope.$parent.ddlItemName.val(response.Data.Id);
                            ClearItemData();
                        }
                    }
                    else
                        window.location.href = "/Item/List/";
                        

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
    };
});