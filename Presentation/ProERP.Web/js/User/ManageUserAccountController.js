ProApp.controller("ManageUserAccountController", function ($parse, $scope, $http) {

    $scope.Rules = [
             { input: '#fname', message: 'First Name is required!', action: 'blur', rule: 'required' },
             { input: '#lname', message: 'Last Name is required!', action: 'blur', rule: 'required' },
             { input: '#email', message: 'Email is required!', action: 'blur', rule: 'required' },
             { input: '#email', message: 'Email is incorrect!', action: 'blur', rule: 'email' }
    ];

    $scope.OnUpdate = function (event) {
        var fname = $('#fname').val();
        var lname = $('#lname').val();
        var email = $('#email').val();

        $http.post('/Manage/UpdateUser', { fname: fname, lname: lname, email: email}).success(function (retData) {
            if (retData.Status == 1) {
                $scope.openMessageBox('Success', retData.Message, 300, 90);
            }
            else {
                $scope.openMessageBox('Error', retData.Message, 'auto', 'auto');
            }
        });

    };
});