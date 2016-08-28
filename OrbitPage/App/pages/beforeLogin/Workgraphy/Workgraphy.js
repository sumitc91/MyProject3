'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginWorkgraphy', function ($scope, $interval, $http, $routeParams, $timeout, $rootScope, $location, Restangular, CookieUtil, SearchApi) {

        $('title').html(window.madetoearn.i18n.beforeLoginOrbitPageCompanyTitle);
        
        $scope.searchBoxText = window.madetoearn.i18n.beforeLoginIndexSearchBoxText;
        $scope.beforeLoginIndexLatestWorkgraphy = window.madetoearn.i18n.beforeLoginIndexLatestWorkgraphy;
        $scope.beforeLoginIndexTopCompanies = window.madetoearn.i18n.beforeLoginIndexTopCompanies;
        
        
        getLatestWorkgraphy();

        function getLatestWorkgraphy() {

            $scope.currentPage = 0;
            $scope.perpage = 10;
            $scope.totalMatch = 10;
            var inputData = { currentPage: $scope.currentPage, perpage: $scope.perpage, totalMatch: $scope.totalMatch };

            startBlockUI('wait..', 3);
            SearchApi.GetLatestWorkgraphy.get(inputData, function (data) {
                if (data.Status == 200) {
                    stopBlockUI();
                    if (data.Status == "200") {
                        $timeout(function () {
                            $scope.totalMatch = data.Message;
                            $scope.LatestWorkGraphyList = data.Payload;
                        });
                    }
                    else {
                        showToastMessage("Warning", data.Message);
                    }
                }
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        }

    });
});


			

