using HotelReservation.Domain.Entities;
using HotelReservation.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Business;

internal class CustomerService : ICustomerService
{
    private readonly HotelReservationDbContext _context;

    public CustomerService(HotelReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDTO>> GetAsync(CustomerSearchCriteria? criteria = null)
    {
        var query = _context.Customers.AsNoTracking();

        if (criteria != null)
        {
            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, criteria.Name));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Email))
            {
                query = query.Where(c => EF.Functions.Like(c.Email, criteria.Email));
            }

            if (!string.IsNullOrWhiteSpace(criteria.PhoneNumber))
            {
                query = query.Where(c => EF.Functions.Like(c.PhoneNumber, criteria.PhoneNumber));
            }

            if (!string.IsNullOrWhiteSpace(criteria.SocietyCardNumber))
            {
                query = query.Where(c => EF.Functions.Like(c.SocietyCardNumber, criteria.SocietyCardNumber));
            }
        }

        var pagination = criteria?.Pagination ?? Pagination.DefaultPagination;

        var customers = await query.Where(e => e.Id > pagination.StartRecordId).Take(pagination.Size).Select(c => new CustomerDTO
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            SocietyCardNumber = c.SocietyCardNumber
        }).ToListAsync();

        return customers;
    }

    public async Task CreateAsync(PutCustomerRequest request)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            SocietyCardNumber = request.SocietyCardNumber
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, PutCustomerRequest request)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            throw new ResourceNotFoundException();
        }

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.PhoneNumber = request.PhoneNumber;
        customer.SocietyCardNumber = request.SocietyCardNumber;

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            throw new ResourceNotFoundException();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
