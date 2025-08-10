/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  CRUD básico para Appointments
    --               contiene el metodo que llama al SP de verificacion de disponibilidad de horarios             
=============================================*/
using MedicalCare.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace MedicalCare.Data
{
    public static class AppointmentData
    {
        // get all appointments
        public static List<AppointmentResponseModel> GetAllAppointments()
        {
            List<AppointmentResponseModel> list = new();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionesData.Conexion()))
                {
                    string query = @"
                SELECT 
                    a.Appointment_Id,
                    a.PatientId,
                    a.DoctorId,
                    a.Appointment_StartUtc,
                    a.Appointment_EndUtc,
                    a.Appointment_Diagnosis,
                    a.Appointment_Room,
                    a.Appointment_Status,
                    a.Appointment_CreatedBy,
                    a.Appointment_CreatedAt,
                    a.Appointment_ModifiedBy,
                    a.Appointment_ModifiedAt,
                    p.Patient_FirstName,
                    p.Patient_LastName,
                    p.Patient_RUT,
                    d.Doctor_FirstName,
                    d.Doctor_LastName
                FROM Appointment a
                INNER JOIN Patient p ON a.PatientId = p.Patient_Id
                INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    using SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new AppointmentResponseModel
                        {
                            Appointment_Id = (int)reader["Appointment_Id"],
                            PatientId = (int)reader["PatientId"],
                            DoctorId = (int)reader["DoctorId"],
                            Appointment_StartUtc = Convert.ToDateTime(reader["Appointment_StartUtc"]),
                            Appointment_EndUtc = Convert.ToDateTime(reader["Appointment_EndUtc"]),
                            Appointment_Diagnosis = reader["Appointment_Diagnosis"] is DBNull ? null : reader["Appointment_Diagnosis"].ToString(),
                            Appointment_Room = reader["Appointment_Room"] is DBNull ? null : reader["Appointment_Room"].ToString(),
                            Appointment_Status = reader["Appointment_Status"] is DBNull ? null : reader["Appointment_Status"].ToString(),
                            Appointment_CreatedBy = reader["Appointment_CreatedBy"].ToString(),
                            Appointment_CreatedAt = Convert.ToDateTime(reader["Appointment_CreatedAt"]),
                            Appointment_ModifiedBy = reader["Appointment_ModifiedBy"] is DBNull ? null : reader["Appointment_ModifiedBy"].ToString(),
                            Appointment_ModifiedAt = reader["Appointment_ModifiedAt"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["Appointment_ModifiedAt"]),
                            Patient_FirstName = reader["Patient_FirstName"].ToString(),
                            Patient_LastName = reader["Patient_LastName"].ToString(),
                            Patient_Rut = reader["Patient_RUT"].ToString(),
                            Doctor_FirstName = reader["Doctor_FirstName"].ToString(),
                            Doctor_LastName = reader["Doctor_LastName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las citas: {ex.Message}");
            }

            return list;
        }

        public static AppointmentResponseModel? GetAppointmentById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionesData.Conexion()))
                {
                    string query = @"
                SELECT 
                    a.Appointment_Id,
                    a.PatientId,
                    a.DoctorId,
                    a.Appointment_StartUtc,
                    a.Appointment_EndUtc,
                    a.Appointment_Diagnosis,
                    a.Appointment_Room,
                    a.Appointment_Status,
                    a.Appointment_CreatedBy,
                    a.Appointment_CreatedAt,
                    a.Appointment_ModifiedBy,
                    a.Appointment_ModifiedAt,
                    p.Patient_FirstName,
                    p.Patient_LastName,
                    p.Patient_RUT,
                    d.Doctor_FirstName,
                    d.Doctor_LastName
                FROM Appointment a
                INNER JOIN Patient p ON a.PatientId = p.Patient_Id
                INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
                WHERE a.Appointment_Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new AppointmentResponseModel
                        {
                            Appointment_Id = (int)reader["Appointment_Id"],
                            PatientId = (int)reader["PatientId"],
                            DoctorId = (int)reader["DoctorId"],
                            Appointment_StartUtc = Convert.ToDateTime(reader["Appointment_StartUtc"]),
                            Appointment_EndUtc = Convert.ToDateTime(reader["Appointment_EndUtc"]),
                            Appointment_Diagnosis = reader["Appointment_Diagnosis"] is DBNull ? null : reader["Appointment_Diagnosis"].ToString(),
                            Appointment_Room = reader["Appointment_Room"] is DBNull ? null : reader["Appointment_Room"].ToString(),
                            Appointment_Status = reader["Appointment_Status"] is DBNull ? null : reader["Appointment_Status"].ToString(),
                            Appointment_CreatedBy = reader["Appointment_CreatedBy"].ToString(),
                            Appointment_CreatedAt = Convert.ToDateTime(reader["Appointment_CreatedAt"]),
                            Appointment_ModifiedBy = reader["Appointment_ModifiedBy"] is DBNull ? null : reader["Appointment_ModifiedBy"].ToString(),
                            Appointment_ModifiedAt = reader["Appointment_ModifiedAt"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["Appointment_ModifiedAt"]),
                            Patient_FirstName = reader["Patient_FirstName"].ToString(),
                            Patient_LastName = reader["Patient_LastName"].ToString(),
                            Patient_Rut = reader["Patient_RUT"].ToString(),
                            Doctor_FirstName = reader["Doctor_FirstName"].ToString(),
                            Doctor_LastName = reader["Doctor_LastName"].ToString()
                        };
                    }

                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


        //insert new appointment
        public static (bool Success, string Message, int? NewId) InsertAppointment(AppointmentModel appointment)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                string query = @"
                        INSERT INTO Appointment 
                        (PatientId, DoctorId, Appointment_StartUtc, Appointment_EndUtc, Appointment_Diagnosis, Appointment_Room, Appointment_Status, Appointment_CreatedBy, Appointment_CreatedAt) 
                        VALUES 
                        (@PatientId, @DoctorId, @StartUtc, @EndUtc, @Diagnosis, @Room, @Appointment_Status, @CreatedBy, @CreatedAt);
                        SELECT CAST(scope_identity() AS int);";

                // Asignar valores por defecto si vienen vacíos o nulos
                string createdBy = string.IsNullOrWhiteSpace(appointment.Appointment_CreatedBy) ? "admin" : appointment.Appointment_CreatedBy;
                DateTime createdAt = appointment.Appointment_CreatedAt.HasValue && appointment.Appointment_CreatedAt.Value != default(DateTime)
                                   ? appointment.Appointment_CreatedAt.Value
                                   : DateTime.UtcNow;

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cmd.Parameters.AddWithValue("@StartUtc", appointment.Appointment_StartUtc);
                cmd.Parameters.AddWithValue("@EndUtc", appointment.Appointment_EndUtc);
                cmd.Parameters.AddWithValue("@Diagnosis", (object?)appointment.Appointment_Diagnosis ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Room", (object?)appointment.Appointment_Room ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Appointment_Status", "Scheduled");  // status de agendado
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmd.Parameters.AddWithValue("@CreatedAt", createdAt);

                conn.Open();
                int newId = (int)cmd.ExecuteScalar();

                return (true, "Cita creada correctamente.", newId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar cita: {ex.Message}");
                return (false, "Error al crear la cita.", null);
            }
        }

        //update an appointment
        public static (bool Success, string Message) UpdateAppointment(AppointmentModel appointment)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                string query = @"
                        UPDATE Appointment SET
                            PatientId = @PatientId,
                            DoctorId = @DoctorId,
                            Appointment_StartUtc = @StartUtc,
                            Appointment_EndUtc = @EndUtc,
                            Appointment_Diagnosis = @Diagnosis,
                            Appointment_Room = @Room,
                            Appointment_Status = @Appointment_Status,
                            Appointment_ModifiedBy = @ModifiedBy,
                            Appointment_ModifiedAt = @ModifiedAt
                        WHERE Appointment_Id = @AppointmentId";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cmd.Parameters.AddWithValue("@StartUtc", appointment.Appointment_StartUtc);
                cmd.Parameters.AddWithValue("@EndUtc", appointment.Appointment_EndUtc);
                cmd.Parameters.AddWithValue("@Diagnosis", (object?)appointment.Appointment_Diagnosis ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Room", (object?)appointment.Appointment_Room ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Appointment_Status", "Rescheduled");  //estatus de agendado
                cmd.Parameters.AddWithValue("@ModifiedBy", appointment.Appointment_ModifiedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ModifiedAt", appointment.Appointment_ModifiedAt ?? DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@AppointmentId", appointment.Appointment_Id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return (true, "Cita actualizada correctamente.");
                else
                    return (false, "No se encontró la cita a actualizar.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cita: {ex.Message}");
                return (false, "Error al actualizar la cita.");
            }
        }


        // delete appointment (by id)
        public static bool DeleteAppointment(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionesData.Conexion()))
                {
                    string query = "DELETE FROM Appointment WHERE Appointment_Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la cita: {ex.Message}");
                return false;
            }
        }

        //Checkeamos la disponibilidad con una tupa que ejecuta un SP y devuelve :
        //    -un estado bool isAvaliable
        //    -un mensaje de estado desde el sp
        //     - se usa desde el service como DI
        public static (bool IsAvailable, string Message) CheckAppointmentAvailability(DateTime startUtc, DateTime endUtc, int doctorId, int patientId)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());

                using SqlCommand cmd = new SqlCommand("sp_CheckAppointmentAvailability", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada para el stored procedure
                cmd.Parameters.AddWithValue("@StartUtc", startUtc);
                cmd.Parameters.AddWithValue("@EndUtc", endUtc);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@PatientId", patientId);

                conn.Open();

                // Ejecutar el SP y leer la respuesta con IsAvailable y Message
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Convertir los valores leídos en la tupla de resultado
                    bool isAvailable = Convert.ToBoolean(reader["IsAvailable"]);
                    string message = reader["Message"].ToString() ?? string.Empty;

                    return (isAvailable, message);
                }

                // En caso de que no retorne datos, asumimos que no está disponible por seguridad
                return (false, "No se pudo verificar la disponibilidad.");
            }
            catch (Exception ex)
            {
                // error y devolver respuesta indicando fallo
                Console.WriteLine($"Error verificando disponibilidad de cita: {ex.Message}");
                return (false, "Error interno al verificar disponibilidad.");
            }
        }


        // Consultar por fecha (solo día específico)
        public static List<AppointmentResponseModel> GetAppointmentsByDate(DateTime date)
        {
            var appointments = new List<AppointmentResponseModel>();
            using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
            string query = @"
                        SELECT a.*, p.Patient_FirstName, p.Patient_LastName, p.Patient_Rut,
                               d.Doctor_FirstName, d.Doctor_LastName, s.Speciality_Name
                        FROM Appointment a
                        INNER JOIN Patient p ON a.PatientId = p.Patient_Id
                        INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
                        INNER JOIN Speciality s ON d.SpecialityId = s.Speciality_Id
                        WHERE CAST(a.Appointment_StartUtc AS DATE) = @Date";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Date", date.Date);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                appointments.Add(MapAppointmentResponse(reader));
            }

            return appointments;
        }

        public static List<AppointmentResponseModel> GetAppointmentsByDateRange(DateTime start, DateTime end)
        {
            var appointments = new List<AppointmentResponseModel>();
            using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
            string query = @"
                        SELECT a.*, p.Patient_FirstName, p.Patient_LastName, p.Patient_Rut,
                               d.Doctor_FirstName, d.Doctor_LastName, s.Speciality_Name
                        FROM Appointment a
                        INNER JOIN Patient p ON a.PatientId = p.Patient_Id
                        INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
                        INNER JOIN Speciality s ON d.SpecialityId = s.Speciality_Id
                        WHERE a.Appointment_StartUtc >= @Start AND a.Appointment_EndUtc <= @End";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Start", start);
            cmd.Parameters.AddWithValue("@End", end);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                appointments.Add(MapAppointmentResponse(reader));
            }

            return appointments;
        }

        // Consultar por doctor (nombre o apellido)
        public static List<AppointmentResponseModel> GetAppointmentsByDoctor(string doctorName)
        {
            var appointments = new List<AppointmentResponseModel>();
            using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
            string query = @"
                        SELECT a.*, p.Patient_FirstName, p.Patient_LastName, p.Patient_Rut,
                               d.Doctor_FirstName, d.Doctor_LastName, s.Speciality_Name
                        FROM Appointment a
                        INNER JOIN Patient p ON a.PatientId = p.Patient_Id
                        INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
                        INNER JOIN Speciality s ON d.SpecialityId = s.Speciality_Id
                        WHERE (d.Doctor_FirstName LIKE @Name OR d.Doctor_LastName LIKE @Name)";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", $"%{doctorName}%");

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                appointments.Add(MapAppointmentResponse(reader));
            }

            return appointments;
        }

        // Consultar por paciente (nombre, apellido o RUT)
        public static List<AppointmentResponseModel> GetAppointmentsByPatient(string search)
        {
            var appointments = new List<AppointmentResponseModel>();
            using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
            string query = @"
                        SELECT a.*, p.Patient_FirstName, p.Patient_LastName, p.Patient_Rut,
                               d.Doctor_FirstName, d.Doctor_LastName, s.Speciality_Name
                        FROM Appointment a
                        INNER JOIN Patient p ON a.PatientId = p.Patient_Id
                        INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
                        INNER JOIN Speciality s ON d.SpecialityId = s.Speciality_Id
                        WHERE (p.Patient_FirstName LIKE @Search 
                               OR p.Patient_LastName LIKE @Search
                               OR p.Patient_Rut LIKE @Search)";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Search", $"%{search}%");

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                appointments.Add(MapAppointmentResponse(reader));
            }

            return appointments;
        }

        // Consultar por especialidad (nombre)
        public static List<AppointmentResponseModel> GetAppointmentsBySpeciality(string specialityName)
        {
            var appointments = new List<AppointmentResponseModel>();
            using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
            string query = @"
                        SELECT a.*, p.Patient_FirstName, p.Patient_LastName, p.Patient_Rut,
                               d.Doctor_FirstName, d.Doctor_LastName, s.Speciality_Name
                        FROM Appointment a
                        INNER JOIN Patient p ON a.PatientId = p.Patient_Id
                        INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
                        INNER JOIN Speciality s ON d.SpecialityId = s.Speciality_Id
                        WHERE s.Speciality_Name LIKE @SpecialityName";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SpecialityName", $"%{specialityName}%");

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                appointments.Add(MapAppointmentResponse(reader));
            }

            return appointments;
        }

        // Obtener duración promedio de atenciones por especialidad (queda igual)
        public static List<(string Speciality, double AvgMinutes)> GetAverageDurationBySpeciality()
        {
            var result = new List<(string, double)>();
            using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
            string query = @"
                        SELECT s.Speciality_Name,
                               AVG(DATEDIFF(MINUTE, a.Appointment_StartUtc, a.Appointment_EndUtc)) AS AvgMinutes
                        FROM Appointment a
                        INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
                        INNER JOIN Speciality s ON d.SpecialityId = s.Speciality_Id
                        WHERE a.Appointment_EndUtc IS NOT NULL
                        GROUP BY s.Speciality_Name";

            using SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string speciality = reader["Speciality_Name"].ToString()!;
                double avgMinutes = Convert.ToDouble(reader["AvgMinutes"]);
                result.Add((speciality, avgMinutes));
            }

            return result;
        }

        // Mapeo centralizado para AppointmentResponseModel
        private static AppointmentResponseModel MapAppointmentResponse(SqlDataReader reader)
        {
            return new AppointmentResponseModel
            {
                Appointment_Id = Convert.ToInt32(reader["Appointment_Id"]),
                PatientId = Convert.ToInt32(reader["PatientId"]),
                DoctorId = Convert.ToInt32(reader["DoctorId"]),
                Appointment_StartUtc = Convert.ToDateTime(reader["Appointment_StartUtc"]),
                Appointment_EndUtc = Convert.ToDateTime(reader["Appointment_EndUtc"]),
                Appointment_Diagnosis = reader["Appointment_Diagnosis"] as string,
                Appointment_Room = reader["Appointment_Room"] as string,
                Appointment_Status = reader["Appointment_Status"] as string,
                Appointment_CreatedBy = reader["Appointment_CreatedBy"] as string,
                Appointment_CreatedAt = reader["Appointment_CreatedAt"] as DateTime?,
                Appointment_ModifiedBy = reader["Appointment_ModifiedBy"] as string,
                Appointment_ModifiedAt = reader["Appointment_ModifiedAt"] as DateTime?,

                Patient_FirstName = reader["Patient_FirstName"] as string,
                Patient_LastName = reader["Patient_LastName"] as string,
                Patient_Rut = reader["Patient_Rut"] as string,

                Doctor_FirstName = reader["Doctor_FirstName"] as string,
                Doctor_LastName = reader["Doctor_LastName"] as string,
            };
        }

    }
}
