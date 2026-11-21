/*
    ZOGLEY - INSERTS CORREGIDOS
    IMPORTANTE:
    - Este script presupone una base de datos VACÍA para estas tablas.
    - Los IDs de CategoriaRopa y ProductosRopa deben iniciar en 1.
    - Categorias esperadas: 1-11.
    - Productos esperados: 1-98 (58 mujer + 40 hombre).
    - Si ya existen registros, NO ejecutes este script directamente:
      los IDENTITY podrían ser diferentes y se duplicarían datos.
    - Confirma que los nombres reales de columnas coincidan con los INSERT.
*/

--- INSERTS CATEGORIAS
INSERT INTO CategoriaRopa (NombreCategoria)
VALUES
    -- Categorías de Mujer
    ('Blusas'),
    ('Vestidos'),
    ('Faldas'),
    ('Conjuntos'),
    ('Ropa Formal'),
    ('Vestidos Tipicos'),

    -- Categorías de Hombre
    ('Pantalones'),
    ('Sweaters'),
    ('Ropa Casual'),
    ('Ropa Formal'),
    ('Trajes Tipicos');

-- INSERTS PARA ROPA DE MUJER --

-- BLUSAS
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Blusa Básica Algodón', 'Mujer', 1, 12.99, 'https://i.ibb.co/XZGX1W6k/Blusa-basica-algodon.jpg', GETDATE()),
('Blusa Manga Corta', 'Mujer', 1, 14.50, 'https://i.ibb.co/rqcwxz9/Blusa-manga-corta.jpg', GETDATE()),
('Blusa Estampado Floral', 'Mujer', 1, 16.99, 'https://i.ibb.co/jPMysHWp/Blusa-estampado-floral.jpg', GETDATE()),
('Blusa Elegante Seda', 'Mujer', 1, 24.90, 'https://i.ibb.co/B2LBWh5Z/Blusa-elegante-seda.jpg', GETDATE()),
('Blusa Casual Cuello V', 'Mujer', 1, 18.50, 'https://i.ibb.co/5hWFHj4k/Blusa-elegante-cuello-V.jpg', GETDATE()),
('Blusa Manga Larga', 'Mujer', 1, 19.99, 'https://i.ibb.co/bMkfggx5/Blusa-manga-larga.jpg', GETDATE()),
('Blusa Crop Top', 'Mujer', 1, 13.50, 'https://i.ibb.co/YTqy22qY/Blusa-crop-top.jpg', GETDATE()),
('Blusa Tejida Ligera', 'Mujer', 1, 17.50, 'https://i.ibb.co/8LQ4R9rG/Blusa-tejida-ligera.jpg', GETDATE()),
('Blusa Estilo Boho', 'Mujer', 1, 21.99, 'https://i.ibb.co/V0kyt4nf/Blusa-estilo-boho.jpg', GETDATE()),
('Blusa de Oficina', 'Mujer', 1, 22.50, 'https://i.ibb.co/twmxvf2c/Blusa-de-oficina.jpg', GETDATE());

-- VESTIDOS
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Vestido Casual Verano', 'Mujer', 2, 27.99, 'https://i.ibb.co/GQHVbySB/Vestido-casual-verano.jpg', GETDATE()),
('Vestido Elegante Liso', 'Mujer', 2, 45.99, 'https://i.ibb.co/WWyXTKwM/Vestido-elegante-liso.jpg', GETDATE()),
('Vestido Floral Corto', 'Mujer', 2, 32.50, 'https://i.ibb.co/psF8Wpb/Vestido-floral-corto.jpg', GETDATE()),
('Vestido de Fiesta Largo', 'Mujer', 2, 59.99, 'https://i.ibb.co/XZTjxxqp/Vestido-de-fiesta-largo.jpg', GETDATE()),
('Vestido Casual Manga Corta', 'Mujer', 2, 24.50, 'https://i.ibb.co/Jwc0w3P0/Vestido-casual-manga-larga.jpg', GETDATE()),
('Vestido Bodycon', 'Mujer', 2, 34.99, 'https://i.ibb.co/Q7w4sCjf/Vestido-bodycon.jpg', GETDATE()),
('Vestido Elegante Brillante', 'Mujer', 2, 69.50, 'https://i.ibb.co/KxyCXNnV/Vestido-elegante-brillante.jpg', GETDATE()),
('Vestido Denim', 'Mujer', 2, 28.50, 'https://i.ibb.co/6Q8Y3Gj/Vestido-denim.jpg', GETDATE()),
('Vestido Plisado', 'Mujer', 2, 33.99, 'https://i.ibb.co/0Rn18qb7/Vestido-plisado.jpg', GETDATE()),
('Vestido Maxi', 'Mujer', 2, 38.75, 'https://i.ibb.co/Q7W2pVGV/Vestido-maxi.jpg', GETDATE());

-- FALDAS
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Falda Plisada', 'Mujer', 3, 18.99, 'https://i.ibb.co/ccDwNyF5/Falda-plisada.jpg', GETDATE()),
('Falda Lápiz', 'Mujer', 3, 24.50, 'https://i.ibb.co/39hJtrTg/Falda-lapiz.jpg', GETDATE()),
('Falda Denim Corta', 'Mujer', 3, 21.99, 'https://i.ibb.co/qF2JzLRs/Falda-denim-corta.jpg', GETDATE()),
('Falda Casual Verano', 'Mujer', 3, 17.50, 'https://i.ibb.co/vvRcNJNh/Falda-casual-verano.jpg', GETDATE()),
('Falda Tabla Escolar', 'Mujer', 3, 16.99, 'https://i.ibb.co/93w5SXf8/Falda-tabla-escolar.jpg', GETDATE()),
('Falda Midi Elegante', 'Mujer', 3, 29.99, 'https://i.ibb.co/Y7C8BjHq/Falda-mini-elegante.jpg', GETDATE()),
('Falda Satinada', 'Mujer', 3, 27.50, 'https://i.ibb.co/chQJVNxH/Falda-satinada.jpg', GETDATE()),
('Falda Vintage', 'Mujer', 3, 23.99, 'https://i.ibb.co/vC8hzvRk/Falda-vintage.jpg', GETDATE()),
('Falda Stretch', 'Mujer', 3, 19.50, 'https://i.ibb.co/wZXNY7xb/Falda-stretch.jpg', GETDATE()),
('Falda Bohemia', 'Mujer', 3, 26.50, 'https://i.ibb.co/TxwfCBky/Falda-bohemia.jpg', GETDATE());

-- CONJUNTOS
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Conjunto Deportivo Mujer', 'Mujer', 4, 32.99, 'https://i.ibb.co/qtLTrRN/Conjunto-deportivo.jpg', GETDATE()),
('Conjunto Casual 2 Piezas', 'Mujer', 4, 29.99, 'https://i.ibb.co/S7PtvXpr/Conjunto-casual-2-piezas.jpg', GETDATE()),
('Conjunto Elegante', 'Mujer', 4, 45.50, 'https://i.ibb.co/BVvmyrK6/Conjunto-elegante.jpg', GETDATE()),
('Conjunto de Verano', 'Mujer', 4, 27.50, 'https://i.ibb.co/21q0BhWQ/Conjunto-de-verano.jpg', GETDATE()),
('Conjunto Oversize', 'Mujer', 4, 34.99, 'https://i.ibb.co/3y0HdJWz/Conjunto-oversize.jpg', GETDATE()),
('Conjunto Formal Femenino', 'Mujer', 4, 49.99, 'https://i.ibb.co/cKWvFdbn/Conjunto-formal-femenino.jpg', GETDATE()),
('Conjunto Croptop + Falda', 'Mujer', 4, 31.75, 'https://i.ibb.co/Lh6S1brH/Conjunto-crop-top-falda.jpg', GETDATE()),
('Conjunto Suave Algodón', 'Mujer', 4, 25.99, 'https://i.ibb.co/dsSZdq2R/Conjunto-suave-algodon.jpg', GETDATE()),
('Conjunto Estampado', 'Mujer', 4, 33.50, 'https://i.ibb.co/XrG3gMN3/Conjunto-estampado.jpg', GETDATE()),
('Conjunto Nocturno Mujer', 'Mujer', 4, 42.99, 'https://i.ibb.co/RksRrp13/Conjunto-nocturno-mujer.jpg', GETDATE());

-- ROPA FORMAL MUJER
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Blazer Formal Mujer', 'Mujer', 5, 39.99, 'https://i.ibb.co/WpzTSTFp/Blazer-formal-mujer.jpg', GETDATE()),
('Blazer Negro Mujer', 'Mujer', 5, 42.50, 'https://i.ibb.co/c78vY1V/Blazer-negro-mujer.jpg', GETDATE()),
('Pantalón Formal Mujer', 'Mujer', 5, 28.99, 'https://i.ibb.co/wZHsQMyg/Pantalon-formal-mujer.jpg', GETDATE()),
('Camisa Formal Femenina', 'Mujer', 5, 21.99, 'https://i.ibb.co/qYX8WTM2/Camisa-formal-femenina.jpg', GETDATE()),
('Falda Formal Oficina', 'Mujer', 5, 24.75, 'https://i.ibb.co/GvBv8B9D/Falda-formal-oficina.jpg', GETDATE()),
('Blazer Beige Mujer', 'Mujer', 5, 44.99, 'https://i.ibb.co/KcjLJqjT/Blazer-beige-mujer.jpg', GETDATE()),
('Camisa Manga Larga Formal', 'Mujer', 5, 23.50, 'https://i.ibb.co/rG5H2yGG/Camisa-manga-larga-formal.jpg', GETDATE()),
('Chaqueta Sastre Negra', 'Mujer', 5, 29.50, 'https://i.ibb.co/hRnwn03N/Chaqueta-sastre-negra.jpg', GETDATE()),
('Blazer Corte Italiano', 'Mujer', 5, 48.99, 'https://i.ibb.co/xSJDXs4H/Blazer-corte-italiano-mujer.jpg', GETDATE()),
('Camisa Satinada Formal', 'Mujer', 5, 27.99, 'https://i.ibb.co/SDz06Ggd/Camisa-satinada-formal.jpg', GETDATE());

-- VESTIDOS TIPICOS
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Conjunto Tipico', 'Mujer', 6, 39.99, 'https://i.ibb.co/jkkCFWx0/Conjunto-tipico-mujer.jpg', GETDATE()),
('Camisa Tipica alusiva Anaranjada', 'Mujer', 6, 30.99, 'https://i.ibb.co/XZGX1W6k/Camisa-alusiva-anaranjada.jpg', GETDATE()),
('Camisa Tipica alusiva Morada', 'Mujer', 6, 30.99, 'https://i.ibb.co/QFZjWpcF/Camisa-tipica-alusiva-morada.jpg', GETDATE()),
('Camisa Tipica - Nagua', 'Mujer', 6, 40.99, 'https://i.ibb.co/4wt0HTBS/Camisa-tipica-nagua.jpg', GETDATE()),
('Camisa Tipica alusiva - Verde/Anaranjada', 'Mujer', 6, 29.99, 'https://i.ibb.co/hx2m883p/Camisa-tipica-verde-anaranjada.jpg', GETDATE()),
('Camisa Tipica alusiva - Blanca', 'Mujer', 6, 50.99, 'https://i.ibb.co/39929G9k/Camisa-tipica-alusiva-blanca.jpg', GETDATE()),
('Camisa Tipica - Camisola', 'Mujer', 6, 44.99, 'https://i.ibb.co/rW6MbYj/Camisa-tipica-camisola.jpg', GETDATE()),
('Camisa Tipica - Tonosieña Rosada', 'Mujer', 6, 44.99, 'https://i.ibb.co/1YXQNnSj/Camisa-tipica-tonosie-a-rosada-mujer.jpg', GETDATE());


-- INSERTS PARA ROPA DE HOMBRE --

-- PANTALONES HOMBRE
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Pantalón Jeans', 'Hombre', 7, 29.99, 'https://i.ibb.co/qYyJ7fs1/Pantalon-jeans.jpg', GETDATE()),
('Pantalón Casual', 'Hombre', 7, 25.50, 'https://i.ibb.co/Z1T7SMmJ/Pantalon-casual.jpg', GETDATE()),
('Pantalón Cargo', 'Hombre', 7, 34.75, 'https://i.ibb.co/mV8Wt1zG/Pantalon-cargo.jpg', GETDATE()),
('Pantalón Chino', 'Hombre', 7, 31.99, 'https://i.ibb.co/R4kQF4Rz/Pantalon-chino.jpg', GETDATE()),
('Pantalón Slim Fit', 'Hombre', 7, 28.99, 'https://i.ibb.co/S40SSH3w/Pantalon-slim-fit.jpg', GETDATE()),
('Pantalón Jogger Hombre', 'Hombre', 7, 22.50, 'https://i.ibb.co/mVHv4Tbx/Pantalon-jogger-hombre.jpg', GETDATE()),
('Pantalón Deportivo Hombre', 'Hombre', 7, 24.99, 'https://i.ibb.co/S4gm8Rxg/Pantalon-deportivo-hombre.jpg', GETDATE()),
('Pantalón Beige Casual', 'Hombre', 7, 26.90, 'https://i.ibb.co/bgq0vsmp/Pantalon-beige-casual.jpg', GETDATE()),
('Pantalón Negro Elegante', 'Hombre', 7, 33.50, 'https://i.ibb.co/rRfVZKdP/Pantalon-negro-elegante.jpg', GETDATE());

-- SUETERES HOMBRE
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Suéter Tejido', 'Hombre', 8, 24.99, 'https://i.ibb.co/9knz7fqD/Sueter-tejido.jpg', GETDATE()),
('Suéter Ligero', 'Hombre', 8, 19.50, 'https://i.ibb.co/pjQvkyG4/Sueter-ligero.jpg', GETDATE()),
('Suéter Invierno', 'Hombre', 8, 29.99, 'https://i.ibb.co/0RSrMhqT/Sueter-invierno.jpg', GETDATE()),
('Suéter Cuello Alto', 'Hombre', 8, 32.50, 'https://i.ibb.co/23bwC61K/Sueter-cuello-alto.jpg', GETDATE()),
('Suéter Casual Algodón', 'Hombre', 8, 22.50, 'https://i.ibb.co/hFQ0MB4c/Sueter-casual-algodon.jpg', GETDATE()),
('Suéter Cuello V', 'Hombre', 8, 34.99, 'https://i.ibb.co/TD629yBH/Sueter-cuello-V.jpg', GETDATE()),
('Suéter Delgado', 'Hombre', 8, 18.75, 'https://i.ibb.co/fVbnB0LP/Sueter-delgado.jpg', GETDATE()),
('Suéter Rayado', 'Hombre', 8, 23.99, 'https://i.ibb.co/zTBYNSJj/Sueter-rayado.jpg', GETDATE()),
('Suéter Deportivo', 'Hombre', 8, 27.50, 'https://i.ibb.co/d4wkcBzT/Sueter-deportivo.jpg', GETDATE()),
('Suéter Blanco Clasico', 'Hombre', 8, 25.99, 'https://i.ibb.co/x8qZ2jxb/Sueter-blanco-clasico.jpg', GETDATE());

-- ROPA CASUAL HOMBRE
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Camiseta Casual', 'Hombre', 9, 12.99, 'https://i.ibb.co/jPSn4LRf/Camiseta-casual.jpg', GETDATE()),
('Hoodie Casual', 'Hombre', 9, 29.99, 'https://i.ibb.co/d0mQsdXg/Hoodie-casual.jpg', GETDATE()),
('Short Casual', 'Hombre', 9, 16.50, 'https://i.ibb.co/Y4BdBzJV/Short-casual.jpg', GETDATE()),
('Camiseta Oversize', 'Hombre', 9, 14.99, 'https://i.ibb.co/xtcV20JN/Camiseta-ovrsize.jpg', GETDATE()),
('Hoodie Estampado', 'Hombre', 9, 32.50, 'https://i.ibb.co/tkJ8c2F/Hoodie-estampado.jpg', GETDATE()),
('Camiseta Básica', 'Hombre', 9, 9.99, 'https://i.ibb.co/HDRN1wkW/Camiseta-basica.jpg', GETDATE()),
('Short Deportivo', 'Hombre', 9, 17.75, 'https://i.ibb.co/fYmDNZtw/Short-deportivo.jpg', GETDATE()),
('Jogger Casual', 'Hombre', 9, 24.99, 'https://i.ibb.co/Kxfr043B/Jogger-casual-hombre.jpg', GETDATE()),
('Sudadera Ligera', 'Hombre', 9, 26.99, 'https://i.ibb.co/BKHVT0Cm/Sudadera-ligera.jpg', GETDATE()),
('Camiseta Rayas', 'Hombre', 9, 13.50, 'https://i.ibb.co/nM7Ydq9h/Camisa-rayas-finas.jpg', GETDATE());

-- ROPA FORMAL HOMBRE
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Saco Formal', 'Hombre', 10, 31.99, 'https://i.ibb.co/GftKwHjV/Saco-formal.jpg', GETDATE()),
('Blazer Formal Hombre', 'Hombre', 10, 54.99, 'https://i.ibb.co/bMjFRtpv/Blazer-formal-hombre.jpg', GETDATE()),
('Camisa Elegante Slim', 'Hombre', 10, 29.50, 'https://i.ibb.co/kgXPP969/Camisa-elegante-slim-hombre.jpg', GETDATE()),
('Camisa Formal Blanca', 'Hombre', 10, 34.90, 'https://i.ibb.co/DgmwD54P/Camisa-formal-blanca.jpg', GETDATE()),
('Camisa Formal Celeste', 'Hombre', 10, 58.50, 'https://i.ibb.co/0ps5V4dN/Camisa-formal-celeste.jpg', GETDATE()),
('Camisa Formal Negra', 'Hombre', 10, 39.99, 'https://i.ibb.co/39TbCVgh/Camisa-formal-negra.jpg', GETDATE()),
('Camisa Rayas Finas', 'Hombre', 10, 27.99, 'https://i.ibb.co/nM7Ydq9h/Camisa-rayas-finas.jpg', GETDATE());

-- ROPA TIPICA HOMBRE
INSERT INTO ProductosRopa (NombreProducto, Genero, IdCategoria, Precio, EnlaceIMG, FechaCreacion)
VALUES
('Camisa Tipica - Rayas Celestes', 'Hombre', 11, 45.00, 'https://i.ibb.co/C5gfCM9v/Camisa-tipica-rayas-celeste.jpg', GETDATE()),
('Camisa Tipica - Rayas Azules', 'Hombre', 11, 50.00, 'https://i.ibb.co/ZkJykHF/Camisa-tipica-rayas-azules.jpg', GETDATE()),
('Camisa Tipica de Gala', 'Hombre', 11, 55.99, 'https://i.ibb.co/rGRCkLZ1/Camisa-tipica-de-gala.jpg', GETDATE()),
('Camisa Tipica- Tonosieña', 'Hombre', 11, 60.00, 'https://i.ibb.co/fzGr8ss1/Camisa-tipica-tonosie-a-hombre.jpg', GETDATE());

--- DESCUENTOS
-- OFERTAS MUJER
-- Categoria 1: Blusas
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (1, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (2, 7.50, 1);

-- Categoria 2: Vestidos
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (11, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (12, 7.50, 1);

-- Categoria 3: Faldas
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (21, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (22, 7.25, 1);

-- Categoria 4: Conjuntos
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (31, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (32, 7.50, 1);

-- Categoria 5: Ropa Formal Mujer
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (41, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (42, 7.50, 1);

-- Categoria 6: Vestidos típicos Mujer
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (51, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (52, 7.25, 1);

-- OFERTAS HOMBRE
-- Categoria 7: Pantalones
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (59, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (60, 7.50, 1);

-- Categoria 8: Suéteres
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (69, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (70, 7.25, 1);

-- Categoria 9: Ropa casual Hombre
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (79, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (80, 7.50, 1);

-- Categoria 10: Ropa formal Hombre
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (89, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (90, 7.50, 1);

-- Categoria 11: Ropa típica Hombre
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (95, 7.00, 1);
INSERT INTO Ofertas (IdProducto, DescuentoMonto, Activa) VALUES (96, 7.25, 1);


-- INVENTARIO COMPLETO: un registro para cada producto (IDs 1-98)
INSERT INTO InventarioProductos (IdProducto, Stock)
VALUES
    (1, 50),
    (2, 40),
    (3, 35),
    (4, 25),
    (5, 30),
    (6, 20),
    (7, 45),
    (8, 50),
    (9, 28),
    (10, 22),
    (11, 40),
    (12, 15),
    (13, 35),
    (14, 10),
    (15, 25),
    (16, 18),
    (17, 8),
    (18, 30),
    (19, 22),
    (20, 20),
    (21, 25),
    (22, 25),
    (23, 25),
    (24, 25),
    (25, 25),
    (26, 25),
    (27, 25),
    (28, 25),
    (29, 25),
    (30, 25),
    (31, 25),
    (32, 25),
    (33, 25),
    (34, 25),
    (35, 25),
    (36, 25),
    (37, 25),
    (38, 25),
    (39, 25),
    (40, 25),
    (41, 25),
    (42, 25),
    (43, 25),
    (44, 25),
    (45, 25),
    (46, 25),
    (47, 25),
    (48, 25),
    (49, 25),
    (50, 25),
    (51, 25),
    (52, 25),
    (53, 25),
    (54, 25),
    (55, 25),
    (56, 25),
    (57, 25),
    (58, 25),
    (59, 25),
    (60, 40),
    (61, 35),
    (62, 30),
    (63, 25),
    (64, 20),
    (65, 45),
    (66, 28),
    (67, 32),
    (68, 18),
    (69, 22),
    (70, 30),
    (71, 25),
    (72, 20),
    (73, 35),
    (74, 28),
    (75, 15),
    (76, 30),
    (77, 22),
    (78, 18),
    (79, 40),
    (80, 20),
    (81, 35),
    (82, 18),
    (83, 25),
    (84, 30),
    (85, 20),
    (86, 28),
    (87, 15),
    (88, 30),
    (89, 22),
    (90, 18),
    (91, 20),
    (92, 15),
    (93, 12),
    (94, 25),
    (95, 18),
    (96, 20),
    (97, 15),
    (98, 10);

-- USUARIOS REGISTRADOS
INSERT INTO UsuariosInfo (NombreApellido, Email, Telefono, Contrasenia, Rol) VALUES
('Zoraima Barsallo', 'zogleyCouture@gmail.com', '6843-1231', '$2a$11$XrZI/0Z3Uf4unkjVuiwr3.0xU8qQ8TabmZ013WiRFmxUzJP4uV8Ce', 'Admin'),
('María Gómez', 'maria.gomez1@gmail.com', '6901-1123', '$2a$11$chqKNweXyIHtPDOvYCIvUes8581Mn7TfjISV5vovoUgKSzW.Dls.2', 'Cliente'),
('Carlos Pérez', 'carlos.perez2@gmail.com', '6123-8841', '$2a$11$bC0NDV.mmksZnTaCbCYyc.ob1HxF9JnXQZlGxoukHpd8dtaf7Ckv.', 'Cliente'),
('Ana Rodríguez', 'ana.rodriguez3@gmail.com', '6782-9911', '$2a$11$xHOcQZBXHjSBp1qYTII4wuySDWfEQFRQynX.NSvlkawzD1uVfqh36', 'Cliente'),
('Luis Morales', 'luis.morales4@gmail.com', '6012-4478', '$2a$11$Pevqy0BzBmRdHIRkRE4hh.TXgn9Ih0k2FvuyFPhwp6OtShxLyrxqi', 'Cliente'),
('Daniela Castillo', 'daniela.castillo5@gmail.com', '6992-3311', '$2a$11$0ZCcYdn8GwxsvtdJ3mNHxelV8FzJ6pP7Pejuh9MuqFhUJOUIgQzrm', 'Cliente'),
('Jorge Sánchez', 'jorge.sanchez6@gmail.com', '6201-1188', '$2a$11$McvilXrbdtW2OH9lMi9A7uHjzi654nEO.VnfAACYdM0NaoFw8mwBq', 'Cliente'),
('Patricia Medina', 'patricia.medina7@gmail.com', '6783-4400', '$2a$11$vcosjTv9MAFf5espBnGJpelWIYVLIjyHUHqVYYnzTeXa3p4Gyc3di', 'Cliente'),
('Ricardo Herrera', 'ricardo.herrera8@gmail.com', '6900-9912', '$2a$11$5Ql5Me6S7qn07xHMhJoQIuD2SW4lWPbTpLVzJUASEdZRmO03qm/Ti', 'Cliente'),
('Laura Torres', 'laura.torres9@gmail.com', '6344-5101', '$2a$11$HjJ2W0KcLI/KQCMoJNdcGeOKALlPJMgr/cLP60oxr/kNulZ9HyGfi', 'Cliente'),
('Miguel Vargas', 'miguel.vargas10@gmail.com', '6611-8710', '$2a$11$idnKC8HyKb/49i4Na.S4/O74Sx7sKproGi1435mtjGs4zMyykKOy.', 'Cliente'),
('Verónica Salcedo', 'veronica.salcedo11@gmail.com', '6904-2232', '$2a$11$JzmuG9teb/DLloSjHDCCpOnYmh6jTpqhjfU2k5BiOuYjQA7M6hkwm', 'Cliente'),
('Jonathan Ríos', 'jonathan.rios12@gmail.com', '6784-3321', '$2a$11$8IerqKLcWu/V6NkYzJOAA.t3IQeL.Gc4nntiiEsHliDZqe6lhzPoa', 'Cliente'),
('Elena Batista', 'elena.batista13@gmail.com', '6019-1002', '$2a$11$24MsDIvcZa1xrjZPYJIOI.mTAY0GVoLJoP6QvF190B0WG0me70mBO', 'Cliente'),
('Oscar Núñez', 'oscar.nunez14@gmail.com', '6931-4482', '$2a$11$nCDgWPdIUbjeFUkvJ3O1R.FzmaKuEDvPqN6gbTFlcnRVA8blqyj1i', 'Cliente'),
('Gabriela Rentería', 'gabriela.renteria15@gmail.com', '6200-9999', '$2a$11$fnsK/boHYBk5kwGlIa/AYerKlXCLJTQKkB/2ACKCdf2C8UttN1dIG', 'Cliente'),
('Samuel Aguilar', 'samuel.aguilar16@gmail.com', '6992-8100', '$2a$11$xPj0.QTwV02YF7ntDNUIR.gN4yIhqbjMQF2ZDn0GAsOS0JOm5nSMO', 'Cliente'),
('Natalia Escobar', 'natalia.escobar17@gmail.com', '6123-5002', '$2a$11$eAIR1/3i97Bt8gSjN/TTPOfAdwSZ7JdNDqgl8N1NRYeFRiDrt8RjW', 'Cliente'),
('Victor Mendoza', 'victor.mendoza18@gmail.com', '6768-1200', '$2a$11$PDoK0sQ2.frLYkv0o8tgD.x4YVGwEE9wwADdspIJ3unAnaTDVdnAu', 'Cliente'),
('Alejandra Domínguez', 'alejandra.dominguez19@gmail.com', '6333-5544', '$2a$11$cDqOzDeID/WR0mPEdfsI7u4Oeh6nPJjNFll5iO/MsV73KqPNF7/RS', 'Cliente'),
('Roberto Carrillo', 'roberto.carrillo20@gmail.com', '6903-1140', '$2a$11$b03AMbrfb.CsaDaUij9sY.m/eAmoH/18n/yxwHHFrpnLQamAAVlHq', 'Cliente'),
('Carmen Rivera', 'carmen.rivera21@gmail.com', '6777-2010', '$2a$11$xu04qXS5OFVlO8ums4Ub5OATaJM1lmhsklj0/f5TYJaNL6d.Asopa', 'Cliente'),
('Héctor Lozano', 'hector.lozano22@gmail.com', '6222-9931', '$2a$11$HPA6z./tVXQjjD56N9TFrO4ZbAG.KSHN9v/8QrTIUvDeaKtvEJYLy', 'Cliente'),
('Lucía Andrade', 'lucia.andrade23@gmail.com', '6390-4400', '$2a$11$JBeOrjWYN76LIjZTPEE1bO6H1zj1Qyra..5kjb72yfFkcWoIANvOy', 'Cliente'),
('Pablo Jiménez', 'pablo.jimenez24@gmail.com', '6888-1290', '$2a$11$buCZltj8iXV8dGwOHfqw4OL9Y6VyzjdBQISbEt58yu7NLW6fx0y3u', 'Cliente'),
('Claudia Rivas', 'claudia.rivas25@gmail.com', '6789-3355', '$2a$11$LeNl4eF0l6UKl7KpdZpFdOjEHtMCwG0AiWqeMloNyXFXZsvHLmogG', 'Cliente'),
('Diego Cárdenas', 'diego.cardenas26@gmail.com', '6904-4120', '$2a$11$ZwNx4551t6E3yseQKkKAyO1dLMlC875UYHSHQKTT0Q0Tcdwtl.5Zm', 'Cliente'),
('Silvia López', 'silvia.lopez27@gmail.com', '6033-8899', '$2a$11$nCRY/LzRp3LHFJHVstiL.OgjfgjernkHpAqPNmtD69KWzGNyVEyKq', 'Cliente'),
('Fernando Garay', 'fernando.garay28@gmail.com', '6910-5550', '$2a$11$O4wdEE8QY7cSPRfCcgY07.HqzLKaPWvp/uXpgHR2asRjJwnwOgdxS', 'Cliente'),
('Paola Bermúdez', 'paola.bermudez29@gmail.com', '6201-6622', '$2a$11$4FsNoByGYvaA0/igrjcFdeoaG7Wbe96tUPLhGacCt/iUlGD4vfU9K', 'Cliente'),
('Javier Sucre', 'javier.sucre30@gmail.com', '6991-2001', '$2a$11$H1.gqDOyz.CG97AymxLS7.gmvDn606QNfy7DDUD4dB2iWxsRqwaE.', 'Cliente'),
('Isabel Cedeño', 'isabel.cedeno31@gmail.com', '6333-7700', '$2a$11$rmAr5rInuHniTvg5ySx4V.dBv0ll3TpjgxZBeIIiiLdeaG.tKFHca', 'Cliente'),
('Rafael Quintero', 'rafael.quintero32@gmail.com', '6800-1125', '$2a$11$lXMv44scJvgAQ64eSXh0oufQdjegTT8yagBIm59xbcEAjBoIrM2bK', 'Cliente'),
('Monica Tapia', 'monica.tapia33@gmail.com', '6012-3331', '$2a$11$W3qM4jnOHGgP5PI1nLc8meN35wW31lmQ4eOrTcgf2hjHxyGYljAJ6', 'Cliente'),
('Esteban Fuentes', 'esteban.fuentes34@gmail.com', '6782-5511', '$2a$11$xX99a965fqdrlgpOuxlIyukqI0okfl.kv7sgrYqoniE3p2uagDCXO', 'Cliente'),
('Rosa Villarreal', 'rosa.villarreal35@gmail.com', '6904-1128', '$2a$11$ARqeOSMsrxsOX2rpH1BuCu82MhpKTqZeGsrzdo1qBipwO0MXmAjhW', 'Cliente'),
('Andrés Correa', 'andres.correa36@gmail.com', '6401-9901', '$2a$11$FnMUbG7vd/Y4nF62Owa5julqYQQmw8mLRsfRKEByvpH9Ep7uZGUf6', 'Cliente'),
('Julieta Olmos', 'julieta.olmos37@gmail.com', '6994-8820', '$2a$11$YZQl1XlHfmvwDrDrmqqrXe6NOCQQ94ak5jibpc6e8XEzq2QrOL.LW', 'Cliente'),
('Marco Jurado', 'marco.jurado38@gmail.com', '6122-7880', '$2a$11$e8yeQlTmk7vQwGkHG44sq.pL5lM2oZAbr6R1ZWV4TrmmZqA76uzlO', 'Cliente'),
('Yolanda Ortega', 'yolanda.ortega39@gmail.com', '6901-5112', '$2a$11$AB57rBhqbR68cXIkiyAWZ.K9aUZSnlEfC0PYDIg2OZu5Ijx5Shv6m', 'Cliente'),
('Tomás Beltrán', 'tomas.beltran40@gmail.com', '6788-3310', '$2a$11$TeoH/ZGydPzVSzTElT5y4.1tZTPUFkJT9a6JTbXzmldP8XNhmVD.i', 'Cliente'),
('Miranda Salazar', 'miranda.salazar41@gmail.com', '6034-4455', '$2a$11$3ksHRBSXTC54UOFYVaCTNuA187SPDa1bsNRElpP7PLoelEbC7YoVu', 'Cliente'),
('Sergio Barrios', 'sergio.barrios42@gmail.com', '6930-2240', '$2a$11$H18LlyJb64CQlzpX98HHhedvo6XbXMH8IHJD9CJYdxIGNILFhUWgm', 'Cliente'),
('Karina Polo', 'karina.polo43@gmail.com', '6204-9877', '$2a$11$PqpOT2eXrbO1jFyqgnwooev4Ox.fjlg/tbgjQMFYiBE6tPsvgPuI6', 'Cliente'),
('Adrián Castillo', 'adrian.castillo44@gmail.com', '6601-7322', '$2a$11$QrXYKP898BA7O/vfBLcYmO7s7yZMd1wRXeb0T1MJE3avE0/2w3f4W', 'Cliente'),
('Marcela Sáenz', 'marcela.saenz45@gmail.com', '6998-2170', '$2a$11$7RvREWeRsVVWTr9cJ5sniesYLRXhWQfNKurKLaXMim16ZEMZnx3.e', 'Cliente'),
('Diana Paredes', 'diana.paredes47@gmail.com', '6712-5520', '$2a$11$hqKcR.T0r48C0U0x6o6J7uemgUWm/0QRBlqWWovWNKWeYKm8fOuLG', 'Cliente'),
('Eduardo Pinto', 'eduardo.pinto48@gmail.com', '6011-2400', '$2a$11$dZ6K10xkbcanujSHj4rXaeJcLGLZaAB.XWi4KYihCrcuI0HN8jYqG', 'Cliente'),
('Melany Castro', 'melany.castro49@gmail.com', '6802-9915', '$2a$11$RoclOy5YAg/6fdqnoo8wuO2rSBDwxEM/q0eHq.HHQxnO6M3mksgPe', 'Cliente'),
('Francisco Villar', 'francisco.villar50@gmail.com', '6909-8840', '$2a$11$jkjkkJjDhSqi6CYc/AWG6uVR7jdtxii1VcbNC1JGr9c6VJpxTL9QO', 'Cliente');



-- COMPRAS USUARIO
INSERT INTO ComprasUsuario (IdUsuario, IdProducto, Cantidad, PrecioUnitario, Talla, FechaCompra) VALUES
(1, 1, 2, 12.99, 'M', '2026-11-30'),
(2, 3, 1, 16.99, 'S', '2026-11-30'),
(3, 5, 3, 18.50, 'L', '2026-11-30'),
(4, 8, 1, 17.50, 'M', '2026-11-30'),
(5, 12, 2, 45.99, 'S', '2026-11-30'),
(6, 15, 1, 24.50, 'M', '2026-11-30'),
(7, 18, 2, 28.50, 'L', '2026-11-30'),
(8, 21, 1, 18.99, 'M', '2026-11-30'),
(9, 24, 2, 17.50, 'S', '2026-11-30'),
(10, 27, 1, 27.50, 'M', '2026-11-30'),
(11, 30, 1, 26.50, 'L', '2026-11-30'),
(12, 33, 1, 45.50, 'S', '2026-11-30'),
(13, 36, 2, 49.99, 'M', '2026-11-30'),
(14, 39, 1, 33.50, 'L', '2026-11-30'),
(15, 42, 1, 42.50, 'M', '2026-11-30'),
(16, 45, 2, 24.75, 'S', '2026-11-30'),
(17, 48, 1, 29.50, 'M', '2026-11-30'),
(18, 51, 1, 39.99, 'L', '2026-11-30'),
(19, 54, 1, 40.99, 'S', '2026-11-30'),
(20, 57, 2, 44.99, 'M', '2026-11-30'),
(21, 60, 1, 29.99, 'L', '2026-11-30'),
(22, 63, 1, 31.99, 'M', '2026-11-30'),
(23, 66, 2, 24.99, 'S', '2026-11-30'),
(24, 69, 1, 24.99, 'L', '2026-11-30'),
(25, 72, 1, 32.50, 'M', '2026-11-30'),
(26, 75, 1, 18.75, 'S', '2026-11-30'),
(27, 78, 2, 25.99, 'M', '2026-11-30'),
(28, 81, 1, 16.50, 'L', '2026-11-30'),
(29, 84, 1, 9.99, 'M', '2026-11-30'),
(30, 87, 2, 26.99, 'S', '2026-11-30'),
(31, 90, 1, 54.99, 'L', '2026-11-30'),
(32, 93, 1, 58.50, 'M', '2026-11-30'),
(33, 96, 1, 45.00, 'S', '2026-11-30'),
(34, 2, 2, 14.50, 'M', '2026-11-30'),
(35, 4, 1, 24.90, 'S', '2026-11-30'),
(36, 7, 1, 13.50, 'L', '2026-11-30'),
(37, 10, 1, 22.50, 'M', '2026-11-30'),
(38, 13, 2, 32.50, 'S', '2026-11-30'),
(39, 16, 1, 34.99, 'L', '2026-11-30'),
(40, 19, 1, 33.99, 'M', '2026-11-30'),
(41, 22, 2, 24.50, 'S', '2026-11-30'),
(42, 25, 1, 16.99, 'M', '2026-11-30'),
(43, 28, 1, 23.99, 'L', '2026-11-30'),
(44, 31, 1, 32.99, 'M', '2026-11-30'),
(45, 34, 2, 27.50, 'S', '2026-11-30'),
(46, 37, 1, 31.75, 'L', '2026-11-30'),
(47, 40, 1, 42.99, 'M', '2026-11-30'),
(48, 43, 2, 28.99, 'S', '2026-11-30')

/* VALIDACIÓN FINAL DE RELACIONES */
SELECT c.IdCategoria, c.NombreCategoria
FROM CategoriaRopa c
ORDER BY c.IdCategoria;

SELECT p.IdProducto, p.NombreProducto, p.Genero, p.IdCategoria, c.NombreCategoria
FROM ProductosRopa p
LEFT JOIN CategoriaRopa c ON c.IdCategoria = p.IdCategoria
ORDER BY p.IdProducto;

-- Productos sin inventario
SELECT p.IdProducto, p.NombreProducto
FROM ProductosRopa p
LEFT JOIN InventarioProductos i ON i.IdProducto = p.IdProducto
WHERE i.IdProducto IS NULL;

-- Ofertas con productos inexistentes
SELECT o.*
FROM Ofertas o
LEFT JOIN ProductosRopa p ON p.IdProducto = o.IdProducto
WHERE p.IdProducto IS NULL;

-- Compras con usuarios o productos inexistentes
SELECT cu.*
FROM ComprasUsuario cu
LEFT JOIN UsuariosInfo u ON u.IdUsuario = cu.IdUsuario
LEFT JOIN ProductosRopa p ON p.IdProducto = cu.IdProducto
WHERE u.IdUsuario IS NULL OR p.IdProducto IS NULL;
