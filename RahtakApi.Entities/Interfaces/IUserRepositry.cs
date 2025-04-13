using Interfaces;
using RahtakApi.Entities.Models;
using System.Threading.Tasks;

namespace RahtakApi.Entities.Interfaces
{
    public interface IUserRepositry : IRepository<Users>
    {
        Users? GetUserByEmail(string email);
        Task<Users?> GetUserByEmailAsync(string email);
        Task<Users?> GetUserByResetCodeAsync(string resetCode); // ✅ إضافة الدالة الجديدة
    }
}
