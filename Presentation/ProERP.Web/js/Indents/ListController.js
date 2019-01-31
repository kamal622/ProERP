ProApp.controller("IndentsController", function ($parse, $scope, $http) {

    $scope.IndentsGrid = {};
    
    $scope.Indents = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'IndentNo', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'QtyInStock', type: 'int' },
           { name: 'QtyNeeded', type: 'int' },
           { name: 'UnitPrice', type: 'int' },
           { name: 'TotalAmount', type: 'int' },
           { name: 'PrioritytName', type: 'int' }
          
        ],
        url: '/Indents/GetIndentsList',
        data: { },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'

    };
    $scope.UpdateIndent = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.IndentsGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Indents/Update/" + dataRecord.Id + "'> Edit/View </a>";
    }
    $scope.onRefreshClick = function (e) {
        $scope.IndentsGrid.jqxGrid('updatebounddata');
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Indent(s)?', 350, 100, function (e) {
            if (e) {
                debugger;
                var rows = $scope.IndentsGrid.jqxGrid('selectedrowindexes');
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.IndentsGrid.jqxGrid('getrowdata', rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }

                $http.post('/Indents/DeleteIndent', { Ids: selectedIds }).success(function (retData) {
                    if (retData.Message == "Success") {

                        //  $('#lstBatch').jqxListBox('refresh');
                        $scope.IndentsGrid.jqxGrid('updatebounddata');

                    } else {
                        debugger;
                        $scope.openMessageBox("Error", retData.Message, 350, 100);

                        $scope.IndentsGrid.jqxGrid('updatebounddata');

                    }

                }).error(function (retData, status, headers, config) {

                });

            }
            else {

            }
        });
    };

    });