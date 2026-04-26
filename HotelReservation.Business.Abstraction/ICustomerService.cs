namespace HotelReservation.Business;

public interface ICustomerService
{
    Task<List<CustomerDTO>> GetAsync(CustomerSearchCriteria? criteria = null);

    Task CreateAsync(PutCustomerRequest customerDTO);

    Task UpdateAsync(int id, PutCustomerRequest customerDTO);

    Task DeleteAsync(int id);

}
