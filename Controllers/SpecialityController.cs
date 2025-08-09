/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Controller CRUD para Specialities
=============================================*/

using Microsoft.AspNetCore.Mvc;
using MedicalCare.Data;
using MedicalCare.Models;
using MedicalCare.Filters;

namespace MedicalCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class SpecialityController : ControllerBase
    {
        // GET: api/specialities
        [HttpGet]
        public IActionResult GetAll()
        {
            var specialities = SpecialityData.GetAllSpecialities();
            return Ok(specialities);
        }

        // GET: api/specialities/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var speciality = SpecialityData.GetSpecialityById(id);
            if (speciality == null)
                return NotFound(new { code = 404, message = "No se encontró la especialidad" });

            return Ok(speciality);
        }

        // POST: api/specialities
        [HttpPost]
        public IActionResult Insert([FromBody] SpecialityModel speciality)
        {
            int idInserted = SpecialityData.InsertSpeciality(speciality);

            if (idInserted > 0)
            {
                return Ok(new
                {
                    id = idInserted,
                    message = $"Especialidad '{speciality.Speciality_Name}' registrada con ID: {idInserted}"
                });
            }

            return BadRequest(new { code = 500, message = "No se pudo insertar la especialidad" });
        }

        // PUT: api/specialities/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SpecialityModel speciality)
        {
            speciality.Speciality_Id = id;

            bool updated = SpecialityData.UpdateSpeciality(speciality);

            if (updated)
                return Ok(new { message = "Datos de la especialidad actualizados correctamente" });

            return NotFound(new { code = 404, message = "No se encontró la especialidad a actualizar" });
        }

        // DELETE: api/specialities/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = SpecialityData.DeleteSpeciality(id);

            if (deleted)
                return Ok(new { message = "Especialidad eliminada correctamente" });

            return NotFound(new { code = 404, message = "No se encontró la especialidad a eliminar" });
        }
    }
}
