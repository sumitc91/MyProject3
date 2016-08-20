'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginIndex', function ($scope, $interval, $http, $timeout, $routeParams, $rootScope, $location, Restangular, CookieUtil, SearchApi) {

        $rootScope.userOrbitFeedList.show = false;
        $('title').html(window.madetoearn.i18n.beforeLoginOrbitPageCompanyTitle);
        
        $scope.searchTypeModel = 'Company';


        $scope.beforeLoginIndexLatestWorkgraphy = window.madetoearn.i18n.beforeLoginIndexLatestWorkgraphy;
        $scope.beforeLoginIndexTopCompanies = window.madetoearn.i18n.beforeLoginIndexTopCompanies;
        $scope.isCollapsed = true;
        $scope.searchBackgroundImageList = [
            { url: "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/test/motivational-hd-wallpaper1_1280x700.jpg", textColor: "blackColor" },
            { url: "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/test/cropped-Amazing-Home-Interi.jpg", textColor: "whiteColor" },
            { url: "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/test/Programmers+Wallpapers+HD+by+PCbots_1280x700.jpg", textColor: "whiteColor" },
            { url: "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/test/d92f42382d1ac0ad61a2f772bf5f47aa_1280x700.jpg", textColor: "whiteColor" },
            { url: "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/test/main_1280x700.jpg", textColor: "blackColor" },
            { url: "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/test/big_thumb_c319bfe7d3002b9bf61c605e409963eb_1280x700.jpg", textColor: "whiteColor" },
            { url: "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/test/Modern-Office-Building-In-P.jpg", textColor: "whiteColor" }
            
        ];
        $scope.searchBackgroundImage = "";

        $scope.searchBackgroundImage = $scope.searchBackgroundImageList[0].url;
        $scope.themeTextColor = $scope.searchBackgroundImageList[0].textColor;
        var counter = 0;
        $interval(function () {
            if (counter == $scope.searchBackgroundImageList.length-1)
                counter = 0;
            else
                counter++;
            $scope.searchBackgroundImage = $scope.searchBackgroundImageList[counter].url;
            $scope.themeTextColor = $scope.searchBackgroundImageList[counter].textColor;
        }, 30000, 0);

        $scope.companyDetails = {

        };

        $scope.modifiedSearchValue = function (value) {           
            $scope.searchTypeModel = value;
        };

        $scope.showLandingPageLogo = false;
        var hi = new Vivus('hi-there', { type: 'async', duration: 250, start: 'autostart', dashGap: 30, forceRender: false },
            function() {
                if (window.console) {
                    $timeout(function () {
                        $scope.showLandingPageLogo = true;
                    });
                }
            });
        
        getSolrServiceCompetitors();

        function getSolrServiceCompetitors() {

            var inputData = { size: '10001', rating: '0', speciality: 'Technology (IT,Telecom,Dot Com Etc)' };

            SearchApi.GetCompanyCompetitorsDetail.get(inputData, function (data) {
                if (data.Status == 200) {
                    
                    $timeout(function () {
                        $scope.competitorDetails = data.Payload;
                        $.each(data.Payload, function (i, val) {
                            $scope.competitorDetails[i].companyname = data.Payload[i].companyname;
                            $scope.competitorDetails[i].website = data.Payload[i].website;
                            $scope.competitorDetails[i].linkurl = "/#companydetails/" + $scope.competitorDetails[i].companyname.replace(/ /g, "_").replace(/\//g, "_OR_") + "/" + $scope.competitorDetails[i].guid;

                        });
                    });
                }
            }, function (error) {
                // Error handler code
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        }

        getLatestWorkgraphy();

        function getLatestWorkgraphy() {

            $scope.currentPage = 0;
            $scope.perpage = 6;
            $scope.totalMatch = 10;

            var inputData = { currentPage: $scope.currentPage, perpage: $scope.perpage, totalMatch: $scope.totalMatch };

            SearchApi.GetLatestWorkgraphy.get(inputData, function (data) {
                if (data.Status == 200) {
                    
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

        $scope.myFunct = function(keyEvent) {
            if (keyEvent.which === 13) {
                location.href = "/#search/?q=" + $("#companyName_value").val() + "&page=1&perpage=10";
            }
        };

        $scope.searchCompany = function() {
            location.href = "/#search/?q=" + $("#companyName_value").val() + "&page=1&perpage=10";
        };

        $scope.imageIndex = 2;
        $scope.sliderImage = "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/slider_final_low_3_" + $scope.imageIndex + ".jpg";

        $scope.prevSlide = function () {
            
            $timeout(function () {
                if ($scope.imageIndex == 1)
                    $scope.imageIndex = 3;
                else
                    $scope.imageIndex = $scope.imageIndex - 1;

                $scope.sliderImage = "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/slider_final_low_3_" + $scope.imageIndex + ".jpg";
            });
        };


        $scope.nextSlide = function () {
            
            $timeout(function () {
                if ($scope.imageIndex == 3)
                    $scope.imageIndex = 1;
                else
                    $scope.imageIndex = $scope.imageIndex + 1;

                $scope.sliderImage = "https://s3-ap-southeast-1.amazonaws.com/urnotice/App/img/indexPageSlider/slider_final_low_3_" + $scope.imageIndex + ".jpg";
            });
        };

        $scope.myInterval = 5000;
        $scope.noWrapSlides = false;
        
        
    });

});


			

