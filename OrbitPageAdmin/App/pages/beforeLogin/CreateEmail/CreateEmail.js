'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('CreateEmailPageController', function ($scope, $http, $route, $rootScope, $uibModal, $log, $routeParams, $location, $timeout, CookieUtil) {
        $('title').html("index"); //TODO: change the title so cann't be tracked in log
        
        $scope.mobileDevice = mobileDevice != null ? true : false;
        
        $scope.sendEmail = function () {
            sendEmailModule();
        };

        function sendEmailModule() {
            var url = ServerContextPath.empty + '/Auth/SendEmail';
            var headers = {
                'Content-Type': 'application/json',
                'UTMZT': CookieUtil.getUTMZT(),
                'UTMZK': CookieUtil.getUTMZK(),
                'UTMZV': CookieUtil.getUTMZV()
            };
            //startBlockUI('wait..', 3);
            
            $http({
                url: url,
                method: "POST",
                data: $scope.emailModel,
                headers: headers
            }).success(function (data, status, headers, config) {
                //$scope.persons = data; // assign  $scope.persons here as promise is resolved here
                stopBlockUI();
                
            }).error(function (data, status, headers, config) {

            });
        
        };
   
    });

});



			

