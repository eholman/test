#region Using directives

using System.Threading.Tasks;
using DataAccess.Entities;

#endregion

namespace Logic.Services
{
    public interface IUserReader
    {
        Task<User> FindByEmail(string emailAddress);
    }
}