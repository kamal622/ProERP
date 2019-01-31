ProApp.controller("AddOrUpdateController", function ($parse, $scope, $http) {


    $scope.onBindingBudgetType = function (event) {
        $scope.ddlBudgetType.val($scope.ddlBudgetType.val());
    }

    $scope.OnSaveBudget = function (event) {
        //Ajax call to save
      
        var IndentBudget = $scope.model;

            $.ajax({
                url: "/Indent/SaveIndentBudget",
                type: "POST",
                //contentType: "application/json;",
                dataType: "json",
                data: { IndentBudget: IndentBudget },
                success: function (response) {
                    if (response.Status == 1) {
                        // Success
                        $scope.$apply(function () {
                            window.location.href = "/Indent/ListIndentBudget/";
                        });
                    }
                    else if (response.Status = 2) {
                        // Warning
                        $scope.openMessageBox('Warning', response.Message, 350, 100);
                    }
                   
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
    };

    
    if (parseInt($('#txtId').val()) > 0) {
        $.ajax({
            url: "/Indent/GetIndentBudgetById",
            type: "GET",
            contentType: "application/json;",
            cache: false,
            dataType: "json",
            data: { Id: parseInt($('#txtId').val()) },
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
});