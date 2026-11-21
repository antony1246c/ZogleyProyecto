using Microsoft.EntityFrameworkCore;
using ProyectoZOGLEY.Models;
using ProyectoZOGLEY.Models.DTOS;

namespace ProyectoZOGLEY.Data
{
    /// <summary>
    /// Clase que controla las conexiones con la base de datos
    /// </summary>
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        /* Declaracion de los modelos que se van a utilizar en la base de datos */
        public DbSet<UserInfo> DatosUsuarios { get; set; }
        public DbSet<ProductosRopa> Productos { get; set; }
        public DbSet<ComprasUsuarios> ComprasUsuarios { get; set; }
        public DbSet<InventarioProductos> InventarioProductos { get; set; }
        public DbSet <CategoriaRopa> CategoriaRopa { get; set; }
        public DbSet<Ofertas> OfertasRopa { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetallesFactura { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        /// <summary>
        /// Encargada de dar el formato a las tablas, referencias y crearlas dentro de nuestra BD
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //Tabla UsuariosInfo
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("UsuariosInfo");

                // GeneraciÃ³n de la clave primaria de forma automÃ¡tica con identity
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreApellido)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Telefono)
                      .HasMaxLength(9);

                entity.Property(e => e.Contrasenia)
                      .HasMaxLength(200);

                entity.Property(e => e.Rol)
                      .HasMaxLength(50);
                entity.Property(e => e.Direccion).HasMaxLength(300);
                entity.Property(e => e.Provincia).HasMaxLength(100);
                entity.Property(e => e.Distrito).HasMaxLength(100);
                entity.Property(e => e.Activo).HasDefaultValue(true);
            });


            //Tabla CategoriaRopa
            modelBuilder.Entity<CategoriaRopa>(entity =>
            {
                entity.ToTable("CategoriaRopa");

                // GeneraciÃ³n de la clave primaria de forma automÃ¡tica con identity
                entity.HasKey(e => e.IdCategoria);
                entity.Property(e => e.IdCategoria)
                      .ValueGeneratedOnAdd();

                // Nombre de la categorÃ­a con tamaÃ±o mÃ¡ximo
                entity.Property(e => e.NombreCategoria)
                      .IsRequired()
                      .HasMaxLength(50);

                // RelaciÃ³n 1 - N con ProductosRopa
                entity.HasMany(c => c.Productos)
                      .WithOne(p => p.Categoria)
                      .HasForeignKey(p => p.IdCategoria)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProductosRopa_CategoriaRopa");
            });


            // Tabla ProductosRopa
            modelBuilder.Entity<ProductosRopa>(entity =>
            {
                entity.ToTable("ProductosRopa");

                // GeneraciÃ³n de la clave primaria de forma automÃ¡tica con identity
                entity.HasKey(e => e.IdProducto);
                entity.Property(e => e.IdProducto)
                      .ValueGeneratedOnAdd();

                // Propiedades del producto
                entity.Property(e => e.NombreProducto)
                      .HasMaxLength(100);

                entity.Property(e => e.Genero)
                      .HasMaxLength(20);

                // Precios con tipo decimal
                entity.Property(e => e.Precio)
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.EnlaceIMG)
                      .HasMaxLength(300);

                // Fecha de creaciÃ³n por defecto
                entity.Property(e => e.FechaCreacion)
                      .HasColumnType("date");

                entity.Property(e => e.Activo).HasDefaultValue(true);

                // RelaciÃ³n con CategoriaRopa (FK IdCategoria)
                entity.HasOne(e => e.Categoria)
                      .WithMany(c => c.Productos)
                      .HasForeignKey(e => e.IdCategoria)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProductosRopa_CategoriaRopa");

                // RelaciÃ³n con Ofertas
                entity.HasMany(e => e.Ofertas)
                      .WithOne(o => o.Producto)
                      .HasForeignKey(o => o.IdProducto)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Ofertas_ProductosRopa");
            });



            // Tabla Comprasusuarios
            modelBuilder.Entity<ComprasUsuarios>(entity =>
            {
                entity.ToTable("ComprasUsuario");

                // GeneraciÃ³n de la clave primaria de forma automÃ¡tica con identity
                entity.HasKey(e => e.IdCompra);
                entity.Property(e => e.IdCompra)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.IdUsuario)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(e => e.Cantidad)
                      .IsRequired();

                entity.Property(e => e.PrecioUnitario)
                      .HasPrecision(10, 2);

                entity.Property(e => e.Talla)
                      .HasMaxLength(10);

                entity.Property(e => e.FechaCompra)
                      .HasColumnType("date")
                      .HasDefaultValueSql("GETDATE()");

                // RelaciÃ³n con UsuariosInfo
                entity.HasOne(c => c.Usuario)
                      .WithMany(u => u.Compras)
                      .HasForeignKey(c => c.IdUsuario)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_UsuarioCompras");

                // RelaciÃ³n con ProductosRopa
                entity.HasOne(d => d.Producto)
                      .WithMany(p => p.Compras)
                      .HasForeignKey(d => d.IdProducto)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProductoCompras");
            });


            //tabla InventarioProductos

            modelBuilder.Entity<InventarioProductos>(entity =>
            {
                entity.ToTable("InventarioProductos");

                // GeneraciÃ³n de la clave primaria de forma automÃ¡tica con identity
                entity.HasKey(e => e.IdInventario);
                entity.Property(e => e.IdInventario)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.IdProducto)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(e => e.Stock)
                      .IsRequired();

                // RelaciÃ³n 1 - 1 con ProductosRopa
                entity.HasOne(d => d.Producto)
                      .WithOne(p => p.Inventario)
                      .HasForeignKey<InventarioProductos>(d => d.IdProducto)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_InventarioProductos_ProductosRopa");
            });

            modelBuilder.Entity<Ofertas>(entity =>
            {
                entity.ToTable("Ofertas");

                entity.HasKey(e => e.IdOferta);
                entity.Property(e => e.IdOferta)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.IdProducto)
                      .IsRequired();

                entity.Property(e => e.DescuentoMonto)
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                entity.Property(e => e.Activa)
                      .HasDefaultValue(true);

                entity.HasOne(d => d.Producto)
                      .WithMany(p => p.Ofertas)
                      .HasForeignKey(d => d.IdProducto)
                      .OnDelete(DeleteBehavior.Cascade);
            });



            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("Facturas");
                entity.HasKey(e => e.IdFactura);
                entity.Property(e => e.NumeroFactura).HasMaxLength(30).IsRequired();
                entity.HasIndex(e => e.NumeroFactura).IsUnique();
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
                entity.Property(e => e.ITBMS).HasColumnType("decimal(10,2)");
                entity.Property(e => e.CostoEnvio).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TipoEntrega).HasMaxLength(20);
                entity.Property(e => e.Estado).HasMaxLength(30);
                entity.Property(e => e.MetodoPago).HasMaxLength(40).HasDefaultValue("Contra entrega");
                entity.Property(e => e.ArchivoPdf).HasColumnType("varbinary(max)");
                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.Detalles)
                    .WithOne(d => d.Factura)
                    .HasForeignKey(d => d.IdFactura)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleFactura>(entity =>
            {
                entity.ToTable("DetalleFactura");
                entity.HasKey(e => e.IdDetalleFactura);
                entity.Property(e => e.NombreProducto).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Talla).HasMaxLength(20);
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
            });

            // DTO para procedimientos almacenados
            modelBuilder.Entity<ProductosRopaDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Precio)
                      .HasColumnType("decimal(10,2)");
            });

            //DTO para procedimiento almacenado de ventas del mes
            modelBuilder.Entity<AdminDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Mes)
                      .HasMaxLength(20);

                entity.Property(e => e.Año)
                       .IsRequired();

                // Precios con tipo decimal
                entity.Property(e => e.IngresosTotales)
                      .HasColumnType("decimal(10,2)");

            });

            // DTO para procedimiento almacenado de usuarios y total de ingreso del dida
            modelBuilder.Entity<Admin2Dto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Totales)
                      .IsRequired();
            });

            modelBuilder.Entity<VentasAgrupadasDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdProducto);
                entity.Property(e => e.NombreProducto);
                entity.Property(e => e.Genero);

                entity.Property(e => e.CantidadTotal);

                entity.Property(e => e.PrecioIndividual)
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.TotalGenerado)
                      .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<UsuariosInformacionCompletaDto>(entity =>
            {
                entity.HasNoKey(); // No tiene clave primaria, solo para SP
                entity.Property(e => e.NombreApellido).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(9);
                entity.Property(e => e.Rol).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductosSimpleDto>(entity =>
            {
                entity.HasNoKey(); // No tiene clave primaria
                entity.Property(e => e.IdProducto);
                entity.Property(e => e.NombreProducto).HasMaxLength(100);
                entity.Property(e => e.Genero).HasMaxLength(20);
                entity.Property(e => e.Categoria).HasMaxLength(100);
                entity.Property(e => e.Precio).HasColumnType("decimal(10,2)");
                entity.Property(e => e.EnlaceIMG).HasMaxLength(300);
            });


            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
