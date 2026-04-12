namespace HotelReservation.Business.Abstraction;

public interface IRoomService
{
    Task<RoomDTO> GetAsync();

    Task CreateAsync(RoomDTO room);

    Task UpdateAsync(RoomDTO roomDTO);

    Task DeleteAsync(int id);
}
