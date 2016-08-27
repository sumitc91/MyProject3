'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginSingleBlog', function ($scope, $http,$routeParams, $rootScope, $timeout, Restangular, CookieUtil, SearchApi) {
        $('title').html("index"); //TODO: change the title so cann't be tracked in log
        
        $scope.WorkgraphyVertexId = $routeParams.blogid;

        getParticularWorkgraphyWithVertexId();
        function getParticularWorkgraphyWithVertexId() {

            startBlockUI('wait..', 3);
            var inputData = { vertexId: $scope.WorkgraphyVertexId };
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



			

