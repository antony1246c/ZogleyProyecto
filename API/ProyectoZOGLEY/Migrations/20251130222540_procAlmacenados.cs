using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoZOGLEY.Migrations
{
    /// <inheritdoc />
    public partial class procAlmacenados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_ProductosPorGenero
    @Genero NVARCHAR(20)
AS
BEGIN
    SELECT 
        p.NombreProducto,
        p.Genero,
        c.NombreCategoria AS Categoria,
        Precio = CONVERT(DECIMAL(10,2), 
                    CASE 
                        WHEN o.IdOferta IS NOT NULL AND o.Activa = 1 
                        THEN p.Precio - o.DescuentoMonto 
                        ELSE p.Precio 
                    END),
        p.EnlaceIMG
    FROM ProductosRopa p
    JOIN CategoriaRopa c ON p.IdCategoria = c.IdCategoria
    LEFT JOIN Ofertas o ON o.IdProducto = p.IdProducto AND o.Activa = 1
    WHERE p.Genero = @Genero
    ORDER BY Precio ASC;
END");


            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_ProductosPorCategoriaYGenero
    @Categoria NVARCHAR(50),
    @Genero NVARCHAR(20)
AS
BEGIN
    SELECT 
        p.NombreProducto,
        p.Genero,
        c.NombreCategoria AS Categoria,
        Precio = CONVERT(DECIMAL(10,2),
                    CASE 
                        WHEN o.IdOferta IS NOT NULL AND o.Activa = 1
                        THEN p.Precio - o.DescuentoMonto
                        ELSE p.Precio
                    END),
        p.EnlaceIMG
    FROM ProductosRopa p
    JOIN CategoriaRopa c ON p.IdCategoria = c.IdCategoria
    LEFT JOIN Ofertas o ON o.IdProducto = p.IdProducto AND o.Activa = 1
    WHERE c.NombreCategoria = @Categoria
      AND p.Genero = @Genero
    ORDER BY Precio ASC;
END
");


            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_ProductosMasComprados
    @Genero NVARCHAR(20)
AS
BEGIN
    SELECT TOP 20 
        p.IdProducto,
        p.NombreProducto,
        p.Genero,
        cat.NombreCategoria AS Categoria,
        Precio = CONVERT(DECIMAL(10,2),
                    CASE 
                        WHEN o.Activa = 1 THEN p.Precio - o.DescuentoMonto
                        ELSE p.Precio
                    END),
        p.EnlaceIMG,
        SUM(cu.Cantidad) AS Ventas
    FROM ProductosRopa p
    JOIN CategoriaRopa cat ON p.IdCategoria = cat.IdCategoria
    JOIN ComprasUsuario cu ON p.IdProducto = cu.IdProducto
    LEFT JOIN Ofertas o ON p.IdProducto = o.IdProducto AND o.Activa = 1
    WHERE p.Genero = @Genero
    GROUP BY 
        p.IdProducto,
        p.NombreProducto,
        p.Genero,
        cat.NombreCategoria,
        p.Precio,
        p.EnlaceIMG,
        o.DescuentoMonto,
        o.Activa
    ORDER BY Ventas DESC;
END
");


            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_ProductosMasBaratos
    @Genero NVARCHAR(20)
AS
BEGIN
    SELECT 
        p.NombreProducto,
        p.Genero,
        c.NombreCategoria AS Categoria,
        Precio = CONVERT(DECIMAL(10,2), p.Precio),
        p.EnlaceIMG
    FROM ProductosRopa p
    JOIN CategoriaRopa c ON p.IdCategoria = c.IdCategoria
    JOIN Ofertas o ON o.IdProducto = p.IdProducto AND o.Activa = 1  -- Solo productos con oferta activa
    WHERE p.Genero = @Genero
    ORDER BY p.Precio ASC;
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_ProductosNuevosPorGenero
    @Genero NVARCHAR(20)
AS
BEGIN
    SELECT TOP 20 
        p.NombreProducto,
        p.Genero,
        c.NombreCategoria AS Categoria,
        Precio = CONVERT(DECIMAL(10,2), p.Precio),
        p.EnlaceIMG
    FROM ProductosRopa p
    JOIN CategoriaRopa c ON p.IdCategoria = c.IdCategoria
    WHERE p.Genero = @Genero
    ORDER BY p.FechaCreacion DESC;
END");


            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_RegistrarCompra
    @IdUsuario INT,
    @IdProducto INT,
    @Cantidad INT,
    @Talla NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Precio DECIMAL(10,2);
    DECLARE @StockActual INT;

    -- Obtener precio del producto
    SELECT @Precio = ISNULL(Precio,0)
    FROM ProductosRopa
    WHERE IdProducto = @IdProducto;

    -- Obtener stock actual
    SELECT @StockActual = Stock
    FROM InventarioProductos
    WHERE IdProducto = @IdProducto;

    -- Validar si hay stock
    IF (@StockActual IS NULL)
    BEGIN
        RAISERROR('Producto no existe en inventario.', 16, 1);
        RETURN;
    END

    IF (@StockActual < @Cantidad)
    BEGIN
        RAISERROR('Stock insuficiente.', 16, 1);
        RETURN;
    END
    -- Registrar compra 
    INSERT INTO ComprasUsuario (IdUsuario, IdProducto, Cantidad, PrecioUnitario, Talla, FechaCompra)
    VALUES (@IdUsuario, @IdProducto, @Cantidad, @Precio, @Talla, GETDATE());

    -- Actualizar inventario
    UPDATE InventarioProductos
    SET Stock = Stock - @Cantidad
    WHERE IdProducto = @IdProducto;

END");

            migrationBuilder.Sql(@"

CREATE PROCEDURE sp_VentasDelDia
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        COUNT(*) AS TotalVentas
    FROM ComprasUsuario
    WHERE CAST(FechaCompra AS DATE) = CAST(GETDATE() AS DATE);
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_IngresosMesActual
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        FORMAT(GETDATE(), 'MMMM', 'es-ES') AS Mes, -- mes en español
        YEAR(GETDATE()) AS Año,
        ISNULL(SUM(Cantidad * PrecioUnitario), 0) AS IngresosTotales
    FROM ComprasUsuario
    WHERE 
        MONTH(FechaCompra) = MONTH(GETDATE())
        AND YEAR(FechaCompra) = YEAR(GETDATE());
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_UsuariosRegistrados
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        COUNT(*) AS TotalUsuarios
    FROM UsuariosInfo;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_VentasAgrupadas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.IdProducto,
        p.NombreProducto,
        p.Genero,
        SUM(c.Cantidad) AS CantidadTotal,
        p.Precio AS PrecioIndividual,
        SUM(c.Cantidad) * p.Precio AS TotalGenerado
    FROM ComprasUsuario c
    INNER JOIN ProductosRopa p ON c.IdProducto = p.IdProducto
    GROUP BY 
        p.IdProducto,
        p.NombreProducto,
        p.Genero,
        p.Precio
    ORDER BY p.IdProducto;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_UsuariosInfoSimple
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        NombreApellido,
        Email,
        Telefono,
        Rol
    FROM UsuariosInfo;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_ProductosRopaSimple
AS
BEGIN
    SET NOCOUNT ON;

    -- Productos con nombre de categoría
    SELECT 
        p.IdProducto,
        p.NombreProducto,
        p.Genero,
        c.NombreCategoria AS Categoria, 
        p.Precio,
        p.EnlaceIMG
    FROM ProductosRopa p
    INNER JOIN CategoriaRopa c
        ON p.IdCategoria = c.IdCategoria
    ORDER BY p.IdProducto;
END");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
