using RahtakApi.DAL.Data;  // هنا عرفنا الـ DbContext بالطريقة الصحيحة
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id)!;  // استخدمت ! عشان تتأكدي إن النتيجة مش Null
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}
