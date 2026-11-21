// Esta función muestra los productos en el HTML utilizando innerHTML para generar la estructura.
function mostrarProductos(lista) {
    const contenedor = document.getElementById("carrusel-tendencias");
    contenedor.innerHTML = "";

    lista.forEach(producto => {
        const item = document.createElement("div");
        item.classList.add("producto");
        item.dataset.id = producto.idProducto || '';
        item.dataset.genero = producto.genero || localStorage.getItem("genero") || '';
        item.dataset.categoria = producto.categoria || '';
        item.innerHTML = `
            <div class="imagen-producto"  style="background-image: url('${producto.enlaceIMG}')">
                <button class="btn-agregar btn-principales">+</button>
            </div>
            <div class="detalles-producto">
                <p class="nombre-producto" style="color: black;">${producto.nombreProducto}</p>
                <p class="precio-producto" style="color: black;">$${producto.precio.toFixed(2)}</p>
            </div>
        `;
        contenedor.appendChild(item);
    });
}


// Recuperamos el género guardado en localStorage y decidimos qué main cargar al inicio.
let generoGuardado = localStorage.getItem("genero");
if (!generoGuardado) {
    generoGuardado = "mujer";
    localStorage.setItem("genero", "mujer");
    cargarMain("main-mujer.html");
    cargarProductosPorGenero("mujer");
} else {
    cargarMain(`main-${generoGuardado}.html`);
    cargarProductosPorGenero(generoGuardado);
}


// Esta función se encarga de cargar los productos según el género usando la función del scriptApi.js.
function cargarProductosPorGenero(genero) {
    ProductosNuevosPorGenero(genero).then(productos => {
        mostrarProductos(productos);
    });
}


// Esta función carga el contenido del main sin modificar el header ni el footer.
function cargarMain(archivo) {
    fetch(archivo)
        .then(response => response.text())
        .then(data => {
            const contenedor = document.getElementById("contenido-main");
            contenedor.innerHTML = data;
            document.querySelector("main").classList.add("activo");
        })
        .catch(error => console.error("Error al cargar el main:", error));
}


// Estos botones permiten seleccionar el género. El texto del botón determina si se carga el main y productos de mujer o hombre.
document.querySelectorAll(".btn-principales[data-group='genero']").forEach(boton => {
    boton.addEventListener("click", () => {
        document.querySelectorAll(".btn-principales[data-group='genero']").forEach(b => b.classList.remove("activo"));
        boton.classList.add("activo");

        const target = boton.textContent.toLowerCase() === "mujer"
            ? "main-mujer.html"
            : "main-hombre.html";

        localStorage.setItem("genero", boton.textContent.toLowerCase());
        cargarMain(target);
        cargarProductosPorGenero(boton.textContent.toLowerCase());
    });
});





// Cuando el documento se carga, activamos el botón correspondiente al género guardado
// y verificamos si hay un token para validar la sesión del usuario.
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".btn-principales[data-group='genero']").forEach(boton => {
        boton.classList.remove("activo");
        if (boton.textContent.trim().toLowerCase() === generoGuardado) {
            boton.classList.add("activo");
        }
    });

    document.getElementById("btnIrUsuario").addEventListener("click", () => {
        const token = localStorage.getItem("tokenseguridad");
        if (token) {
            window.location.href = "editarPerfil.html";
        } else {
            window.location.href = "login.html";
        }
    });
});


// Este botón redirige a la sección de todos los productos según el género seleccionado.
document.getElementById("todos-productos").addEventListener("click", () => {
    const genero = localStorage.getItem("genero") || "mujer";

    if (genero === "hombre") {
        window.location.href = "products-hombre.html";
    } else {
        window.location.href = "products-mujer.html";
    }
});



// Delegación porque las categorías del main se insertan dinámicamente.
document.addEventListener("click", (evento) => {
    const tarjeta = evento.target.closest(".categoria");
    if (!tarjeta) return;
    evento.preventDefault();
    const genero = localStorage.getItem("genero") || "mujer";
    const enlace = tarjeta.querySelector("a");
    let categoria = (enlace?.textContent || tarjeta.textContent)
        .trim().toLowerCase().replace(/\s+/g, " ");
    const equivalencias = {
        "pantalones y jeans": "Pantalones",
        "vestidos y faldas": "Vestidos",
        "tops y blusas": "Blusas",
        "camisas y polos": "Ropa Casual",
        "t-shirt / básicos yjeans": "Ropa Casual"
    };
    categoria = equivalencias[categoria] || categoria;
    localStorage.setItem("genero", genero);
    localStorage.removeItem("categoria");
    window.location.href = `products-${genero}.html?categoria=${encodeURIComponent(categoria)}`;
});

// Este botón redirige a la sección de ofertas según el género seleccionado.
document.getElementById("ofertas").addEventListener("click", () => {
    const genero = localStorage.getItem("genero") || "mujer";

    if (genero === "hombre") {
        window.location.href = "OfertasHombre.html";
    } else {
        window.location.href = "OfertasMujer.html";
    }
});

// Este es el botón de los productos más vendidos en el menú lateral.
document.getElementById("mas-vendidos").addEventListener("click", () => {
    const genero = localStorage.getItem("genero");

    if (genero === "hombre") {
        window.location.href = "MasVendidosHombre.html";
    } else {
        window.location.href = "MasVendidosMujer.html";
    }
});


// Este botón redirige a la sección de novedades según el género seleccionado.
document.getElementById("novedades").addEventListener("click", () => {
    localStorage.removeItem("categoria");
    window.location.href = "index.html";
});
