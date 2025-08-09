/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Controller del API REST para CRUD doctores
=============================================*/

using Microsoft.AspNetCore.Mvc;
using MedicalCare.Models;
using MedicalCare.Data;
using MedicalCare.Filters;

namespace MedicalCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKeyAuth]
    public class DoctorController : ControllerBase
    {
        // GET: api/doctor
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = DoctorData.GetAllDoctors();

            if (result == null || !result.Any())
            {
                return Ok(new { code = 204, message = "Sin datos a mostrar" });
            }

            return Ok(result);
        }

        // GET: api/doctor/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var doctor = DoctorData.GetDoctorById(id);

            if (doctor == null)
                return NotFound(new { code = 404, message = "Doctor no encontrado" });

            return Ok(doctor);
        }

        // POST: api/doctor
        [HttpPost]
        public IActionResult Insert([FromBody] DoctorModel doctor)
        {
            int idInserted = (int)DoctorData.InsertDoctor(doctor);

            if (idInserted > 0)
            {
                return Ok(new
                {
                    id = idInserted,
                    message = $"Doctor {doctor.Doctor_FirstName} {doctor.Doctor_LastName}, con ID : {idInserted} ingresado exitosamente"
                });
            }

            return BadRequest(new { code = 500, message = "No se pudo insertar el doctor" });
        }


        // PUT: api/doctor/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DoctorModel doctor)
        {
            doctor.Doctor_Id = id;  // asignamos el id recibido en la ruta al modelo

            bool updated = DoctorData.UpdateDoctor(doctor);

            if (updated)
                return Ok(new { message = "Datos del Doctor actualizados correctamente" });
            else
                return NotFound(new { code = 404, message = "No se encontró el doctor a actualizar" });


        }

        // DELETE: api/doctor/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = DoctorData.DeleteDoctor(id);

            if (deleted)
                return Ok(new { message = "Doctor eliminado correctamente" });

            return NotFound(new { code = 404, message = "No se encontró el doctor a eliminar" });
        }
    }
}
