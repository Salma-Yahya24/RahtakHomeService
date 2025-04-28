using Interfaces;
using RahtakApi.DAL.Data;
using RahtakApi.DAL.Repository;
using RahtakApi.Entities.Interfaces;
using RahtakApi.Entities.Models;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        
        
            private readonly AppDbContext _context;

            public UnitOfWork(AppDbContext context)
            {
                _context = context;

                Users = new UserRepository(_context); // ✅ استخدم IUserRepository
                Addresses = new Repository<Address>(_context);
                Bookings = new Repository<Booking>(_context);
                BookingDetails = new Repository<BookingDetails>(_context);
                BookingStatuses = new Repository<BookingStatus>(_context);
                PaymentMethods = new Repository<PaymentMethod>(_context);
                Payments = new Repository<Payments>(_context);
                Reviews = new Repository<Reviews>(_context);
                ServiceGroups = new Repository<ServiceGroups>(_context);
                ServiceProviders = new Repository<ServiceProviders>(_context);
                ServiceProviderTypes = new Repository<ServiceProviderType>(_context);
                SubServices = new Repository<SubService>(_context);
            }

            public IUserRepositry Users { get; } // ✅ غيرنا النوع لـ IUserRepository
            public IRepository<Address> Addresses { get; }
            public IRepository<Booking> Bookings { get; }
            public IRepository<BookingDetails> BookingDetails { get; }
            public IRepository<BookingStatus> BookingStatuses { get; }
            public IRepository<PaymentMethod> PaymentMethods { get; }
            public IRepository<Payments> Payments { get; }
            public IRepository<Reviews> Reviews { get; }
            public IRepository<ServiceGroups> ServiceGroups { get; }
            public IRepository<ServiceProviders> ServiceProviders { get; }
            public IRepository<ServiceProviderType> ServiceProviderTypes { get; }
            public IRepository<SubService> SubServices { get; }

            public int Save()
            {
                return _context.SaveChanges();
            }

            public async Task<int> SaveAsync()
            {
                return await _context.SaveChangesAsync();
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }

    }
