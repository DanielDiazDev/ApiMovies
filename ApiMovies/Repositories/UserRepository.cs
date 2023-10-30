using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.Models.DTOs;
using ApiMovies.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiMovies.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private string _secretKey;

        public UserRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _secretKey = config.GetValue<string>("ApiSettings:Secret");
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(u => u.Username).ToList();
        }

        public User GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);

        }

        public bool IsUniqueUser(string user)
        {
            var userDb = _context.Users.FirstOrDefault(u => u.Username == user);
            return userDb == null;
        }
        public async Task<User> Register(UserRegisterDTO userRegisterDTO)
        {
            string passwordEncrypted = GetMD5(userRegisterDTO.Password);
            User user = new User()
            {
                Username = userRegisterDTO.Username,
                Password = passwordEncrypted,
                Name = userRegisterDTO.Name,
                Role = userRegisterDTO.Role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            user.Password = passwordEncrypted;
            return user;
        }

        private string GetMD5(string password)
        {
            var x = MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(password);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;
        }

        public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO)
        {
            string passwordEncrypted = GetMD5(userLoginDTO.Password);
            var user = _context.Users.FirstOrDefault(u=>u.Username.ToLower() == userLoginDTO.Username.ToLower() && u.Password == passwordEncrypted);
            if(user == null) //Validamos si esta todo correcto
            {
                return new UserLoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }
            //Si existe podemos procesar el login
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())

                }),
                Expires = DateTime.UtcNow.AddDays(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserLoginResponseDTO userLoginResponseDTO = new UserLoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            return userLoginResponseDTO;

        }


    }
}
