ProApp.controller("DashboardController", function ($scope, $http, $timeout) {

    $scope.ShowFilterNew = false;
    $scope.ShowFilterRecevied = false;
    $scope.ShowFilterProgress = false;
    $scope.ShowFilterTest = false;
    $scope.ShowFilterOther = false;
    $scope.ShowFilterRMRequest = false;
    $scope.ShowFilterRMRecevied = false;
    $scope.ShowFilterRMCloseRecevied = false;
    $scope.ShowFilterClose = false;
    var TestFail = false;
    var RequestId = 0;
    var RequestStatus = 0;
    var viewFormulationId = 0;
    var FormulationDetailsData = [];
    $scope.IsSaved = false;
    $scope.IsDownload = false;
    $scope.IsBatch = false;
    $scope.IsClosed = false;
    var LineId = 0;
    var allMachineData = [];
    var allWIPData = [];
    $scope.IsDispatch = false;
    $scope.IsMaterialReceived = false;
    $scope.IsMaterialSlip = false;
    $scope.IsRMReceived = false;
    $scope.enableProperty = false;
    var colourSpecData = [];
    var qaSpecData = [];
    var masterData = [];
    var IsColoursQA = "";
    $scope.IsTestResult = false;
    var rawMaterialId = 0;
    var lotNo = "";
    var parentId = 0;
    var IsFormulationchanged = false;
    var DeleteFormulationID = [];
    var userRole = $('#hdnRole').val();
    $scope.TestResultList = 0;

    var currentMonth = (new Date()).getMonth() + 1;
    var currentYear = (new Date()).getFullYear();

    $scope.FormulationCloseRules = [
       { input: '#txtFGDeposition', message: 'FGDeposition is required!', action: 'blur', rule: 'required' },
       { input: '#txtNSP', message: 'NSP is required!', action: 'blur', rule: 'required' },
       { input: '#txtStartUpSCOffSpecs', message: 'Start Up/SC/Off Specs is required!', action: 'blur', rule: 'required' },
       { input: '#txtQCRejected', message: 'QC Rejected is required!', action: 'blur', rule: 'required' },
       { input: '#txtMixMaterial', message: 'Mix Material is required!', action: 'blur', rule: 'required' },
       { input: '#txtLumps', message: 'Lumps is required!', action: 'blur', rule: 'required' },
       { input: '#txtLongsandFines', message: 'Longs & Fines is required!', action: 'blur', rule: 'required' },
       { input: '#txtLabSample', message: 'LabSample is required!', action: 'blur', rule: 'required' },
       { input: '#txtSweepaged', message: 'Sweepaged is required!', action: 'blur', rule: 'required' },
       { input: '#txtAdditives', message: 'Additives is required!', action: 'blur', rule: 'required' },
       { input: '#txtPackingBags', message: 'Packing bags is required!', action: 'blur', rule: 'required' },
       { input: '#txtCloseRemarks', message: 'Remarks is required!', action: 'blur', rule: 'required' },

    ];

    $scope.RemarksRules = [
      { input: '#txtformulationRemarks', message: 'Remarks is required!', action: 'blur', rule: 'required' }
    ];

    $scope.ColoursQARules = [
        { input: '#txtDeltaE', message: 'Delta E is required!', action: 'blur', rule: 'required' },
        { input: '#txtDeltaL', message: 'Delta L is required!', action: 'blur', rule: 'required' },
        { input: '#txtDeltaa', message: 'Delta a is required!', action: 'blur', rule: 'required' },
        { input: '#txtDeltab', message: 'Delta b is required!', action: 'blur', rule: 'required' },
        { input: '#txtMFI220C10kg', message: 'MFI220C10kg is required!', action: 'blur', rule: 'required' },
        { input: '#txtSPGravity', message: 'SP Gravity is required!', action: 'blur', rule: 'required' },
        { input: '#txtAshcontent', message: 'Ash content is required!', action: 'blur', rule: 'required' },
        { input: '#txtNotchimpact', message: 'Notch impact is required!', action: 'blur', rule: 'required' },
        { input: '#txtTensile', message: 'Tensile is required!', action: 'blur', rule: 'required' },
        { input: '#txtFlexuralmodulus', message: 'Flexural modulus is required!', action: 'blur', rule: 'required' },
        { input: '#txtFlexuralStrength', message: 'Flexural Strength is required!', action: 'blur', rule: 'required' },
        { input: '#txtElongation', message: 'Elongation is required!', action: 'blur', rule: 'required' },
        { input: '#txtFlammablity', message: 'Flammablity is required!', action: 'blur', rule: 'required' },
        { input: '#txtGWTat', message: 'GWT at is required!', action: 'blur', rule: 'required' },
        { input: '#txtColorQANotes', message: 'Notes is required!', action: 'blur', rule: 'required' }
    ];

    $scope.FormulationRules = [
      { input: '#txtLotNo', message: 'Lot no is required!', action: 'blur', rule: 'required' },
      { input: '#txtGradeName', message: 'Grade name is required!', action: 'blur', rule: 'required' }
    ];


    $scope.FormulationNewSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'UOM', type: 'string' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'Notes', type: 'string' },
           { name: 'StatusName', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'ProductId', type: 'int' },
           { name: 'LineId', type: 'int' },
           { name: 'RMStatus', type: 'string' },
           { name: 'RMStatusId', type: 'int' },
           //{ name: 'ParentId', type: 'int' },
           { name: 'VerNo', type: 'int' }
        ],
        url: '/FormulationRequest/GetDataForDashboardGrid',
        data: { StatusId: 1 }
    };

    $scope.FormulationReceviedSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'UOM', type: 'string' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'Notes', type: 'string' },
           { name: 'StatusName', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'ProductId', type: 'int' },
           { name: 'LineId', type: 'int' }
        ],
        url: '/FormulationRequest/GetDataForDashboardGrid',
        data: { StatusId: 3 }
    };

    $scope.FormulationProgressSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'UOM', type: 'string' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'Notes', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'StatusName', type: 'string' },
           { name: 'ProductId', type: 'int' },
           { name: 'LineId', type: 'int' },
           { name: 'VerNo', type: 'int' }
        ],
        url: '/FormulationRequest/GetDataForDashboardGrid',
        data: { StatusId: 2 }
    };

    $scope.FormulationTestSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'UOM', type: 'string' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'Notes', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'StatusName', type: 'string' },
           { name: 'ProductId', type: 'int' },
           { name: 'LineId', type: 'int' },
           { name: 'VerNo', type: 'int' }
        ],
        url: '/FormulationRequest/GetDataForDashboardGrid',
        data: { StatusId: 3 }
    };

    $scope.FormulationOtherSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'UOM', type: 'string' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'Notes', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'StatusName', type: 'string' },
           { name: 'ProductId', type: 'int' },
           { name: 'LineId', type: 'int' },
           { name: 'QAStatusId', type: 'int' },
           { name: 'QAStatusName', type: 'string' },
           { name: 'VerNo', type: 'int' }
        ],
        url: '/FormulationRequest/GetDataForDashboardGrid',
        data: { StatusId: 4 }
    };

    $scope.FormulationCloseSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'UOM', type: 'string' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'Notes', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'StatusName', type: 'string' },
           { name: 'ProductId', type: 'int' },
           { name: 'LineId', type: 'int' },
           { name: 'VerifyBy', type: 'int' },
           { name: 'VerifyUser', type: 'string' },
           { name: 'QAStatusName', type: 'string' },
           { name: 'VerNo', type: 'int' }
        ],
        url: '/FormulationRequest/GetDataForCloseDashboardGrid',
        data: { StatusId: 6, Month: currentMonth, Year: currentYear }
    };

    $scope.MaterialCloseSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'ItemId', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'RequestedQty', type: 'decimal' },
           { name: 'IssuedQty', type: 'decimal' },
           { name: 'ReturnQty', type: 'decimal' },
           { name: 'WIPId', type: 'int' },
           { name: 'WIPQty', type: 'decimal' },
        ],
        url: '/FormulationRequest/GetRawMaterialListForCloseRequest',
        data: { formulationId: 0 }
    };

    $scope.MachineReadingSource = {
        datatype: "json",
        datafields: [
           { name: 'MachineId', type: 'int' },
           { name: 'MachineName', type: 'string' },
           { name: 'Reading', type: 'decimal' }
        ],
        url: '/FormulationRequest/GetMachineReadingDataById',
        data: { LineId: 0 }
    };

    $scope.RMRequestSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'RMRequestStatus', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'RequestBy', type: 'string' }
        ],
        url: '/FormulationRequest/GetRMRequestDataGird',
        data: { StatusId: 1 }
    };

    $scope.RMReceviedSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'RMRequestStatus', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'RequestBy', type: 'string' }
        ],
        url: '/FormulationRequest/GetRMRequestDataGird',
        data: { StatusId: 2 }
    };

    $scope.RMCloseReceviedSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LotNo', type: 'string' },
           { name: 'GradeName', type: 'string' },
           { name: 'QtyToProduce', type: 'string' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'LOTSize', type: 'string' },
           { name: 'ColorSTD', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'RMRequestStatus', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'RequestBy', type: 'string' }
        ],
        url: '/FormulationRequest/GetRMCloseRequestDataGird',
        data: { StatusId: 3 , Year : currentYear , Month : currentMonth ,LotNo : '' }
    };

    $scope.RMRequestDetailSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'RMRequestId', type: 'int' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'ItemId', type: 'string' },
           { name: 'RequestedQty', type: 'decimal' },
           { name: 'IssuedQty', type: 'decimal' },
        ],
        url: '/FormulationRequest/GetRMRequestDetailsById',
        data: { Id: 0, IsRecevied: false }
    };

    $scope.RawMaterialDetailSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'ItemId', type: 'string' },
           { name: 'RequestedQty', type: 'decimal' }
        ],
        url: '/FormulationRequest/GetRawMaterialsRequestById',
        data: { Id: 0 }
    };

    $scope.FormulationDetailSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'MachineId', type: 'int' },
           { name: 'ItemId', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'RequestedQty', type: 'decimal' },
           { name: 'IssuedQty', type: 'decimal' },
           { name: 'VerNo', type: 'int' }
        ],
        url: '/FormulationRequest/GetFormulationDetailsListById',
        data: { requestId: 0, IsParent : false }
    };

    $scope.FormulationRequestDetailsSource = {
        datatype: "json",
        localdata: FormulationDetailsData,
        autoBind: true,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'MachineId', type: 'int' },
           { name: 'MachineName', type: 'string' },
           { name: 'ItemId', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'ItemQtyGram', type: 'string' },
           { name: 'ItemQtyPercentage', type: 'string' }
        ]
    };

    $scope.FormulationDetailsSource = {
        datatype: "json",
        localdata: FormulationDetailsData,
        autoBind: true,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'FormulationRequestId', type: 'int' },
           { name: 'MachineId', type: 'int' },
           { name: 'MachineName', type: 'string' },
           { name: 'ItemId', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'ItemQtyGram', type: 'string' },
           { name: 'ItemQtyPercentage', type: 'string' }
        ]
    };

    $scope.FormulationRemarksSource = {
        datatype: "json",
        cache: false,
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'StatusName', type: 'string' },
            { name: 'Remarks', type: 'string' },
            { name: 'RemarksDate', type: 'date' },
            { name: 'RemarksBy', type: 'string' }
        ],
        url: '/FormulationRequest/GetFormulationRemarksById',
        data: { FormulationId: 0 }
    };

    $scope.MachineSource = {
        datatype: "json",
        datafields: [
           { name: 'MachineId', type: 'int' },
           { name: 'Name', type: 'string' },
        ],
        url: '/Machine/GetMachinebyLineId',
        data: { Id: 0 }
    };

    $scope.RMItemSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
        ],
        url: '/RMItem/GetItemListForFormula'
    };

    $scope.MonthSource = [{ Id: 1, Name: 'January' }, { Id: 2, Name: 'february' }, { Id: 3, Name: 'March' }, { Id: 4, Name: 'April' },
                 { Id: 5, Name: 'May' }, { Id: 6, Name: 'June' }, { Id: 7, Name: 'July' }, { Id: 8, Name: 'August' },
                 { Id: 9, Name: 'September' }, { Id: 10, Name: 'October' }, { Id: 11, Name: 'November' }, { Id: 12, Name: 'December' }];

    $scope.onMonthBindingComplete = function (e) {
        $scope.ddlMonth.selectedIndex = (new Date()).getMonth();
        //$scope.txtSearchYear.val((new Date()).getFullYear());
    }

    $scope.RMMonthSource = [{ Id: 1, Name: 'January' }, { Id: 2, Name: 'february' }, { Id: 3, Name: 'March' }, { Id: 4, Name: 'April' },
                { Id: 5, Name: 'May' }, { Id: 6, Name: 'June' }, { Id: 7, Name: 'July' }, { Id: 8, Name: 'August' },
                { Id: 9, Name: 'September' }, { Id: 10, Name: 'October' }, { Id: 11, Name: 'November' }, { Id: 12, Name: 'December' }];

    $scope.onRMMonthBindingComplete = function (e) {
        $scope.ddlRMMonth.selectedIndex = (new Date()).getMonth();
        //$scope.txtSearchYear.val((new Date()).getFullYear());
    }

    var y = (new Date()).getFullYear().toString();
    var year = [];
    for (var i = 0; i <= 15; i++) {
        year.push(y.toString());
        y--;
    }
    $scope.YearSource = {
        localdata: year,
        datatype: "array"
    };

    $scope.onYearBindingComplete = function (e) {
        $scope.ddlYear.selectedIndex = 0;
    }

    $scope.RMYearSource = {
        localdata: year,
        datatype: "array"
    };

    $scope.onRMYearBindingComplete = function (e) {
        $scope.ddlRMYear.selectedIndex = 0;
    }


    $scope.formulationDetailsColumn = [
        { text: 'Item', datafield: 'ItemName', editable: false },
        {
            text: 'Machine', datafield: 'MachineId', displayfield: 'Name', width: 150, sortable: false, columntype: 'combobox', editable: true,
            createeditor: function (row, cellvalue, editor, celltext, pressedChar) {
                var data = this.owner.getrowdata(row);
                editor.jqxComboBox({ theme: $scope.theme, autoComplete: true, dropDownHeight: 250, source: allMachineData, displayMember: 'Name', valueMember: 'MachineId', searchMode: 'containsignorecase' });
                editor.jqxComboBox('val', celltext);
            }
        },
         { text: 'Requested Qty', datafield: 'RequestedQty', width: 100, editable: false },
         { text: 'Issued Qty', datafield: 'IssuedQty', width: 100, editable: true }
    ];

    $scope.MaterialCloseColumns = [
        { text: 'Item', datafield: 'ItemName', editable: false },
        { text: 'Requested Qty ', datafield: 'RequestedQty', width: 130, editable: false, aggregates: ['sum'] },
        { text: 'Issued Qty', datafield: 'IssuedQty', width: 100, editable: false, aggregates: ['sum'] },
        { text: 'Return Qty', datafield: 'ReturnQty', width: 100, editable: true, aggregates: ['sum'] },
         {
             text: 'WIP Store', datafield: 'WIPId', displayfield: 'Name', width: 150, sortable: false, columntype: 'combobox', editable: true,
             createeditor: function (row, cellvalue, editor, celltext, pressedChar) {
                 var data = this.owner.getrowdata(row);
                 editor.jqxComboBox({ theme: $scope.theme, autoComplete: true, dropDownHeight: 250, source: allWIPData, displayMember: 'Name', valueMember: 'Id', searchMode: 'containsignorecase', promptText: "Please Choose:" });
                 editor.jqxComboBox('val', celltext);
             }
         },

        { text: 'WIP Qty', datafield: 'WIPQty', width: 100, editable: true, aggregates: ['sum'] }
    ];

    $scope.onFormulaNewFilterClick = function () {
        $scope.ShowFilterNew = !$scope.ShowFilterNew;
    };

    $scope.onFormulaReceviedFilterClick = function () {
        $scope.ShowFilterRecevied = !$scope.ShowFilterRecevied;
    };

    $scope.onFormulaProgressFilterClick = function () {
        $scope.ShowFilterProgress = !$scope.ShowFilterProgress;
    };

    $scope.onFormulaTestFilterClick = function () {
        $scope.ShowFilterTest = !$scope.ShowFilterTest;
    };

    $scope.onFormulaOtherFilterClick = function () {
        $scope.ShowFilterOther = !$scope.ShowFilterOther;
    };

    $scope.onFormulaCloseFilterClick = function () {
        $scope.ShowFilterClose = !$scope.ShowFilterClose;
    };

    $scope.OnRMFilterClick = function () {
        $scope.ShowFilterRMRequest = !$scope.ShowFilterRMRequest;
    };

    $scope.onFilterRMReceviedClick = function () {
        $scope.ShowFilterRMRecevied = !$scope.ShowFilterRMRecevied;
    };

    $scope.onFilterCloseRMReceviedClick = function () {
        $scope.ShowFilterRMCloseRecevied = !$scope.ShowFilterRMCloseRecevied;
    };

    var filterInfoNew = [];
    var filterRecevied = [];
    var filterInfoProgress = [];
    var filterInfoTest = [];
    var filterInfoOther = [];
    var filterClosed = [];
    var filterRM = [];
    var filterRMRecevied = [];
    var filterRMCloseReceived = [];

    $scope.onGridNewBinidingComplete = function () {
        $.each(filterInfoNew, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdFormulationNew.addfilter(filterGroup.datafield, group);
            $scope.GrdFormulationNew.applyfilters();
        });
    };

    //$scope.onGridReceviedBinidingComplete = function () {
    //    $.each(filterRecevied, function (i, filterGroup) {
    //        var group = new $.jqx.filter();
    //        $.each(filterGroup.filter.getfilters(), function (j, filter) {
    //            group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
    //        });
    //        $scope.GrdFormulationRecevied.addfilter(filterGroup.datafield, group);
    //        $scope.GrdFormulationRecevied.applyfilters();
    //    });
    //};

    $scope.onGridProgressBinidingComplete = function () {
        $.each(filterInfoProgress, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdFormulationProgress.addfilter(filterGroup.datafield, group);
            $scope.GrdFormulationProgress.applyfilters();
        });
    };

    $scope.onGridTestBinidingComplete = function () {
        $.each(filterInfoOther, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdFormulationOther.addfilter(filterGroup.datafield, group);
            $scope.GrdFormulationOther.applyfilters();
        });
    };

    $scope.onGridOtherBinidingComplete = function () {
        $.each(filterInfoTest, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdFormulationTest.addfilter(filterGroup.datafield, group);
            $scope.GrdFormulationTest.applyfilters();
        });
    };

    $scope.onGridCloseBinidingComplete = function () {
        $.each(filterClosed, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdFormulationClose.addfilter(filterGroup.datafield, group);
            $scope.GrdFormulationClose.applyfilters();
        });
    };

    $scope.onGridRMRequestBinidingComplete = function () {
        $.each(filterRM, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdRMRequest.addfilter(filterGroup.datafield, group);
            $scope.GrdRMRequest.applyfilters();
        });
    };

    $scope.onGridRMReceviedBinidingComplete = function () {
        $.each(filterRMRecevied, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdRMRecevied.addfilter(filterGroup.datafield, group);
            $scope.GrdRMRecevied.applyfilters();
        });
    };

    $scope.onGridRMCloseReceviedBinidingComplete = function () {
        $.each(filterRMCloseReceived, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GrdRMCloseRecevied.addfilter(filterGroup.datafield, group);
            $scope.GrdRMCloseRecevied.applyfilters();
        });
    };

    $scope.OnExport = function (exportType, grid) {
        if (exportType == 'excel') {
            if (grid == 'new') {
                var StatusId = 1;
                window.location = '/Download/FormulationRequestList?statusId=' + StatusId;
            }
            //else if (grid == 'recevied') {
            //    var StatusId = 3;
            //    window.location = '/Download/FormulationRequestList?statusId=' + StatusId;
            //}
            else if (grid == 'progress') {
                var StatusId = 2;
                window.location = '/Download/FormulationRequestList?statusId=' + StatusId;
            }
            else if (grid == 'test') {
                var StatusId = 3;
                window.location = '/Download/FormulationRequestList?statusId=' + StatusId;
            }
            else if (grid == 'other') {
                var StatusId = 4;
                window.location = '/Download/FormulationRequestList?statusId=' + StatusId;
            }
            else if (grid == 'close') {
                var StatusId = 6;
                window.location = '/Download/FormulationRequestList?statusId=' + StatusId;
            }
            else if (grid == 'RMRequest') {
                var StatusId = 1;
                window.location = '/Download/RMRequestList?statusId=' + StatusId;
            }
            else if (grid == 'RMReceived') {
                var StatusId = 2;
                window.location = '/Download/RMRequestList?statusId=' + StatusId;
            }
            else if (grid == 'IsReceived') {
                var StatusId = 3;
                window.location = '/Download/RMRequestList?statusId=' + StatusId;
            }
        }
    }

    $scope.OnPrint = function (grid) {
        if (grid == 'new') {
            var gridContent = $scope.GrdFormulationNew.exportdata('html');
        }
        else if (grid == 'recevied') {
            var gridContent = $scope.GrdFormulationRecevied.exportdata('html');
        }
        else if (grid == 'progress') {
            var gridContent = $scope.GrdFormulationProgress.exportdata('html');
        }
        else if (grid == 'test') {
            var gridContent = $scope.GrdFormulationTest.exportdata('html');
        }
        else if (grid == 'close') {
            var gridContent = $scope.GrdFormulationClose.exportdata('html');
        }
        else if (grid == 'other') {
            var gridContent = $scope.GrdFormulationOther.exportdata('html');
        }
        else if (grid == 'RMRequest') {
            var gridContent = $scope.GrdRMRequest.exportdata('html');
        }
        else if (grid == 'RMReceived') {
            var gridContent = $scope.GrdRMRecevied.exportdata('html');
        }
        else if (grid == 'IsReceived') {
            var gridContent = $scope.GrdRMCloseRecevied.exportdata('html');
        }
        var newWindow = window.open('', '', 'width=800, height=500'),
        document = newWindow.document.open(),
        pageContent =
            '<!DOCTYPE html>\n' +
            '<html>\n' +
            '<head>\n' +
            '<meta charset="utf-8" />\n' +
            '<title>jQWidgets Grid</title>\n' +
            '</head>\n' +
            '<body>\n' + gridContent + '\n</body>\n</html>';
        document.write(pageContent);
        document.close();
        newWindow.print();
    }

    $scope.onFormulaNewRefreshClick = function (e) {
        filterInfoNew = $scope.GrdFormulationNew.getfilterinformation();
        $scope.GrdFormulationNew.updatebounddata();
    };

    $scope.onFormulaReceviedRefreshClick = function (e) {
        filterRecevied = $scope.GrdFormulationRecevied.getfilterinformation();
        $scope.GrdFormulationRecevied.updatebounddata();
    };

    $scope.onFormulaProgressRefreshClick = function (e) {
        filterInfoProgress = $scope.GrdFormulationProgress.getfilterinformation();
        $scope.GrdFormulationProgress.updatebounddata();
    };

    $scope.onFormulaTestRefreshClick = function (e) {
        filterInfoTest = $scope.GrdFormulationTest.getfilterinformation();
        $scope.GrdFormulationTest.updatebounddata();
    };

    $scope.onFormulaOtherRefreshClick = function (e) {
        filterInfoOther = $scope.GrdFormulationOther.getfilterinformation();
        $scope.GrdFormulationOther.updatebounddata();
    };

    $scope.onFormulaCloseRefreshClick = function (e) {
        filterClosed = $scope.GrdFormulationClose.getfilterinformation();
        $scope.GrdFormulationClose.updatebounddata();
    };

    $scope.OnRMRefreshClick = function (e) {
        filterRM = $scope.GrdRMRequest.getfilterinformation();
        $scope.GrdRMRequest.updatebounddata();
    };

    $scope.OnRMReceviedRefreshClick = function (e) {
        filterRMRecevied = $scope.GrdRMRecevied.getfilterinformation();
        $scope.GrdRMRecevied.updatebounddata();
    };

    $scope.OnRMCloseReceviedRefreshClick = function (e) {
        filterRMCloseReceived = $scope.GrdRMCloseRecevied.getfilterinformation();
        $scope.GrdRMCloseRecevied.updatebounddata();
    };

    $scope.RMRequestrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.GrdFormulationNew.getrowdata(row);
        if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
            return "Request RM";
        }
        else {
            $scope.GrdFormulationNew.hidecolumn("RMRequest");
        }

    }
    var requestId = 0;
    $scope.RMRequest = function (row, event) {
        var dataRecord = $scope.GrdFormulationNew.getrowdata(row);
        requestId = dataRecord.Id;
        RequestStatus = dataRecord.StatusId;
        $http.post('/FormulationRequest/GetRMRequestStatus', { RequestId: requestId }).then(function (response) {
            //First function handles success
           // alert(response);
            if (response.data.Data == true) {
                //if (RequestStatus == 1) { 
                    $scope.RawMaterialDetailSource.data.Id = requestId;
                    $scope.GrdRawMaterial.updatebounddata();
                    $scope.GrdRawMaterial.editable = true;
                    $scope.RequestRemarkWindow.open();
                //}
            }
            else
                $scope.openMessageBox("Warning", "RM request already generated", 300, 100);
        }, function (response) {
            //Second function handles error
        });
       
        //else if (RequestStatus == 1 && dataRecord.RMStatusId == 3) {
        //    MachineData(dataRecord.LineId);
        //    rFormulationId = dataRecord.Id;
        //    RequestStatus = dataRecord.StatusId;
        //    $scope.FormulationDetailSource.data.requestId = dataRecord.Id;
        //    $scope.GrdFormulationDetail.updatebounddata();
        //    $scope.GrdFormulationDetail.editable = true;
        //    $scope.FormulationStartWindow.open();
        //}
        //else
        //    $scope.openMessageBox("Warning", "RM request already generated",300,100);
    };

    $scope.StartNewFormulationtrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.GrdFormulationNew.getrowdata(row);
        if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
            return "Start";
        }
        else {
            $scope.GrdFormulationNew.hidecolumn("Start");
        }
    }
    var newChangeFormulationId = 0;
    $scope.StartNewFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationNew.getrowdata(row);
        requestId = dataRecord.Id;
        RequestStatus = dataRecord.StatusId;
        //parentId = dataRecord.ParentId;
        if (requestId > 0) {
            $http.post('/FormulationRequest/IsRMReceived', { RequestId: requestId }).then(function (response) {
                if (response.data.Data == true) {
                    MachineData(dataRecord.LineId);
                    rFormulationId = dataRecord.Id;
                    RequestStatus = dataRecord.StatusId;
                    $scope.FormulationDetailSource.data.requestId = dataRecord.Id;
                    $scope.FormulationDetailSource.data.IsParent = false;
                    $scope.GrdFormulationDetail.updatebounddata();
                    $scope.GrdFormulationDetail.editable = true;
                    $scope.FormulationStartWindow.open();
                }
                else
                    $scope.openMessageBox("Warning", "Raw material not received", 300, 100);
            }, function (response) {
                //Second function handles error
            });
        }
    };

    //var UpdateRMChangeFormulation = function () {
    //    if (parentId > 0) {
    //        $http.post('/FormulationRequest/UpdateChangeRMData', { FormulationId: newChangeFormulationId }).then(function (result) {
    //            if (result.data.Status == 1) {
    //                $scope.GrdFormulationNew.updatebounddata();
    //                newChangeFormulationId = 0;
    //            }
    //            else {
    //                $scope.GrdFormulationNew.updatebounddata();
    //                $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
    //            }
    //        }, function (result, status, headers, config) {
    //            alert("status");
    //        });
    //    }
    //}

    $scope.onRequestRemarksClick = function (event) {
        var rmData = [];
        var rows = $scope.GrdRawMaterial.getrows();
        for (var i = 0; i < rows.length; i++) {
            var item = rows[i];
            var postItem = {
                Id: 0, RMRequestId: 0, FormulationRequestId: item.FormulationRequestId, ItemId: item.ItemId, RequestedQty: item.RequestedQty
            };
            rmData.push(postItem);
        }
        var requestRemarks = $scope.txtRequestRemarks.val();
        $http.post('/FormulationRequest/SaveRMRequest', { requestId: requestId, statusId: RequestStatus, remarks: requestRemarks, rmDetailsData: rmData }).then(function (result) {
            if (result.data.Status == 1) {
                $scope.GrdFormulationNew.updatebounddata();
                $scope.GrdRMRequest.updatebounddata();
                $scope.GrdRMRecevied.updatebounddata();
                $scope.RequestRemarkWindow.close();
                requestId = 0;
                RequestStatus = 0;
                $scope.txtRequestRemarks.val("");
                $scope.openMessageBox('Success', result.data.Message, 350, 90);
            }
            else {
                $scope.GrdFormulationNew.updatebounddata();
                $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    }



    $scope.Newformulationrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        return "Start";
    }
    LineId = 0;
    var rFormulationId = 0;
    $scope.NewFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationNew.getrowdata(row);
        if (dataRecord.StatusId == 4) {
            MachineData(dataRecord.LineId);
            rFormulationId = dataRecord.Id;
            RequestStatus = dataRecord.StatusId;
            $scope.FormulationDetailSource.data.requestId = dataRecord.Id;
            $scope.FormulationDetailSource.data.IsParent = false;
            $scope.GrdFormulationDetail.updatebounddata();
            $scope.GrdFormulationDetail.editable = true;
            $scope.FormulationStartWindow.open();
        }
        else {
            $scope.openMessageBox('Warning', "Raw material not received. Process can not start.", 350, 90);
        }

    };

  

    var MachineData = function (LineId) {
        $.ajax({
            dataType: "json",
            type: 'GET',
            contentType: "application/json;",
            url: '/Machine/GetMachinebyLineId',
            data: { Id: LineId },
            success: function (response) {
                allMachineData = response;
            },
            error: function (response) {
            }
        });
    }

    var WIPStoreData = function () {
        $.ajax({
            dataType: "json",
            type: 'GET',
            contentType: "application/json;",
            url: '/WIPStore/GetWIPForCloseRequest',
            success: function (response) {
                allWIPData = response;
            },
            error: function (response) {
            }
        });
    }



    $scope.ViewNewformulationrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        return "View";
    }
    viewFormulationId = 0;
    $scope.ViewNewFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationNew.getrowdata(row);
        viewFormulationId = dataRecord.Id;
        $scope.IsBatch = false;
        $scope.IsTestResult = false;
        GetFormulationById(viewFormulationId);
        //$scope.FormulationViewWindow.open();
    }

    //$scope.Receviedformulationrender = function (row, column, value) {
    //    return "Start";
    //}
    //$scope.StartRemarks = '';
    //$scope.FormulationRecevied = function (row, event) {
    //    var dataRecord = $scope.GrdFormulationRecevied.getrowdata(row);
    //    MachineData(dataRecord.LineId);
    //    rFormulationId = dataRecord.Id;
    //    RequestStatus = dataRecord.StatusId;
    //    $scope.FormulationDetailSource.data.requestId = dataRecord.Id;
    //    $scope.GrdFormulationDetail.updatebounddata();
    //    $scope.GrdFormulationDetail.editable = true;
    //    $scope.FormulationStartWindow.open();
    //}

    $scope.onStartClick = function (event) {
        var detailsData = [];
        if (rFormulationId > 0) {
            var Remarks = $scope.txtStartRemarks.val();
            var rows = $scope.GrdFormulationDetail.getrows();

            var fomulationdetails = $.grep(rows, function (a) {
                return a.MachineId > 0;
            })

            if (fomulationdetails.length == rows.length) {
                for (var i = 0; i < fomulationdetails.length; i++) {
                    var item = fomulationdetails[i];
                    var postItem = {
                        Id: 0, FormulationRequestId: item.FormulationRequestId, MachineId: item.MachineId, ItemId: item.ItemId, ItemQtyPercentage: item.RequestedQty, ItemQtyGram: item.IssuedQty , VerNo : item.VerNo
                    };
                    detailsData.push(postItem);
                }
                $http.post('/FormulationRequest/UpdateFormulationMachine', { RequestId: rFormulationId, detailsData: detailsData, Remarks: Remarks }).then(function (result) {
                    if (result.data.Status == 1) {
                        //$scope.GrdFormulationRecevied.updatebounddata();
                        $scope.GrdFormulationNew.updatebounddata();
                        $scope.GrdRMRequest.updatebounddata();
                        $scope.GrdFormulationProgress.updatebounddata();
                        $scope.GrdFormulationOther.updatebounddata();
                        $scope.FormulationStartWindow.close();
                        $scope.txtStartRemarks.val("");
                        $scope.StartRemarks = "";
                        RequestStatus = 0;
                        rFormulationId = 0;
                    }
                    else {
                        $scope.GrdFormulationNew.updatebounddata();
                        $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                });
            }
            else {
                $scope.openMessageBox('Waring', "Please select Machine!", 300, 90);
            }
        }
    }

    $scope.ViewReceviedformulationrender = function (row, column, value) {
        return "View";
    }
    $scope.ViewReceviedFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationRecevied.getrowdata(row);
        viewFormulationId = dataRecord.Id;
        $scope.IsBatch = false;
        $scope.IsTestResult = false;
        GetFormulationById(viewFormulationId);
    }

    $scope.Progressformulationrender = function (row, column, value) {
        var dataRecord = $scope.GrdFormulationProgress.getrowdata(row);
        if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
            return "Sample Ready";
        }
        else {
            $scope.GrdFormulationProgress.hidecolumn("SampleReady");
        }
    }
    $scope.ProgressFormulation = function (row, event) {
        $scope.RemarksValidator.hide();
        var dataRecord = $scope.GrdFormulationProgress.getrowdata(row);
        RequestId = dataRecord.Id;
        RequestStatus = dataRecord.StatusId;
        TestFail = false;
        $scope.FormulationRemarksWindow.open();
    };

    $scope.ViewProgressformulationrender = function (row, column, value) {
        return "View";
    }

    $scope.ViewProgressFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationProgress.getrowdata(row);
        viewFormulationId = dataRecord.Id;
        $scope.IsBatch = false;
        $scope.IsTestResult = false;
        GetFormulationById(viewFormulationId);
        //$scope.FormulationViewWindow.open();
    }

    $scope.testPassFormulationrender = function (row, column, value) {
        var dataRecord = $scope.GrdFormulationTest.getrowdata(row);
        if (userRole == "QA" || userRole == "QAManager" || userRole == "Admin") {
            return "Finish Testing";
        }
        else {
            $scope.GrdFormulationTest.hidecolumn("Pass");
        }
    }
    $scope.testPassFormulation = function (row, event) {
        $scope.RemarksValidator.hide();
        var dataRecord = $scope.GrdFormulationTest.getrowdata(row);
        RequestId = dataRecord.Id;
        RequestStatus = dataRecord.StatusId;
        TestFail = false;
        $scope.ReadyForTestingWindow.open();
        //$scope.FormulationRemarksWindow.open();
    };

    $scope.onOkGoClick = function (event) {
        var isValidate = $scope.ColoursQAValidator.validate();
        if (!isValidate)
            return;

        $scope.model.ColourSpec.FormulationRequestId = RequestId;
        $scope.model.QASpec.FormulationRequestId = RequestId;
        colourSpecData = $scope.model.ColourSpec;
        qaSpecData = $scope.model.QASpec;
        IsColoursQA = "Ok";
        CommanColourssQA(colourSpecData, qaSpecData, IsColoursQA);
    };

    $scope.onStopProductionClick = function (event) {
        var isValidate = $scope.ColoursQAValidator.validate();
        if (!isValidate)
            return;

        $scope.model.ColourSpec.FormulationRequestId = RequestId;
        $scope.model.QASpec.FormulationRequestId = RequestId;
        colourSpecData = $scope.model.ColourSpec;
        qaSpecData = $scope.model.QASpec;
        IsColoursQA = "Stop Production";
        CommanColourssQA(colourSpecData, qaSpecData, IsColoursQA);
    };
    IsFormulationchanged = false;

    $scope.onChangeFormulationClick = function (event) {
        var isValidate = $scope.ColoursQAValidator.validate();
        if (!isValidate)
            return;
        $scope.model.ColourSpec.FormulationRequestId = RequestId;
        $scope.model.QASpec.FormulationRequestId = RequestId;
        colourSpecData = $scope.model.ColourSpec;
        qaSpecData = $scope.model.QASpec;
        IsColoursQA = "Changed Formulation";
        if (IsFormulationchanged) {
            //$scope.openMessageBox('Warning', 'Formulation changed now', 200, 50);
            colourSpecData = [];
            qaSpecData = [];
        }
        else {
            FormulationDetailsData = [];
            
            $.ajax({
                url: "/FormulationRequest/GetFormulationById",
                type: "GET",
                dataType: "json",
                cache: false,
                data: { Id: RequestId },
                success: function (data) {
                    $scope.$apply(function () {
                        $scope.enableProperty = true;
                        FId = data.formulation.Id;
                        $scope.model.FormulationRequests = data.formulation;
                        $scope.model.FormulationRequests.Id = data.formulation.Id;
                        $scope.model.FormulationRequests.LotNo = data.formulation.LotNo;
                        $scope.model.FormulationRequests.GradeName = data.formulation.GradeName;
                        $scope.model.FormulationRequests.LOTSize = data.formulation.LOTSize;
                        $scope.model.FormulationRequests.ColorSTD = data.formulation.ColorSTD;
                        $scope.model.FormulationRequests.QtyToProduce = data.formulation.QtyToProduce;
                        $scope.model.FormulationRequests.WorkOrderNo = data.formulation.WorkOrderNo;
                        qtytoProduce = data.formulation.QtyToProduce;
                        $scope.model.FormulationRequests.Notes = data.formulation.Notes;
                        FormulationDetailsData = data.formulationDetail;
                        $scope.FormulationDetailsSource.localdata = $.grep(FormulationDetailsData, function (item, i) {
                            return item;
                        });
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
            $scope.WindowChangeFormualtion.open();
        }
        //$scope.GrdFormulationNew.updatebounddata();
        //$scope.GrdFormulationOther.updatebounddata();
        //$scope.GrdFormulationTest.updatebounddata();
       
            //$scope.ReadyForTestingWindow.close();
    };

    

    var CommanColourssQA = function (colourSpecData, qaSpecData, IsColoursQA) {
        var QANotes = $scope.txtColorQANotes.val();
        $http.post('/FormulationRequest/SaveColourQAData', { colourSpecData: colourSpecData, qaSpecData: qaSpecData, IsColoursQA: IsColoursQA ,Notes : QANotes }).then(function (result) {
            if (result.data.Status == 1) {
                    RequestId = 0;
                    colourSpecData = [];
                    qaSpecData = [];
                    $scope.GrdFormulationNew.updatebounddata();
                    $scope.GrdFormulationOther.updatebounddata();
                    $scope.GrdFormulationTest.updatebounddata();
                    $scope.ReadyForTestingWindow.close();
                    $scope.txtColorQANotes.val("");
                    var IsColoursQA = "";
                    ClearData();
            }
            else {
                $scope.GrdFormulationTest.updatebounddata();
                $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    }

    $scope.testFailFormulationrender = function (row, column, value) {
        return "Test Fail";
    }
    $scope.testFailFormulation = function (row, event) {
        $scope.RemarksValidator.hide();
        var dataRecord = $scope.GrdFormulationTest.getrowdata(row);
        RequestId = dataRecord.Id;
        RequestStatus = dataRecord.StatusId;
        TestFail = true;
        $scope.FormulationRemarksWindow.open();
    };

    $scope.testViewFormulationrender = function (row, column, value) {
        return "View";
    }

    $scope.ViewtestFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationTest.getrowdata(row);
        viewFormulationId = dataRecord.Id;
        $scope.IsBatch = false;
        $scope.IsTestResult = false;
        GetFormulationById(viewFormulationId);
        //$scope.FormulationViewWindow.open();
    }

    $scope.CloseOtherFormulationrender = function (row, column, value) {
        var dataRecord = $scope.GrdFormulationOther.getrowdata(row);
        if (dataRecord.StatusId == 4 || dataRecord.QAStatusId == 2) {
            if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
                return "Close";
            }
            else {
                $scope.GrdFormulationOther.hidecolumn("Close");
            }
        }
        else if (dataRecord.StatusId == 8) {
            if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
                return "Start";
            }
            else {
                $scope.GrdFormulationOther.hidecolumn("Close");
            }
        }
    }
    var VersionNo = 0;
    $scope.CloseOtherFormulation = function (row, event) {
        RequestId = 0;
        rFormulationId = 0;
        var dataRecord = $scope.GrdFormulationOther.getrowdata(row);
        RequestId = dataRecord.Id;
        RequestStatus = dataRecord.StatusId;
        lotNo = dataRecord.LotNo;
        VersionNo = dataRecord.VerNo;
        $scope.model=[];
        if (dataRecord.StatusId == 2 || dataRecord.StatusId == 4) {
            WIPStoreData();
            $scope.MaterialCloseSource.data.formulationId = dataRecord.Id;
            $scope.GrdFormulationMaterial.updatebounddata();
            $scope.MachineReadingSource.data.LineId = dataRecord.LineId;
            $scope.GrdMachineReading.updatebounddata();
            $scope.FormulationCloseValidator.hide();
            $('#liformulationclose1').removeClass('active');
            $('#liformulationclose2').removeClass('active');
            $('#liformulationclose3').removeClass('active');
            $('#tab_Close_1').removeClass('active');
            $('#tab_Close_2').removeClass('active');
            $('#tab_Close_3').removeClass('active');
            $('#liformulationclose1').addClass('active');
            $('#tab_Close_1').addClass('active');
            $scope.FormulationCloseWindow.open();
        }
        else if (dataRecord.StatusId == 8) {
            $http.post('/FormulationRequest/IsChangedRMReceived', { RequestId: RequestId, VersionNo: VersionNo }).then(function (response) {
                if (response.data.Data == true) {
                    MachineData(dataRecord.LineId);
                    rFormulationId = dataRecord.Id;
                    RequestStatus = dataRecord.StatusId;
                    $scope.FormulationDetailSource.data.requestId = dataRecord.Id;
                    $scope.FormulationDetailSource.data.IsParent = false;
                    $scope.GrdFormulationDetail.updatebounddata();
                    $scope.GrdFormulationDetail.editable = true;
                    $scope.FormulationStartWindow.open();
                }
                else
                    $scope.openMessageBox("Warning", "Raw material not received", 300, 100);
            });
        }
    };

    $scope.UpdateItemQtyPercentagechange = function (rowindex, datafield, columntype, oldvalue, newvalue) {
        if (newvalue == "") {
            return oldvalue;
        }
        var data = $scope.GrdFormulationRequest.getrowdata(rowindex);
        if (typeof (data.ItemQtyPercentage) != 'undefined' && data.ItemQtyPercentage != null) {
            data.ItemQtyGram = (newvalue * qtytoProduce) / 100;
        }

    };

    $scope.UpdateItemInGramrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        var data = $scope.GrdFormulationRequest.getrowdata(row);
        if (value != null || value != 'undefined') {
            return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + ';">' + value + '</span>';
        }
    };

    $scope.DeleteItemrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        return 'Delete';
    };

    $scope.DeleteItem = function (row, event) {
        var data = $scope.GrdFormulationRequest.getrowdata(row);
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Item?', 350, 100, function (e) {
            if (e) {
                $scope.GrdFormulationRequest.deleterow(row);
                DeleteFormulationID.push(data.Id);
            }
        });
    };

    $scope.onAddNewItem = function (event) {
        $scope.WindowAddItem.open();
       
    }

    $scope.SaveItem = function (event) {
        var itemId = $scope.ddlItemId.getSelectedItem().value;
        var ItemName = $scope.ddlItemId.getSelectedItem().label;
        var PercebaseValue = $scope.txtIteminPerc.val();
        var baseValueGram = (PercebaseValue * qtytoProduce) / 100;
        var rows = {
            Id: 0, ItemId: itemId, ItemName: ItemName,
            ItemQtyPercentage: PercebaseValue, ItemQtyGram: baseValueGram
        };
        $scope.GrdFormulationRequest.addrow(null, rows);
        $scope.ddlItemId.clearSelection();
        $scope.txtIteminPerc.val("");
        $scope.WindowAddItem.close()
    }

    $scope.onUpdateFormulationClick = function (event) {
        var postData = [];
        masterData = [];
        var rows = $scope.GrdFormulationRequest.getrows();
        for (var i = 0; i < rows.length; i++) {
            var item = rows[i];
            var postItem = {
                Id: 0, FormulationRequestId: $scope.model.FormulationRequests.Id, MachineId: item.MachineId, ItemId: item.ItemId, ItemQtyPercentage: item.ItemQtyPercentage, ItemQtyGram: item.ItemQtyGram
            };
            postData.push(postItem);
        }
        $scope.model.FormulationRequests.Id = $scope.model.FormulationRequests.Id;
        $scope.model.LineId = $scope.model.FormulationRequests.LineId;
        $scope.model.LotNo = $scope.model.FormulationRequests.LotNo;
        $scope.model.GradeName = $scope.model.FormulationRequests.GradeName;
        $scope.model.FormulationRequests.StatusId = 1;
        $scope.model.FormulationRequests.ProductId = $scope.model.FormulationRequests.ProductId;;
        $scope.model.FormulationRequestsDetails = postData;
        masterData = $scope.model.FormulationRequests;
        masterData.FormulationRequestsDetails = $scope.model.FormulationRequestsDetails;
        IsFormulationchanged = true;
        $scope.WindowChangeFormualtion.close();
        var QANotes = $scope.txtColorQANotes.val();
        $http.post('/FormulationRequest/SaveChangedColourQAData', { colourSpecData: colourSpecData, qaSpecData: qaSpecData, IsColoursQA: IsColoursQA, Notes: QANotes, formulationData: masterData, DeleteFormulationDetailsID: DeleteFormulationID }).then(function (result) {
            if (result.data.Status == 1) {
                RequestId = 0;
                colourSpecData = [];
                qaSpecData = [];
                DeleteFormulationID = [];
                IsFormulationchanged = false;
                $scope.GrdFormulationNew.updatebounddata();
                $scope.GrdFormulationOther.updatebounddata();
                $scope.GrdFormulationTest.updatebounddata();
                $scope.ReadyForTestingWindow.close();
                $scope.txtColorQANotes.val("");
                var IsColoursQA = "";
                ClearData();
                masterData = [];
            }
            else {
                $scope.GrdFormulationTest.updatebounddata();
                $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    };

    $scope.onSaveRemarksClick = function () {
        var isValidate = $scope.RemarksValidator.validate();
        if (!isValidate)
            return;

        var notes = $scope.fomulationRemarks;
        $http.post('/FormulationRequest/UpdateFormulationNotes', { RequestId: RequestId, RequestStatus: RequestStatus, TestFail: TestFail, Notes: notes }).then(function (result) {
            if (result.data.Status == 1) {
                $scope.FormulationRemarksWindow.close();
                RequestId = 0;
                RequestStatus = 0;
                $scope.fomulationRemarks = "";
                $scope.GrdFormulationProgress.updatebounddata();
                $scope.GrdFormulationTest.updatebounddata();
                $scope.GrdFormulationOther.updatebounddata();
                $scope.RemarksValidator.hide();
            }
            else {
                $scope.GrdFormulationNew.updatebounddata();
                $scope.GrdFormulationProgress.updatebounddata();
                $scope.GrdFormulationTest.updatebounddata();
                $scope.GrdFormulationOther.updatebounddata();
                $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    };
    $scope.onSaveFormulationClose = function (event) {
        fomulationCloseData = [];
        var isValidate = $scope.FormulationCloseValidator.validate();
        if (!isValidate) {
            $scope.openMessageBox('Warning', 'Please update formulation close details', 350, 100);
            return;
        }


        var rows = $scope.GrdFormulationMaterial.getrows();
        var machinerows = $scope.GrdMachineReading.getrows();
        var fomulationData = $.grep(rows, function (a) {
            return a.ReturnQty >= 0;
        });

        var machineReadingData = $.grep(machinerows, function (a) {
            return a.MachineId >= 0;
        });

        $scope.openConfirm("Confirmation", 'Please make sure you have update all required information. Are you want to close this batch?', 360, 100, function (e) {
            if (e) {
                if (fomulationData.length == rows.length) {
                    for (var i = 0; i < fomulationData.length; i++) {
                        var item = fomulationData[i];
                        if ((item.WIPId > 0 && item.WIPQty > 0) || (item.WIPId == 0 && item.WIPQty == 0)) {
                            var postItem = {
                                Id: item.Id, FormulationRequestId: item.FormulationRequestId, ItemId: item.ItemId, RequestedQty: item.RequestedQty, IssuedQty: item.IssuedQty, ReturnQty: item.ReturnQty, WIPId: item.WIPId, WIPQty: item.WIPQty
                            };
                            fomulationCloseData.push(postItem);
                        }
                        else
                        {
                            $scope.openMessageBox("Warnig", "Please enter WIP qty as per selected store",300,100);
                            return;
                        }
                        
                    }
                    var Mdata = [];
                    for (var j = 0; j < machineReadingData.length; j++) {
                        var machineItem = machineReadingData[j];
                        var postMachineItem = {
                            Id: 0, FormulationRequestId: RequestId, MachineId: machineItem.MachineId, Reading: machineItem.Reading
                        };
                        Mdata.push(postMachineItem);
                    }

                    var formulationClose = $scope.model.FormulationClose;
                    formulationClose.FormulationRequestId = RequestId;
                    formulationClose.LotNo = lotNo;
                    var closeRemarks = $scope.CloseNotes;
                    $http.post('/FormulationRequest/SaveFormulationClose', { formulationClose: formulationClose, formulationId: RequestId, closeRemarks: closeRemarks, fomulationCloseData: fomulationCloseData, machineReadingData: Mdata }).then(function (result) {
                        if (result.data.Status == 1) {
                            $scope.model = {};
                            RequestId = 0;
                            $scope.GrdFormulationOther.updatebounddata();
                            $scope.GrdFormulationClose.updatebounddata();
                            $scope.FormulationCloseWindow.close();
                            ClearData();
                            $scope.model = {};
                        }
                        else {
                            $scope.GrdFormulationOther.updatebounddata();
                            $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                        }
                    }, function (result, status, headers, config) {
                        alert("status");
                    });
                }
            }
            else {

            }
        })
    };

    $scope.RMRequstOtherFormulationrender = function (row, column, value) {
        var dataRecord = $scope.GrdFormulationOther.getrowdata(row);
        if (dataRecord.StatusId == 2 || dataRecord.QAStatusId == 2 || dataRecord.StatusId == 8) {
            if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
                return "Request RM";
            }
            else {
                $scope.GrdFormulationOther.hidecolumn("RequestRM");
            }
        }
    }

    $scope.RMRequestOtherFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationOther.getrowdata(row);
        requestId = dataRecord.Id;
        if (dataRecord.StatusId == 2 || dataRecord.QAStatusId == 2 || dataRecord.StatusId == 8) {
            $http.post('/FormulationRequest/IsChangeRMExists', { RequestId: requestId }).then(function (response) {
                if (response.data.Data == true) {
                    RequestStatus = 1;
                    $scope.RawMaterialDetailSource.data.Id = requestId;
                    $scope.GrdRawMaterial.updatebounddata();
                    $scope.GrdRawMaterial.editable = true;
                    $scope.RequestRemarkWindow.open();
            }
            else
                $scope.openMessageBox("Warning", "RM request already generated", 300, 100);
        }, function (response) {
            //Second function handles error
        });
            
        }
        
    }


    $scope.ViewOtherFormulationrender = function (row, column, value) {
        return "View";
    }

    $scope.ViewOtherFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationOther.getrowdata(row);
        viewFormulationId = dataRecord.Id;
        $scope.IsBatch = false;
        $scope.IsTestResult = true;
        GetFormulationById(viewFormulationId);
        //$scope.FormulationViewWindow.open();
    }

    $scope.ProgressRMRequestrender = function (row, column, value) {
        var dataRecord = $scope.GrdFormulationProgress.getrowdata(row);
        if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
            return "Request RM";
        }
        else {
            $scope.GrdFormulationProgress.hidecolumn("RequestRM");
        }
    }

    $scope.ProgressRMRequest = function (row, event) {
        var dataRecord = $scope.GrdFormulationProgress.getrowdata(row);
        requestId = 0;
        RequestStatus = 0;
        if (dataRecord.StatusId == 2 || dataRecord.QAStatusId == 2 || dataRecord.StatusId == 8) {
            requestId = dataRecord.Id;
            RequestStatus = 1;
            $scope.RawMaterialDetailSource.data.Id = requestId;
            $scope.GrdRawMaterial.updatebounddata();
            $scope.GrdRawMaterial.editable = true;
            $scope.RequestRemarkWindow.open();
        }
    }
    var materialRequestId = 0;
    $scope.ViewRMNewrender = function (row, column, value) {
        var dataRecord = $scope.GrdRMRequest.getrowdata(row);
        if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
            return "View";
        }
        else {
            $scope.GrdRMRequest.hidecolumn("View");
        }
    }
    var formulationId = 0;
    $scope.ViewRMNewRequest = function (row, event) {
        var dataRecord = $scope.GrdRMRequest.getrowdata(row);
        formulationId = dataRecord.FormulationRequestId;
        rawMaterialId = dataRecord.Id;
        materialRequestId = dataRecord.Id;
        $scope.RMRequestDetailSource.data.Id = dataRecord.Id;
        $scope.RMRequestDetailSource.data.IsRecevied = false;
        $scope.$apply(function () {
            $scope.IsMaterialReceived = false;
            $scope.IsDispatch = true;
            $scope.IsMaterialSlip = true;
            $scope.IsRMReceived = true;
        });
        $scope.GrdRMRequestDetails.editable = true;
        $scope.GrdRMRequestDetails.updatebounddata();
        $scope.RawMaterialWindow.open();
    };


    $scope.onDispatchClick = function (event) {
        var rmDetailsData = [];
        var rows = $scope.GrdRMRequestDetails.getrows();
        for (var i = 0; i < rows.length; i++) {
            var item = rows[i];
            var postItem = {
                Id: item.Id, RMRequestId: item.RMRequestId, FormulationRequestId: item.FormulationRequestId, ItemId: item.ItemId, RequestedQty: item.RequestedQty, IssuedQty: item.IssuedQty
            };
            rmDetailsData.push(postItem);
        }
        var remarks = $scope.RMRemarks;
        $http.post('/FormulationRequest/UpdateRMRequest', { rmDetailsData: rmDetailsData, formulationId: formulationId, rawMaterialId: rawMaterialId, remarks: remarks }).then(function (result) {
            if (result.data.Status == 1) {
                $scope.GrdRMRequest.updatebounddata();
                $scope.GrdRMRecevied.updatebounddata();
                $scope.GrdRMCloseRecevied.updatebounddata();
                $scope.RawMaterialWindow.close();
                rawMaterialId = 0;
                materialRequestId = 0;
                $scope.RMRemarks = "";
            }
            else {
                $scope.GrdFormulationNew.updatebounddata();
                $scope.GrdRMRequest.updatebounddata();
                $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    };

    $scope.RMReceviedrender = function (row, column, value) {
        var dataRecord = $scope.GrdRMRecevied.getrowdata(row);
        if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
            return "RM Received";
        }
        else {
            $scope.GrdRMRecevied.hidecolumn("Recevied");
        }
    }

    $scope.RMRecevied = function (row, event) {
        var dataRecord = $scope.GrdRMRecevied.getrowdata(row);
        $scope.$apply(function () {
            $scope.IsMaterialReceived = true;
            $scope.IsDispatch = false;
            $scope.IsMaterialSlip = true;
            $scope.IsRMReceived = true;
        });
        rawMaterialId = dataRecord.Id;
        materialRequestId = dataRecord.Id;
        requestId = dataRecord.FormulationRequestId;
        $scope.GrdRMRequestDetails.editable = false;
        //$scope.RMRequestDetailSource.data.Id = dataRecord.FormulationRequestId;
        $scope.RMRequestDetailSource.data.Id = dataRecord.Id;
        $scope.RMRequestDetailSource.data.IsRecevied = true;
        $scope.GrdRMRequestDetails.updatebounddata();
        $scope.RMRemarks = "";
        $scope.RawMaterialWindow.open();
    }

    $scope.onReceviedClick = function (event) {
        if (requestId > 0) {
            var remarks = $scope.RMRemarks;
            $http.post('/FormulationRequest/UpdateRMStatus', { formulationId: requestId, rawMaterialId: rawMaterialId, remarks: remarks }).then(function (result) {
                if (result.data.Status == 1) {
                   // $scope.GrdFormulationRecevied.updatebounddata();
                    $scope.GrdRMRequest.updatebounddata();
                    $scope.GrdRMRecevied.updatebounddata();
                    $scope.GrdRMCloseRecevied.updatebounddata();
                    $scope.GrdFormulationNew.updatebounddata();
                    $scope.RawMaterialWindow.close();
                    requestId = 0;
                    rawMaterialId = 0;
                    materialRequestId = 0;
                    $scope.RMRemarks = "";
                }
                else {
                    $scope.GrdFormulationNew.updatebounddata();
                    $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                }
            }, function (result, status, headers, config) {
                alert("status");
            });
        }

    }

    $scope.RMCloseViewrender = function (row, column, value) {
        //var dataRecord = $scope.GrdRMCloseRecevied.getrowdata(row);
        //if (userRole == "Lavel1" || userRole == "Lavel3" || userRole == "Admin") {
            return "View";
       // }
        //else {
          //  $scope.GrdRMCloseRecevied.hidecolumn("View");
        //}
    }

    $scope.RMCloseView = function (row, event) {
        var dataRecord = $scope.GrdRMCloseRecevied.getrowdata(row);
        $scope.$apply(function () {
            $scope.IsMaterialReceived = false;
            $scope.IsDispatch = false;
            $scope.IsMaterialSlip = true;
            $scope.IsRMReceived = false;
        });
        rawMaterialId = dataRecord.Id;
        materialRequestId = dataRecord.Id;
        requestId = dataRecord.FormulationRequestId;
        $scope.GrdRMRequestDetails.editable = false;
        //$scope.RMRequestDetailSource.data.Id = dataRecord.FormulationRequestId;
        $scope.RMRequestDetailSource.data.Id = dataRecord.Id;
        $scope.RMRequestDetailSource.data.IsRecevied = true;
        $scope.GrdRMRequestDetails.updatebounddata();
        $scope.RMRemarks = "";
        $scope.RawMaterialWindow.open();
    }

    $scope.VerifiedCloseFormulationrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.GrdFormulationClose.getrowdata(row);
        if (dataRecord.VerifyBy > 0) {
            return dataRecord.VerifyUser;
        }
        else {
            return "Verify";
        }
    }

    $scope.VerifiedCloseFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationClose.getrowdata(row);
        RequestId = dataRecord.Id;
        if (dataRecord.VerifyBy == null) {
            $scope.VerifyRemarkWindow.open();
        }
        if (dataRecord.VerifyBy > 0) {
            $scope.openConfirm("Confirmation", 'Are sure you want to clear verification details?', 360, 100, function (e) {
                if (e) {
                    $http.post('/FormulationRequest/ClearVerifyFomulationById', { formulationId: dataRecord.Id }).then(function (result) {
                        if (result.data.Status == 1) {
                            $scope.GrdFormulationClose.updatebounddata();
                        }
                    }, function (result, status, headers, config) {
                        alert("status");
                    });
                }
                else {
                    $scope.GrdFormulationClose.updatebounddata();
                }
            });
        }
    }

    $scope.onVerifyRemarksClick = function (event) {
        var Notes = $scope.txtVerifyNotes.val();
        $http.post('/FormulationRequest/VerifyFomulationById', { formulationId: RequestId, Notes: Notes }).then(function (result) {
            if (result.data.Status == 1) {
                $scope.GrdFormulationClose.updatebounddata();
                $scope.VerifyRemarkWindow.close();
                RequestId = 0;
                $scope.txtVerifyNotes.val("");
            }
        }, function (result, status, headers, config) {
            alert("status");
        });
    }

    $scope.ViewCloseFormulationrender = function (row, column, value) {
        return "View";
    }

    $scope.ViewCloseFormulation = function (row, event) {
        var dataRecord = $scope.GrdFormulationClose.getrowdata(row);
        viewFormulationId = dataRecord.Id;
        $scope.IsBatch = true;
        $scope.IsClosed = true;
        $scope.IsTestResult = true;
        GetFormulationById(viewFormulationId);
    }
    var downFormulationId = 0;

    var GetFormulationById = function (viewFormulationId) {
        $scope.model = [];
        $.ajax({
            url: "/FormulationRequest/GetFormulationById",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: viewFormulationId },
            success: function (data) {
                $scope.$apply(function () {
                    
                    if (data.formulation.StatusId == 4 || data.formulation.StatusId == 8 || data.formulation.StatusId == 6 || (data.formulation.StatusId == 2 && data.formulation.QAStatusId == 2)) {
                        $scope.IsTestResult = true;
                        $('#litestResult1').removeClass('active');
                        $('#litestResult2').removeClass('active');
                        $('#tab_TestResult_1').removeClass('active');
                        $('#tab_TestResult_2').removeClass('active');
                        $('#litestResult1').addClass('active');
                        $('#tab_TestResult_1').addClass('active');
                        GetColourQAData(viewFormulationId);
                        GetColorQAVerList(viewFormulationId);
                        $scope.IsDownload = false;
                    }
                    else {
                        $scope.IsTestResult = false;
                        $('#litestResult1').removeClass('active');
                        $('#litestResult2').removeClass('active');
                        $('#litestResult1').addClass('active');
                        $('#tab_TestResult_1').addClass('active');
                        $scope.IsDownload = true;
                    }
                    
                    $scope.enableProperty = true;
                    $scope.IsSaved = false;
                    downFormulationId = data.formulation.Id;
                    $scope.model.Id = data.formulation.Id;
                    $scope.model.LotNo = data.formulation.LotNo;
                    $scope.model.GradeName = data.formulation.GradeName;
                    $scope.model.LOTSize = data.formulation.LOTSize;
                    $scope.model.ColorSTD = data.formulation.ColorSTD;
                    $scope.model.QtyToProduce = data.formulation.QtyToProduce;
                    $scope.model.WorkOrderNo = data.formulation.WorkOrderNo;
                    $scope.model.Notes = data.formulation.Notes;
                    

                    $scope.FormulationRemarksSource.data.FormulationId = data.formulation.Id;
                    $scope.grdFormulationRemarks.updatebounddata();

                    FormulationDetailsData = data.formulationDetail;
                    $scope.FormulationRequestDetailsSource.localdata = $.grep(FormulationDetailsData, function (item, i) {
                        return item;
                    });
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        
        $scope.FormulationViewWindow.open();
    }

    $scope.OnDownloadClick = function () {
        window.location.href = "/Download/FormulationRequest/?Id=" + downFormulationId;
    }
    $scope.OnBatchDownloadClick = function () {
        window.location.href = "/Download/BatchYieldReport/?Id=" + downFormulationId;
    }

    $scope.OnColorQADownloadClick = function (event) {
        var VersionNo = event.testResult.VersionNo;
        window.location.href = "/Download/FormulationRequest/?Id=" + downFormulationId + "&VerNo=" + VersionNo;
    }

    $scope.OnTestResultDownloadClick = function (event) {
        var VersionNo = event.testResult.VersionNo;
        window.location.href = "/Download/ColourQASepcsReport/?Id=" + downFormulationId + "&VerNo=" + VersionNo;
    }

    $scope.OnMaterialIssuedReturnDownloadClick = function () {
        window.location.href = "/Download/MaterialIssuedReturnReport/?Id=" + downFormulationId;
    }

    $scope.onMaterialIssueDownloadClick = function () {
        window.location.href = "/Download/MaterialIssueSlipReport/?RMRequestId=" + materialRequestId;
    }
    

    $scope.onMonthYearSearch = function () {
        var year = $scope.ddlYear.val();
        var month = $scope.ddlMonth.val();
        $scope.FormulationCloseSource.data.StatusId = 6;
        $scope.FormulationCloseSource.data.Month = month;
        $scope.FormulationCloseSource.data.Year = year;
        $scope.GrdFormulationClose.updatebounddata();
    }

    $scope.onRMMonthYearSearch = function () {
        var year = $scope.ddlRMYear.val();
        var month = $scope.ddlRMMonth.val();
        var lotNo = $scope.txtRMLotNo.val();
        $scope.RMCloseReceviedSource.data.StatusId = 3;
        $scope.RMCloseReceviedSource.data.Month = month;
        $scope.RMCloseReceviedSource.data.Year = year;
        $scope.RMCloseReceviedSource.data.LotNo = lotNo;
        $scope.GrdRMCloseRecevied.updatebounddata();
    }

    $scope.RMIssuedQtycellrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        var datarecord = $scope.GrdRMRequestDetails.getrowdata(row);
        if (datarecord.RequestedQty > datarecord.IssuedQty) {
            return '<span style="margin: 4px; float: ' + columnproperties.cellsalign + '; color: #FF0000;">' + value + '</span>';
        }
        else if (datarecord.RequestedQty < datarecord.IssuedQty) {
            return '<span style="margin: 4px; float: ' + columnproperties.cellsalign + '; color: #008000;">' + value + '</span>';
        }
    }

    var GetColourQAData = function (viewFormulationId) {
        ClearData();
        //$.ajax({
        //    url: "/FormulationRequest/GetColourQaSpecByFormulationId",
        //    type: "GET",
        //    dataType: "json",
        //    cache: false,
        //    data: { FormulationId: viewFormulationId },
        //    success: function (data) {
        //        $scope.$apply(function () {
        //            //$scope.txtViewDeltaE.val(data.DeltaE);
        //            //$scope.txtViewDeltaL.val(data.DeltaL);
        //            //$scope.txtViewDeltaa.val(data.Deltaa);
        //            //$scope.txtViewDeltab.val(data.Deltab);
        //            //$scope.txtViewMFI220C10kg.val(data.MFI220c10kg);
        //            //$scope.txtViewSPGravity.val(data.SPGravity);
        //            //$scope.txtViewAshContent.val(data.AshContent);
        //            //$scope.txtViewNotchImpact.val(data.NotchImpact);
        //            //$scope.txtViewTensile.val(data.Tensile);
        //            //$scope.txtViewFlexuralmodulus.val(data.FlexuralModule);
        //            //$scope.txtViewFlexuralStrength.val(data.FlexuralStrength);
        //            //$scope.txtViewElongation.val(data.Elongation);
        //            //$scope.txtViewFlammablity.val(data.Flammability);
        //            //$scope.txtViewGWTat.val(data.GWTAt);
        //           // $scope.txtViewCloseNotes.val(data.GWTAt);
        //        });
        //    },
        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //        alert('oops, something bad happened');
        //    }
        //});
    }

    var GetColorQAVerList = function (viewFormulationId) {
        $scope.TestResultList = 0;
        $.ajax({
            url: "/FormulationRequest/GetColourQaSpecByFormulationId",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { FormulationId: viewFormulationId },
            success: function (data) {
                $scope.$apply(function () {
                    $scope.TestResultList = data;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    var ClearData = function () {
        if ((typeof ($scope.model) != 'undefined') && $scope != null) {
            $scope.txtFGDeposition.val("");
            $scope.txtNSP.val("");
            $scope.txtStartUpSCOffSpecs.val("");
            $scope.txtQCRejected.val("");
            $scope.txtMixMaterial.val("");
            $scope.txtLumps.val("");
            $scope.txtLongsandFines.val("");
            $scope.txtLabSample.val("");
            $scope.txtSweepaged.val("");
            $scope.txtAdditives.val("");
            $scope.txtPackingBags.val("");
            $scope.CloseNotes = "";
            $scope.txtDeltaE.val("");
            $scope.txtDeltaL.val("");
            $scope.txtDeltaa.val("");
            $scope.txtDeltab.val("");
            $scope.txtMFI220C10kg.val("");
            $scope.txtSPGravity.val("");
            $scope.txtAshcontent.val("");
            $scope.txtNotchimpact.val("");
            $scope.txtTensile.val("");
            $scope.txtFlexuralmodulus.val("");
            $scope.txtFlexuralStrength.val("");
            $scope.txtElongation.val("");
            $scope.txtFlammablity.val("");
            $scope.txtGWTat.val("");

            //$scope.txtViewDeltaE.val("");
            //$scope.txtViewDeltaE.val("");
            //$scope.txtViewDeltaL.val("");
            //$scope.txtViewDeltaa.val("");
            //$scope.txtViewDeltab.val("");
            //$scope.txtViewMFI220C10kg.val("");
            //$scope.txtViewSPGravity.val("");
            //$scope.txtViewAshContent.val("");
            //$scope.txtViewNotchImpact.val("");
            //$scope.txtViewTensile.val("");
            //$scope.txtViewFlexuralmodulus.val("");
            //$scope.txtViewFlexuralStrength.val("");
            //$scope.txtViewElongation.val("");
            //$scope.txtViewFlammablity.val("");
            //$scope.txtViewGWTat.val("");
            IsFormulationchanged = false;
            $scope.FormulationCloseValidator.hide();
        }
    }

    $.ajax({
        url: "/Home/GetVersionStatus",
        type: "GET",
        cache: false,
        dataType: "json",
        success: function (data) {
            $scope.IsVersionUpdated = data;
            if (typeof ($scope.WinVersionUpdated) != 'undefined')
                if (data)
                    $scope.WinVersionUpdated.open();
                else
                    $scope.WinVersionUpdated.close();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('oops, something bad happened');
        }
    });

    $scope.OnVersionUpdateWinClose = function (event) {
        if ($scope.chkNextTime.checked)
            $http.post('/Home/UpdateVersionStatus');
    };

    $scope.OnOkClick = function (event) {
        if ($scope.chkNextTime.checked) {
            $http.post('/Home/UpdateVersionStatus').then(function (response) {
                //First function handles success
                $scope.WinVersionUpdated.close();
            }, function (response) {
                //Second function handles error
            });
        }
        else
            $scope.WinVersionUpdated.close();
    };

});