using backend.DTOs;
using backend.Models;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        //Firmas de los metodos de consulta del UserRepository
        object? UserAuth(UserDTO userDto); //Auth usuario
        User AddUser(UserDTO userDto); //Registro usuario
        //TODO -> fixear
        //User? UpdateUser(GetUserDTO userDto); //Modificaicon usuario
        bool DeleteUser(int id); //Borrado usuario
        IEnumerable<UserDTO> GetAllUsers(); //Obtener todos los usuarios
        UserDTO? GetUserById(int id); //Obtener un usuario 
        UserDTO? GetUserRole(int id); //Obtener rol del usuario
        User? GetUserByUsername(string username);



        // IEnumerable<GetUserBookingsDTO> GetUserBookings() // Obtener lista de reservas del usuario
        // IEnumerable<GetUserNotificationsDTO> GeUserNotifications(); //Obtener lista de notificaciones del usuario
    }
}
