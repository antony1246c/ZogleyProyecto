using System.Globalization;
using System.Text;
using ProyectoZOGLEY.Models;

namespace ProyectoZOGLEY.Services
{
    public static class PdfFacturaGenerator
    {
        public static byte[] Generar(Factura factura, IEnumerable<DetalleFactura> detalles, string nombreCliente, string correo)
        {
            var lineas = new List<string>
            {
                "ZOGLEY - FACTURA",
                $"Factura: {factura.NumeroFactura}",
                $"Fecha: {factura.Fecha.ToLocalTime():dd/MM/yyyy HH:mm}",
                $"Cliente: {nombreCliente}",
                $"Correo: {correo}",
                 $"Entrega: {factura.TipoEntrega}",
                $"Método de pago: {factura.MetodoPago}",
                "------------------------------------------------------------",
                "PRODUCTO                 CANT.     PRECIO       SUBTOTAL"
            };

            foreach (var d in detalles)
            {
                var nombre = d.NombreProducto.Length > 23 ? d.NombreProducto[..23] : d.NombreProducto;
                lineas.Add($"{nombre,-23} {d.Cantidad,4} {d.PrecioUnitario,10:F2} {d.Subtotal,12:F2}");
                if (!string.IsNullOrWhiteSpace(d.Talla))
                    lineas.Add($"   Talla: {d.Talla}");
            }

            lineas.Add("------------------------------------------------------------");
            lineas.Add($"Subtotal: {factura.Subtotal.ToString("C2", CultureInfo.GetCultureInfo("es-PA"))}");
            lineas.Add($"ITBMS (7%): {factura.ITBMS.ToString("C2", CultureInfo.GetCultureInfo("es-PA"))}");
            lineas.Add($"Envio: {factura.CostoEnvio.ToString("C2", CultureInfo.GetCultureInfo("es-PA"))}");
            lineas.Add($"TOTAL: {factura.Total.ToString("C2", CultureInfo.GetCultureInfo("es-PA"))}");
            if (!string.IsNullOrWhiteSpace(factura.Direccion))
                lineas.Add($"Direccion: {factura.Direccion}, {factura.Distrito}, {factura.Provincia}");
            if (!string.IsNullOrWhiteSpace(factura.Notas))
                lineas.Add($"Notas: {factura.Notas}");

            var contenido = new StringBuilder();
            contenido.AppendLine("BT");
            contenido.AppendLine("/F1 10 Tf");
            contenido.AppendLine("50 790 Td");
            foreach (var linea in lineas)
            {
                contenido.Append('(').Append(Escapar(linea)).AppendLine(") Tj");
                contenido.AppendLine("0 -16 Td");
            }
            contenido.AppendLine("ET");

            var streamBytes = Encoding.Latin1.GetBytes(contenido.ToString());
            var objetos = new List<byte[]>
            {
                Encoding.Latin1.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"),
                Encoding.Latin1.GetBytes("<< /Type /Pages /Kids [3 0 R] /Count 1 >>"),
                Encoding.Latin1.GetBytes("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>"),
                Encoding.Latin1.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>"),
                BuildStream(streamBytes)
            };

            using var ms = new MemoryStream();
            void Write(string value) { var b = Encoding.Latin1.GetBytes(value); ms.Write(b, 0, b.Length); }
            Write("%PDF-1.4\n");
            var offsets = new List<long> { 0 };
            for (int i = 0; i < objetos.Count; i++)
            {
                offsets.Add(ms.Position);
                Write($"{i + 1} 0 obj\n");
                ms.Write(objetos[i], 0, objetos[i].Length);
                Write("\nendobj\n");
            }
            var xref = ms.Position;
            Write($"xref\n0 {objetos.Count + 1}\n");
            Write("0000000000 65535 f \n");
            foreach (var offset in offsets.Skip(1))
                Write($"{offset:0000000000} 00000 n \n");
            Write($"trailer\n<< /Size {objetos.Count + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF");
            return ms.ToArray();
        }

        private static byte[] BuildStream(byte[] bytes)
        {
            using var ms = new MemoryStream();
            var header = Encoding.Latin1.GetBytes($"<< /Length {bytes.Length} >>\nstream\n");
            ms.Write(header, 0, header.Length);
            ms.Write(bytes, 0, bytes.Length);
            var footer = Encoding.Latin1.GetBytes("\nendstream");
            ms.Write(footer, 0, footer.Length);
            return ms.ToArray();
        }

        private static string Escapar(string value) =>
            value.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)")
                 .Replace("\r", " ").Replace("\n", " ");
    }
}
