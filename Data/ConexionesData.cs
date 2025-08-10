/*=============================================
    Author:         Guillermo Reyes
    Create date:    2025
    Description:    Maneja las conexiones a orígenes de datos SQL Server.
                    - Incluye (comentado) un ejemplo de conexión directa 
                      en el código. (motivo de cambio: secreto expuesto en github).
                    - El string de conexion y el apikey se leen 
                      desde appsettings.json
					- se realiza con un constructor para cargar y preparar el acceso a la config 
=============================================*/

using Microsoft.Extensions.Configuration;
using System.IO;
using MedicalCare.Models;

namespace MedicalCare.Data
{
    public static class ConexionesData
    {
        // Configuración cargada desde appsettings.json
        private static readonly IConfiguration _config;

        // Constructor estático que se ejecuta al primer uso de la clase
        static ConexionesData()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Cargar configuración
                .Build();
        }

        //cadena de conexión por nombre desde appsettings.json
        public static string Conexion()
        {
            return _config.GetConnectionString("DefaultConnection");
        }

        //API Key desde la sección ApiSettings de appsettings.json
        public static string ApiKey()
        {
            return _config["ApiSettings:ApiKey"];
        }

        //conexion directa con conection string desde clase
        //public static string Conexion()
        //{
        //    string conexion = "Server=tcp:xxxxxx-xxxx.database.windows.net,1433;Initial Catalog=xxxxxxxx;Persist Security Info=False;User ID=candidato;Password=***********;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //    return conexion;
        //}

        //public static string ApiKey()
        //{
        //    string APIKEY = "0f8000a9-b4b9-45e6-b5ad-6a4696c198b7";
        //    return APIKEY;
        //}
    }
}
