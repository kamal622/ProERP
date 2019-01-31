ProApp.controller("IndentsController", function ($parse, $scope, $http) {
    var ItemName = $scope.getCookie('ItemName');
    var User = $scope.getCookie('User');
    var RequisitionType = $scope.getCookie('RequisitionType');
    if (typeof (ItemName) == 'undefined')
        $scope.SearchItemName = '';

    if (User == '')
        User = 0;

    if (typeof (RequisitionType) == 'undefined')
        RequisitionType = 0;



    $scope.SearchItemName = ItemName;
    $scope.PrioritySource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/MaintenanceRequest/GetPriorityList'
    };
    $scope.UserSource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'UserName', type: 'string' }
        ],
        url: '/Indent/GetUserList'
    };

    var MainIndentGridSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'IndentNo', type: 'string' },
           { name: 'PriorityName', type: 'string' },
           { name: 'RequisitionType', type: 'string' },
           { name: 'CreatedOn', type: 'date' },
           { name: 'Status', type: 'string' }
        ],
        url: '/Indent/GetIndentGridData',
        data: { User: User, Name: $scope.SearchItemName, RequisitionType: RequisitionType }
    };
    $scope.onRefreshClick = function (e) {
        $scope.MainGrid.updatebounddata();
    }

    var initrowdetails = function (index, parentElement, gridElement, record) {
        var id = record.uid.toString();
        var grid = $($(parentElement).children()[0]);
        var rowData = $scope.GridSettings.apply('getrowdata', index);

        var nestedSource = {
            datafields: [
                       { name: 'Id', type: 'int' },
                       { name: 'IndentId', type: 'int' },
                       { name: 'ItemCode', type: 'int' },
                       { name: 'Name', type: 'string' },
                       { name: 'JobDescription', type: 'string' },
                       { name: 'PlantName', type: 'string' },
                       { name: 'QtyNeeded', type: 'decimal' },
                       { name: 'StatusName', type: 'string' },
                       { name: 'RequiredByDate', type: 'date' }
            ],
            datatype: 'json',
            //type: 'POST',
            //root: 'Details',
            url: '/Indent/GetNestedGridData',
            Id: "IndentId",
            data: { IndentId: rowData.Id }
        };

        var nestedAdapter = new $.jqx.dataAdapter(nestedSource);

        if (grid != null) {
            grid.jqxGrid({
                source: nestedAdapter,
                width: '98%',
                height: 200,
                columnsresize: true,
                theme: $scope.theme,
                ready: function () {
                    if (rowData.RequisitionType == "PR") {
                        grid.jqxGrid('hidecolumn', 'JobDescription');
                    }
                    else if (rowData.RequisitionType == "JR") {
                        grid.jqxGrid('hidecolumn', 'ItemCode');
                        grid.jqxGrid('hidecolumn', 'Name');
                    }
                },
                columns: [
                            { text: 'Plant Name', datafield: 'PlantName', width: '130' },
                            { text: 'Item Code', datafield: 'ItemCode', width: '70' },
                            { text: 'Item Name', datafield: 'Name' },
                             { text: 'Job Description', datafield: 'JobDescription' },
                            { text: 'Requested Quantity', datafield: 'QtyNeeded', width: '130' },
                            { text: 'Status', datafield: 'StatusName', width: '80' },
                            { text: 'Required By Date', datafield: 'RequiredByDate', width: '110', cellsformat: 'dd/MM/yyyy' }
                ]
            });
        }
    }

    $scope.GridSettings = {
        width: '100%',
        source: MainIndentGridSource,
        theme: $scope.theme,
        columnsresize: true,
        rowdetails: true,
        rowsheight: 35,
        initrowdetails: initrowdetails,
        rowdetailstemplate: {
            rowdetails: "<div id='grid' style='margin: 10px;'></div>",
            rowdetailsheight: 220,
            rowdetailshidden: true
        },
    };
    $scope.UpdateIndent = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.MainGrid.getrowdata(row);
        return "<a style='margin: 4px;text-decoration:underline;' href='/Indent/Update/" + dataRecord.Id + "'> Edit/View</a>";
    }

    $scope.onUserBindingComplete = function (e) {
        if (User > 0)
            $scope.ddlUser.val(User);
    }

    $scope.onRequisitionBindingComplete = function (e) {
        if (RequisitionType !='')
            $scope.ddlRequisitionType.val(RequisitionType);
    }

    $scope.onSearch = function (e) {
        var User = ($scope.ddlUser.val() == "" ? 0 : $scope.ddlUser.val());
        var RequisitionType = ($scope.ddlRequisitionType.val() == "" ? 0 : $scope.ddlRequisitionType.val());
        MainIndentGridSource.data = { User: User, Name: $scope.SearchItemName, RequisitionType: RequisitionType };
        if (typeof ($scope.SearchMachineName) == 'undefined')
            $scope.delete_cookie('ItemName');

        $scope.setCookie('ItemName', $scope.SearchItemName, 1);
        $scope.setCookie('User', User, 1);
        $scope.setCookie('RequisitionType', RequisitionType, 1);
    };
});