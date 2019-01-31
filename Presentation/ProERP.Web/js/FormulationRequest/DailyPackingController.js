ProApp.controller("DailyPackingController", function ($scope, $http) {

    var allLotData = [];
    var categoryId = 0;
    var formulationID = 0;
    var allGradeData = [];
    var DeletedDailyPackingId = [];

    var ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    };

        $.ajax({
            dataType: "json",
            type: 'GET',
            contentType: "application/json;",
            url: '/FormulationRequest/GetAllLotNo',
            success: function (response) {
                allLotData = response;
            },
            error: function (response) {
            }
        });


    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetFormulationLine',
    };

    $scope.ProductCategorySource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Product/GetProductCategoryList',
    };

    $scope.ProductSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Product/GetProductListById',
        data: { categoryId: 0 }
    };

    $scope.DailyPackingSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BatchId', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeId', type: 'int' },
           { name: 'GradeName', type: 'string' },
           { name: 'BagFrom', type: 'int' },
           { name: 'BagTo', type: 'int' },
           { name: 'TotalBags', type: 'int' },
           { name: 'Quantity', type: 'int' },
           { name: 'ProductionRemarks', type: 'string' }
        ],
        url: '/FormulationRequest/GetDailyPackingGridData',
        data: { currentDate: (new Date()).toISOString() }
    };

    $scope.DailyPackingColumns = [
         {
             text: '#', sortable: false, filterable: false, editable: true,
             groupable: false, draggable: false, resizable: false, pinned: true,
             datafield: 'SRNo', columntype: 'number', width: 50,
             cellsrenderer: function (row, column, value) {
                 //lotData();
                 return "<div style='margin:4px;'>" + (value + 1) + "</div>";
             }
         },
      {
          text: 'Lot No', datafield: 'BatchId', displayfield: 'LotNo', width: 130, sortable: false, columntype: 'combobox', editable: true,
          createeditor: function (row, cellvalue, editor, celltext, pressedChar) {
              var data = this.owner.getrowdata(row);
              editor.jqxComboBox({ theme: $scope.theme, autoComplete: true, dropDownHeight: 250, source: allLotData, displayMember: 'LotNo', valueMember: 'BatchId', searchMode: 'containsignorecase' });
              editor.jqxComboBox('val', celltext);
             
          },
          cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
              formulationID = 0;
              if (newvalue.value == "") {
                  return oldvalue;
              }
              var data = this.owner.getrowdata(rowindex);
              if (newvalue.value != 'undefined' && newvalue.value != null) {
                  data.GradeId = 0;
                  data.GradeName = "";
                  for (var i = 0; i < allLotData.length; i++) {
                      if (allLotData[i].BatchId == newvalue.value) {
                          data.GradeId = allLotData[i].ProductId;
                          data.GradeName = allLotData[i].GradeName;
                      }
                  }
                  allGradeData = [];
                  gradeName = "";
              }
          }
      },
      {
          text: 'Grade', datafield: 'GradeName', width: 200, sortable: false, editable: false
      },
       {
           text: 'From', datafield: 'BagFrom', width: 120, editable: true, columngroup: 'BagNo', aggregates: ['sum'],
           cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
               if (newvalue == "") {
                   return oldvalue;
               }
               var data = this.owner.getrowdata(rowindex);
               if (typeof (data.BagTo) != 'undefined' && data.BagTo != null) {
                   data.TotalBags = data.BagFrom - newvalue;
               }
           }
       },
       {
           text: 'To', datafield: 'BagTo', width: 120, editable: true, columngroup: 'BagNo', aggregates: ['sum'],
           cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
               if (newvalue == "") {
                   return oldvalue;
               }
               var data = this.owner.getrowdata(rowindex);
               if (typeof (data.BagTo) != 'undefined' && data.BagTo != null) {
                   data.TotalBags = newvalue - data.BagFrom;
               }
           }
       },
       {
           text: 'Total<br> No Bags', datafield: 'TotalBags', width: 100, editable: false, aggregates: ['sum'],
           cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
               var data = this.owner.getrowdata(row);
               return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + ';">' + value + '</span>';
           }
       },
       {
            text: 'Quantity', datafield: 'Quantity', width: 100, editable: true,aggregates: ['sum']
       },
       {
           text: 'Production Remarks', datafield: 'ProductionRemarks', editable: true
       },
    ];

    $scope.DailyPackingGroupColumns = [{ text: 'Bag No', align: 'center', name: 'BagNo' }];

   
    

    $scope.onRefreshClick = function () {
        $scope.gridDailyPacking.updatebounddata();
    }

    $scope.onSaveClick = function (event) {
        var rows = $scope.gridDailyPacking.getrows();

        var packingData = $.grep(rows, function (a) {
            return a.BatchId > 0 && a.GradeId > 0 && a.BagFrom >= 0
            && a.BagTo >= 0 && a.TotalBags >= 0 && a.Quantity >= 0;
        })

        var postData = [];
        $http.post('/FormulationRequest/SaveDailyPackingData', { dailyPackingData: packingData, dailypackingDate: $scope.dtPackingDate.getDate().toISOString() }).then(function (result) {
            if (result.data.Status == 1) {
                $scope.openMessageBox('Success', result.data.Message, 250, 90);
                $scope.gridDailyPacking.updatebounddata();
                DeletedDailyPackingId = [];
            }
            else {
                $scope.gridDailyPacking.updatebounddata();
                $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    }

    //$scope.OnCellBeginEdit = function (event) {
    //    // event arguments.
    //    var args = event.args;
    //    // row's data.
    //    oldData = jQuery.extend(true, {}, args.row);
    //}

    $scope.onSearchClick = function () {
        var newDate = $scope.dtPackingDate.getDate().toISOString();
        $scope.DailyPackingSource.data = { currentDate: newDate };
    }

    $scope.onDeleteClick = function () {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete selected item(s)?', 350, 100, function (isYes) {
            if (isYes) {
                var rows = $scope.gridDailyPacking.selectedrowindexes;
                var DeletedDailyPackingId = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.gridDailyPacking.getrowdata(rows[m]);
                    if (row != null && row.Id > 0)
                        DeletedDailyPackingId.push(row.Id);
                }
                if (DeletedDailyPackingId.length == 0) {
                    $scope.gridDailyPacking.clearselection();
                    return;
                }

                $http.post('/FormulationRequest/DeleteDailyPackingData', { DeletedId: DeletedDailyPackingId }).then(function (result) {
                    if (result.data.Status == 1) {
                        $scope.openMessageBox('Success', result.data.Message, 200, 80);
                        $scope.gridDailyPacking.updatebounddata();
                        $scope.gridDailyPacking.clearselection();
                        DeletedDailyPackingId = [];
                    }
                    else {
                        $scope.gridDailyPacking.updatebounddata();
                        $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                });

            }
            });
    }

    $scope.OnExport = function (exportType) {
        if (exportType == 'excel') {
            var newDate = $scope.dtPackingDate.getDate().toISOString();
            window.location = '/Download/GetDailyPackingDetailsExcel?currentDate=' + newDate;
        }
    }

    $scope.onAddNewClick = function (event) {
        var rows = new Array();
        for (var i = 0; i < 5; i++) {
           var Datarow = {
               Id: 0, BatchId: 0, LotNo: "", GradeId: 0,
               GradeName: "", BagFrom: 0, BagTo: 0,
               TotalBags : 0 ,Quantity: 0, ProductionRemarks: ""
            };
           rows.push(Datarow);
        }
        $scope.gridDailyPacking.addrow(null, rows);
    }

    
});