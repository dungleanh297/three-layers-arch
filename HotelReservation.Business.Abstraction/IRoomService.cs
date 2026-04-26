namespace HotelReservation.Business;

public interface IRoomService
{
    Task<List<RoomDTO>> GetAsync();

    Task CreateAsync(PutRoomInformation room);

    Task UpdateAsync(int id, PutRoomInformation roomDTO);

    Task DeleteAsync(int id);
}
