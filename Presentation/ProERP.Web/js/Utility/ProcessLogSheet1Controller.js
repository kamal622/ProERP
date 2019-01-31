ProApp.controller("ProcessLogSheet1Controller", function ($scope, $http) {

    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetFormulationLine',
    };

    $scope.ProductSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Product/GetProductByLineId',
        data: { LineId: 0 }
    };

    $scope.ProcessLogSheet1Source = {
        datatype: "json",
        datafields: [
          // { name: 'Id', type: 'int' },
           { name: 'C1', type: 'string' },
           { name: 'C2', type: 'int' },
           { name: 'C3', type: 'int' },
           { name: 'C4', type: 'int' },
           { name: 'C5', type: 'int' },
           { name: 'C6', type: 'int' },
           { name: 'C7', type: 'int' },
           { name: 'C8', type: 'int' },
           { name: 'C9', type: 'int' },
           { name: 'C10', type: 'int' },
           { name: 'C11', type: 'int' },
           { name: 'C12', type: 'int' },
           { name: 'C13', type: 'int' },
           { name: 'C14', type: 'int' },
           { name: 'C15', type: 'int' },
           { name: 'C16', type: 'int' },
           { name: 'C17', type: 'int' }
        ],
        url: '/Utility/GetProcessLogSheet1GridData',
        data: { LineId: 0, selectedDate: '' }
    };

    $scope.ProcessLogSheet1Columns = [
         {
             text: 'Time', sortable: false, filterable: false, editable: false,
             groupable: false, draggable: false, resizable: false, pinned: true,
             datafield: 'C1', width: 70
         },
      { text: 'TZ-1', datafield: 'C2', editable: true, width: 60 },
      { text: 'TZ-2', datafield: 'C3', editable: true, width: 60 },
      { text: 'TZ-3', datafield: 'C4', editable: true, width: 60 },
      { text: 'TZ-4', datafield: 'C5', editable: true, width: 60 },
      { text: 'TZ-5', datafield: 'C6', editable: true, width: 60 },
      { text: 'TZ-6', datafield: 'C7', editable: true, width: 60 },
      { text: 'TZ-7', datafield: 'C8', editable: true, width: 60 },
      { text: 'TZ-8', datafield: 'C9', editable: true, width: 60 },
      { text: 'TZ-9', datafield: 'C10', editable: true, width: 60 },
      { text: 'TZ-10', datafield: 'C11', editable: true, width: 60 },
      { text: 'TZ-11', datafield: 'C12', editable: true, width: 60 },
      { text: 'TZ-12-DIE', datafield: 'C13', editable: true, width: 100 },
      { text: 'TM1', datafield: 'C14', editable: true, width: 100 },
      { text: 'PM1', datafield: 'C15', editable: true, width: 100 },
      { text: 'PM1.1', datafield: 'C16', editable: true, width: 100 },
      { text: 'Vaccum Mbar', datafield: 'C17', editable: true },
    ];

    $scope.onLineChange = function (e) {
        $scope.ProductSource.data = { LineId : $scope.ddlLine.val() };
    }

    $scope.onProductChange = function (e) {
        var currentDate = $scope.dtpSearchDate.getDate().toISOString()
        $scope.ProcessLogSheet1Source.data = { LineId: $scope.ddlLine.val(), selectedDate: currentDate };
        $scope.gridProcessLogSheet1.updatebounddata();
        var gridRows = $scope.gridProcessLogSheet1.getrows();
        var minId = Math.min.apply(Math, gridRows.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;

        var timeStart = 9;
        var rows = new Array();

        for (var i = 0; i <= 7; i++) {
            var datarow = { C1: timeStart + ":00", PlantId: 1, LineId: $scope.ddlLine.val() };
            rows.push(datarow);
            timeStart = timeStart + 3;
            if (timeStart >= 24) {
                timeStart = 0;
            }
                
        }
        $scope.gridProcessLogSheet1.addrow(null, rows);
    }

    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
        dateTimeInput.setRange(firstDay, lastDay);
    }

    $scope.onSaveClick = function (event) {
        var rows = $scope.gridProcessLogSheet1.getrows();
        var TemplateId = 2;
        $.ajax({
            dataType: "json",
            type: 'POST',
            url: '/Utility/SaveData',
            data: { readingData: rows, templateId: TemplateId },
            success: function (response) {
                if (response.Status == 1) {
                    //BindGrid();
                    $scope.openMessageBox('Success', 'Data saved successfully.', 250, 85);
                }
                else {
                    $scope.openMessageBox('Error', response.Message, 'auto', 'auto');
                }
            },
            error: function (jqXHR, exception) {
            }
        });
    }

});