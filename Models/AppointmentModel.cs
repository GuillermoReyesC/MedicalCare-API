/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Modelo de datos para la tabla Appointment
=============================================*/


using System.Text.Json.Serialization;

namespace MedicalCare.Models
{
    public class AppointmentModel
    {
        [JsonIgnore]
        public int Appointment_Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime Appointment_StartUtc { get; set; }
        public DateTime Appointment_EndUtc { get; set; }

        public string Appointment_Diagnosis { get; set; }
        public string Appointment_Room { get; set; }
        [JsonIgnore]
        public string? Appointment_Status { get; set; }

        public string? Appointment_CreatedBy { get; set; }
        public DateTime? Appointment_CreatedAt { get; set; }

        public string? Appointment_ModifiedBy { get; set; }
        public DateTime? Appointment_ModifiedAt { get; set; }

        //datos de doctor y paciente
        [JsonIgnore]
        public string? Patient_FirstName { get; set; }
        [JsonIgnore]
        public string? Patient_LastName { get; set; }
        [JsonIgnore]
        public string? Doctor_FirstName { get; set; }
        [JsonIgnore]
        public string? Doctor_LastName { get; set; }
    }

    // Para respuestas GET (output)
    public class AppointmentResponseModel : AppointmentModel
    {
        public int Appointment_Id { get; set; }
        public string? Patient_FirstName { get; set; }
        public string? Patient_LastName { get; set; }
        public string? Patient_Rut { get; set; }
        public string? Doctor_FirstName { get; set; }
        public string? Doctor_LastName { get; set; }
        public string? Appointment_Status { get; set; }
    }
}
