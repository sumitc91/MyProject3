'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginSearch', function ($scope, $http, $rootScope,$timeout, $routeParams, $location, Restangular, CookieUtil, SearchApi) {
        $('title').html("indexsea"); //TODO: change the title so cann't be tracked in log
        
        $scope.queryParam = {
            q: "",
            currentPage: "",
            totalMatch: "",
            perpage: "",
            totalNumberOfPages:""
        };

        $scope.pagination = {
            show: false,
            maxSize: 5,

        };
        
        $scope.queryParam.q = $location.search().q;
        $scope.queryParam.currentPage = $location.search().page;
        $scope.queryParam.totalMatch = $location.search().totalMatch;
        $scope.queryParam.perpage = $location.search().perpage;
        
        //console.log($scope.queryParam);

        $scope.maxSize = 5;
           
        if ($scope.queryParam.q != null)
            getCompanySearchDetail();

        function getCompanySearchDetail() {
            
            var inputData = {
                q: $scope.queryParam.q,
                page: $scope.queryParam.currentPage,
                perpage: $scope.queryParam.perpage,
                totalMatch: $scope.queryParam.totalMatch
            };

            startBlockUI('wait..', 3);
            SearchApi.Search.get(inputData, function (data) {

                stopBlockUI();
                if (data.Status == "200") {

                    $timeout(function () {
                        $scope.searchResult = data.Payload.result;
                        $scope.queryParam.totalMatch = data.Payload.count;
                        $scope.queryParam.totalNumberOfPages = Math.ceil((data.Payload.count / $scope.queryParam.perpage));

                        $.each(data.Payload.result, function (i, val) {
                            $scope.searchResult[i].companyname = data.Payload.result[i].companyname;
                            $scope.searchResult[i].website = data.Payload.result[i].website;

                            if ($scope.searchResult[i].logourl == 'tps://s3-ap-southeast-1.amazonaws.com/urnotice/company/small/LogoUploadEmpty.png')
                                $scope.searchResult[i].logourl = "http://placehold.it/50x50";

                            $scope.searchResult[i].linkurl = "/#companydetails/" + $scope.searchResult[i].companyname.replace(/ /g, "_").replace(/\//g, "_OR_") + "/" + $scope.searchResult[i].guid;
                            $scope.pagination.show = true;
                        });
                    });
                }
                else {
                    showToastMessage("Warning", data.Message);
                }
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        }

        $scope.selectCompany = function (selected) {
            console.log(selected);            
            location.href = "/#companydetails/" + selected.originalObject.companyname.replace(/ /g, "_").replace(/\//g, "_OR_") + "/" + selected.originalObject.guid;
        };

        $scope.searchCompany = function() {
            location.href = "/#search/?q=" + $("#companyName_value").val() + "&page=1&perpage=10";
        };

        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;

        };

        $scope.myFunct = function (keyEvent) {
            if (keyEvent.which === 13) {
                location.href = "/#search/?q=" + $("#companyName_value").val() + "&page=1&perpage=10";
            }
        }

        $scope.SearchPageSelectPaginationId = function () {
            console.log("/#search/?q=" + $scope.queryParam.q + "&page=" + $scope.queryParam.currentPage + "&perpage=10&totalMatch=" + $scope.queryParam.totalMatch + "");
            //getCompanySearchDetail(q, $scope.currentPage, perpage);
            //getCompanySearchDetail();
            console.log(userSession.selectedPagePagination);
            if (userSession.selectedPagePagination != '...')
                location.href = "/#search/?q=" + $scope.queryParam.q + "&page=" + $scope.queryParam.currentPage + "&perpage=10&totalMatch=" + $scope.queryParam.totalMatch + "";
        };
        
        
        $scope.advanceSearchCriteriaList = [
            {
                searchCriteriaKey: "SEARCHTYPE",
                searchCriteriaValue: "Search Type",
                searchSelectionType: "CHECKBOX",
                searchCriteriaOptions: [
                    {
                        optionKey: "COMPANY",
                        optionValue: " Company",
                        isSelected: false
                    },
                    {
                        optionKey: "USER",
                        optionValue: " User",
                        isSelected: false
                    },
                    {
                        optionKey: "WORKGRAPHY",
                        optionValue: " Workgraphy",
                        isSelected: false
                    }
                ]
            },
            {
                searchCriteriaKey: "RATING",
                searchCriteriaValue: "Rating",
                searchSelectionType: "CHECKBOX",
                searchCriteriaOptions: [
                    {
                        optionKey: "RANGE4PLUS",
                        optionValue: " 4+",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE3TO4",
                        optionValue: " 3 - 4",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE2TO3",
                        optionValue: " 2 - 3",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE1T02",
                        optionValue: " 1 - 2",
                        isSelected: false
                    }
                ]
            },
            {
                searchCriteriaKey: "COMPANYSIZE",
                searchCriteriaValue: "Company Size",
                searchSelectionType: "CHECKBOX",
                searchCriteriaOptions: [
                    {
                        optionKey: "RANGE10001PLUS",
                        optionValue: " 10001+",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE5001TO10000",
                        optionValue: " 5001 - 10000",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE1001TO5000",
                        optionValue: " 1001 - 5000",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE501TO1000",
                        optionValue: " 501 - 1000",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE201TO500",
                        optionValue: " 201 - 500",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE51TO200",
                        optionValue: " 51 - 200",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE11TO50",
                        optionValue: " 11 - 50",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE1TO10",
                        optionValue: "  1 - 10",
                        isSelected: false
                    }
                ]
            },
            {
                searchCriteriaKey: "COMPANYTURNOVER",
                searchCriteriaValue: "Company turn over",
                searchSelectionType: "CHECKBOX",
                searchCriteriaOptions: [
                    {
                        optionKey: "RANGE10000PLUS",
                        optionValue: "  10000+",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE5000TO10000",
                        optionValue: " 5000 - 10000",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE2500TO5000",
                        optionValue: " 2500 - 5000",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE1000TO2500",
                        optionValue: " 1000 - 2500",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE500TO1000",
                        optionValue: " 500 - 1000",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE100TO500",
                        optionValue: "100 - 500",
                        isSelected: false
                    },
                    {
                        optionKey: "RANGE1TO100",
                        optionValue: "1 - 100",
                        isSelected: false
                    }
                ]
            }
        ];
    });

    jQuery(".block-text .success-inner-content").each(function () {
        if (jQuery(this).text().length > 100) {
            var str = jQuery(this).text().substr(0, 98);
            var wordIndex = str.lastIndexOf(" ");

            jQuery(this).text(str.substr(0, wordIndex) + '..');
        }
    });
});



			

