#region Using directives

using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;
using Events.Repository.Abstractions;

#endregion

namespace Logic.Services
{
    public class UserReader : IUserReader
    {
        private readonly IReadRepository<User> _userRepository;

        public UserReader(IReadRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> FindByEmail(string emailAddress)
        {
            var user = (await _userRepository.FindAllAsync(f => f.EmailAddress.ToLower()
                                                                    .Contains(emailAddress) &&
                                                                !f.IsDeleted))
                .FirstOrDefault();

            return user;
        }
    }
}