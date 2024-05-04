namespace Application.Infrastructure.Models.Configurations;

public class JwtConfiguration
{
    public string SecretKey { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public int TokenValidityInMinutes { get; set; }
}
