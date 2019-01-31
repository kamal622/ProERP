ProApp.controller("ListController", function ($parse, $scope, $http) {

    $scope.SearchBudgetType = '';
    $scope.IndentBudgetSource = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BudgetType', type: 'string' },
           { name: 'BudgetCode', type: 'string' },
           { name: 'FinancialYear', type: 'string' },
           { name: 'Amount', type: 'decimal' },
           { name: 'IsActive', type: 'bool' },
        ],
        url: '/Indent/GetIndentBudgetData',
        data: { BudgetType: $scope.SearchBudgetType },
    };
    $scope.onRefreshClick = function (e) {
        $scope.BudgetGrid.updatebounddata();
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete item(s)?', 350, 100, function (e) {
            if (e) {
                var rows = $scope.BudgetGrid.selectedrowindexes;
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.BudgetGrid.getrowdata(rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }
                $.ajax({
                    dataType: "json",
                    type: 'POST',
                    url: '/Indent/DeleteIndentBudget',
                    data: {
                        Ids: selectedIds
                    },
                    success: function (response) {
                        $scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        $scope.BudgetGrid.updatebounddata();
                    },
                    error: function (jqXHR, exception) {
                    }
                });
            }
        });
    };
    $scope.onSearch = function (event) {
        $scope.IndentBudgetSource.data = { BudgetType: $scope.SearchBudgetType };
    };
    $scope.UpdateBudget = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.BudgetGrid.getrowdata(row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Indent/UpdateIndentBudget/" + dataRecord.Id + "'> Edit/View</a>";
    }

    $scope.IsActiveCellsRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.BudgetGrid.getrowdata(row);
        return dataRecord.IsActive ? "<div class=\"jqx-grid-cell-center-align\" style=\"margin-top: 6px;margin-left: 10px;\">Yes</div>" : "<div class=\"jqx-grid-cell-center-align\" style=\"margin-top: 6px;margin-left: 10px;\">No</div>";
    }

});