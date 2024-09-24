﻿using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository; // ver si se aplica Para obtener y notificar al usuario.

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }

        // Implementación del método para crear una reserva
        public Booking CreateBooking(NewbookingDTO newBookingDTO)
        {
            // Obtener todas las reservas que se solapan con la nueva reserva en la misma sala
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
            // Crear nueva reserva
            var newBooking = new Booking(
                newBookingDTO.User,
                new Room { Id = newBookingDTO.RoomId }, // Pregutar aqui el DTO esta definido como RoomName
                newBookingDTO.Priority,
                newBookingDTO.StartDate,
                newBookingDTO.EndDate);

            return _bookingRepository.AddBooking(newBooking);
        }
        // Método para comparar prioridades
        private int ComparePriority(string newPriority, string existingPriority)
        {
            var priorityOrder = new Dictionary<string, int> { { "LOW", 1 }, { "MEDIUM", 2 }, { "HIGH", 3 } };
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