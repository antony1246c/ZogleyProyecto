document.addEventListener("DOMContentLoaded", () => {
    if (typeof exigirSesion !== "function" || !exigirSesion()) return;
    const payload = obtenerPayloadToken();
    const rolToken = payload?.role || payload?.rol || payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    if ((localStorage.getItem("rol") || rolToken || "").toLowerCase() !== "admin") {
        alert("No tienes permisos para acceder al administrador.");
        window.location.replace("index.html");
        return;
    }
    document.getElementById("btnCerrarSesion")?.addEventListener("click", cerrarSesion);
    cargarVentasTotalesDelDia();
    cargarVentasTotalesDelMes();
    cargarUsuariosRegistrados();
});



// Esta función muestra los productos en el HTML mediante un innerHTML - Ventas del dia
function mostrarVentas(lista) {
    const contenedor = document.getElementById("contenedor-estadistica");
    contenedor.innerHTML = ""; // Limpiamos antes de mostrar

    lista.forEach(ventas => {
        const item = document.createElement("div"); // Creamos el div
        if(ventas.totales>20){
        item.classList.add("contenido"); // Clase para estilos
        item.innerHTML = `
            <p class="numero-estadistica">Prendas vendidas: ${ventas.totales}+</p>
            <p class="cambio-estadistica aumento"></p>
            <div class="mini-grafica">
            <img src="Recursos/imagenes/iconos-admin/grafico.png" alt="grafica">
            </div>
        `;
        }else{
            item.classList.add("contenido"); // Clase para estilos
             item.innerHTML = `
            <p class="numero-estadistica">${ventas.totales}+</p>
            <p class="cambio-estadistica aumento"></p>
            <div class="mini-grafica">
            <img src="Recursos/imagenes/iconos-admin/image.png" alt="grafica">
            </div>
        `;
        }
        contenedor.appendChild(item);
    });
}

// Funcion para mostrar el encabezado de acuerdo a los meses de nuestra base de datos
function mostrarVentasPorMes(lista) {
    const contenedor = document.getElementById("contenedor-encabezado");
    const contenedor1 = document.getElementById("contenedor-estadistica1");

    contenedor.innerHTML = "";
    contenedor1.innerHTML = "";

    lista.forEach(ventasDelMes => {
        // Encabezado
        const encabezado = document.createElement("div");
        encabezado.classList.add("encabezado-estadistica");
        encabezado.innerHTML = `
            <h3>Ingresos de ${ventasDelMes.mes} del ${ventasDelMes.año}</h3>
            <span class="icono-estadistica"></span>
        `;
        contenedor.appendChild(encabezado);

        // Contenido
        const contenido = document.createElement("div");
        contenido.classList.add("contenido");
        contenido.innerHTML = `
            <p class="numero-estadistica">$${ventasDelMes.ingresosTotales.toFixed(2)}</p>
            <div class="mini-grafica">
                <img src="Recursos/imagenes/iconos-admin/informe-grafico.png" alt="grafica"/>
            </div>
        `;
        contenedor1.appendChild(contenido);
    });
}

// Esta función muestra los productos en el HTML mediante un innerHTML - Ventas del dia
function mostrarRegistrosUsuarios(lista) {
    const contenedor = document.getElementById("contenedor-estadistica2");
    contenedor.innerHTML = ""; // Limpiamos antes de mostrar

    lista.forEach(usuarios => {
        const item = document.createElement("div"); // Creamos el div
        item.classList.add("contenido1");
        item.innerHTML = `
             <p class="numero-estadistica">${usuarios.totales}</p>
             <p class="cambio-estadistica aumento">+</p>
             <div class="mini-grafica">
             <img src="Recursos/imagenes/iconos-admin/User.png" alt="grafica">
             </div>
        `;
        contenedor.appendChild(item);
    });
}


//Funcion que carga las ventas del dia
async function cargarVentasTotalesDelDia() {
    try {
        const ventasDelDia = await VentasDelDia();
        if (ventasDelDia && ventasDelDia.length > 0) {
            mostrarVentas(ventasDelDia);
        } else {
            console.log("No se encontraron ventas para el día de hoy.");
        }
    } catch (error) {
        console.error("Error al cargar las ventas totales:", error);
    }
}

//Funcion que cargan las ventas del mes
async function cargarVentasTotalesDelMes() {
    try {
        const ventasDelMes = await IngresosMesActual();
        if (ventasDelMes && ventasDelMes.length > 0) {
            mostrarVentasPorMes(ventasDelMes);
        } else {
            console.log("No se encontraron ventas en este mes.");
        }
    } catch (error) {
        console.error("Error al cargar las ventas totales:", error);
    }
}

//Funcion que carga los usuarios registrados
async function cargarUsuariosRegistrados() {
    try {
        const registrosUsuarios = await TotalUsuariosRegistrados();
        if (registrosUsuarios && registrosUsuarios.length > 0) {
            mostrarRegistrosUsuarios(registrosUsuarios);
        } else {
            console.log("No se encontraron usuarios registrados.");
        }
    } catch (error) {
        console.error("Error al cargar los registros de usuarios:", error);
    }
}


//FUNCIONES DE LAS TABLAS

function TablaSeleccionada(selectedCategory) {
	if (selectedCategory === 'ventas') {
    cargarTablaVentas();
	}else if (selectedCategory==='usuarios'){
    cargarTablaUsuarios();
    }else if (selectedCategory==='productos'){
     cargarTablaProductos();
    }
}


// MOSTRAR TABLA VENTAS
function mostrarTablaVentas(lista) {
    const contenedor = document.getElementById("contenido-tablas");
    contenedor.innerHTML = "";
    const divTabla = document.createElement("div");
    divTabla.classList.add("contenido-tabla");
    const tabla = document.createElement("table");
    tabla.innerHTML = `<caption>VENTAS</caption>
      <thead><tr><th>ID Venta</th><th>Fecha</th><th>Cliente</th><th>Total</th><th>Método de pago</th><th>Estado</th><th>Actualizar</th></tr></thead><tbody></tbody>`;
    const tbody = tabla.querySelector("tbody");
    lista.forEach(venta => {
      const fila = document.createElement("tr");
      fila.innerHTML = `<td>${venta.idVenta}</td><td>${venta.fecha ? new Date(venta.fecha).toLocaleString() : ""}</td>
        <td>${venta.cliente || venta.nombreCliente || ""}</td><td>$${Number(venta.total || 0).toFixed(2)}</td>
        <td>${venta.metodoPago || "No indicado"}</td><td>${venta.estado || ""}</td><td><select class="admin-estado-venta" data-id="${venta.idVenta}">
        ${["Pendiente","Pagada","Preparando","Enviada","Entregada","Cancelada"].map(e => `<option value="${e}" ${e === venta.estado ? "selected" : ""}>${e}</option>`).join("")}
        </select><button class="admin-guardar-venta" data-id="${venta.idVenta}">Guardar</button>
        <button class="admin-eliminar-venta" data-id="${venta.idVenta}">Eliminar</button></td>`;
      tbody.appendChild(fila);
    });
    divTabla.appendChild(tabla);
    contenedor.appendChild(divTabla);
    tbody.addEventListener("click", async e => {
      const id = Number(e.target.dataset.id);
      if (e.target.classList.contains("admin-eliminar-venta")) {
        if (!confirm("¿Deseas cancelar/eliminar esta venta? Se recomienda conservar el historial y marcarla como Cancelada.")) return;
        try { await EliminarVenta(id); await cargarTablaVentas(); }
        catch (err) { alert("No se pudo eliminar/cancelar la venta: " + err.message); }
        return;
      }
      if (!e.target.classList.contains("admin-guardar-venta")) return;
      const estado = tbody.querySelector(`.admin-estado-venta[data-id="${id}"]`)?.value;
      try { await ActualizarEstadoVenta(id, estado); alert("Estado de venta actualizado."); }
      catch (err) { alert("No se pudo actualizar la venta: " + err.message); }
    });
}

// MOSTRAR TABLA USUARIO
function mostrarTablaUsuarios(lista) {
    const contenedor = document.getElementById("contenido-tablas");
    contenedor.innerHTML = ""; // Limpiar contenido anterior

    // Crear contenedor para la tabla (tarjeta)
    const divTabla = document.createElement("div");
    divTabla.classList.add("contenido-tabla"); // aquí se aplica el CSS

    // Crear tabla
    const tabla = document.createElement("table");
    tabla.innerHTML = `
        <caption>USUARIOS</caption>
        <thead>
            <tr>
                <th>Nombre completo</th><th>Email</th><th>Telefono</th><th>Rol</th><th>Acciones</th>
            </tr>
        </thead>
        <tbody></tbody>
    `;

    const tbody = tabla.querySelector("tbody");

    // Agregar filas dinámicamente
    lista.forEach(usuario => {
        const fila = document.createElement("tr");
        fila.innerHTML = `
            <td>${usuario.nombreApellido}</td>
            <td>${usuario.email}</td>
            <td>${usuario.telefono}</td>
            <td>${usuario.rol}</td>
            <td><button type="button" class="admin-desactivar-usuario" data-id="${usuario.idUsuario}">Desactivar</button></td>
        `;
        tbody.appendChild(fila);
    });

    // Añadir la tabla dentro del div
    divTabla.appendChild(tabla);

    // Añadir el div al contenedor principal
    contenedor.appendChild(divTabla);
    tbody.addEventListener("click", async e => {
        if (!e.target.classList.contains("admin-desactivar-usuario")) return;
        const id = Number(e.target.dataset.id);
        if (!confirm("¿Desactivar este usuario? Se conservarán sus compras y facturas.")) return;
        try { await DesactivarUsuario(id); await cargarTablaUsuarios(); }
        catch (err) { alert("No se pudo desactivar el usuario: " + err.message); }
    });
}


// MOSTRAR TABLA DE PRODUCTOS

// MOSTRAR TABLA DE PRODUCTOS
function mostrarTablaProductos(lista) {
    const contenedor = document.getElementById("contenido-tablas");
    contenedor.innerHTML = "";

    const divTabla = document.createElement("div");
    divTabla.classList.add("contenido-tabla");

    const tabla = document.createElement("table");

    tabla.innerHTML = `
        <caption>PRODUCTOS</caption>
        <thead>
            <tr>
                <th>ID</th>
                <th>Nombre producto</th>
                <th>Género</th>
                <th>Categoría</th>
                <th>Precio</th>
                <th>Imagen</th>
                <th>Estado</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody></tbody>
    `;

    const tbody = tabla.querySelector("tbody");

    if (lista.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8">No hay productos registrados.</td>
            </tr>
        `;
    }

    lista.forEach(producto => {
        const fila = document.createElement("tr");

        const activo = producto.activo === true;
        const precio = Number(producto.precio ?? 0);

        fila.innerHTML = `
            <td>${producto.idProducto}</td>
            <td>${producto.nombreProducto ?? ""}</td>
            <td>${producto.genero ?? ""}</td>
            <td>${producto.categoria ?? ""}</td>
            <td>$${precio.toFixed(2)}</td>

            <td>
                <div class="imagen-producto1"
                    style="background-image: url('${producto.enlaceIMG ?? ""}')">
                </div>
            </td>

            <td>
                <span class="${activo ? "estado-activo" : "estado-inactivo"}">
                    ${activo ? "Activo" : "Inactivo"}
                </span>
            </td>

            <td>
                <button type="button"
                    class="admin-editar-producto"
                    data-id="${producto.idProducto}">
                    Editar
                </button>

                <button type="button"
                    class="admin-cambiar-estado"
                    data-id="${producto.idProducto}"
                    data-activo="${activo}">
                    ${activo ? "Desactivar" : "Activar"}
                </button>
            </td>
        `;

        tbody.appendChild(fila);
    });

    divTabla.appendChild(tabla);
    contenedor.appendChild(divTabla);

    // EVENTOS DE LOS BOTONES
    tbody.addEventListener("click", async (e) => {
        const boton = e.target.closest("button");

        if (!boton) return;

        const id = Number(boton.dataset.id);

        if (!id) return;

        // ACTIVAR / DESACTIVAR PRODUCTO
        if (boton.classList.contains("admin-cambiar-estado")) {
            const activo = boton.dataset.activo === "true";
            const nuevoEstado = !activo;

            const mensaje = nuevoEstado
                ? "¿Deseas volver a activar este producto?"
                : "¿Deseas desactivar este producto? Dejará de aparecer en la tienda.";

            if (!confirm(mensaje)) return;

            try {
                await CambiarEstadoProducto(id, nuevoEstado);

                alert(nuevoEstado
                    ? "Producto activado correctamente."
                    : "Producto desactivado correctamente.");

                await cargarTablaProductos();

            } catch (err) {
                alert("No se pudo cambiar el estado del producto: " + err.message);
            }

            return;
        }

        // EDITAR PRODUCTO
        if (boton.classList.contains("admin-editar-producto")) {
            const fila = boton.closest("tr");
            const celdas = fila.querySelectorAll("td");

            const nombreProducto = prompt(
                "Nombre del producto:",
                celdas[1].textContent
            );

            if (nombreProducto === null) return;

            const genero = prompt(
                "Género:",
                celdas[2].textContent
            );

            if (genero === null) return;

            const categoria = prompt(
                "Categoría:",
                celdas[3].textContent
            );

            if (categoria === null) return;

            const precioOriginalTexto = prompt(
                "Precio normal:",
                celdas[4].textContent.replace(/[^0-9.]/g, "")
            );

            if (precioOriginalTexto === null) return;

            const precioOriginal = Number(precioOriginalTexto);

            const precioOfertaTexto = prompt(
                "Precio de oferta (escribe el mismo precio si no tendrá oferta):",
                precioOriginal
            );

            if (precioOfertaTexto === null) return;

            const precioOferta = Number(precioOfertaTexto);

            // Recuperar el enlace de la imagen
            const imagen = celdas[5].querySelector(".imagen-producto1");
            const imagenActual = imagen.style.backgroundImage
                .replace(/^url\(["']?/, "")
                .replace(/["']?\)$/, "");

            const enlaceIMG = prompt(
                "Enlace de la imagen:",
                imagenActual
            );

            if (enlaceIMG === null) return;

            // VALIDAR DATOS
            if (
                !nombreProducto.trim() ||
                !genero.trim() ||
                !categoria.trim() ||
                !Number.isFinite(precioOriginal) ||
                precioOriginal < 0 ||
                !Number.isFinite(precioOferta) ||
                precioOferta < 0 ||
                precioOferta > precioOriginal ||
                !enlaceIMG.trim()
            ) {
                alert("Datos inválidos. Verifica los campos y los precios.");
                return;
            }

            try {
                await ActualizarProducto(id, {
                    nombreProducto: nombreProducto.trim(),
                    genero: genero.trim(),
                    categoria: categoria.trim(),
                    precio: precioOriginal,
                    precioOriginal: precioOriginal,
                    precioOferta: precioOferta,
                    enlaceIMG: enlaceIMG.trim()
                });

                alert("Producto actualizado correctamente.");

                await cargarTablaProductos();

            } catch (err) {
                alert("No se pudo actualizar el producto: " + err.message);
            }
        }
    });
}


//Funcion que carga todas las ventas
async function cargarTablaVentas() {
    try {
        const ventasTotales = await VentasDetalladas();
        if (ventasTotales && ventasTotales.length > 0) {
            mostrarTablaVentas(ventasTotales)
        } else {
            console.log("No se encontraron los datos de la tabla de ventas.");
        }
    } catch (error) {
        console.error("Error al cargar:", error);
    }
}


//Funcion que carga todos los usuarios
async function cargarTablaUsuarios() {
    try {
        const usuariosTot = await UsuariosAdmin();
        if (usuariosTot && usuariosTot.length > 0) {
            mostrarTablaUsuarios(usuariosTot)
        } else {
            console.log("No se encontraron los datos de la tabla de usuarios.");
        }
    } catch (error) {
        console.error("Error al cargar:", error);
    }
}


//Funcion que carga todos los productos

async function cargarTablaProductos() {
    try {
        const productosTot = await ProductosTotales();

        if (Array.isArray(productosTot)) {
            mostrarTablaProductos(productosTot);
        } else {
            console.error("La respuesta de ProductosTotales no es una lista.");
        }

    } catch (error) {
        console.error("Error al cargar los productos:", error);
        alert("No se pudieron cargar los productos: " + error.message);
    }
}
