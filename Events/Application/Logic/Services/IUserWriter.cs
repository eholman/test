#region Using directives

using System.Threading.Tasks;

#endregion

namespace Logic.Services
{
    public interface IUserWriter
    {
        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns>
        /// <langword>true</langword> is user is created. <langword>false</langword> if <paramref name="emailAddress" />
        /// is not unique
        /// </returns>
        Task<bool> CreateUser(string emailAddress);

        Task VerifyUser(string userId);
        Task ChangeEmailAddress(string userId, string newEmailAddress);
        Task DeleteUser(string userId);
    }
}