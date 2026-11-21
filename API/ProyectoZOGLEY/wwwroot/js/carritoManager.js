class CarritoManager {
  constructor() {
    this.carrito = JSON.parse(localStorage.getItem("carritoZogley")) || [];
    this.initPromise = this.waitForDOM();
  }

  waitForDOM() {
    return new Promise((resolve) => {
      if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", () => {
          this.inicializar();
          resolve();
        });
      } else {
        this.inicializar();
        resolve();
      }
    });
  }

  inicializar() {
    this.actualizarContador();
    this.configurarEventosCarrito();
    this.activarBotonPagar();  // <- nuevo nombre
    this.actualizarVistaCarrito();
}


  guardar() {
    localStorage.setItem("carritoZogley", JSON.stringify(this.carrito));
    this.actualizarContador();
    this.actualizarVistaCarrito();
    this.actualizarTotales();
  }

  async agregar(producto) {
    if (typeof sesionValida !== "function" || !sesionValida()) {
      alert("Debes iniciar sesión para agregar productos al carrito.");
      window.location.href = "login.html";
      return false;
    }
    try {
      if (typeof ValidarSesionCliente !== "function") throw new Error("No se pudo validar la sesión.");
      await ValidarSesionCliente(); // La API verifica firma, expiración y rol Cliente del JWT.
    } catch (error) {
      alert("Tu sesión no es válida o ha expirado. Inicia sesión nuevamente.");
      if (typeof cerrarSesion === "function") cerrarSesion();
      else window.location.replace("login.html");
      return false;
    }
    producto.nombre = producto.nombre?.trim() || "Producto";
    producto.precio = producto.precio?.trim() || "$0.00";
    producto.talla = producto.talla || "Única";

    const existe = this.carrito.find(
      (item) => item.nombre === producto.nombre && item.talla === producto.talla
    );

    if (existe) {
      existe.cantidad += producto.cantidad || 1;
    } else {
      this.carrito.push({ ...producto, cantidad: producto.cantidad || 1 });
    }

    this.guardar();
    this.mostrarNotificacion("✓ Producto agregado al carrito");
    return true;
  }

  eliminar(index) {
    this.carrito.splice(index, 1);
    this.guardar();
  }

  actualizarCantidad(index, nuevaCantidad) {
    if (this.carrito[index]) {
      this.carrito[index].cantidad = nuevaCantidad;
      this.guardar();
    }
  }

  // Helper para extraer precio numérico
  extraerPrecioNumerico(precioStr) {
    const match = precioStr?.match(/\d+\.?\d*/);
    return match ? parseFloat(match[0]) : 0;
  }

  obtenerSubtotal() {
    return this.carrito.reduce((total, item) => {
      const precioNum = this.extraerPrecioNumerico(item.precio);
      return total + precioNum * (item.cantidad || 1);
    }, 0);
  }

  obtenerCantidadTotal() {
    return this.carrito.reduce(
      (total, item) => total + (item.cantidad || 1),
      0
    );
  }

  actualizarContador() {
    const cantidad = this.obtenerCantidadTotal();
    const contadores = document.querySelectorAll(".contador-carrito");

    contadores.forEach((contador) => {
      contador.textContent = cantidad;
      contador.style.display = cantidad > 0 ? "flex" : "none";
    });
  }

  actualizarTotales() {
    const subtotalElement = document.querySelector(
      ".contenedor-subtotal .contenedor-cantidad span"
    );
    const precioElement = document.querySelector(".carrito-precio");

    if (subtotalElement) {
      const cantidad = this.obtenerCantidadTotal();
      subtotalElement.textContent = `(${cantidad} ${
        cantidad === 1 ? "producto" : "productos"
      })`;
    }

    if (precioElement) {
      const subtotal = this.obtenerSubtotal();
      precioElement.textContent = `$${subtotal.toFixed(2)}`;
    }
  }

  actualizarVistaCarrito() {
    const tablaCarrito = document.querySelector(".lista-carrito");
    if (!tablaCarrito) return;

    let tbody =
      tablaCarrito.querySelector("tbody") || this.crearTbody(tablaCarrito);
    tbody.innerHTML = "";

    if (this.carrito.length === 0) {
      tbody.appendChild(this.crearFilaVacia());
    } else {
      this.carrito.forEach((item, index) => {
        tbody.appendChild(this.crearFilaProducto(item, index));
      });
    }

    this.actualizarTotales();
  }

  crearTbody(tablaCarrito) {
    const tbody = document.createElement("tbody");
    tablaCarrito.appendChild(tbody);
    return tbody;
  }

  crearFilaVacia() {
    const row = document.createElement("tr");
    const td = document.createElement("td");
    td.colSpan = 2;
    td.style.cssText = "text-align: center; padding: 20px; color: #666;";
    td.textContent = "Tu carrito está vacío";
    row.appendChild(td);
    return row;
  }

  crearFilaProducto(item, index) {
    const precioNum = this.extraerPrecioNumerico(item.precio);
    const subtotal = precioNum * (item.cantidad || 1);

    const row = document.createElement("tr");
    row.innerHTML = `
      <td>
        <img src="${item.imagen || "Recursos/imagenes/placeholder.jpg"}" 
             alt="${item.nombre}" 
             class="imagen-producto-carrito"
             onerror="this.src='Recursos/imagenes/placeholder.jpg'">
      </td>
      <td>
        <div class="nombre-talla-linea">
          <span class="nombre-producto">${
            item.nombre?.trim() || "Producto"
          }</span>
          <span class="talla-producto">Talla: ${
            item.talla || "No especificada"
          }</span>
        </div>
        
        <div class="contenido-producto">
          <div class="columna-izquierda">
            <div class="info-cantidad">
              <span class="label-cantidad">Cantidad:</span>
              <span class="controles-cantidad">
                <button class="btn-cantidad btn-cantidad-menos" data-index="${index}">-</button>
                <span class="cantidad-numero">${item.cantidad || 1}</span>
                <button class="btn-cantidad btn-cantidad-mas" data-index="${index}">+</button>
              </span>
            </div>
            
            <div class="info-precio">
              <span class="precio-label">Precio unitario:</span>
              <span class="precio-valor">${item.precio || "$0.00"}</span>
            </div>
            
            <div class="info-subtotal">
              <span class="subtotal-label">Subtotal:</span>
              <span class="subtotal-valor">$${subtotal.toFixed(2)}</span>
            </div>
          </div>
          
          <div class="columna-derecha">
            <button class="btn-eliminar" data-index="${index}" title="Eliminar producto">X</button>
          </div>
        </div>
      </td>
    `;
    return row;
  }

  configurarEventosCarrito() {
    const tablaCarrito = document.querySelector(".lista-carrito");
    if (!tablaCarrito) return;

    // Event delegation centralizado
    tablaCarrito.addEventListener("click", (e) => {
      this.handleCarritoClick(e);
    });

    // Carrito checkbox
    const checkboxCarrito = document.getElementById("checkbox-carrito");
    if (checkboxCarrito) {
      checkboxCarrito.addEventListener("change", () => {
        if (checkboxCarrito.checked) this.actualizarVistaCarrito();
      });
    }

    // Botones del carrito
    document
      .querySelectorAll('.btn-iconos.carrito, label[for="checkbox-carrito"]')
      .forEach((el) => {
        el.addEventListener("click", () => {
          requestAnimationFrame(() => this.actualizarVistaCarrito());
        });
      });
  }

  handleCarritoClick(e) {
    const boton = e.target;
    const index = parseInt(boton.getAttribute("data-index"));

    if (isNaN(index)) return;

    if (boton.classList.contains("btn-eliminar")) {
      this.eliminar(index);
    } else if (boton.classList.contains("btn-cantidad-menos")) {
      this.actualizarCantidad(
        index,
        Math.max(1, (this.carrito[index]?.cantidad || 1) - 1)
      );
    } else if (boton.classList.contains("btn-cantidad-mas")) {
      this.actualizarCantidad(index, (this.carrito[index]?.cantidad || 1) + 1);
    }
  }

  mostrarNotificacion(mensaje) {
    const notificacion = document.createElement("div");
    notificacion.textContent = mensaje;
    notificacion.style.cssText = `
    position: fixed;
    top: 100px;
    right: 20px;
    background: #b8725e;
    color: white;
    padding: 8px 16px;
    border-radius: 5px;
    z-index: 10001;
    font-size: 13px;
    font-weight: 500;
    box-shadow: 0 2px 8px rgba(0,0,0,0.15);
  `;

    document.body.appendChild(notificacion);
    setTimeout(() => notificacion.remove(), 3000);
  }

  vaciarCarrito() {
    this.carrito = [];
    this.guardar();
  }

  activarBotonPagar() {
    const btnPagar = document.querySelector(".btn-pagar");

    if (btnPagar) {
        btnPagar.addEventListener("click", () => {
            if (typeof sesionValida !== "function" || !sesionValida()) {
                alert("Inicia sesión para continuar con tu compra.");
                window.location.href = "login.html";
                return;
            }
            window.location.href = "procesar-transaccion.html";
        });
    }
}

}

// Inicializar
const carritoManager = new CarritoManager();
window.carritoManager = carritoManager;
