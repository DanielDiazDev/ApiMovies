using ApiMovies.Models;
using ApiMovies.Models.DTOs;

namespace ApiMovies.Repositories.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        bool IsUniqueUser(string user);
        Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO);
        Task<User> Register(UserRegisterDTO userRegisterDTO);
    }
}
