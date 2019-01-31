
ProApp.controller("PlantUpdateController", function ($parse, $scope, $http) {

    // now create the widget.

    $scope.createWidget = true;
   
    $scope.Rules = [
         { input: '#txtPlantName', message: 'Plant name is required!', action: 'keyup, blur', rule: 'required' }
         //{ input: '#txtPlantName', message: 'Plant name is required', action: 'blur', rule: 'required' }
    ];

    $scope.SavePlant = function () {
        debugger;
        var isValidate = $scope.PlantValidator.validate();
            if (!isValidate)
                return;
   
        
    }
});