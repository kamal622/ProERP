ProApp.controller("HistroyController", function ($scope, $http, $timeout) {

    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
        dateTimeInput.setRange(firstDay, lastDay);
    }

    var from = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        from = $scope.dtpSearchDate.getRange().from;

    var to = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        to = $scope.dtpSearchDate.getRange().to;

    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetFormulationLine',
    };

    var FormulationHistorySource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'StatusName', type: 'string' },
           { name: 'QAStatusName', type: 'string' },
           { name: 'CreateDate', type: 'date' },
           { name: 'CreateBy', type: 'string' },
        ],
        url: '/FormulationRequest/GetFormulationHistoryData',
        data: {
            LineId: 0, fromDate: new Date(from.getFullYear(), from.getMonth(), 1).toISOString(),
            toDate: new Date(from.getFullYear(), from.getMonth() + 1, 0).toISOString()
        }
    };

    var initRequestDetails = function (index, parentElement, gridElement, record) {
        var id = record.uid.toString();
        var grid = $($(parentElement).children()[0]);
        var rowData = $scope.GridSettings.apply('getrowdata', index);
        var nestedSource = {
            datafields: [
                       { name: 'Id', type: 'int' },
                       { name: 'ItemName', type: 'string' },
                       { name: 'ItemQtyPercentage', type: 'decimal' },
                       { name: 'ItemQtyGram', type: 'decimal' },
                       { name: 'VerNo', type: 'int' },
            ],
            datatype: 'json',
            url: '/FormulationRequest/GetFormulationDetailsHistory',
            data: { FormulationId: record.Id }
        };
        if (grid != null) {
            var nestedAdapter = new $.jqx.dataAdapter(nestedSource);
            grid.jqxGrid({
                source: nestedAdapter,
                height: '70%',
                columns: [
                            { text: 'Item', datafield: 'ItemName' },
                            { text: 'Item Qty %', datafield: 'ItemQtyPercentage', width: '150' },
                            { text: 'Item Qty Gram', datafield: 'ItemQtyGram', width: '150' },
                            { text: 'VersionNo', datafield: 'VerNo', width: '150' },
                ]
            });
        }
    }

    var initrawMaterialDetails = function (index, parentElement, gridElement, record) {
        var id = record.uid.toString();
        var grid = $($(parentElement).children()[0]);
        var rowData = $scope.GridSettings.apply('getrowdata', index);
        var nestedSource = {
            datafields: [
                       { name: 'Id', type: 'int' },
                       { name: 'ItemName', type: 'string' },
                       { name: 'RequestedQty', type: 'decimal' },
                       { name: 'IssuedQty', type: 'decimal' },
                       { name: 'ReturnQty', type: 'decimal' },
                       { name: 'WIPQty', type: 'decimal' }
            ],
            datatype: 'json',
            url: '/FormulationRequest/GetRMDetailsHistoryData',
            data: { RMRequestId: record.Id }
        };
        if (grid != null) {
            var nestedAdapter = new $.jqx.dataAdapter(nestedSource);
            grid.jqxGrid({
                source: nestedAdapter,
                height: '70%',
                columns: [
                            { text: 'Item', datafield: 'ItemName' },
                            { text: 'Requested Qty', datafield: 'RequestedQty', width: '100' },
                            { text: 'Issued Qty', datafield: 'IssuedQty', width: '100' },
                            { text: 'Return Qty', datafield: 'ReturnQty', width: '100' },
                            { text: 'WIP Qty', datafield: 'WIPQty', width: '100' }
                ]
            });
        }
    }

    var intiFormulationDetails = function (formulationId) {
        var nestedSource = {
            datafields: [
                       { name: 'Id', type: 'int' },
                       { name: 'LotNo', type: 'string' },
                       { name: 'GradeName', type: 'string' },
                       { name: 'QtyToProduce', type: 'int' },
                       { name: 'LOTSize', type: 'string' },
                       { name: 'QAStatusName', type: 'string' },
            ],
            datatype: 'json',
            url: '/FormulationRequest/GetFormulationNestedGridData',
            data: { Id: formulationId }
        };
        if (formulationId>0) {
            var nestedAdapter = new $.jqx.dataAdapter(nestedSource);
            $("#formulationDetails").jqxGrid({
                rowdetails: true,
                rowdetailstemplate: {
                    rowdetails: "<div id='requestDetailsgrid' style='margin: 10px;'></div>",
                    //rowdetailsheight: 230,
                    rowdetailshidden: true
                },
                initrowdetails: initRequestDetails,
                source: nestedAdapter,
                theme: $scope.theme,
                width: '97%',
                height: 230,
                columns: [
                            { text: 'LotNo', datafield: 'LotNo', width: '130' },
                            { text: 'GradeName', datafield: 'GradeName' },
                            { text: 'QtyToProduce', datafield: 'QtyToProduce', width: '130' },
                            { text: 'Lot Size', datafield: 'LOTSize' },
                            { text: 'QA Status', datafield: 'QAStatusName' },
                ]
            });
        }
    }

    var initRawMaterialsDetails = function (formulationId) {
        var nestedSource = {
            datafields: [
                       { name: 'Id', type: 'int' },
                       { name: 'LotNo', type: 'string' },
                       { name: 'RequestBy', type: 'string' },
                       { name: 'RequestDate', type: 'date' },
                       { name: 'DispatchBy', type: 'string' },
                       { name: 'DispatchDate', type: 'date' },
                       { name: 'ReceviedBy', type: 'string' },
                       { name: 'RMRequestStatus', type: 'string' },
                       { name: 'ReceviedDate', type: 'date' },
                       { name: 'FormulationRequestId', type: 'int' },
            ],
            datatype: 'json',
            url: '/FormulationRequest/GetRawmaterialNestedGridData',
            data: { FormulationId: formulationId }
        };
        if (formulationId > 0) {
            var nestedAdapter = new $.jqx.dataAdapter(nestedSource);
            $("#materialDetails").jqxGrid({
                rowdetails: true,
                rowdetailstemplate: {
                    rowdetails: "<div id='rmDetailsgrid' style='margin: 10px;'></div>",
                    rowdetailsheight: 230,
                    rowdetailshidden: true
                },
                initrowdetails: initrawMaterialDetails,
                source: nestedAdapter,
                theme: $scope.theme,
                width: '97%',
                height: 230,
                columns: [
                   { text: 'LotNo', datafield: 'LotNo' },
                   { text: 'LotNo', datafield: 'RMRequestStatus' },
                   { text: 'Request By', datafield: 'RequestBy', width: '130' },
                   { text: 'Request On', datafield: 'RequestDate', cellsformat: 'dd/MM/yyyy', width: '130' },
                   { text: 'Dispatch By', datafield: 'DispatchBy', width: '130' },
                   { text: 'Dispatch On', datafield: 'DispatchDate', cellsformat: 'dd/MM/yyyy', width: '130' },
                   { text: 'Recevied By', datafield: 'ReceviedBy', width: '130' },
                   { text: 'Recevied On', datafield: 'ReceviedDate', cellsformat: 'dd/MM/yyyy', width: '130' }
                ]
            });
        }
    }

    var initlevel1 = function (index, parentElement, gridElement, record) {
        var tabsdiv = null;
        var formulationId = 0;
        tabsdiv = $($(parentElement).children()[0]);
        var formulationId = record.Id;
        if (tabsdiv != null) {
            var container = $('<div style="margin: 5px;"></div>');
            var initWidgets = function (tab) {
                switch (tab) {
                    case 0:
                        intiFormulationDetails(formulationId);
                        break;
                    case 1:
                        initRawMaterialsDetails(formulationId);
                        break;
                }
            }
            $(tabsdiv).jqxTabs({ width: '95%',height:220,  initTabContent: initWidgets });
        }

    }


    $scope.GridSettings = {
        width: '100%',
        source: FormulationHistorySource,
        theme: $scope.theme,
        columnsresize: true,
        rowdetails: true,
        rowsheight: 35,
        initrowdetails: initlevel1,
        rowdetailstemplate:
            {
                rowdetails: "<div style='margin: 10px;'><ul style='margin-left: 30px;'><li>Formulation Details</li><li>Raw Materials</li></ul><div class='formulationDetails' id='formulationDetails'></div><div id='materialDetails' class='materialDetails'></div></div>",
                rowdetailsheight: 250
            }
    };

    $scope.onSearch = function (event) {
        var lineId;
        if ($scope.ddlLine.val() == "" || $scope.ddlLine.val() == "undefined")
            lineId = 0;
        else
            lineId = parseInt($scope.ddlLine.val());
        var selection = $scope.dtpSearchDate.getRange();
        FormulationHistorySource.data = { LineId: lineId, fromDate: selection.from.toISOString(), toDate: selection.to.toISOString() }
    }

    $scope.onRefreshClick = function (event) {
        $scope.GrdFormulationHistory.updatebounddata();
    }

});