'use strict';
define([appLocation.preLogin], function (app) {
    
    app.factory('SearchApi', ['$resource',
     function ($resource) {

         var headers = {
             'Content-Type': 'application/json',
             'UTMZT': $.cookie('utmzt'),
             'UTMZK': $.cookie('utmzk'),
             'UTMZV': $.cookie('utmzv'),
         };

         return {
             GetCompanyCompetitorsDetail: $resource(
                 ServerContextPath.solrServer + '/Search/GetCompanyCompetitorsDetail?size=:size&rating=:rating&speciality=:speciality', { id: '@id' }, {
                 query: {
                     isArray: false,
                     method: 'GET'
                 }
                }),

             GetLatestBlogs: $resource(
                ServerContextPath.solrServer + '/Search/GetLatestBlogs?page=:currentPage&perpage=:perpage&totalMatch=:totalMatch',
                { currentPage: '@currentPage', perpage: '@perpage', totalMatch: '@totalMatch' },
                {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetLatestWorkgraphy: $resource(
                ServerContextPath.solrServer + '/Search/GetLatestWorkgraphy?page=:currentPage&perpage=:perpage&totalMatch=:totalMatch',
                { currentPage: '@currentPage', perpage: '@perpage', totalMatch: '@totalMatch' },
                {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             CompanyDetailsById: $resource(
                ServerContextPath.solrServer + '/Search/CompanyDetailsById?cid=:cid',
                { cid: '@cid' },
                {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             GetDetails: $resource(
                ServerContextPath.solrServer + '/Search/GetDetails?userType=:userType',
                { userType: '@userType'},
                {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             UserDetailsById: $resource(
                ServerContextPath.solrServer + '/Search/UserDetailsById?uid=:uid',
                { uid: '@uid' },
                {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),

             SearchAll: $resource(
                ServerContextPath.solrServer + '/Search/SearchAll?type=:type&q=:q',
                { type: '@type', q: '@q' },
                {
                    get: {
                        method: 'GET',
                        headers: headers
                    }
                }),
         };
     }]);

    //app.factory('SolrServiceUtil', function ($resource) {
    //    return $resource('http://www.orbitpage.com/searchapi/Search/GetCompanyCompetitorsDetail?size=:size&rating=:rating&speciality=:speciality', { id: '@id' }, {
    //        query: {
    //            isArray: false,
    //            method: 'GET'
    //        }
    //    });
    //});

    app.factory('SolrServiceCompanyDetailsById', function ($resource) {
        var headers = {
            'Content-Type': 'application/json',
            'UTMZT': $.cookie('utmzt'),
            'UTMZK': $.cookie('utmzk'),
            'UTMZV': $.cookie('utmzv'),
        };

        return $resource(
            ServerContextPath.solrServer + '/Search/CompanyDetailsById?cid=:cid',
            { cid: '@cid' },
            {
                get: {
                    method: 'GET',
                    headers: headers
                }
            }
       );
    });
});