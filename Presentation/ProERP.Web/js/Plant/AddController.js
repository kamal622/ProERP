ProApp.controller("PlantAddController", function ($scope, $http) {

    // now create the widget.

    $scope.createWidget = true;

    $scope.Rules = [
        { input: '#txtPlantName', message: 'Plant name is required', action: 'blur,valuechanged', rule: 'required' }
    ];

    $scope.SavePlant = function () {
        var isValidate = $scope.PlantValidator.validate();
        function apply(){
            if (!isValidate)
                return;
        }
     

    }


});