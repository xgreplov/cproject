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

namespace DemoEshop.BusinessLayer.Services.Users
{
    public class UserService : ServiceBase, IUserService
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;
        
        private readonly IRepository<Customer> customerRepository;
        private readonly QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject;

        public UserService(IMapper mapper, IRepository<Customer> customerRepository, QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject) 
            : base(mapper)
        {
            this.customerRepository = customerRepository;
            this.userQueryObject = userQueryObject;
        }

        public async Task<UserDto> GetUserAccordingToUsernameAsync(string username)
        {
            var queryResult = await userQueryObject.ExecuteQuery(new UserFilterDto() { Username = username });
            return queryResult.Items.SingleOrDefault();
        }

        public async Task<Guid> RegisterUserAsync(UserCreateDto userDto)
        {
            var customer = Mapper.Map<Customer>(userDto);
            
            if (await GetIfUserExistsAsync(customer.Username))
            {
                throw new ArgumentException();
            }

            var password = CreateHash(userDto.Password);
            customer.PasswordHash = password.Item1;
            customer.PasswordSalt = password.Item2;

            customerRepository.Create(customer);

            return customer.Id;
        }

        public async Task<(bool success, string roles)> AuthorizeUserAsync(string username, string password)
        {
            var userResult = await userQueryObject.ExecuteQuery(new UserFilterDto { Username = username });
            var user = userResult.Items.SingleOrDefault();

            var succ = user != null && VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password);
            var roles = user?.Roles ?? "";
            return (succ, roles);
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
