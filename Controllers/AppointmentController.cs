/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Controller API REST para CRUD de citas (Appointments), y las busquedas relacionadas.

=============================================*/

using Microsoft.AspNetCore.Mvc;
using MedicalCare.Models;
using MedicalCare.Data;
using MedicalCare.Services;
using MedicalCare.Filters;

namespace MedicalCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKeyAuth]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        // DI
        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/appointment
        [HttpGet]
        public IActionResult GetAll()
        {
            var appointments = AppointmentData.GetAllAppointments();

            if (appointments == null || !appointments.Any())
                return Ok(new { code = 204, message = "No hay citas registradas." });

            return Ok(appointments);
        }

        // GET: api/appointment/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var appointment = AppointmentData.GetAppointmentById(id);

            if (appointment == null)
                return NotFound(new { code = 404, message = "Cita no encontrada." });

            return Ok(appointment);
        }


        // POST: api/appointment
        [HttpPost]
        public IActionResult Insert([FromBody] AppointmentModel appointment)
        {
            if (appointment == null)
                return BadRequest(new { code = 400, message = "Datos de cita inválidos." });
            //uso de metodo inyectado para verificr disponibilidad
            var (isAvailable, message) = _appointmentService.CheckAvailability(
                appointment.Appointment_StartUtc,
                appointment.Appointment_EndUtc,
                appointment.DoctorId,
                appointment.PatientId);

            if (!isAvailable)
                return Conflict(new { code = 409, message });

            // Ajuste: recibimos la tupla que retorna InsertAppointment
            var (success, insertMessage, newId) = AppointmentData.InsertAppointment(appointment);

            if (!success || newId == null || newId <= 0)
                return StatusCode(500, new { code = 500, message = insertMessage ?? "Error al crear la cita." });

            return Ok(new
            {
                id = newId,
                message = insertMessage ?? $"Cita creada exitosamente con ID: {newId}"
            });
        }

        // PUT: api/appointment/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AppointmentModel appointment)
        {
            if (appointment == null)
                return BadRequest(new { code = 400, message = "Datos inválidos o ID inconsistente." });
            //uso de metodo inyectado para verificr disponibilidad
            var (isAvailable, message) = _appointmentService.CheckAvailability(
                appointment.Appointment_StartUtc,
                appointment.Appointment_EndUtc,
                appointment.DoctorId,
                appointment.PatientId);

            if (!isAvailable)
                return Conflict(new { code = 409, message });

            // si esta disponible continuamos y agregamos el di el modelo
            appointment.Appointment_Id = id;
            // Ajuste: recibimos la tupla que retorna UpdateAppointment
            var (success, updateMessage) = AppointmentData.UpdateAppointment(appointment);

            if (success)
                return Ok(new { message = updateMessage ?? "Datos de la cita actualizados correctamente" });

            return NotFound(new { code = 404, message = updateMessage ?? "No se encontró la cita a actualizar" });
        }


        // DELETE: api/appointment/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = AppointmentData.DeleteAppointment(id);

            if (deleted)
                return Ok(new { message = "Cita eliminada correctamente" });

            return NotFound(new { code = 404, message = "No se encontró la cita a eliminar" });
        }

        // ----------------- CONSULTAS -----------------

        // 1. Por fecha
        [HttpGet("by-date/{date}")]
        public IActionResult GetByDate(DateTime date)
        {
            var result = AppointmentData.GetAppointmentsByDate(date);
            if (!result.Any())
                return Ok(new { code = 204, message = "No se encontraron citas para la fecha indicada." });

            return Ok(result);
        }


        // GET: api/appointment/by-date-range?start=2025-08-10T08:00:00&end=2025-08-10T18:00:00
        [HttpGet("by-date-range")]
        public IActionResult GetAppointmentsByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (start > end)
                return BadRequest(new { code = 400, message = "La fecha de inicio no puede ser mayor que la fecha de fin." });

            var appointments = AppointmentData.GetAppointmentsByDateRange(start, end);

            if (appointments == null || !appointments.Any())
                return Ok(new { code = 204, message = "No se encontraron citas en el rango de fechas especificado." });

            return Ok(appointments);
        }

        // 2. Por doctor (nombre o apellido)
        [HttpGet("by-doctor/{name}")]
        public IActionResult GetByDoctor(string name)
        {
            var result = AppointmentData.GetAppointmentsByDoctor(name);
            if (!result.Any())
                return Ok(new { code = 204, message = "No se encontraron citas para el doctor indicado." });

            return Ok(result);
        }

        // busuqeda por paciente (nombre, apellido o RUT)
        [HttpGet("by-patient/{search}")]
        public IActionResult GetByPatient(string search)
        {
            var result = AppointmentData.GetAppointmentsByPatient(search);
            if (!result.Any())
                return Ok(new { code = 204, message = "No se encontraron citas para el paciente indicado." });

            return Ok(result);
        }

        // busqueda pornombre de especialidad
        [HttpGet("by-specialty/{name}")]
        public IActionResult GetBySpecialty(string name)
        {
            var result = AppointmentData.GetAppointmentsBySpeciality(name);
            if (!result.Any())
                return Ok(new { code = 204, message = "No se encontraron citas para la especialidad indicada." });

            return Ok(result);
        }

        // Duración promedio por especialidad
        [HttpGet("average-duration-by-specialty")]
        public IActionResult GetAverageDurationBySpecialty()
        {
            var result = AppointmentData.GetAverageDurationBySpeciality();
            if (!result.Any())
                return Ok(new { code = 204, message = "No se encontraron datos de duración promedio." });

            return Ok(result.Select(r => new
            {
                Specialty = r.Speciality,
                AverageMinutes = r.AvgMinutes
            }));
        }
    }
}
