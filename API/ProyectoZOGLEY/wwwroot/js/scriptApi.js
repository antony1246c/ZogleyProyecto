

const API_URL = "/api";

// JWT: la validación del navegador mejora la experiencia, pero la API SIEMPRE
// debe validar firma, expiración y rol en cada endpoint protegido.
function obtenerPayloadToken(token = localStorage.getItem("tokenseguridad")) {
    try {
        if (!token) return null;
        const parte = token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/");
        return JSON.parse(decodeURIComponent(escape(atob(parte))));
    } catch { return null; }
}
function sesionValida() {
    const payload = obtenerPayloadToken();
    if (!payload) return false;
    const exp = Number(payload.exp || payload.Expiration || 0);
    return !exp || exp * 1000 > Date.now();
}
function cerrarSesion() {
    ["tokenseguridad", "rol", "usuarioZogley", "carritoZogley"].forEach(k => localStorage.removeItem(k));
    sessionStorage.clear();
    window.location.replace("login.html");
}
function exigirSesion(destino = "login.html") {
    if (!sesionValida()) {
        ["tokenseguridad", "rol", "usuarioZogley"].forEach(k => localStorage.removeItem(k));
        window.location.replace(destino);
        return false;
    }
    return true;
}
async function apiFetch(ruta, opciones = {}) {
    const token = localStorage.getItem("tokenseguridad");
    const headers = { "Content-Type": "application/json", ...(opciones.headers || {}) };
    if (token) headers.Authorization = `Bearer ${token}`;
    const respuesta = await fetch(`${API_URL}${ruta}`, { ...opciones, headers, cache: "no-store" });
    if (respuesta.status === 401) cerrarSesion();
    if (!respuesta.ok) {
        const detalle = await respuesta.text();
        throw new Error(detalle || `Error API ${respuesta.status}`);
    }
    if (respuesta.status === 204) return null;
    return respuesta.json();
}



// INICIAR SESION
async function iniciarsesion() {
    const datos = {
        email: document.getElementById("Useremail").value,
        contrasenia: document.getElementById("password").value  
    };

    try {
        const response = await fetch(`${API_URL}/UserInfo/Login`, {  
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(datos),
        });

        const resultado = await response.json();

        if (!response.ok) {
            return { exito: false, resultado };
        }

        // Guardar token y rol en localStorage
        localStorage.setItem("tokenseguridad", resultado.token);
        localStorage.setItem("rol", resultado.rol || resultado.usuario?.rol || "");
        if (resultado.usuario) localStorage.setItem("usuarioZogley", JSON.stringify(resultado.usuario));

        return { exito: true, resultado };

    } catch (error) {
        console.error('Error al iniciar sesión:', error);
        throw error;
    }
}


// REGISTRAR USUARIO
async function RegistrarUsuario() {

    console.log("Inicio del registro...");

    const datos = {
        NombreApellido: document.getElementById("Username").value,
        Email: document.getElementById("Useremail").value,
        Telefono: document.getElementById("phone").value,
        Contrasenia:document.getElementById('password').value,
    };

    console.log("Datos enviados a la API:", datos);
    try {
        const response = await fetch(`${API_URL}/UserInfo/RegistrarUsuario`, {  
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(datos),
        });

        const resultado = await response.json();

        if (!response.ok) {
            return { exito: false, resultado };
        }

        return { exito: true, resultado };
    } catch (error) {
        console.error('Error al registrar usuario:', error);
        throw error;
    }
}

// CONSULTA TODOS LOS PRODUCTOS NUEVOS POR GENERO
async function ProductosNuevosPorGenero() {
    let genero = localStorage.getItem("genero");
    console.log("➡ Género que se enviará a la API:", genero);  
    if (!genero) {
        console.error("No hay género seleccionado en localStorage");
        return [];
    }

    try {
        const response = await fetch(`${API_URL}/ProductoRopa/ProductosNuevosPorGenero?genero=${genero}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            
        });
        console.log("➡ URL enviada:", response);              
        console.log('Se han recibido los productos:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const productos = await response.json();
        console.log('Productos obtenidos:', productos);
        return productos;

    } catch (error) {
        console.error('Error en la consulta de productos nuevos:', error);
        throw error;
    }
}

// CONSULTA TODOS LOS PRODUCTOS POR GENERO
async function ProductosPorGenero() {
    let genero = localStorage.getItem("genero");
    console.log("➡ Género que se enviará a la API:", genero);  
    if (!genero) {
        console.error("No hay género seleccionado en localStorage");
        return [];
    }

    try {
        const response = await fetch(`${API_URL}/ProductoRopa/GetProductosRopaGenero?genero=${genero}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            
        });
        console.log("➡ URL enviada:", response);              
        console.log('Se han recibido los productos:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const productos = await response.json();
        console.log('Productos obtenidos:', productos);
        return productos;

    } catch (error) {
        console.error('Error en la consulta de productos nuevos:', error);
        throw error;
    }
}


// CONSULTA TODOS LOS PRODUCTOS POR GENERO Y CATEGORIA
async function ProductosPorGeneroCategoria(genero, categoria) {
    if (!genero || !categoria) {
        console.error("No hay género o categoría seleccionados");
        return [];
    }

    try {
        const url = `${API_URL}/ProductoRopa/GetProductosRopaCategoriaYgenero?genero=${encodeURIComponent(genero)}&categoria=${encodeURIComponent(categoria)}`;
        const response = await fetch(url, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
            cache: 'no-store'
        });
        if (!response.ok) throw new Error(`Error en la solicitud: ${response.status}`);
        return await response.json();
    } catch (error) {
        console.error('Error al consultar productos por categoría:', error);
        throw error;
    }
}

// PRODUCTOS MAS BARATOS - EN OFERTA
async function ProductosEconomicos() {
    let genero = localStorage.getItem("genero");
    console.log("➡ Género que se enviará a la API:", genero);
    try {
        const response = await fetch(`${API_URL}/ProductoRopa/ListaMasBaratosGenero?genero=${genero}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            
        });
        console.log("➡ URL enviada:", response);              
        console.log('Se han recibido los productos:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const productos = await response.json();
        console.log('Productos obtenidos:', productos);
        return productos;

    } catch (error) {
        console.error('Error en la consulta de productos nuevos:', error);
        throw error;
    }
}


// PRODUCTOS MAS COMPRADOS
async function ProductosMasComprados() {
    let genero = localStorage.getItem("genero");
    console.log("➡ Género que se enviará a la API:", genero);
    try {
        const response = await fetch(`${API_URL}/ProductoRopa/ListaMasCompradosGenero?genero=${genero}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            
        });
        console.log("➡ URL enviada:", response);              
        console.log('Se han recibido los productos:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const productos = await response.json();
        console.log('Productos obtenidos:', productos);
        return productos;

    } catch (error) {
        console.error('Error en la consulta de productos nuevos:', error);
        throw error;
    }
}


// REGISTRAR COMPRA (HAY QUE CAMBIAR)
// REGISTRAR COMPRA
async function RegistrarCompra(datosCompra) {
    console.log("Inicio del registro de compra...");
    console.log("Datos enviados a la API:", datosCompra);

    const token = localStorage.getItem("tokenseguridad"); // Token guardado

    try {
        const response = await fetch(`${API_URL}/ComprasUsuario/RegistrarCompra`, {  
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Enviar token
            },
            body: JSON.stringify(datosCompra),
        });

        const resultado = await response.json();

        if (!response.ok) {
            return { exito: false, resultado };
        }

        return { exito: true, resultado };
    } catch (error) {
        console.error('Error al registrar compra:', error);
        throw error;
    }
}






// CONSULTAS DEL ADMINISTRADOR
// VENTAS DEL DIA
async function VentasDelDia() {
    const token = localStorage.getItem("tokenseguridad"); // Token guardado

    try {
        const response = await fetch(`${API_URL}/Administrador/GetVentasDelDia`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Enviamos el token
            },
        });
        console.log("➡ URL enviada:", response.url);              
        console.log('Estado de la respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const ventasDelDia = await response.json();
        console.log('Total de ventas por dia:', ventasDelDia);
        return ventasDelDia;

    } catch (error) {
        console.error('Error en la consulta las ventas del dia:', error);
        throw error;
    }
}


// INGRESOS DEL MES
async function IngresosMesActual() {
    const token = localStorage.getItem("tokenseguridad"); // Token guardado

    try {
        const response = await fetch(`${API_URL}/Administrador/GetIngresosDelMes`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Enviamos el token
            },
        });
        console.log("➡ URL enviada:", response.url);              
        console.log('Estado de la respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const ventasDelDia = await response.json();
        console.log('Total de ingresos por mes:', ventasDelDia);
        return ventasDelDia;

    } catch (error) {
        console.error('Error en la consulta las ventas del mes:', error);
        throw error;
    }
}


// USUARIOS REGISTRADOS TOTALES
async function TotalUsuariosRegistrados() {
    const token = localStorage.getItem("tokenseguridad"); // Token guardado

    try {
        const response = await fetch(`${API_URL}/Administrador/GetTodosLosRegistros`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Enviamos el token
            },
        });
        console.log("➡ URL enviada:", response.url);              
        console.log('Estado de la respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const registros = await response.json();
        console.log('Total de usuarios:', registros);
        return registros;

    } catch (error) {
        console.error('Error en la consulta de los usuarios:', error);
        throw error;
    }
}


// PRODUCTOS TOTALES
async function ProductosTotales() {
    const token = localStorage.getItem("tokenseguridad"); // Token guardado

    try {
        const response = await fetch(`${API_URL}/Administrador/ProductosTotales`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Enviamos el token
            },
        });

        console.log("➡ URL enviada:", response.url);              
        console.log('Estado de la respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const productos = await response.json();
        console.log('Productos obtenidos:', productos);
        return productos;

    } catch (error) {
        console.error('Error en la consulta de productos totales:', error);
        throw error;
    }
}

// VENTAS TOTALES
async function VentasTotales() {
    const token = localStorage.getItem("tokenseguridad"); // Token guardado
     try {
        const response = await fetch(`${API_URL}/Administrador/VentasAgrupadasTotales`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Enviamos el token
            },
        });

        console.log("➡ URL enviada:", response.url);              
        console.log('Estado de la respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const ventas = await response.json();
        console.log('Ventas obtenidos:', ventas);
        return ventas;

    } catch (error) {
        console.error('Error en la consulta de ventas totales:', error);
        throw error;
    }
}

// USUARIOS TOTALES
async function UsuariosTotales() {
    const token = localStorage.getItem("tokenseguridad"); // Token guardado
     try {
        const response = await fetch(`${API_URL}/Administrador/UsuariosTotales`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Enviamos el token
            },
        });

        console.log("➡ URL enviada:", response.url);              
        console.log('Estado de la respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const usuarios = await response.json();
        console.log('Usuarios obtenidos:', usuarios);
        return usuarios;

    } catch (error) {
        console.error('Error en la consulta de usuarios totales:', error);
        throw error;
    }
}

// Validación real del JWT en el servidor antes de aceptar un artículo en el carrito.
async function ValidarSesionCliente() {
    if (!sesionValida()) throw new Error("Debes iniciar sesión para comprar.");
    return apiFetch("/UserInfo/ValidarSesionCliente");
}

// PERFIL DEL CLIENTE AUTENTICADO.
async function ObtenerMiPerfil() {
    if (!sesionValida()) throw new Error("La sesión expiró. Inicia sesión nuevamente.");
    return apiFetch("/UserInfo/Perfil");
}


// ACTUALIZAR PERFIL DEL USUARIO AUTENTICADO
async function ActualizarMiPerfil(datos) {
    if (!sesionValida()) {
        throw new Error("La sesión expiró. Inicia sesión nuevamente.");
    }

    return apiFetch("/UserInfo/ActualizarPerfil", {
        method: "PUT",
        body: JSON.stringify(datos)
    });
}


// CREAR FACTURA: el servidor obtiene el cliente desde los claims del JWT.
// No se debe confiar en un UsuarioId enviado desde el navegador.
async function CrearFactura(datos) {
    if (!sesionValida()) throw new Error("Debes iniciar sesión para comprar.");
    return apiFetch("/Facturas/Crear", { method: "POST", body: JSON.stringify(datos) });
}

// OPERACIONES ADMINISTRATIVAS. La API debe restringirlas al rol Admin.
async function ActualizarProducto(id, datos) {
    return apiFetch(`/Administrador/Productos/${id}`, {
        method: "PUT",
        body: JSON.stringify(datos)
    });
}

async function EliminarProducto(id) {
    return apiFetch(`/Administrador/Productos/${id}`, { method: "DELETE" });
}
async function ActualizarEstadoVenta(id, estado) {
    return apiFetch(`/Administrador/Ventas/${id}/estado`, { method: "PATCH", body: JSON.stringify({ estado }) });
}

async function VentasDetalladas() {
    return apiFetch("/Administrador/Ventas");
}


async function CambiarEstadoProducto(id, activo) {
    return apiFetch(`/Administrador/Productos/${id}/Estado`, {
        method: "PUT",
        body: JSON.stringify({ activo: activo })
    });
}

async function EliminarVenta(id) { return apiFetch(`/Administrador/Ventas/${id}`, { method: "DELETE" }); }

async function UsuariosAdmin() {return await apiFetch("/Administrador/UsuariosTotales");}
async function DesactivarUsuario(id) { return apiFetch(`/Administrador/Usuarios/${id}`, { method: "DELETE" }); }
async function ActivarUsuario(id) { return apiFetch(`/Administrador/Usuarios/${id}/activar`, { method: "PATCH" }); }
