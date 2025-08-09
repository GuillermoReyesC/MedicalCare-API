/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Fltro de autorizacion de ApiKey
=============================================*/

using MedicalCare.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MedicalCare.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string APIKEY_HEADER_NAME = "X-API-KEY";

        // en prod configurar enn (appsettings.json)
        string APIKEY = ConexionesData.ApiKey();

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "API Key requerida" });
                return;
            }

            if (!APIKEY.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "API Key inválida" });
                return;
            }

            // Si pasa, continúa normalmente
        }
    }
}

