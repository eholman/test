#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using Console.Commands;
using Logic.Services;
using MediatR;

#endregion

namespace Console.Handlers
{
    public class UserCommandsHandler : IRequestHandler<CreateUserCommand, bool>,
        IRequestHandler<VerifyUserCommand, bool>, IRequestHandler<SearchUserCommand, bool>,
        IRequestHandler<ChangeUserEmailAddressCommand, bool>, IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserReader _userReader;
        private readonly IUserWriter _writeService;

        public UserCommandsHandler(
            IUserWriter writeService,
            IUserReader userReader)
        {
            _writeService = writeService;
            _userReader = userReader;
        }

        /// <inheritdoc />
        public Task<bool> Handle(ChangeUserEmailAddressCommand request, CancellationToken cancellationToken)
        {
            var input = GetConsoleInput("\bPlease enter User ID:");
            var newEmailAddress = GetConsoleInput("Please new Email Address:");

            _writeService.ChangeEmailAddress(input, newEmailAddress);

            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var input = GetConsoleInput("\bPlease enter email address: ");

            System.Console.WriteLine("Sending command. Awaiting notification...");
            if (!await _writeService.CreateUser(input))
            {
                System.Console.WriteLine("User was not created; Email address not unique?");
            }

            return true;
        }

        /// <inheritdoc />
        public Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var input = GetConsoleInput("\bPlease enter User ID:");

            _writeService.DeleteUser(input);

            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public async Task<bool> Handle(SearchUserCommand request, CancellationToken cancellationToken)
        {
            var input = GetConsoleInput("\bSearch for email address:");

            System.Console.Write("Searching... ");
            var foundUser = await _userReader.FindByEmail(input);

            System.Console.WriteLine(
                foundUser != null
                    ? $"{Environment.NewLine}Found: {foundUser.EmailAddress}. Verified: {foundUser.IsVerified}. ID: {foundUser.Id}"
                    : "Found nothing...");

            return true;
        }

        /// <inheritdoc />
        public Task<bool> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
        {
            var input = GetConsoleInput("\bPlease enter User ID:");

            System.Console.WriteLine("Sending command. Awaiting notification...");
            _writeService.VerifyUser(input);

            return Task.FromResult(true);
        }

        private static string GetConsoleInput(string inputMessage)
        {
            System.Console.Write(inputMessage);
            var input = System.Console.ReadLine();
            System.Console.WriteLine();

            return input;
        }
    }
}