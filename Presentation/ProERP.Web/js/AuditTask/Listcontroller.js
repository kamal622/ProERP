ProApp.controller("ATController", function ($parse, $scope, $http) {

     $scope.SearchMachineName = '';

    $scope.ATGrid = {};
    var updateId = 0;
    var StatusId = 0;

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
              { name: 'Description', type: 'string' }
          ],
          url: '/MaintenanceRequest/GetStatusList'
      };

    $scope.Gridcolumns = [


                            { text: 'Machine Name', datafield: 'MachineName' },
                            { text: 'Task Description', datafield: 'TaskDescription' },
                            { text: 'Priority', datafield: 'PrioritytName',width:'60' },
                            { text: 'Status', datafield: 'StatusName', width:'70' },
                            { text: 'Audit By', datafield: 'UserName' },
                            { text: 'Audit Date', datafield: 'AuditDate', cellsformat: 'MM/dd/yyyy',width:'80' },
                            {
                               text: 'Task Approve?',
                               width: 100,
                               columntype: 'button',
                               cellsrenderer: function () {
                                   return "Approve";
                               },
                               buttonclick: function (row) {

                                   var dataRecord = $scope.auditinstance.getrowdata(row);
                                   //alert(dataRecord.Id);
                                   updateId = dataRecord.Id;
                                   StatusId = dataRecord.StatusId;

                                   //Ajax call to save

                                   $.ajax({
                                       url: "/AuditTask/SaveApproveTask",
                                       type: "GET",
                                       contentType: "application/json;",
                                       dataType: "json",
                                       data: { MRId: updateId, StatusId:StatusId},
                                       success: function (data) {
                                           $scope.auditinstance.updatebounddata();
                                       },
                                       error: function (XMLHttpRequest, textStatus, errorThrown) {
                                           alert('oops, something bad happened');
                                       }
                                   });

                               }
                           },

                            {
                                text: 'Task Reject?',
                                width: 100,
                                columntype: 'button',
                                cellsrenderer: function () {
                                    return "Reject";
                                },
                                buttonclick: function (row) {

                                    var dataRecord = $scope.auditinstance.getrowdata(row);
                                   // alert(dataRecord.Id);
                                    updateId = dataRecord.Id;
                                    StatusId = dataRecord.StatusId;
                                    //Ajax call to save

                                     $.ajax({
                                         url: "/AuditTask/SaveRejectTask",
                                         type: "GET",
                                         contentType: "application/json;",
                                         dataType: "json",
                                         data: { MRId: updateId, StatusId: StatusId },
                                         success: function (data) {
                                             $scope.auditinstance.updatebounddata();
                                         },
                                         error: function (XMLHttpRequest, textStatus, errorThrown) {
                                             alert('oops, something bad happened');
                                         }
                                     });

                                }
                            }
                             
                             
    ];

    $scope.AT = {
        datatype: "json",

        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'MachineName', type: 'string' },
           { name: 'TaskDescription', type: 'string' },
           { name: 'PrioritytName', type: 'string' },
           { name: 'StatusName', type: 'string' },
           { name: 'UserName', type: 'string' },
           { name: 'AuditDate', type: 'date' },
           { name: 'StatusId', type: 'int' }
          
        ],
        url: '/AuditTask/GetATList',
        data: { Name: $scope.SearchMachineName, PlantId: 0, PriorityId: 0,StatusId: 0},
        Id: "Id",
        sortcolumn: 'Name',
        sortdirection: 'asc'

    };

     $scope.onSearch = function (e) {
        //alert($scope.SearchMachineName);
         //alert($scope.Statusinstance);
        var PlantId = ($scope.ddlPlant.val() == "" ? 0 : $scope.ddlPlant.val());
        var PriorityId = ($scope.Priorityinstance.val() == "" ? 0 : $scope.Priorityinstance.val());
        var StatusId = ($scope.Statusinstance.val() == "" ? 0 : $scope.Statusinstance.val());

        $scope.AT.data = { Name: $scope.SearchMachineName, PlantId: PlantId, PriorityId: PriorityId, StatusId: StatusId };
    };

     $scope.onRefreshClick = function (e) {
         $scope.auditinstance.updatebounddata();
     }
    //$scope.onDelete = function (e) {
    //    $scope.openConfirm("Confirmation", 'Are you sure you want to Delete Machine(s)?', 350, 100, function (e) {
    //        if (e) {
    //            debugger;
    //            var rows = $scope.ATGrid.jqxGrid('selectedrowindexes');
    //            var selectedIds = [];
    //            for (var m = 0; m < rows.length; m++) {
    //                var row = $scope.ATGrid.jqxGrid('getrowdata', rows[m]);
    //                if (row != null)
    //                    selectedIds.push(row.Id);
    //            }

    //            $http.post('/AuditTask/DeleteAT', { Ids: selectedIds }).success(function (retData) {
    //                if (retData.Message == "Success") {

    //                    //  $('#lstBatch').jqxListBox('refresh');
    //                    $scope.ATGrid.jqxGrid('updatebounddata');

    //                } else {
    //                    debugger;
    //                    $scope.openMessageBox("Error", retData.Message, 350, 100);

    //                    $scope.ATGrid.jqxGrid('updatebounddata');

    //                }

    //            }).error(function (retData, status, headers, config) {

    //            });

    //        }
    //        else {

    //        }
    //    });
    //};


    $scope.createWidget = true;
});