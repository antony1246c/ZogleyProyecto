using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Models;
using ProyectoZOGLEY.Models.DTOS;

namespace ProyectoZOGLEY.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AdministradorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdministradorController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetVentasDelDia")]
        public async Task<IEnumerable<Admin2Dto>> ListaVentasDelDia() =>
            await _context.Set<Admin2Dto>().FromSqlRaw("EXEC sp_VentasDelDia").ToListAsync();

        [HttpGet("GetIngresosDelMes")]
        public async Task<IEnumerable<AdminDto>> ListaIngresosDelMes() =>
            await _context.Set<AdminDto>().FromSqlRaw("EXEC sp_IngresosMesActual").ToListAsync();

        [HttpGet("GetTodosLosRegistros")]
        public async Task<IEnumerable<Admin2Dto>> ListaUsuariosRegistrados() =>
            await _context.Set<Admin2Dto>().FromSqlRaw("EXEC sp_UsuariosRegistrados").ToListAsync();

        [HttpGet("VentasAgrupadasTotales")]
        public async Task<IEnumerable<VentasAgrupadasDto>> ListaVentasTotales() =>
            await _context.Set<VentasAgrupadasDto>().FromSqlRaw("EXEC sp_VentasAgrupadas").ToListAsync();

        [HttpGet("UsuariosTotales")]
        public async Task<IEnumerable<UsuariosInformacionCompletaDto>> ListaUsuariosTotales() =>
            await _context.Set<UsuariosInformacionCompletaDto>().FromSqlRaw("EXEC sp_UsuariosInfoSimple").ToListAsync();
        [HttpGet("ProductosTotales")]
        public async Task<IActionResult> ListaProductosTotales()
        {
            var productos = await _context.Productos.AsNoTracking()
                // No filtramos por Activo: el administrador debe ver todos.
                .Select(p => new
                {
                    idProducto = p.IdProducto,
                    nombreProducto = p.NombreProducto,
                    genero = p.Genero,
                    categoria = p.Categoria != null ? p.Categoria.NombreCategoria : "",
                    precio = p.Precio ?? 0m,
                    precioOriginal = p.Precio ?? 0m,
                    precioOferta = p.Ofertas!.Where(o => o.Activa)
                        .Select(o => (decimal?)((p.Precio ?? 0m) >= o.DescuentoMonto
                            ? (p.Precio ?? 0m) - o.DescuentoMonto
                            : 0m))
                        .FirstOrDefault() ?? (p.Precio ?? 0m),
                    enlaceIMG = p.EnlaceIMG,

                    // Nuevo campo para identificar el estado.
                    activo = p.Activo
                })
                .ToListAsync();

            return Ok(productos);
        }

        [HttpPut("Productos/{id:int}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ActualizarProductoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NombreProducto) ||
                string.IsNullOrWhiteSpace(request.Genero) ||
                string.IsNullOrWhiteSpace(request.Categoria) ||
                request.Precio < 0)
                return BadRequest(new { mensaje = "Completa los datos del producto y utiliza precios válidos." });

            var producto = await _context.Productos
                .Include(p => p.Ofertas)
                .FirstOrDefaultAsync(p => p.IdProducto == id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado." });

            var categoria = await _context.CategoriaRopa
                .FirstOrDefaultAsync(c => c.NombreCategoria != null &&
                    c.NombreCategoria.ToLower() == request.Categoria.Trim().ToLower());
            if (categoria == null) return BadRequest(new { mensaje = "La categoría indicada no existe." });

            var precioOriginal = request.PrecioOriginal ?? request.Precio;
            var precioOferta = request.PrecioOferta ?? precioOriginal;
            if (precioOriginal < 0 || precioOferta < 0 || precioOferta > precioOriginal)
                return BadRequest(new { mensaje = "El precio de oferta debe estar entre cero y el precio normal." });

            producto.NombreProducto = request.NombreProducto.Trim();
            producto.Genero = request.Genero.Trim();
            producto.IdCategoria = categoria.IdCategoria;
            producto.Precio = precioOriginal;
            if (request.EnlaceIMG != null)
            {
                producto.EnlaceIMG = request.EnlaceIMG.Trim();
            }
            var ofertaActiva = producto.Ofertas?.FirstOrDefault(o => o.Activa);
            foreach (var oferta in producto.Ofertas ?? new List<Ofertas>())
                oferta.Activa = false;

            var descuento = precioOriginal - precioOferta;
            if (descuento > 0)
            {
                if (ofertaActiva == null)
                {
                    _context.OfertasRopa.Add(new Ofertas
                    {
                        IdProducto = producto.IdProducto,
                        DescuentoMonto = descuento,
                        Activa = true
                    });
                }
                else
                {
                    ofertaActiva.DescuentoMonto = descuento;
                    ofertaActiva.Activa = true;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Productos/{id:int}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado." });
            producto.Activo = false; // Baja lógica: conserva facturas, compras y estadísticas.
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("Ventas")]
        public async Task<IActionResult> VentasDetalladas()
        {
            var ventas = await _context.Facturas.AsNoTracking()
                .OrderByDescending(f => f.Fecha)
                .Select(f => new
                {
                    idVenta = f.IdFactura,
                    numeroFactura = f.NumeroFactura,
                    fecha = f.Fecha,
                    cliente = f.Usuario != null ? f.Usuario.NombreApellido : "Cliente",
                    email = f.Usuario != null ? f.Usuario.Email : "",
                    total = f.Total,
                    estado = f.Estado,
                    correoEnviado = f.CorreoEnviado
                }).ToListAsync();
            return Ok(ventas);
        }

        [HttpPatch("Ventas/{id:int}/estado")]
        public async Task<IActionResult> ActualizarEstadoVenta(int id, [FromBody] CambiarEstadoVentaRequest request)
        {
            var estados = new[] { "Pendiente", "Pagada", "Preparando", "Enviada", "Entregada", "Cancelada" };
            var estado = estados.FirstOrDefault(e => e.Equals(request.Estado, StringComparison.OrdinalIgnoreCase));
            if (estado == null) return BadRequest(new { mensaje = "Estado de venta no válido." });

            var factura = await _context.Facturas.FirstOrDefaultAsync(f => f.IdFactura == id);
            if (factura == null) return NotFound(new { mensaje = "Venta no encontrada." });
            if (factura.Estado == "Cancelada" && estado != "Cancelada")
                return Conflict(new { mensaje = "Una venta cancelada no puede reactivarse desde este panel." });

            if (estado == "Cancelada" && factura.Estado != "Cancelada")
                await ReintegrarInventarioAsync(factura.IdFactura);

            factura.Estado = estado;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Ventas/{id:int}")]
        public async Task<IActionResult> EliminarVenta(int id)
        {
            var factura = await _context.Facturas.FirstOrDefaultAsync(f => f.IdFactura == id);
            if (factura == null) return NotFound(new { mensaje = "Venta no encontrada." });
            if (factura.Estado != "Cancelada")
            {
                await ReintegrarInventarioAsync(factura.IdFactura);
                factura.Estado = "Cancelada";
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
        [HttpPut("Productos/{id:int}/Estado")]
        public async Task<IActionResult> CambiarEstadoProducto(
    int id,
    [FromBody] CambiarEstadoProductoRequest request)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado." });

            producto.Activo = request.Activo;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = request.Activo
                    ? "Producto activado correctamente."
                    : "Producto desactivado correctamente.",
                activo = producto.Activo
            });
        }
        private async Task ReintegrarInventarioAsync(int idFactura)
        {
            var detalles = await _context.DetallesFactura
                .Where(d => d.IdFactura == idFactura)
                .ToListAsync();

            foreach (var detalle in detalles)
            {
                var inventario = await _context.InventarioProductos
                    .FirstOrDefaultAsync(i => i.IdProducto == detalle.IdProducto);
                if (inventario != null)
                    inventario.Stock += detalle.Cantidad;
            }
        }


    }


}
