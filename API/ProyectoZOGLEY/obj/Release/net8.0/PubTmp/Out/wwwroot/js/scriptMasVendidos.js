// Esta función muestra los productos en el HTML mediante un innerHTML.
function mostrarProductos(lista) {
    const contenedor = document.getElementById("contenedor-productos");
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
const generoGuardado = localStorage.getItem("genero");
cargarMain(`${generoGuardado}-productos.html`);


// Esta función se encarga de cargar los productos dependiendo del género.
function cargarProductosMasVendidos(genero) {
    ProductosMasComprados(genero).then(productos => {
        mostrarProductos(productos); 
    });
}

// Estos son los botones de los géneros, donde se obtiene el texto para identificar si el usuario seleccionó 'hombre' o 'mujer'.
document.querySelectorAll(".btn-principales[data-group='genero']").forEach(boton => {
    boton.addEventListener("click", () => {

        document.querySelectorAll(".btn-principales[data-group='genero']").forEach(b => b.classList.remove("activo"));
        boton.classList.add("activo");

        const genero = boton.textContent.toLowerCase();

        if (genero === "hombre") {
           window.location.href = "MasVendidosHombre.html";
        } else {
            window.location.href = "MasVendidosMujer.html";
        }

        localStorage.setItem("genero", genero);

        cargarProductosMasVendidos(genero); // Actualiza los divs
    });
});


// Este evento cierra el carrito cuando se hace clic en el overlay.
document.getElementById("overlay").addEventListener("click", () => {
  document.getElementById("checkbox-carrito").checked = false;
});



// Estas variables y eventos manejan los botones de más y menos en la cantidad de productos.
const btnMas = document.querySelector(".btn-mas");
const btnMenos = document.querySelector(".btn-menos");
const inputCantidad = document.querySelector("#cantidad");
btnMas.addEventListener("click", () => {
    inputCantidad.value = Number(inputCantidad.value) + 1;
});
btnMenos.addEventListener("click", () => {
    if (Number(inputCantidad.value) > inputCantidad.min) {
        inputCantidad.value = Number(inputCantidad.value) - 1;
    }
});



// Este evento se ejecuta al cargar el documento; toma el token registrado y valida si el usuario necesita iniciar sesión o no.
document.addEventListener("DOMContentLoaded", () => {
    const token = localStorage.getItem("tokenseguridad");

        document.getElementById("btnIrUsuario").addEventListener("click", () => {
            const token = localStorage.getItem("tokenseguridad");
           if (token) {
            window.location.href = "editarPerfil.html";
            } else {
            window.location.href = "login.html";
        }
        });


    // Al recargar la página se asigna el botón activo dependiendo del género guardado en el local storage.
    document.querySelectorAll(".btn-principales[data-group='genero']").forEach(boton => {
        boton.classList.remove("activo");
        if (boton.textContent.toLowerCase() === generoGuardado) {
            boton.classList.add("activo");
        }
    });

    // Cargar productos del género guardado
    cargarProductosMasVendidos(generoGuardado);
}); 



// Este es el botón de ofertas en el menú lateral.
document.getElementById("ofertas").addEventListener("click", () => {
    const genero = localStorage.getItem("genero");

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


// Este botón selecciona el género correcto al elegir "todos los productos".
document.getElementById("todos-productos").addEventListener("click", () => {
    const genero = localStorage.getItem("genero");

    if (genero === "hombre") {
        window.location.href = "products-hombre.html";
    } else {
        window.location.href = "products-mujer.html";
    }
});