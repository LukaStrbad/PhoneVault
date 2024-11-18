using PhoneVault.Enums;
using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}

