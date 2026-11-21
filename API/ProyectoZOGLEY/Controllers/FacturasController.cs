using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Models.DTOS;
using ProyectoZOGLEY.Services;

namespace ProyectoZOGLEY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Cliente")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class FacturasController : ControllerBase
    {
        private readonly FacturaService _facturaService;
        private readonly ApplicationDbContext _context;

        public FacturasController(FacturaService facturaService, ApplicationDbContext context)
        {
            _facturaService = facturaService;
            _context = context;
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearFacturaRequest request)
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(claimId, out var idUsuario))
                return Unauthorized(new { mensaje = "El token no contiene un identificador de usuario válido." });

            var resultado = await _facturaService.CrearAsync(idUsuario, request);
            if (!resultado.Exito)
                return BadRequest(resultado.Resultado);

            return Ok(resultado.Resultado);
        }

        [HttpGet("Descargar/{id:int}")]
        public async Task<IActionResult> Descargar(int id)
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(claimId, out var idUsuario))
                return Unauthorized();

            var factura = await _context.Facturas.AsNoTracking()
                .FirstOrDefaultAsync(f => f.IdFactura == id && f.IdUsuario == idUsuario);

            if (factura?.ArchivoPdf == null)
                return NotFound(new { mensaje = "No se encontró la factura." });

            return File(factura.ArchivoPdf, "application/pdf", $"Factura-{factura.NumeroFactura}.pdf");
        }
    }
}
