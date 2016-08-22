using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Newtonsoft.Json;
using SolrNet;
using urNotice.Common.Infrastructure.Common.Config;
using urNotice.Common.Infrastructure.Common.Constants;
using urNotice.Common.Infrastructure.Common.Enum;
using urNotice.Common.Infrastructure.commonMethods;
using urNotice.Common.Infrastructure.Encryption;
using urNotice.Common.Infrastructure.Model.Person;
using urNotice.Common.Infrastructure.Model.urNoticeModel.AssetClass;
using urNotice.Common.Infrastructure.Model.urNoticeModel.DynamoDb;
using urNotice.Common.Infrastructure.Model.urNoticeModel.GraphModel;
using urNotice.Common.Infrastructure.Model.urNoticeModel.RequestWrapper;
using urNotice.Common.Infrastructure.Model.urNoticeModel.RequestWrapper.EditProfile;
using urNotice.Common.Infrastructure.Model.urNoticeModel.ResponseWrapper;
using urNotice.Common.Infrastructure.Model.urNoticeModel.Solr;
using urNotice.Common.Infrastructure.Model.urNoticeModel.User;
using urNotice.Common.Infrastructure.Session;
using urNotice.Common.Infrastructure.signalRPushNotifications;
using urNotice.Services.Email.EmailTemplate;
using urNotice.Services.GraphDb;
using urNotice.Services.GraphDb.GraphDbContract;
using urNotice.Services.Management.AccountManagement;
using urNotice.Services.Management.CompanyManagement;
using urNotice.Services.Management.NotificationManagement;
using urNotice.Services.Management.PostManagement;
using urNotice.Services.NoSqlDb.DynamoDb;
using urNotice.Services.Person.PersonContract.LoginOperation;
using urNotice.Services.Person.PersonContract.RegistrationOperation;
using urNotice.Services.Solr.SolrCompany;
using urNotice.Services.Solr.SolrUser;
using urNotice.Services.Factory.AccountManagement;
using urNotice.Services.Factory.PostManagement;
using urNotice.Services.Factory.NotificationManagement;
using urNotice.Services.Factory.CompanyManagement;
using urNotice.Common.Infrastructure.Model.urNoticeModel.EmailModel;

namespace urNotice.Services.Person
{
    public class Consumer : IPerson
    {
        //private delegate void ContactUsEmailSendDelegate(String emails, ContactUsRequest req);

        //Account Management.
        public ResponseModel<LoginResponse> RegisterMe(RegisterationRequest req, HttpRequestBase request)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.RegisterMe(req,request);
        }
        public ResponseModel<LoginResponse> SocialRegisterMe(RegisterationRequest req, HttpRequestBase request)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.SocialRegisterMe(req,request);
        }
        public ResponseModel<LoginResponse> Login(string userName, string password, bool isSocialLogin)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.Login(userName, password, isSocialLogin);            
        }
        public ResponseModel<OrbitPageUser> GetFullUserDetail(string userEmail)
        {
            throw new NotImplementedException();
        }
        public ResponseModel<ClientDetailsModel> GetPersonDetails(string userEmail)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetPersonDetails(userEmail);
        }
        public ResponseModel<EditPersonModel> EditPersonDetails(urNoticeSession session, EditPersonModel editPersonModel)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.EditPersonDetails(session,editPersonModel);
        }

        // Account Management - For Gremling Query
        public string GetUserNotification(urNoticeSession session, string from, string to)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserNotification(session, from, to);
        }

        public string GetAllFollowers(string vertexId)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetAllFollowers(vertexId);
        }

        public string GetUserFriendRequestNotification(urNoticeSession session, string from, string to)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserFriendRequestNotification(session, from, to);
        }

        public string GetUserPost(string userVertexId, string @from, string to, string userEmail)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserPost(userVertexId, from, to, userEmail);
        }
        public string GetUserOrbitFeedPost(string userVertexId, string @from, string to, string userEmail)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserOrbitFeedPost(userVertexId, from, to, userEmail);
        }
        public string GetUserPostMessages(string userVertexId, string @from, string to, string userEmail)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserPostMessages(userVertexId, from, to, userEmail);
        }
        public string GetUserPostLikes(string userVertexId, string @from, string to)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserPostLikes(userVertexId, from, to);
        }
        public string GetPostByVertexId(string vertexId, string userEmail)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetPostByVertexId(vertexId, userEmail);
        }

        public string GetUserNetworkDetail(urNoticeSession session,string vertexId, string @from, string to)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserNetworkDetail(session, vertexId, from, to);
        }

        public long GetUserUnreadNotificationCount(urNoticeSession session)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserUnreadNotificationCount(session);
        }
        public long GetUserUnreadFriendRequestNotificationCount(urNoticeSession session)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.GetUserUnreadFriendRequestNotificationCount(session);
        }
        public ResponseModel<IDictionary<string, string>> UserConnectionRequest(urNoticeSession session, UserConnectionRequestModel userConnectionRequestModel, out HashSet<string> sendNotificationHashSetResponse)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.UserConnectionRequest(session, userConnectionRequestModel, out sendNotificationHashSetResponse);
        }

        //Post Management.
        public ResponseModel<string> EditMessageDetails(urNoticeSession session, EditMessageRequest messageReq)
        {
            string version = OrbitPageVersionConstants.v1;
            IPostManagement postManagementModel = PostManagementFactory.GetPostManagementInstance(version);
            return postManagementModel.EditMessageDetails(session,messageReq);
        }
        public ResponseModel<UserPostVertexModel> CreateNewUserPost(urNoticeSession session, string message, string image, string userWallVertexId, List<TaggedVertexIdModel> taggedVertexId, out HashSet<string> sendNotificationResponse)
        {
            string version = OrbitPageVersionConstants.v1;
            IPostManagement postManagementModel = PostManagementFactory.GetPostManagementInstance(version);
            return postManagementModel.CreateNewUserPost(session,message,image,userWallVertexId,taggedVertexId,out sendNotificationResponse);  
        }


        public HashSet<string> SendNotificationToUser(urNoticeSession session, string userWallVertexId, string postVertexId, string commentVertexId, string postPostedByVertexId, string notificationType, List<TaggedVertexIdModel> taggedVertexId)
        {
            string version = OrbitPageVersionConstants.v1;
            INotificationManagement notificationManagement = NotificationManagementFactory.GetNotificationManagement(version);
            return notificationManagement.SendNotificationToUser(session, userWallVertexId, postVertexId, commentVertexId, postPostedByVertexId, notificationType, taggedVertexId);
        }
        public ResponseModel<UserPostCommentModel> CreateNewCommentOnUserPost(urNoticeSession session, string message, string image, string postVertexId, string userWallVertexId, string postPostedByVertexId,List<TaggedVertexIdModel> taggedVertexId, out HashSet<string> sendNotificationResponse)
        {
            string version = OrbitPageVersionConstants.v1;
            IPostManagement postManagementModel = PostManagementFactory.GetPostManagementInstance(version);
            return postManagementModel.CreateNewCommentOnUserPost(session,message,image,postVertexId,userWallVertexId,postPostedByVertexId, taggedVertexId,out sendNotificationResponse);
        }
        public ResponseModel<String> DeleteCommentOnPost(urNoticeSession session, string vertexId)
        {
            string version = OrbitPageVersionConstants.v1;
            IPostManagement postManagementModel = PostManagementFactory.GetPostManagementInstance(version);
            return postManagementModel.DeleteCommentOnPost(session,vertexId);
        }
        public ResponseModel<UserVertexModel> CreateNewReactionOnUserPost(urNoticeSession session, UserNewReactionRequest userNewReactionRequest,List<TaggedVertexIdModel> taggedVertexId, out HashSet<string> sendNotificationResponse)
        {
            string version = OrbitPageVersionConstants.v1;
            IPostManagement postManagementModel = PostManagementFactory.GetPostManagementInstance(version);
            return postManagementModel.CreateNewReactionOnUserPost(session,userNewReactionRequest,taggedVertexId,out sendNotificationResponse);
        }
        public ResponseModel<String> RemoveReactionOnUserPost(urNoticeSession session, string vertexId)
        {
            string version = OrbitPageVersionConstants.v1;
            IPostManagement postManagementModel = PostManagementFactory.GetPostManagementInstance(version);
            return postManagementModel.RemoveReactionOnUserPost(session,vertexId);
        }
        

        public ResponseModel<string> ValidateAccountService(ValidateAccountRequest req)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.ValidateAccountService(req);
        }
        public ResponseModel<string> ResendValidationCodeService(ValidateAccountRequest req, HttpRequestBase request)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.ResendValidationCodeService(req,request);
        }
        public ResponseModel<string> ForgetPasswordService(string id, HttpRequestBase request)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.ForgetPasswordService(id,request);
        }
        public ResponseModel<string> ResetPasswordService(ResetPasswordRequest req)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.ResetPasswordService(req);
        }
        public ResponseModel<string> ContactUsService(ContactUsRequest req)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.ContactUsService(req);
        }
        public ResponseModel<string> SeenNotification(string userName)
        {
            string version = OrbitPageVersionConstants.v1;
            IAccountManagement accountManagementModel = AccountManagementFactory.GetAccountManagementInstance(version);
            return accountManagementModel.SeenNotification(userName);
        }


        //Company Management
        public SolrQueryResults<UnCompanySolr> CompanyDetailsById(string userVertexId, string cid)
        {
            string version = OrbitPageVersionConstants.v1;
            ICompanyManagement companyManagementModel = CompanyManagementFactory.GetCompanyManagementInstance(version);
            return companyManagementModel.CompanyDetailsById(userVertexId, cid);
        }
        public Dictionary<string, string> CreateNewCompanyDesignationEdge(urNoticeSession session, string designation, string salary,
            string jobFromYear, string jobToYear, string companyVertexId)
        {
            throw new NotImplementedException();
        }
        public bool CreateNewDesignation(string designationName, string createdBy)
        {
            throw new NotImplementedException();
        }
        public bool CreateNewCompanyDesignationSalary(string companyName, string designationName, string salary, string createdBy)
        {
            throw new NotImplementedException();
        }
        public bool CreateNewCompanyDesignationNoticePeriod(string companyName, string designationName, string noticePeriodRange,
            string createdBy)
        {
            throw new NotImplementedException();
        }
        public bool CreateNewCompany(OrbitPageCompany company, string createdBy)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<string> SendEmail(CreateOrbitPageEmailRequest req, HttpRequestBase request)
        {
            throw new NotImplementedException();
        }
    }
}
