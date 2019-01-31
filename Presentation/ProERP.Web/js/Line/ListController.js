
ProApp.controller("LineListController", function ($scope, $http) {
  
     $scope.LineGrid = {};
     $scope.SearchLineName = '';
    $scope.lines = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'InCharge', type: 'string' },
           { name: 'Location', type: 'string' },
           { name: 'PlantId', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'IsActive', type: 'bool' },
           
        ],
        url: '/Line/GetLineList',
        data: { Name: $scope.SearchLineName, SiteId: 0, PlantId: 0 },
        //data: { Name: "kal"},
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };

    $scope.UpdateLine = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.LineGrid.jqxGrid('getrowdata', row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Line/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }
    $scope.onSearch = function (e) {
        //debugger;
        var SiteId = ($("#ddlSite").val() == "" ? 0 : $("#ddlSite").val());
        var PlantId = ($("#ddlPlant").val() == "" ? 0 : $("#ddlPlant").val());
        $scope.lines.data = { Name: $scope.SearchLineName, SiteId: SiteId, PlantId: PlantId };
    };
    $scope.onRefreshClick = function (e) {
        $scope.LineGrid.jqxGrid('updatebounddata');
    }
    $scope.onDelete = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Line(s)?', 350, 100, function (e) {
            if (e) {
                //debugger;
                var rows = $scope.LineGrid.jqxGrid('selectedrowindexes');
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.LineGrid.jqxGrid('getrowdata', rows[m]);
                    if (row != null)
                        selectedIds.push(row.Id);
                }

                $http.post('/Line/DeleteLine', { Ids: selectedIds }).success(function (retData) {
                    if (retData.Message == "Success") {

                        //  $('#lstBatch').jqxListBox('refresh');
                        $scope.LineGrid.jqxGrid('updatebounddata');

                    } else {
                        debugger;
                        $scope.openMessageBox("Error", retData.Message, 350, 100);

                        $scope.LineGrid.jqxGrid('updatebounddata');

                    }

                }).error(function (retData, status, headers, config) {

                });

            }
            else {

            }
        });
    };

    $scope.selectItem = function (event) {
        //alert("hi");
        //debugger;
        if (event.args) {
            var item = event.args.item;
            if (item) {
                $scope.log = "Value: " + item.value + ", Label: " + item.label;
            }
        }
    };
    $('#ddlSite').on('select', function (event) {
       // debugger;
       // alert("hi");
        var args = event.args;
        if (args) {
            // index represents the item's index.                
            var index = args.index;
            var item = args.item;
            // get item's label and value.
            var label = item.label;
            var value = item.value;
        }
    });
   
   
    $scope.createWidget = true;
});