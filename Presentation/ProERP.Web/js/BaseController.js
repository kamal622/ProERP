var ProApp = angular.module("ProApp", ["jqwidgets"]).factory('$exceptionHandler', function () {
    return function (exception, cause) {
        exception.message += ' (caused by "' + cause + '")';
        //console.log(exception, cause);
        //alert(exception.message);
        //console.log(exception);
        throw exception;
    }
});

ProApp.config(['$httpProvider', function ($httpProvider) {
    //initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    //disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
}]);

ProApp.controller("BaseController", function ($scope, $http) {
    //alert('Base');
    $scope.theme = 'metro';
    $scope.DisableSiteList = true;
    $scope.GridPageSizeOption = ['10', '20', '50', '100'];
    //$scope.GridHeight = 350;
    $scope.GridPageSize = 10;
    $scope.LoadingText = '<b>Please Wait...</b>'

    $scope.ActivateMenu = function (id) {
        //$('#sideMenu li .active').removeClass('active');
        //$(id).addClass('active');
    };

    // $scope.ToJavaScriptDate = function(value) {
    //    var pattern = /Date\(([^)]+)\)/;
    //    var results = pattern.exec(value);
    //    var dt = new Date(parseFloat(results[1]));
    //    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear()+" " +dt.getHours()+":"+dt.getMinutes();
    //};

    $scope.openConfirm = function (title, message, width, height, callBack) {
        var win = $('<div><div><b>' + title + '</b></div><div><div style="padding: 5px;">' + message + '</div><div></div></div></div>');
        // 
        var btnOk = $('<input type="button" value="Yes" style="margin-right: 10px" />');
        var btnCancel = $('<input type="button" value="No" />');
        var lastChild = win[0].lastChild.lastChild;
        $(lastChild).append(btnOk);
        $(lastChild).append(btnCancel);

        win.jqxWindow({
            height: height,
            width: width,
            theme: $scope.theme,
            autoOpen: true,
            isModal: true,
            resizable: false,
            okButton: btnOk,
            cancelButton: btnCancel,
            initContent: function () {
                btnOk.jqxButton({
                    width: '65px',
                    theme: $scope.theme
                });
                btnCancel.jqxButton({
                    width: '65px',
                    theme: $scope.theme
                });
                $('#ok').focus();
            }
        });

        win.on('close', function (event) {
            if (event.args.dialogResult.OK) {
                if (typeof (callBack) === 'function')
                    callBack(true);
            } else {
                if (typeof (callBack) === 'function')
                    callBack(false);
            }
        });
    }

    $scope.openMessageBox = function (title, message, width, height) {
        var win = $('<div><div><b>' + title + '</b></div><div><div style="padding: 5px;">' + message + '</div><div></div></div></div>');
        // 
        var btnOk = $('<input type="button" value="OK" style="margin-right: 10px; float: right;" />');

        var lastChild = win[0].lastChild.lastChild;
        $(lastChild).append(btnOk);

        win.jqxWindow({
            height: height,
            width: width,
            theme: $scope.theme,
            autoOpen: false,
            isModal: true,
            resizable: true,
            okButton: btnOk,
            initContent: function () {
                btnOk.jqxButton({
                    width: '65px',
                    theme: $scope.theme
                });

                $('#ok').focus();
            }
        });

        win.jqxWindow('open');
    }

    $scope.ShowLoader = function () {
        $('#jqxLoader').jqxLoader('open');
    }

    $scope.HideLoader = function () {
        $('#jqxLoader').jqxLoader('close');
    }

    $scope.setCookie = function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }

    $scope.getCookie = function (cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
    $scope.delete_cookie = function (name) {
        document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    };

    $http.get("/Home/GetCounts").then(function (response) {

        //$scope.PMPendingCount = response.data.PMPendingCount;
        //$scope.PMShutdownCount = response.data.PMShutdownCount;
        //$scope.PMOverDueCount = response.data.PMOverDueCount;
        //$scope.MaintenanceRequestOpenCount = response.data.MaintenanceRequestOpenCount;
        //$scope.MaintenanceRequestInProcessCount = response.data.MaintenanceRequestInProcessCount;
        //$scope.FirstName = response.data.FirstName;
        //$scope.LastName = response.data.LastName;
        //$scope.PreventiveNotifationCnt = 0;
        //$scope.OverdueNotifationCnt = 0;
        //if ($scope.PMPendingCount > 0)
        //    $scope.PreventiveNotifationCnt += 1;
        //if ($scope.PMShutdownCount > 0)
        //    $scope.PreventiveNotifationCnt += 1;
        //if ($scope.PMOverDueCount > 0)
        //    $scope.OverdueNotifationCnt += 1;

        $scope.NewFormulationRequestCount = response.data.NewFormulationRequestCount;
        $scope.RMRequestCount = response.data.RMRequestCount;
        $scope.RMDispatchCount = response.data.RMDispatchCount;
        $scope.ReadyForTestingCount = response.data.ReadyForTestingCount;
        $scope.FormulationNotifationCnt = 0;
        if ($scope.NewFormulationRequestCount > 0)
            $scope.FormulationNotifationCnt += 1;
        if ($scope.RMRequestCount > 0)
            $scope.FormulationNotifationCnt += 1;
        if ($scope.RMDispatchCount > 0)
            $scope.FormulationNotifationCnt += 1;
        if ($scope.ReadyForTestingCount > 0)
            $scope.FormulationNotifationCnt += 1;
    });
});

$(document).ready(function () {
    $.ajaxSetup({ cache: false });

    function escapeRegExp(str) {
        return str.toString().replace(/[.*+?^${}()|[\]\\]/g, "\\$&"); // $& means the whole matched string
    }

    String.prototype.replaceAll = function (search, replacement) {
        var target = this;
        return target.replace(new RegExp(escapeRegExp(search), 'g'), replacement);
    };
});

$(document).ajaxStart(function () {
    $('#jqxLoader').jqxLoader('open');
}).ajaxComplete(function () {
    $('#jqxLoader').jqxLoader('close');
});