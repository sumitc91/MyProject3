'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginSingleWorkgraphy', function ($scope, $http, $routeParams, $rootScope, $timeout, Restangular, CookieUtil, SearchApi) {
        $('title').html("index"); //TODO: change the title so cann't be tracked in log
        
        $scope.WorkgraphyVertexId = $routeParams.storyid;

        getParticularWorkgraphyWithVertexId();

        function getParticularWorkgraphyWithVertexId() {

            var inputData = { vertexId: $scope.WorkgraphyVertexId };
            startBlockUI('wait..', 3);
            SearchApi.GetParticularWorkgraphyWithVertexId.get(inputData, function (data) {
                stopBlockUI();
                if (data.Status == "200") {
                    $timeout(function () {
                        $scope.WorkGraphyDetail = data.Payload[0];
                    });
                }
                else {
                    showToastMessage("Warning", data.Message);
                }
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        }
    });
});



			

