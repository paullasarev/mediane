using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace Mediane.DomainModel
{
    public class MedianeMembershipProvider : ExtendedMembershipProvider
    {
        private IUserRepository repository;

        public MedianeMembershipProvider(IUserRepository repository)
        {
            this.repository = repository;
        }

        public MedianeMembershipProvider()
        {
            this.repository = RepositoryTable.Repositories.Locate<IUserRepository>();
        }

#region ActualOverrides

        public override bool ValidateUser(string username, string password)
        {
            if (username == "administrator" && password == "root")
            {
                return true;
            }

            return repository.Validate(username, password);
        }

        // see http://camelot-sion.appspot.com/github.com/mazhekin/MVC4CustomMembershipSolution/blob/master/App.Web/Code/Membership/CustomMembershipProvider.cs
        public override string
        CreateUserAndAccount(string userName, string password, bool requireConfirmation, IDictionary<string, object> values)
        {
            repository.CreateLocal(userName, password);
            return "";
        }

        public override System.Web.Security.MembershipUser
        GetUser(string username, bool userIsOnline)
        {
            int providerUserKey = repository.GetUserId(username);
            return new MembershipUser(this.Name, username, providerUserKey, "", "", "", true, false,
                new DateTime(2013, 1, 1), DateTime.Now, DateTime.Now,
                new DateTime(2013, 1, 1), new DateTime(1, 1, 1));
            //var user = new MedianeMembershipUser(repository, username);
            //return user;
        }

        public override bool
        HasLocalAccount(int id)
        {
            //*** https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/WebMatrix.WebData/SimpleMembershipProvider.cs
            return repository.GetUserById(id) != null;
        }

        public override ICollection<OAuthAccountData>
        GetAccountsForUser(string userName)
        {
            //*** https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/WebMatrix.WebData/SimpleMembershipProvider.cs
            throw new NotImplementedException();
        }

        public override void
        CreateOrUpdateOAuthAccount(string provider, string providerUserId, string userName)
        {
            //*** https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/WebMatrix.WebData/SimpleMembershipProvider.cs
            throw new NotImplementedException();
        }

        public override int
        GetUserIdFromOAuth(string provider, string providerUserId)
        {
            //*** https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/WebMatrix.WebData/SimpleMembershipProvider.cs
            throw new NotImplementedException();
        }

#endregion ActualOverrides

#region NotImplementedOverrides
        public override bool ConfirmAccount(string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool ConfirmAccount(string userName, string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override string CreateAccount(string userName, string password, bool requireConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAccount(string userName)
        {
            throw new NotImplementedException();
        }

        public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreateDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastPasswordFailureDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetPasswordChangedDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            throw new NotImplementedException();
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            throw new NotImplementedException();
        }

        public override bool IsConfirmed(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }

#endregion NotImplementedOverrides

    }

    //public class MedianeMembershipUser : System.Web.Security.MembershipUser
    //{
    //    private IUserRepository repository;

    //    public MedianeMembershipUser(IUserRepository repository, string username)
    //        : base("MedianeMembershipUser", username, null, "", "", "", true, false, 
    //            new DateTime(2013,1,1), DateTime.Now, DateTime.Now,
    //            new DateTime(2013,1,1), new DateTime(1,1,1) )
    //    {
    //        this.repository = repository;
    //    }

    //}
}