

// Variables para cada input que realice el usuario 
const form= document.getElementById('loginForm');
const email=document.getElementById('Useremail');
const password=document.getElementById('password');

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

// Con eso hacemos las verificaciones de las entradas del usuario
const validateInputs = () => {
    let valido = true;

    const emailValue= email.value.trim();
    const passwordValue= password.value.trim();

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

    return valido;
};



document.getElementById('iniciarSesion').addEventListener('click', async function(e) {
   
    if (!validateInputs()) return;

    try{ 
        const res= await iniciarsesion();
        
        if(res.exito==false){
            const mensaje = res.resultado?.mensaje|| "Error Desconocido";
            console.error("Error del servidor:",mensaje);

            // Vacias divs antes de volver a llenarlos
            document.getElementById("error-email").textContent = "";
            document.getElementById("error-contraseña").textContent = "";

            // Verificamos si en la respuesta json que nos devuelve el api existe la palabra usuario o contraseña
            if(mensaje.toLowerCase().includes("encontrado")){
                document.getElementById("error-email").textContent = "El usuario no existe.";
            } else if (mensaje.toLowerCase().includes("incorrecta")) {
                document.getElementById("error-contraseña").textContent = "Contraseña incorrecta.";
            }
            return; 
        }
    console.log ("Inicio de sesion correcto")

    // El signo de interrogacion es por si recibimos nulos, no lanza error la ejecucion
    // y asigna nulo
    const rol= res.resultado?.rol;
            if(rol === "Admin"){
            console.log("Usuario administrador");
            window.location.href = 'admin.html'; // página de admin
        } else if(rol === "Cliente"){
            console.log("Usuario cliente");
            window.location.href = 'index.html'; // página de cliente
        }
    }catch(err){
        console.error("Error en la petición:", err);
    }
});



