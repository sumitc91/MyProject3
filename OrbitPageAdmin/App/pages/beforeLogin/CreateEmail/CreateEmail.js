'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('CreateEmailPageController', function ($scope, $http, $route, $rootScope, $uibModal, $log, $routeParams, $location, $timeout, CookieUtil) {
        $('title').html("index"); //TODO: change the title so cann't be tracked in log
        
        $scope.mobileDevice = mobileDevice != null ? true : false;
        
        $scope.emailModel = {
            fromEmail: "",
            fromName: "",
            password: "",
            sendToEmail: "",
            emailHeading: "",
            emailBody:""
        };
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
            
            $scope.emailModel.emailBody = $('#emailBodyId').val();

            $http({
                url: url,
                method: "POST",
                data: $scope.emailModel,
                headers: headers
            }).success(function (data, status, headers, config) {
                //$scope.persons = data; // assign  $scope.persons here as promise is resolved here
                stopBlockUI();
                if(data.Status== 200)
                {
                    showToastMessage("Success", "Successfully sent mail.");
                }
                else if (data.Status == 401) {
                    showToastMessage("Warning", "Password is Incorrect.");
                }
                else {
                    showToastMessage("Warning", "Some Error occured while sending mail.");
                }
            }).error(function (data, status, headers, config) {
                showToastMessage("Warning", "Internal Server Error Occured.");
            });
        
        };
   
    });

});



			

