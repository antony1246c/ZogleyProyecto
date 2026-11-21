using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoZOGLEY.Models.DTOS;
using ProyectoZOGLEY.Services;

namespace ProyectoZOGLEY.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Cliente")]
    [ApiController]
    public class ComprasUsuarioController : ControllerBase
    {
        private readonly ComprasUsuarioService _comprasUsuarioService;
        public ComprasUsuarioController(ComprasUsuarioService comprasUsuarioService)
        {
            _comprasUsuarioService = comprasUsuarioService;
        }

        [HttpPost("RegistrarCompra")]
        public async Task<IActionResult> RegistrarCompra([FromBody] ComprasUsuariosDto compra)
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(claimId, out var idUsuario)) return Unauthorized();

            var resultado = await _comprasUsuarioService.RegistrarComprasAsync(compra, idUsuario);
            if (!resultado.Exito) return BadRequest(resultado.Resultado);
            return Ok(resultado.Resultado);
        }
    }
}
