using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface IBookingService
    {
        Booking CreateBooking(NewBookingDTO newBookingDTO);
        List<Booking> GetBookingsForRoom(int roomId);
        Booking GetBookingById(int id);
        void CancelBooking(int id);
        Booking UpdateBooking(int id, NewBookingDTO updatedBooking);
        bool DeleteBooking(int id);
        List<Booking> GetBookingsForRoomAndTime(int roomId, DateTime startDate, DateTime endDate);
    }
}
