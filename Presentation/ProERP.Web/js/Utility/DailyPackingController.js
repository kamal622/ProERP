ProApp.controller("DailyPackingController", function ($scope, $http) {

    var allLotData = [];
    var categoryId = 0;
    var formulationID = 0;
    var allGradeData = [];
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
           { name: 'SRNo', type: 'int' },
          // { name: 'GradeName', type: 'int' },
           { name: 'FormulationId', type: 'int' },
           { name: 'ProductId', type: 'int' },
           { name: 'From', type: 'int' },
           { name: 'To', type: 'int' },
           { name: 'TotalNoBags', type: 'int' }
        ],
        url: '/Utility/GetDailyPackingGridData',
        data: { CategoryId: 0 }
    };

    $scope.DailyPackingColumns = [
         {
             text: '#', sortable: false, filterable: false, editable: true ,
             groupable: false, draggable: false, resizable: false, pinned: true,
             datafield: 'SRNo', columntype: 'number', width: 50,
             cellsrenderer: function (row, column, value) {
                 return "<div style='margin:4px;'>" + (value + 1) + "</div>";
             }
       },
      {
          text: 'Lot No', datafield: 'FormulationId', displayfield: 'LotNo', width: 400, sortable: false, columntype: 'combobox', editable: true,
          createeditor: function (row, cellvalue, editor, celltext, pressedChar) {
              var data = this.owner.getrowdata(row);
              editor.jqxComboBox({ theme: $scope.theme, autoComplete: true, dropDownHeight: 250, source: allLotData, displayMember: 'LotNo', valueMember: 'FormulationId', searchMode: 'containsignorecase' });
              editor.jqxComboBox('val', celltext);
          },
          cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
              formulationID = 0;
              if (newvalue.value == "") {
                  return oldvalue;
              }
              var data = this.owner.getrowdata(rowindex);
              if (newvalue.value != 'undefined' && newvalue.value != null) {
                  gradeName = "";
                  formulationID = newvalue.value;
                  gradeData(formulationID);
              }
          }
      },
      {
          text: 'Grade', datafield: 'ProductId', displayfield: 'Name', width: 400, sortable: false, columntype: 'combobox', editable: true,
          initeditor: function (row, cellvalue, editor, celltext, cellwidth, cellheight) {
              var DataId = $('#gridDailyPacking').jqxGrid('getcellvalue', row, "FormulationId");
              gradeData(DataId);
              editor.jqxComboBox({ autoDropDownHeight: true, source: allGradeData });
          }
      },
       {
           text: 'From', datafield: 'From', width: 120, editable: true,
           cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
               if (newvalue == "") {
                   return oldvalue;
               }
               var data = this.owner.getrowdata(rowindex);
               if (typeof (data.To) != 'undefined' && data.To != null) {
                   data.TotalNoBags = data.From - newvalue;
               }
           }
       },
       {
           text: 'To', datafield: 'To', width: 120, editable: true,
           cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
               if (newvalue == "") {
                   return oldvalue;
               }
               var data = this.owner.getrowdata(rowindex);
               if (typeof (data.To) != 'undefined' && data.To != null) {
                   data.TotalNoBags = newvalue - data.From;
               }
           }
       },
       {
           text: 'TotalNoBags', datafield: 'TotalNoBags', width: 205, editable: false,
           cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
               var data = this.owner.getrowdata(row);
                   return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + ';">' + value + '</span>';
           }
       }
    ];

    $scope.onLineChange = function (e) {
        var gridRows = $scope.gridDailyPacking.getrows();
        var minId = Math.min.apply(Math, gridRows.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;
        var rows = new Array();
        for (var i = 0; i < 5; i++) {
            minId -= 1;
            var datarow = { Id: minId, SRNo: 0, LotNo: "", Grade: 0, From: 0, To: 0, TotalNoBags: 0 };
            rows.push(datarow);
        }
        $scope.gridDailyPacking.addrow(null, rows);
        var LineId = $scope.ddlLine.val();
        lotData(LineId);

    }

    var lotData = function (LineId) {
        $.ajax({
            dataType: "json",
            type: 'GET',
            contentType: "application/json;",
            url: '/FormulationRequest/GetLotNoByLine',
            data: { LineId: LineId },
            success: function (response) {
                allLotData = response;
            },
            error: function (response) {
            }
        });
    }

    var gradeData = function (formulationID) {
        allGradeData = [];
        $.ajax({
            dataType: "json",
            type: 'GET',
            contentType: "application/json;",
            url: '/Product/GetGradeNameByID',
            data: { Id: formulationID },
            success: function (response) {
                allGradeData = response;
            },
            error: function (response) {
            }
        });
    }

    //$scope.gradeCellrender = function (row, columnfield, value, defaulthtml, columnproperties) {
    //    var data = $scope.gridDailyPacking.getrowdata(row);
    //}
   

    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
        dateTimeInput.setRange(firstDay, lastDay);
    }

});