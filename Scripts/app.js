var app = angular.module('Calculator', ['ngRoute','ngResource']); //Define our angular app.
app.config(function($routeProvider) {
     //The routing app simply reroutes any url of the form http://localhost/#/*** to http://localhost/#/input, which is where our angular application is.
   $routeProvider
    .when("/input", {
        templateUrl : "partialviews/input.html", //The view for our object is stored seperately.
        controller: "InputController" //Ensure we use the correct angular controller.
    })
    .otherwise({ //Redirect all angular pages to the /input page.
        redirectTo : "/input"
    });

});
