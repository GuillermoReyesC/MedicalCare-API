/*=============================================
		-- Author:		Guillermo Reyes
        -- Create date: 2025
        -- Description:	 Clase Modelo para pacientes

=============================================*/
using System.Text.Json.Serialization;

namespace MedicalCare.Models
{
    public class PatientModel
    {
        [JsonIgnore]
        public int PatientId { get; set; }

        public string PatientFirstName { get; set; } = null!;

        public string PatientLastName { get; set; } = null!;

        public string PatientRUT { get; set; } = null!;

        public DateTime PatientDateOfBirth { get; set; }

        public char? PatientGender { get; set; }

        public string? PatientPhone { get; set; }

        public string? PatientEmail { get; set; }

        public string? PatientAddressLine1 { get; set; }

        public string? PatientAddressLine2 { get; set; }

        public string? PatientCity { get; set; }

        public string? PatientState { get; set; }

        public string? PatientPostalCode { get; set; }

        public string PatientCreatedBy { get; set; } = null!;

        public DateTime PatientCreatedAt { get; set; }

        public string? PatientModifiedBy { get; set; }

        public DateTime? PatientModifiedAt { get; set; }
    }
}
