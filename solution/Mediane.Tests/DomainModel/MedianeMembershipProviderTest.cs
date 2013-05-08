using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMatrix.WebData;
using Mediane.DomainModel;
using System.Web.Security;

namespace Mediane.Tests.DomainModel
{
    [TestClass]
    public class MedianeMembershipProviderTest
    {
        IUserRepository repository = new FakeUserRepository();
        MedianeMembershipProvider provider;

        public MedianeMembershipProviderTest()
        {
            provider = new MedianeMembershipProvider(repository);
        }

        [TestMethod]
        public void ShouldInstantiate()
        {
            Assert.IsInstanceOfType(provider, typeof(ExtendedMembershipProvider));
        }
        
        [TestMethod]
        public void ValidateUserShouldValidateAdminCredentials()
        {
            Assert.IsTrue(provider.ValidateUser("administrator", "root"));
        }

        [TestMethod]
        public void CreateUserAndAccountShouldStoreCreds()
        {
            Assert.IsFalse(provider.ValidateUser("user", "asdf"));

            string confirmToken = provider.CreateUserAndAccount("user", "asdf", false, null);

            Assert.IsTrue(provider.ValidateUser("user", "asdf"));
        }

        [TestMethod]
        public void GetUserShouldBuildMembership()
        {
            string username = "administrator";
            
            MembershipUser user = provider.GetUser(username, true);
            
            Assert.AreEqual(username, user.UserName);
        }
    }

    [TestClass]
    public class MedianeRoleProviderTest
    {
        [TestMethod]
        public void ShouldInstanciate()
        {
            var provider = new MedianeRoleProvider();
            Assert.IsInstanceOfType(provider, typeof(RoleProvider));
        }

        [TestMethod]
        public void AdministratorShouldHaveAdminRole()
        {
            var provider = new MedianeRoleProvider();
            var roles = provider.GetRolesForUser("administrator");
            Assert.IsTrue(Array.IndexOf(roles, "admin") >= 0);
        }

        [TestMethod]
        public void EmptyUserShouldNotHaveRoles()
        {
            var provider = new MedianeRoleProvider();
            var roles = provider.GetRolesForUser("");
            Assert.AreEqual(0, roles.Length);
        }
    }
}
