ProApp.controller("FormulationRequestController", function ($scope, $http, $timeout) {

    var formulationId = 0;
    var detailId = 0;
    var formulaDetailDeletedIds = [];
    var FormulationDetailsData = [];
    var qtytoProduce = 0;
    $scope.model = [];
    var QAManager = false;
    $scope.enableProperty = false;
    $scope.IsSaved = false;
    $scope.IsAdd = false;
    $scope.IsBatch = false;
    var IsChangeResult = false;
    var changeComment = "";

    var userRole = $('#hdnRole').val();

    $scope.FormulationRules = [
        { input: '#txtLotNo', message: 'Lot no is required!', action: 'blur', rule: 'required' },
        { input: '#txtGradeName', message: 'Grade name is required!', action: 'blur', rule: 'required' }
    ];

    $scope.FormulationDetailsRules = [
        //{
        //    input: '#ddlWinMachine', message: 'Please choose Machine', action: 'change',
        //    rule: function (input, commit) {
        //        var index = $scope.ddlWinMachine.getSelectedIndex();
        //        return index != -1;
        //    }
        //},
        {
            input: '#ddlWinItem', message: 'Please choose Item', action: 'change',
            rule: function (input, commit) {
                var index = $scope.ddlWinItem.getSelectedIndex();
                return index != -1;
            }
        }
    ];

    $scope.commentRules = [
       { input: '#txtChangeRemarks', message: 'Comment is required!', action: 'blur', rule: 'required' }
    ];


    $scope.onAddClick = function () {
        ClearData();
        $scope.IsDownload = false;
        $scope.IsSaved = true;
        QAManager = true;
        $scope.enableProperty = false;
        $scope.FormulationRequestDetailsSource = {};
        $scope.ddlWinStatus.SelectedIndex = 1;
        $scope.WinAddOrUpdateFormula.open();
    };

    $scope.onSearch = function () {
        var lotno = $scope.SearchLotNo;
        var searchStatus = 0;
        if ($scope.ddlStatus.getSelectedItem() != null) {
            var searchStatus = $scope.ddlStatus.getSelectedItem().value;
        }
        $scope.FormulationRequestSource.data = { LotNo: lotno, StatusId: searchStatus };
    };



    $scope.FormulationRequestSource = {
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
           { name: 'Status', type: 'string' },
           { name: 'StatusId', type: 'int' },
           { name: 'CreateDate', type: 'date' }
        ],
        url: '/FormulationRequest/GetFormulationGridData',
        data: { LotNo: '', StatusId: 0 }
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

    $scope.StatusSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'StatusName', type: 'string' }
        ],
        url: '/FormulationRequest/GetRequestStatusList',
    };
    $scope.WinStatusSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'StatusName', type: 'string' }
        ],
        url: '/FormulationRequest/GetRequestStatusList',
    };
    //$scope.WinMachineSource = {
    //    datatype: "json",
    //    datafields: [
    //       { name: 'Id', type: 'int' },
    //       { name: 'Name', type: 'string' }
    //    ],
    //    url: '/Machine/GetMachineListForFormula',
    //};
    $scope.WinItemSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/RMItem/GetItemListForFormula',
    };

    $scope.WinQtyUOMSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Measurement', type: 'string' }
        ],
        url: '/Item/GetMOUList',
    };

    $scope.WinItemUOMSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Measurement', type: 'string' }
        ],
        url: '/Item/GetMOUList',
    };


    $scope.onWinItemChange = function (e) {
        var itemId = $scope.ddlWinItem.getSelectedItem().value
        $.ajax({
            url: "/Item/GetItemUOMByItemId",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { itemId: itemId },
            success: function (data) {
                $scope.$apply(function () {
                    //$scope.ddlWinUOM.val(data[0].UnitOfMeasure);
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.onSaveFormulationDetails = function () {
        var IsValidate = $scope.FormulationDetailsValidator.validate();
        if (!IsValidate)
            return;
        $scope.model.FormulationRequestsDetails.FormulationRequestId = formulationId;
        var Detailsdata = $scope.model.FormulationRequestsDetails;
        var minId = Math.min.apply(Math, FormulationDetailsData.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;

        if (detailId != 0) {
            for (var j = 0; j < FormulationDetailsData.length; j++) {
                if (FormulationDetailsData[j].Id == detailId) {
                    //FormulationDetailsData[j].MachineId = $scope.ddlWinMachine.getSelectedItem().value;
                    FormulationDetailsData[j].ItemId = $scope.ddlWinItem.getSelectedItem().value;
                    FormulationDetailsData[j].ItemQtyGram = Detailsdata.ItemQtyGram;
                    //FormulationDetailsData[j].UOM = $scope.ddlWinUOM.getSelectedItem().value;
                    //FormulationDetailsData[j].MachineName = $scope.ddlWinMachine.getSelectedItem().label;
                    FormulationDetailsData[j].ItemName = $scope.ddlWinItem.getSelectedItem().label;
                    //FormulationDetailsData[j].DetailUOM = $scope.ddlWinUOM.getSelectedItem().label;
                }
            }
        }
        else {
            //$scope.model.FormulationRequestsDetails.MachineId = $scope.ddlWinMachine.getSelectedItem().value;
            $scope.model.FormulationRequestsDetails.ItemId = $scope.ddlWinItem.getSelectedItem().value;
            //$scope.model.FormulationRequestsDetails.UOM = $scope.ddlWinUOM.getSelectedItem().value;
            var iteminPerce = $scope.txtItemQty.val();
            var iteminGram = (iteminPerce * qtytoProduce) / 100;
            FormulationDetailsData.push({
                Id: minId - 1, FormulationRequestId: formulationId, MachineId: 0,
                ItemId: $scope.model.FormulationRequestsDetails.ItemId, ItemQtyPercentage: $scope.txtItemQty.val(),
                ItemQtyGram : iteminGram,
                ItemName: $scope.ddlWinItem.getSelectedItem().label
            });//, DetailUOM: $scope.ddlWinUOM.getSelectedItem().label
        }
        $scope.FormulationRequestDetailsSource.localdata = FormulationDetailsData;
        $scope.GrdFormulationDetailsRequest.updatebounddata();
        $scope.WinAddOrUpdateFormulationDetails.close();
    };

    $scope.OnSaveClick = function () {
        var IsValidate = $scope.FormulationValidator.validate();
        if (!IsValidate)
            return;
        if (IsChangeResult) {
            var detailsData = $scope.GrdFormulationDetailsRequest.getrows();
            // $scope.model.FormulationRequests.FormulationRequestsDetails = FormulationDetailsData
            $scope.model.FormulationRequests.FormulationRequestsDetails = detailsData
            $http.post('/FormulationRequest/SaveFormulationData', { formulationData: $scope.model.FormulationRequests, formulaDetailDeletedIds: formulaDetailDeletedIds, Comment: changeComment }).then(function (result) {
                if (result.data.Status == 1) {
                    $scope.GrdFormulationRequest.updatebounddata();
                    $scope.openMessageBox('Success', result.data.Message, 300, 90);
                    FormulationDetailsData = [];
                    formulaDetailDeletedIds = [];
                    IsChangeResult = false;
                    $scope.WinAddOrUpdateFormula.close();
                }
                else {
                    $scope.GrdFormulationRequest.updatebounddata();
                    $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                }
            }, function (result, status, headers, config) {
                alert("status");
            });
        }
        else {
            $scope.txtChangeRemarks.val("");
            $scope.WinComment.open();
        }
      
    };

    $scope.onSaveChangeReason = function (event) {
        var IsValidate = $scope.commentValidator.validate();
        if (!IsValidate)
            return;

        changeComment = $scope.txtChangeRemarks.val();
        IsChangeResult = true;
        $scope.WinComment.close();
    }


    $scope.model.FormulationRequests = [];
    $scope.UpdateFormulation = function (row, column, value) {
        return "<center><a ng-click='EditFormulation(" + row + ", event)' class='fa fa-edit fa-2' href='javascript:;'></a></center>";
    };
    var FId = 0;
    var VersionNo = 0;
    $scope.EditFormulation = function (row, event) {
        VersionNo = 0;
        $scope.FormulationRequestDetailsSource = {};
        var dataRecord = $scope.GrdFormulationRequest.getrowdata(row);
        $.ajax({
            url: "/FormulationRequest/GetFormulationById",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                $scope.$apply(function () {
                    userRole = data.formulation.UserRole;
                    if ((userRole == "QAManager" || userRole == "QA" || userRole == "Admin") && data.formulation.StatusId == 1) {
                        $scope.enableProperty = false;
                        $scope.IsSaved = true;
                        $scope.IsAdd = true;
                        $scope.GrdFormulationDetailsRequest.editable = true;
                        $scope.GrdFormulationDetailsRequest.showcolumn("Delete");
                    }
                    else if ((userRole == "QAManager" || userRole == "Admin") && data.formulation.StatusId >= 1 && data.formulation.StatusId <=5 ) {
                        $scope.enableProperty = true;
                        $scope.IsSaved = true;
                        $scope.IsAdd = false;
                        $scope.GrdFormulationDetailsRequest.editable = true;
                        $scope.GrdFormulationDetailsRequest.hidecolumn("Delete");
                    }
                    else if ((userRole == "QAManager" || userRole == "QA" || userRole == "Admin") && data.formulation.StatusId == 6) {
                        $scope.enableProperty = true;
                        $scope.IsSaved = false;
                        $scope.IsAdd = false;
                        $scope.GrdFormulationDetailsRequest.editable = false;
                        $scope.GrdFormulationDetailsRequest.hidecolumn("Delete");
                    }
                    else if ((userRole == "Lavel1" || userRole == "Lavel3") && data.formulation.StatusId > 1) {
                        $scope.enableProperty = true;
                        $scope.IsSaved = false;
                        $scope.IsAdd = false;
                        $scope.GrdFormulationDetailsRequest.editable = false;
                        $scope.GrdFormulationDetailsRequest.hidecolumn("Delete");
                    }
                    else {
                        $scope.IsSaved = false;
                        $scope.IsAdd = false;
                        $scope.GrdFormulationDetailsRequest.editable = false;
                        $scope.GrdFormulationDetailsRequest.hidecolumn("Delete");
                        $scope.enableProperty = true;
                    }

                    if (data.formulation.StatusId == 6) {
                        $scope.IsBatch = true;
                    }
                    else
                        $scope.IsBatch = false;

                    FId = data.formulation.Id;
                    VersionNo = data.formulation.VerNo;
                    $scope.model.FormulationRequests = data.formulation;
                    $scope.model.FormulationRequests.Id = data.formulation.Id;
                    $scope.model.FormulationRequests.LotNo = data.formulation.LotNo;
                    $scope.model.FormulationRequests.GradeName = data.formulation.GradeName;
                    $scope.model.FormulationRequests.LOTSize = data.formulation.LOTSize;
                    $scope.model.FormulationRequests.ColorSTD = data.formulation.ColorSTD;
                    $scope.model.FormulationRequests.QtyToProduce = data.formulation.QtyToProduce;
                    $scope.model.FormulationRequests.WorkOrderNo = data.formulation.WorkOrderNo;
                    //$scope.ddlWinQtyUOM.val(data.formulation.UOM);
                    //$scope.ddlWinStatus.val(data.formulation.StatusId);
                    qtytoProduce = data.formulation.QtyToProduce;
                    $scope.model.FormulationRequests.Notes = data.formulation.Notes;
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
        $scope.IsSaved = true;
        $scope.IsDownload = true;
        $scope.txtItemQty.val("");
        $scope.WinAddOrUpdateFormula.open();
    };

    $scope.DeleteFormulation = function (row, column, value) {
        return "<center><a ng-click='DeleteFormula(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></center>";
    };
    $scope.DeleteFormula = function (row, evet) {
        var dataRecord = $scope.GrdFormulationRequest.getrowdata(row);
        if (dataRecord.StatusId == 1) {
            $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Formula request?', 350, 100, function (e) {
                if (e) {
                    $http.post('/FormulationRequest/DeleteFormula', { Id: dataRecord.Id }).then(function (result) {
                        if (result.data.Message == "Success") {
                            $scope.GrdFormulationRequest.updatebounddata();
                            $scope.openMessageBox('Success', 'Formulation request deleted', 300, 90);
                        }
                        else {
                            $scope.openMessageBox('Warning', result.data.Message, 300, 90);
                        }
                    }, function (result, status, headers, config) {
                        alert("status");
                        $scope.GrdFormulationRequest.updatebounddata();
                    });
                }
            });
        }
        else {
            $scope.openMessageBox("Warning", 'Formula request status not new?', 350, 100);
        }
    };

    $scope.ViewFormulation = function (row, column, value) {
        var dataRecord = $scope.GrdFormulationRequest.getrowdata(row);
        return "<center><a style='margin: 4px;text-decoration:underline;' href='/FormulationRequest/Create/" + dataRecord.Id + "' ><i class='fa fa-eye'></i> </a></center>";
    };

    $scope.model.FormulationRequestsDetails = [];
    //$scope.UpdateFormulationDetails = function (row, column, value) {
    //    return "Edit";
    //}

    //$scope.EditFormulaDetails = function (row) {
    //    var dataRecord = $scope.GrdFormulationDetailsRequest.getrowdata(row);
    //    detailId = dataRecord.Id;
    //    var formulaid = dataRecord.Id;
    //    for (var i = 0; i < FormulationDetailsData.length; i++) {
    //        if (formulaid == FormulationDetailsData[i].Id) {
    //            //$scope.ddlWinMachine.val(FormulationDetailsData[i].MachineId);
    //            $scope.ddlWinItem.val(FormulationDetailsData[i].ItemId);
    //            //$scope.ddlWinUOM.val(FormulationDetailsData[i].UOM);
    //            $scope.model.FormulationRequestsDetails.ItemQtyGram = parseInt(FormulationDetailsData[i].ItemQtyGram);
    //            $scope.GrdFormulationDetailsRequest.updatebounddata();
    //        }
    //    }
    //    $scope.WinAddOrUpdateFormulationDetails.open();
    //};

    $scope.DeleteFormulationDetails = function (row, column, value) {
        return "Delete";
    }

    $scope.DeleteFormulaDetails = function (row) {
        var dataRecord = $scope.GrdFormulationDetailsRequest.getrowdata(row);
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Formulaltion Item?', 350, 100, function (e) {
            if (e) {
                formulaDetailDeletedIds.push(dataRecord.Id);
                FormulationDetailsData = $.grep(FormulationDetailsData, function (item, i) {
                    return item.Id != dataRecord.Id;
                });
                $scope.FormulationRequestDetailsSource.localdata = $.grep(FormulationDetailsData, function (item, i) {
                    return item;
                });
                $scope.GrdFormulationDetailsRequest.updatebounddata();
            }
        });
        $scope.GrdFormulationDetailsRequest.updatebounddata();
    }

    $scope.ItemQtyPercentagechange = function (rowindex, datafield, columntype, oldvalue, newvalue) {
        if (newvalue == "") {
            return oldvalue;
        }
        var data = $scope.GrdFormulationDetailsRequest.getrowdata(rowindex);
        if (typeof (data.ItemQtyPercentage) != 'undefined' && data.ItemQtyPercentage != null) {
            data.ItemQtyGram = (newvalue * qtytoProduce) / 100;
        }

    };

    $scope.ItemInGramrender = function (row, columnfield, value, defaulthtml, columnproperties) {
        var data = $scope.GrdFormulationDetailsRequest.getrowdata(row);
        if (value != null || value != 'undefined') {
            return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + ';">' + value + '</span>';
        }
    };

    $scope.percentageValidation = function (cell, value) {
        if (value > 0 && value < 100) {
            return true;
        }
        else {
            return { result: false, message: "Percenatge can not be text." };
        }
    }

    $scope.onQtytoProduceChange = function () {
        if (FId > 0) {
            qtytoProduce = $scope.model.FormulationRequests.QtyToProduce;
            var data = $scope.GrdFormulationDetailsRequest.getrows();
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                if (typeof (item.ItemQtyPercentage) != 'undefined' && item.ItemQtyPercentage != null) {
                    item.ItemQtyGram = (item.ItemQtyPercentage * qtytoProduce) / 100;
                }
                FormulationDetailsData = data;
                $scope.FormulationRequestDetailsSource.localdata = $.grep(FormulationDetailsData, function (item, i) {
                    return item;
                });
                $scope.GrdFormulationDetailsRequest.updatebounddata();
            }

        }
    }

    $scope.OnDownloadClick = function () {
        window.location.href = "/Download/FormulationRequest/?Id=" + FId + "&VerNo=" + VersionNo;
    };

    $scope.OnDownloadBatchYieldClick = function () {
        window.location.href = "/Download/BatchYieldReport/?Id=" + FId;
    };



    $scope.AddNewItem = function () {
        $scope.WinAddOrUpdateFormulationDetails.open();
    };

    $scope.onRefresh = function () {
        $scope.GrdFormulationRequest.updatebounddata();
    }



    var ClearData = function (event) {
        if ((typeof ($scope.model) != 'undefined') && $scope.model != null) {
            $scope.model.FormulationRequests = {};
            $scope.model.FormulationRequests.QtyToProduce = 0;
            $scope.model.FormulationRequests.LotNo = null;
            $scope.model.FormulationRequests.GradeName = null;
            $scope.model.FormulationRequests.LOTSize = null;
            $scope.model.FormulationRequests.ColorSTD = null;
            $scope.enableProperty = false;
            //$scope.ddlWinQtyUOM.clearSelection();
            //$scope.ddlWinStatus.clearSelection();
            FormulationDetailsData = [];
            formulaDetailDeletedIds = [];
            formulationId = 0;
            detailId = 0;
            $scope.IsDownload = false;
            $scope.IsSaved = true;
            $scope.FormulationRequestDetailsSource = {};
            $scope.FormulationValidator.hide();
            FormulationDetailsClearData();
            FId = 0;
            QAManager = false;
            qtytoProduce = 0;
        }
    }



    var FormulationDetailsClearData = function (event) {
        if ((typeof ($scope.model.FormulationRequestsDetails) != 'undefined') && $scope.model.FormulationRequestsDetails != null) {
            $scope.ddlWinMachine.clearSelection();
            $scope.ddlWinItem.clearSelection();
            $scope.model.FormulationRequestsDetails.ItemQty = 0;
            //$scope.ddlWinUOM.clearSelection();
            detailId = 0;
            $scope.FormulationDetailsValidator.hide();
        }
    };

});