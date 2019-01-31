ProApp.controller("FormulationCreateController", function ($scope, $http, $timeout) {

    var qtytoProduce = 0;
    var MachineData = [];
    $scope.IsCreate = true;
    $scope.IsDownload = false;
    $scope.disableProperty = false;
    $scope.IsGenerated = true;
    $scope.IsBaseValue = false;
    $scope.IsNew = false;
    $scope.IsDeleted = false;
    $scope.model = {};
    var formulaDetailDeletedIds = [];

    $scope.GenerateRules = [
        {
            input: '#ddlLine', message: 'Line is required', action: 'change',
            rule: function (input, commit) {
                if (parseInt($('#txtId').val()) > 0) {
                    $scope.GenerateValidator.hide();
                    return true;
                }
                else {
                    var index = $scope.ddlLine.getSelectedIndex();
                    return index != -1;
                }

            }
        },
        {
            input: '#ddlProductCategory', message: 'Product category is required', action: 'change',
            rule: function (input, commit) {
                if (parseInt($('#txtId').val()) > 0) {
                    $scope.GenerateValidator.hide();
                    return true;
                }
                else {
                    var index = $scope.ddlProductCategory.getSelectedIndex();
                    return index != -1;
                }

            }
        },
         {
             input: '#ddlProduct', message: 'Product name is required', action: 'change',
             rule: function (input, commit) {
                 if (parseInt($('#txtId').val()) > 0) {
                     $scope.GenerateValidator.hide();
                     return true;
                 }
                 else {
                     var index = $scope.ddlProduct.getSelectedIndex();
                     return index != -1;
                 }
             }
         }
    ];

    $scope.FormulationRules = [
        { input: '#txtLotNo', message: 'LotNo is required!', action: 'blur', rule: 'required' },
        {
            input: '#txtLotNo', message: 'Name already exists!', action: 'keyup,blur,valueChanged', rule: function (input, commit) {
                var LotNo = '';
                if (typeof ($scope.model.LotNo) != 'undefined')
                    LotNo = $scope.model.LotNo;
                if (LotNo == '')
                    return true;

                var isValidate = false;
                $.ajax({
                    async: false,
                    url: "/FormulationRequest/IsLotNoExists",
                    type: "GET",
                    contentType: "application/json;",
                    dataType: "json",
                    data: { LotNo: LotNo },
                    success: function (response) {
                        if (response.Status == 1) {
                            isValidate = !response.Data;
                        }
                        else {
                            $scope.openMessageBox('Error', respond.Message, 400, 100);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('oops, something bad happened');
                    }
                });
                commit(isValidate);
                if (!isValidate)
                    return false;
            }
        },
        { input: '#txtLotSize', message: 'LotSize is required!', action: 'blur', rule: 'required' }
    ];

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

    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetFormulationLine',
    };

    $scope.popItemSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/RMItem/GetItemListForFormula',
    };

    $scope.FormulationSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'ItemId', type: 'int' },
           { name: 'MachineId', type: 'int' },
           { name: 'ItemName', type: 'string' },
           { name: 'ProductId', type: 'int' },
           { name: 'ProductName', type: 'string' },
           { name: 'PreviousBaseValue', type: 'decimal' },
           { name: 'BaseValue', type: 'decimal' },
           { name: 'InGrams', type: 'decimal' },
           { name: 'StatusId', type: 'int' }
        ],
        url: '/FormulationRequest/GetFormulationByProductId',
        data: { productId: 0, IsRequest: false }
    };

    $scope.FormulationColumn = [
         { text: 'Bill of materials', datafield: 'ItemName', editable: false },
         {
             text: 'Previous %', datafield: 'PreviousBaseValue', editable: false, width: 200, aggregates: ['sum'],
             cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                 var data = this.owner.getrowdata(row);
                 if (data.StatusId > 0)
                     $scope.GrdFormulation.hidecolumn("PreviousBaseValue");
             }
         },
         {
             text: '%', datafield: 'BaseValue', editable: true, width: 200, aggregates: ['sum'],
             cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
                 if (newvalue == "") {
                     return oldvalue;
                 }
                 var data = this.owner.getrowdata(rowindex);
                 if (typeof (data.BaseValue) != 'undefined' && data.BaseValue != null) {
                     data.InGrams = newvalue;
                 }
             },
             validation: function (cell, value) {
                 if (value > 0 && value <= 100) {
                     return true;
                 }
                 else {
                     return { result: false, message: "Percentage can not be text." };
                 }
             }
         },
         {
             text: 'In Grams', datafield: 'InGrams', editable: false, width: 250,
             cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                 var data = this.owner.getrowdata(row);
                 if (data.StatusId == 0) {
                     return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + ';">' + ((value * qtytoProduce) / 100) + '</span>';
                 }
                 else {
                     return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + ';">' + value + '</span>';
                 }
             }
         }
    ];

    $scope.onCategoryChange = function (e) {
        $scope.ProductSource.data = { categoryId: $scope.ddlProductCategory.val() };
    }

    $scope.onBindingCategory = function (e) {
        $scope.ddlProductCategory.val($scope.model.CategoryId);
    };
    $scope.onBindingProduct = function (e) {
        $scope.ddlProduct.val($scope.model.ProductId);
    };

    $scope.onGenerateRequest = function (event) {
        var isValidate = $scope.GenerateValidator.validate();
        if (!isValidate)
            return;

        $scope.disableProperty = false;
        if ($scope.model.QtyToProduce == null || $scope.model.QtyToProduce == 0)
            $scope.openMessageBox('Warning', 'Qty prouce must be greater than zero(0).', 350, 90);
        else {
            var categoryId = $scope.ddlProductCategory.getSelectedItem().value;
            var productId = $scope.ddlProduct.getSelectedItem().value;
            qtytoProduce = $scope.model.QtyToProduce;
            var currentDate = new Date();
            var fullDate = [currentDate.getFullYear().toString().substr(-2), ('0' + (currentDate.getMonth() + 1)).slice(-2), ('0' + currentDate.getDate()).slice(-2)].join('');
            var lotno = $scope.ddlLine.getSelectedItem().label + fullDate;
            $scope.model.LotNo = lotno;
            var lineName = $scope.ddlLine.getSelectedItem().label;
            $scope.FormulationSource.data = { productId: productId, IsRequest: false };
            $scope.GrdFormulation.updatebounddata();
            $scope.IsCreate = true;
            $scope.IsDownload = false;
            $scope.IsBaseValue = true;
            $scope.IsNew = true;
            $scope.IsDeleted = true;
        }
    }

    $scope.onAddNewFormuation = function (event) {
        $scope.WinAddFormulation.open();
    }

    $scope.onDeleteFormulation = function (event) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Formulation?', 350, 100, function (e) {
            if (e) {
                var rows = $scope.GrdFormulation.selectedrowindexes;
                if (rows == null)
                    $scope.openMessageBox("Warning", "Please select row.", 200, 90);
                for (var m = 0; m <= rows.length; m++) {
                    var row = $scope.GrdFormulation.getrowdata(rows[m]);
                    if (row != null) {
                        formulaDetailDeletedIds.push(row.Id);
                        $scope.GrdFormulation.deleterow(rows[m]);
                    }
                }
            }
        });

    }

    $scope.onSaveNewFormula = function (event) {
        var itemId = $scope.ddlPopItem.getSelectedItem().value;
        var ItemName = $scope.ddlPopItem.getSelectedItem().label;
        var PercebaseValue = $scope.PercBaseValue;
        var baseValueGram = (PercebaseValue * qtytoProduce) / 100;
        var rows = {
            Id: 0, ItemId: itemId, MachineId: 0, ItemName: ItemName,
            ProductId: $scope.ddlProduct.getSelectedItem().value, PreviousBaseValue: PercebaseValue,
            BaseValue: PercebaseValue, InGrams: PercebaseValue, StatusId: 0
        };
        $scope.GrdFormulation.addrow(null, rows);
        $scope.ddlPopItem.clearSelection();
        $scope.PercBaseValue = 0;
        $scope.WinAddFormulation.close()
    }

    var formuationId = 0;
    $scope.onCreateFormulation = function (event) {
        var isValidate = $scope.FormulationValidator.validate();
        if (!isValidate)
            return;
        var totalPercentage = 0;
        var rows = $scope.GrdFormulation.getrows();
        var gramCalculation = $.grep(rows, function (a) {
            return a.InGrams = (a.InGrams * qtytoProduce) / 100;
        })
        $scope.model.LineId = $scope.ddlLine.getSelectedItem().value;
        $scope.model.GradeName = $scope.ddlProduct.getSelectedItem().label;
        $scope.model.QtyToProduce = $scope.model.QtyToProduce;
        //$scope.model.ColorSTD = $scope.ddlProduct.getSelectedItem().label;
        $scope.model.ProductId = $scope.ddlProduct.getSelectedItem().value;
        var postData = [];
        for (var i = 0; i < rows.length; i++) {
            var item = rows[i];
            totalPercentage = totalPercentage + item.BaseValue;
            var postItem = {
                Id: 0, FormulationRequestId: 0, MachineId: 0, ItemId: item.ItemId, ItemQtyPercentage: item.BaseValue, ItemQtyGram: item.InGrams
            };
            postData.push(postItem);
        }
        var data = $scope.model;
        $scope.model.FormulationRequestsDetails = postData;

        if (totalPercentage == 100) {
            $http.post('/FormulationRequest/SaveFormulationData', { formulationData: $scope.model }).then(function (result) {
                if (result.data.Status == 1) {
                    $scope.GrdFormulation.updatebounddata();
                    $scope.openMessageBox('Success', result.data.Message, 300, 90);
                    $scope.IsCreate = false;
                    $scope.IsDownload = true;
                    $scope.IsGenerated = false;
                    totalPercentage = 0;
                    window.location = "/FormulationRequest/Create/" + result.data.Data;
                    $scope.FormulationValidator.hide();
                    $scope.GenerateValidator.hide();
                    ClearData();
                }
                else {
                    $scope.GrdFormulation.updatebounddata();
                    $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                }
            }, function (result, status, headers, config) {
                alert("status");
            });
        }
        else if (totalPercentage < 100 || totalPercentage > 100) {
            $scope.openConfirm("Warning", 'Formulation request percetage not 100%, Are you sure you want to Create Formulation?', 350, 100, function (e) {
                if (e) {
                    $http.post('/FormulationRequest/SaveFormulationData', { formulationData: $scope.model }).then(function (result) {
                        if (result.data.Status == 1) {
                            $scope.GrdFormulation.updatebounddata();
                            $scope.openMessageBox('Success', result.data.Message, 300, 90);
                            $scope.IsCreate = false;
                            $scope.IsDownload = true;
                            $scope.IsGenerated = false;
                            totalPercentage = 0;
                            window.location = "/FormulationRequest/Create/" + result.data.Data;
                            $scope.FormulationValidator.hide();
                            $scope.GenerateValidator.hide();
                            ClearData();
                        }
                        else {
                            $scope.GrdFormulation.updatebounddata();
                            $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                        }
                    }, function (result, status, headers, config) {
                        alert("status");
                    });
                }
                else {
                    return;
                }
            });
        }

      
    }

    $scope.onUpdateBaseValue = function (event) {
        var rows = $scope.GrdFormulation.getrows();
        $scope.openConfirm("Confirmation", 'Are you sure you want to update Base value?', 350, 100, function (e) {
            if (e) {
                var masterData = [];
                for (var i = 0; i < rows.length; i++) {
                    var item = rows[i];
                    var postItem = {
                        Id: item.Id, ProductId: item.ProductId, BaseValue: item.BaseValue, PreviousBaseValue: item.PreviousBaseValue
                    };
                    masterData.push(postItem);
                }
                $http.post('/FormulationRequest/UpdateBaseValue', { masterData: masterData }).then(function (result) {
                    if (result.data.Status == 1) {
                        $scope.openMessageBox('Success', result.data.Message, 300, 90);
                    }
                    else {
                        $scope.openMessageBox('Error', result.data.Message, 'auto', 'auto');
                    }
                }, function (result, status, headers, config) {
                    alert("status");
                });
            }
            else {
                $scope.GrdFormulation.updatebounddata();
            }
        });
    }

    $scope.onDownloadClick = function () {
        var Id = parseInt($('#txtId').val());
        if (Id > 0) {
            window.location.href = "/Download/FormulationRequest/?Id=" + Id;
        }
    }

    $scope.OnGridReady = function () {
        var Id = parseInt($('#txtId').val());
        if (Id > 0) {
            $scope.FormulationSource.data.productId = Id;
            $scope.FormulationSource.data.IsRequest = true;
            $scope.GrdFormulation.updatebounddata();
            $.ajax({
                url: "/FormulationRequest/GetFormulationRequestById",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { Id: Id },
                success: function (data) {
                    $scope.$apply(function () {
                        $scope.FormulationValidator.hide();
                        $scope.GenerateValidator.hide();
                        $scope.disableProperty = true;
                        $scope.IsCreate = false;
                        $scope.IsDownload = true;
                        $scope.IsGenerated = false;
                        $scope.IsBaseValue = false;
                        $scope.IsNew = false;
                        $scope.IsDeleted = false;
                        qtytoProduce = data.QtyToProduce;
                        $scope.model = data;
                        $scope.ddlProductCategory.val(data.CategoryId);
                        $scope.ProductSource.data = { categoryId: data.CategoryId };
                    });

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });

        }
    };




    var ClearData = function () {
        if ((typeof ($scope.model) != 'undefined') && $scope.model != null) {
            $scope.model.FormulationRequests = {};
            MachineData = [];
            $scope.model.LotNo = null;
            $scope.model.LOTSize = null;
            $scope.model.QtyToProduce = null;
            $scope.GrdFormulation.updatebounddata();

        }
    }

});