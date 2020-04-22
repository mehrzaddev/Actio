using Actio.Common.Exeptions;
using Actio.Services.Identity.Domain.Repositories;
using Actio.Services.Identity.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;

        public UserService(IUserRepository userRepository, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }
        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new ActioExeption("email_in_use",
                    $"Email :'{email}' is already in use");
            }

            user = new Domain.Models.User(email, name);
            user.SetPassword(password, _encrypter);
            await _userRepository.AddAsync(user);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ActioExeption("invalid_credentials",
                    $"Invalid User");
            }

            if (!user.ValidatePassword(password, _encrypter))
            {
                throw new ActioExeption("invalid_credentials",
                    $"Invalid User");
            }

            //ToDO : Get Token 
        }
    }
}
