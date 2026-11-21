using Microsoft.EntityFrameworkCore;
using System.Data;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Models;
using ProyectoZOGLEY.Models.DTOS;

namespace ProyectoZOGLEY.Services
{
    public class FacturaService
    {
        private const decimal TasaItbms = 0.07m;
        private const decimal CostoEnvio = 5.00m;
        private readonly ApplicationDbContext _context;
        private readonly CorreoFacturaService _correo;

        public FacturaService(ApplicationDbContext context, CorreoFacturaService correo)
        {
            _context = context;
            _correo = correo;
        }

        public async Task<(bool Exito, object Resultado)> CrearAsync(int idUsuario, CrearFacturaRequest request)
        {
            if (request.Detalles == null || request.Detalles.Count == 0)
                return (false, new { mensaje = "El carrito está vacío." });

            if (request.Detalles.Any(d => d.IdProducto <= 0 || d.Cantidad <= 0))
                return (false, new { mensaje = "Hay productos o cantidades inválidas." });

            var tipoEntrega = (request.TipoEntrega ?? "recoger").Trim().ToLowerInvariant();
            var metodoPago = (request.MetodoPago ?? "").Trim();
            var metodosPago = new[] { "Contra entrega", "Tarjeta de crédito/débito", "Yappy", "Transferencia bancaria" };
            if (!metodosPago.Contains(metodoPago, StringComparer.OrdinalIgnoreCase))
                return (false, new { mensaje = "Selecciona un método de pago válido." });
            if (tipoEntrega is not ("recoger" or "envio"))
                return (false, new { mensaje = "El tipo de entrega no es válido." });

            if (tipoEntrega == "envio" &&
                (request.Direccion == null ||
                 string.IsNullOrWhiteSpace(request.Direccion.Direccion) ||
                 string.IsNullOrWhiteSpace(request.Direccion.Provincia) ||
                 string.IsNullOrWhiteSpace(request.Direccion.Distrito)))
                return (false, new { mensaje = "Completa la dirección, provincia y distrito." });

            var usuario = await _context.DatosUsuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && u.Activo && u.Rol == "Cliente");
            if (usuario == null)
                return (false, new { mensaje = "No se encontró el usuario autenticado." });

            await using var transaccion = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                var factura = new Factura
                {
                    NumeroFactura = $"ZG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}",
                    IdUsuario = idUsuario,
                    Fecha = DateTime.UtcNow,
                    TipoEntrega = tipoEntrega,
                    Direccion = request.Direccion?.Direccion,
                    Provincia = request.Direccion?.Provincia,
                    Distrito = request.Direccion?.Distrito,
                    Referencia = request.Direccion?.Referencia,
                    Notas = request.Notas,
                    Estado = "Pendiente",
                    MetodoPago = metodosPago.First(m => m.Equals(metodoPago, StringComparison.OrdinalIgnoreCase))
                };

                decimal subtotal = 0m;
                foreach (var linea in request.Detalles)
                {
                    var producto = await _context.Productos
                        .Include(p => p.Inventario)
                        .Include(p => p.Ofertas)
                        .FirstOrDefaultAsync(p => p.IdProducto == linea.IdProducto && p.Activo);

                    if (producto == null)
                        throw new InvalidOperationException($"El producto {linea.IdProducto} no existe o no está disponible.");

                    if (producto.Inventario == null || producto.Inventario.Stock < linea.Cantidad)
                        throw new InvalidOperationException($"No hay inventario suficiente para {producto.NombreProducto}.");

                    var precioNormal = producto.Precio ?? 0m;
                    var oferta = producto.Ofertas?.FirstOrDefault(o => o.Activa);
                    var precioUnitario = oferta == null
                        ? precioNormal
                        : Math.Max(0m, precioNormal - oferta.DescuentoMonto);

                    var totalLinea = decimal.Round(precioUnitario * linea.Cantidad, 2);
                    subtotal += totalLinea;
                    producto.Inventario.Stock -= linea.Cantidad;

                    factura.Detalles.Add(new DetalleFactura
                    {
                        IdProducto = producto.IdProducto,
                        NombreProducto = producto.NombreProducto ?? "Producto",
                        Talla = linea.Talla,
                        Cantidad = linea.Cantidad,
                        PrecioUnitario = precioUnitario,
                        Subtotal = totalLinea
                    });

                    // Se conserva el registro histórico de compras que usa el panel de estadísticas actual.
                    _context.ComprasUsuarios.Add(new ComprasUsuarios
                    {
                        IdUsuario = idUsuario,
                        IdProducto = producto.IdProducto,
                        Cantidad = linea.Cantidad,
                        PrecioUnitario = precioUnitario,
                        Talla = linea.Talla,
                        FechaCompra = DateTime.Now
                    });
                }

                factura.Subtotal = decimal.Round(subtotal, 2);
                factura.ITBMS = decimal.Round(factura.Subtotal * TasaItbms, 2);
                factura.CostoEnvio = tipoEntrega == "envio" ? CostoEnvio : 0m;
                factura.Total = factura.Subtotal + factura.ITBMS + factura.CostoEnvio;

                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();

                var pdf = PdfFacturaGenerator.Generar(factura, factura.Detalles, usuario.NombreApellido ?? "", usuario.Email ?? "");
                factura.ArchivoPdf = pdf;
                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();

                var correoEnviado = await _correo.EnviarFacturaAsync(usuario, factura, pdf);
                if (correoEnviado)
                {
                    factura.CorreoEnviado = true;
                    await _context.SaveChangesAsync();
                }

                return (true, new
                {
                    idFactura = factura.IdFactura,
                    numeroFactura = factura.NumeroFactura,
                    total = factura.Total,
                    correoEnviado,
                    urlFactura = $"/api/Facturas/Descargar/{factura.IdFactura}"
                });
            }
            catch (InvalidOperationException ex)
            {
                await transaccion.RollbackAsync();
                return (false, new { mensaje = ex.Message });
            }
            catch
            {
                await transaccion.RollbackAsync();
                return (false, new { mensaje = "No se pudo registrar la factura. No se realizó ningún cobro ni descuento de inventario." });
            }
        }
    }
}
