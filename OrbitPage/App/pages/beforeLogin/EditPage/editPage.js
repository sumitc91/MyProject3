'use strict';
define([appLocation.preLogin], function (app) {

    app.controller('beforeLoginEditPage', function ($scope, $http, $upload, $timeout, $rootScope, CookieUtil, SearchApi, OrbitPageApi) {
        $('title').html("edit page"); //TODO: change the title so cann't be tracked in log
        
        $rootScope.userOrbitFeedList.show = false;
        $scope.loadingUserDetails = false;
        $rootScope.clientDetailResponse = {};
        $scope.disabledEmailModel = "";
        //$('title').html("index"); //TODO: change the title so cann't be tracked in log

        if (CookieUtil.getUTMZT() != null && CookieUtil.getUTMZT() != '' && CookieUtil.getUTMZT() != "") {
            //console.log("cookie available.");
            loadClientDetails();
        } else {
            //console.log("cookie not available.");
        };


        function loadClientDetails() {

            var inputData =
               {
                   userType: 'user'
               };

            startBlockUI('wait..', 3);
            SearchApi.GetDetails.get(inputData, function (data) {
                if (data.Status == "200") {
                    $timeout(function () {
                        stopBlockUI();
                        $scope.loadingUserDetails = false;
                        if (data.Status == "200") {
                            $rootScope.clientDetailResponse = data.Payload;
                            $scope.disabledEmailModel = $rootScope.clientDetailResponse.Email;
                            CookieUtil.setUserName(data.Payload.Firstname + ' ' + data.Payload.LastName, userSession.keepMeSignedIn);
                            CookieUtil.setUserImageUrl(data.Payload.Profilepic, userSession.keepMeSignedIn);
                            $rootScope.isUserLoggedIn = true;
                            if (data.Payload.isLocked == "true") {
                                location.href = "/Auth/LockAccount?status=true";
                            }
                        }
                        else if (data.Status == "404") {

                            alert("This template is not present in database");
                        }
                        else if (data.Status == "500") {

                            alert("Internal Server Error Occured");
                        }
                        else if (data.Status == "401") {
                            $rootScope.isUserLoggedIn = false;
                        }
                    });
                }
                else {
                    showToastMessage("Warning", data.Message);
                }

            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        };

        $scope.submitEditProfile = function() {

            if ($rootScope.clientDetailResponse.Firstname.length > 15) {
                showToastMessage("Warning", "First Name Cann't be greater than 15 characters");
                return;
            }

            if ($rootScope.clientDetailResponse.Lastname.length > 15) {
                showToastMessage("Warning", "Last Name Cann't be greater than 15 characters");
                return;
            }

            var editPersonRequest = {
                FirstName: $rootScope.clientDetailResponse.Firstname,
                LastName: $rootScope.clientDetailResponse.Lastname,
                ImageUrl: $rootScope.clientDetailResponse.Profilepic,
                CoverPic: $rootScope.clientDetailResponse.Coverpic,
                Email: $rootScope.clientDetailResponse.Email
            };

            startBlockUI('wait..', 3);
            OrbitPageApi.EditPersonDetails.post(editPersonRequest, function (data) {
                stopBlockUI();
                showToastMessage("Success", "Successfully Edited");
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        };

        $scope.onCoverPicFileSelectLogoUrl = function ($files) {

            startBlockUI('wait..', 3);            
            for (var i = 0; i < $files.length; i++) {
                var file = $files[i];
                $scope.upload = $upload.upload({
                    url: '/Upload/UploadAngularFileOnImgUr', //UploadAngularFileOnImgUr                    
                    data: { myObj: $scope.myModelObj },
                    file: file, // or list of files ($files) for html5 only                    
                }).progress(function (evt) {
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    stopBlockUI();                    
                    $timeout(function () {
                        $rootScope.clientDetailResponse.Coverpic = data.data.link;
                    });                    
                });
            }
        };

        $scope.onProfilePicFileSelectLogoUrl = function ($files) {

            startBlockUI('wait..', 3);
            for (var i = 0; i < $files.length; i++) {
                var file = $files[i];
                $scope.upload = $upload.upload({
                    url: '/Upload/UploadAngularFileOnImgUr', //UploadAngularFileOnImgUr                    
                    data: { myObj: $scope.myModelObj },
                    file: file, // or list of files ($files) for html5 only                    
                }).progress(function (evt) {
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    stopBlockUI();
                    $timeout(function () {
                        $rootScope.clientDetailResponse.Profilepic = data.data.link;
                    });
                });
            }
        };

    });

});

