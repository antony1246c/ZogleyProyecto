
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Models.DTOS;
using ProyectoZOGLEY.Services;

namespace ProyectoZOGLEY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UserInfoController : ControllerBase
    {
        private readonly UserInfoService _userInfoService;
        private readonly ApplicationDbContext _context;

        public UserInfoController(
            UserInfoService userInfoS,
            ApplicationDbContext context)
        {
            _userInfoService = userInfoS;
            _context = context;
        }

        [HttpPost("RegistrarUsuario")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuario(
            [FromBody] UserInfoDto userInfoDto)
        {
            var resultado = await _userInfoService
                .RegistrarUsuarioAsync(userInfoDto);

            if (!resultado.Exito)
                return BadRequest(resultado.Resultado);

            return Ok(resultado.Resultado);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto loginDto)
        {
            var resultado = await _userInfoService
                .LoginUsuariosAsync(loginDto);

            if (!resultado.Exito)
                return BadRequest(resultado.Resultado);

            return Ok(resultado.Resultado);
        }

        [HttpGet("ValidarSesionCliente")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ValidarSesionCliente()
        {
            var claimId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(claimId, out var idUsuario))
                return Unauthorized();

            var activo = await _context.DatosUsuarios.AnyAsync(u =>
                u.IdUsuario == idUsuario &&
                u.Activo &&
                u.Rol == "Cliente");

            if (!activo)
            {
                return Unauthorized(new
                {
                    mensaje = "La cuenta está desactivada."
                });
            }

            return Ok(new
            {
                autenticado = true,
                rol = "Cliente"
            });
        }

        // ==========================================
        // CONSULTAR PERFIL DEL USUARIO AUTENTICADO
        // ==========================================

        [HttpGet("Perfil")]
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var claimId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(claimId, out var idUsuario))
                return Unauthorized();

            var usuario = await _context.DatosUsuarios
                .AsNoTracking()
                .Where(u =>
                    u.IdUsuario == idUsuario &&
                    u.Activo)
                .Select(u => new
                {
                    idUsuario = u.IdUsuario,
                    nombreApellido = u.NombreApellido,
                    email = u.Email,
                    telefono = u.Telefono,
                    direccion = u.Direccion,
                    distrito = u.Distrito,
                    provincia = u.Provincia,
                    rol = u.Rol
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new
                {
                    mensaje = "Usuario no encontrado."
                });
            }

            return Ok(new { usuario });
        }

        // ==========================================
        // ACTUALIZAR PERFIL DEL USUARIO AUTENTICADO
        // ==========================================

        [HttpPut("ActualizarPerfil")]
        [Authorize]
        public async Task<IActionResult> ActualizarPerfil(
            [FromBody] ActualizarPerfilDto datos)
        {
            var claimId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(claimId, out var idUsuario))
                return Unauthorized();

            // Buscar usuario autenticado
            var usuario = await _context.DatosUsuarios
                .FirstOrDefaultAsync(u =>
                    u.IdUsuario == idUsuario &&
                    u.Activo);

            if (usuario == null)
            {
                return NotFound(new
                {
                    mensaje = "Usuario no encontrado."
                });
            }

            // Comprobar si el correo ya pertenece a otro usuario
            var emailExiste = await _context.DatosUsuarios
                .AnyAsync(u =>
                    u.Email == datos.Email.Trim() &&
                    u.IdUsuario != idUsuario);

            if (emailExiste)
            {
                return Conflict(new
                {
                    mensaje = "Este correo electrónico ya está registrado."
                });
            }

            // Actualizar datos personales
            usuario.NombreApellido = datos.NombreApellido.Trim();
            usuario.Email = datos.Email.Trim();
            usuario.Telefono = datos.Telefono?.Trim();

            // Actualizar dirección por separado
            usuario.Direccion = datos.Direccion?.Trim();
            usuario.Distrito = datos.Distrito?.Trim();
            usuario.Provincia = datos.Provincia?.Trim();

            // Guardar cambios en SQL Server
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Perfil actualizado correctamente.",

                usuario = new
                {
                    idUsuario = usuario.IdUsuario,
                    nombreApellido = usuario.NombreApellido,
                    email = usuario.Email,
                    telefono = usuario.Telefono,
                    direccion = usuario.Direccion,
                    distrito = usuario.Distrito,
                    provincia = usuario.Provincia,
                    rol = usuario.Rol
                }
            });
        }
    }
}
