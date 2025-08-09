/*=============================================
		-- Author:		Guillermo Reyes
        -- Create date: 2025
        -- Description:	 Maneja las conexiones a origenes de datos SQL Server

=============================================*/

using MedicalCare.Models;
using System.Web;

namespace MedicalCare.Data
{
    public class ConexionesData
    {

        public static string Conexion()
        {
            string conexion = "Server=tcp:datencat-pt008.database.windows.net,1433;Initial Catalog=FichaClinica;Persist Security Info=False;User ID=candidato;Password=2VoXty56SNA5;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return conexion;
        }

        public static string ApiKey()
        {
            string APIKEY = "0f8000a9-b4b9-45e6-b5ad-6a4696c198b7";
            return APIKEY;
        }

    }

}

