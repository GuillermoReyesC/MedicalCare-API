/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Modelo de datos para la tabla Speciality
=============================================*/

using System.Text.Json.Serialization;

namespace MedicalCare.Models
{
    public class SpecialityModel
    {
        [JsonIgnore]
        public int Speciality_Id { get; set; }
        public string Speciality_Name { get; set; } = string.Empty;
        public string? Speciality_Description { get; set; }
        public string Speciality_CreatedBy { get; set; } = string.Empty;
        public DateTime Speciality_CreatedAt { get; set; }
        public string? Speciality_ModifiedBy { get; set; }
        public DateTime? Speciality_ModifiedAt { get; set; }
    }
}
