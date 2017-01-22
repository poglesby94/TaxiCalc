//Unit tests for the angular code.
//This is tested using karma.
//Run simply with the command ">karma start".

describe('Calculator Controller', function() {
     beforeEach(module('Calculator')); //Define the angular app we are testing.

     beforeEach(inject(function(_$controller_){ //Ensure the controller property is defined.
         $controller = _$controller_;
     }));

     //Test the combineStrings function, to ensure it functions correctly.
     //We test given 2 inputs dateData[i],timeData[i] that the result of combineStrings is a datetime object with the date of dateData[i] and the time of timeData[i], which is stored in resultData[i].
     describe('$scope.combineStrings(a,b)',function(){

          it('functions correctly with normal data ', function() {
               var $scope = {};
               var controller = $controller('InputController', { $scope: $scope });

               //Test normal data input.
               var dateData = ["2012-12-04T00:00:00-00:00","2017-01-22T00:00:00-00:00"];
               var timeData = ["0000-01-01T03:30:00-00:00","0000-01-01T13:17:37"];
               var resultData = ["2012-12-04T03:30:00-00:00","2017-01-22T13:17:37"];

               for(var i = 0;i<dateData.length;i++){
                    $scope.dateOrigin = new Date(dateData[i]);
                    $scope.timeOrigin = new Date(timeData[i]);
                    $scope.getCost();
                    expect($scope.origin).toEqual(new Date(resultData[i]));
               }

          });

          it('functions correctly with extreme data',function(){
               var $scope = {};
               var controller = $controller('InputController', { $scope: $scope });

               //Test extreme data input [Midnight, 11:59pm, Midnight on 1/1, 11:59pm on 12/31]
               var dateData = ["2012-12-04T00:00:00-00:00","2017-01-22T00:00:00-00:00","2009-01-01T00:00:00-00:00","2007-12-31T00:00:00-00:00"];
               var timeData = ["0000-01-01T00:00:00-00:00","0000-01-01T23:59:59","0000-01-01T00:00:00-00:00","0000-01-01T23:59:59"];
               var resultData = ["2012-12-04T00:00:00-00:00","2017-01-22T23:59:59","2009-01-01T00:00:00-00:00","2007-12-31T23:59:59"];

               for(var i = 0;i<dateData.length;i++){
                    $scope.dateOrigin = new Date(dateData[i]);
                    $scope.timeOrigin = new Date(timeData[i]);
                    $scope.getCost();
                    expect($scope.origin).toEqual(new Date(resultData[i]));
               }

          });
     });

     //Test the getCost function to ensure it either sets the state to "error", or sets the state to "result"
     describe('$scope.getCost()',function(){

          it('correctly sets $scope.state to "error" if it doesnt get a result from the .net app', inject(function($http,$httpBackend) {
               var $scope = {};
               var controller = $controller('InputController', { $scope: $scope });

               //Sample Data
               $scope.slow = 2;
               $scope.fast = 4;
               $scope.dateOrigin = new Date("2012-12-10");
               $scope.timeOrigin = new Date("0000-01-01T18:30:00");

               $scope.getCost();

               //Mock the .net application to return an error code.
               $httpBackend
                 .when('GET','/Calculator/Calc?f=4&o=2012-12-10T18:30:00.000Z&s=2')
                 .respond(404);

               //Resolve the promise
               $httpBackend.flush();

               //Test Criteria
               $httpBackend.expect('GET','/Calculator/Calc?f=4&o=2012-12-10T18:30:00.000Z&s=2'); //Expect a call to the .net application.
               expect($scope.state).toEqual("error"); //Expect to be in "error" state
          }));

          it('correctly sets $scope.state to "result" if the .net app returns a response and records it to $scope.result', inject(function($http,$httpBackend) {
               var $scope = {};
               var controller = $controller('InputController', { $scope: $scope });

               //Sample Data
               $scope.slow = 2;
               $scope.fast = 4;
               $scope.dateOrigin = new Date("2017-01-23");
               $scope.timeOrigin = new Date("0000-01-01T18:30:00");

               $scope.getCost();

               //Mock the .net app to return a correct json object.
               $httpBackend
                 .when('GET','/Calculator/Calc?f=4&o=2017-01-23T18:30:00.000Z&s=2')
                 .respond(200,{slow:2,fast:2,origin:'2017-01-23T18:30:00.000Z',eveningCharge:false,weekdayCharge:true,cost:8.7});

                 //Resolve the promise
                 $httpBackend.flush();

                 //Test Criteria
                 $httpBackend.expect('GET','/Calculator/Calc?f=4&o=2017-01-23T18:30:00.000Z&s=2');
                 expect($scope.state).toEqual("result");
                 expect($scope.result.slow).toEqual(2);
                 expect($scope.result.fast).toEqual(2);
                 expect($scope.result.origin).toEqual("2017-01-23T18:30:00.000Z");
                 expect($scope.result.weekdayCharge).toEqual(true);
                 expect($scope.result.eveningCharge).toEqual(false);
                 expect($scope.result.cost).toEqual(8.7);


          }));
     });

});
