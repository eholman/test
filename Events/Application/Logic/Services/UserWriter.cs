#region Using directives

using System;
using System.Threading.Tasks;
using Domain;
using Events.Repository.Abstractions;

#endregion

namespace Logic.Services
{
    public class UserWriter : IUserWriter
    {
        private readonly IUserReader _userReader;
        private readonly IEventStoreRepository<User, UserId> _userRepository;

        public UserWriter(IEventStoreRepository<User, UserId> userRepository, IUserReader userReader)
        {
            _userRepository = userRepository;
            _userReader = userReader;
        }


        /// <inheritdoc />
        public async Task<bool> CreateUser(string emailAddress)
        {
            var existingUser = await _userReader.FindByEmail(emailAddress);
            if (existingUser != null)
            {
                // Notifications pattern could be used here
                return false;
            }

            var userEvent = new User(UserId.NewId(), emailAddress);

            await _userRepository.SaveAsync(userEvent);

            return true;
        }

        public async Task VerifyUser(string userId)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(userId));
            user.SetVerificationState(true);
            await _userRepository.SaveAsync(user);
        }

        /// <inheritdoc />
        public async Task ChangeEmailAddress(string userId, string newEmailAddress)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(userId));
            user.SetEmailAddress(newEmailAddress);
            user.SetVerificationState(false);
            await _userRepository.SaveAsync(user);
        }

        /// <inheritdoc />
        public async Task DeleteUser(string userId)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(userId));
            user.MarkForDeletion(DateTime.Now.AddDays(30));
            await _userRepository.SaveAsync(user);
        }
    }
}