using DemoEshop.BusinessLayer.Services.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using AutoMapper;
using DemoEshop.Infrastructure;
using System.Security.Cryptography;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.Services.Users
{
    public class UserService : ServiceBase, IUserService
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        private readonly IRepository<User> userRepository;
        private readonly QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject;

        public UserService(IMapper mapper, IRepository<User> userRepository, QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject)
            : base(mapper)
        {
            this.userRepository = userRepository;
            this.userQueryObject = userQueryObject;
        }

        public async Task<UserDto> GetUserAccordingToUsernameAsync(string username)
        {
            var queryResult = await userQueryObject.ExecuteQuery(new UserFilterDto() { Username = username });
            return queryResult.Items.SingleOrDefault();
        }

        public async Task<Guid> RegisterUserAsync(UserCreateDto userDto)
        {
            var user = Mapper.Map<User>(userDto);

            if (await GetIfUserExistsAsync(user.Username))
            {
                throw new ArgumentException("username is already used");
            }

            var password = CreateHash(userDto.Password);
            user.PasswordHash = password.Item1;
            user.PasswordSalt = password.Item2;

            user.Role = DataAccessLayer.EntityFramework.Enums.Role.Basic;
            user.Email = userDto.Email;

            userRepository.Create(user);

            return user.Id;
        }

        public async Task<(bool success, Role role)> AuthorizeUserAsync(string username, string password)
        {
            var userResult = await userQueryObject.ExecuteQuery(new UserFilterDto { Username = username });
            var user = userResult.Items.SingleOrDefault();

            var succ = user != null && VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password);
            var role = user?.Role ?? Role.Basic;
            return (succ, role);
        }

        private async Task<bool> GetIfUserExistsAsync(string username)
        {
            var queryResult = await userQueryObject.ExecuteQuery(new UserFilterDto { Username = username });
            return (queryResult.Items.Count() == 1);
        }

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            var saltBytes = Convert.FromBase64String(salt);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                var generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
                return hashedPasswordBytes.SequenceEqual(generatedSubkey);
            }
        }

        private Tuple<string, string> CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return Tuple.Create(Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }
    }
}
