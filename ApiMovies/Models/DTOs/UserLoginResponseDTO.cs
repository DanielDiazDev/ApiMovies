namespace ApiMovies.Models.DTOs
{
    public class UserLoginResponseDTO
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
