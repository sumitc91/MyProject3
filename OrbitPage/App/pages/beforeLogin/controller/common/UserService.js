'use strict';
define([appLocation.preLogin], function (app) {
   
    app.factory('UserApi', ['$resource',
     function ($resource) {

         var headers = {
             'Content-Type': 'application/json',
             'UTMZT': $.cookie('utmzt'),
             'UTMZK': $.cookie('utmzk'),
             'UTMZV': $.cookie('utmzv'),
         };

         return {
             GetCompanySalaryInfo: $resource(
                 ServerContextPath.userServer + '/User/GetCompanySalaryInfo?from=:from&to=:to&vertexId=:companyid', { from: '@from', to: '@to', companyid: '@companyid' }, {
                 query: {
                     isArray: false,
                     method: 'GET',
                     headers: headers
                 }
                }),

             GetCompanyNoticePeriodInfo: $resource(
                ServerContextPath.userServer + '/User/GetCompanyNoticePeriodInfo?from=:from&to=:to&vertexId=:companyid', { from: '@from', to: '@to', companyid: '@companyid' }, {
                    query: {
                        isArray: false,
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetCompanyWorkgraphyInfo: $resource(
                ServerContextPath.userServer + '/User/GetCompanyWorkgraphyInfo?from=:from&to=:to&vertexId=:companyid', { from: '@from', to: '@to', companyid: '@companyid' }, {
                    query: {
                        isArray: false,
                        method: 'GET',
                        headers: headers
                    }
                }),

         };
     }]);

});