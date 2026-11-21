// Protege las páginas privadas incluso al volver desde la caché de navegación.
(function () {
  const ruta = location.pathname.toLowerCase();
  const esAdmin = ruta.endsWith("/admin.html");
  const paginaPrivada = esAdmin || ruta.endsWith("/editarperfil.html") ||
    ruta.endsWith("/procesar-transaccion.html");

  function leerPayload(token) {
    try {
      const parte = token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/");
      return JSON.parse(decodeURIComponent(escape(atob(parte))));
    } catch { return null; }
  }

  function validarPagina() {
    if (!paginaPrivada) return;
    const token = localStorage.getItem("tokenseguridad");
    const payload = token ? leerPayload(token) : null;
    const exp = Number(payload?.exp || 0);
    const vigente = !!payload && (!exp || exp * 1000 > Date.now());

    if (!vigente) {
      ["tokenseguridad", "rol", "usuarioZogley", "carritoZogley"].forEach(k => localStorage.removeItem(k));
      sessionStorage.clear();
      location.replace("login.html");
      return;
    }

    if (esAdmin) {
      const rol = localStorage.getItem("rol") || payload?.role || payload?.rol ||
        payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || "";
      if (rol.toLowerCase() !== "admin") location.replace("index.html");
    }
  }

  document.addEventListener("DOMContentLoaded", validarPagina);
  window.addEventListener("pageshow", validarPagina);
  window.addEventListener("pagehide", () => {
    if (paginaPrivada) {
      document.querySelectorAll("input[type=password]").forEach(i => i.value = "");
      document.querySelectorAll("input[autocomplete]").forEach(i => {
        if (i.autocomplete === "current-password") i.value = "";
      });
    }
  });
})();
