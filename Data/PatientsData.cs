/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Operaciones CRUD para la entidad Paciente
=============================================*/
using System.Data;
using Microsoft.Data.SqlClient;

using MedicalCare.Models;

namespace MedicalCare.Data
{
    public class PatientsData
    {
        //insert new patient
        public static int InsertPatient(PatientModel patient)
        {
            try
            {
                // Validar que el RUT no se repita antes de insertar
                if (RutExists(patient.PatientRUT))
                {
                    Console.WriteLine($"El RUT {patient.PatientRUT} ya existe en la base de datos.");
                    return -1; // Indicador de que ya existe el RUT
                }

                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                conn.Open();

                string insertPatientQuery = @"
                        INSERT INTO Patient (
                            Patient_FirstName, Patient_LastName, Patient_RUT, Patient_DateOfBirth, Patient_Gender, Patient_Phone, Patient_Email,
                            Patient_AddressLine1, Patient_AddressLine2, Patient_City, Patient_State, Patient_PostalCode,
                            Patient_CreatedBy, Patient_CreatedAt
                        ) VALUES (
                            @FirstName, @LastName, @RUT, @DateOfBirth, @Gender, @Phone, @Email,
                            @AddressLine1, @AddressLine2, @City, @State, @PostalCode,
                            @CreatedBy, @CreatedAt
                        );
                        SELECT CAST(SCOPE_IDENTITY() AS int);";

                using SqlCommand cmd = new SqlCommand(insertPatientQuery, conn);

                cmd.Parameters.AddWithValue("@FirstName", patient.PatientFirstName);
                cmd.Parameters.AddWithValue("@LastName", patient.PatientLastName);
                cmd.Parameters.AddWithValue("@RUT", patient.PatientRUT);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.PatientDateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", patient.PatientGender.HasValue ? (object)patient.PatientGender.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", patient.PatientPhone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", patient.PatientEmail ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AddressLine1", patient.PatientAddressLine1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AddressLine2", patient.PatientAddressLine2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", patient.PatientCity ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@State", patient.PatientState ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PostalCode", patient.PatientPostalCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", patient.PatientCreatedBy);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar paciente: {ex.Message}");
                return 0;
            }
        }


        //get all patients
        public static List<PatientModel> GetAllPatients()
        {
            List<PatientModel> list = new();

            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                string query = @"
                    SELECT 
                        Patient_Id,
                        Patient_FirstName,
                        Patient_LastName,
                        Patient_RUT,
                        Patient_DateOfBirth,
                        Patient_Gender,
                        Patient_Phone,
                        Patient_Email,
                        Patient_AddressLine1,
                        Patient_AddressLine2,
                        Patient_City,
                        Patient_State,
                        Patient_PostalCode,
                        Patient_CreatedBy,
                        Patient_CreatedAt,
                        Patient_ModifiedBy,
                        Patient_ModifiedAt
                    FROM Patient";

                using SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new PatientModel
                    {
                        PatientId = (int)reader["Patient_Id"],
                        PatientFirstName = reader["Patient_FirstName"].ToString(),
                        PatientLastName = reader["Patient_LastName"].ToString(),
                        PatientRUT = reader["Patient_RUT"].ToString(),
                        PatientDateOfBirth = (DateTime)reader["Patient_DateOfBirth"],
                        PatientGender = reader["Patient_Gender"] is DBNull ? (char?)null : Convert.ToChar(reader["Patient_Gender"]),
                        PatientPhone = reader["Patient_Phone"] is DBNull ? null : reader["Patient_Phone"].ToString(),
                        PatientEmail = reader["Patient_Email"] is DBNull ? null : reader["Patient_Email"].ToString(),
                        PatientAddressLine1 = reader["Patient_AddressLine1"] is DBNull ? null : reader["Patient_AddressLine1"].ToString(),
                        PatientAddressLine2 = reader["Patient_AddressLine2"] is DBNull ? null : reader["Patient_AddressLine2"].ToString(),
                        PatientCity = reader["Patient_City"] is DBNull ? null : reader["Patient_City"].ToString(),
                        PatientState = reader["Patient_State"] is DBNull ? null : reader["Patient_State"].ToString(),
                        PatientPostalCode = reader["Patient_PostalCode"] is DBNull ? null : reader["Patient_PostalCode"].ToString(),
                        PatientCreatedBy = reader["Patient_CreatedBy"].ToString(),
                        PatientCreatedAt = (DateTime)reader["Patient_CreatedAt"],
                        PatientModifiedBy = reader["Patient_ModifiedBy"] is DBNull ? null : reader["Patient_ModifiedBy"].ToString(),
                        PatientModifiedAt = reader["Patient_ModifiedAt"] is DBNull ? (DateTime?)null : (DateTime)reader["Patient_ModifiedAt"]
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener pacientes: {ex.Message}");
            }

            return list;
        }

        // get patient by ID
        public static PatientModel? GetPatientById(int patientId)
        {
            PatientModel? patient = null;

            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                string query = @"
                    SELECT 
                        Patient_Id,
                        Patient_FirstName,
                        Patient_LastName,
                        Patient_RUT,
                        Patient_DateOfBirth,
                        Patient_Gender,
                        Patient_Phone,
                        Patient_Email,
                        Patient_AddressLine1,
                        Patient_AddressLine2,
                        Patient_City,
                        Patient_State,
                        Patient_PostalCode,
                        Patient_CreatedBy,
                        Patient_CreatedAt,
                        Patient_ModifiedBy,
                        Patient_ModifiedAt
                    FROM Patient
                    WHERE Patient_Id = @PatientId";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    patient = new PatientModel
                    {
                        PatientId = (int)reader["Patient_Id"],
                        PatientFirstName = reader["Patient_FirstName"].ToString(),
                        PatientLastName = reader["Patient_LastName"].ToString(),
                        PatientRUT = reader["Patient_RUT"].ToString(),
                        PatientDateOfBirth = (DateTime)reader["Patient_DateOfBirth"],
                        PatientGender = reader["Patient_Gender"] is DBNull ? (char?)null : Convert.ToChar(reader["Patient_Gender"]),
                        PatientPhone = reader["Patient_Phone"] is DBNull ? null : reader["Patient_Phone"].ToString(),
                        PatientEmail = reader["Patient_Email"] is DBNull ? null : reader["Patient_Email"].ToString(),
                        PatientAddressLine1 = reader["Patient_AddressLine1"] is DBNull ? null : reader["Patient_AddressLine1"].ToString(),
                        PatientAddressLine2 = reader["Patient_AddressLine2"] is DBNull ? null : reader["Patient_AddressLine2"].ToString(),
                        PatientCity = reader["Patient_City"] is DBNull ? null : reader["Patient_City"].ToString(),
                        PatientState = reader["Patient_State"] is DBNull ? null : reader["Patient_State"].ToString(),
                        PatientPostalCode = reader["Patient_PostalCode"] is DBNull ? null : reader["Patient_PostalCode"].ToString(),
                        PatientCreatedBy = reader["Patient_CreatedBy"].ToString(),
                        PatientCreatedAt = (DateTime)reader["Patient_CreatedAt"],
                        PatientModifiedBy = reader["Patient_ModifiedBy"] is DBNull ? null : reader["Patient_ModifiedBy"].ToString(),
                        PatientModifiedAt = reader["Patient_ModifiedAt"] is DBNull ? (DateTime?)null : (DateTime)reader["Patient_ModifiedAt"]
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente por ID: {ex.Message}");
            }

            return patient;
        }

        // update patient
        public static bool UpdatePatient(PatientModel patient)
        {
            // Validar que el RUT no se repita en otro paciente distinto al actual
            if (RutExists(patient.PatientRUT, patient.PatientId))
            {
                Console.WriteLine($"El RUT {patient.PatientRUT} ya existe en otro paciente.");
                return false; // Indicar duplicidad
            }

            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                conn.Open();

                string updateQuery = @"
                        UPDATE Patient SET
                            Patient_FirstName = @FirstName,
                            Patient_LastName = @LastName,
                            Patient_RUT = @RUT,
                            Patient_DateOfBirth = @DateOfBirth,
                            Patient_Gender = @Gender,
                            Patient_Phone = @Phone,
                            Patient_Email = @Email,
                            Patient_AddressLine1 = @AddressLine1,
                            Patient_AddressLine2 = @AddressLine2,
                            Patient_City = @City,
                            Patient_State = @State,
                            Patient_PostalCode = @PostalCode,
                            Patient_ModifiedBy = @ModifiedBy,
                            Patient_ModifiedAt = @ModifiedAt
                        WHERE Patient_Id = @PatientId";

                using SqlCommand cmd = new SqlCommand(updateQuery, conn);

                cmd.Parameters.AddWithValue("@FirstName", patient.PatientFirstName);
                cmd.Parameters.AddWithValue("@LastName", patient.PatientLastName);
                cmd.Parameters.AddWithValue("@RUT", patient.PatientRUT);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.PatientDateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", patient.PatientGender.HasValue ? (object)patient.PatientGender.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", patient.PatientPhone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", patient.PatientEmail ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AddressLine1", patient.PatientAddressLine1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AddressLine2", patient.PatientAddressLine2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", patient.PatientCity ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@State", patient.PatientState ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PostalCode", patient.PatientPostalCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ModifiedBy", patient.PatientModifiedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ModifiedAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar paciente: {ex.Message}");
                return false;
            }
        }
        // delete patient
        public static bool DeletePatient(int patientId)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                conn.Open();

                string deleteQuery = "DELETE FROM Patient WHERE Patient_Id = @PatientId";

                using SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@PatientId", patientId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar paciente: {ex.Message}");
                return false;
            }
        }


        //serch patient 
        public static List<PatientModel> SearchPatients(string? rut = null, string? name = null, DateTime? birthDateFrom = null, DateTime? birthDateTo = null)
        {
            List<PatientModel> list = new();

            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());

                // Construcción dinámica de la consulta con condiciones opcionales
                string query = @"
                        SELECT 
                            Patient_Id,
                            Patient_FirstName,
                            Patient_LastName,
                            Patient_RUT,
                            Patient_DateOfBirth,
                            Patient_Gender,
                            Patient_Phone,
                            Patient_Email,
                            Patient_AddressLine1,
                            Patient_AddressLine2,
                            Patient_City,
                            Patient_State,
                            Patient_PostalCode,
                            Patient_CreatedBy,
                            Patient_CreatedAt,
                            Patient_ModifiedBy,
                            Patient_ModifiedAt
                        FROM Patient
                        WHERE 1=1";

                // Lista para parámetros dinámicos
                List<SqlParameter> parameters = new();

                if (!string.IsNullOrWhiteSpace(rut))
                {
                    query += " AND Patient_RUT LIKE @RUT";
                    parameters.Add(new SqlParameter("@RUT", rut.Trim() + "%"));
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query += " AND (Patient_FirstName LIKE @Name OR Patient_LastName LIKE @Name)";
                    parameters.Add(new SqlParameter("@Name", "%" + name.Trim() + "%"));
                }

                if (birthDateFrom.HasValue)
                {
                    query += " AND Patient_DateOfBirth >= @BirthDateFrom";
                    parameters.Add(new SqlParameter("@BirthDateFrom", birthDateFrom.Value.Date));
                }

                if (birthDateTo.HasValue)
                {
                    query += " AND Patient_DateOfBirth <= @BirthDateTo";
                    parameters.Add(new SqlParameter("@BirthDateTo", birthDateTo.Value.Date));
                }

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new PatientModel
                    {
                        PatientId = (int)reader["Patient_Id"],
                        PatientFirstName = reader["Patient_FirstName"].ToString(),
                        PatientLastName = reader["Patient_LastName"].ToString(),
                        PatientRUT = reader["Patient_RUT"].ToString(),
                        PatientDateOfBirth = (DateTime)reader["Patient_DateOfBirth"],
                        PatientGender = reader["Patient_Gender"] is DBNull ? (char?)null : Convert.ToChar(reader["Patient_Gender"]),
                        PatientPhone = reader["Patient_Phone"] is DBNull ? null : reader["Patient_Phone"].ToString(),
                        PatientEmail = reader["Patient_Email"] is DBNull ? null : reader["Patient_Email"].ToString(),
                        PatientAddressLine1 = reader["Patient_AddressLine1"] is DBNull ? null : reader["Patient_AddressLine1"].ToString(),
                        PatientAddressLine2 = reader["Patient_AddressLine2"] is DBNull ? null : reader["Patient_AddressLine2"].ToString(),
                        PatientCity = reader["Patient_City"] is DBNull ? null : reader["Patient_City"].ToString(),
                        PatientState = reader["Patient_State"] is DBNull ? null : reader["Patient_State"].ToString(),
                        PatientPostalCode = reader["Patient_PostalCode"] is DBNull ? null : reader["Patient_PostalCode"].ToString(),
                        PatientCreatedBy = reader["Patient_CreatedBy"].ToString(),
                        PatientCreatedAt = (DateTime)reader["Patient_CreatedAt"],
                        PatientModifiedBy = reader["Patient_ModifiedBy"] is DBNull ? null : reader["Patient_ModifiedBy"].ToString(),
                        PatientModifiedAt = reader["Patient_ModifiedAt"] is DBNull ? (DateTime?)null : (DateTime)reader["Patient_ModifiedAt"]
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar pacientes: {ex.Message}");
            }

            return list;
        }



        //metodo para verificar si el rut a ingresar existe
        public static bool RutExists(string rut, int? excludePatientId = null)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                string query = "SELECT COUNT(1) FROM Patient WHERE Patient_RUT = @RUT";

                if (excludePatientId.HasValue)
                    query += " AND Patient_Id <> @ExcludeId";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RUT", rut);
                if (excludePatientId.HasValue)
                    cmd.Parameters.AddWithValue("@ExcludeId", excludePatientId.Value);

                conn.Open();

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validando RUT: {ex.Message}");
                return false;
            }
        }
    }
}
