// scriptModal.js - VERSIÓN CORREGIDA
class ModalProducto {
    constructor() {
        this.modal = document.getElementById('modal-producto');
        this.overlay = document.getElementById('overlay-modal');
        this.init();
    }
    
    init() {
        // Inicializar tallas
        this.inicializarTallas();
        
        // Abrir modal al hacer clic en productos
        document.addEventListener('click', (e) => {
            const producto = e.target.closest('.producto');
            if (producto) {
                e.preventDefault();
                this.abrir(producto);
            }
        });
        
        // Botón cerrar
        this.modal.querySelector('.btn-cerrar-detalles-producto').addEventListener('click', () => {
            this.cerrar();
        });
        
        // Botón agregar al carrito
        this.modal.querySelector('.btn-agregar-carrito').addEventListener('click', () => {
            this.agregarAlCarrito();
        });
        
        // Botones de cantidad
        this.modal.querySelector('.btn-menos').addEventListener('click', () => {
            this.cambiarCantidad(-1);
        });
        
        this.modal.querySelector('.btn-mas').addEventListener('click', () => {
            this.cambiarCantidad(1);
        });
        
        // Cerrar al hacer clic en overlay
        if (this.overlay) {
            this.overlay.addEventListener('click', () => this.cerrar());
        }
    }
    
    inicializarTallas() {
        const tallas = ['S', 'M', 'L', 'XL'];
        const contenedorTallas = document.getElementById('modal-tallas');
        
        if (!contenedorTallas) return;
        
        contenedorTallas.innerHTML = '';
        
        tallas.forEach((talla, index) => {
            const input = document.createElement('input');
            input.type = 'radio';
            input.name = 'talla-modal';
            input.id = `talla-${talla}`;
            input.value = talla;
            input.hidden = true;
            
            const label = document.createElement('label');
            label.htmlFor = `talla-${talla}`;
            label.textContent = talla;
            
            if (index === 0) input.checked = true;
            
            contenedorTallas.appendChild(input);
            contenedorTallas.appendChild(label);
        });
    }
    
    abrir(productoElement) {
        const producto = {
            idProducto: Number(productoElement.dataset.id) || null,
            genero: productoElement.dataset.genero || localStorage.getItem("genero") || "",
            categoria: productoElement.dataset.categoria || "",
            nombre: productoElement.querySelector('.nombre-producto')?.textContent.trim() || 'Producto',
            precio: productoElement.querySelector('.precio-producto')?.textContent.trim() || '$0.00',
            imagen: this.obtenerImagen(productoElement)
        };
        
        this.productoActual = producto;
        // Mostrar en el modal
        this.modal.querySelector('#modal-imagen').src = producto.imagen;
        this.modal.querySelector('#modal-imagen').alt = producto.nombre;
        this.modal.querySelector('#modal-nombre').textContent = producto.nombre;
        this.modal.querySelector('#modal-precio').textContent = producto.precio;
        
        // Resetear cantidad
        this.modal.querySelector('#modal-cantidad').value = 1;
        
        // Abrir modal
        this.modal.classList.add('activo');
        if (this.overlay) this.overlay.classList.add('activo');
        document.body.style.overflow = 'hidden';
    }
    
    obtenerImagen(productoElement) {
        const imagenDiv = productoElement.querySelector('.imagen-producto');
        if (!imagenDiv) return '';
        
        const bgImage = imagenDiv.style.backgroundImage;
        if (bgImage && bgImage !== 'none') {
            return bgImage.replace(/url\(['"]?(.*?)['"]?\)/i, '$1');
        }
        
        return '';
    }
    
    cambiarCantidad(cambio) {
        const input = this.modal.querySelector('#modal-cantidad');
        let cantidad = parseInt(input.value) || 1;
        cantidad += cambio;
        
        if (cantidad < 1) cantidad = 1;
        if (cantidad > 99) cantidad = 99;
        
        input.value = cantidad;
    }
    
    agregarAlCarrito() {
        const cantidad = parseInt(this.modal.querySelector('#modal-cantidad').value) || 1;
        const tallaSeleccionada = this.modal.querySelector('input[name="talla-modal"]:checked');
        const talla = tallaSeleccionada ? tallaSeleccionada.value : 'M';
        
        const productoParaCarrito = {
            nombre: this.modal.querySelector('#modal-nombre').textContent,
            precio: this.modal.querySelector('#modal-precio').textContent,
            imagen: this.modal.querySelector('#modal-imagen').src,
            talla: talla,
            cantidad: cantidad,
            idProducto: this.productoActual?.idProducto || null
        };
        
        if (window.carritoManager) {
            window.carritoManager.agregar(productoParaCarrito);
        }
        
        this.cerrar();
    }
    
    cerrar() {
        this.modal.classList.remove('activo');
        if (this.overlay) this.overlay.classList.remove('activo');
        document.body.style.overflow = '';
    }
}

// Inicializar
document.addEventListener('DOMContentLoaded', () => {
    window.modalProducto = new ModalProducto();
});