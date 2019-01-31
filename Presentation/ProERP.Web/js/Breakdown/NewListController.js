ProApp.controller("BreakdownListController", function ($scope, $http) {

    $scope.ServiceRules = [{ input: '#txtVendorName', message: 'Vendor name is required!', action: 'blur,change', rule: 'required' }];
    $scope.ManPowerRules = [{ input: '#txtEmployeeName', message: 'Employee name is required!', action: 'blur,change', rule: 'required' },
                            {
                                input: '#EmployeeType', message: 'Please choose employee type', action: 'change',
                                rule: function (input, commit) {
                                    var index = $scope.ddlEmployeeType.getSelectedIndex();
                                    return index != -1;
                                }
                            }
    ];
    $scope.PartRules = [{
        input: '#partId', message: 'Please choose part', action: 'change',
        rule: function (input, commit) {
            var index = $scope.ddlpart.getSelectedIndex();
            return index != -1;
        }
    }];
    $scope.PartNameRules = [{ input: '#txtPartName', message: 'Part name is required!', action: 'blur,change', rule: 'required' }];

    $scope.MenPowerItem = { HourlyRate: 100, EmployeeType: {} };
    $scope.PartItem = { Part: {} }

    $scope.SearchDateCreated = function (args) {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var firstDay = new Date(y, m, 1);
        var lastDay = new Date(y, m + 1, 0);
        dateTimeInput = args.instance;
        dateTimeInput.setRange(firstDay, lastDay);
    }

    var sortDataField = null;
    var sortOrder = null;

    var from = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        from = $scope.dtpSearchDate.getRange().from;

    var to = new Date();
    if (typeof ($scope.dtpSearchDate) == 'object')
        to = $scope.dtpSearchDate.getRange().to;
    var allMachineData = [];
    var DeletedServiceId = [];
    var DeletedMenPowerId = [];
    var DeletedPartId = [];
    var DeletedAttId = [];

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

    $scope.onPlantChange = function (e) {
        $scope.LineSource.data = { PlantId: $scope.ddlPlant.val() };
    }

    $scope.onLineChange = function (e) {
        $.ajax({
            dataType: "json",
            type: 'GET',
            contentType: "application/json;",
            url: '/Breakdown/GetMachineListForLine',
            data: { lineId: $scope.ddlLine.val() },
            success: function (response) {
                allMachineData = response.MachineData;
                $scope.$apply(function (e) {
                    RebindGrid();
                });
            },
            error: function (response) {
                //alert(JSON.stringify(response));
            }
        });
    }



    $scope.BreakdownColumns = [
       {
           text: '#', sortable: false, filterable: false, editable: false,
           groupable: false, draggable: false, resizable: false, pinned: true,
           datafield: '', columntype: 'number', width: 50,
           cellsrenderer: function (row, column, value) {
               return "<div style='margin:4px;'>" + (value + 1) + "</div>";
           }
       },
       { text: 'History', columntype: 'checkbox', datafield: 'IsHistory', width: 50, editable: true, pinned: true },
       { text: 'Repeated', columntype: 'checkbox', datafield: 'IsRepeated', width: 70, editable: true, pinned: true },
       { text: 'Major', columntype: 'checkbox', datafield: 'IsMajor', width: 50, editable: true, pinned: true },
       {
           text: 'Machine', datafield: 'MachineId', displayfield: 'MachineName', width: 150, sortable: false, columntype: 'combobox',
           createeditor: function (row, cellvalue, editor, celltext, pressedChar) {
               var data = this.owner.getrowdata(row);
               editor.jqxComboBox({ theme: $scope.theme, autoComplete: true, dropDownHeight: 250, source: allMachineData, displayMember: 'MachineName', valueMember: 'MachineId', searchMode: 'containsignorecase' });
               editor.jqxComboBox('val', celltext);
           },
           cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {

               if (oldvalue == newvalue.value)
                   return;

               var data = this.owner.getrowdata(rowindex);
               data.SubAssemblyId = 0;
               data.SubAssemblyName = '';
           }
       },
       { text: 'Date', datafield: 'Date', width: 80, align: 'center', sortable: true, columntype: 'datetimeinput', cellsformat: 'MM/dd/yyyy' },
                                {
                                    text: 'Start', datafield: 'StartTime', columngroup: 'Time', width: 50, align: 'center', columntype: 'datetimeinput', cellsformat: "HH:mm", sortable: true,
                                    createeditor: function (row, value, editor) {
                                        editor.jqxDateTimeInput({ showTimeButton: false, showCalendarButton: false, formatString: 'HH:mm' });
                                        //editor.jqxMaskedInput({ width: 50, height: 25, mask: '[0-2][0-9]:[0-5][0-9]' });
                                        //editor.jqxNumberInput({
                                        //    width: 50, height: 25, spinButtons: false, decimalDigits: 0,
                                        //    allowNull: false, digits: 4, groupSeparator: '', min: 0000, max: 2400, theme: $scope.theme
                                        //});
                                    },
                                    cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
                                        if (newvalue == "") {
                                            return oldvalue;
                                        }
                                        var data = this.owner.getrowdata(rowindex);
                                        if (typeof (data.StopTime) != 'undefined' && data.StopTime != null)
                                            data.TotalTime = data.StopTime - newvalue;
                                    }, validation: function (cell, value) {
                                        if (value == null || value == "") {
                                            return { result: false, message: "Start time is required." };
                                        }

                                        var data = this.owner.getrowdata(cell.row);
                                        if (data.StopTime == null)
                                            return true;

                                        var data = this.owner.getrowdata(cell.row);
                                        var diff = data.StopTime - value;
                                        if (diff < 0) {
                                            return { result: false, message: "Start time should be less then stop time." };
                                        }
                                        return true;
                                    }
                                },
                                {
                                    text: 'Stop', datafield: 'StopTime', columngroup: 'Time', width: 50, align: 'center', columntype: 'datetimeinput', cellsformat: "HH:mm", sortable: false
                                    , createeditor: function (row, value, editor) {
                                        editor.jqxDateTimeInput({ showTimeButton: false, showCalendarButton: false, formatString: 'HH:mm' });
                                    },
                                    cellvaluechanging: function (rowindex, datafield, columntype, oldvalue, newvalue) {
                                        if (newvalue == "") {
                                            return oldvalue;
                                        }
                                        var data = this.owner.getrowdata(rowindex);
                                        if (typeof (data.StartTime) != 'undefined' && data.StartTime != null) {
                                            data.TotalTime = newvalue - data.StartTime;
                                        }
                                    }, validation: function (cell, value) {
                                        if (value == null || value == "") {
                                            return { result: false, message: "Stop time is required." };
                                        }

                                        var data = this.owner.getrowdata(cell.row);
                                        var diff = value - data.StartTime;
                                        if (diff < 0) {
                                            return { result: false, message: "Stop time should be greater then start time." };
                                        }
                                        return true;
                                    }
                                },
                                {
                                    text: 'Total', datafield: 'TotalTime', columngroup: 'Time', width: 45, align: 'center', editable: false, sortable: false,
                                    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                                        if (typeof (value) === 'undefined' || value == null || value == '')
                                            return null;
                                        var ms = Math.abs(value);
                                        // 1- Convert to seconds:
                                        var seconds = ms / 1000;
                                        // 2- Extract hours:
                                        var hours = parseInt(seconds / 3600); // 3,600 seconds in 1 hour
                                        seconds = seconds % 3600; // seconds remaining after extracting hours
                                        // 3- Extract minutes:
                                        var minutes = parseInt(seconds / 60); // 60 seconds in 1 minute
                                        // 4- Keep only seconds not extracted to minutes:
                                        seconds = seconds % 60;
                                        if (value >= 0)
                                            return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + '; color: #0000ff;">' + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + '</span>';
                                        else
                                            return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + '; color: #ff0000;">' + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + '</span>';
                                        //return hours + ":" + minutes;
                                    }
                                },
                               { text: 'Descriptoin of Failure', datafield: 'FailureDescription', align: 'center', width: 300, columntype: 'textbox', sortable: false, editable: true },
                               { text: 'Elec.', datafield: 'ElecticalTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { text: 'Mech.', datafield: 'MechTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { text: 'Instr.', datafield: 'InstrTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { text: 'Util', datafield: 'UtilityTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { text: 'Power', datafield: 'PowerTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { text: 'Proc.', datafield: 'ProcessTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { text: 'Prv', datafield: 'PrvTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { text: 'Idle', datafield: 'IdleTime', columngroup: 'TimeHrsMin', columntype: 'checkbox', width: 50, align: 'center', editable: true, sortable: false },
                               { hidden: true, text: 'Done By', datafield: 'DoneBy', align: 'center', width: 200, columngroup: 'ResolutionDetails' },
                               { text: 'Root Cause', datafield: 'RootCause', align: 'center', width: 200, columngroup: 'ResolutionDetails', columntype: 'textbox', sortable: false, },
                               { text: 'Correction', datafield: 'Correction', align: 'center', width: 200, columngroup: 'ResolutionDetails', columntype: 'textbox', sortable: false },
                               { text: 'Corrective Action', datafield: 'CorrectiveAction', align: 'center', width: 200, columngroup: 'ResolutionDetails', columntype: 'textbox', sortable: false },
                               { text: 'Preventive Action', datafield: 'PreventingAction', align: 'center', width: 200, columngroup: 'ResolutionDetails', columntype: 'textbox', sortable: false },
                               {
                                   text: 'Spares Used', columngroup: 'ResolutionDetails', columntype: 'button', sortable: false, cellsrenderer: function () { return "View"; }
                                    , buttonclick: function (editrow) {
                                        // get the clicked row's data and initialize the input fields.
                                        var dataRecord = this.owner.getrowdata(editrow); //.jqxGrid('getrowdata', editrow);
                                        $scope.BreakDownId = dataRecord.Id;
                                        $scope.LineId = dataRecord.LineId;

                                        $.ajax({
                                            dataType: "json",
                                            type: 'GET',
                                            url: '/Breakdown/GetSparesData',
                                            data: { breakDownId: dataRecord.Id },
                                            success: function (response) {
                                                //Service
                                                for (i = 0; i < response.Data.ServiceData.length; i++) {
                                                    var serviceItem = response.Data.ServiceData[i];

                                                    var existingServiceItem = $.grep($scope.ServiceData, function (item, i) {
                                                        return item.Id == serviceItem.Id;
                                                    });

                                                    if (existingServiceItem.length == 0)
                                                        $scope.ServiceData.push(serviceItem);
                                                }
                                                //MenPower
                                                for (i = 0; i < response.Data.MenPowerData.length; i++) {
                                                    var MenPowerItem = response.Data.MenPowerData[i];

                                                    var existingMenPowerItem = $.grep($scope.MenPowerData, function (item, i) {
                                                        return item.Id == MenPowerItem.Id;
                                                    });

                                                    if (existingMenPowerItem.length == 0)
                                                        $scope.MenPowerData.push(MenPowerItem);
                                                }
                                                //Part
                                                for (i = 0; i < response.Data.PartData.length; i++) {
                                                    var PartItem = response.Data.PartData[i];

                                                    var existingPartItem = $.grep($scope.PartData, function (item, i) {
                                                        return item.Id == PartItem.Id;
                                                    });

                                                    if (existingPartItem.length == 0)
                                                        $scope.PartData.push(PartItem);
                                                }
                                                //Attachments
                                                for (i = 0; i < response.Data.AttachmentData.length; i++) {
                                                    var AttachmentItem = response.Data.AttachmentData[i];

                                                    var existingAttachmentItem = $.grep($scope.AttachmentData, function (item, i) {
                                                        return item.Id == AttachmentItem.Id;
                                                    });

                                                    if (existingAttachmentItem.length == 0)
                                                        $scope.AttachmentData.push(AttachmentItem);
                                                }

                                                $scope.$apply(function (e) {
                                                    $scope.ServiceGridSource.localdata = $.grep($scope.ServiceData, function (item, i) {
                                                        return parseInt(item.BreakDownId) == parseInt($scope.BreakDownId);

                                                    });
                                                    $scope.MenPoweGridSource.localdata = $.grep($scope.MenPowerData, function (item, i) {
                                                        return parseInt(item.BreakDownId) == parseInt($scope.BreakDownId);

                                                    });
                                                    $scope.PartGridSource.localdata = $.grep($scope.PartData, function (item, i) {
                                                        return parseInt(item.BreakDownId) == parseInt($scope.BreakDownId);

                                                    });
                                                    $scope.AttachmentGridSource.localdata = $.grep($scope.AttachmentData, function (item, i) {
                                                        return parseInt(item.BreakDownId) == parseInt($scope.BreakDownId);

                                                    });
                                                });
                                            }
                                        });
                                        ClearData();
                                        $('#liService').addClass('active');
                                        $('#liMenPower').removeClass('active');
                                        $('#liPart').removeClass('active');
                                        $('#liAttachment').removeClass('active');
                                        $('#tab1').addClass('active');
                                        $('#tab2').removeClass('active');
                                        $('#tab3').removeClass('active');
                                        $('#tab4').removeClass('active');
                                        $scope.WinSpares.open();
                                    }
                               }
    ];

    $scope.BreakdownGroupColumns = [{ text: 'Time', align: 'center', name: 'Time' },
                                    { text: 'Time in Hrs and Min', align: 'center', name: 'TimeHrsMin' },
                                    { text: 'Resolution Details', align: 'center', name: 'ResolutionDetails' }];


    var oldData = {};

    $scope.GridSource = {
        cache: false,
        dataType: "json",
        autoBind: false,
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'IsHistory', type: 'bool' },
            { name: 'IsRepeated', type: 'bool' },
            { name: 'IsMajor', type: 'bool' },
            { name: 'PlantId', type: 'int' },
            { name: 'LineId', type: 'int' },
            { name: 'MachineId', type: 'int' },
            { name: 'MachineName', type: 'string' },
            { name: 'SubAssemblyId', type: 'int' },
            { name: 'SubAssemblyName', type: 'string' },
            { name: 'Date', type: 'date', format: 'MM/dd/yyyy' },
            { name: 'StartTime', type: 'date', format: 'HH:mm' },
            { name: 'StopTime', type: 'date', format: 'HH:mm' },
            { name: 'TotalTime', type: 'int' },
            { name: 'FailureDescription', type: 'string' },
            { name: 'ElecticalTime', type: 'bool' },
            { name: 'MechTime', type: 'bool' },
            { name: 'InstrTime', type: 'bool' },
            { name: 'UtilityTime', type: 'bool' },
            { name: 'PowerTime', type: 'bool' },
            { name: 'ProcessTime', type: 'bool' },
            { name: 'PrvTime', type: 'bool' },
            { name: 'IdleTime', type: 'bool' },
            { name: 'ResolveTimeTaken', type: 'date', format: 'HH:mm' },
            { name: 'SpareTypeId', type: 'int' },
            { name: 'SpareTypeName', type: 'int' },
            { name: 'SpareDescription', type: 'string' },
            { name: 'DoneBy', type: 'string' },
            { name: 'RootCause', type: 'string' },
            { name: 'Correction', type: 'string' },
            { name: 'CorrectiveAction', type: 'string' },
            { name: 'PreventingAction', type: 'string' }
        ],
        url: '/Breakdown/GetGridData',
        data: {
            siteId: 0,
            plantId: 0,
            lineId: 0,
            fromDate: from.toISOString(),
            toDate: to.toISOString(),
            sortdatafield: sortDataField,
            sortorder: sortOrder
        },
        Id: 'Id',
        // update the grid and send a request to the server.
        sort: function (sortdatafield, sortorder, e) {
            sortDataField = sortdatafield;
            sortOrder = sortorder;
            $scope.gridBreakdown.updatebounddata();
        },
        formatData: function (data, e) {
            data.sortdatafield = sortDataField;
            data.sortorder = sortOrder;
        },
        updaterow: function (rowid, newdata, commit) {
            var IsElecTime = (newdata.ElecticalTime != oldData.ElecticalTime);
            var IsMechTime = (newdata.MechTime != oldData.MechTime);
            var IsInstrTime = (newdata.InstrTime != oldData.InstrTime);
            var IsUtilityTime = (newdata.UtilityTime != oldData.UtilityTime);
            var IsPowerTime = (newdata.PowerTime != oldData.PowerTime);
            var IsProcessTime = (newdata.ProcessTime != oldData.ProcessTime);
            var IsPrvTime = (newdata.PrvTime != oldData.PrvTime);
            var IsIdleTime = (newdata.IdleTime != oldData.IdleTime);

            if (IsElecTime || IsMechTime || IsInstrTime || IsUtilityTime || IsPowerTime || IsProcessTime || IsPrvTime || IsIdleTime) {
                if ((IsElecTime && !newdata.ElecticalTime) || (IsMechTime && !newdata.MechTime) || (IsInstrTime && !newdata.InstrTime) || (IsUtilityTime && !newdata.UtilityTime)
                    || (IsPowerTime && !newdata.PowerTime) || (IsProcessTime && !newdata.ProcessTime) || (IsPrvTime && !newdata.PrvTime) || (IsIdleTime && !newdata.IdleTime)) {
                    return commit(false);
                }

                oldData.ElecticalTime = IsElecTime;
                oldData.MechTime = IsMechTime;
                oldData.InstrTime = IsInstrTime;
                oldData.UtilityTime = IsUtilityTime;
                oldData.PowerTime = IsPowerTime;
                oldData.ProcessTime = IsProcessTime;
                oldData.PrvTime = IsPrvTime;
                oldData.IdleTime = IsIdleTime;

                newdata.ElecticalTime = IsElecTime;
                newdata.MechTime = IsMechTime;
                newdata.InstrTime = IsInstrTime;
                newdata.UtilityTime = IsUtilityTime;
                newdata.PowerTime = IsPowerTime;
                newdata.ProcessTime = IsProcessTime;
                newdata.PrvTime = IsPrvTime;
                newdata.IdleTime = IsIdleTime;

            }
            commit(true);
        }
    };

    $scope.OnCellBeginEdit = function (event) {
        // event arguments.
        var args = event.args;
        // row's data.
        oldData = jQuery.extend(true, {}, args.row);
    }

    $scope.onSearchClick = function (event) {
        RebindGrid();
    }

    var RebindGrid = function () {
        var selection = $scope.dtpSearchDate.getRange();
        var siteId = 0;
        var plantId = parseInt($scope.ddlPlant.val());
        var lineId = parseInt($scope.ddlLine.val());
        $scope.GridSource.data = { siteId: siteId, plantId: plantId, lineId: lineId, fromDate: selection.from.toISOString(), toDate: selection.to.toISOString(), sortdatafield: "", sortorder: "" };
    }

    $scope.onSaveClick = function (e) {

        var rows = $scope.gridBreakdown.getrows();

        var breakDownData = $.grep(rows, function (a) {
            return a.MachineId > 0 && a.Date != null && a.StartTime != null
            && a.StopTime != null && a.TotalTime >= 0;
        })

        //if (breakDownData.length != 0)
        //{
        //    alert("please add record then after you can save the record");
        //}

        var postData = [];
        for (i = 0; i < breakDownData.length; i++) {
            var item = breakDownData[i];
            if (item.SubAssemblyId <= 0)
                item.SubAssemblyId = null;
            //if (item.Id = 'undefined')
            //    item.Id = -1;
            var postItem = {
                Id: item.Id, IsHistory: item.IsHistory, IsRepeated: item.IsRepeated, IsMajor: item.IsMajor, PlantId: item.PlantId, LineId: parseInt($scope.ddlLine.val()), MachineId: item.MachineId, MachineName: item.MachineName,
                SubAssemblyId: item.SubAssemblyId, SubAssemblyName: item.SubAssemblyName, Date: item.Date.toISOString(), StartTime: item.StartTime.toISOString(),
                StopTime: item.StopTime.toISOString(), TotalTime: item.TotalTime, FailureDescription: item.FailureDescription, ElecticalTime: item.ElecticalTime,
                MechTime: item.MechTime, InstrTime: item.InstrTime, UtilityTime: item.UtilityTime, PowerTime: item.PowerTime, ProcessTime: item.ProcessTime,
                PrvTime: item.PrvTime, IdleTime: item.IdleTime, SpareTypeId: item.SpareTypeId, SpareTypeName: item.SpareTypeName, SpareDescription: '',
                DoneBy: item.DoneBy, RootCause: item.RootCause, Correction: item.Correction, CorrectiveAction: item.CorrectiveAction, PreventingAction: item.PreventingAction
            };

            if (item.ResolveTimeTaken != null)
                postItem.ResolveTimeTaken = item.ResolveTimeTaken.toISOString();

            postData.push(postItem);
        }

        $.ajax({
            dataType: "json",
            type: 'POST',
            url: '/Breakdown/SaveBreakdownData',
            data: {
                breakDownData: postData, ServiceData: $scope.ServiceData, MenPowerData: $scope.MenPowerData, PartData: $scope.PartData, AttachmentData: $scope.AttachmentData
                , DeletedServiceIds: DeletedServiceId, DeletedMenPowerIds: DeletedMenPowerId, DeletedPartIds: DeletedPartId, DeletedAttIds: DeletedAttId
            },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.openMessageBox('Success', response.Message, 200, 90);
                    $scope.gridBreakdown.updatebounddata();
                    $scope.ServiceData = [];
                    $scope.MenPowerData = [];
                    $scope.PartData = [];
                    $scope.AttachmentData = [];
                    DeletedServiceId = [];
                    DeletedMenPowerId = [];
                    DeletedPartId = [];
                    DeletedAttId = [];
                }
                else {
                    $scope.openMessageBox('Error', response.Message, 'auto', 'auto');
                }
            },
            error: function (jqXHR, exception) {
            }
        });
    };

    $scope.onAddNewClick = function (e) {
        var gridRows = $scope.gridBreakdown.getrows();
        var minId = Math.min.apply(Math, gridRows.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;
        var rows = new Array();
        for (var i = 0; i < 5; i++) {
            minId -= 1;
            var datarow = { Id: minId, PlantId: $scope.ddlPlant.val(), LineId: $scope.ddlLine.val(), ElecticalTime: false, MechTime: false, InstrTime: false, UtilityTime: false, PowerTime: false, ProcessTime: false, PrvTime: false, IdleTime: true };
            rows.push(datarow);
        }

        $scope.gridBreakdown.addrow(null, rows);
    }

    $scope.onRefreshClick = function (e) {
        $scope.gridBreakdown.updatebounddata();
    }

    $scope.onExportClick = function (e) {
        var selection = $scope.dtpSearchDate.getRange();
        var siteId = 0;
        var plantId = $scope.ddlPlant.val();
        var lineId = $scope.ddlLine.val();
        var fromDate = selection.from.toISOString();
        var toDate = selection.to.toISOString();
        var sortdatafield = sortDataField;
        var sortorder = sortOrder;
        if (plantId == "" || plantId == "undefined" && lineId == "" || lineId == "undefined")
            $scope.openMessageBox("Warning", "Please choose plant and line.", 200, 90);
        else {
            //checkChangesEnabled = false;
            window.location = '/Download/BreakdownList?siteId=' + siteId + '&plantId=' + plantId + '&lineId=' + lineId + '&fromDate=' + fromDate + '&toDate=' + toDate + '&sortdatafield=' + sortdatafield + '&sortorder=' + sortorder;
        }

    }

    $scope.onDeleteClick = function (e) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete selected item(s)?', 350, 100, function (isYes) {
            if (isYes) {
                var rows = $scope.gridBreakdown.selectedrowindexes;
                var selectedIds = [];
                for (var m = 0; m < rows.length; m++) {
                    var row = $scope.gridBreakdown.getrowdata(rows[m]);
                    if (row != null && row.Id > 0)
                        selectedIds.push(row.Id);
                }
                if (selectedIds.length == 0) {
                    $scope.gridBreakdown.clearselection();
                    return;
                }
                $.ajax({
                    dataType: "json",
                    type: 'POST',
                    url: '/Breakdown/DeleteBreakdownData',
                    data: {
                        Ids: selectedIds
                    },
                    success: function (response) {
                        $scope.openMessageBox('Success', 'Data deleted successfully.', 200, 90);
                        $scope.gridBreakdown.updatebounddata();
                        $scope.gridBreakdown.clearselection();
                        $scope.ServiceData = [];
                        $scope.MenPowerData = [];
                        $scope.PartData = [];
                        DeletedServiceId = [];
                        DeletedMenPowerId = [];
                        DeletedPartId = [];
                        DeletedAttId = [];
                    },
                    error: function (jqXHR, exception) {
                    }
                });
            }
        });
    }

    $scope.PartSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Breakdown/GetPartList',
    };
    $scope.EmployeeTypeSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Type', type: 'string' }
        ],
        url: '/Breakdown/GetEmployeeTypeList',
    };
    $scope.ServiceGridSource = {
        datatype: "json",
        localdata: $scope.ServiceData,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BreakDownId', type: 'int' },
           { name: 'VendorName', type: 'string' },
           { name: 'Cost', type: 'float' }
        ],
    };
    $scope.MenPoweGridSource = {
        datatype: "json",
        localdata: $scope.MenPowerData,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BreakDownId', type: 'int' },
           { name: 'Name', type: 'string' },
           { name: 'EmployeeType', type: 'string' },
           { name: 'EmployeeTypeId', type: 'int' },
           { name: 'HourlyRate', type: 'float' },
           { name: 'IsOverTime', type: 'bool' },

        ],
    };
    $scope.PartGridSource = {
        datatype: "json",
        localdata: $scope.PartData,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BreakDownId', type: 'int' },
           { name: 'PartId', type: 'int' },
           { name: 'PartName', type: 'string' },
           { name: 'Comments', type: 'string' },
           { name: 'Quantity', type: 'int' }
        ],
    };
    $scope.AttachmentGridSource = {
        datatype: "json",
        localdata: $scope.AttachmentData,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'BreakDownId', type: 'int' },
           { name: 'OriginalFileName', type: 'string' },
        ],
    };

    $scope.ServiceData = [];
    $scope.MenPowerData = [];
    $scope.PartData = [];
    $scope.AttachmentData = [];
    var DeletedServiceId = [];
    var DeletedMenPowerId = [];
    var DeletedPartId = [];
    var DeletedAttId = [];

    $scope.AddServiceClick = function (event) {
        var IsValidate = $scope.ServiceValidator.validate();
        if (!IsValidate)
            return;
        var minId = Math.min.apply(Math, $scope.ServiceData.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;
        $scope.ServiceItem.Id = minId - 1;
        $scope.ServiceItem.BreakDownId = $scope.BreakDownId;
        $scope.ServiceData.push({ Id: $scope.ServiceItem.Id, BreakDownId: $scope.ServiceItem.BreakDownId, VendorName: $scope.ServiceItem.VendorName, Cost: $scope.ServiceItem.Cost, Comments: $scope.ServiceItem.Comments });
        $scope.ServiceGridSource.localdata = $.grep($scope.ServiceData, function (item, i) {
            return item.BreakDownId == $scope.BreakDownId;
        });
        $scope.GrdService.updatebounddata();
        ClearData();
    };
    $scope.AddMenPowerClick = function (event) {
        var IsValidate = $scope.ManPowerValidator.validate();
        if (!IsValidate)
            return;
        var minId = Math.min.apply(Math, $scope.MenPowerData.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;
        $scope.MenPowerItem.Id = minId - 1;
        $scope.MenPowerItem.BreakDownId = $scope.BreakDownId;
        if (!$scope.checked)
            $scope.MenPowerItem.HourlyRate = 0;
        $scope.MenPowerData.push({
            Id: $scope.MenPowerItem.Id, BreakDownId: $scope.MenPowerItem.BreakDownId, Name: $scope.MenPowerItem.Name,
            EmployeeTypeId: $scope.MenPowerItem.EmployeeType.Id, EmployeeType: $scope.MenPowerItem.EmployeeType.Type,
            HourlyRate: $scope.MenPowerItem.HourlyRate, Comments: $scope.MenPowerItem.Comments, IsOverTime: $scope.checked
        });
        $scope.MenPoweGridSource.localdata = $.grep($scope.MenPowerData, function (item, i) {
            return item.BreakDownId == $scope.BreakDownId;
        });
        $scope.GrdService.updatebounddata();
        ClearData();

    };
    $scope.AddPartClick = function (event) {
        var IsValidate = $scope.PartValidator.validate();
        if (!IsValidate)
            return;
        var minId = Math.min.apply(Math, $scope.PartData.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;
        $scope.PartItem.Id = minId - 1;
        $scope.PartItem.BreakDownId = $scope.BreakDownId;
        if (typeof ($scope.PartItem.Part.Id) === "undefined") {
            $scope.PartItem.Part.Id = $scope.PartItem.Part.value;
            $scope.PartItem.Part.Name = $scope.PartItem.Part.label;
        }
        $scope.PartData.push({
            Id: $scope.PartItem.Id,
            BreakDownId: $scope.PartItem.BreakDownId,
            PartId: $scope.PartItem.Part.Id,
            PartName: $scope.PartItem.Part.Name,
            Comments: $scope.PartItem.Comments,
            Quantity: $scope.PartItem.Quantity

        });
        $scope.PartGridSource.localdata = $.grep($scope.PartData, function (item, i) {
            return item.BreakDownId == $scope.BreakDownId;
        });
        $scope.GrdPart.updatebounddata();
        ClearData();
    };

    $scope.AddNewPartClick = function (event) {
        $scope.Mainwindow.open();
    };

    $scope.ServiceDelete = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete Service Item?', 350, 100, function (e) {
            if (e) {
                var dataRecord = $scope.GrdService.getrowdata(row);
                DeletedServiceId.push(dataRecord.Id);

                $scope.ServiceData = $.grep($scope.ServiceData, function (item, i) {
                    return item.Id != dataRecord.Id;
                });

                $scope.ServiceGridSource.localdata = $.grep($scope.ServiceData, function (item, i) {
                    return item.BreakDownId == $scope.BreakDownId;
                });

                $scope.GrdService.updatebounddata();
            }
            else {

            }
        });
    }
    $scope.MenPowerDelete = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete MenPower Item?', 350, 100, function (e) {
            if (e) {
                var dataRecord = $scope.GrdMenPower.getrowdata(row);
                DeletedMenPowerId.push(dataRecord.Id);

                $scope.MenPowerData = $.grep($scope.MenPowerData, function (item, i) {
                    return item.Id != dataRecord.Id;
                });

                $scope.MenPoweGridSource.localdata = $.grep($scope.MenPowerData, function (item, i) {
                    return item.BreakDownId == $scope.BreakDownId;
                });

                $scope.GrdMenPower.updatebounddata();
            }
            else {

            }
        });
    }
    $scope.PartDelete = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete Part Item?', 350, 100, function (e) {
            if (e) {
                var dataRecord = $scope.GrdPart.getrowdata(row);
                DeletedPartId.push(dataRecord.Id);

                $scope.PartData = $.grep($scope.PartData, function (item, i) {
                    return item.Id != dataRecord.Id;
                });

                $scope.PartGridSource.localdata = $.grep($scope.PartData, function (item, i) {
                    return item.BreakDownId == $scope.BreakDownId;
                });

                $scope.GrdPart.updatebounddata();
            }
            else {

            }
        });
    }
    $scope.ViewImageClick = function (row) {
        var dataRecord = $scope.GrdAttachment.getrowdata(row);
        var fileName = dataRecord.OriginalFileName;
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);

        if (fileExtension.toLowerCase() == 'jpg' || fileExtension.toLowerCase() == 'jpeg' || fileExtension.toLowerCase() == 'png') {
            $.ajax({
                url: "/Breakdown/GetImageData",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { Id: dataRecord.Id },
                success: function (response) {
                    //$scope.$apply(function (e) {
                    if (response.Data != null) {
                        $('#spanTitle').text(dataRecord.OriginalFileName);
                        $('#imgView').attr('src', 'data:image/png;base64,' + response.Data);
                        //$scope.ImageOriginalName = dataRecord.OriginalFileName;
                        //$scope.ImageData = response.Data;
                        $scope.ImgWindow.open();
                        //});
                        //$('#imgModal').modal('show')
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
        }
        else {
            if (dataRecord.Id > 0) {
                window.location.href = "/Breakdown/DownloadBreakDownAttachmentFile/?Id=" + dataRecord.Id;
            }
        }

       
    }

    $scope.DeleteAttachment = function (row) {
        $scope.openConfirm("Confirmation", 'Are you sure you want to delete Service Item?', 350, 100, function (e) {
            if (e) {
                var dataRecord = $scope.GrdAttachment.getrowdata(row);
                DeletedAttId.push(dataRecord.Id);

                $scope.AttachmentData = $.grep($scope.AttachmentData, function (item, i) {
                    return item.Id != dataRecord.Id;
                });

                $scope.AttachmentGridSource.localdata = $.grep($scope.AttachmentData, function (item, i) {
                    return item.BreakDownId == $scope.BreakDownId;
                });

                $scope.GrdAttachment.updatebounddata();
            }
            else {

            }
        });
    }
    $scope.onNewPartSave = function (event) {
        var IsValidate = $scope.PartNameValidator.validate();
        if (!IsValidate)
            return;
        //Ajax call to save
        $.ajax({
            url: "/Breakdown/SavePartData",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Name: $scope.Name, Description: $scope.Description },
            success: function (response) {
                $scope.Mainwindow.close();
                //PartdataAdapter.dataBind();
                $scope.ddlpart.addItem(response.Data);
                $scope.ddlpart.val(response.Data.Id);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    };

    $scope.DeleteServiceCellRenderer = function (row, column, value) {

        return "Delete";
    };
    $scope.DeleteMenPowerCellRenderer = function (row, column, value) {

        return "Delete";
    };
    $scope.DeletePartCellRenderer = function (row, column, value) {

        return "Delete";
    };
    $scope.DeleteAttachmentCellRenderer = function (row, column, value) {

        return "Delete";
    };
    $scope.DownloadAttachmentCellRenderer = function (row, column, value) {
        var dataRecord = $scope.GrdAttachment.getrowdata(row);
        var fileName = dataRecord.OriginalFileName;
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
        if (fileExtension.toLowerCase() == 'jpg' || fileExtension.toLowerCase() == 'jpeg' || fileExtension.toLowerCase() == 'png') {
            return "View";
        }
        else {
            return "Download";
        }
        
    };

    $scope.BDAttachment_UploadEnd = function (event) {
        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;
        var jsonResponse = JSON.parse(serverResponce);
        //$.merge(UploadFiles, jsonResponse);

        var minId = Math.min.apply(Math, $scope.AttachmentData.map(function (o) { return o.Id; }))
        if (minId > 0)
            minId = 0;
        var Id = minId - 1;
        $scope.BreakDownId = $scope.BreakDownId;
        $scope.AttachmentData.push({ Id: Id, BreakDownId: $scope.BreakDownId, OriginalFileName: jsonResponse[0].OriginalFileName, SysFileName: jsonResponse[0].SysFileName });
        $scope.AttachmentGridSource.localdata = $.grep($scope.AttachmentData, function (item, i) {
            return item.BreakDownId == $scope.BreakDownId;
        });
        $scope.GrdAttachment.updatebounddata();
        // Ajax call here...
        //$.ajax({
        //    url: "/Breakdown/SaveAttachments",
        //    type: "Post",
        //    //contentType: "application/json;",
        //    dataType: "json",
        //    data: { BreakDownId: $scope.BreakDownId, OriginalFileName: jsonResponse[0].OriginalFileName, SysFileName: jsonResponse[0].SysFileName },
        //    success: function (response) {
        //        if (response.Status == 1) {
        //            // Success
        //            $scope.GrdAttachment.updatebounddata();
        //        }
        //        else if (response.Status = 2) {
        //            // Warning
        //            $scope.openMessageBox('Warning', response.Message, 400, 100);
        //        }
        //        else {
        //            // Error
        //            $scope.openMessageBox('Error', response.Message, 400, 100);
        //        }
        //    },
        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //        alert('oops, something bad happened');
        //    }
        //});
    }

    var ClearData = function (e) {
        if (typeof ($scope.ServiceItem) == 'undefined')
            $scope.ServiceItem = {};
        if (typeof ($scope.MenPowerItem) == 'undefined')
            $scope.MenPowerItem = {};
        if (typeof ($scope.PartItem) == 'undefined')
            $scope.PartItem = {};

        //if ((typeof ($scope.ServiceItem) != 'undefined') && (typeof ($scope.MenPowerItem) != 'undefined') && (typeof ($scope.PartItem) != 'undefined')) {
        $scope.ServiceItem.VendorName = null;
        $scope.ServiceItem.Cost = 1;
        $scope.ServiceItem.Comments = null;
        $scope.MenPowerItem.Name = null;
        $scope.ddlpart.selectIndex(0);
        $scope.ddlEmployeeType.selectIndex(0);
        $scope.MenPowerItem.HourlyRate = null;
        $scope.MenPowerItem.Comments = null;
        $scope.PartItem.Comments = null;
        $scope.PartItem.Quantity = 0;
        UploadFiles = [];
        //}
    }

    $scope.GridCellValueChanged = function (event) {
        // event arguments.
        var args = event.args;
        // column data field.
        var datafield = event.args.datafield;
        // row's bound index.
        var rowBoundIndex = args.rowindex;
        // new cell value.
        var value = args.newvalue;
        // old cell value.
        var oldvalue = args.oldvalue;

        var dataItem = $scope.gridBreakdown.getrowdata(rowBoundIndex);

        if (value == true) {
            if (datafield == 'ElecticalTime') {
                dataItem.MechTime = false;
                dataItem.InstrTime = false;
                dataItem.UtilityTime = false;
                dataItem.PowerTime = false;
                dataItem.ProcessTime = false;
                dataItem.PrvTime = false;
                dataItem.IdleTime = false;
            }
        }
        else {
            if (dataItem.MechTime == false && dataItem.InstrTime == false && dataItem.UtilityTime == false && dataItem.PowerTime == false
                && dataItem.ProcessTime == false && dataItem.PrvTime == false && dataItem.IdleTime == false) {
                dataItem[datafield] = true;
            }
        }
        $scope.$apply();
    }

    $scope.onUploadClick = function (event) {
        var plantid = $scope.ddlPlant.val();
        var lineid = $scope.ddlLine.val();
        if (plantid == "" || lineid == "")
            $scope.openMessageBox("Warning", "Please choose plant and line.", 200, 90);
        else
            $scope.Uploadwindow.open();
    }

    var UploadFiles = [];
    $scope.SingleFile_UploadEnd = function (event) {

        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;
        var jsonResponse = JSON.parse(serverResponce);
        $.merge(UploadFiles, jsonResponse);
        var plantid = $scope.ddlPlant.val();
        var lineid = $scope.ddlLine.val();
        $.ajax({

            url: "/Breakdown/SaveExcelFile",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: {
                UploadFiles: UploadFiles, PlantId: plantid, LineId: lineid
            },
            success: function (response) {
                if (response.Status == 1) {
                    // Success
                    $scope.$apply(function () {
                        ClearData();
                        //$scope.ddlUploadFile.refresh();
                        $scope.Uploadwindow.close();
                        $scope.gridBreakdown.updatebounddata();
                        $scope.openMessageBox('Success', response.Message, 'auto', 'auto');
                    });
                }
                else {
                    // Error
                    ClearData();
                    //$scope.ddlUploadFile.refresh();
                    $scope.openMessageBox('Error', response.Message, 'auto', 'auto');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $scope.Uploadwindow.refresh();
                alert('oops, something bad happened');
            }
        });
    }

    $scope.onDownloadClick = function (event) {
        window.location = '/Download/BreakDownSampleFile';
    }
});