ProApp.controller("HomeController", function ($scope, $http, $timeout) {

    $scope.model = { SiteId: 0, PlantId: 0, LineId: 0, MachineId: 0, WorkStartDate: new Date() }

    var updateId = 0;
    var scheduleType = 0;
    var Interval = 0;
    var isOverDueWinOpen = false;
    var isShutDownOpen = false;
    $scope.IsBreakdown = false;
    $scope.IsBreakdownType = false;
    $scope.enableProperty = false;
    $scope.enableRadioBtn = false;
    $scope.IsAssigned = false;
    $scope.InProgress = false;
    $scope.IsCompelete = false;
    $scope.IsClose = false;
    $scope.IsRemarks = false;
    $scope.IsSave = false;
    $scope.IsDownload = false;

    $scope.ShowFilter = false;
    $scope.ShowFilterAt = false;
    $scope.ShowFilterAtline = false;
    $scope.ShowFilterAtmachine = false;
    $scope.ShowFiltershutdown = false;
    $scope.ShowFiltertoday = false;
    $scope.ShowFilternextweek = false;
    $scope.ShowFilterOpenMR = false;
    $scope.ShowFilterCompletedMR = false;
    $scope.ShowFilterAssignMR = false;
    $scope.ShowFilterProgressMR = false;
    $scope.ShowFilterOtherMR = false;
    $scope.IsCd = true;
    $scope.Rules = [
           { input: '#txtforNote', message: 'Overdue note is required!', action: 'blur', rule: 'required' }
    ];
    $scope.MaintanceRules = [
        {
            input: '#ddlUser', message: 'Please choose User', action: 'change',
            rule: function (input, commit) {
                if ($scope.model.StatusId == 1) {
                    var index = $scope.ddlUser.getSelectedIndex();
                    return index != -1;
                }
                else {
                    $scope.MRValidator.hide();
                    return true;
                }
            }
        },
        {
             input: '#dtWorkStartDate', message: 'Start Date is required!', action: 'valueChanged,blur',
             rule: function (input, commit) {
                 if ($scope.model.StatusId == 2) {
                     var sdate = $scope.dtWorkStartDate.getDate();
                     return sdate != null;
                 }
                 else {
                     $scope.MRValidator.hide();
                     return true;
                 }
             }
        },
        {
            input: '#dtWorkStartTime', message: 'Start time is required!', action: 'valueChanged,blur',
            rule: function (input, commit) {
                if ($scope.model.StatusId == 2) {
                    var sdate = $scope.dtWorkStartTime.getDate();
                    return sdate != null;
                }
                else {
                    $scope.MRValidator.hide();
                    return true;
                }
            }
        },
        {
            input: '#dtWorkEndDate', message: 'End Date is required!', action: 'valueChanged,blur',
            rule: function (input, commit) {
                if ($scope.model.StatusId == 3) {
                    var sdate = $scope.dtWorkEndDate.getDate();
                    return sdate != null;
                }
                else {
                    $scope.MRValidator.hide();
                    return true;
                }
            }
        },
        {
            input: '#dtWorkEndTime', message: 'End Time is required!', action: 'valueChanged,blur',
            rule: function (input, commit) {
                if ($scope.model.StatusId == 3) {
                    var sdate = $scope.dtWorkEndTime.getDate();
                    return sdate != null;
                }
                else {
                    $scope.MRValidator.hide();
                    return true;
                }
            }
        }
    ];

    $scope.GridColumns1 = [
                            { text: 'Plant', datafield: 'PlantName', editable: false },
                            { text: 'Line', datafield: 'LineName', editable: false },
                            { text: 'Machine Name', datafield: 'MachineName', editable: false },
                            { text: 'Work Description', datafield: 'WorkName', editable: false },
                            { text: 'Start Date', datafield: 'NextReviewDate', editable: false, width: 120, filtertype: 'range', cellsformat: 'MM/dd/yyyy' },
                            { text: 'Check Point', datafield: 'Checkpoints', editable: false },
                            { text: 'Schedule Name', datafield: 'ScheduleTypeName', editable: false, width: 120 },
                            { text: 'Frequency', datafield: 'Interval', editable: false, width: 100 },
                            { text: 'Severity', datafield: 'Severity', editable: false, width: 80 },
                            { text: 'Observation', datafield: 'IsObservation', editable: false, width: 80, columntype: 'checkbox' },
                            {
                                text: 'Task Done?',
                                width: 120,
                                columntype: 'button',
                                filterable: false,
                                sortable: false,
                                pinned: true,
                                cellsrenderer: function () {
                                    return "Done";
                                },
                                buttonclick: function (row) {
                                    var dataRecord = $scope.GridMain1.getrowdata(row);
                                    updateId = dataRecord.Id;
                                    scheduleType = dataRecord.ScheduleType;
                                    Interval = dataRecord.Interval;
                                    reviewDate = dataRecord.NextReviewDate;
                                    isOverDue = true;
                                    $scope.$apply(function (e) { $scope.Note = ''; });
                                    $scope.Mainwindow1.open();
                                    isOverDueWinOpen = true;
                                    isShutDownOpen = false;
                                }
                            },
                            {
                                text: 'Hold?',
                                width: 120,
                                columntype: 'button',
                                filterable: false,
                                sortable: false,
                                pinned: true,
                                hidden: true,
                                cellsrenderer: function () {
                                    return "Hold";
                                },
                                buttonclick: function (row) {
                                    var dataRecord = $scope.GridMain1.getrowdata(row);
                                    //alert(dataRecord.Id);
                                    updateId = dataRecord.Id;
                                    //scheduleType = dataRecord.ScheduleType;

                                    $scope.$apply(function (e) {
                                        switch (dataRecord.ScheduleType) {
                                            case 1:
                                                $scope.HoldIntervalType = "Day"
                                                break;
                                            case 2:
                                                $scope.HoldIntervalType = "Week"
                                                break;
                                            case 3:
                                                $scope.HoldIntervalType = "Month"
                                                break;
                                            case 4:
                                                $scope.HoldIntervalType = "Year"
                                                break;
                                        }
                                    });
                                    $scope.Mainwindow2.open();
                                    isOverDueWinOpen = true;
                                    isShutDownOpen = false;
                                }
                            }
    ];

    $scope.GridColumns2 = [
                            { text: 'Plant', datafield: 'PlantName', editable: false },
                            { text: 'Line', datafield: 'LineName', editable: false },
                            { text: 'Machine Name', datafield: 'MachineName', editable: false },
                            { text: 'Work Description', datafield: 'WorkName', editable: false },
                            { text: 'Start Date', datafield: 'NextReviewDate', editable: false, width: 120, filtertype: 'range', cellsformat: 'MM/dd/yyyy' },
                            { text: 'Check Point', datafield: 'Checkpoints', editable: false, width: 150 },
                            { text: 'Schedule Name', datafield: 'ScheduleTypeName', editable: false, width: 120 },
                            { text: 'Frequency', datafield: 'Interval', width: 100, editable: false },
                            { text: 'Severity', datafield: 'Severity', editable: false, width: 120 },
                            { text: 'Observation', datafield: 'IsObservation', editable: false, width: 80, columntype: 'checkbox' },
                            {
                                text: 'Task Done?',
                                width: 120,
                                columntype: 'button',
                                filterable: false,
                                sortable: false,
                                pinned: true,
                                cellsrenderer: function () {
                                    return "Done";
                                },
                                buttonclick: function (row) {
                                    var dataRecord = $scope.GridMain2.getrowdata(row);
                                    //alert(dataRecord.Id);
                                    updateId = dataRecord.Id;
                                    scheduleType = dataRecord.ScheduleType;
                                    reviewDate = dataRecord.NextReviewDate;
                                    isOverDue = false;
                                    $scope.$apply(function (e) { $scope.Note = ''; });
                                    $scope.Mainwindow1.open();
                                    isOverDueWinOpen = false;
                                    isShutDownOpen = false;
                                }
                            },
                            {
                                text: 'Hold?',
                                width: 120,
                                columntype: 'button',
                                filterable: false,
                                sortable: false,
                                pinned: true,
                                hidden: true,
                                cellsrenderer: function () {
                                    return "Hold";
                                },
                                buttonclick: function (row) {
                                    var dataRecord = $scope.GridMain2.getrowdata(row);
                                    //alert(dataRecord.Id);
                                    updateId = dataRecord.Id;
                                    //scheduleType = dataRecord.ScheduleType;
                                    $scope.Mainwindow2.open();
                                    isOverDueWinOpen = false;
                                    isShutDownOpen = false;

                                    $scope.$apply(function (e) {
                                        switch (dataRecord.ScheduleType) {
                                            case 1:
                                                $scope.HoldIntervalType = "Day"
                                                break;
                                            case 2:
                                                $scope.HoldIntervalType = "Week"
                                                break;
                                            case 3:
                                                $scope.HoldIntervalType = "Month"
                                                break;
                                            case 4:
                                                $scope.HoldIntervalType = "Year"
                                                break;
                                        }
                                    });
                                }
                            }
    ];

    $scope.GridColumns3 = [
                               { text: 'Plant', datafield: 'PlantName', editable: false },
                               { text: 'Line', datafield: 'LineName', editable: false },
                               { text: 'Machine Name', datafield: 'MachineName', editable: false },
                               { text: 'Work Description', datafield: 'WorkName', editable: false },
                               { text: 'Due Date', datafield: 'NextReviewDate', editable: false, width: 120, filtertype: 'range', cellsformat: 'MM/dd/yyyy' },
                               { text: 'Check Point', datafield: 'Checkpoints', editable: false, width: 150 },
                               { text: 'Schedule Name', datafield: 'ScheduleTypeName', editable: false, width: 120 },
                               { text: 'Frequency', datafield: 'Interval', editable: false, width: 100 },
                               { text: 'Severity', datafield: 'Severity', editable: false, width: 120 },
                               { text: 'Observation', datafield: 'IsObservation', editable: false, width: 80, columntype: 'checkbox' },

    ];

    $scope.Gridcolumns4 = [

                           { text: 'Plant', datafield: 'PlantName' },
                           { text: 'Line', datafield: 'LineName' },
                           { text: 'Machine Name', datafield: 'MachineName' },
                           { text: 'Task Description', datafield: 'TaskDescription' },
                           { text: 'Priority', datafield: 'PrioritytName', width: '60' },
                           { text: 'Status', datafield: 'StatusName', width: '70' },
                           { text: 'Update By', datafield: 'UpdateUserName', width: '80' },
                           { text: 'Update Date', datafield: 'UpdateDate', filtertype: 'range', cellsformat: 'MM/dd/yyyy', width: '80' },

                            {
                                text: 'Update?',
                                width: 120,
                                columntype: 'button',
                                filterable: false,
                                cellsrenderer: function () {
                                    return "Update";
                                },
                                buttonclick: function (row) {
                                    var dataRecord = $scope.GridAuditTask.getrowdata(row);
                                    //alert(dataRecord.Interval);
                                    updateId = dataRecord.Id;
                                    //statusId = dataRecord.statusId;
                                    //$scope.Mainwindow3.open();
                                    $.ajax({
                                        url: "/Home/GetAuditTaskData",
                                        type: "GET",
                                        contentType: "application/json;",
                                        dataType: "json",
                                        data: { Id: updateId },
                                        success: function (data) {
                                            $scope.$apply(function () {
                                                $scope.Mainwindow3.open();
                                                $scope.ddlStatus.val(data.StatusId);
                                                $scope.ddlWorkconducted.val(data.WorkConducted);
                                                if (typeof (data.CloseDateTime) != 'undefined' && data.CloseDateTime != null) {
                                                    data.CloseDateTime = new Date($scope.ToJavaScriptDate(data.CloseDateTime));
                                                    $scope.dtClose.setDate(data.CloseDateTime);
                                                }
                                            });
                                        },
                                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                                            alert('oops, something bad happened');
                                        }
                                    });
                                }
                            },
    ];

    $scope.GridColumns5 = [
                            { text: 'Plant', datafield: 'PlantName', editable: false },
                            { text: 'Line', datafield: 'LineName', editable: false },
                            { text: 'Machine Name', datafield: 'MachineName', editable: false },
                            { text: 'Work Description', datafield: 'WorkName', editable: false },
                            { text: 'Start Date', datafield: 'NextReviewDate', editable: false, width: 120, filtertype: 'range', cellsformat: 'MM/dd/yyyy' },
                            { text: 'Check Point', datafield: 'Checkpoints', editable: false, width: 150 },
                            { text: 'Schedule Name', datafield: 'ScheduleTypeName', editable: false, width: 120 },
                            { text: 'Severity', datafield: 'Severity', editable: false, width: 120 },
                            { text: 'Observation', datafield: 'IsObservation', editable: false, width: 80, columntype: 'checkbox' },
                               {
                                   text: 'Task Done?',
                                   width: 120,
                                   columntype: 'button',
                                   filterable: false,
                                   sortable: false,
                                   pinned: true,
                                   cellsrenderer: function () {
                                       return "Done";
                                   },
                                   buttonclick: function (row) {
                                       var dataRecord = $scope.ShutdownActivity.getrowdata(row);
                                       //alert(dataRecord.Id);
                                       updateId = dataRecord.Id;
                                       scheduleType = dataRecord.ScheduleType;
                                       reviewDate = dataRecord.NextReviewDate;
                                       isOverDue = false;
                                       $scope.$apply(function (e) { $scope.Note = ''; });
                                       $scope.Mainwindow1.open();
                                       isOverDueWinOpen = false;
                                       isShutDownOpen = true;
                                   }
                               },
                            {
                                text: 'Hold?',
                                width: 120,
                                columntype: 'button',
                                filterable: false,
                                sortable: false,
                                pinned: true,
                                hidden: true,
                                cellsrenderer: function () {
                                    return "Hold";
                                },
                                buttonclick: function (row) {
                                    var dataRecord = $scope.ShutdownActivity.getrowdata(row);
                                    //alert(dataRecord.Id);
                                    updateId = dataRecord.Id;
                                    //scheduleType = dataRecord.ScheduleType;
                                    $scope.Mainwindow2.open();
                                    isOverDueWinOpen = false;
                                    isShutDownOpen = false;

                                    $scope.$apply(function (e) {
                                        switch (dataRecord.ScheduleType) {
                                            case 1:
                                                $scope.HoldIntervalType = "Day"
                                                break;
                                            case 2:
                                                $scope.HoldIntervalType = "Week"
                                                break;
                                            case 3:
                                                $scope.HoldIntervalType = "Month"
                                                break;
                                            case 4:
                                                $scope.HoldIntervalType = "Year"
                                                break;
                                        }
                                    });
                                }
                            }
    ];

    angular.element(document).ready(function () {
        $scope.onUpdate = function (event) {
            var statusId = parseInt($scope.ddlStatus.val());
            var item = $scope.ddlStatus.getSelectedItem();
            var status = item.label;

            if (status == "Close")
                var CloseDateTime = new Date($scope.dtClose.getDate()).toISOString();
            else
                var CloseDateTime = null;
            //Ajax call to save
            if (status == "Close" && (CloseDateTime == 'undefined' || CloseDateTime == null)) {
                $scope.openMessageBox('Warning', 'Please choose close date&time.', 350, 90);
            }
            else
                $.ajax({
                    url: "/Home/SaveAuditTask",
                    type: "GET",
                    contentType: "application/json;",
                    dataType: "json",
                    data: { ATId: updateId, StatusId: statusId, WorkConducted: $scope.WorkConducted, CloseDateTime: CloseDateTime },
                    success: function (data) {
                        $scope.Mainwindow3.close();
                        $scope.openMessageBox('Success', 'Update successfully.', 200, 90);
                        $scope.GridAuditTask.updatebounddata();
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('oops, something bad happened');
                    }
                });
        };
    });
    $scope.onSave = function (event) {

        //Ajax call to save
        $.ajax({
            url: "/Home/SaveHoldDays",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { PMId: updateId, HoldDays: $scope.HoldDays, Reason: $scope.Reason },
            success: function (data) {
                $scope.Mainwindow2.close();
                $scope.openMessageBox('Success', 'Hold successfully.', 200, 90);
                filterInfo = $scope.GridMain1.getfilterinformation();
                filterInfo2 = $scope.GridMain2.getfilterinformation();
                filterInfo4 = $scope.ShutdownActivity.getfilterinformation();
                if (isOverDueWinOpen)
                    $scope.GridMain1.updatebounddata();
                else
                    $scope.GridMain2.updatebounddata();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    };
    $scope.onReviewNow = function (event) {
        if (isOverDue == true) {
            var isValidate = $scope.ReviewValidator.validate();
            if (!isValidate)
                return;
        }
        //Ajax call to save
        $.ajax({
            url: "/Home/SaveReview",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { PMId: updateId, Note: $scope.Note, scheduleType: scheduleType, interval: Interval, ReviewDate: reviewDate.toUTCString(), isOverDue: isOverDue },
            success: function (data) {
                $scope.Mainwindow1.close();
                $scope.openMessageBox('Success', 'Reviewed successfully.', 200, 90);
                filterInfo = $scope.GridMain1.getfilterinformation();
                filterInfo2 = $scope.GridMain2.getfilterinformation();
                filterInfo4 = $scope.ShutdownActivity.getfilterinformation();

                if (isShutDownOpen)
                    $scope.ShutdownActivity.updatebounddata();
                if (isOverDueWinOpen)
                    $scope.GridMain1.updatebounddata();
                else
                    $scope.GridMain2.updatebounddata();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    };

    $scope.OverDueSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'WorkName', type: 'string' },
           { name: 'NextReviewDate', type: 'date' },
           { name: 'Checkpoints', type: 'string' },
           { name: 'ScheduleTypeName', type: 'string' },
           { name: 'ScheduleType', type: 'int' },
           { name: 'Severity', type: 'string' },
           { name: 'IsObservation', type: 'bool' },
           { name: 'Interval', type: 'int' }
        ],
        url: '/Home/GetPMScheduleListForUser',
        data: { PMType: 1 }
    };
    $scope.TodaySource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'WorkName', type: 'string' },
           { name: 'NextReviewDate', type: 'date' },
           { name: 'Checkpoints', type: 'string' },
           { name: 'ScheduleTypeName', type: 'string' },
           { name: 'ScheduleType', type: 'int' },
           { name: 'Severity', type: 'string' },
           { name: 'IsObservation', type: 'bool' },
           { name: 'Interval', type: 'int' }
        ],
        url: '/Home/GetPMScheduleListForUser',
        data: { PMType: 2 }
    };
    $scope.UpComingSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'WorkName', type: 'string' },
           { name: 'NextReviewDate', type: 'date' },
           { name: 'Checkpoints', type: 'string' },
           { name: 'ScheduleTypeName', type: 'string' },
           { name: 'ScheduleType', type: 'int' },
           { name: 'Severity', type: 'string' },
           { name: 'IsObservation', type: 'bool' },
           { name: 'Interval', type: 'int' }
        ],
        url: '/Home/GetPMScheduleListForUser',
        data: { PMType: 3 }
    };
    $scope.ShutdownActivitySource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'WorkName', type: 'string' },
           { name: 'NextReviewDate', type: 'date' },
           { name: 'Checkpoints', type: 'string' },
           { name: 'ScheduleTypeName', type: 'string' },
           { name: 'ScheduleType', type: 'int' },
           { name: 'Severity', type: 'string' },
           { name: 'IsObservation', type: 'bool' },

        ],
        url: '/Home/GetPMScheduleListForUser',
        data: { PMType: 4 }
    };

    //$scope.AuditTasksource = {
    //    datatype: "json",
    //    datafields: [
    //       { name: 'Id', type: 'int' },
    //       { name: 'PlantName', type: 'string' },
    //       { name: 'LineName', type: 'string' },
    //       { name: 'MachineName', type: 'string' },
    //       { name: 'TaskDescription', type: 'string' },
    //       { name: 'PrioritytName', type: 'string' },
    //       { name: 'StatusName', type: 'string' },
    //       { name: 'UpdateUserName', type: 'string' },
    //       { name: 'UpdateDate', type: 'date' },
    //       { name: 'StatusId', type: 'int' }
    //    ],
    //    url: '/Home/GetAuditTaskList'
    //};

    $scope.OpenMR = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'SerialNo', type: 'string' },
           { name: 'Problem', type: 'string' },
           { name: 'RequestBy', type: 'int' },
           { name: 'StatusId', type: 'int' },
           { name: 'Status', type: 'string' },
           { name: 'Priority', type: 'string' },
           { name: 'RequestUserName', type: 'string' },
           { name: 'AssignUserName', type: 'string' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'IsBreakdown', type: 'bool' }
        ],
        url: '/Home/GetMRListForDashboard',
        data: { StatusId: 1 },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'

    };
    $scope.CompletedMR = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'SerialNo', type: 'string' },
           { name: 'Problem', type: 'string' },
           { name: 'RequestBy', type: 'int' },
           { name: 'StatusId', type: 'int' },
           { name: 'Status', type: 'string' },
           { name: 'Priority', type: 'string' },
           { name: 'RequestUserName', type: 'string' },
           { name: 'AssignUserName', type: 'string' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'IsBreakdown', type: 'bool' }
        ],
        url: '/Home/GetMRListForDashboard',
        data: { StatusId: 4 },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'

    };

    $scope.AssignMR = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'SerialNo', type: 'string' },
           { name: 'Problem', type: 'string' },
           { name: 'RequestBy', type: 'int' },
           { name: 'StatusId', type: 'int' },
           { name: 'Status', type: 'string' },
           { name: 'Priority', type: 'string' },
           { name: 'RequestUserName', type: 'string' },
           { name: 'AssignUserName', type: 'string' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'IsBreakdown', type: 'bool' }
        ],
        url: '/Home/GetMRListForDashboard',
        data: { StatusId: 2 },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'

    };

    $scope.ProgressMR = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'SerialNo', type: 'string' },
           { name: 'Problem', type: 'string' },
           { name: 'RequestBy', type: 'int' },
           { name: 'StatusId', type: 'int' },
           { name: 'Status', type: 'string' },
           { name: 'Priority', type: 'string' },
           { name: 'RequestUserName', type: 'string' },
           { name: 'AssignUserName', type: 'string' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'IsBreakdown', type: 'bool' }
        ],
        url: '/Home/GetMRListForDashboard',
        data: { StatusId: 3 },
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'
    };
    var sortDataField = null;
    var sortOrder = null;
    $scope.OtherMR = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'SerialNo', type: 'string' },
           { name: 'Problem', type: 'string' },
           { name: 'RequestBy', type: 'int' },
           { name: 'StatusId', type: 'int' },
           { name: 'Status', type: 'string' },
           { name: 'Priority', type: 'string' },
           { name: 'RequestUserName', type: 'string' },
           { name: 'AssignUserName', type: 'string' },
           { name: 'PlantName', type: 'string' },
           { name: 'LineName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'RequestDate', type: 'date' },
           { name: 'IsBreakdown', type: 'bool' }
        ],
        url: '/Home/GetMRListForDashboard',
        data: { StatusId: 0 },
        root: 'Rows',
        sort: function () {
            $scope.grdOtherMR.updatebounddata();
        },
        filter: function () {
            $scope.grdOtherMR.updatebounddata();
        },
        beforeprocessing: function (data) {
            $scope.OtherMR.totalrecords = data.TotalRows;
        }
    };

    var dataadapter = new $.jqx.dataAdapter($scope.OtherMR);

    $scope.OtherMRGrid = {
        source: dataadapter,
        virtualmode: true,
        sortable: true,
        filterable: true,
        rendergridrows: function (obj) {
            return  obj.data;
        }
    };

    $scope.UserSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'UserName', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetUserList'
    };

    $scope.WinPlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Plant/GetPlantsForSite',
        data: { SiteId: 1 }
    };
    $scope.WinLineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Line/GetLinesForPlant',
        data: { PlantId: 0 }
    };
    $scope.WinMachineSource = {
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
    $scope.StatusSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'StatusName', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetStatusList'
    };

    $scope.onWinPlantChange = function (e) {
        $scope.WinLineSource.data = { PlantId: $scope.ddlWinPlant.val($scope.model.PlantId) };
    }
    $scope.onPlantWinBindingComplete = function (e) {
        $scope.ddlWinPlant.val($scope.model.PlantId);
    }
    $scope.onWinLineChange = function (e) {
        $scope.WinMachineSource.data = { LineId: $scope.ddlWinLine.val($scope.model.LineId) };
    }
    $scope.onWinLineBindingComplete = function (e) {
        $scope.ddlWinLine.val($scope.model.LineId);
    }
    $scope.onWinBindingMachine = function (e) {
        $scope.ddlWinMachine.val($scope.model.MachineId);
    }

    $scope.UpdateMaitanceRequest = function (row, column, value) {
        return "Assign";
    }

    $scope.model = {};
    var MRId = 0;
    $scope.AssignToUser = function (row, event) {
        var dataRecord = $scope.grdOpenMR.getrowdata(row);
        $scope.ddlUser.selectIndex(-1);  //
        $.ajax({
            url: "/MaintenanceRequest/GetMRForDashboard",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                //$scope.$apply(function () {
                    $scope.model = data;
                    MRId = dataRecord.Id;
                    $scope.IsAssigned = true;
                    $scope.IsRemarks = false;

                    if (data.IsBreakdown == true) {
                        $scope.IsBreakdown = true;
                        $scope.IsBreakdownType = true;
                        $scope.ddlWinPlant.val($scope.model.PlantId);
                    }
                    else
                    {
                        $scope.IsBreakdown = false;
                        $scope.IsBreakdownType = false;
                    }
                    if (typeof (data.RequestDate) != 'undefined' && data.RequestDate != null) {
                        data.RequestDate = new Date($scope.ToJavaScriptDate(data.RequestDate));
                        $scope.dtRequestTime.setDate(data.RequestDate);
                    }
                    if (typeof (data.RequestTime) != 'undefined' && data.RequestTime != null) {
                        data.RequestTime = new Date($scope.ToJavaScriptDate(data.RequestTime));
                        $scope.dtRequestTime.setDate(data.RequestTime);
                    }

                    if (data.BreakdownType != null) {
                        if (data.BreakdownType == 1)
                            $scope.btnElectrical = true;
                        else if (data.BreakdownType == 2)
                            $scope.btnMechanical = true;
                        else if (data.BreakdownType == 3)
                            $scope.btnInstrument = true;

                    }

                    $scope.enableProperty = true;
                    $scope.enableRadioBtn = false;
                    $scope.InProgress = false;
                    $scope.IsClose = false;
                    $scope.IsSave = true;
                    $scope.IsDownload = false;
                    $scope.WinMaintenanceRequest.open();
                //});
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.OnSaveMaintenanceRequest = function (event) {
        var isValidate = $scope.MRValidator.validate();
        if (!isValidate)
            return;

        $scope.model.BreakdownType = $("input[name='btnBreakdwonType']:checked").val();
        if ($scope.model.StatusId == 2) {//($scope.dtWorkStartDate.getDate() != null)
            if ($scope.dtWorkStartDate.getDate() != null)
            {
                $scope.model.WorkStartDate = new Date($scope.dtWorkStartDate.getDate()).toISOString();
                $scope.model.WorkStartTime = new Date($scope.dtWorkStartTime.getDate()).toISOString();
            }
        }
        if ($scope.model.StatusId == 3) {//$scope.dtWorkEndDate.getDate() != null
            if ($scope.dtWorkEndDate.getDate() != null)
            {
                $scope.model.WorkEndDate = new Date($scope.dtWorkEndDate.getDate()).toISOString();
                $scope.model.WorkEndTime = new Date($scope.dtWorkEndTime.getDate()).toISOString();
            }
        }
        $http.post('/MaintenanceRequest/SaveRemarks', { MaintenanceRequest: $scope.model }).success(function (result) {
            if (result.Status == 1) {
                $scope.WinMaintenanceRequest.close();
                $scope.grdOtherMR.updatebounddata();
                $scope.grdMRRemarks.updatebounddata();
                //otherfilter = $scope.grdOtherMR.getfilterinformation();
                if ($scope.model.StatusId == 1 || $scope.model.StatusId == 4) {
                    $scope.grdOpenMR.updatebounddata();
                    $scope.grdCompletedMR.updatebounddata();
                    $scope.grdOtherMR.updatebounddata();
                    $scope.model.Remarks = null;
                    $scope.model.CloseRemarks = null;
                    //openfilter = $scope.grdOpenMR.getfilterinformation();
                    //completefilter = $scope.grdCompletedMR.getfilterinformation();
                     $scope.MRValidator.hide();
                }
                if ($scope.model.StatusId == 2 || $scope.model.StatusId == 3) {
                    $scope.grdAssignMR.updatebounddata();
                    $scope.grdProgressMR.updatebounddata();
                    $scope.grdOtherMR.updatebounddata();
                    $scope.model.ProgressRemarks = null;
                    $scope.model.CompleteRemarks = null;
                    //assignfilter = $scope.grdAssignMR.getfilterinformation();
                    //progressfilter = $scope.grdProgressMR.getfilterinformation();
                    $scope.MRValidator.hide();
                }
                $scope.openMessageBox('Success', result.Message, 300, 90);
            } else if (result.Status == 2) {
                $scope.openMessageBox('Warning', result.Message, 300, 90);
            }
            else if (result.Status == 3) {
                $scope.openMessageBox('Error', result.Message, 'auto', 'auto');
            }

        }).error(function (result, status, headers, config) {
            alert("status");
        });
    }

    $scope.StartMaintenanceWork = function (row, column, value) {
        return "In Progress";
    }

    $scope.model = {};
    var MaitanceId = 0;
    MRId = 0;
    $scope.StartWork = function (row, event) {
        ClearData();
        var dataRecord = $scope.grdAssignMR.getrowdata(row);
        $.ajax({
            url: "/MaintenanceRequest/GetMRForDashboard",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                $scope.$apply(function (e) {
                    $scope.model = data;
                    MaitanceId = data.Id;
                    MRId = dataRecord.Id;
                    $scope.IsAssigned = false;
                    $scope.IsRemarks = true;
                    if (data.IsBreakdown == true) {
                        $scope.IsBreakdown = true;
                        $scope.IsBreakdownType = true;
                        $scope.ddlWinPlant.val($scope.model.PlantId);
                    }
                    else {
                        $scope.IsBreakdown = false;
                        $scope.IsBreakdownType = false;
                    }
                    if (typeof (data.RequestDate) != 'undefined' && data.RequestDate != null) {
                        data.RequestDate = new Date($scope.ToJavaScriptDate(data.RequestDate));
                        $scope.dtRequestDate.setDate(data.RequestDate);
                    }
                    if (typeof (data.RequestTime) != 'undefined' && data.RequestTime != null) {
                        data.RequestTime = new Date($scope.ToJavaScriptDate(data.RequestTime));
                        $scope.dtRequestTime.setDate(data.RequestTime);
                    }
                    if (typeof ($scope.model.WorkStartDate) == 'undefined' || $scope.model.WorkStartDate == null) {
                    $scope.model.WorkStartDate = new Date();
                    $scope.dtWorkStartDate.setDate(new Date());
                    }
                    if (typeof ($scope.model.WorkStartTime) == 'undefined' || $scope.model.WorkStartTime == null) {
                    $scope.model.WorkStartTime = new Date();
                    $scope.dtWorkStartTime.setDate(new Date());
                    }
                    if (data.BreakdownType != null) {
                        if (data.BreakdownType == 1)
                            $scope.btnElectrical = true;
                        else if (data.BreakdownType == 2)
                            $scope.btnMechanical = true;
                        else if (data.BreakdownType == 3)
                            $scope.btnInstrument = true;
                    }
                    $scope.enableProperty = true;
                    $scope.enableRadioBtn = true;
                    $scope.InProgress = true;
                    $scope.IsCompelete = false;
                    $scope.IsClose = false;
                    $scope.IsSave = true;
                    $scope.IsDownload = false;
                });
                    
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.WinMaintenanceRequest.open();
    }

    $scope.EndMaintenanceWork = function (row, column, value) {
        return "Compelete";
    }
    var EndMaitanceId = 0;
    MRId = 0;
    $scope.EndWork = function (row, event) {
        var dataRecord = $scope.grdProgressMR.getrowdata(row);
        $.ajax({
            url: "/MaintenanceRequest/GetMRForDashboard",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                $scope.$apply(function (e) {
                    $scope.model = data;
                    MRId = dataRecord.Id;
                    EndMaitanceId = data.Id;
                    $scope.IsAssigned = false;
                    $scope.IsRemarks = true;
                    if (data.IsBreakdown == true) {
                        $scope.IsBreakdown = true;
                        $scope.IsBreakdownType = true;
                        $scope.ddlWinPlant.val($scope.model.PlantId);
                    }
                    else {
                        $scope.IsBreakdown = false;
                        $scope.IsBreakdownType = false;
                    }
                    if (typeof (data.RequestDate) != 'undefined' && data.RequestDate != null) {
                        data.RequestDate = new Date($scope.ToJavaScriptDate(data.RequestDate));
                        $scope.dtRequestTime.setDate(data.RequestDate);
                    }
                    if (typeof (data.RequestTime) != 'undefined' && data.RequestTime != null) {
                        data.RequestTime = new Date($scope.ToJavaScriptDate(data.RequestTime));
                        $scope.dtRequestTime.setDate(data.RequestTime);
                    }
                    if (typeof ($scope.model.WorkEndDate) == 'undefined' || $scope.model.WorkEndDate == null) {
                        $scope.model.WorkEndDate = new Date();
                        $scope.dtWorkEndDate.setDate(new Date());
                    }
                    if (typeof ($scope.model.WorkEndTime) == 'undefined' || $scope.model.WorkEndTime == null) {
                        $scope.model.WorkEndTime = new Date();
                        $scope.dtWorkEndTime.setDate(new Date());
                    }
                    if (data.BreakdownType != null) {
                        if (data.BreakdownType == 1)
                            $scope.btnElectrical = true;
                        else if (data.BreakdownType == 2)
                            $scope.btnMechanical = true;
                        else if (data.BreakdownType == 3)
                            $scope.btnInstrument = true;
                    }

                    $scope.enableProperty = true;
                    $scope.enableRadioBtn = true;
                    $scope.InProgress = false;
                    $scope.IsCompelete = true;
                    $scope.IsClose = false;
                    $scope.IsSave = true;
                    $scope.IsDownload = false;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.WinMaintenanceRequest.open();
    }

    $scope.CloseMaitanceRequest = function (row, column, value) {
        return "Close";
    }
    MRId = 0;
    $scope.CloseMRequest = function (row, event) {
        var dataRecord = $scope.grdCompletedMR.getrowdata(row);
        $.ajax({
            url: "/MaintenanceRequest/GetMRForDashboard",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                    $scope.model = data;
                    MRId = dataRecord.Id;
                    MaitanceId = data.Id;
                    $scope.IsAssigned = false;
                    $scope.IsRemarks = true;
                    if (data.IsBreakdown == true) {
                        $scope.IsBreakdown = true;
                        $scope.IsBreakdownType = true;
                        $scope.ddlWinPlant.val($scope.model.PlantId);
                    }
                    else {
                        $scope.IsBreakdown = false;
                        $scope.IsBreakdownType = false;
                    }
                    if (typeof (data.RequestDate) != 'undefined' && data.RequestDate != null) {
                        data.RequestDate = new Date($scope.ToJavaScriptDate(data.RequestDate));
                        $scope.dtRequestDate.setDate(data.RequestDate);
                    }
                    if (typeof (data.RequestTime) != 'undefined' && data.RequestTime != null) {
                        data.RequestTime = new Date($scope.ToJavaScriptDate(data.RequestTime));
                        $scope.dtRequestTime.setDate(data.RequestTime);
                    }

                    if (data.IsBreakdown == false) {
                        $scope.IsBreakdown = false;
                    }
                    if (data.BreakdownType != null) {
                        if (data.BreakdownType == 1)
                            $scope.btnElectrical = true;
                        else if (data.BreakdownType == 2)
                            $scope.btnMechanical = true;
                        else if (data.BreakdownType == 3)
                            $scope.btnInstrument = true;
                    }

                    $scope.enableProperty = true;
                    $scope.enableRadioBtn = true;
                    $scope.InProgress = false;
                    $scope.IsCompelete = false;
                    $scope.IsClose = true;
                    $scope.IsSave = true;
                    $scope.IsDownload = false;
               
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.grdCompletedMR.updatebounddata();
        $scope.WinMaintenanceRequest.open();
    }

    $scope.ViewMR = function (row, column, value) {
        return "View";
    }
    MRId = 0;
    $scope.ViewMRDetails = function (row, event) {
       
        var dataRecord = $scope.grdOtherMR.getrowdata(row);
        $.ajax({
            url: "/MaintenanceRequest/GetMRForDashboard",
            type: "GET",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                $scope.model = data;
                MRId = dataRecord.Id;
                MaitanceId = data.Id;
                $scope.IsAssigned = false;
                $scope.IsRemarks = true;
                if (data.IsBreakdown == true) {
                    $scope.IsBreakdown = true;
                    $scope.IsBreakdownType = true;
                    $scope.ddlWinPlant.val($scope.model.PlantId);
                }
                else {
                    $scope.IsBreakdown = false;
                    $scope.IsBreakdownType = false;
                }
                if (typeof (data.RequestDate) != 'undefined' && data.RequestDate != null) {
                    data.RequestDate = new Date($scope.ToJavaScriptDate(data.RequestDate));
                    $scope.dtRequestDate.setDate(data.RequestDate);
                }
                if (typeof (data.RequestTime) != 'undefined' && data.RequestTime != null) {
                    data.RequestTime = new Date($scope.ToJavaScriptDate(data.RequestTime));
                    $scope.dtRequestTime.setDate(data.RequestTime);
                }

                if (data.BreakdownType != null) {
                    if (data.BreakdownType == 1)
                        $scope.btnElectrical = true;
                    else if (data.BreakdownType == 2)
                        $scope.btnMechanical = true;
                    else if (data.BreakdownType == 3)
                        $scope.btnInstrument = true;
                }

                $scope.enableProperty = true;
                $scope.enableRadioBtn = true;
                $scope.InProgress = false;
                $scope.IsCompelete = false;
                $scope.IsClose = false;
                $scope.IsSave = false;
                $scope.IsDownload = true;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.grdOtherMR.updatebounddata();
        $scope.grdMRRemarks.updatebounddata();
        $scope.WinMaintenanceRequest.open();
    }

    $scope.OnDownloadMaitanceRequest = function (event) {
        window.location.href = "/Download/MaintenanceRequest/?Id=" + MRId;
    }

    $scope.ViewAllMRRemarks = function (event) {
        //alert(MRId);
        $scope.MRRemarks.data = { mrId: MRId };
        $scope.WinMRRemarks.open();
    }
    $scope.MRRemarks = {
        datatype: "json",
        cache: false,
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'StatusName', type: 'string' },
            { name: 'Remarks', type: 'string' },
            { name: 'RemarksDate', type: 'date' },
            { name: 'RemarksBy', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetMRRemarks',
        data: { mrId: 0 }
    };

    $scope.MRRemarksId = function (row, column, value) {
        return "<div style='margin:4px;'>" + (value + 1) + "</div>";
    }

    $scope.StatusSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetStatusList'
    };
    $scope.onStatusChange = function (e) {
        var item = $scope.ddlStatus.getSelectedItem();
        var status = item.label;
        if (status == "Open") {
            $scope.IsCd = true;
        }
        else if (status == "Close") {
            $scope.IsCd = false;
        }
        else if (status == "InProcess") {
            $scope.IsCd = true;
        }
    }
    $scope.onBindingStatus = function (event) {
        $scope.ddlStatus.val($scope.ddlStatus.val());
    }

    var filterInfo = [];
    var filterInfo2 = [];
    var filterInfo4 = [];
    var openfilter = [];
    var assignfilter = [];
    var progressfilter = [];
    var completefilter = [];
    var otherfilter = [];
    $scope.OnRefresh1 = function (e) {
        filterInfo = $scope.GridMain1.getfilterinformation();
        $scope.GridMain1.updatebounddata();
    };

    $scope.OnRefresh2 = function (e) {
        filterInfo2 = $scope.GridMain2.getfilterinformation();
        $scope.GridMain2.updatebounddata();
    };

    $scope.OnOpenRefresh = function (e) {
        openfilter = $scope.grdOpenMR.getfilterinformation();
        $scope.grdOpenMR.updatebounddata();
    };
    $scope.OnCompleteRefresh = function (e) {
        completefilter = $scope.grdCompletedMR.getfilterinformation();
        $scope.grdCompletedMR.updatebounddata();
    };
    $scope.OnAssignRefresh = function (e) {
        assignfilter = $scope.grdAssignMR.getfilterinformation();
        $scope.grdAssignMR.updatebounddata();
    };
    $scope.OnProgressRefresh = function (e) {
        progressfilter = $scope.grdProgressMR.getfilterinformation();
        $scope.grdProgressMR.updatebounddata();
    };
    $scope.OnOtherRefresh = function (e) {
        otherfilter = $scope.grdOtherMR.getfilterinformation();
        $scope.grdOtherMR.updatebounddata();
    };

    $scope.OnRefresh3 = function (e) {
        $scope.GridMain3.updatebounddata();
    };
    $scope.lineOnRefresh = function (e) {
        $scope.ddlLineInstance.updatebounddata();
    };
    $scope.machineOnRefresh = function (e) {
        $scope.ddlMachineInstance.updatebounddata();
    };
    $scope.shutdownOnRefresh = function (e) {
        filterInfo4 = $scope.ShutdownActivity.getfilterinformation();
        $scope.ShutdownActivity.updatebounddata();
    };

    $scope.onGrid1BinidingComplete = function (event) {
        $.each(filterInfo, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GridMain1.addfilter(filterGroup.datafield, group);
            $scope.GridMain1.applyfilters();
        });
    };

    $scope.onGrid2BinidingComplete = function (event) {
        $.each(filterInfo2, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.GridMain2.addfilter(filterGroup.datafield, group);
            $scope.GridMain2.applyfilters();
        });
    };

    $scope.onGrid4BinidingComplete = function (event) {
        $.each(filterInfo4, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.ShutdownActivity.addfilter(filterGroup.datafield, group);
            $scope.ShutdownActivity.applyfilters();
        });
    };

    $scope.OpenMRBinidingComplete = function (event) {
        $.each(openfilter, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.grdOpenMR.addfilter(filterGroup.datafield, group);
            $scope.grdOpenMR.applyfilters();
        });
    };

    $scope.CompletedMRBinidingComplete = function (event) {
        $.each(completefilter, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.grdCompletedMR.addfilter(filterGroup.datafield, group);
            $scope.grdCompletedMR.applyfilters();
        });
    };

    $scope.AssignMRBinidingComplete = function (event) {
        $.each(assignfilter, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.grdAssignMR.addfilter(filterGroup.datafield, group);
            $scope.grdAssignMR.applyfilters();
        });
    };

    $scope.ProgressMRBinidingComplete = function (event) {
        $.each(progressfilter, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.grdProgressMR.addfilter(filterGroup.datafield, group);
            $scope.grdProgressMR.applyfilters();
        });
    };

    $scope.OtherMRBinidingComplete = function (event) {
        $.each(otherfilter, function (i, filterGroup) {
            var group = new $.jqx.filter();
            $.each(filterGroup.filter.getfilters(), function (j, filter) {
                group.addfilter(1, group.createfilter(filter.type, filter.value, filter.condition));
            });
            $scope.grdOtherMR.addfilter(filterGroup.datafield, group);
            $scope.grdOtherMR.applyfilters();
        });
    };

    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    };
    $scope.OnAssignRefresh = function (e) {
        $scope.grdAssignMR.updatebounddata();
    };
    $scope.OnCompleteRefresh = function (e) {
        $scope.grdCompletedMR.updatebounddata();
    };
    $scope.OnOpenRefresh = function (e) {
        $scope.grdOpenMR.updatebounddata();
    };
    $scope.OnOtherRefresh = function (e) {
        $scope.grdOtherMR.updatebounddata();
    };
    $scope.OnatFilterClick = function () {
        $scope.ShowFilterAt = !$scope.ShowFilterAt;
    };
    $scope.lineOnFilterClick = function () {
        $scope.ShowFilterAtline = !$scope.ShowFilterAtline;
    };
    $scope.machineOnFilterClick = function () {
        $scope.ShowFilterAtmachine = !$scope.ShowFilterAtmachine;
    };
    $scope.shutdownOnFilterClick = function () {
        $scope.ShowFiltershutdown = !$scope.ShowFiltershutdown;
    };
    $scope.todayOnFilterClick = function () {
        $scope.ShowFiltertoday = !$scope.ShowFiltertoday;
    };
    $scope.nextweekOnFilterClick = function () {
        $scope.ShowFilternextweek = !$scope.ShowFilternextweek;
    };
    $scope.OpenMRFilterClick = function () {
        $scope.ShowFilterOpenMR = !$scope.ShowFilterOpenMR;
    };
    $scope.CompletedMRFilterClick = function () {
        $scope.ShowFilterCompletedMR = !$scope.ShowFilterCompletedMR;
    };
    $scope.AssignMRFilterClick = function () {
        $scope.ShowFilterAssignMR = !$scope.ShowFilterAssignMR;
    };
    $scope.ProgressMRFilterClick = function () {
        $scope.ShowFilterProgressMR = !$scope.ShowFilterProgressMR;
    };
    $scope.OtherMRFilterClick = function () {
        $scope.ShowFilterOtherMR = !$scope.ShowFilterOtherMR;
    };
    
    $scope.ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes();
    };

    //pie chart
    $scope.piechartsource = {
        datatype: "json",
        datafield: [
           { name: 'Id', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'Count', type: 'int' }
        ],
        url: '/Home/GetBreakdownCountsByPlant'
    };
    $scope.piechartseriesGroups = [
        {
            type: 'pie',
            showLabels: true,
            click: myEventHandler,
            series: [
                    {
                        //data: $scope.PieChartAdapter,
                        dataField: 'Count',
                        displayText: 'PlantName',
                        labelRadius: 170,
                        initialAngle: 15,
                        radius: 80,
                        centerOffset: 0,
                        //formatSettings: { sufix: '%', decimalPlaces: 1 }
                    }
            ]
        }
    ];
    function myEventHandler(event) {
        //debugger;
        // var data = $scope.piechartsource.records;
        //   var eventData = '<div> DataField: </b>' + event.serie.displayText + '<b>, Value: </b>' + event.elementValue + "</div>";
        //$timeout(function () {
        //    $scope.eventText = eventData;
        //})
        // console.log(event);
    };

    // Bar Chart
    $scope.PlantSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Home/GetPlantsForChart'

    };
    $scope.onPlantChange = function (e) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.chartsource.data = { PlantId: selectedId };
        $scope.LineSource.data = { PlantId: selectedId };
    };
    $scope.onPlantBindingComplete = function (event) {
        var selectedId = parseInt($scope.ddlPlant.val());
        $scope.chartsource.data = { PlantId: selectedId };
        $scope.LineSource.data = { PlantId: selectedId };
    };

    $scope.chartsource = {
        autoBind: false,
        datatype: "json",
        datafield: [
           { name: 'Id', type: 'int' },
           { name: 'LineName', type: 'string' },
           { name: 'Count', type: 'int' }
        ],
        url: '/Home/GetBreakdownCountsByLine',
        data: { plantId: 0 }
    };
    $scope.xAxis = {
        dataField: 'LineName',
        gridLines: { visible: false },
        tickMarks: { visible: true },
        labels: {
            angle: 90,
            horizontalAlignment: 'right',
            verticalAlignment: 'center',
            rotationPoint: 'left',
            offset: { y: -30 }
        },
    };
    $scope.valueAxis = {
        minValue: 0,
        maxValue: 100,
        unitInterval: 10,
        title: { text: 'Breakdowns' }
    };
    $scope.seriesGroups = [
        {
            type: 'column',
            columnsGapPercent: 80,
            seriesGapPercent: 10,
            series: [
                    { dataField: 'Count', displayText: 'Breakdowns' },
            ]
        }
    ];

    // Machine Grid
    $scope.LineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Home/GetLinesForPlant',
        data: { PlantId: 0 }
    };
    $scope.onLineChange = function (e) {
        var selectedId = parseInt($scope.ddlLine.val());
        $scope.MachineGridSource.data = { LineId: selectedId };
        $scope.ddlLine.selectedIndex = 0;
    };
    $scope.onLineBindingComplete = function (e) {
        var selectedId = parseInt($scope.ddlLine.val());
        $scope.MachineGridSource.data = { LineId: selectedId };
        $scope.ddlLine.selectedIndex = 0;
    }

    $scope.gridLineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LineName', type: 'string' },
           { name: 'PlantId', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'IsShutdown', type: 'bool' },
           { name: 'ShutdownBy', type: 'string' },
           { name: 'ShutdownDate', type: 'date' }
        ],
        url: '/Line/GetShutdownLineList',
        updaterow: function (rowid, rowdata, commit) {
            $.ajax({
                url: "/Line/UpdateIsShutdown",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { lineId: rowdata.Id, IsShutdown: rowdata.IsShutdown },
                success: function (data) {
                    if (data.Status == 1) {
                        $scope.openMessageBox('Success', 'Update successfully.', 200, 90);
                    }
                    else if (data.Status == 2) {
                        $scope.openMessageBox('Warning', data.Message, 500, 100);
                        $scope.ddlLineInstance.updatebounddata();
                    }
                    else {
                        $scope.openMessageBox('Error', data.Message, 500, 100);
                        $scope.ddlLineInstance.updatebounddata();

                    }
                    $scope.ShutdownActivity.updatebounddata();

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
        }
    };

    $scope.gridMachineSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'LineId', type: 'int' },
           { name: 'LineName', type: 'string' },
           { name: 'PlantId', type: 'int' },
           { name: 'PlantName', type: 'string' },
           { name: 'MachineName', type: 'string' },
           { name: 'IsShutdown', type: 'bool' },
           { name: 'ShutdownBy', type: 'string' },
           { name: 'ShutdownDate', type: 'date' }

        ],
        url: '/Machine/GetShutdownMachineList',
        data: { Name: '', SiteId: 0, PlantId: 0, LineId: 0 },
        updaterow: function (rowid, rowdata, commit) {
            $.ajax({
                url: "/Machine/UpdateIsShutdown",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { machineId: rowdata.Id, IsShutdown: rowdata.IsShutdown },
                success: function (data) {
                    if (data.Status == 1) {
                        $scope.openMessageBox('Success', 'Update successfully.', 200, 90);
                    }
                    else {
                        $scope.openMessageBox('Warning', data.Message, 500, 100);
                        $scope.ddlMachineInstance.updatebounddata();
                    }
                    $scope.ShutdownActivity.updatebounddata();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
        }
    };

    $scope.MachineGridSource = {
        autoBind: false,
        datatype: "json",
        datafield: [
           { name: 'Id', type: 'int' },
           { name: 'MachineName', type: 'string' },
           { name: 'Count', type: 'int' },
            { name: 'TotalTime', type: 'int' }
        ],
        url: '/Home/GetBreakdownCountsByMachine',
        data: { LineId: 0 }
    };
    $scope.TimeFormate = function (row, columnfield, value, defaulthtml, columnproperties) {
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
            return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + '; color: #000000;">' + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + '</span>';
        else
            return '<span style="padding-top: 2px; margin: 4px; float: ' + columnproperties.cellsalign + '; color: #ff000000;">' + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + '</span>';
        //return hours + ":" + minutes;
    }

    $scope.OnExport = function (exportType, grid) {
        if (exportType == 'excel') {
            if (grid == 'today') {
                var PMType = 2;
                window.location = '/Download/PreventiveActilityList?PMType=' + PMType;
            }
            else if (grid == 'overdue') {
                var PMType = 1;
                window.location = '/Download/PreventiveActilityList?PMType=' + PMType;
            }
            else if (grid == 'shutdownactivity') {
                var PMType = 4;
                window.location = '/Download/PreventiveActilityList?PMType=' + PMType;
            }
        }
    }

    $scope.OnPrint = function (grid) {
        if (grid == 'overdue') {
            var gridContent = $scope.GridMain1.exportdata('html');
        }
        else if (grid == 'today') {
            var gridContent = $scope.GridMain2.exportdata('html');
        }
        else if (grid == 'nextweek') {
            var gridContent = $scope.GridMain3.exportdata('html');
        }
        else {//shutdown actvity
            var gridContent = $scope.ShutdownActivity.exportdata('html');
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

    $scope.OnPrintLine = function (grid) {
        var gridContent = $scope.ddlLineInstance.exportdata('html');
        if (grid = "overline") { // line
            var gridContent = $scope.ddlLineInstance.exportdata('html');
        }
        else if (grid = "overmachine") { // machine
            var gridContent = $scope.ddlMachineInstance.exportdata('html');
        }
        else {
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

    $scope.OnPrintMachine = function (grid) {
        var gridContent = $scope.ddlMachineInstance.exportdata('html');
        if (grid = "overmachine") { // machine
            var gridContent = $scope.ddlMachineInstance.exportdata('html');
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

    $scope.createWidget = true;

    $scope.VersionCellRenderer = function (row, column, value) {
        return "<div style='margin:4px;'>" + (value + 1) + "</div>";
    }

    $scope.gridVersionSource = {
        datatype: "json",
        cache: false,
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'ReleaseNote', type: 'string' },
           { name: 'ReleaseVersion', type: 'string' }
        ],
        url: '/Home/GetLatestVersion',
    };

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
    var ClearData = function (event) {
        if ((typeof ($scope.model) != 'undefined') && $scope.model != null) {
            $scope.dtWorkStartDate.setDate(new Date());
            $scope.dtWorkStartTime.setDate(new Date());
            $scope.dtWorkEndDate.setDate(new Date());
            $scope.dtWorkEndTime.setDate(new Date());
            //$('#dtWorkStartDate').jqxDateTimeInput('setDate', new Date());
            //$('#dtWorkStartTime').jqxDateTimeInput('setDate', new Date());
            $scope.MRValidator.hide();
        }

    }
});