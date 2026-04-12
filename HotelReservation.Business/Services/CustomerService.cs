using HotelReservation.Domain.Entities;
using HotelReservation.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Business.Services;

internal class CustomerService : ICustomerService
{
    private readonly HotelReservationDbContext _context;

    public CustomerService(HotelReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDTO>> GetAsync(CustomerSearchCriteria? criteria = null)
    {
        var query = _context.Customers.AsQueryable();

        if (criteria != null)
        {
            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                query = query.Where(c => c.Name.Contains(criteria.Name));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Email))
            {
                query = query.Where(c => c.Email.Contains(criteria.Email));
            }

            if (!string.IsNullOrWhiteSpace(criteria.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumber.Contains(criteria.PhoneNumber));
            }

            if (!string.IsNullOrWhiteSpace(criteria.SocietyCardNumber))
            {
                query = query.Where(c => c.SocietyCardNumber.Contains(criteria.SocietyCardNumber));
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

    public async Task CreateAsync(CustomerDTO customerDTO)
    {
        var customer = new Customer
        {
            Name = customerDTO.Name,
            Email = customerDTO.Email,
            PhoneNumber = customerDTO.PhoneNumber,
            SocietyCardNumber = customerDTO.SocietyCardNumber
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CustomerDTO customerDTO)
    {
        var customer = await _context.Customers.FindAsync(customerDTO.Id);

        if (customer == null)
        {
            throw new ResourceNotFoundException();
        }

        // Manual mapping from CustomerDTO to Customer entity
        customer.Name = customerDTO.Name;
        customer.Email = customerDTO.Email;
        customer.PhoneNumber = customerDTO.PhoneNumber;
        customer.SocietyCardNumber = customerDTO.SocietyCardNumber;

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
