/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Metodo de validacion de rut existente, en una capa de Servicios
    --               separando las responsabilidades (capa negocio)
=============================================*/

using System;
using System.Collections.Generic;
using MedicalCare.Data;
using MedicalCare.Models;

namespace MedicalCare.Services
{
    public class PatientService
    {
        public bool RutExists(string rut, int? excludePatientId = null)
        {
            return PatientsData.RutExists(rut, excludePatientId);
        }

        //mas reglas de negocio
    }
}
