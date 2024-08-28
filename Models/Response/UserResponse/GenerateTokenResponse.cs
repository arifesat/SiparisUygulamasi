namespace SiparisUygulamasi.Models.Response.UserResponse
{
    public class GenerateTokenResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
    }
}
