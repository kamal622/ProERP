ProApp.controller("IndentController", function ($parse, $scope, $http) {

    $scope.IsUpdate = false;
    $scope.IsIssue = true;
    $scope.IsPo = true;
    $scope.IsIndentPoDetails = true;
    $scope.IsApprovedBy = true;
    $scope.IsApprovedDate = true;
    $scope.IsRejectedBy = true;
    $scope.IsRejectedDate = true;
    $scope.IsPR = true;
    $scope.IsJR = true;
    $scope.IsDisabled = true;
    $scope.ShowAttachment = false;
    //$scope.model.Indent = { BudgetType: 'CapEx' };    

    $scope.model = {
        Indent: { Id: 0 },
        Item: {},
        IndentDetail: {},
    };
    var IndentDetailId = 0;

    $scope.model.IndentDetail.RequiredByDate = new Date(); //.toISOString();

    $scope.ItemNameSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Indent/GetItemNameList',
    };
    $scope.CurrencySource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Currency', type: 'string' }
        ],
        url: '/Indent/GetCurrencyList',
    };
    $scope.PlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
    };

    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0 }
    };

    $scope.MachineSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Machine/GetMachinesForLine',
        data: { LineId: 0 }
    };

    $scope.PrioritySource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetPriorityList'
    };

    $scope.UOMSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'UnitOfMeasure', type: 'string' }
        ],
        url: '/Indent/GetUOMList'
    };

    $scope.VendorSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
        ],
        url: '/Indent/GetVendorList',
    };

    $scope.StatusSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/Indent/GetStatusList'
    };

    $scope.IndentStatusSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/Indent/GetIndentStatusList'
    };

    $scope.BudgetCodeSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'BudgetCode', type: 'string' }
        ],
        url: '/Indent/GetBudgetCodeList',
        data: { BudgetType: '' }
    };

    $scope.ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes();
    };

    $scope.onPlantChange = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.LineSource.data = { PlantId: selectedId };
    }
    $scope.onPlantBindingComplete = function (e) {
        $scope.ddlPlant.val($scope.model.IndentDetail.PlantId)
    }

    $scope.onLineChange = function (e) {
        $scope.MachineSource.data = { LineId: $scope.ddlLine.val() };
    }
    $scope.onLineBindingComplete = function (e) {
        $scope.ddlLine.val($scope.model.IndentDetail.LineId);
    }

    $scope.onBindingMachine = function (e) {
        $scope.MachineInstance.val($scope.model.IndentDetail.MachineId);
    }

    $scope.onItemNameChange = function (event) {
        $scope.ddlVendor.clearSelection();
        var itemId = parseInt($scope.ddlItemName.val());
        if (isNaN(itemId))
            itemId = 0;

        $.ajax({
            url: "/Indent/GetItemById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Id: itemId },
            success: function (data) {
                $scope.$apply(function () {

                    $scope.model.Item = data;
                    //$scope.model.IndentDetail.ItemId = data.Id;
                    $scope.ddlUOM.val($scope.model.Item.UnitOfMeasure);
                    $scope.ddlVendor.val(data.VendorId);
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    };
    $scope.onBindingItemName = function (e) {
        // $scope.ddlItemName.val($scope.model.Name);
    }

    $scope.onBindingPriority = function (event) {
        if (typeof ($scope.model.Indent.Priority) != 'undefined')
            $scope.PriorityInstance.val($scope.model.Indent.Priority);
        else
            $scope.model.Indent.Priority = 1
    }

    $scope.onBindingVendor = function (e) {
        $scope.ddlVendor.val($scope.model.IndentDetail.PreferredVendorId);
    }

    $scope.onStatusChange = function (e) {
        var item = $scope.ddlStatus.getSelectedItem();
        var status = item.label;
        if (status == "New") {
            $scope.IsPo = true;
            $scope.IsIssue = true;
            $scope.IsSave = false;
            $scope.IsCancel = false;
            $scope.IsApprovedBy = true;
            $scope.IsApprovedDate = true;
            $scope.IsRejectedBy = true;
            $scope.IsRejectedDate = true;
        }
        if (status == "Approved") {
            $scope.IsPo = true;
            $scope.IsIssue = true;
            $scope.IsSave = false;
            $scope.IsCancel = false;
            $scope.IsApprovedBy = true;
            $scope.IsApprovedDate = true;
            $scope.IsRejectedBy = true;
            $scope.IsRejectedDate = true;
        }
        if (status == "Rejected") {
            $scope.IsPo = true;
            $scope.IsIssue = true;
            $scope.IsSave = false;
            $scope.IsCancel = false;
            $scope.IsApprovedBy = true;
            $scope.IsApprovedDate = true;
            $scope.IsRejectedBy = true;
            $scope.IsRejectedDate = true;
        }
        if (status == "PO") {
            $scope.IsPo = false;
            $scope.IsIssue = true;
            $scope.IsSave = false;
            $scope.IsCancel = false;
            $scope.IsApprovedBy = true;
            $scope.IsApprovedDate = true;
            $scope.IsRejectedBy = true;
            $scope.IsRejectedDate = true;
        }
        if (status == "Received") {
            $scope.IsIssue = false;
            $scope.IsPo = true;
            $scope.IsSave = false;
            $scope.IsCancel = false;
            $scope.IsApprovedBy = true;
            $scope.IsApprovedDate = true;
            $scope.IsRejectedBy = true;
            $scope.IsRejectedDate = true;
        }
    }
    $scope.onBindingStatus = function (e) {
        if (typeof ($scope.model.IndentDetail.StatusId) != 'undefined')
            $scope.ddlStatus.val($scope.model.IndentDetail.StatusId);
        else
            $scope.model.IndentDetail.StatusId = parseInt($scope.ddlStatus.val());
    }

    $scope.onIndentStatusChange = function (e) {
        var item = $scope.ddlIndentStatus.getSelectedItem();
        var status = item.label;
        if (status == "Approved") {
            $scope.IsIndentPoDetails = true;
        }
        if (status == "Rejected") {
            $scope.IsIndentPoDetails = true;
        }
        if (status == "PO") {
            $scope.IsIndentPoDetails = false;
        }
    }
    $scope.onIndentBindingStatus = function (e) {
        if (typeof ($scope.model.Indent.StatusId) != 'undefined')
            $scope.ddlIndentStatus.val($scope.model.Indent.StatusId);
        else
            $scope.model.Indent.StatusId = parseInt($scope.ddlIndentStatus.val());
    }

    $scope.onBudgetTypeChange = function (event) {
        $scope.BudgetCodeSource.data = { BudgetType: $scope.ddlBudgetType.getSelectedItem().label };

        if (event.args.item.label == 'CapEx')
            $scope.BudgetCodeTitle = 'Budget Code';
        else
            $scope.BudgetCodeTitle = 'Budget Head';

    }

    $scope.onBindingBudgetType = function (event) {
        if (typeof ($scope.model.Indent.BudgetType) != 'undefined')
            $scope.ddlBudgetType.val($scope.model.Indent.BudgetType);
        else {
            //$scope.model.Indent.BudgetType = 'CapEx';
            $scope.ddlBudgetType.selectedIndex = 0;
        }

        var budgetType = $scope.ddlBudgetType.getSelectedItem().label;
        if (budgetType == 'CapEx')
            $scope.BudgetCodeTitle = 'Budget Code';
        else
            $scope.BudgetCodeTitle = 'Budget Head';

        $scope.BudgetCodeSource.data = { BudgetType: budgetType };
    }
    $scope.onBindingBudgetCode = function (event) {
        $scope.ddlBudgetCode.val($scope.model.Indent.BudgetId);
    }

    $scope.onBindingBudgetHead = function (event) {
        if (typeof ($scope.model.Indent.BudgetHead) != 'undefined')
            $scope.ddlBudgetHead.val($scope.model.Indent.BudgetHead);
        else {
            $scope.ddlBudgetHead.selectedIndex = 0;
        }
    }

    $scope.onRequisitiChange = function (event) {

    }
    $scope.onBindingRequisition = function (event) {
        if (typeof ($scope.model.Indent.RequisitionType) != 'undefined')
            $scope.ddlRequisition.val($scope.model.Indent.RequisitionType);
        else {
            $scope.model.Indent.RequisitionType = 'PR';
        }
    }

    $scope.onBudgetCodeChange = function (event) {
        var budgetId = $scope.ddlBudgetCode.val();

        $.ajax({
            url: "/Indent/GetRemainingBudgetData",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Id: budgetId },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.$apply(function (e) {
                        $scope.RemainingBudget = response.Data
                    });
                }
                else
                    $scope.openMessageBox('Error', response.Message, 600, 400);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.OnGridReady = function () {
        //var item = $scope.ddlRequisition.getSelectedItem();
        //var status = item.label;
        //if (status == "PR") {
        //    $scope.GrdItem.showcolumn('ItemCode');
        //    $scope.GrdItem.showcolumn('Name');
        //    $scope.GrdItem.hidecolumn('JobDescription');
        //} else if (status == "JR") {
        //    $scope.GrdItem.hidecolumn('ItemCode');
        //    $scope.GrdItem.hidecolumn('Name');
        //    $scope.GrdItem.showcolumn('JobDescription');
        //}

        var item = $scope.ddlRequisition.getSelectedItem();
        var status = item.label;
        if (status == "PR") {
            $scope.IsPR = false;
            $scope.IsJR = true;
        }
        else if (status == "JR") {
            $scope.IsPR = true;
            $scope.IsJR = false;
        }
    }

    $scope.onGenerateClick = function (e) {
        var Indent = $scope.model.Indent;
        Indent.BudgetHead = 'Mechanical';
        Indent.PoAmount = 0;
        Indent.IndentBudget = {
            BudgetType: $scope.ddlBudgetType.getSelectedItem().label
        };
        //Ajax call to save
        $http.post('/Indent/SaveGenerateIndent', { Indent: Indent }).success(function (retData) {

            if (retData.Message == "Success") {

                $scope.IsDisabled = false;
                $scope.IsUpdate = true;
                $scope.IsIssue = true;
                $scope.IsPo = true;
                var item = $scope.ddlRequisition.getSelectedItem();
                var status = item.label;
                $('#txtId').val(retData.Data.IndentId);
                $scope.model.IndentDetail.IndentId = retData.Data.IndentId;
                $scope.model.Indent.IndentNo = retData.Data.IndentNo
                $('#lnkDownload').attr('href', '/Download/Indent/' + retData.Data.IndentId);

                //if (status == "PR")
                //    $('#lnkDownload').attr('href', '/Extras/Download.aspx?RequestCode=DownloadIndent&Type=PR&IndentId=' + retData.Data.IndentId);
                //else
                //    $('#lnkDownload').attr('href', '/Extras/Download.aspx?RequestCode=DownloadIndent&Type=JR&IndentId=' + retData.Data.IndentId);

            } else {
                $scope.openMessageBox('Error', retData.Message, 500, 400);
            }

        }).error(function (retData, status, headers, config) {
            alert("status");
        });
    };

    $scope.onAddClick = function (e) {
        $scope.model.IndentDetail.IndentId = parseInt($('#txtId').val());
        var IndentDetail = $scope.model.IndentDetail;
        var item = $scope.ddlStatus.getSelectedItem();
        var status = item.label;
        IndentDetail.RequiredByDate = $scope.dtRequiredBy.getDate().toISOString();
        IndentDetail.Currency = $scope.ddlCurrency.getSelectedItem().value;
        IndentDetail.ExchangeRate = $scope.txtExchangeRate.getDecimal();

        if (IndentDetail.PoAmount == 0 || IndentDetail.PoAmount == null)
            IndentDetail.PoAmount = 0;

        var item1 = $scope.ddlRequisition.getSelectedItem();
        var RequisitionType = item1.label;
        if (RequisitionType == "PR" && (IndentDetail.ItemId == null || IndentDetail.ItemId == 0)) {
            $scope.openMessageBox('Warning', 'Please choose item.', 200, 90);
        }
        else if (RequisitionType == "JR" && (IndentDetail.JobDescription == "" || typeof (IndentDetail.JobDescription) == 'undefined')) {
            $scope.openMessageBox('Warning', 'Please insert job description.', 200, 90);
        }
        else if (IndentDetail.EstimatePrice == null || IndentDetail.EstimatePrice == 0)
            $scope.openMessageBox('Warning', 'Estimate price must be greater than zero(0).', 350, 90);
            //else if (IndentDetail.PlantId == null || IndentDetail.PlantId == 0) {
            //    $scope.openMessageBox('Warning', 'Please choose plant.', 200, 90);
            //}

        else if (IndentDetail.QtyNeeded == null) {
            $scope.openMessageBox('Warning', 'Requested quantity must be greater than zero(0).', 350, 90);
        }
        else if (IndentDetail.PreferredVendorId == null || IndentDetail.PreferredVendorId == 0) {
            $scope.openMessageBox('Warning', 'Please choose preferred vendor.', 250, 90);
        }
        else
            $.ajax({

                url: "/Indent/SaveIndentDetail",
                type: "Post",
                //contentType: "application/json;",
                dataType: "json",
                data: {
                    IndentDetail: IndentDetail, itemDescription: $scope.model.Item.Description
                },
                success: function (response) {
                    if (response.Status == 1) {
                        // Success
                        $scope.$apply(function () {
                            $scope.IndentGridSource.data.IndentId = parseInt($('#txtId').val());
                            $scope.GrdItem.updatebounddata();
                            //ClearData();
                            IndentDetailId = 0;
                            $scope.model.IndentDetail.Id = response.Data.Id;
                            $scope.ShowAttachment = true;
                        });
                    }
                    else if (response.Status = 2) {
                        // Warning
                        $scope.openMessageBox('Warning', response.Message, 400, 100);
                    }
                    else {
                        // Error
                        $scope.openMessageBox('Error', response.Message, 700, 500);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });

    };

    $scope.onUpdateClick = function (e) {
        var item = $scope.ddlIndentStatus.getSelectedItem();
        var status = item.label;

        var PoDate = null;
        if ($scope.model.Indent.PoAmount == null || $scope.model.Indent.PoAmount == 0)
            $scope.model.Indent.PoAmount = 0;

        if ($scope.dtIndentPoDate.getDate() != null)
            PoDate = $scope.dtIndentPoDate.getDate().toISOString();

        var DeliveryDate = null;
        
        if ($scope.dtIndentDeliveryDate.getDate() != null)
            DeliveryDate = $scope.dtIndentDeliveryDate.getDate().toISOString();

        if (status == "PO" && $scope.model.Indent.PoDate == null)
            $scope.openMessageBox('Warning', 'Insert Po date', 350, 90);
            //else if (status == "PO" && $scope.model.Indent.DeliveryDate == null)
            //    $scope.openMessageBox('Warning', 'Insert Delivery Date', 350, 90);
        else if (status == "PO" && $scope.model.Indent.PoNo == null)
            $scope.openMessageBox('Warning', 'Insert PoNo', 350, 90);
        else if (status == "PO" && ($scope.model.Indent.PoAmount == null || $scope.model.Indent.PoAmount < 0))
            $scope.openMessageBox('Warning', 'Po amount is greater than zero', 350, 90);

        else

            $.ajax({
                url: "/Indent/UpdateStatusandNote",
                type: "Post",
                //contentType: "application/json;",
                dataType: "json",
                data: {
                    indentId: parseInt($('#txtId').val()), note: $scope.model.Indent.Note, StatusId: $scope.model.Indent.StatusId, Subject: $scope.model.Indent.Subject,
                    PoDate: PoDate, //$scope.model.Indent.PoDate.toISOString()
                    DeliveryDate: DeliveryDate,//$scope.model.Indent.DeliveryDate.toISOString()
                    PoNo: $scope.model.Indent.PoNo,
                    PoAmount: $scope.model.Indent.PoAmount
                },
                success: function (response) {
                    if (response.Status == 1) {
                        // Success
                        $scope.openMessageBox('Success', 'record successfully updated', 400, 100);
                        $scope.GrdItem.updatebounddata();
                    }
                    else if (response.Status = 2) {
                        // Warning
                        $scope.openMessageBox('Warning', response.Message, 400, 100);
                    }
                    else {
                        // Error
                        $scope.openMessageBox('Error', response.Message, 400, 100);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
    }


    $scope.IndentGridSource = {
        datatype: "json",
        data: {
            IndentId: parseInt($('#txtId').val())
        },
        datafields: [
           {
               name: 'Id', type: 'int'
           },
           {
               name: 'ItemCode', type: 'int'
           },
           {
               name: 'JobDescription', type: 'string'
           },
           {
               name: 'PlantName', type: 'string'
           },
           {
               name: 'Name', type: 'string'
           },
           {
               name: 'QtyNeeded', type: 'decimal'
           },
           {
               name: 'StatusName', type: 'string'
           },
           { name: 'RequiredByDate', type: 'date' }
        ],
        url: '/Indent/GetIndentGridList'
    };

    $scope.DeleteCellRenderer = function (row, column, value) {

        return "Delete";
    };
    $scope.UpdateCellRenderer = function (row, column, value) {

        return "Edit";
    };

    $scope.DeleteIndentDetail = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete selected item(s)?', 350, 100, function (isYes) {
            if (isYes) {

                var rows = $scope.GrdItem.selectedrowindexes;
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.GrdItem.getrowdata(rows[m]);
                    if (row != null && row.Id > 0)
                        selectedIds.push(row.Id);
                }

                $.ajax({
                    dataType: "json",
                    type: 'POST',
                    url: '/Indent/DeleteIndentDetail',
                    data: {
                        Ids: selectedIds
                    },
                    success: function (response) {
                        $scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        $scope.GrdItem.updatebounddata();
                    },
                    error: function (jqXHR, exception) {
                    }
                });
            }
        });
    }
    $scope.DeleteAll = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete all item(s)?', 350, 100, function (isYes) {
            if (isYes) {


                $.ajax({
                    dataType: "json",
                    type: 'POST',
                    url: '/Indent/DeleteAllIndent',
                    data: {
                        Id: parseInt($('#txtId').val())
                    },
                    success: function (response) {
                        //$scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        window.location.href = "/Indent/List/";
                        // $scope.GrdItem.updatebounddata();
                    },
                    error: function (jqXHR, exception) {
                    }
                });
            }
        });
    };

    $scope.EditIndentDetail = function (row) {
        var dataRecord = $scope.GrdItem.getrowdata(row);
        IndentDetailId = dataRecord.Id;
        $scope.model.IndentDetail.Id = IndentDetailId;
        $.ajax({
            url: "/Indent/GetIndentDetailsById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: {
                Id: IndentDetailId
            },
            success: function (data) {

                $scope.$apply(function () {
                    $scope.model.IndentDetail = data;
                    $scope.model.IndentDetail.RequiredByDate = new Date($scope.ToJavaScriptDate(data.RequiredByDate));
                    if (typeof (data.PoDate) != 'undefined' && data.PoDate != null) {
                        $scope.model.IndentDetail.PoDate = new Date($scope.ToJavaScriptDate(data.PoDate));
                    }
                    if (typeof (data.DeliveryDate) != 'undefined' && data.DeliveryDate != null) {
                        $scope.model.IndentDetail.DeliveryDate = new Date($scope.ToJavaScriptDate(data.DeliveryDate));
                    }
                    if (typeof (data.ApprovedOn) != 'undefined' && data.ApprovedOn != null) {
                        $scope.model.IndentDetail.ApprovedOn = new Date($scope.ToJavaScriptDate(data.ApprovedOn));
                    }
                    if (typeof (data.Rejectedon) != 'undefined' && data.Rejectedon != null) {
                        $scope.model.IndentDetail.Rejectedon = new Date($scope.ToJavaScriptDate(data.Rejectedon));
                    }
                    $scope.ddlStatus.val(data.StatusId);
                    var item = $scope.ddlStatus.getSelectedItem();
                    var status = item.label;
                    if (status == "New") {
                        $scope.IsIssue = true;
                        $scope.IsPo = true;
                        $scope.IsSave = false;
                        $scope.IsCancel = false;
                        $scope.IsApprovedBy = true;
                        $scope.IsApprovedDate = true;
                        $scope.IsRejectedBy = true;
                        $scope.IsRejectedDate = true;
                    }
                    else if (status == "Approved") {
                        $scope.IsIssue = true;
                        $scope.IsPo = true;
                        $scope.IsSave = false;
                        $scope.IsCancel = false;
                        $scope.IsApprovedBy = false;
                        $scope.IsApprovedDate = false;
                        $scope.IsRejectedBy = true;
                        $scope.IsRejectedDate = true;
                    }
                    else if (status == "Po") {
                        $scope.IsIssue = true;
                        $scope.IsPo = false;
                        $scope.IsSave = false;
                        $scope.IsCancel = false;
                        $scope.IsApprovedBy = true;
                        $scope.IsApprovedDate = true;
                        $scope.IsRejectedBy = true;
                        $scope.IsRejectedDate = true;
                    }
                    else if (status == "Rejected") {
                        $scope.IsIssue = true;
                        $scope.IsPo = true;
                        $scope.IsSave = true;
                        $scope.IsCancel = true;
                        $scope.IsApprovedBy = true;
                        $scope.IsApprovedDate = true;
                        $scope.IsRejectedBy = false;
                        $scope.IsRejectedDate = false;
                    }
                    else if (status == "Received") {
                        $scope.IsIssue = false;
                        $scope.IsPo = true;
                        $scope.IsSave = true;
                        $scope.IsCancel = true;
                        $scope.IsApprovedBy = true;
                        $scope.IsApprovedDate = true;
                        $scope.IsRejectedBy = true;
                        $scope.IsRejectedDate = true;
                    }
                    $scope.ShowAttachment = true;

                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

                alert('oops, something bad happened');
            }
        });

    }

    var indentId = parseInt($('#txtId').val());
    if (indentId > 0) {
        $.ajax({
            url: "/Indent/GetIndentById",
            type: "GET",
            cache: false,
            contentType: "application/json;",
            dataType: "json",

            data: {
                IndentId: indentId
            },
            success: function (data) {
                $scope.$apply(function () {
                    //$scope.ddlIndentStatus.val(data.StatusId);
                    $scope.IsDisabled = false;
                    $scope.IsUpdate = true;
                    $scope.ShowAttachment = false;
                    $scope.model.Indent = data;

                    if (typeof (data.PoDate) != 'undefined' && data.PoDate != null) {
                        $scope.model.Indent.PoDate = new Date($scope.ToJavaScriptDate(data.PoDate));
                    }
                    if (typeof (data.DeliveryDate) != 'undefined' && data.DeliveryDate != null) {
                        $scope.model.Indent.DeliveryDate = new Date($scope.ToJavaScriptDate(data.DeliveryDate));
                    }

                    if (data.RequisitionType == "PR") {
                        $scope.IsPR = false;
                        $scope.IsJR = true;
                        //$('#lnkDownload').attr('href', '/Extras/Download.aspx?RequestCode=DownloadIndent&Type=PR&IndentId=' + indentId);
                    }
                    if (data.RequisitionType == "JR") {
                        $scope.IsPR = true;
                        $scope.IsJR = false;
                        //$('#lnkDownload').attr('href', '/Extras/Download.aspx?RequestCode=DownloadIndent&Type=JR&IndentId=' + indentId);
                    }
                    $('#lnkDownload').attr('href', '/Download/Indent/' + indentId);
                });

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

                alert('oops, something bad happened');
            }
        });
    }

    $scope.onCancelClick = function (e) {
        ClearData();
    }

    var ClearData = function (e) {
        $scope.ShowAttachment = false;
        IndentDetailId = 0;
        $scope.model.IndentDetail.Id = 0;
        $scope.ddlItemName.clearSelection();
        //$scope.ddlPlant.clearSelection();
        //$scope.ddlLine.clearSelection();
        //$scope.LineSource.data = { PlantId: 0 };
        //$scope.MachineSource.data = { LineId: 0 };
        //$scope.ddlLine.clear();
        //$scope.MachineInstance.clearSelection();
        //$scope.MachineInstance.clear();
        $scope.ddlVendor.clearSelection();
        $scope.ddlStatus.selectIndex(0);
        $scope.ddlCurrency.selectIndex(1);

        $scope.model.IndentDetail.QtyNeeded = 0;
        $scope.ddlDescription.val('');

        $scope.model.IndentDetail.ItemId = 0;
        $scope.model.Item.ItemCode = null;
        $scope.model.Item.Price = 0;
        $scope.model.Item.Make = null;
        $scope.model.Item.Model = null;
        $scope.model.Item.UnitOfMeasure = null;
        $scope.model.Item.Description = null;
        $scope.model.Item.AvailableQty = null;
        //$scope.model.IndentDetail.PlantId = null;
        //$scope.model.IndentDetail.LineId = null;
        //$scope.model.IndentDetail.MachineId = null;
        $scope.model.IndentDetail.QtyNeeded = null;
        $scope.model.IndentDetail.PreferredVendorId = null;
        $scope.model.IndentDetail.EstimatePrice = 0;
        $scope.model.IndentDetail.ExchangeRate = 1;
        $scope.model.IndentDetail.Make = '';
        $scope.model.IndentDetail.UnitOfMeasure = '';
        $scope.model.IndentDetail.JobDescription = '';

        $scope.model.IndentDetail.RequiredByDate = new Date()
    }

    $scope.AddNewItem = function (event) {
        $scope.Itemwindow.open();
    };

    $scope.AddNewVendor = function (event) {
        $scope.Vendorwindow.open();
    };

    $scope.onAttachmentClick = function () {
        $scope.AttachmentSource.data = { IndentDetailId: $scope.model.IndentDetail.Id };
        $scope.Attachmentwindow.open();
    };

    $scope.onSendEmailClick = function () {
        //var indentId = parseInt($('#txtId').val());
        $.ajax({
            url: "/Download/SendIndentEmail",
            type: "Post",
            dataType: "json",
            data: { indentId: indentId },
            success: function (response) {
                if (response.Status == 1) {
                    // Success
                    $scope.openMessageBox('Success', "Email sent successfully.", 400, 100);
                }
                else {
                    // Error
                    $scope.openMessageBox('Error', response.Message, 400, 100);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.CloseAttachmentWindow = function () {
        $scope.Attachmentwindow.close();
    }


    $scope.DownloadAttachmentCellRenderer = function (row, column, value) {
        var dataItem = $scope.GrdAttachment.getrowdata(row);
        //return "<a target='_blank' href='/Extras/Download.aspx?RequestCode=IndentAttachment&Id=" + dataItem.Id + "'>Download</a>";
        return "<a href='/Download/IndentAttachment/" + dataItem.Id + "'>Download</a>";
        //return 'Download';
    };
    $scope.DeleteAttachmentCellRenderer = function (row, column, value) {

        return "Delete";
    };
    $scope.SchoolLogo_UploadEnd = function (event) {
        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;

        var jsonResponse = JSON.parse(serverResponce);

        // Ajax call here...
        $.ajax({

            url: "/Indent/SaveAttachments",
            type: "Post",
            //contentType: "application/json;",
            dataType: "json",
            data: { IndentDetailId: $scope.model.IndentDetail.Id, OriginalFileName: jsonResponse[0].OriginalFileName, SysFileName: jsonResponse[0].SysFileName },
            success: function (response) {
                if (response.Status == 1) {
                    // Success
                    $scope.GrdAttachment.updatebounddata();
                    // $scope.Attachmentwindow.close();

                }
                else if (response.Status = 2) {
                    // Warning
                    $scope.openMessageBox('Warning', response.Message, 400, 100);
                }
                else {
                    // Error
                    $scope.openMessageBox('Error', response.Message, 400, 100);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    };

    $scope.AttachmentSource = {
        datatype: "json",
        data: {
            IndentDetailId: 0
        },
        datafields: [
           {
               name: 'Id', type: 'int'
           },
           {
               name: 'SysFileName', type: 'string'
           },
           {
               name: 'OriginalFileName', type: 'string'
           },

        ],
        url: '/Indent/GetAttachmentGridList'
    };
    $scope.DeleteAttachment = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete selected item(s)?', 350, 100, function (isYes) {
            if (isYes) {

                var rows = $scope.GrdAttachment.selectedrowindexes;
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.GrdAttachment.getrowdata(rows[m]);
                    if (row != null && row.Id > 0)
                        selectedIds.push(row.Id);
                }

                $.ajax({
                    dataType: "json",
                    type: 'GET',
                    url: '/Indent/DeleteAttachment',
                    data: {
                        Ids: selectedIds
                    },
                    success: function (response) {
                        $scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        $scope.GrdAttachment.updatebounddata();
                    },
                    error: function (jqXHR, exception) {
                    }
                });
            }
        });
    }

    $scope.DownloadClick = function (row) {
        var dataItem = $scope.GrdAttachment.getrowdata(row);
        $.ajax({
            dataType: "json",
            type: 'POST',
            url: '/Download/DownloadIndentAttachment',
            data: { Id: dataItem.Id },
            success: function (response) {
            },
            error: function (jqXHR, exception) {
            }
        });
    }

    //$scope.SaveNewItem = function (event) {
    //    $scope.$broadcast('onSaveItem', {
    //        Id: 0, callback: function(response) {
    //            if (response.Type == 'Success') {
    //                $scope.Itemwindow.close();
    //                $scope.ddlItemName.addItem(response.Data);
    //                $scope.ddlItemName.val(response.Data.Id);
    //            }
    //        }
    //    });
    //};
});