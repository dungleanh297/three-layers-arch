using HotelReservation.Business;

namespace HotelReservation.Business;

public interface ICustomerService
{
    Task<List<CustomerDTO>> GetAsync(CustomerSearchCriteria? criteria = null);

    Task CreateAsync(CustomerDTO customerDTO);

    Task UpdateAsync(CustomerDTO customerDTO);

    Task DeleteAsync(int id);

}
