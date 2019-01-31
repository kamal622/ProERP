ProApp.controller("ListUserController", function ($scope, $http) {

    $scope.SearchUserName = '';
    $scope.SearchEmail = '';
    $scope.NewPassword = '';
    $scope.ConfNewPassword = '';

    $scope.Rules = [
             {
                 input: '#dropRole', message: 'Please Select Role', action: 'blur',
                 rule: function (input, commit) {
                     var index = $scope.ddlRole.getSelectedIndex();
                     return index != -1;
                 }
             },
             { input: '#uname', message: 'Username is required!', action: 'blur', rule: 'required' },
             { input: '#email', message: 'Email is required!', action: 'blur', rule: 'required' },
             { input: '#email', message: 'Email is incorrect!', action: 'blur', rule: 'email' },
             { input: '#fname', message: 'First Name is required!', action: 'blur', rule: 'required' },
             { input: '#lname', message: 'Last Name is required!', action: 'blur', rule: 'required' },
             { input: '#password', message: 'Password is required!', action: 'blur', rule: 'required' }
    ];

    //debugger;
    $scope.Pass = [
        { input: '#txtNewPass', message: 'Password is required!', action: 'blur', rule: 'required' }
    ];

    $scope.Users = {
        datatype: "json",
        datafields: [
          { name: 'Id', type: 'int' },
          { name: 'UserProfile_Id', type: 'int' },
          { name: 'UserName', type: 'string' },
          { name: 'FirstName', type: 'string' },
          { name: 'LastName', type: 'string' },
          { name: 'Email', type: 'string' },
           { name: 'IsActive', type: 'bool' },
           { name: 'IsBlocked', type: 'bool' }

        ],
        url: '/Manage/GetUsersList',
        data: { UserName: $scope.SearchUserName, Email: $scope.SearchEmail },
        updaterow: function (rowid, rowdata, commit) {
            $.ajax({
                type: "POST",
                url: '/Manage/UpdateUserData',
                data: { Id: rowdata.UserProfile_Id, IsActive: rowdata.IsActive, IsBlocked: rowdata.IsBlocked },
                datatype: "json",
                success: function (data) {
                    commit((data.Message == 'Success'));
                    $scope.UserGrid.updatebounddata();
                    //$('#gridMain').unblock();
                }
            });
        }
    };

    $scope.UpdatePassword = function (row, columnfield, value, defaulthtml, columnproperties) {
        var dataRecord = $scope.UserGrid.getrowdata(row);
        // return "<a style='margin: 4px;text-decoration:underline;' href='#" + dataRecord.Id + "'> Change Password</a>";
        return "<div class='text-center pad' style='width:120%;'><a ng-click='Edit(" + dataRecord.Id + ")'  href='javascript:;'> Change Password </a></div>";
    }

    var editUserId = 0;
    $scope.Edit = function (Id) {
        editUserId = Id;
        $scope.PasswordValidator.hide();
        $scope.NewPassword = null;

        $scope.UpPass.open();
    }

    $scope.onRefreshClick = function (e) {
        $scope.UserGrid.updatebounddata();
    }

    $scope.onSearch = function (e) {

        $scope.Users.data = { UserName: $scope.SearchUserName, Email: $scope.SearchEmail };
    };

    //debugger;
    $scope.UpdatePasswordClick = function (event) {
        //$scope.PasswordValidator.validate();

        var isValidate = $scope.PasswordValidator.validate();
        if (!isValidate)
            return;

        $.ajax({
            type: "POST",
            url: '/Manage/UpdatePassword',
            data: { NewPassword: $scope.NewPassword, userId: editUserId },
            dataType: "json",
            success: function (msg) {
                if (msg.Status == 1) {
                    $scope.PasswordValidator.hide();
                    $scope.NewPassword = null;
                    $scope.openMessageBox('Success', msg.Message, 200, 90);
                }
                else {
                    $scope.openMessageBox('Error', msg.Message, 'auto', 'auto');
                }

            },
            error: function (msg) {
                $scope.openMessageBox('Error', 'Unknown error occured!', 250, 90);
            }
        });
    };

    $scope.AddUserClick = function (e) {
        ClearData();
        $scope.AddUser.open();
        //ClearData();
    };

    $scope.RoleSource = {
        datatype: "json",
        datafields: [
           { name: 'Id', type: 'int' },
           { name: 'Name', type: 'string' }
        ],
        url: '/Manage/GetUserRole'
    };

    $scope.CreateUserClick = function (e) {
        var isValidate = $scope.UserValidator.validate();
        if (!isValidate)
            return;

        var roleName = $scope.ddlRole.getSelectedItem().label;
        var fname = $scope.model.FirstName;
        var lname = $scope.model.LastName;
        var uname = $scope.model.UserName;
        var email = $scope.model.Email;
        var pass = $scope.model.Password;

        $http.post('/Manage/AddUser', { roleName: roleName, fname: fname, lname: lname, uname: uname, email: email, pass: pass }).success(function (retData) {
            if (retData.Status == 1) {
                $scope.UserGrid.updatebounddata();
                $scope.AddUser.close();
                ClearData();
                $scope.openMessageBox('Success', retData.Message, 200, 90);
            }
            else {
                $scope.openMessageBox('Error', retData.Message, 'auto', 'auto');
            }
        }).error(function (retData, status, headers, config) {

        });
    };

    $scope.onRoleBindingComplete = function (e) {
        //var selectedId = parseInt($scope.ddlRole.val());
        //$scope.PlantSource.data = { SiteId: selectedId };
    }

    var ClearData = function (e) {
        if (typeof ($scope.model) == 'undefined' || $scope.model == null)
            return;
        $scope.model.FirstName = null;
        $scope.model.LastName = null;
        $scope.model.UserName = null;
        $scope.model.Email = null;
        $scope.model.Password = null;
        $scope.ddlRole.selectIndex(0);
        $scope.UserValidator.hide();
    }

    $scope.createWidget = true;
});