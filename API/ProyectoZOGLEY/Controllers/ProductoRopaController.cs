using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Models.DTOS;

namespace ProyectoZOGLEY.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProductoRopaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductoRopaController(ApplicationDbContext context) => _context = context;

        private IQueryable<ProductoCatalogoDto> ConsultaProductos()
        {
            return _context.Productos.AsNoTracking()
                .Where(p => p.Activo)
                .Select(p => new ProductoCatalogoDto
                {
                    IdProducto = p.IdProducto,
                    NombreProducto = p.NombreProducto ?? "",
                    Genero = p.Genero ?? "",
                    Categoria = p.Categoria != null ? p.Categoria.NombreCategoria ?? "" : "",
                    PrecioOriginal = p.Precio ?? 0m,
                    PrecioOferta = p.Ofertas!.Where(o => o.Activa)
                        .Select(o => (decimal?)((p.Precio ?? 0m) >= o.DescuentoMonto ? (p.Precio ?? 0m) - o.DescuentoMonto : 0m))
                        .FirstOrDefault() ?? (p.Precio ?? 0m),
                    Precio = p.Ofertas!.Where(o => o.Activa)
                        .Select(o => (decimal?)((p.Precio ?? 0m) >= o.DescuentoMonto ? (p.Precio ?? 0m) - o.DescuentoMonto : 0m))
                        .FirstOrDefault() ?? (p.Precio ?? 0m),
                    EnlaceIMG = p.EnlaceIMG
                });
        }

        [HttpGet("GetProductosRopaGenero")]
        public async Task<IEnumerable<ProductoCatalogoDto>> ListaProductosGenero([FromQuery] string genero)
        {
            return await ConsultaProductos()
                .Where(p => p.Genero.ToLower() == genero.ToLower())
                .OrderBy(p => p.Precio)
                .ToListAsync();
        }

        [HttpGet("GetProductosRopaCategoriaYgenero")]
        public async Task<IEnumerable<ProductoCatalogoDto>> ListaProductosGeneroCategoria(
            [FromQuery] string genero, [FromQuery] string categoria)
        {
            return await ConsultaProductos()
                .Where(p => p.Genero.ToLower() == genero.ToLower() &&
                            p.Categoria.ToLower() == categoria.ToLower())
                .OrderBy(p => p.Precio)
                .ToListAsync();
        }

        [HttpGet("ListaMasCompradosGenero")]
        public async Task<IEnumerable<ProductoCatalogoDto>> ListaMasCompradosGenero([FromQuery] string genero)
        {
            var ids = await _context.ComprasUsuarios
                .Where(c => c.Producto != null && c.Producto.Activo && c.Producto.Genero == genero)
                .GroupBy(c => c.IdProducto)
                .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .OrderByDescending(x => x.Cantidad)
                .Take(20)
                .Select(x => x.IdProducto)
                .ToListAsync();

            var productos = await ConsultaProductos().Where(p => p.Genero.ToLower() == genero.ToLower()).ToListAsync();
            return ids.Select(id => productos.FirstOrDefault(p => p.IdProducto == id))
                      .OfType<ProductoCatalogoDto>();
        }

        [HttpGet("ListaMasBaratosGenero")]
        public async Task<IEnumerable<ProductoCatalogoDto>> ListaMasBaratosGenero([FromQuery] string genero)
        {
            return await ConsultaProductos()
                .Where(p => p.Genero.ToLower() == genero.ToLower() && p.PrecioOferta < p.PrecioOriginal)
                .OrderBy(p => p.PrecioOferta)
                .ToListAsync();
        }

        [HttpGet("ProductosNuevosPorGenero")]
        public async Task<IEnumerable<ProductoCatalogoDto>> ProductosNuevosPorGenero([FromQuery] string genero)
        {
            var ids = await _context.Productos.AsNoTracking()
                .Where(p => p.Activo && p.Genero == genero)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(20)
                .Select(p => p.IdProducto)
                .ToListAsync();

            var productos = await ConsultaProductos().Where(p => p.Genero.ToLower() == genero.ToLower()).ToListAsync();
            return ids.Select(id => productos.FirstOrDefault(p => p.IdProducto == id))
                      .OfType<ProductoCatalogoDto>();
        }
    }
}
