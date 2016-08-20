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
                ServerContextPath.empty + '/User/ResendValidationCode', {}, {
                    post: {
                        method: "POST",
                        isArray: false,
                        headers: headers
                    },
                }),

         };
     }]);

});