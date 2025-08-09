/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  controller CRUD patients
=============================================*/

using Microsoft.AspNetCore.Mvc;
using MedicalCare.Data;
using MedicalCare.Models;
using MedicalCare.Services;
using MedicalCare.Filters;

namespace MedicalCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _patientService;

        // Inyección de dependencia vía constructor
        public PatientsController(PatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patients
        [HttpGet]
        public IActionResult GetAll()
        {
            var patients = PatientsData.GetAllPatients();


            if (patients == null || !patients.Any())
            {
                return Ok(new { code = 204, message = "Sin datos a mostrar" });
            }

            return Ok(patients);
        }

        // GET: api/patients/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var patient = PatientsData.GetPatientById(id);
            if (patient == null)
                return NotFound($"Paciente con Id {id} no encontrado.");

            return Ok(patient);
        }

        // POST: api/patients
        [HttpPost]
        public IActionResult Insert([FromBody] PatientModel patient)
        {
            if (patient == null)
                return BadRequest(new { code = 400, message = "Datos del paciente inválidos." });

            // Validar existencia de RUT vía servicio DI
            if (_patientService.RutExists(patient.PatientRUT))
                return Conflict(new { code = 409, message = $"El RUT {patient.PatientRUT} ya existe en la base de datos." });

            int newPatientId = PatientsData.InsertPatient(patient);

            if (newPatientId <= 0)
                return StatusCode(500, new { code = 500, message = "Error al crear paciente." });

            patient.PatientId = newPatientId;

            return Ok(new
            {
                id = newPatientId,
                message = $"Paciente {patient.PatientFirstName} {patient.PatientLastName} ingresado exitosamente",
                patient
            });
        }

        // PUT: api/patients/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] PatientModel patient)
        {
            if (patient == null)
                return BadRequest(new { code = 400, message = "Datos del paciente inválidos o Id inconsistente." });

            // Validar RUT excluyendo paciente actual
            if (_patientService.RutExists(patient.PatientRUT, id))
                return Conflict(new { code = 409, message = $"El RUT {patient.PatientRUT} ya existe en otro paciente." });
            //asigno id
            patient.PatientId = id;

            bool updated = PatientsData.UpdatePatient(patient);

            if (!updated)
                return StatusCode(500, new { code = 500, message = "Error al actualizar paciente." });

            return Ok(new { message = "Datos del paciente actualizados correctamente" });
        }

        // DELETE: api/patients/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            bool deleted = PatientsData.DeletePatient(id);
            if (!deleted)
                return NotFound($"Paciente con Id {id} no encontrado o no pudo ser eliminado.");

            return Ok(new { message = "Paciente eliminado con éxito" });
        }

        // GET: api/patients/search
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? rut, [FromQuery] string? name, [FromQuery] DateTime? birthDateFrom, [FromQuery] DateTime? birthDateTo)
        {
            var results = PatientsData.SearchPatients(rut, name, birthDateFrom, birthDateTo);
            return Ok(results);
        }
    }
}
