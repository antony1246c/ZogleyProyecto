# Cambios en el frontend de Zogley

El frontend ahora consulta el perfil autenticado, valida la sesión en el servidor antes de agregar artículos al carrito, envía el checkout al endpoint de facturación, muestra precios normal y rebajado en Ofertas, conserva la categoría seleccionada desde las páginas principales, y ofrece acciones administrativas para productos y ventas.

La configuración de la API está en `js/scriptApi.js` (`API_URL`). La guía de instalación e integración del backend y SQL está en `../INSTRUCCIONES_INTEGRACION.md`.


## Integración final de checkout y administración

- Los accesos directos de categorías envían la categoría en la URL; no se conserva en `localStorage`. Al refrescar el catálogo se elimina el filtro temporal.
- Se normalizaron las categorías visibles de los accesos directos con el catálogo oficial de la base de datos.
- El checkout recupera el nombre, correo, teléfono y dirección del perfil autenticado antes de habilitar la confirmación.
- El cliente elige un método de pago y la API valida y almacena la selección en `Facturas.MetodoPago`; también aparece en el PDF y en el panel de ventas.
- El registro permite guardar dirección, provincia y distrito como datos opcionales para agilizar futuras compras.
- La administración lista y desactiva usuarios mediante baja lógica. Los usuarios desactivados no pueden iniciar sesión ni validar operaciones de cliente.
- Productos y ventas conservan baja lógica/cancelación para proteger el historial.
- Ejecuta primero `../SQL_Migracion_Zogley_Facturas_Ofertas_Admin.sql` sobre la base de datos `Zogley`.

**Límite de pagos:** seleccionar tarjeta, Yappy o transferencia registra el método preferido, pero no ejecuta un cobro real. Para cobrar electrónicamente se necesita integrar las credenciales y el flujo de una pasarela bancaria.
