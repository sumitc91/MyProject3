'use strict';
define([appLocation.preLogin], function (app) {

    app.controller('beforeLoginViewPostDetail', function ($scope, $http, $upload, $timeout, $location, $routeParams, $rootScope, $q, $sce, mentioUtil, CookieUtil, UserApi, OrbitPageApi, SearchApi) {
        $('title').html("edit page"); //TODO: change the title so cann't be tracked in log

        $rootScope.userOrbitFeedList.show = true;
        _.defer(function () { $scope.$apply(); });
        $scope.postVertexId = $routeParams.vertexId;
        var messagesPerCall = 5;
        $scope.UserPostLikesPerCall = 5;

        $scope.CurrentUserDetails = {};
        $scope.UserPostList = [];
        $scope.visitedUserVertexId = 0;
        getPostByVertexId();

        

        $scope.commentOnUserPost = function (postIndex) {
            createNewMessageOnUserPost(postIndex);
        };

        $scope.reactionOnUserPost = function (postIndex) {
            createNewReactionOnUserPost(postIndex);
        };

        $scope.reactionOnUserPostComment = function (postIndex, commentIndex) {
            createNewReactionOnUserPostComment(postIndex, commentIndex);
        };

        $scope.removeReactionOnUserPost = function (postIndex) {
            removeReactionOnUserPost(postIndex);
        };

        $scope.removeReactionOnUserPostComment = function (postIndex, commentIndex) {
            removeReactionOnUserPostComment(postIndex, commentIndex);
        };

        $scope.removeImageOnUserPost = function (postIndex) {
            
            $timeout(function () {
                $scope.UserPostList[postIndex].postInfo.OriginalPostImage = $scope.UserPostList[postIndex].postInfo.PostImage;
                $scope.UserPostList[postIndex].postInfo.PostImage = "";
            });
        };

        $scope.removeImageOnUserPostComment = function (postIndex, commentIndex) {

            $timeout(function () {
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.OriginalPostImage = $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostImage;
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostImage = "";
            });
           
        };

        $scope.NewPostImageUrl = {
            //link_s:"https://s3-ap-southeast-1.amazonaws.com/urnotice/OrbitPage/User/Sumit/WallPost/9ac2bfce-a1eb-4a51-9f18-ad5591a72cc0.png"
        };
        
        $scope.deleteOnUserPostComment = function (postIndex, commentIndex) {
            deleteCommentOnUserPost(postIndex, commentIndex);
        };

        $scope.deleteUserPost = function (postIndex) {
            deleteOnUserPost(postIndex);
        };

        $scope.closeModelAndNavigateTo = function (vid) {
            $(".modal-backdrop.in").hide();
            $('#closeModalId').click();
            $location.url('/userprofile/' + vid);
        };

        $scope.showLikedByUsers = function (postVertexId) {
            $scope.UserPostLikes = [];
            $scope.UserPostLikesFrom = 0;
            $scope.UserPostLikesTo = $scope.UserPostLikesFrom + $scope.UserPostLikesPerCall - 1;
            $scope.UserPostLikesCurrentPostVertexId = postVertexId;
            showLikedByUsersOnUserPost(postVertexId, $scope.UserPostLikesFrom, $scope.UserPostLikesTo);
        };

        $scope.showMoreLikedByUsers = function () {

            $scope.UserPostLikesFrom = $scope.UserPostLikesTo;
            $scope.UserPostLikesTo = $scope.UserPostLikesFrom + $scope.UserPostLikesPerCall - 1;
            showLikedByUsersOnUserPost($scope.UserPostLikesCurrentPostVertexId, $scope.UserPostLikesFrom, $scope.UserPostLikesTo);

        };

        $scope.loadMoreMessage = function (postVerexId, postIndex) {
            
            $scope.UserPostList[postIndex].messageFromIndex = $scope.UserPostList[postIndex].messageToIndex;
            $scope.UserPostList[postIndex].messageToIndex = $scope.UserPostList[postIndex].messageFromIndex + messagesPerCall - 1;
            loadMoreMessage(postVerexId, postIndex, $scope.UserPostList[postIndex].messageFromIndex, $scope.UserPostList[postIndex].messageToIndex);            
        };

        function showLikedByUsersOnUserPost(vertexId, from, to) {
            
            var inputData = { from: from, to: to, vertexId: vertexId };
            $scope.UserPostLikesLoading = true;
            UserApi.GetUserPostLikes.get(inputData, function (data) {

                $timeout(function () {
                    $scope.UserPostLikesLoading = false;
                    for (var i = 0; i < data.results.length; i++) {
                        $scope.UserPostLikes.push(data.results[i]);
                    }
                });

            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        };

        function loadMoreMessage(vertexId, postIndex, from, to) {
            
            $scope.UserPostList[postIndex].loadingIcon = true;

            var inputData = { from: from, to: to, vertexId: vertexId };
            UserApi.GetUserPostMessages.get(inputData, function (data) {

                $timeout(function () {
                    $scope.UserPostList[postIndex].loadingIcon = false;

                    if (data.results != null && data.results.length > 0) {
                        data.results = reverseCommentsInfoList(data.results);
                        $scope.UserPostList[postIndex].commentsInfo = appendOldCommentsToCommentList($scope.UserPostList[postIndex].commentsInfo, data.results);
                    }

                });
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        };

        function createNewReactionOnUserPost(postIndex) {

            var userPostCommentData = {
                Reaction: UserReaction.Like,                
                VertexId: $scope.UserPostList[postIndex].postInfo._id,
                WallVertexId: $scope.UserPostList[postIndex].postedToUser[0]._id,
                PostPostedByVertexId: $scope.UserPostList[postIndex].userInfo[0]._id,
                IsParentPost: true,
                ParentVertexId: $scope.UserPostList[postIndex].postInfo._id
            };

            if ($rootScope.isUserLoggedIn) {
                //startBlockUI('wait..', 3);
                $scope.UserPostList[postIndex].alreadyLiked = true;
                $scope.UserPostList[postIndex].likeInfoHtml = appentToCommentLikeString($scope.UserPostList[postIndex].likeInfoHtml,$scope.UserPostList[postIndex].likeInfoCount);
                $scope.UserPostList[postIndex].likeInfoCount = $scope.UserPostList[postIndex].likeInfoCount + 1;

                OrbitPageApi.UserReactionOnPost.post(userPostCommentData, function (data) {

                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });
                
            } else {
                showToastMessage("Warning", "Please Login to Make your reaction on post.");
            }
            

        };

        function createNewReactionOnUserPostComment(postIndex, commentIndex) {

            var userPostReactionData = {
                Reaction: UserReaction.Like,
                VertexId: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo._id,
                WallVertexId: $scope.visitedUserVertexId,
                PostPostedByVertexId: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentedBy[0]._id,
                IsParentPost: false,
                ParentVertexId: $scope.UserPostList[postIndex].postInfo._id
            };

            if ($rootScope.isUserLoggedIn) {
                //startBlockUI('wait..', 3);
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].alreadyLiked = true;
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].likeCount = $scope.UserPostList[postIndex].commentsInfo[commentIndex].likeCount + 1;

                OrbitPageApi.UserReactionOnPost.post(userPostReactionData, function (data) {

                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });
                
            } else {
                showToastMessage("Warning", "Please Login to Make your reaction on post.");
            }

        };

        function removeReactionOnUserPost(postIndex) {

            var userPostReactionData = {
                Reaction: UserReaction.Like,
                VertexId: $scope.UserPostList[postIndex].postInfo._id,
                WallVertexId: $scope.visitedUserVertexId,
                PostPostedByVertexId: $scope.UserPostList[postIndex].userInfo[0]._id
            };

            if ($rootScope.isUserLoggedIn) {
                $scope.UserPostList[postIndex].alreadyLiked = false;
                $scope.UserPostList[postIndex].likeInfoHtml = removeFromCommentLikeString($scope.UserPostList[postIndex].likeInfoHtml, $scope.UserPostList[postIndex].likeInfoCount);
                $scope.UserPostList[postIndex].likeInfoCount = $scope.UserPostList[postIndex].likeInfoCount - 1;

                OrbitPageApi.RemoveReactionOnPost.post({ vertexId: $scope.UserPostList[postIndex].postInfo._id }, userPostReactionData, function (data) {

                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });

            } else {
                showToastMessage("Warning", "Please Login to Make your reaction on post.");
            }

        };

        function removeReactionOnUserPostComment(postIndex, commentIndex) {

            var userPostReactionData = {
                Reaction: UserReaction.Like,
                VertexId: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo._id,
                WallVertexId: $scope.visitedUserVertexId,
                PostPostedByVertexId: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentedBy[0]._id
            };

            if ($rootScope.isUserLoggedIn) {
                //startBlockUI('wait..', 3);
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].alreadyLiked = false;
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].likeCount = $scope.UserPostList[postIndex].commentsInfo[commentIndex].likeCount - 1;
                //$scope.UserPostList[postIndex].likeInfoCount = $scope.UserPostList[postIndex].likeInfoCount - 1;

                OrbitPageApi.RemoveReactionOnPost.post({ vertexId: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo._id }, userPostReactionData, function (data) {

                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });
                
            } else {
                showToastMessage("Warning", "Please Login to Make your reaction on post.");
            }

        };

        function deleteOnUserPost(postIndex) {

            var userPostReactionData = {
                Reaction: UserReaction.Like,
                VertexId: $scope.UserPostList[postIndex].postInfo._id,
                WallVertexId: $scope.visitedUserVertexId,
                PostPostedByVertexId: $scope.UserPostList[postIndex].userInfo[0]._id
            };

            if ($rootScope.isUserLoggedIn) {
                spliceOnUserPost(postIndex);

                OrbitPageApi.DeleteCommentOnPost.post({ vertexId: userPostReactionData.VertexId }, userPostReactionData, function (data) {

                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });
                
            } else {
                showToastMessage("Warning", "Please Login to Make your reaction on post.");
            }

        };

        function spliceOnUserPost(postIndex) {

            $timeout(function () {
                $scope.UserPostList.splice(postIndex, 1);
            });
           
        };

        function deleteCommentOnUserPost(postIndex, commentIndex) {

            var userPostReactionData = {
                Reaction: UserReaction.Like,
                VertexId: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo._id,
                WallVertexId: $scope.visitedUserVertexId,
                PostPostedByVertexId: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentedBy[0]._id
            };

            if ($rootScope.isUserLoggedIn) {
                spliceCommentOnUserPost(postIndex, commentIndex);


                OrbitPageApi.DeleteCommentOnPost.post({ vertexId: userPostReactionData.VertexId }, userPostReactionData, function (data) {

                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });
                
            } else {
                showToastMessage("Warning", "Please Login to Make your reaction on post.");
            }

        };

        function spliceCommentOnUserPost(postIndex, commentIndex) {
            
            $timeout(function () {
                $scope.UserPostList[postIndex].commentsInfo.splice(commentIndex, 1);
            });
            
        };

        function createNewMessageOnUserPost(postIndex) {

            var userPostCommentData = {
                Message: $scope.UserPostList[postIndex].postInfo.postUserComment,
                Image: $scope.UserPostList[postIndex].postInfo.newCommentImage,
                VertexId: $scope.UserPostList[postIndex].postInfo._id,
                WallVertexId: $scope.UserPostList[postIndex].postedToUser[0]._id,
                PostPostedByVertexId: $scope.UserPostList[postIndex].userInfo[0]._id
            };

            if (isNullOrEmpty($scope.UserPostList[postIndex].postInfo.postUserComment) && isNullOrEmpty($scope.UserPostList[postIndex].postInfo.newCommentImage)) {
                showToastMessage("Warning", "You cannot submit empty message.");
                return;
            }

            var newCommentPosted = {
                "commentInfo": {
                    "PostImage": $scope.UserPostList[postIndex].postInfo.newCommentImage,
                    "PostedByUser": $rootScope.clientDetailResponse.Email,
                    "PostedTime": new Date($.now()),
                    "PostMessage": $scope.UserPostList[postIndex].postInfo.postUserComment,
                    "PostMessageHtml": replaceTextWithLinks($scope.UserPostList[postIndex].postInfo.postUserComment),
                    "_id": "",
                    "_type": null
                },
                "commentedBy": [
                    {
                        "FirstName": $rootScope.clientDetailResponse.Firstname,
                        "LastName": $rootScope.clientDetailResponse.Lastname,
                        "Username": $rootScope.clientDetailResponse.Email,
                        "Gender": $rootScope.clientDetailResponse.Gender,
                        "CreatedTime": new Date($.now()),
                        "ImageUrl": $rootScope.clientDetailResponse.Profilepic,
                        "CoverImageUrl": $rootScope.clientDetailResponse.Coverpic,
                        "_id": $rootScope.clientDetailResponse.VertexId,
                        "_type": null
                    }
                ],
                "isAuthenticToEdit": true,
                "likeCount": 0,
                "isLiked": []
            };

            if ($rootScope.isUserLoggedIn) {

                $scope.UserPostList[postIndex].postInfo.postUserComment = "";
                $scope.UserPostList[postIndex].postInfo.newCommentImage = "";
                $scope.UserPostList[postIndex].commentsInfo.push(newCommentPosted);
                var commentAddedAtIndex = $scope.UserPostList[postIndex].commentsInfo.length - 1;

                $scope.UserPostList[postIndex].commentsInfo[commentAddedAtIndex].loadingIcon = true;
                $scope.UserPostList[postIndex].commentsInfo[0].disableInputBox = true;

                OrbitPageApi.UserCommentOnPost.post(userPostCommentData, function (data) {

                    $timeout(function () {
                        $scope.UserPostList[postIndex].commentsInfo[commentAddedAtIndex].loadingIcon = false;
                        $scope.UserPostList[postIndex].commentsInfo[commentAddedAtIndex].commentInfo._id = data.Payload.commentInfo._id;
                        $scope.UserPostList[postIndex].commentsInfo[0].disableInputBox = false;
                        $scope.userPostCommentData = "";
                        $scope.NewPostImageUrl.link_s = "";
                    });
                }, function (error) {
                    showToastMessage("Error", "Internal Server Error Occured!");
                });

            } else {
                showToastMessage("Warning", "Please Login to reply on post.");
            }
            

        };

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
                        
                    $timeout(function () {
                        $scope.NewPostImageUrl = data.data;
                    });
                    
                        
                    
                });

            }

        };

        $scope.onEditFileSelectLogoUrl = function ($files, postIndex) {

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

                    $timeout(function () {
                        $scope.UserPostList[postIndex].postInfo.PostImage = data.data.link_s;
                    });

                });

            }

        };
        
        $scope.onCommentImageFileUpload = function ($files, postIndex) {

            console.log("postIndex : " + postIndex);
            console.log();
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

                    $timeout(function () {
                        $scope.UserPostList[postIndex].postInfo.newCommentImage = data.data.link_s;
                    });

                });

            }

        };

        $scope.onEditFileSelectLogoUrlForComments = function ($files, postIndex, commentIndex) {

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

                    $timeout(function () {
                        $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostImage = data.data.link_s;
                    });

                });

            }

        };

        $scope.removeUploadedCommentImage = function (postIndex) {
            $scope.UserPostList[postIndex].postInfo.newCommentImage = "";
        };

        function getPostByVertexId() {
           
            startBlockUI('wait..', 3);

            var inputData = { vertexId: $scope.postVertexId };
            UserApi.GetPostByVertexId.get(inputData, function (data) {
                stopBlockUI();
                $timeout(function () {
                    if ($scope.UserPostList != null && data.results.length > 0) {
                        var absoluteIndex = 0; // only 1 post available.
                        $scope.visitedUserVertexId = data.results[absoluteIndex].postedToUser[0]._id;
                        data.results[absoluteIndex].postInfo.PostMessageHtml = replaceTextWithLinks(data.results[absoluteIndex].postInfo.PostMessage);
                        if (data.results[absoluteIndex].commentsInfo != null && data.results[absoluteIndex].commentsInfo.length > 0) {
                            data.results[absoluteIndex].commentsInfo = reverseCommentsInfoList(data.results[absoluteIndex].commentsInfo);
                        }

                        $scope.UserPostList.push(data.results[absoluteIndex]);

                        $scope.UserPostList[absoluteIndex].likeInfoHtml = parseCommentLikeString($scope.UserPostList[absoluteIndex].likeInfo, $scope.UserPostList[absoluteIndex].likeInfoCount);
                        $scope.UserPostList[absoluteIndex].messageFromIndex = 0;
                        $scope.UserPostList[absoluteIndex].messageToIndex = $scope.UserPostList[absoluteIndex].messageFromIndex + messagesPerCall - 1;
                        if ($scope.UserPostList[absoluteIndex].isLiked != null && $scope.UserPostList[absoluteIndex].isLiked.length > 0) {
                            $scope.UserPostList[absoluteIndex].alreadyLiked = true;
                        } else {
                            $scope.UserPostList[absoluteIndex].alreadyLiked = false;
                        }

                    } else {
                        showToastMessage("Warning", "Post Not found.");
                    }
                });
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });
        };

        function appendOldCommentsToCommentList(oldList, newList) {
            var newCommentList = [];
            for (var j = 0; j < newList.length; j++) {
                newCommentList.push(newList[j]);
            }

            for (var k = 0; k < oldList.length; k++) {
                newCommentList.push(oldList[k]);
            }
            return newCommentList;
        }

        function reverseCommentsInfoList(newList) {
            var reversedList = [];
            for (var i = newList.length - 1; i >= 0; i--) {
                newList[i].editableMode = false;

                if (newList[i].isLiked.length > 0) {
                    newList[i].alreadyLiked = true;
                }

                if (newList[i].commentInfo.PostedByUser == $rootScope.clientDetailResponse.Email) {
                    newList[i].isAuthenticToEdit = true;
                } else {
                    newList[i].isAuthenticToEdit = false;
                }

                if ($scope.visitedUserVertexId == $rootScope.clientDetailResponse.VertexId) {
                    newList[i].isLoggedInUserWall = true;
                } else {
                    newList[i].isLoggedInUserWall = false;
                }

                if (newList.length > 0) {
                    newList[i].commentInfo.PostMessageHtml = replaceTextWithLinks(newList[i].commentInfo.PostMessage);
                }

                reversedList.push(newList[i]);
            }
            return reversedList;
        }

        function parseCommentLikeString(likeInfo, likeInfoCount) {
            var str = "";
            //console.log("likeInfo " + likeInfo);
            for (var i = 0; i < likeInfo.length; i++) {
                str += " " + likeInfo[i].FirstName + " " + likeInfo[i].LastName;
                if (i != likeInfo.length - 1) {
                    str += ",";
                } else {
                    str += " ";
                }
            }
            if (likeInfoCount > 2) {
                str += "and " + (likeInfoCount - likeInfo.length) + " more liked this";
            } else {
                if (likeInfoCount > 0)
                    str += " liked this";
                else
                    str += "be the first one to like this";
            }
            return str;
        };

        function appentToCommentLikeString(str, likeInfoCount) {
            if (str == null) str = "";
            if (likeInfoCount > 0) {
                //str = "<a href='#/userprofile/" + $rootScope.clientDetailResponse.VertexId + "'>" + $rootScope.clientDetailResponse.Firstname + " " + $rootScope.clientDetailResponse.Lastname + "</a>," + str;
                str = "" + $rootScope.clientDetailResponse.Firstname + " " + $rootScope.clientDetailResponse.Lastname + "," + str;
            } else {
                str = "" + $rootScope.clientDetailResponse.Firstname + " " + $rootScope.clientDetailResponse.Lastname + " liked this";
            }
            return str;
        };

        function removeFromCommentLikeString(str, likeInfoCount) {
            if (str == null) str = "";
            if (likeInfoCount <= 1) {
                //str = "<a href='#/userprofile/" + $rootScope.clientDetailResponse.VertexId + "'>" + $rootScope.clientDetailResponse.Firstname + " " + $rootScope.clientDetailResponse.Lastname + "</a>," + str;
                str = "be the first one to like this";
            } else {
                str = str.replace($rootScope.clientDetailResponse.Firstname + " " + $rootScope.clientDetailResponse.Lastname + ",", "").replace($rootScope.clientDetailResponse.Firstname + " " + $rootScope.clientDetailResponse.Lastname, "");
            }
            return str;
        };

        $scope.enableEditOnUserPost = function (postIndex) {

            $timeout(function () {
                $scope.UserPostList[postIndex].postInfo.editableMode = true;
                $scope.UserPostList[postIndex].postInfo.OriginalPostMessage = $scope.UserPostList[postIndex].postInfo.PostMessage;
                $scope.UserPostList[postIndex].postInfo.OriginalPostImage = $scope.UserPostList[postIndex].postInfo.PostImage;
            });
            
        };

        $scope.cancelEditOnUserPost = function (postIndex) {

            $timeout(function () {
                $scope.UserPostList[postIndex].postInfo.editableMode = false;
                $scope.UserPostList[postIndex].postInfo.PostMessage = $scope.UserPostList[postIndex].postInfo.OriginalPostMessage;
                $scope.UserPostList[postIndex].postInfo.PostImage = $scope.UserPostList[postIndex].postInfo.OriginalPostImage;
            });
            
        };

        $scope.submitEditOnUserPost = function (postIndex) {

            $timeout(function () {
                $scope.UserPostList[postIndex].postInfo.editableMode = false;
            });

            submitEditPost(postIndex);
        };

        function submitEditPost(postIndex) {

            if (isNullOrEmpty($scope.UserPostList[postIndex].postInfo.PostMessage)) {
                showToastMessage("Warning", "You cann't submit empty message.");
                return;
            }

            //if ($scope.UserPostList[postIndex].postInfo.PostMessage == $scope.UserPostList[postIndex].postInfo.OriginalPostMessage) {
            //    //showToastMessage("Warning", "You cann't submit empty message.");
            //    return;
            //}

            var editMessageRequest = {
                message: $scope.UserPostList[postIndex].postInfo.PostMessage,
                imageUrl: $scope.UserPostList[postIndex].postInfo.PostImage,
                messageVertex: $scope.UserPostList[postIndex].postInfo._id,
                userVertex: $rootScope.clientDetailResponse.VertexId,
                userEmail: $rootScope.clientDetailResponse.Email,
                wallVertex: $scope.visitedUserVertexId
            };

            $scope.UserPostList[postIndex].postInfo.loadingIcon = true;

            OrbitPageApi.EditMessageDetails.post(editMessageRequest, function (data) {

                $timeout(function () {
                    $scope.UserPostList[postIndex].postInfo.loadingIcon = false;
                });

                showToastMessage("Success", "Successfully Edited");
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });

        };

        $scope.uploadImageOncomment = function (postIndex) {

            $timeout(function () {
                $scope.currentUploadingPostIndex = postIndex;
            });
            
            document.getElementById('my_comment_file').click();
        };

        $scope.enableEditcommentOnUserPost = function (postIndex, commentIndex) {

            $timeout(function () {
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].editableMode = true;
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.OriginalPostMessage = $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessage;
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.OriginalPostImage = $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostImage;
            });
        };

        $scope.cancelEditcommentOnUserPost = function (postIndex, commentIndex) {
            $timeout(function () {
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].editableMode = false;
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessage = $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.OriginalPostMessage;
            });
        };

        $scope.submitEditcommentOnUserPost = function (postIndex, commentIndex) {

            $timeout(function () {
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].editableMode = false;
            });
            
            submitEditMessage(postIndex, commentIndex);
        };

        function submitEditMessage(postIndex, commentIndex) {

            if (isNullOrEmpty($scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessage)) {
                showToastMessage("Warning", "You cann't submit empty message.");
                return;
            }

            //if ($scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessage == $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.OriginalPostMessage) {
            //    //showToastMessage("Warning", "You cann't submit empty message.");
            //    return;
            //}

            var editMessageRequest = {
                message: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessage,
                imageUrl: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostImage,
                messageVertex: $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo._id,
                userVertex: $rootScope.clientDetailResponse.VertexId,
                userEmail: $rootScope.clientDetailResponse.Email,
                wallVertex: $scope.visitedUserVertexId
            };

            $scope.UserPostList[postIndex].commentsInfo[commentIndex].loadingIcon = true;

            OrbitPageApi.EditMessageDetails.post(editMessageRequest, function (data) {

                $timeout(function () {
                    $scope.UserPostList[postIndex].commentsInfo[commentIndex].loadingIcon = false;
                });

                showToastMessage("Success", "Successfully Edited");
            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });

        };

        $scope.searchPeople = function (term) {
            var peopleList = [];
            if (term.length < 2) {
                $scope.people = [];
                peopleList = [];
                return $q.when(peopleList);
            }

            var inputData = { type: 'All', q: term };
            return SearchApi.SearchAll.get(inputData, function (data) {
                peopleList = data.Payload;
                $timeout(function () {
                    $scope.people = peopleList;
                });

                return $q.when(peopleList);

            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });

        };

        $scope.searchCommentPeople = function (postIndex, term) {
            var peopleList = [];
            if (term.length < 2) {
                $scope.people = [];
                peopleList = [];
                return $q.when(peopleList);
            }
            $scope.UserPostList[postIndex].postInfo.startedSearch = true;

            var inputData = { type: 'All', q: term };
            return SearchApi.SearchAll.get(inputData, function (data) {
                peopleList = data.Payload;
                $timeout(function () {
                    $scope.people = peopleList;
                });

                return $q.when(peopleList);

            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });

        };

        $scope.searchPostCommentPeople = function (postIndex, commentIndex, term) {
            var peopleList = [];
            if (term.length < 2) {
                $scope.people = [];
                peopleList = [];
                return $q.when(peopleList);
            }
            $scope.UserPostList[postIndex].commentsInfo[commentIndex].startedSearch = true;

            var inputData = { type: 'All', q: term };
            return SearchApi.SearchAll.get(inputData, function (data) {
                peopleList = data.Payload;
                $timeout(function () {
                    $scope.people = peopleList;
                });

                return $q.when(peopleList);

            }, function (error) {
                showToastMessage("Error", "Internal Server Error Occured!");
            });

        };

        $scope.getPeopleText = function (item) {
            // note item.label is sent when the typedText wasn't found
            return '[~<i>' + (item.name || item.label) + '</i>]';
        };

        $scope.getPeopleTextRaw = function (item) {
            //return '@' + item.name;
            $scope.people = [];
            return '@[tag:' + replaceAll(replaceAll(replaceAll(item.name, ',', '_'), '-', '_'), ' ', '_') + '|' + item.vertexId + '|' + item.type + ']';
        };

        $scope.getPeopleCommentTextRaw = function (postIndex, item) {
            //return '@' + item.name;
            $scope.people = [];
            $timeout(function () {
                $scope.UserPostList[postIndex].postInfo.startedSearch = false;
            }, 250);

            return '@[tag:' + replaceAll(replaceAll(replaceAll(item.name, ',', '_'), '-', '_'), ' ', '_') + '|' + item.vertexId + '|' + item.type + ']';
        };

        $scope.getPeoplePostCommentTextRaw = function (postIndex, commentIndex, item) {
            //return '@' + item.name;
            $scope.people = [];
            $timeout(function () {
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].startedSearch = false;
            }, 250);

            return '@[tag:' + replaceAll(replaceAll(replaceAll(item.name, ',', '_'), '-', '_'), ' ', '_') + '|' + item.vertexId + '|' + item.type + ']';
        };

        $scope.updateUserNewPostMessageHtml = function () {
            var re = /\@\[tag:.\w+\|+.\d+\|\d]/gm;

            var match;
            var toReplace = [];
            var replacedWith = [];
            $scope.UserPostMessageHtmlTaggedVertexId = [];
            while (match = re.exec($scope.UserPostMessage)) {
                // full match is in match[0], whereas captured groups are in ...[1], ...[2], etc.
                //console.log(match[0]);
                toReplace.push(match[0]);
                var userInfo = match[0].replace('@[tag:', '').split('|');
                userInfo[2] = userInfo[2].replace(']', '');
                if (userInfo[2] == '1') {
                    replacedWith.push("@<a href='/#/userprofile/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostMessageHtmlTaggedVertexId.push({ Type: 1, VertexId: userInfo[1] });
                }
                else if (userInfo[2] == '2') {
                    replacedWith.push("@<a href='/#companydetails/" + userInfo[0].replace(' ', '_') + "/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostMessageHtmlTaggedVertexId.push({ Type: 2, VertexId: userInfo[1] });
                }

            }
            $scope.UserPostMessageHtml = $scope.UserPostMessage;
            for (var i = 0; i < toReplace.length; i++) {
                //console.log("toReplace : " + toReplace[i]);
                //console.log("replacedWith : " + replacedWith[i]);
                $scope.UserPostMessageHtml = $scope.UserPostMessageHtml.replace(toReplace[i], replacedWith[i]);
            }

            //console.log("$scope.UserPostMessageHtml : " + $scope.UserPostMessageHtml);
        };

        $scope.updateUserCommentMessageHtml = function (postIndex) {
            var re = /\@\[tag:.\w+\|+.\d+\|\d]/gm;

            var match;
            var toReplace = [];
            var replacedWith = [];
            $scope.UserPostList[postIndex].postInfo.postUserCommentHtmlTaggedVertexId = [];
            while (match = re.exec($scope.UserPostList[postIndex].postInfo.postUserComment)) {
                // full match is in match[0], whereas captured groups are in ...[1], ...[2], etc.
                //console.log(match[0]);
                toReplace.push(match[0]);
                var userInfo = match[0].replace('@[tag:', '').split('|');
                userInfo[2] = userInfo[2].replace(']', '');
                if (userInfo[2] == '1') {
                    replacedWith.push("@<a href='/#/userprofile/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostList[postIndex].postInfo.postUserCommentHtmlTaggedVertexId.push({ Type: 1, VertexId: userInfo[1] });
                }
                else if (userInfo[2] == '2') {
                    replacedWith.push("@<a href='/#companydetails/" + userInfo[0].replace(' ', '_') + "/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostList[postIndex].postInfo.postUserCommentHtmlTaggedVertexId.push({ Type: 2, VertexId: userInfo[1] });
                }

            }
            $scope.UserPostList[postIndex].postInfo.postUserCommentHtml = $scope.UserPostList[postIndex].postInfo.postUserComment;
            for (var i = 0; i < toReplace.length; i++) {
                //console.log("toReplace : " + toReplace[i]);
                //console.log("replacedWith : " + replacedWith[i]);
                $scope.UserPostList[postIndex].postInfo.postUserCommentHtml = $scope.UserPostList[postIndex].postInfo.postUserCommentHtml.replace(toReplace[i], replacedWith[i]);
            }

            //console.log("$scope.UserPostMessageHtml : " + $scope.UserPostList[postIndex].postInfo.postUserCommentHtml);
        };

        $scope.updateUserPostCommentMessageHtml = function (postIndex, commentIndex) {
            var re = /\@\[tag:.\w+\|+.\d+\|\d]/gm;

            var match;
            var toReplace = [];
            var replacedWith = [];
            $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessageHtmlTaggedVertexId = [];
            while (match = re.exec($scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessage)) {
                // full match is in match[0], whereas captured groups are in ...[1], ...[2], etc.
                //console.log(match[0]);
                toReplace.push(match[0]);
                var userInfo = match[0].replace('@[tag:', '').split('|');
                userInfo[2] = userInfo[2].replace(']', '');
                if (userInfo[2] == '1') {
                    replacedWith.push("@<a href='/#/userprofile/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessageHtmlTaggedVertexId.push({ Type: 1, VertexId: userInfo[1] });
                }
                else if (userInfo[2] == '2') {
                    replacedWith.push("@<a href='/#companydetails/" + userInfo[0].replace(' ', '_') + "/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessageHtmlTaggedVertexId.push({ Type: 2, VertexId: userInfo[1] });
                }

            }
            $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessageHtml = $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessage;
            for (var i = 0; i < toReplace.length; i++) {
                //console.log("toReplace : " + toReplace[i]);
                //console.log("replacedWith : " + replacedWith[i]);
                $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessageHtml = $scope.UserPostList[postIndex].commentsInfo[commentIndex].commentInfo.PostMessageHtml.replace(toReplace[i], replacedWith[i]);
            }

            //console.log("$scope.UserPostMessageHtml : " + $scope.UserPostList[postIndex].postInfo.postUserCommentHtml);
        };

        $scope.updateUserPostMessageHtml = function (postIndex) {
            var re = /\@\[tag:.\w+\|+.\d+\|\d]/gm;

            var match;
            var toReplace = [];
            var replacedWith = [];
            $scope.UserPostList[postIndex].postInfo.PostMessageHtmlTaggedVertexId = [];
            while (match = re.exec($scope.UserPostList[postIndex].postInfo.PostMessage)) {
                // full match is in match[0], whereas captured groups are in ...[1], ...[2], etc.
                //console.log(match[0]);
                toReplace.push(match[0]);
                var userInfo = match[0].replace('@[tag:', '').split('|');
                userInfo[2] = userInfo[2].replace(']', '');
                if (userInfo[2] == '1') {
                    replacedWith.push("@<a href='/#/userprofile/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostList[postIndex].postInfo.PostMessageHtmlTaggedVertexId.push({ Type: 1, VertexId: userInfo[1] });
                }
                else if (userInfo[2] == '2') {
                    replacedWith.push("@<a href='/#companydetails/" + userInfo[0].replace(' ', '_') + "/" + userInfo[1] + "'>" + userInfo[0] + "</a>");
                    $scope.UserPostList[postIndex].postInfo.PostMessageHtmlTaggedVertexId.push({ Type: 2, VertexId: userInfo[1] });
                }

            }
            $scope.UserPostList[postIndex].postInfo.PostMessageHtml = $scope.UserPostList[postIndex].postInfo.PostMessage;
            for (var i = 0; i < toReplace.length; i++) {
                //console.log("toReplace : " + toReplace[i]);
                //console.log("replacedWith : " + replacedWith[i]);
                $scope.UserPostList[postIndex].postInfo.PostMessageHtml = $scope.UserPostList[postIndex].postInfo.PostMessageHtml.replace(toReplace[i], replacedWith[i]);
            }

            //console.log("$scope.UserPostMessageHtml : " + $scope.UserPostList[postIndex].postInfo.postUserCommentHtml);
        };
    });

      
});

