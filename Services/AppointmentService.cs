/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Metodo de verificacion de disponibilidad de citas
    --               se usa como inyeccion de dependencias en el controller    
=============================================*/

using MedicalCare.Data;

namespace MedicalCare.Services
{
    public class AppointmentService
    {
        // 
        public (bool IsAvailable, string Message) CheckAvailability(DateTime startUtc, DateTime endUtc, int doctorId, int patientId)
        {
            return AppointmentData.CheckAppointmentAvailability(startUtc, endUtc, doctorId, patientId);
        }
    }
}
