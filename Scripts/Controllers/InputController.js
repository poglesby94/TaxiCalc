app.controller('InputController', function($scope, $http) {
    $scope.getCost = function(){
         //Function that takes the input form and interacts with the .net app to get the resulting cost.
         //First combine the date and time fields to a single date time object.
         $scope.origin = $scope.combineTime($scope.dateOrigin,$scope.timeOrigin);
          //Connects to the asp.net web app which performs the calculation on the data parameters.
         $http({
              url:"/Calculator/Calc",
              method:"GET",
              params:{o:$scope.origin,s:$scope.slow,f:$scope.fast} //Pass in the values from the field.
         }).then(function(response) {   //If the get request is successful.
                $scope.result = response.data; //Record the resultant json object, which contains all appropriate results.
                $scope.state = "result"; //Set the state so that the result receipt is shown.
           },function(response){   //Otherwise the GET request did not work.
                $scope.state = "error"; //Display the error message box.
           });
    };

    $scope.combineTime = function(date,time){
         //Function that combines the result of 2 date type objects into 1, the first gives the date portion and the second gives the time.
         var result = new Date(date.toISOString());
         result.setUTCHours(time.getUTCHours()); //Work with UTC for easiest preservation of dates wrt timezones.
         var offset = date.getTimezoneOffset()-time.getTimezoneOffset(); //The time object may come from a different time zone(ie when daylight savings are applied and not for the date).
         if(offset!=0){ //If there is a difference we need to ensure that the time is the same HH:MM not the same wrt UTC.
             var trueHour = result.getUTCHours()+(offset/60); //We calculate what the hour should in fact be and change the result to reflect this.
             result.setUTCHours(trueHour);
        }
         result.setUTCMinutes(time.getUTCMinutes()); //Record the minutes and seconds from the time datetime object.
         result.setUTCSeconds(time.getUTCSeconds());
         return result;
    }
});
