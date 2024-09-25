using backend.Data;
using backend.DTOs;
using backend.Models;
using System.Data;

namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiContext _context;

        // Constructor para inyectar el contexto de base de datos
        public UserRepository(ApiContext context)
        {
            _context = context;

        }



        public object? UserAuth(UserDTO userDto)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Username == userDto.Username && u.Password == userDto.Password);
            if (user == null)
            {
                return null;
            }
            return new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                location = user.Location,
                role = user.Role
            };
        }



        public User AddUser(UserDTO userDto)
        {
            var user = new User
             (
                username: userDto.Username,
                password: userDto.Password,
                email: userDto.Email,
                location: userDto.Location,
                role: userDto?.Role ?? "user"
            );
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }


        //TODO -> fixear
        //public User? UpdateUser(GetUserDTO userDto)
        //{
        //    var user = _context.Users.Find(userDto.Id);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    user.Username = userDto.Username;
        //    user.Email = userDto.Email;
        //    user.Location = userDto.Location;
        //    user.Role = userDto.Role;
        //    _context.SaveChanges();
        //    return user;

        //}



        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return true;
        }



        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _context.Users.Select(u => new UserDTO(u)).ToList();
        }



        public UserDTO? GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                return new UserDTO(user);
            }
            return null;
        }



        public UserDTO? GetUserRole(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                return new UserDTO(user.Id, user.Username, user.Role);

            }
            return null;
        }

        public User? GetUserByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            return user;
        }
    }

}