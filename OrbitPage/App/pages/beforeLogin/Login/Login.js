'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginLoginPage', function ($scope, $http, $route, $rootScope, $routeParams, $location, $timeout, CookieUtil, OrbitPageApi) {
        $('title').html("index"); //TODO: change the title so cann't be tracked in log
        
        $scope.mobileDevice = mobileDevice != null ? true : false;
        $scope.EmailId = "";
        $scope.Password = "";
        $scope.KeepMeSignedInCheckBox = true;
        $scope.showHeaderErrors = false;
        $scope.showFooterErrors = false;

        if (getParameterByName('returnUrl') != null && getParameterByName('returnUrl') != "null") {
            CookieUtil.setReturnUrl(getParameterByName('returnUrl'), userSession.keepMeSignedIn);
        } else {
            //setReturnUrlInCookie($location.path());
        }
          
        $scope.EmailIdAlert = {
            visible: false,
            message: ''
        };
        $scope.PasswordAlert = {
            visible: false,
            message: ''
        };
        $scope.HeaderAlert = {
            visible: false,
            message: '',
            classType: ''
        };
        $scope.ForgetPasswordAlert = {
            visible: false,
            message: ''
        }
        $scope.userConstants = userConstants;
        $scope.clientConstants = clientConstants;

        if (getParameterByName("type") == "info") {
            $scope.showHeaderErrors = true;
            $scope.HeaderAlert.visible = true;
            $scope.HeaderAlert.classType = "warning";
            $scope.HeaderAlert.message = getParameterByName("mssg");
        }
        if ($routeParams.code == "Password200") {
            showToastMessage("Success", "Password has been successfully changed.");
            $scope.showHeaderErrors = true;
            $scope.HeaderAlert.visible = true;
            $scope.HeaderAlert.classType = "success";
            $scope.HeaderAlert.message = "Your Password has been successfully changed. To continue, please login.";
        }
        if ($routeParams.code == "ConatctUs200") {
            showToastMessage("Success", "Your message has been successfully submitted.");
            $scope.showHeaderErrors = true;
            $scope.HeaderAlert.visible = true;
            $scope.HeaderAlert.classType = "success";
            $scope.HeaderAlert.message = "Your Message has been successfully submitted. To continue, please login.";
        }
        if (CookieUtil.getLoginType() == null || CookieUtil.getLoginType() == "") {
            CookieUtil.setLoginType("user", $scope.KeepMeSignedInCheckBox); // by default set type as user..       
            $('#loginUserTypeRadioButtonId').attr('checked', true);
            $('.userTypeId').html(userConstants.name_abb);
            $scope.userType = userConstants.name_abb;
            $scope.isUser = true;            
        }           
        else {
            if (CookieUtil.getLoginType() == "user") {
                $('#loginUserTypeRadioButtonId').attr('checked', true);
                $('.userTypeId').html(userConstants.name_abb);
                $scope.userType = userConstants.name_abb;
                $scope.isUser = true;               
            }
            else {
                $('#loginClientTypeRadioButtonId').attr('checked', true);
                $('.userTypeId').html(clientConstants.name_abb);
                $scope.userType = clientConstants.name_abb;
                $scope.isUser = false;
                
            }
        }

        $scope.Login = function() {
            $scope.showFooterErrors = false;


            if ($scope.EmailId == null || $scope.EmailId == "") {
                if ($('#LoginEmailId').val() != "" || $('#LoginEmailId').val() != null)
                    $scope.EmailId = $('#LoginEmailId').val();
            }

            if ($scope.Password == null || $scope.Password == "") {
                if ($('#LoginPasswordId').val() != "" || $('#LoginPasswordId').val() != null)
                    $scope.Password = $('#LoginPasswordId').val();
            }

            var userLoginData = {
                Username: $scope.EmailId,
                Password: $scope.Password,
                Type: 'web',
                KeepMeSignedInCheckBox: $scope.KeepMeSignedInCheckBox
            }

            userSession.keepMeSignedIn = $scope.KeepMeSignedInCheckBox;

            var validatePassword = false;
            var validateUsername = false;

            if ($scope.EmailId != "") {
                validateUsername = true;
                $scope.EmailIdAlert.visible = false;
                $scope.EmailIdAlert.message = "";
            } else {
                $scope.EmailIdAlert.visible = true;
                $scope.EmailIdAlert.message = "Enter UserId.";
            }
            if ($scope.Password != "") {
                validatePassword = true;
                $scope.PasswordAlert.visible = false;
                $scope.PasswordAlert.message = "";
            } else {
                $scope.PasswordAlert.visible = true;
                $scope.PasswordAlert.message = "Password cannot be empty.";
            }

            if (validateUsername && validatePassword) {
                startBlockUI('wait..', 3);

                OrbitPageApi.Login.post(userLoginData, function (data) {
                    stopBlockUI();

                    $timeout(function () {
                        if (data.Status == "401") {
                            showToastMessage("Notice", "The username/password combination is incorrect !");
                            $scope.showHeaderErrors = true;
                            $scope.HeaderAlert.visible = true;
                            $scope.HeaderAlert.classType = "danger";
                            $scope.HeaderAlert.message = "The username/password combination you entered is incorrect. Please try again(make sure your caps lock is off).";
                            $scope.ForgetPasswordAlert.visible = true;
                            $scope.ForgetPasswordAlert.message = "Forgot your password?";
                        } else if (data.Status == "500") {
                            $scope.showHeaderErrors = true;
                            $scope.HeaderAlert.visible = true;
                            $scope.HeaderAlert.classType = "danger";
                            $scope.HeaderAlert.message = "Internal server error occured. Please try again.";
                            showToastMessage("Error", "Internal Server Error Occured !");
                        } else if (data.Status == "403") {
                            showToastMessage("Warning", "Your Account is not verified. Please check your mail !");
                            $scope.showHeaderErrors = true;
                            $scope.HeaderAlert.visible = true;
                            $scope.HeaderAlert.classType = "danger";
                            $scope.HeaderAlert.message = "Your Account is not verified yet. Please check your mail and verfiy your account.";
                        } else if (data.Status == "200") {
                            
                            CookieUtil.setUTMZT(data.Payload.UTMZT, userSession.keepMeSignedIn);
                            CookieUtil.setUTMZK(data.Payload.UTMZK, userSession.keepMeSignedIn);
                            CookieUtil.setUTMZV(data.Payload.UTMZV, userSession.keepMeSignedIn);
                            CookieUtil.setUTIME(data.Payload.TimeStamp, userSession.keepMeSignedIn);

                            $.cookie('uservertexid', data.Payload.VertexId, { expires: 365, path: '/', domain: ServerContextPath.cookieDomain });
                            $.cookie('userName', data.Payload.FirstName, { expires: 365, path: '/', domain: ServerContextPath.cookieDomain });
                            $.cookie('userImageUrl', data.Payload.imageUrl, { expires: 365, path: '/', domain: ServerContextPath.cookieDomain });

                            CookieUtil.setKMSI(userSession.keepMeSignedIn, true); // to store KMSI value for maximum possible time.


                            $rootScope.clientDetailResponse.FirstName = data.Payload.FirstName;
                            $rootScope.clientDetailResponse.LastName = data.Payload.LastName;
                            $rootScope.clientDetailResponse.Username = data.Payload.Username;
                            $rootScope.clientDetailResponse.imageUrl = data.Payload.imageUrl;
                            $rootScope.isUserLoggedIn = true;

                            redirectAfterLogin();
                        }
                    });
                    
                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });
            } else {
                $scope.showFooterErrors = true;
                showToastMessage("Error", "Some Fields are Invalid.");
            }

        };

        $scope.openFacebookAuthWindow = function () {
            
            startBlockUI('wait..', 3);

            OrbitPageApi.FBLoginGetRedirectUri.get({}, function (data) {
                
                stopBlockUI();
                if (data.Status == "199") {
                    location.href = data.Message;
                }
                else {
                    showToastMessage("Warning", "some error occured");
                }
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        }

        $scope.openLinkedinAuthWindow = function () {

            startBlockUI('wait..', 3);
            OrbitPageApi.LinkedinLoginGetRedirectUri.get({}, function (data) {

                stopBlockUI();
                if (data.Status == "199") {
                    location.href = data.Message;
                }
                else {
                    alert("some error occured");
                }
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        }

        $scope.openGoogleAuthWindow = function () {

            startBlockUI('wait..', 3);
            OrbitPageApi.GoogleLoginGetRedirectUri.get({}, function (data) {

                stopBlockUI();
                if (data.Status == "199") {
                    location.href = data.Message;
                }
                else {
                    alert("some error occured");
                }
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        }

        $('.TextBoxBeforeLoginFormSubmitButtonClass').keypress(function (e) {
            if (e.keyCode == 13)
                $('#LoginFormSubmitButtonId').click();
        });

        //radiobutton
        $('.loginUserTypeRadioButton').on('change', function () {
            CookieUtil.setLoginType(this.value, userSession.keepMeSignedIn);
            if (this.value == "user") {
                
                $timeout(function () {
                    $scope.userType = userConstants.name_abb;
                    $scope.isUser = true;
                });
                
                $('.userTypeId').html(userConstants.name_abb);

            }
            else {
                $('.userTypeId').html(clientConstants.name_abb);
                $timeout(function () {
                    $scope.userType = clientConstants.name_abb;
                    $scope.isUser = false;
                });
                
            }
        });
    });
    
});



			

