ProApp.controller("POController", function ($parse, $scope, $http) {

    $scope.model = { }
   
    //alert($scope.model.Id);
    
    $scope.model.Id = $('#txtId').val();

    $.ajax({
        url: "/PurchaseOrder/GetPOById",
        type: "GET",
        contentType: "application/json;",
        dataType: "json",
        data: { Id: $scope.model.Id },
        success: function (data) {

            $scope.$apply(function () {

                $scope.model = data;

            });

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert('oops, something bad happened');
        }
    });
      
});