using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Jwt_ConsoleApp
{
    class Program
    {
        const string _customClaimIdentifier = "custom-claim";

        static void Main(string[] args)
        {
            Console.WriteLine("Creating JWT token...");

            var jwt = GenerateToken(userId: 123456, customClaim: "custom-claim-value_684a093f7fde4116a2f5c8143b939963");
            Console.WriteLine(jwt);

            Console.WriteLine("Validating JWT token...");
            var (success, principal) = ValidateToken(jwt);
            Console.WriteLine(success);

            if (principal != null)
            {
                var claimvalue1 = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                var claimvalue2 = principal.FindFirst(_customClaimIdentifier).Value;
                Console.WriteLine($"Claim value 1: {claimvalue1}");
                Console.WriteLine($"Claim value 2: {claimvalue2}");
            }
        }

        public static string GenerateToken(int userId, string customClaim)
        {
            var certificate = new X509Certificate2(File.ReadAllBytes("private-certificate.pfx"));
            X509SecurityKey privateKey = new X509SecurityKey(certificate);
            var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256Signature);

            var myIssuer = "https://kelvin-ferreira-dev.com";
            var myAudience = "https://kelvin-ferreira-dev.com";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(_customClaimIdentifier, customClaim)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static (bool, ClaimsPrincipal) ValidateToken(string token)
        {
            var certificate = new X509Certificate2(File.ReadAllBytes("public-certificate.cer"));
            X509SecurityKey publickey = new X509SecurityKey(certificate);

            var myIssuer = "https://kelvin-ferreira-dev.com";
            var myAudience = "https://kelvin-ferreira-dev.com";

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = publickey
                }, out SecurityToken validatedToken);

                return (principal.Identity.IsAuthenticated, principal);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
