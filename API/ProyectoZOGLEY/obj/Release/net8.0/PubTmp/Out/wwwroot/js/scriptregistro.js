// Variables para cada input que realice el usuario 
const form= document.getElementById('loginForm');
const userName=document.getElementById('Username');
const email=document.getElementById('Useremail');
const password=document.getElementById('password');
const password2= document.getElementById('password2');
const phone=document.getElementById('phone');

// Agregar un evento de escucha para el envío del formulario
form.addEventListener('submit', (e)=>{
    e.preventDefault(); // Evitar el envío del formulario por defecto
    validateInputs();
});

// Funciones de validación de los inputs del formulario
const setError=(element,message)=>{
    const inputControl= element.parentElement;
    const errorDisplay= inputControl.querySelector('.error');
    errorDisplay.innerText= message;
    inputControl.classList.add('error');
    inputControl.classList.remove('success');
};
// Función para mostrar que el input es correcto
const setSuccess= element=>{
    const inputControl= element.parentElement;
    const errorDisplay= inputControl.querySelector('.error');
    errorDisplay.innerText= '';
    inputControl.classList.add('success');
    inputControl.classList.remove('error');
}

// aqui utilizamos una expresion regular que es la que nos permite la verificacion del correo electronico
const isValidEmail=email=>{
    // esta es una expresion regular en la que verificamos el correo electronico
    const re= /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

// Expresion regular para verificar que en el nombre no se ingresen numeros ni caracteres especiales
const isValidNombre=userName=>{
    const re= /^([A-Za-zÑñÁáÉéÍíÓóÚú]+['\-]{0,1}[A-Za-zÑñÁáÉéÍíÓóÚú]+)(\s+([A-Za-zÑñÁáÉéÍíÓóÚú]+['\-]{0,1}[A-Za-zÑñÁáÉéÍíÓóÚú]+))*$/;
    return re.test(String(userName));
}

// Con eso hacemos las verificaciones de las entradas del usuario
const validateInputs = () => {
    let valido = true;

    const userNameValue= userName.value.trim();
    const emailValue= email.value.trim();
    const passwordValue= password.value.trim();
    const password2Value= password2.value.trim();
    const phoneValue= phone.value.trim();
    
    if (userNameValue === '') {
        setError(userName, 'El nombre de usuario es obligatorio');
        valido = false;
    }  else if (!isValidNombre(userNameValue)) {
        setError(userName, 'El nombre de usuario no es válido');
        valido = false;
    }
    else {
        setSuccess(userName);
    }

    if (emailValue === '') {
        setError(email, 'El correo electrónico es obligatorio');
        valido = false;
    } else if (!isValidEmail(emailValue)) {
        setError(email, 'El correo electrónico no es válido');
        valido = false;
    } else {
        setSuccess(email);
    }

    if (passwordValue === '') {
        setError(password, 'La contraseña es obligatoria');
        valido = false;
    } else if (passwordValue.length < 8) {
        setError(password, 'Debe tener al menos 8 caracteres');
        valido = false;
    } else {
        setSuccess(password);
    }

    if (phoneValue === '') {
        setError(phone, 'Ingrese un número telefónico');
        valido = false;
    } else if (!/^(6|2)\d{3}-\d{4}$/.test(phoneValue)) {
        setError(phone, 'El número telefónico no es válido');
        valido = false;
    } else {
        setSuccess(phone);
    }

    if (password2Value === '') {
        setError(password2, 'Por favor confirma tu contraseña');
        valido = false;
    } else if (password2Value !== passwordValue) {
        setError(password2, 'Las contraseñas no coinciden');
        valido = false;
    } else {
        setSuccess(password2);
    }

    return valido;
};


// Evento para el boton de registrarse que valida los inputs y llama a la funcion de registrar usuario
document.getElementById('registrarse').addEventListener('click', async function(e) {
    e.preventDefault();
    
    if (!validateInputs()) return;

    try {
        const res = await RegistrarUsuario();

        if (res.exito === false) {
            // Si no retorna una respuesta coloca error desconocido, sino toma el error que se envia
            const mensaje = res.resultado?.mensaje || "Error desconocido";

            console.error("Error del servidor:", mensaje);

            // Vacia los divs antes de volver a llenarlos
            document.getElementById("error-email").textContent = "";
            document.getElementById("error-phone").textContent = "";

            // Verifica si en la respuesta json que nos devuelve la api existe la palabra correo y muestra el mensaje.
            if (mensaje.toLowerCase().includes("correo")) {
                document.getElementById("error-email").textContent = "El correo electrónico ya está registrado.";
            } else if (mensaje.toLowerCase().includes("teléfono") || mensaje.toLowerCase().includes("telefono")) {
                document.getElementById("error-phone").textContent = "El número telefónico ya está registrado.";
            }
            return; 
        }
        // Registro correcto
        console.log("Registro correcto");
        window.location.href = "login.html";

    } catch(err) {
        console.error("Error en la petición:", err);

    }
});




