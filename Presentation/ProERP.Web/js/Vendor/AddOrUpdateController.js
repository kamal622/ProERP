ProApp.controller("AddOrUpdateVendorController", function ($parse, $scope, $http) {

    if (parseInt($('#hdnVendorId').val()) > 0) {
        $.ajax({
            url: "/Vendor/GetVendorById",
            type: "GET",
            contentType: "application/json;",
            cache: false,
            dataType: "json",
            data: { Id: parseInt($('#hdnVendorId').val()) },
            success: function (response) {
                $scope.$apply(function () {
                    $scope.model = response;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    var ClearVendorData = function (e) {
        $scope.model.Name = null;
        $scope.model.Address = null;
        $scope.model.PhoneNo = null;
        $scope.model.Email = null;
    }

    $scope.OnSavevendor = function (e) {
        var vendor = $scope.model;
        if ($scope.model.Name == null ||$scope.model.Name == undefined){
            $scope.openMessageBox('Warning', 'Please insert name.', 200, 90);
        }
        else
        $.ajax({
            url: "/Vendor/SaveVendor",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { vendor: vendor },
            success: function (response) {
                if (e == 2) {
                    if (response.Type == 'Success') {
                    // Success
                    $scope.Vendorwindow.close();
                    $scope.ddlVendor.addItem(response.Data);
                    $scope.ddlVendor.val(response.Data.Id);
                    ClearVendorData();
                }
                    
                }
                else
                    window.location.href = "/Vendor/List/";

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    };
});