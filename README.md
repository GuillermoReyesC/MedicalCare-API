# MedicalCare
## Descripci�n

MedicalCare es una API REST desarrollada en .NET 8 con C# y SQL Server, dise�ada para gestionar las atenciones m�dicas de pacientes en cl�nicas u hospitales. Este proyecto demuestra habilidades t�cnicas, enfoc�ndose en buenas pr�cticas, arquitectura limpia y seguridad b�sica.

## Caracter�sticas

- se utiliza una arquitectura modular y separada por capas  para una mayor separacion de resposabilidades, separando cada capa dependiendo el contenido de las clases /controllers, etc. permitiendo separar la capa de negocio de las demas capas prpias del desarrollo.
- se separa la l�gica de sentencias sql con la logica de negocios
- - Endpoints RESTful para operaciones CRUD sobre atenciones m�dicas y pacientes.
-si bien utilic� mayormente ADO.NET, tambien utilc� Dapper para manejar las operaciones de las especialidades.

- Se utiliza un servicio creado de Inyecci�n de dependencias para validar la existencia previa de rut y para validar que las horas no se solapen con otras, tanto por doctor, y paciente sus horarios.
-se utiliza una tupla con el objtivo de poder manejar y devolver un mensaje y estado de disponibilidad de horarios.

-Se crean procedimientos Almacenados, uno para manejar la insercion de actualizacion  de Doctor (sp_UpdateDoctor), Se usa el procedimiento  porque primero revisamos si la especialidad cambia, si cambia, insertamos una nueva, cambia cuando se inserta nombre o descripcion. el paso siguiente inserta  los nuevos datos a actualizar.

--el otro Procedimiento almacenado se llama  sp_CheckAppointmentAvailability,  se utiliza con el metodo en service para DI con el fin de  revisar disponibilidad de horario recibe fecha inicio, fecha fin, iddoctor, IdPaciente
es un procedimiento que funciona por pasos;
si el doctor esta ocupado en ese horario, entrega un mensaje
si el paciente tiene una hora asignada en ese horario, entrega otro mensaje
cuando es correcto retorna 'Disponible'.

- se agregan 10 minutos al inicio y al final por concepto de demora de paciente o por atraso de doctor.

-Se agrega autenticaci�n por API Key a trav�s de headers HTTP.


- Idealmente  me hubiese gustado hacer mas cosas, como cambiar el eliminado robusto por un barrido l�gico, agregando estados e las entidades SQL, de ese modo s�lo cambiamos estados de cada objeto. sin eliminar nada.
- Tambien me hubiese gustado hacer los dem�s metodos con Dapper, ya que los ice con ADO.net, por costumbre y facilidad, aunque ya pude ver los beneficios de Dapper.
- Hubiese hecho pruebas, pero por tiempo no pude??. 



## Requisitos T�cnicos

- C# (.NET 8)
- SQL Server (incluye script base)
- Visual Studio 2022

## Instrucciones

1. Descarga el proyecto del repositorio
2. Configura la cadena de conexi�n a SQL Server en la capa 'Data' en ConexionesData, actualmente estan mis credenciales a la bd.
3. abre el archivo .sln dentro de la soluci�n
4. ejecutalo y automaticamente estar� Swagger listo para testear


## Uso

Accede a los endpoints protegidos usando una API Key v�lida en el header `X-Api-Key`. puedes usar Swagger o bien Postman con la info del archivo .http

## Documentaci�n

La documentaci�n de la API est� disponible en formato Swagger al iniciar el proyecto.
y tambien en un archivo  .http para tomar y probar

## Autor

Guillermo Reyes C
