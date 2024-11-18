using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Enums;
using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PhoneVaultContext _context;

        public UserService(IUserRepository userRepository, PhoneVaultContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers() =>
            await _userRepository.GetAllUsers();

        public async Task<User> GetUserById(string id) =>
            await _userRepository.GetUserById(id);

        public async Task AddUser(UserDTO userDTO)
        {
            if (userDTO == null) throw new ArgumentNullException(nameof(userDTO));
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                PhoneNumber = userDTO.PhoneNumber,
                Address = userDTO.Address,
                UserType = userDTO.UserType,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                Orders = [],
                ShoppingCart = new(),
                Reviews = [],
            };
            await _userRepository.UpdateUser(user);
        }

        public async Task UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _userRepository.UpdateUser(user);
        }

        public async Task UpdateUser(User user, bool isAdmin)
        {
            user.UserType = isAdmin ? UserTypes.Admin : UserTypes.Customer;
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(string id)
        {
            await _userRepository.DeleteUser(id);
        }

        public async Task<EmailSettings> GetEmailSettings(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue("id")
                         ?? claimsPrincipal.FindFirstValue("user_id");

            if (userId is null)
            {
                throw new Exception("User not found");
            }

            var emailSettings = await _context.EmailSettings
                .Where(es => es.UserId == userId)
                .FirstOrDefaultAsync();

            if (emailSettings is not null) return emailSettings;

            emailSettings = new EmailSettings
            {
                UserId = userId
            };
            await _context.EmailSettings.AddAsync(emailSettings);
            await _context.SaveChangesAsync();

            return emailSettings;
        }

        public async Task SetEmailSettings(ClaimsPrincipal claimsPrincipal, IEnumerable<string> emailTypes)
        {
            var userId = claimsPrincipal.FindFirstValue("id")
                         ?? claimsPrincipal.FindFirstValue("user_id");

            if (userId is null)
            {
                throw new Exception("User not found");
            }

            var emailSettings = await _context.EmailSettings
                .Where(es => es.UserId == userId)
                .FirstOrDefaultAsync();

            var enumEmailTypesStrings = Enum.GetNames(typeof(EmailSettings.EmailType));
            var emailType = emailTypes
                .Where(et => enumEmailTypesStrings.Contains(et))
                .Select(Enum.Parse<EmailSettings.EmailType>)
                .Aggregate((EmailSettings.EmailType) 0, (current, et) => current | et);

            if (emailSettings is not null)
            {
                emailSettings.EmailTypes = emailType;
                await _context.SaveChangesAsync();
                return;
            }

            emailSettings = new EmailSettings
            {
                UserId = userId,
                EmailTypes = emailType
            };
            await _context.EmailSettings.AddAsync(emailSettings);
            await _context.SaveChangesAsync();
        }
    }
}