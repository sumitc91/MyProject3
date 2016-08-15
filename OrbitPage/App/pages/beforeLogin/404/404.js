'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLogin404', function ($scope, $http, $rootScope, Restangular, CookieUtil) {
        $('title').html("index"); //TODO: change the title so cann't be tracked in log
        $rootScope.userOrbitFeedList.show = true;
        
    });
});



			

