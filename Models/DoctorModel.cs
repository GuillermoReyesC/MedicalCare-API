/*=============================================
		-- Author:		Guillermo Reyes
        -- Create date: 2025
        -- Description:	 Clase Modelo para doctor

=============================================*/

using System.Text.Json.Serialization;

namespace MedicalCare.Models
{
    public class DoctorModel
    {
        [JsonIgnore]
        public int Doctor_Id { get; set; }
        public string Doctor_FirstName { get; set; }
        public string Doctor_LastName { get; set; }
        public string? Doctor_Email { get; set; }
        public string? Doctor_Phone { get; set; }
        public string Doctor_LicenseNumber { get; set; }
        public string Doctor_CreatedBy { get; set; }
        public string Doctor_CreatedAt { get; set; }
        public string? Doctor_ModifiedBy { get; set; }
        public string? Doctor_ModifiedAt { get; set; }

        //referencia especialidad
        [JsonIgnore]
        public int Speciality_Id { get; set; }
        public string Speciality_Name { get; set; }

        public string Speciality_Description { get; set; }



    }
}
