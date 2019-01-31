﻿ProApp.controller("UtilityConfigurationController", function ($scope, $http) {
    
   // var TemplateMappingData = [];
    var TemplateMappingData = [];
    var TempMappingId = [];
    var columnId = 0;
    var templateId = 0;
    $scope.IsAggregateFunction = false;

    $scope.ReportRules = [
        { input: '#txtReportName', message: 'Report name is required!', action: 'blur', rule: 'required' }//,
        //{ input: '#txtDescription', message: 'Description is required!', action: 'blur', rule: 'required' }
    ];

    $scope.UtilityColumnRules = [
        { input: '#txtColumnName', message: 'Column name is required!', action: 'blur', rule: 'required' },
        {
            input: '#dllDatatype', message: 'Please choose Datatype', action: 'change',
            rule: function (input, commit) {
                var index = $scope.dllDatatype.getSelectedIndex();
                return index != -1;
            }},
        { input: '#txtGroupName', message: 'Group name is required!', action: 'blur', rule: 'required' },
        //{ input: '#txtOrderNo', message: 'Order no is required!', action: 'blur', rule: 'required' },
        { input: '#txtWidth', message: 'Width is required!', action: 'blur', rule: 'required' }
    ];


    $scope.UtilityConfigSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'Description', type: 'string' },
           { name: 'IsActive', type: 'bool' }
        ],
        url: '/Utility/GetReportList',
        data: { ReportName: '' },
        updaterow: function (rowid, rowdata, commit) {
            $.ajax({
                url: "/Utility/UpdateIsActive",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { TempId: rowdata.Id, IsActive: rowdata.IsActive },
                success: function (data) {
                    if (data.Status == 1) {
                        $scope.openMessageBox('Success', data.Message, 200, 90);
                        //$scope.GridUtilityConfig.updatebounddata();
                    }
                    else {
                        $scope.openMessageBox('Error', data.Message, 500, 100);
                        //$scope.GridUtilityConfig.updatebounddata();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
        },
    };

    $scope.TemplateMappingSource = {
        datatype: "json",
        localdata: TemplateMappingData,
        autoBind: true,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'TemplateId', type: 'int' },
           { name: 'ColumnName', type: 'string' },
           { name: 'GroupName', type: 'string' },
           { name: 'DataType', type: 'string' },
           { name: 'IsRequired', type: 'bool' },
           { name: 'IsAggregate', type: 'bool' },
           { name: 'AggregateFunction', type: 'string' },
           { name: 'IsSearchColumn', type: 'bool' },
           { name: 'IsEditable', type: 'bool' },
           { name: 'OnChangeFormula', type: 'string' },
           { name: 'IsAutoGenerated', type: 'bool' },
           { name: 'IsOrderBy', type: 'bool' },
           { name: 'Width', type: 'string' },
           { name: 'DefaultValue', type: 'string' },
           { name: 'OrderNo', type: 'int' }
        ]
    };
   
    $scope.onRefreshClick = function (e) {
        $scope.GridUtilityConfig.updatebounddata();
    }

    $scope.onAddNewReport = function (e) {
       // $scope.IsActive = true;
        $scope.model = [];
        templateId = 0;
        $scope.TemplateMappingSource = {};
        TemplateMappingData = [];
        //TemplateMappingData = [];
        $scope.ReportValidator.hide();
        $scope.model.Id = 0;
        $scope.model.Name = null;
        $scope.txtDescription.val('');
        $scope.WinAddReport.open();
    }
    //var templateId = 0;

    $scope.OnAddColumn = function (e) {
        $scope.UtilityColumnValidator.hide();
        var IsValidate = $scope.ReportValidator.validate();
        if (!IsValidate)
            return;
        $scope.model = [];
        //$scope.model.Id = 0;
        ClearData();
        $scope.WinAddColumn.open();
    }

    $scope.Aggregate = function (value) {
        if (value == "True")
            $scope.IsAggregateFunction = true;
        else
            $scope.IsAggregateFunction = false;
    }


    
    $scope.OnSaveRow = function (e) {
        var IsValidate = $scope.UtilityColumnValidator.validate();
        if (!IsValidate)
            return;
        $scope.model.TemplateId = templateId;
        var data = $scope.model;
        var minId = Math.min.apply(Math, TemplateMappingData.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;
       // var aggratefun = $scope.dllAggregateype.getSelectedItem().label;

        if (columnId != 0 )
        {
            for (var j = 0; j < TemplateMappingData.length; j++) {
                if (TemplateMappingData[j].Id == columnId) {
                    TemplateMappingData[j].ColumnName = data.ColumnName;
                    TemplateMappingData[j].GroupName = data.GroupName;
                    TemplateMappingData[j].IsRequired = data.IsRequired;
                    TemplateMappingData[j].IsAggregate = data.IsAggregate;
                    TemplateMappingData[j].OnChangeFormula = data.OnChangeFormula;
                    TemplateMappingData[j].IsAutoGenerated = data.IsAutoGenerated;
                    TemplateMappingData[j].IsOrderBy = data.IsOrderBy;
                    TemplateMappingData[j].Width = data.Width;
                    TemplateMappingData[j].DefaultValue = data.DefaultValue;
                    TemplateMappingData[j].DataType = $scope.dllDatatype.val();
                    TemplateMappingData[j].AggregateFunction = $scope.dllAggregateype.getSelectedItem().value;
                }
            }
        }
        else
        {
            Id = minId - 1;
            var datatype = $scope.dllDatatype.getSelectedItem().value;
            var aggratefun = $scope.dllAggregateype.getSelectedItem().value;
            TemplateMappingData.push({
                Id: Id, TemplateId: templateId, ColumnName: data.ColumnName, DataType: datatype, GroupName: data.GroupName,
                IsRequired: data.IsRequired, IsAggregate: data.IsAggregate,
                AggregateFunction: aggratefun, IsSearchColumn: data.IsSearchColumn, IsEditable: data.IsEditable,
                OnChangeFormula: data.OnChangeFormula, IsAutoGenerated: data.IsAutoGenerated, IsOrderBy: data.IsOrderBy,
                Width: data.Width, DefaultValue: data.DefaultValue
            });//FixColumnId: fixcolumn     OrderNo: $scope.model.OrderNo,
        }
     
        $scope.TemplateMappingSource.localdata = TemplateMappingData;
        $scope.GridTemplateMapping.updatebounddata();
        //ClearData();
        $scope.WinAddColumn.close();
    }

    $scope.OnSaveAllColumn = function (e) {
        var reportName = $scope.txtName.val();
        var desc = $scope.txtDescription.val();
        var active = $scope.chkIsActive.val();
       
        $http.post('/Utility/SaveReport', { TemplateId: templateId, Name: reportName, Description: desc, IsActive: active, templatecolumn: TemplateMappingData, TempMappingIds: TempMappingId }).success(function (response) {
            if (response.Status == 1) {
                $scope.GridUtilityConfig.updatebounddata();
                $scope.WinAddReport.close();
                TemplateMappingData = [];
                //TemplateMappingData = [];
                templateId = 0;
                $scope.openMessageBox('Success', response.Message, 300, 90);
            } else if (response.Status == 3) {
                $scope.openMessageBox('Error', response.Message, 'auto', 'auto');
            }

        }).error(function (result, status, headers, config) {
            alert("status");
        });
    }

    $scope.model = [];

    $scope.EditReport = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Edit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    };
    $scope.Edit = function (row) {
        var dataRecord = $scope.GridUtilityConfig.getrowdata(row);
        templateId = dataRecord.Id;
        $.ajax({
            url: "/Utility/GetColumnByTemplateId",
            type: "GET",
            contentType: "application/json;",
            cache: false,
            dataType: "json",
            data: { TemplateId: templateId },
            success: function (data) {
                $scope.$apply(function () {
                    TemplateMappingData = data;
                    $scope.model.Name = dataRecord.Name;
                    $scope.model.Description = dataRecord.Description;
                    $scope.model.IsActive = dataRecord.IsActive;
                    $scope.TemplateMappingSource.localdata = $.grep(TemplateMappingData, function (item, i) {
                        return item;
                    });
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.WinAddReport.open();
    }


    $scope.EditCellRenderer = function (row, column, value) {
        return "Edit";
    };

    $scope.EditRow = function (row) {
        var dataRecord = $scope.GridTemplateMapping.getrowdata(row);
        columnId = dataRecord.Id;
        var mappingId = dataRecord.Id;
        for (var i = 0; i < TemplateMappingData.length ; i++) {
            if (mappingId == TemplateMappingData[i].Id) {
                $scope.$apply(function () {
                    $scope.dllDatatype.val(TemplateMappingData[i].DataType);
                    if (dataRecord.IsAggregate == true)
                    {
                        $scope.IsAggregateFunction = true;
                        $scope.dllAggregateype.val(TemplateMappingData[i].AggregateFunction);
                    }
                    else
                        $scope.IsAggregateFunction = false;
                    $scope.model = TemplateMappingData[i];
                    $scope.GridTemplateMapping.updatebounddata();
                });
            }
        }
        $scope.WinAddColumn.open();
    }

    $scope.DeleteCellRenderer = function (row, column, value) {
        return "Delete";
    }

    $scope.DeleteRow = function (row) {
        var dataRecord = $scope.GridTemplateMapping.getrowdata(row);
        TempMappingId.push(dataRecord.Id);

        TemplateMappingData = $.grep(TemplateMappingData, function (item, i) {
            return item.Id != dataRecord.Id;
        });
        $scope.TemplateMappingSource.localdata = $.grep(TemplateMappingData, function (item, i) {
            return item;
        });
        $scope.GridTemplateMapping.updatebounddata();
    }

    $scope.onSearch = function (e) {
        $scope.UtilityConfigSource.data = { ReportName: $scope.SearchReportName };
    }

    $scope.UpRowCellRenderer = function (row, column, value) {
        return "Up";
    };

    $scope.UpRow = function (row) {
        //alert(row);
        if (row != 0) {
            var currentrow = $scope.GridTemplateMapping.getrowdata(row);
            var previousrow = $scope.GridTemplateMapping.getrowdata(row - 1);
            if (currentrow.Id != 0) {
                for (var i = 0; i < TemplateMappingData.length; i++) {
                    if (currentrow.Id == TemplateMappingData[i].Id) {
                        TemplateMappingData[i].OrderNo = currentrow.boundindex;
                    }
                }
            }

            if (previousrow.Id != 0) {
                for (j = 0; j < TemplateMappingData.length; j++) {
                    if (previousrow.Id == TemplateMappingData[j].Id) {
                        TemplateMappingData[j].OrderNo = previousrow.boundindex + 2;
                    }
                }
            }
            $scope.$apply(function () {
                $scope.TemplateMappingSource.localdata = TemplateMappingData.sort(function (a, b) {
                    if (a.OrderNo < b.OrderNo) //sort string ascending
                        return -1
                    if (a.OrderNo > b.OrderNo)
                        return 1
                    return 0 //default return value (no sorting)
                });
            });
            $scope.GridTemplateMapping.updatebounddata();
        }
    }

    $scope.DownRowCellRenderer = function (row, column, value) {
        return "Down";
    };
    $scope.DownRow = function (row) {
        if (row !=(TemplateMappingData.length - 1)) {
            var currentRow = $scope.GridTemplateMapping.getrowdata(row);
            var nextRow = $scope.GridTemplateMapping.getrowdata(row + 1);
            if (currentRow.Id != 0) {
                for (var i = 0; i < TemplateMappingData.length; i++) {
                    if (TemplateMappingData[i].Id == currentRow.Id) {
                        TemplateMappingData[i].OrderNo = currentRow.boundindex + 2;
                    }
                }
            }

            if (nextRow.Id != 0) {
                for (var j = 0; j < TemplateMappingData.length; j++) {
                    if (TemplateMappingData[j].Id == nextRow.Id) {
                        TemplateMappingData[j].OrderNo = currentRow.boundindex + 1;
                    }
                }
            }
            $scope.$apply(function () {
                $scope.TemplateMappingSource.localdata = TemplateMappingData.sort(function (a, b) {
                    if (a.OrderNo < b.OrderNo) //sort string ascending
                        return -1
                    if (a.OrderNo > b.OrderNo)
                        return 1
                    return 0 //default return value (no sorting)
                });
            });
            $scope.GridTemplateMapping.updatebounddata();
        }
      
    }

    var ClearData = function (e) {
        $scope.ReportValidator.hide();
        $scope.UtilityColumnValidator.hide();
        columnId = 0;
        //$scope.model.Id = 0;
        $scope.model.ColumnName = null;
        $scope.dllDatatype.selectIndex(-1);
        $scope.model.GroupName = null;
        $scope.model.OrderNo = null;
        $scope.model.IsRequired = false;
        $scope.model.FixColumnId = null;
        $scope.model.IsAggregate = false;
        $scope.dllAggregateype.selectIndex(0);
        $scope.model.IsSearchColumn = false;
        $scope.model.IsEditable = false;
        $scope.model.OnChangeFormula = null;
        $scope.model.IsAutoGenerated = false;
        $scope.model.IsOrderBy = false;
        //$scope.txtWidth.clear();
        $scope.model.Width = null;
        $scope.model.DefaultValue = null;
        $scope.IsAggregateFunction = false;
    }

});