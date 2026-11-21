using Microsoft.EntityFrameworkCore;
using ProyectoZOGLEY.Custom;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Models;
using ProyectoZOGLEY.Models.DTOS;
using System.Text.RegularExpressions;

namespace ProyectoZOGLEY.Services
{
    public class UserInfoService
    {

        private readonly ApplicationDbContext _context;
        private readonly Utilidades _utilidades;

        public UserInfoService(ApplicationDbContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }


        /// <summary>
        /// Creacion del registro de usuario en la base de datos, con sus validaciones
        /// </summary>
        public async Task<(bool Exito, object? Resultado)> RegistrarUsuarioAsync(UserInfoDto userInfoDto)
        // El bool exito y el objeto resultado son las respuesta que retornamos si fue false o true y con este el mensaje de error o correcto

        {
            // VALIDAMOS QUE LOS CAMPOS NO ESTEN VACIOS
            if (string.IsNullOrWhiteSpace(userInfoDto.Email) ||
                            string.IsNullOrWhiteSpace(userInfoDto.Contrasenia) ||
                            string.IsNullOrWhiteSpace(userInfoDto.NombreApellido) ||
                            string.IsNullOrWhiteSpace(userInfoDto.Telefono))
            {
                // CREACION DEL OBJETO RESULTADO EN EL QUE CONSTRUIMOS EL MENSAJE A DEVOLVER
                return (false, new
                {
                    mensaje = "No puede dejar campos vacÃ­os.",
                    errores = new
                    {
                        NombreApellido = string.IsNullOrWhiteSpace(userInfoDto.NombreApellido) ? "Tiene que ingresar su nombre y apellido" : null,
                        Telefono = string.IsNullOrWhiteSpace(userInfoDto.Telefono) ? "Tiene que ingresar su nÃºmero de telÃ©fono" : null,
                        Email = string.IsNullOrWhiteSpace(userInfoDto.Email) ? "Tiene que ingresar su correo electrÃ³nico" : null,
                        Contrasenia = string.IsNullOrWhiteSpace(userInfoDto.Contrasenia) ? "Tiene que ingresar su contraseÃ±a" : null
                    }
                });
            }

            /* VALIDACIÃ“N TELÃ‰FONO EL REGEX LO UTILIZAMOS PARA PODER CREAR EL OBJETO QUE ALMACENARA
            LA EXPRESION REGULAR DEL FORMATO DE TELÃ‰FONO QUE DEBE TENER EL USUARIO 6XXX-XXXX
            */
            var regexTelefono = new Regex(@"^6\d{3}-\d{4}$");
            if (!regexTelefono.IsMatch(userInfoDto.Telefono))
            {
                return (false, new
                {
                    mensaje = "Formato de telÃ©fono invÃ¡lido. Debe ser 6XXX-XXXX"
                });
            }

            // VALIDACIÃ“N CONTRASEÃ‘A EN EL QUE VERIFICAMOS LA CANTIDAD DE CARACTERES    
            if (userInfoDto.Contrasenia.Length < 8)
            {
                return (false, new
                {
                    mensaje = "La contraseÃ±a no puede tener menos de 8 caracteres"
                });
            }

            /* Creamos una variable booleana que tomara o buscara en la base de datos, si el email ya existe
             * con el AnyAsync nos aseguramos que se ejecute esta accion de forma sincronica sin truncar con nuestros procesos
             * y devolvemos un true o false dependiendo de la compracion que tenemos en esta misma. */
            bool emailExiste = await _context.DatosUsuarios.AnyAsync(u => u.Email == userInfoDto.Email);

            if (emailExiste)
            {
                return (false, new { mensaje = "El correo ya estÃ¡ registrado" });
            }

            /* Creamos lo mismo que lo anterior pero para verificar que el telefono no se repita */
            bool telefonoExiste = await _context.DatosUsuarios.AnyAsync(t => t.Telefono == userInfoDto.Telefono);

            if (telefonoExiste)
            {
                return (false, new { mensaje = "El telefono ya estÃ¡ registrado" });
            }


            // HASH PASSWORD UTILIZANDO Bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userInfoDto.Contrasenia);
            // Con esto estamos guardando el hash en vez de la contraseÃ±a del mismo usuario es cuestion de seguridad
            /* Creacion del modelo del usuario que estaremos guardando en la base de datos
             * utilizando el modelo del Dto que recibimos en el metodo
             */
            var modeloUsuario = new UserInfo
            {
                NombreApellido = userInfoDto.NombreApellido,
                Email = userInfoDto.Email,
                Telefono = userInfoDto.Telefono,
                Contrasenia = hashedPassword,
                Rol = "Cliente"
            };
            // GUARDAR EN BD
            _context.DatosUsuarios.Add(modeloUsuario);
            await _context.SaveChangesAsync();

            return (true, new { mensaje = "Usuario registrado exitosamente" });
        }

        /// <summary>
        /// Creacion del registro de usuario en la base de datos, con sus validaciones
        /// </summary>
        public async Task<(bool Exito, object? Resultado)> LoginUsuariosAsync(LoginDto loginDto)
        /*Por medio de esta tarea asincronica hacemos el login del usuario utilizando la clase LoginDto para guardar los datos temporales 
        * del usuario que ingresa al sistema, verificando si el email existe y si la contraseÃ±a es correcta */
        {
            // aqui solo verificamos el email ya que la contraseÃ±a nunca sera igual porque la que se guarda esta hasheada
            var usuario = await _context.DatosUsuarios.FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Activo);
            if (usuario == null)
                return (false, new { mensaje = "Usuario no encontrado" });

            //VALIDAR CONTRASEÃ‘A HASHED 
            bool isValid = BCrypt.Net.BCrypt.Verify(loginDto.Contrasenia, usuario.Contrasenia);
            if (!isValid)
                return (false, new { mensaje = "ContraseÃ±a incorrecta" });

            // Retornamos el mensaje de login exitoso con el token generado por el metodo generarJWT guardado en utilidades
            return (true, new
            {
                rol = usuario.Rol,
                mensaje = "Login exitoso",
                token = _utilidades.generarJWT(usuario),
                usuario = new
                {
                    idUsuario = usuario.IdUsuario,
                    nombreApellido = usuario.NombreApellido,
                    email = usuario.Email,
                    telefono = usuario.Telefono,
                    rol = usuario.Rol
                }
            });
        }


    }
}
