using HotelReservation.Domain;

namespace HotelReservation.Business;

public interface IReservationService
{
    Task<List<ReservationDTO>> GetAsync(ReservationSearchCriteria? searchCriteria = null);

    Task PlaceReservation(PlaceReservationRequest request);

    Task UpdateReservationStatus(int reservationId, ReservationStatus status);
}
