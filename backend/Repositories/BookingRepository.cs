using backend.Data;
using backend.Models;

namespace backend.Repositories
{
    public class BookingRepository : IBookingRepository

    {
        private readonly ApiContext _context;

        public BookingRepository(ApiContext context)
        {
            _context = context;
        }

        // Obtener reservas con el tiempo dado para una sala específica
        public IEnumerable<Booking> GetBookingsForRoomAndTime(int? roomId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Bookings;
            if (roomId != null)
            query.Where(b => b.Room.Id == roomId);
            if (startDate != null)
                query.Where(b => b.StartDate > startDate);
            if (endDate != null)
            query.Where(b => b.EndDate < endDate);

               return query.ToList();
        }

        // Añadir una nueva reserva
        public Booking AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges(); // Guardar cambios en la base de datos
            return booking;
        }

        // Eliminar una reserva por su ID
        public async Task<bool> DeleteBooking(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync(); // Guardar cambios en la base de datos
                return true;
            }
            return false;
        }

        // Obtener una reserva por su ID
        public Booking GetBookingById(int id)
        {
            return _context.Bookings.FirstOrDefault(b => b.Id == id); ///verificar esta advertencia
        }

        public Task<Booking> CreateBooking(Booking booking)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Booking>> GetBookingsByRoomId(int roomId)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> UpdateBooking(Booking booking)
        {
            throw new NotImplementedException();
        }

        public List<Booking> GetBookingsForRoomAndTime(int roomId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
