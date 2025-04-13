using Microsoft.EntityFrameworkCore;
using RahtakApi.DAL.Data;
using RahtakApi.Entities.Interfaces;
using RahtakApi.Entities.Models;
using Repository;
using System.Threading.Tasks;

namespace RahtakApi.DAL.Repository
{
    public class UserRepository : Repository<Users>, IUserRepositry
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Users? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Users?> GetUserByResetCodeAsync(string resetCode) // ✅ تنفيذ الدالة الجديدة
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ResetCode == resetCode);
        }
    }
}
