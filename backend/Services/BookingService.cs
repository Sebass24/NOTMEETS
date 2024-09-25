using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApiContext _context;                               // ver si se aplica Para obtener y notificar al usuario.

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository,ApiContext context)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _context = context;

        }

        public void CancelBooking(int id)
        {
            throw new NotImplementedException();
        }

        // Implementación del método para crear una reserva
        public Booking CreateBooking(NewBookingDTO newBookingDTO)
        {
            // Obtener todas las reservas que coinciden con la nueva reserva en la misma sala
            var conflictingBookings = _bookingRepository.GetBookingsForRoomAndTime(
                newBookingDTO.RoomId, newBookingDTO.StartDate, newBookingDTO.EndDate); //Preguntar aqui como definir 

            foreach (var existingBooking in conflictingBookings)
            {
                // Comparar la prioridad de la nueva reserva con la existente // aqui tengo problemas 
                if (ComparePriority(newBookingDTO.Priority, existingBooking.Priority) > 0)
                {
                    // Si la nueva tiene mayor prioridad, cancelar la existente y notificar al usuario
                    _bookingRepository.DeleteBooking(existingBooking.Id);
                    NotifyUser(existingBooking.User, existingBooking); // Notificar al usuario afectado
                }
                else
                {
                    // Si la nueva tiene igual o menor prioridad, lanzar excepción
                    throw new Exception("La sala ya está reservada con una mayor o igual prioridad.");
                }
            }
            //Busca la sala si todo esta ok
            var room = _context.Rooms.Where(r=>r.Id==newBookingDTO.RoomId).FirstOrDefault();
            var user = _context.Users.Where(u => u.Id == newBookingDTO.UserId).FirstOrDefault();

            // Crear nueva reserva
            var newBooking = new Booking(
                newBookingDTO.User,
                room,
                newBookingDTO.RoomId, // Pasa solo el RoomId
                newBookingDTO.Priority,
                newBookingDTO.StartDate,
                newBookingDTO.EndDate,newBookingDTO.Attendees); 
                

            return _bookingRepository.AddBooking(newBooking);
        }

        public bool DeleteBooking(int id)
        {
            throw new NotImplementedException();
        }

        public Booking GetBookingById(int id)
        {
            return _bookingRepository.GetBookingById(id);
        }

        public List<Booking> GetBookingsForRoom(int roomId)
        {
            throw new NotImplementedException();
        }

        public List<Booking> GetBookingsForRoomAndTime(int roomId, DateTime startDate, DateTime endDate)
        {
            return _bookingRepository.GetBookingsForRoomAndTime(roomId, startDate, endDate).ToList();
        }

        public Booking UpdateBooking(int id, NewBookingDTO updatedBooking)
        {
            throw new NotImplementedException();
        }

        // Método para comparar prioridades
        private int ComparePriority(string newPriority, string existingPriority)
        {
            var priorityOrder = new Dictionary<string, int> { { "LOW", 3 }, { "MEDIUM", 2 }, { "HIGH", 1 } };
            return priorityOrder[newPriority] - priorityOrder[existingPriority];
        }

        // Método para notificar al usuario
        private void NotifyUser(string username, Booking canceledBooking)
        {
            var user = _userRepository.GetUserByUsername(username); //ver error

            if (user != null)
            {
                // Aquí se puede implementar el envío de un correo electrónico o notificación al usuario //ver si se usa
                Console.WriteLine($"Notificación enviada a {user.Email}: Su reserva ha sido cancelada.");
            }

            //// Métodos adicionales de la interfaz
           // public List<Booking> GetBookingsForRoom(int roomId)
            {
                // Implementar lógica para obtener todas las reservas de una sala
               // return _bookingRepository.GetBookingsForRoomAndTime(roomId, DateTime.MinValue, DateTime.MaxValue);
            }

           // public Booking GetBookingById(int id)
            {
                // Implementar lógica para obtener una reserva por su ID
               // return _bookingRepository.GetBookingById(id);
            }

            //public void CancelBooking(int id)
            {
                // Implementar lógica para cancelar una reserva
               // _bookingRepository.DeleteBooking(id);
            }
        }
    }
}