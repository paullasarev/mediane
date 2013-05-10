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

namespace Mediane.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        const string DbName = "TestDb.sdf";
        const string ConnectionStringName = "MedianeDb";
        static string CurrentDirectory = Directory.GetCurrentDirectory();

        static string DbFullFileName;
        static MedianeSql Sql;
        static InitDb InitDbInstance;
        static IUserRepository userRepository;

        public static void EnableConnectionStringConfigurationEditing()
        {
            typeof(ConfigurationElementCollection)
                .GetField("bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(ConfigurationManager.ConnectionStrings, false);
        }

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Sql = new MedianeSql();
            DbFullFileName = Path.Combine(CurrentDirectory, DbName);
            File.Delete(DbFullFileName);

            InitDbInstance = new InitDb(DbName, Sql, DbFullFileName);
            InitDbInstance.CreateDbIfNotExist();

            EnableConnectionStringConfigurationEditing();

            ConfigurationManager.ConnectionStrings.Clear();
            ConfigurationManager.ConnectionStrings.Add(
                new ConnectionStringSettings(
                    ConnectionStringName,
                    InitDbInstance.GetConnectionString(),
                    InitDbInstance.GetProviderName()
                )
            );

            RepositoryTable.Repositories.Clear();
            userRepository = new UserRepository(
                    InitDbInstance.GetConnectionString(),
                    InitDbInstance.GetProviderName()
                );

            RepositoryTable.Repositories.Register<IUserRepository>(userRepository);
        }

        [ClassCleanup]
        public static void Done()
        {
            RepositoryTable.Repositories.Clear();
        }

        [TestMethod]
        public void ExternalLoginConfirmationShouldCreateOAuthUser()
        {
            string username = "user1";
            string url = "http://localhost:56773/Account/Manage";
            string providerName = "Facebook";
            string providerUserId = "user1@gmail.com";

            var stringBuilder = new StringBuilder();
            var memoryWriter = new StringWriter(stringBuilder);
            var fakeResponse = new HttpResponse(memoryWriter);

            HttpRequest fakeRequest = new HttpRequest("Global.aspx", url, "");

            //var identity = new GenericIdentity(username);
            var identity = new NoAuthIdentity(username);
            string[] roles = { /*"Administrator"*/ };
            var principal = new GenericPrincipal(identity, roles);

            HttpContext httpContext = new HttpContext(fakeRequest, fakeResponse) {
                User = principal
            };

            var controllerContext = new ControllerContext() {
                HttpContext = new HttpContextWrapper(httpContext),
            };
            
            var controller = new AccountController() {
                ControllerContext = controllerContext,
            };

            string externalLoginData = OAuthWebSecurity.SerializeProviderUserId(providerName, providerUserId);

            var model = new RegisterExternalLoginModel() {
                UserName = username,
                ExternalLoginData = externalLoginData
            };


            Assert.IsFalse(userRepository.UserExist(username));

            controller.ExternalLoginConfirmation(model, url);

            Assert.IsTrue(userRepository.UserExist(username));

        }

        class NoAuthIdentity : IIdentity
        {
            string username;

            public NoAuthIdentity(string username)
            {
                this.username = username;
            }

            public string AuthenticationType
            {
                get { return "Basic"; }
            }

            public bool IsAuthenticated
            {
                get { return false; }
            }

            public string Name
            {
                get { return username; }
            }
        }
    }
}
