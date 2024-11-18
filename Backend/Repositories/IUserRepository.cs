using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task AddUser(User user); 
        Task UpdateUser(User user);
        Task DeleteUser(string id);
    }
}
