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
                    get: {
                     method: 'GET',
                     headers: headers
                 }
                }),

             GetCompanyNoticePeriodInfo: $resource(
                ServerContextPath.userServer + '/User/GetCompanyNoticePeriodInfo?from=:from&to=:to&vertexId=:companyid', { from: '@from', to: '@to', companyid: '@companyid' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetCompanyWorkgraphyInfo: $resource(
                ServerContextPath.userServer + '/User/GetCompanyWorkgraphyInfo?from=:from&to=:to&vertexId=:companyid', { from: '@from', to: '@to', companyid: '@companyid' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),


             GetUserPostLikes: $resource(
                ServerContextPath.userServer + '/User/GetUserPostLikes?from=:from&to=:to&vertexId=:vertexId', { from: '@from', to: '@to', vertexId: '@vertexId' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetUserNetworkDetail: $resource(
                ServerContextPath.userServer + '/User/GetUserNetworkDetail?from=:from&to=:to&vertexId=:vertexId', { from: '@from', to: '@to', vertexId: '@vertexId' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetUserPostMessages: $resource(
                ServerContextPath.userServer + '/User/GetUserPostMessages?from=:from&to=:to&vertexId=:vertexId', { from: '@from', to: '@to', vertexId: '@vertexId' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetUserOrbitFeedPost: $resource(
                ServerContextPath.userServer + '/User/GetUserOrbitFeedPost?from=:from&to=:to&vertexId=:vertexId', { from: '@from', to: '@to', vertexId: '@vertexId' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             SeenNotification: $resource(
                ServerContextPath.userServer + '/User/SeenNotification', { }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetPostByVertexId: $resource(
                ServerContextPath.userServer + '/User/GetPostByVertexId?vertexId=:vertexId', { vertexId: ':vertexId' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetNotificationDetails: $resource(
                ServerContextPath.userServer + '/User/GetNotificationDetails?from=:from&to=:to', { from: '@from', to: '@to' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetFriendRequestNotificationDetails: $resource(
                ServerContextPath.userServer + '/User/GetFriendRequestNotificationDetails?from=:from&to=:to', { from: '@from', to: '@to' }, {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

         };
     }]);

});