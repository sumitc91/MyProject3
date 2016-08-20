'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginUrnotice', function ($scope, $interval, $http, $routeParams, $rootScope, $location, Restangular, CookieUtil) {

        $('title').html(window.madetoearn.i18n.beforeLoginOrbitPageCompanyTitle);
        
        $scope.searchBoxText = window.madetoearn.i18n.beforeLoginIndexSearchBoxText;
        $scope.beforeLoginIndexLatestWorkgraphy = window.madetoearn.i18n.beforeLoginIndexLatestWorkgraphy;
        $scope.beforeLoginIndexTopCompanies = window.madetoearn.i18n.beforeLoginIndexTopCompanies;
        $scope.isCollapsed = true;
       
        $scope.myFunct = function(keyEvent) {
            if (keyEvent.which === 13) {
                location.href = "/#search/?q=" + $("#companyName_value").val() + "&page=1&perpage=10";
            }
        };

        $scope.searchCompany = function() {
            location.href = "/#search/?q=" + $("#companyName_value").val() + "&page=1&perpage=10";
        };

        $scope.selectCompany = function (selected) {
            console.log(selected);
            location.href = "/#companydetails/" + selected.originalObject.companyname.replace(/ /g, "_").replace(/\//g, "_OR_") + "/" + selected.originalObject.guid;
            
        };

    });

});


			

