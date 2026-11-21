
document.addEventListener("DOMContentLoaded", async function () {

    // ==========================================
    // VALIDAR SESION
    // ==========================================

    if (typeof exigirSesion !== "function" || !exigirSesion()) {
        return;
    }

    const formulario = document.getElementById("form-editar-perfil");

    const botonGuardar = formulario.querySelector(
        'button[type="submit"]'
    );

    // Campos del formulario
    const nombre = document.getElementById("nombre");
    const email = document.getElementById("email");
    const telefono = document.getElementById("telefono");
    const direccion = document.getElementById("direccion");
    const distrito = document.getElementById("distrito");
    const provincia = document.getElementById("provincia");

    // ==========================================
    // CARGAR PERFIL DESDE LA API
    // ==========================================

    async function cargarPerfil() {

        try {

            const respuesta = await ObtenerMiPerfil();

            const usuario = respuesta.usuario || respuesta;

            // Cargar datos personales
            nombre.value = usuario.nombreApellido || "";
            email.value = usuario.email || "";
            telefono.value = usuario.telefono || "";

            // Cargar dirección dividida
            direccion.value = usuario.direccion || "";
            distrito.value = usuario.distrito || "";
            provincia.value = usuario.provincia || "";

            // Actualizar encabezado
            document.getElementById("nombre-usuario").textContent =
                usuario.nombreApellido || "Usuario";

            document.getElementById("email-usuario").textContent =
                usuario.email || "email@ejemplo.com";

            // Actualizar información local
            localStorage.setItem(
                "usuarioZogley",
                JSON.stringify(usuario)
            );

        } catch (error) {

            console.error("Error al cargar el perfil:", error);

            alert(
                "No se pudo cargar tu perfil. " +
                "Intenta nuevamente."
            );
        }
    }

    // ==========================================
    // ACTUALIZAR PERFIL
    // ==========================================

    formulario.addEventListener("submit", async function (e) {

        e.preventDefault();

        // Obtener datos del formulario
        const datos = {

            nombreApellido: nombre.value.trim(),
            email: email.value.trim(),
            telefono: telefono.value.trim(),

            // Dirección separada por columnas
            direccion: direccion.value.trim(),
            distrito: distrito.value.trim(),
            provincia: provincia.value.trim()
        };

        // Validar campos obligatorios
        if (!datos.nombreApellido || !datos.email) {

            alert(
                "El nombre y el correo electrónico son obligatorios."
            );

            return;
        }

        // Validar formato del correo
        if (!email.checkValidity()) {

            alert("Introduce un correo electrónico válido.");

            email.focus();

            return;
        }

        try {

            // Deshabilitar botón mientras se guarda
            botonGuardar.disabled = true;
            botonGuardar.textContent = "Guardando...";

            // Enviar información al backend
            const respuesta = await ActualizarMiPerfil(datos);

            const usuario = respuesta.usuario || respuesta;

            // Actualizar localStorage
            localStorage.setItem(
                "usuarioZogley",
                JSON.stringify(usuario)
            );

            // Actualizar encabezado
            document.getElementById("nombre-usuario").textContent =
                usuario.nombreApellido || "Usuario";

            document.getElementById("email-usuario").textContent =
                usuario.email || "email@ejemplo.com";

            alert(
                respuesta.mensaje ||
                "Perfil actualizado correctamente."
            );

        } catch (error) {

            console.error("Error al actualizar el perfil:", error);

            let mensaje = error.message;

            // Intentar interpretar el mensaje enviado por la API
            try {

                const detalle = JSON.parse(error.message);

                mensaje = detalle.mensaje ||
                    detalle.title ||
                    mensaje;

            } catch { }

            alert(
                "No se pudieron guardar los cambios: " + mensaje
            );

        } finally {

            // Restaurar botón
            botonGuardar.disabled = false;
            botonGuardar.textContent = "Guardar Cambios";
        }
    });

    // Cargar información al abrir la página
    await cargarPerfil();

});
