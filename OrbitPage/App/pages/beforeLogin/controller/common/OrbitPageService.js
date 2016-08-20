'use strict';
define([appLocation.preLogin], function (app) {
   
    app.factory('OrbitPageApi', ['$resource',
     function ($resource) {

         var headers = {
             'Content-Type': 'application/json',
             'UTMZT': $.cookie('utmzt'),
             'UTMZK': $.cookie('utmzk'),
             'UTMZV': $.cookie('utmzv'),
         };

         return {
             
             EditPersonDetails: $resource(
                ServerContextPath.empty + '/User/EditPersonDetails', {}, {
                    post: {
                        method: "POST",
                        isArray: false,
                        headers: headers
                    },
                }),

             ResendValidationCode: $resource(
                ServerContextPath.empty + '/Auth/ResendValidationCode', {}, {
                    post: {
                        method: "POST",
                        isArray: false,
                        headers: headers
                    },
                }),

             Login: $resource(
                ServerContextPath.empty + '/Auth/Login', {}, {
                    post: {
                        method: "POST",
                        isArray: false,
                        headers: headers
                    },
                }),

             FBLoginGetRedirectUri: $resource(
                 ServerContextPath.empty + '/SocialAuth/FBLoginGetRedirectUri', { }, {
                     query: {
                         isArray: false,
                         method: 'GET',
                         headers: headers
                     }
                 }),

             LinkedinLoginGetRedirectUri: $resource(
                 ServerContextPath.empty + '/SocialAuth/LinkedinLoginGetRedirectUri', {}, {
                     query: {
                         isArray: false,
                         method: 'GET',
                         headers: headers
                     }
                 }),

             GoogleLoginGetRedirectUri: $resource(
                 ServerContextPath.empty + '/SocialAuth/GoogleLoginGetRedirectUri', {}, {
                     query: {
                         isArray: false,
                         method: 'GET',
                         headers: headers
                     }
                 }),
         };
     }]);

});