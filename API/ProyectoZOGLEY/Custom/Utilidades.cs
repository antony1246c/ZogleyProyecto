using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ProyectoZOGLEY.Models;

namespace ProyectoZOGLEY.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;
        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generacion del JSON Web Token por medio de la libreria System.IdentityModel.Tokens.Jwt y utilizando
        /// el modelo perteneciente a la base de datos UserInfo
        /// </summary>
        public string generarJWT(UserInfo modelo)
        {
         /* Creacion del modelo en el cual pedimos datos obligatorios del UserInfo
            para la generacion del token solicitamos el Id,email y rol (ya definido como cliente) */
            var userClaims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, modelo.IdUsuario.ToString()),
        new Claim(ClaimTypes.Email, modelo.Email!),
        new Claim(ClaimTypes.Role, modelo.Rol)
    };
            // Llave de seguridad y algoritmo de encriptacion
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Configuracion de la llave de seguridad y tiempo de expiracion.
            var jwtConfig = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

    }
}
