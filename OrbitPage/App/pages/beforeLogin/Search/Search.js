'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginSearch', function ($scope, $http, $rootScope,$timeout, $routeParams, $location, Restangular, CookieUtil, SearchApi) {
        $('title').html("indexsea"); //TODO: change the title so cann't be tracked in log
        
        $scope.queryParam = {
            q: "",
            currentPage: "",
            totalMatch: "",
            perpage: "",
            totalNumberOfPages: "",
            searchType: "",
            searchCriteria: "",
            rating: "",
            ratingList: [],
            companySize: "",
            companySizeList: [],
            companyTurnOver: "",
            companyTurnOverList: [],
            userMutualFriendsType: "",
            datePosted:""
        };

        $scope.pagination = {
            show: false,
            maxSize: 5,

        };
        
        $scope.queryParam.q = $location.search().q;
        $scope.queryParam.currentPage = $location.search().page;
        $scope.queryParam.totalMatch = $location.search().totalMatch;
        $scope.queryParam.perpage = $location.search().perpage;
        $scope.queryParam.searchType = $location.search().searchType;
        $scope.queryParam.searchCriteria = $location.search().searchCriteria;
        $scope.queryParam.rating = $location.search().rating;
        $scope.queryParam.companySize = $location.search().companySize;
        $scope.queryParam.companyTurnOver = $location.search().companyTurnOver;
        $scope.queryParam.userMutualFriendsType = $location.search().userMutualFriendsType;
        $scope.queryParam.datePosted = $location.search().datePosted;

        if ($scope.queryParam.searchType == null)
            $scope.queryParam.searchType = "COMPANY";

        if ($scope.queryParam.searchCriteria == null)
            $scope.queryParam.searchCriteria = "CONTAINS";

        if ($scope.queryParam.rating == null)
            $scope.queryParam.rating = "";

        if ($scope.queryParam.companySize == null)
            $scope.queryParam.companySize = "";

        if ($scope.queryParam.companyTurnOver == null)
            $scope.queryParam.companyTurnOver = "";

        if ($scope.queryParam.userMutualFriendsType == null)
            $scope.queryParam.userMutualFriendsType = ""

        if ($scope.queryParam.datePosted == null)
            $scope.queryParam.datePosted = ""

        if ($scope.queryParam.rating)
        {
            $timeout(function () {
                $scope.queryParam.ratingList = $scope.queryParam.rating.split(',');
                for (var i = 0; i < $scope.advanceSearchCriteriaList[2].searchCriteriaOptions.length; i++) {
                    if ($scope.queryParam.ratingList.indexOf($scope.advanceSearchCriteriaList[2].searchCriteriaOptions[i].optionKey) !== -1) {
                        $scope.advanceSearchCriteriaList[2].searchCriteriaOptions[i].isSelected = true;
                    }
                }
            });
            
        }
        
        if ($scope.queryParam.companySize) {
            $timeout(function () {
                $scope.queryParam.companySizeList = $scope.queryParam.companySize.split(',');
                for (var i = 0; i < $scope.advanceSearchCriteriaList[3].searchCriteriaOptions.length; i++) {
                    if ($scope.queryParam.companySize.indexOf($scope.advanceSearchCriteriaList[3].searchCriteriaOptions[i].optionKey) !== -1) {
                        $scope.advanceSearchCriteriaList[3].searchCriteriaOptions[i].isSelected = true;
                    }
                }
            });
        }

        if ($scope.queryParam.companyTurnOver) {
            $timeout(function () {
                $scope.queryParam.companyTurnOverList = $scope.queryParam.companyTurnOver.split(',');
                for (var i = 0; i < $scope.advanceSearchCriteriaList[4].searchCriteriaOptions.length; i++) {
                    if ($scope.queryParam.companyTurnOver.indexOf($scope.advanceSearchCriteriaList[4].searchCriteriaOptions[i].optionKey) !== -1) {
                        $scope.advanceSearchCriteriaList[4].searchCriteriaOptions[i].isSelected = true;
                    }
                }
            });
        }


        $scope.maxSize = 5;
           
        if ($scope.queryParam.q != null)
            getCompanySearchDetail();

        function getCompanySearchDetail() {
            
            var inputData = {
                q: $scope.queryParam.q,
                page: $scope.queryParam.currentPage,
                perpage: $scope.queryParam.perpage,
                totalMatch: $scope.queryParam.totalMatch,
                searchType: $scope.queryParam.searchType,
                searchCriteria: $scope.queryParam.searchCriteria,
                rating: $scope.queryParam.rating,
                companySize: $scope.queryParam.companySize,
                companyTurnOver: $scope.queryParam.companyTurnOver,
                userMutualFriendsType: $scope.queryParam.userMutualFriendsType,
                datePosted: $scope.queryParam.datePosted
            };

            startBlockUI('wait..', 3);
            SearchApi.Search.get(inputData, function (data) {

                stopBlockUI();
                if (data.Status == "200") {

                    $timeout(function () {
                        $scope.searchResult = data.Payload.searchResult;
                        $scope.queryParam.totalMatch = data.Payload.searchCount;
                        $scope.queryParam.totalNumberOfPages = Math.ceil((data.Payload.searchCount / $scope.queryParam.perpage));

                        $.each($scope.searchResult, function (i, val) {
                            $scope.searchResult[i].heading = data.Payload.searchResult[i].heading;
                            $scope.searchResult[i].url = data.Payload.searchResult[i].url;

                            if ($scope.searchResult[i].logoUrl == 'tps://s3-ap-southeast-1.amazonaws.com/urnotice/company/small/LogoUploadEmpty.png')
                                $scope.searchResult[i].logoUrl = "http://placehold.it/50x50";

                            $scope.searchResult[i].linkurl = "/#companydetails/" + $scope.searchResult[i].heading.replace(/ /g, "_").replace(/\//g, "_OR_") + "/" + $scope.searchResult[i].vertexId;
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

        $scope.changeSearchSelection = function (inputType,searchCriteriaKey,optionKey,parentIndex,index) {
            //console.log("inputType : " + inputType);
            //console.log("searchCriteriaKey : " + searchCriteriaKey);
            //console.log("optionKey : " + optionKey);
            //console.log("parentIndex : " + parentIndex);
            //console.log("index : " + index);

            switch (searchCriteriaKey) {
                case 'SEARCHTYPE':
                    $scope.queryParam.searchType = optionKey;
                    break;
                case 'SEARCHCRITERIA':
                    $scope.queryParam.searchCriteria = optionKey;
                    break;
                case 'USERMUTUALFRIENDSTYPE':
                    $scope.queryParam.userMutualFriendsType = optionKey;
                    break;
                case 'DATEPOSTED':
                    $scope.queryParam.datePosted = optionKey;
                    break;
                case 'RATING':
                    if ($scope.queryParam.ratingList.indexOf(optionKey) !== -1) {
                        var index = $scope.queryParam.ratingList.indexOf(optionKey);
                        $scope.queryParam.ratingList.splice(index, 1);
                    }
                    else
                    {
                        $scope.queryParam.ratingList.push(optionKey);
                    }
                    break;
                case 'COMPANYSIZE':
                    if ($scope.queryParam.companySize.indexOf(optionKey) !== -1) {
                        var index = $scope.queryParam.companySizeList.indexOf(optionKey);
                        $scope.queryParam.companySizeList.splice(index, 1);
                    }
                    else {
                        $scope.queryParam.companySizeList.push(optionKey);
                    }
                    break;
                case 'COMPANYTURNOVER':
                    if ($scope.queryParam.companyTurnOver.indexOf(optionKey) !== -1) {
                        var index = $scope.queryParam.companyTurnOverList.indexOf(optionKey);
                        $scope.queryParam.companyTurnOverList.splice(index, 1);
                    }
                    else {
                        $scope.queryParam.companyTurnOverList.push(optionKey);
                    }
                    break;
                default:

            }

            var url = "";
            if ($scope.advanceSearchCriteriaList[0].searchCriteriaOptedValue == "COMPANY")
            {
                url = "/#search/?" +
                    "q=" + $scope.queryParam.q +
                    "&page=" + $scope.queryParam.currentPage +
                    "&perpage=10" +
                    "&totalMatch=" + $scope.queryParam.totalMatch +
                    "&searchType=" + $scope.queryParam.searchType +
                    "&searchCriteria=" + $scope.queryParam.searchCriteria +
                    "&rating=" + $scope.parseArrayForQueryString($scope.queryParam.ratingList) +
                    "&companySize=" + $scope.parseArrayForQueryString($scope.queryParam.companySizeList) +
                    "&companyTurnOver=" + $scope.parseArrayForQueryString($scope.queryParam.companyTurnOverList) +
                    "";
            }
            else if ($scope.advanceSearchCriteriaList[0].searchCriteriaOptedValue == "USER") {
                url = "/#search/?" +
                    "q=" + $scope.queryParam.q +
                    "&page=" + $scope.queryParam.currentPage +
                    "&perpage=10" +
                    "&totalMatch=" + $scope.queryParam.totalMatch +
                    "&searchType=" + $scope.queryParam.searchType +
                    "&searchCriteria=" + $scope.queryParam.searchCriteria +
                    "&userMutualFriendsType=" + $scope.queryParam.userMutualFriendsType +
                    "";
            }
            else if ($scope.advanceSearchCriteriaList[0].searchCriteriaOptedValue == "WORKGRAPHY") {
                url = "/#search/?" +
                    "q=" + $scope.queryParam.q +
                    "&page=" + $scope.queryParam.currentPage +
                    "&perpage=10" +
                    "&totalMatch=" + $scope.queryParam.totalMatch +
                    "&searchType=" + $scope.queryParam.searchType +
                    "&searchCriteria=" + $scope.queryParam.searchCriteria +
                    "&datePosted=" + $scope.queryParam.datePosted +
                    "";
            }
            //console.log(url);
            location.href = url;
        };

        $scope.parseArrayForQueryString = function(arr)
        {
            return arr.join(',');
        };

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
                searchSelectionType: "radio",
                searchCriteriaOptedValue: $scope.queryParam.searchType,
                searchCriteriaVisisbleWith: "ALWAYS",
                searchCriteriaOptions: [
                    {
                        optionKey: "COMPANY",
                        optionValue: " Company",
                        isSelected: true
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
                searchCriteriaKey: "SEARCHCRITERIA",
                searchCriteriaValue: "Search Criteria",
                searchSelectionType: "radio",
                searchCriteriaOptedValue: $scope.queryParam.searchCriteria,
                searchCriteriaVisisbleWith: "ALWAYS",
                searchCriteriaOptions: [
                    {
                        optionKey: "CONTAINS",
                        optionValue: " Contains",
                        isSelected: false
                    },
                    {
                        optionKey: "STARTSWITH",
                        optionValue: " Starts With",
                        isSelected: false
                    },
                    {
                        optionKey: "EXACTMATCH",
                        optionValue: " Exact Match",
                        isSelected: false
                    }
                ]
            },
            {
                searchCriteriaKey: "RATING",
                searchCriteriaValue: "Rating",
                searchSelectionType: "checkbox",
                searchCriteriaVisisbleWith: "COMPANY",
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
                searchSelectionType: "checkbox",
                searchCriteriaVisisbleWith: "COMPANY",
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
                searchSelectionType: "checkbox",
                searchCriteriaVisisbleWith: "COMPANY",
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
            },

            //Users
            {
                searchCriteriaKey: "USERMUTUALFRIENDSTYPE",
                searchCriteriaValue: "Mutual Friends",
                searchSelectionType: "radio",
                searchCriteriaVisisbleWith: "USER",
                searchCriteriaOptedValue: $scope.queryParam.userMutualFriendsType,
                searchCriteriaOptions: [
                    {
                        optionKey: "ANYONE",
                        optionValue: "  Anyone",
                        isSelected: false
                    },
                    {
                        optionKey: "FRIENDS",
                        optionValue: " Friends",
                        isSelected: false
                    },
                    {
                        optionKey: "FRIENDSOFFRIENDS",
                        optionValue: " Friends of your Friends",
                        isSelected: false
                    }
                ]
            },

            //Workgraphy

                {
                searchCriteriaKey: "DATEPOSTED",
                searchCriteriaValue: "Date Posted",
                searchSelectionType: "radio",
                searchCriteriaVisisbleWith: "WORKGRAPHY",
                searchCriteriaOptedValue: $scope.queryParam.datePosted,
                searchCriteriaOptions: [
                    {
                        optionKey: "OPTION2016",
                        optionValue: "  2016",
                        isSelected: false
                    },
                    {
                        optionKey: "OPTION2015",
                        optionValue: " 2015",
                        isSelected: false
                    },
                    {
                        optionKey: "OPTION2014",
                        optionValue: " 2014",
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



			

