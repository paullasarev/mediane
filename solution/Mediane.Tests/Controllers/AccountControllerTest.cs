using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Mediane.DomainModel;
using System.IO;
using Mediane.Controllers;
using Mediane.Models;
using System.Reflection;
using System.Web.Mvc;
using System.Web;
using System.Text;
using System.Security.Principal;
using Microsoft.Web.WebPages.OAuth;
using Moq;
using System.Web.Routing;
using System.Collections.Generic;
using DotNetOpenAuth.AspNet;

namespace Mediane.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountController Controller { get; set; }
        private RouteCollection Routes { get; set; }
        private Mock<IWebSecurity> WebSecurity { get; set; }
        private Mock<IOAuthWebSecurity> OAuthWebSecurity { get; set; }
        private Mock<HttpResponseBase> Response { get; set; }
        private Mock<HttpRequestBase> Request { get; set; }
        private Mock<HttpContextBase> Context { get; set; }
        private Mock<ControllerContext> ControllerContext { get; set; }
        private Mock<IPrincipal> User { get; set; }
        private Mock<IIdentity> Identity { get; set; }
        private Mock<IUserRepository> TheUserRepository { get; set; }

        string returnUrl = "/Home/Index";

        string providerName = "facebook";
        string providerUserId = "Id";
        string providerDisplayName = "Facebook";

        string userName = "user";
        int userId = 100;

        public AccountControllerTest()
        {
            WebSecurity = new Mock<IWebSecurity>(MockBehavior.Strict);
            OAuthWebSecurity = new Mock<IOAuthWebSecurity>(MockBehavior.Strict);

            Identity = new Mock<IIdentity>(MockBehavior.Strict);
            Identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            User = new Mock<IPrincipal>(MockBehavior.Strict);
            User.SetupGet(u => u.Identity).Returns(Identity.Object);

            Routes = new RouteCollection();
            RouteConfig.RegisterRoutes(Routes);

            Request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            Request.SetupGet(x => x.ApplicationPath).Returns("/");
            Request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
            Request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            Response = new Mock<HttpResponseBase>(MockBehavior.Strict);

            Context = new Mock<HttpContextBase>(MockBehavior.Strict);
            Context.SetupGet(x => x.Request).Returns(Request.Object);
            Context.SetupGet(x => x.Response).Returns(Response.Object);
            Context.SetupGet(x => x.User).Returns(User.Object);

            TheUserRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            Controller = new AccountController(WebSecurity.Object, OAuthWebSecurity.Object, TheUserRepository.Object);
            var routeData = new RouteData();
            Controller.ControllerContext = new ControllerContext(Context.Object, routeData, Controller);
            Controller.Url = new UrlHelper(new RequestContext(Context.Object, routeData), Routes);

        }

        [TestMethod]
        public void Login_UserCanLogin()
        {
            string password = "password";
            var model = new LoginModel
            {
                UserName = userName,
                Password = password
            };

            WebSecurity.Setup(s => s.Login(userName, password, false)).Returns(true);

            var result = Controller.Login(model, returnUrl) as RedirectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(returnUrl, result.Url);
        }

        [TestMethod]
        public void Disassociate_UserCanRemoveOAuthProvider()
        {
            var accounts = new List<OAuthAccount> {
                new OAuthAccount(providerName, providerUserId)
            };

            OAuthWebSecurity.Setup(o => o.GetUserName(providerName, providerUserId)).Returns(userName);
            Identity.SetupGet(i => i.Name).Returns(userName);
            WebSecurity.Setup(s => s.GetUserId(userName)).Returns(userId);
            OAuthWebSecurity.Setup(o => o.HasLocalAccount(userId)).Returns(true);
            OAuthWebSecurity.Setup(o => o.GetAccountsFromUserName(userName)).Returns(accounts);
            OAuthWebSecurity.Setup(o => o.DeleteAccount(providerName, providerUserId)).Returns(true);

            var result = Controller.Disassociate(providerName, providerUserId) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.RouteValues["Message"]);

            OAuthWebSecurity.Verify(o => o.DeleteAccount(providerName, providerUserId), Times.Exactly(1));
        }

        [TestMethod]
        public void ExternalLoginConfirmation_NotAuthentecated_Should_StoreOAuthAndRedirectToReturnUrl()
        {
            var model = new RegisterExternalLoginModel
            {
                UserName = userName,
                ExternalLoginData = "data"
            };

            Identity.SetupGet(i => i.IsAuthenticated).Returns(false);
            OAuthWebSecurity.Setup(o => o.TryDeserializeProviderUserId(model.ExternalLoginData, out  providerName, out providerUserId)).Returns(true);

            TheUserRepository.Setup(o => o.UserExist(userName)).Returns(false);
            TheUserRepository.Setup(o => o.CreateUser(userName)).Returns(userId);

            OAuthWebSecurity.Setup(o => o.CreateOrUpdateAccount(providerName, providerUserId, userName));
            OAuthWebSecurity.Setup(o => o.Login(providerName, providerUserId, false)).Returns(true);


            var result = Controller.ExternalLoginConfirmation(model, returnUrl) as RedirectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(returnUrl, result.Url);

        }

        [TestMethod]
        public void ExternalLoginConfirmation_Authentecated_Should_RedirectToManageAction()
        {
            var model = new RegisterExternalLoginModel
            {
                UserName = userName,
                ExternalLoginData = "data"
            };

            Identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            var result = Controller.ExternalLoginConfirmation(model, returnUrl) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Manage", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ExternalLoginConfirmation_InvalidModelState_Should_AddModelError()
        {
            var model = new RegisterExternalLoginModel
            {
                UserName = userName,
                ExternalLoginData = "data"
            };

            Identity.SetupGet(i => i.IsAuthenticated).Returns(false);
            OAuthWebSecurity.Setup(o => o.TryDeserializeProviderUserId(model.ExternalLoginData, out  providerName, out providerUserId)).Returns(true);

            Controller.ModelState.AddModelError("TestError", "test error");

            OAuthWebSecurity.Setup(o => o.GetOAuthClientData(providerName)).Returns(AuthData(providerDisplayName));

            var result = Controller.ExternalLoginConfirmation(model, returnUrl) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(returnUrl, result.ViewBag.ReturnUrl);
            Assert.AreEqual(providerDisplayName, result.ViewBag.ProviderDisplayName);

            Assert.IsTrue(ModelStateHaveError(Controller, "TestError"));
            Assert.IsFalse(ModelStateHaveError(Controller, "UserName"));
        }

        [TestMethod]
        public void ExternalLoginConfirmation_NotAuthenticatedNoUser_Should_AddError()
        {
            var model = new RegisterExternalLoginModel
            {
                UserName = userName,
                ExternalLoginData = "data"
            };

            Identity.SetupGet(i => i.IsAuthenticated).Returns(false);
            OAuthWebSecurity.Setup(o => o.TryDeserializeProviderUserId(model.ExternalLoginData, out  providerName, out providerUserId)).Returns(true);

            TheUserRepository.Setup(o => o.UserExist(userName)).Returns(true);

            OAuthWebSecurity.Setup(o => o.GetOAuthClientData(providerName)).Returns(AuthData(providerDisplayName));

            var result = Controller.ExternalLoginConfirmation(model, returnUrl) as ViewResult;

            Assert.IsNotNull(result);

            Assert.IsFalse(Controller.ModelState.IsValid);
            Assert.IsTrue(ModelStateHaveError(Controller, "UserName"));
        }


        private static bool ModelStateHaveError(Controller controller, string key)
        {
            return controller.ModelState.ContainsKey(key) 
                && controller.ModelState[key].Errors.Count > 0;
        }

        private static AuthenticationClientData AuthData(string providerDisplayName)
        {
            return new AuthenticationClientData(
                            new Mock<IAuthenticationClient>(MockBehavior.Default).Object,
                            providerDisplayName, new Dictionary<string, object>());
        }

    }
}
