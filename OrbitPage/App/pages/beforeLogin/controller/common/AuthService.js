'use strict';
define([appLocation.preLogin], function (app) {
   
    app.factory('AuthApi', ['$resource',
     function ($resource) {

         var headers = {
             'Content-Type': 'application/json',
             'UTMZT': $.cookie('utmzt'),
             'UTMZK': $.cookie('utmzk'),
             'UTMZV': $.cookie('utmzv'),
         };

         return {
             
             ForgetPassword: $resource(
                 ServerContextPath.authServer + '/Auth/ForgetPassword?id=:id', { id: '@id' }, {
                     query: {
                         isArray: false,
                         method: 'GET',
                         headers: headers
                     }
                 }),
         };
     }]);

});