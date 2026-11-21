// Esta función muestra los productos en el HTML mediante un innerHTML.
function mostrarProductos(lista) {
    const contenedor = document.getElementById("contenedor-productos");
    contenedor.innerHTML = "";

    lista.forEach(producto => {
        const item = document.createElement("div");
        item.classList.add("producto");
        
        // Agregar data attributes para género y categoría
        item.dataset.id = producto.idProducto || '';
        item.dataset.genero = producto.genero || localStorage.getItem("genero") || '';
        item.dataset.categoria = producto.categoria || '';
        
        item.innerHTML = `
            <div class="imagen-producto" style="background-image: url('${producto.enlaceIMG}')">
                <button class="btn-agregar btn-principales">+</button>
            </div>
            <div class="detalles-producto">
                <p class="nombre-producto">${producto.nombreProducto}</p>
                <p class="precio-producto">$${producto.precio.toFixed(2)}</p>
            </div>
        `;
        contenedor.appendChild(item);
    });
}


// Esta función se encarga de cargar los divs de cada producto sin modificar el header ni el footer, utilizando el contenedor 'contenedor-productos'.
function cargarMain(archivo) {
  fetch(archivo)
    .then(response => response.text())
    .then(data => {
      const contenedor = document.getElementById("contenedor-productos");
      contenedor.innerHTML = data;
    })
    .catch(error => console.error("Error al cargar los productos:", error));
}

// Aquí recuperamos del local storage el valor del género y lo concatenamos al momento de enviar el archivo a cargar a la función cargarMain.
const generoGuardado = localStorage.getItem("genero") || "mujer";


// Estos son los botones de los géneros, donde se obtiene el texto para identificar si el usuario seleccionó 'hombre' o 'mujer'.
document.querySelectorAll(".btn-principales[data-group='genero']").forEach(boton => {
    boton.addEventListener("click", () => {

        document.querySelectorAll(".btn-principales[data-group='genero']").forEach(b => b.classList.remove("activo"));
        boton.classList.add("activo");

        const genero = boton.textContent.toLowerCase();

        if (genero === "hombre") {
            window.location.href = "products-hombre.html";
        } else {
            window.location.href = "products-mujer.html";
        }

        localStorage.setItem("genero", genero);

        cargarProductosPorGenero(genero);
    });
});


// Aquí obtenemos la categoría seleccionada desde los inputs de tipo radio.
document.querySelectorAll('.contenedor-categoria input[type="radio"]').forEach(checkbox => {
    checkbox.addEventListener('change', () => {
        const genero = localStorage.getItem("genero");
        const categoria = checkbox.nextElementSibling.textContent.trim().toLowerCase().replace(/\s+/g, " ");

        localStorage.removeItem("categoria");

        cargarProductosPorGeneroYCategoria(genero, categoria);
    });
});


// Este evento cierra el carrito cuando se hace clic en el overlay.
document.getElementById("overlay").addEventListener("click", () => {
  document.getElementById("checkbox-carrito").checked = false;
});

// Esta función desmarca los filtros seleccionados y recarga los productos según el género actual.
function limpiarFiltros() {
    const genero = localStorage.getItem("genero");
    cargarProductosPorGenero(genero);

    document.querySelectorAll(' input[type="radio"]').forEach(input => {
        input.checked = false;
    });
}



// Este evento se ejecuta cuando el documento ha cargado y verifica si el usuario tiene un token para determinar si debe iniciar sesión o ir al perfil.
document.addEventListener("DOMContentLoaded", () => {
    localStorage.removeItem("categoria");
    const token = localStorage.getItem("tokenseguridad");

    document.getElementById("btnIrUsuario").addEventListener("click", () => {
        const token = localStorage.getItem("tokenseguridad");

        if (token) {
            window.location.href = "editarPerfil.html";
        } else {
            window.location.href = "login.html";
        }
    });

    // Aquí se asigna el botón activo según el género guardado en el local storage.
    document.querySelectorAll(".btn-principales[data-group='genero']").forEach(boton => {
        boton.classList.remove("activo");
        if (boton.textContent.toLowerCase() === generoGuardado) {
            boton.classList.add("activo");
        }
    });

    // Aplicar únicamente la categoría temporal recibida por URL.
    const params = new URLSearchParams(window.location.search);
    const categoriaInicial = params.get("categoria");
    if (categoriaInicial) {
        document.querySelectorAll('.contenedor-categoria input[type="radio"]').forEach(input => {
            const texto = (input.nextElementSibling?.textContent || "").trim().toLowerCase().replace(/\s+/g, " ");
            input.checked = texto === categoriaInicial.toLowerCase();
        });
        cargarProductosPorGeneroYCategoria(generoGuardado, categoriaInicial);
        // Al refrescar o volver a entrar al catálogo no se conserva el filtro anterior.
        history.replaceState({}, document.title, window.location.pathname);
    } else {
        localStorage.removeItem("categoria");
        cargarProductosPorGenero(generoGuardado);
    }
});


// Esta función se encarga de cargar los productos según el género seleccionado.
function cargarProductosPorGenero(genero) {
    ProductosPorGenero(genero).then(productos => {
        mostrarProductos(productos);
    });
}

// Esta función obtiene los productos del género seleccionado y también su categoría.
function cargarProductosPorGeneroYCategoria(genero, categoria) {
    ProductosPorGeneroCategoria(genero, categoria).then(productos => {
        mostrarProductos(productos);
    });
}


// Este botón permite visualizar todos los productos dependiendo del género seleccionado.
document.getElementById("todos-productos").addEventListener("click", () => {
    const genero = localStorage.getItem("genero") || "mujer";

    if (genero === "hombre") {
        window.location.href = "products-hombre.html";
    } else {
        window.location.href = "products-mujer.html";
    }
});

// Este botón redirige a la página de ofertas según el género del usuario.
document.getElementById("ofertas").addEventListener("click", () => {
    const genero = localStorage.getItem("genero") || "mujer";

    if (genero === "hombre") {
        window.location.href = "OfertasHombre.html";
    } else {
        window.location.href = "OfertasMujer.html";
    }
});

// Este botón redirige a la página de los productos más vendidos según el género seleccionado.
document.getElementById("mas-vendidos").addEventListener("click", () => {
    const genero = localStorage.getItem("genero");

    if (genero === "hombre") {
        window.location.href = "MasVendidosHombre.html";
    } else {
        window.location.href = "MasVendidosMujer.html";
    }
});
