using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PhoneVaultContext _context;
        public UserRepository(PhoneVaultContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            if (user == null) 
            { 
                throw new ArgumentNullException(nameof(user));
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers() =>
            await _context.Users.ToListAsync();
        public async Task<User> GetUserById(string id) =>
            await _context.Users.FindAsync(id);

        public async Task UpdateUser(User user)
        {
            if (user == null) 
            { 
                throw new ArgumentNullException(nameof(user));
            }
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
