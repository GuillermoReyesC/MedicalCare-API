# MedicalCare
## Descripción

MedicalCare-API es una API REST desarrollada en .NET 8 con C# y SQL Server, diseñada para gestionar las atenciones médicas de pacientes en clínicas u hospitales. Este proyecto es para demostrar habilidades técnicas de desarrollo backend, enfocándose en buenas prácticas y arquitectura limpia.

## Características

- Se utiliza una arquitectura modular y separada por capas para una mayor separacion de responsabilidades, separando cada capa dependiendo el contenido ya sean controllers, Carpetas de clases relacionadas con SQL, lógica de negocios etc. permitiendo separar la capa de negocio de las demas capas propias del desarrollo.
- se separa la lógica de sentencias sql con la logica de negocios.
- se crean Endpoints RESTful para operaciones CRUD.
- si bien utilicé mayormente ADO.NET, tambien utilcé Dapper para manejar las operaciones de las especialidades.

- Se utiliza un servicio creado de Inyección de dependencias para validar la existencia previa de rut y para validar que las horas no se solapen con otras, tanto por doctor, y paciente sus horarios.
- se utiliza una tupla con el objtivo de poder manejar y devolver un mensaje y estado de disponibilidad de horarios.

- Se crean procedimientos Almacenados, uno para manejar la insercion de actualizacion  de Doctor (sp_UpdateDoctor), Se usa el procedimiento  porque primero revisamos si la especialidad cambia, si cambia, insertamos una nueva, cambia cuando se inserta nombre o descripcion. el paso siguiente inserta  los nuevos datos a actualizar.

- El otro Procedimiento almacenado se llama  (sp_CheckAppointmentAvailability),  se utiliza con el metodo en service para DI con el fin de  revisar disponibilidad de horario recibe fecha inicio, fecha fin, iddoctor, IdPaciente
es un procedimiento que funciona por pasos;
- si el doctor esta ocupado en ese horario, entrega un mensaje
- si el paciente tiene una hora asignada en ese horario, entrega otro mensaje
- cuando es correcto retorna 'Disponible'.

- Se agregan 10 minutos al inicio y al final por concepto de demora de paciente o por atraso de doctor.

- Se agrega autenticación por API Key a través de headers HTTP.


- Idealmente  me hubiese gustado hacer mas cosas, como cambiar el eliminado robusto por un barrido lógico, agregando estados e las entidades SQL, de ese modo sólo cambiamos estados de cada objeto. sin eliminar nada.
- Tambien me hubiese gustado hacer los demás metodos con Dapper, ya que los ice con ADO.net, por costumbre y facilidad, aunque ya pude ver los beneficios de Dapper.
- Hubiese hecho pruebas, pero por tiempo no pude. 



## Requisitos Técnicos

- C# (.NET 8)
- SQL Server (incluye script base)
- Visual Studio 2022

## Instrucciones

1. Descarga el proyecto del repositorio 'https://github.com/GuillermoReyesC/MedicalCare-API'
2. Configura el string de conexión a SQL Server con tu conectionString en la capa 'Data' en ConexionesData, actualmente está comentado el método directo.

El método actual utiliza el string de conexión y el apiKey desde appsettings.json.  Esto lo cambie de como se llamaba de la forma directa a trabajar desde appsettings.json por el aviso de github de un secreto expuesto en el repositorio.

3. IMPORTANTE. recuerda agregar tus credenciales de conexion, ya sea en ConexionesData y descomentando los metodos directos, o bien, agregando tus credenciales donde corresponden, en appsettings.json.

4. abre el archivo .sln dentro de la solución
5. ejecutalo y automaticamente estará Swagger listo para testear, adicional desde el archivo .http puedes hacer pruebas desde Visual Studio.


## Uso

Accede a los endpoints protegidos usando una API Key válida en el header `X-Api-Key`. puedes usar Swagger o bien Postman con la info del archivo .http

## Documentación

La documentación de la API está disponible en formato Swagger al iniciar el proyecto.
y tambien en un archivo  .http para tomar y probar

## Autor

Guillermo Reyes C
