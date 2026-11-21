class ProcesadorTransaccion {
  constructor() {
    this.carrito = JSON.parse(localStorage.getItem("carritoZogley")) || [];
    this.tipoEntrega = "recoger";
    this.init();
  }

  async init() {
    if (typeof exigirSesion !== "function" || !exigirSesion()) return;
    await this.cargarDatosCliente();
    if (this.carrito.length === 0) {
      alert("Tu carrito está vacío. Serás redirigido al inicio.");
      window.location.href = "index.html";
      return;
    }

    this.cargarProductos();
    this.actualizarTotales();
    this.configurarEventos();
  }

  // Helper para extraer precio numérico
  extraerPrecioNumerico(precioStr) {
    const match = precioStr?.match(/\d+\.?\d*/);
    return match ? parseFloat(match[0]) : 0;
  }

  async cargarDatosCliente() {
    try {
      const perfil = await ObtenerMiPerfil();
      const datos = perfil.usuario || perfil;
      document.getElementById("nombre").value = datos.nombreApellido || datos.nombre || "";
      document.getElementById("email").value = datos.email || "";
      document.getElementById("telefono").value = datos.telefono || "";
      document.getElementById("direccion").value = datos.direccion || "";
      document.getElementById("provincia").value = datos.provincia || "";
      document.getElementById("distrito").value = datos.distrito || "";
    } catch (error) { console.error("No se pudieron cargar los datos del cliente:", error); }
  }

  cargarProductos() {
    const listaProductos = document.getElementById("lista-productos");
    listaProductos.innerHTML = "";

    this.carrito.forEach((producto) => {
      const precioNum = this.extraerPrecioNumerico(producto.precio);
      const subtotal = precioNum * (producto.cantidad || 1);

      const productoHTML = `
        <div class="producto-item">
          <img src="${producto.imagen || "Recursos/imagenes/placeholder.jpg"}" 
               alt="${producto.nombre}" 
               class="producto-imagen"
               onerror="this.src='Recursos/imagenes/placeholder.jpg'">
          <div class="producto-info">
            <div class="producto-nombre">${producto.nombre}</div>
            <div class="producto-detalles">Talla: ${producto.talla || "Única"}</div>
            <div class="producto-precio-cantidad">
              <span class="producto-cantidad">Cantidad: ${producto.cantidad || 1}</span>
              <span class="producto-precio">$${subtotal.toFixed(2)}</span>
            </div>
          </div>
        </div>
      `;

      listaProductos.innerHTML += productoHTML;
    });
  }

  actualizarTotales() {
    const subtotal = this.carrito.reduce((total, item) => {
      const precioNum = this.extraerPrecioNumerico(item.precio);
      return total + precioNum * (item.cantidad || 1);
    }, 0);

    const costoEnvio = this.tipoEntrega === "envio" ? 5.0 : 0;
    const itbms = subtotal * 0.07; // 7% de impuesto
    const total = subtotal + costoEnvio + itbms;

    document.getElementById("subtotal-valor").textContent = `${subtotal.toFixed(2)}`;
    document.getElementById("envio-valor").textContent = costoEnvio > 0 ? `${costoEnvio.toFixed(2)}` : "Gratis";
    document.getElementById("itbms-valor").textContent = `${itbms.toFixed(2)}`;
    document.getElementById("total-valor").textContent = `${total.toFixed(2)}`;
  }

  configurarEventos() {
    // Cambio de tipo de entrega
    const opcionesEntrega = document.querySelectorAll('.opcion-entrega input[type="radio"]');
    opcionesEntrega.forEach((radio) => {
      radio.addEventListener("change", (e) => {
        // Actualizar clases activas
        document.querySelectorAll(".opcion-entrega").forEach((opt) => {
          opt.classList.remove("active");
        });
        e.target.closest(".opcion-entrega").classList.add("active");

        // Mostrar/ocultar sección de dirección
        const seccionDireccion = document.getElementById("seccion-direccion");
        this.tipoEntrega = e.target.value;

        if (this.tipoEntrega === "envio") {
          seccionDireccion.style.display = "block";
        } else {
          seccionDireccion.style.display = "none";
        }

        this.actualizarTotales();
      });
    });

    // Botón editar productos
    document.getElementById("btn-editar").addEventListener("click", () => {
      this.abrirModalEdicion();
    });

    // Botón confirmar pedido
    document.getElementById("btn-confirmar").addEventListener("click", () => {
      this.confirmarPedido();
    });

    // Cerrar modal
    const btnsCerrar = document.querySelectorAll(".btn-cerrar-modal");
    btnsCerrar.forEach((btn) => {
      btn.addEventListener("click", () => {
        this.cerrarModal("modal-editar");
      });
    });

    // Cerrar modal al hacer clic fuera
    document.querySelectorAll(".modal").forEach((modal) => {
      modal.addEventListener("click", (e) => {
        if (e.target === modal) {
          this.cerrarModal(modal.id);
        }
      });
    });
  }

  abrirModalEdicion() {
    const modal = document.getElementById("modal-editar");
    const productosEditables = document.getElementById("productos-editables");

    productosEditables.innerHTML = "";

    this.carrito.forEach((producto, index) => {
      const precioNum = this.extraerPrecioNumerico(producto.precio);
      const subtotal = precioNum * (producto.cantidad || 1);

      const productoHTML = `
        <div class="producto-editable" data-index="${index}">
          <img src="${producto.imagen || "Recursos/imagenes/placeholder.jpg"}" 
               alt="${producto.nombre}" 
               class="producto-editable-imagen"
               onerror="this.src='Recursos/imagenes/placeholder.jpg'">
          <div class="producto-editable-info">
            <div class="producto-editable-nombre">${producto.nombre}</div>
            <div class="producto-editable-detalles">
              Talla: ${producto.talla || "Única"} | 
              Precio unitario: ${producto.precio} | 
              Subtotal: $${subtotal.toFixed(2)}
            </div>
            <div class="producto-editable-controles">
              <div class="control-cantidad">
                <label>Cantidad:</label>
                <button class="btn-cantidad-modal btn-menos" data-index="${index}">-</button>
                <input type="number" 
                       class="cantidad-input" 
                       value="${producto.cantidad || 1}" 
                       min="1" 
                       data-index="${index}">
                <button class="btn-cantidad-modal btn-mas" data-index="${index}">+</button>
              </div>
              <button class="btn-eliminar-modal" data-index="${index}">Eliminar</button>
            </div>
          </div>
        </div>
      `;

      productosEditables.innerHTML += productoHTML;
    });

    // Configurar eventos del modal
    this.configurarEventosModal();

    modal.classList.add("show");
  }

  configurarEventosModal() {
    // Botones de cantidad
    document.querySelectorAll(".btn-menos").forEach((btn) => {
      btn.addEventListener("click", (e) => {
        const index = parseInt(e.target.getAttribute("data-index"));
        const input = document.querySelector(`.cantidad-input[data-index="${index}"]`);
        const nuevaCantidad = Math.max(1, parseInt(input.value) - 1);
        input.value = nuevaCantidad;
        this.actualizarSubtotalModal(index, nuevaCantidad);
      });
    });

    document.querySelectorAll(".btn-mas").forEach((btn) => {
      btn.addEventListener("click", (e) => {
        const index = parseInt(e.target.getAttribute("data-index"));
        const input = document.querySelector(`.cantidad-input[data-index="${index}"]`);
        const nuevaCantidad = parseInt(input.value) + 1;
        input.value = nuevaCantidad;
        this.actualizarSubtotalModal(index, nuevaCantidad);
      });
    });

    document.querySelectorAll(".cantidad-input").forEach((input) => {
      input.addEventListener("change", (e) => {
        const index = parseInt(e.target.getAttribute("data-index"));
        const nuevaCantidad = Math.max(1, parseInt(e.target.value) || 1);
        e.target.value = nuevaCantidad;
        this.actualizarSubtotalModal(index, nuevaCantidad);
      });
    });

    // Botones eliminar
    document.querySelectorAll(".btn-eliminar-modal").forEach((btn) => {
      btn.addEventListener("click", (e) => {
        const index = parseInt(e.target.getAttribute("data-index"));
        if (confirm("¿Eliminar este producto del carrito?")) {
          this.carrito.splice(index, 1);
          
          if (this.carrito.length === 0) {
            alert("Tu carrito está vacío. Serás redirigido al inicio.");
            localStorage.setItem("carritoZogley", JSON.stringify(this.carrito));
            window.location.href = "index.html";
            return;
          }
          
          this.abrirModalEdicion(); // Recargar modal
        }
      });
    });

    // Botón guardar cambios
    document.querySelector(".btn-guardar-cambios").addEventListener("click", () => {
      // Guardar cantidades actualizadas
      document.querySelectorAll(".cantidad-input").forEach((input) => {
        const index = parseInt(input.getAttribute("data-index"));
        const cantidad = parseInt(input.value);
        if (this.carrito[index]) {
          this.carrito[index].cantidad = cantidad;
        }
      });

      // Guardar en localStorage y actualizar vista
      localStorage.setItem("carritoZogley", JSON.stringify(this.carrito));
      this.cargarProductos();
      this.actualizarTotales();
      this.cerrarModal("modal-editar");
    });
  }

  actualizarSubtotalModal(index, nuevaCantidad) {
    const producto = this.carrito[index];
    if (producto) {
      const precioNum = this.extraerPrecioNumerico(producto.precio);
      const subtotal = precioNum * nuevaCantidad;
      const detallesElement = document.querySelector(
        `.producto-editable[data-index="${index}"] .producto-editable-detalles`
      );
      if (detallesElement) {
        detallesElement.innerHTML = `
          Talla: ${producto.talla || "Única"} | 
          Precio unitario: ${producto.precio} | 
          Subtotal: $${subtotal.toFixed(2)}
        `;
      }
    }
  }

  cerrarModal(modalId) {
    const modal = document.getElementById(modalId);
    modal.classList.remove("show");
  }

  validarFormulario() {
    const nombre = document.getElementById("nombre").value.trim();
    const email = document.getElementById("email").value.trim();
    const telefono = document.getElementById("telefono").value.trim();

    const metodoPago = document.getElementById("metodo-pago")?.value || "";
    if (!nombre || !email || !telefono) {
      alert("Por favor completa todos los campos obligatorios del formulario.");
      return false;
    }

    if (!metodoPago) {
      alert("Selecciona un método de pago.");
      return false;
    }

    // Validar email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      alert("Por favor ingresa un correo electrónico válido.");
      return false;
    }

    // Si es envío, validar dirección
    if (this.tipoEntrega === "envio") {
      const direccion = document.getElementById("direccion").value.trim();
      const provincia = document.getElementById("provincia").value.trim();
      const distrito = document.getElementById("distrito").value.trim();

      if (!direccion || !provincia || !distrito) {
        alert("Por favor completa todos los campos de dirección de envío.");
        return false;
      }
    }

    return true;
  }

  async confirmarPedido() {
    if (!this.validarFormulario()) return;
    if (!sesionValida()) { alert("Tu sesión expiró. Inicia sesión nuevamente."); cerrarSesion(); return; }
    const datosFactura = {
      tipoEntrega: this.tipoEntrega,
      notas: document.getElementById("notas").value.trim(),
      metodoPago: document.getElementById("metodo-pago").value,
      direccion: this.tipoEntrega === "envio" ? {
        direccion: document.getElementById("direccion").value.trim(),
        provincia: document.getElementById("provincia").value.trim(),
        distrito: document.getElementById("distrito").value.trim(),
        referencia: document.getElementById("referencia").value.trim()
      } : null,
      detalles: this.carrito.map(p => ({ idProducto: p.idProducto, cantidad: Number(p.cantidad || 1), talla: p.talla || "Única" }))
    };
    const boton = document.getElementById("btn-confirmar");
    boton.disabled = true; boton.textContent = "Procesando...";
    try {
      const respuesta = await CrearFactura(datosFactura);
      localStorage.removeItem("carritoZogley");
      document.getElementById("numero-pedido").textContent = respuesta.numeroFactura || respuesta.numeroPedido || respuesta.idFactura || "Registrada";
      const mensaje = document.querySelector("#modal-confirmacion p");
      if (mensaje) mensaje.textContent = respuesta.correoEnviado ? "Tu factura fue guardada y enviada a tu correo." : "Tu compra y factura fueron registradas correctamente.";
      let enlaceFactura = document.getElementById("enlace-descargar-factura");
      if (!enlaceFactura) {
        enlaceFactura = document.createElement("a");
        enlaceFactura.id = "enlace-descargar-factura";
        enlaceFactura.textContent = "Descargar factura PDF";
        enlaceFactura.style.display = "inline-block";
        enlaceFactura.style.marginTop = "12px";
        document.querySelector("#modal-confirmacion")?.appendChild(enlaceFactura);
      }
      if (respuesta.urlFactura) {
        enlaceFactura.href = `${API_URL}${respuesta.urlFactura}`;
        enlaceFactura.target = "_blank";
        enlaceFactura.rel = "noopener";
        enlaceFactura.onclick = async (evento) => {
          evento.preventDefault();
          const token = localStorage.getItem("tokenseguridad");
          const descarga = await fetch(enlaceFactura.href, { headers: { Authorization: `Bearer ${token}` }, cache: "no-store" });
          if (!descarga.ok) { alert("No se pudo descargar la factura."); return; }
          const blob = await descarga.blob();
          const url = URL.createObjectURL(blob);
          const a = document.createElement("a");
          a.href = url;
          a.download = `Factura-${respuesta.numeroFactura || respuesta.idFactura}.pdf`;
          a.click();
          URL.revokeObjectURL(url);
        };
        enlaceFactura.style.pointerEvents = "auto";
      }
      document.getElementById("modal-confirmacion").classList.add("show");
    } catch (error) {
      console.error("Error al crear la factura:", error);
      alert(`No se pudo completar la compra: ${error.message}`);
    } finally {
      boton.disabled = false; boton.textContent = "Confirmar Pedido";
    }
  }
}

// Inicializar cuando el DOM esté listo
document.addEventListener("DOMContentLoaded", () => {
  new ProcesadorTransaccion();
});