'use strict';
define([appLocation.preLogin], function (app) {
    app.controller('beforeLoginPostBlog', function ($scope, $http, $upload, $timeout, $rootScope, $window, $location, Restangular, CookieUtil, OrbitPageApi) {
        $('title').html("index"); //TODO: change the title so cann't be tracked in log

        $scope.PostStoryModel = {
            heading: "",
            companyName: "",
            companyVertexId:"",
            story: "",
            name: "",
            email: "",
            designation: "",
            designationVertexId:"",
            location: "",
            shareAnonymously: false,
            employeeType:""

        };

        $scope.details = {
            address_components: "",
            formatted_address:""
        };
        $scope.refreshModeratingPhotosListDiv = function() {
            $scope.imgurImageTemplateModeratingPhotos = userSession.imgurImageTemplateModeratingPhotos;
            $('.fancybox').fancybox();
        }

        $scope.DeleteEditImgurImageByIdFunction = function(id) {
            var i;
            for (i = 0; i < userSession.imgurImageTemplateModeratingPhotos.length; i++) {
                if (userSession.imgurImageTemplateModeratingPhotos[i].data.id == id) {
                    break;
                }
            }
            userSession.imgurImageTemplateModeratingPhotos.splice(i, 1);
            $scope.imgurImageTemplateModeratingPhotos = userSession.imgurImageTemplateModeratingPhotos;

            $('.fancybox').fancybox();
        }

        $scope.onFileSelectLogoUrl = function ($files) {

            startBlockUI('wait..', 3);
            //$files: an array of files selected, each file has name, size, and type.
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

                    userSession.imgurImageTemplateModeratingPhotos.push(data);
                    $scope.refreshModeratingPhotosListDiv();
                    //angular.element(document.getElementById('ModeratingPhotosViewAfterUploadId')).scope().refreshModeratingPhotosListDiv(); 
                    console.log("moderationgphotosscript");
                    console.log(userSession.imgurImageTemplateModeratingPhotos);

                    $timeout(function () {
                        $scope.NewPostImageUrl = data.data;
                    });

                });

            }

        };

        $scope.onFileSelect = function ($files) {

            startBlockUI('wait..', 3);
            //$files: an array of files selected, each file has name, size, and type.
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

                    //userSession.imgurImageTemplateModeratingPhotos.push(data);
                    //$scope.refreshModeratingPhotosListDiv();
                    //angular.element(document.getElementById('ModeratingPhotosViewAfterUploadId')).scope().refreshModeratingPhotosListDiv(); 
                    //console.log("moderationgphotosscript");
                    

                    $rootScope.wysiHTML5InputImageTextBoxId = data.data.link_m;
                    console.log($scope.wysiHTML5InputImageTextBoxId);

                    $timeout(function () {
                        $scope.NewPostImageUrl = data.data;
                    });

                });

            }

        };
        $scope.InsertPostStoryContent = function() {

            var PostStoryContentData = $('#PostStoryContentData').val();
            //console.log(addFancyBoxInImages);
            var i = 0;
            $.each(userSession.wysiHtml5UploadedInstructionsImageUrlLink, function() {

                PostStoryContentData = replaceImageWithFancyBoxImage(PostStoryContentData, userSession.wysiHtml5UploadedInstructionsImageUrlLink[i].link_s, userSession.wysiHtml5UploadedInstructionsImageUrlLink[i].link);
                i++;
            });


            //$('#TextBoxQuestionTextBoxQuestionData').data("wysihtml5").editor.clear();
            refreshPostStoryPreview(PostStoryContentData);
        }

        function refreshPostStoryPreview(PostStoryContentData) {

            $('#PostStoryIDPreview').html(PostStoryContentData);
            //$('#addQuestionTextBoxAnswerCloseButton').click();
            refreshComponentsAfterEdit();
        }

        function refreshComponentsAfterEdit() {
            $('.fancybox').fancybox();
        }

        function replaceImageWithFancyBoxImage(text, smallImage, largeImage) {
            //console.log(text);
            //console.log("<img src=\"" + smallImage + "\" title=\"Image: " + smallImage + "\">");

            text = text.replace("<img title=\"Image: " + smallImage + "\" src=\"" + smallImage + "\">", "<a class='fancybox' href='" + largeImage + "' data-fancybox-group='gallery' title='Personalized Title'><img class='MaxUploadedSmallSized' src='" + smallImage + "' alt=''></a>");
            text = text.replace("<img src=\"" + smallImage + "\" title=\"Image: " + smallImage + "\">", "<a class='fancybox' href='" + largeImage + "' data-fancybox-group='gallery' title='Personalized Title'><img class='MaxUploadedSmallSized' src='" + smallImage + "' alt=''></a>");
            text = text.replace("<img src=\"" + smallImage + "\">", "<a class='fancybox' href='" + largeImage + "' data-fancybox-group='gallery' title='Personalized Title'><img class='MaxUploadedSmallSized' src='" + smallImage + "' alt=''></a>");
            return text;
        }

        $scope.selectedDesignation = function (selected) {
            //console.log(selected);
            $scope.PostStoryModel.designation = selected.description.designation;
            $scope.PostStoryModel.designationVertexId = selected.description.vertexId;
            //console.log($scope.PostStoryModel);
            //location.href = "/#companydetails/" + selected.originalObject.companyname.replace(/ /g, "_").replace(/\//g, "_OR_") + "/" + selected.originalObject.guid;

        };

        $scope.selectedCompany = function (selected) {
            console.log(selected);
            $scope.PostStoryModel.companyName = selected.description.companyname;
            $scope.PostStoryModel.companyVertexId = selected.description.guid;
            //console.log($scope.PostStoryModel);
            //location.href = "/#companydetails/" + selected.originalObject.companyname.replace(/ /g, "_").replace(/\//g, "_OR_") + "/" + selected.originalObject.guid;

        };

        $scope.SubmitJobStoryToServer = function () {

            if ($rootScope.isUserLoggedIn == true) {
                $scope.PostStoryModel.email = $rootScope.clientDetailResponse.Email;
            }

            $scope.PostStoryModel.story = $('#PostStoryContentData').val();
            $scope.PostStoryModel.subTitle = $('#PostStoryContentData').val().replace(/&nbsp;/g, '').replace(/(<([^>]+)>)/ig, "");

            if (isNullOrEmpty($scope.PostStoryModel.story)) {
                showToastMessage("Success", "Body cannot be Blank");
                return;
            }

            if (isNullOrEmpty($scope.PostStoryModel.heading)) {
                showToastMessage("Success", "Heading cannot be Blank");
                return;
            }

            var jobStoryData = { Data: $scope.PostStoryModel, ImgurList: userSession.imgurImageTemplateModeratingPhotos, location: $scope.details.address_components, formatted_address: $scope.details.formatted_address };

            startBlockUI('wait..', 3);
            OrbitPageApi.CreateBlog.post(jobStoryData, function (data) {
                stopBlockUI();
                $window.location.reload();
                showToastMessage("Success", "Successfully Created");
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
            
        };

        function clearPostStoryScreen() {
            $scope.PostStoryModel.companyname = "";
            $scope.PostStoryModel.companyVertexId = "";
            $scope.PostStoryModel.designation = "";
            $scope.PostStoryModel.designationVertexId = "";
            $scope.PostStoryModel.story = "";
            $scope.PostStoryModel.heading = "";
            userSession.listOfImgurImages = [];
            $('#PostStoryContentData').html();
        }

        $scope.result = ''
        //    $scope.details = ''
        $scope.options = {};

        $scope.form = {
            type: 'geocode',
            //bounds: { SWLat: 49, SWLng: -97, NELat: 50, NELng: -96 },
            //country: 'ca',
            typesEnabled: false,
            boundsEnabled: false,
            componentEnabled: false,
            watchEnter: true
        }

        //watch form for changes
        $scope.watchForm = function () {
            //showToastMessage("Success", "Pressed Enter");
            return $scope.form;
        };
        $scope.$watch($scope.watchForm, function () {
            $scope.checkForm();
        }, true);


        //set options from form selections
        $scope.checkForm = function () {

            $scope.options = {};

            $scope.options.watchEnter = $scope.form.watchEnter;

            
        };
    });

});



			

