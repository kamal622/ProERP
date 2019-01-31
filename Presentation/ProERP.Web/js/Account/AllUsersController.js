ProApp.controller("AllUsersController", function ($scope, $http) {

    $scope.SearchUserName = '';
    $scope.SearchEmail = '';

     $scope.AllUsers = {
        datatype: "json",
        datafields: [
          { name: 'Id', type: 'int' },
          { name: 'UserName', type: 'string' },
          { name: 'Email', type: 'string' },
          
        ],
        url: '/Account/GetAllUsersList',
        data: { UserName: $scope.SearchUserName, Email: $scope.SearchEmail }
    };

     $scope.onRefreshClick = function (e) {
         $scope.GrdUsers.updatebounddata();
     }

     $scope.onSearch = function (e) {

         $scope.AllUsers.data = { UserName: $scope.SearchUserName, Email: $scope.SearchEmail};
     };

});