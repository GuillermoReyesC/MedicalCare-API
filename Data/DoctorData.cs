/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  Operaciones CRUD para la entidad Doctor
=============================================*/

using System.Data;
using Microsoft.Data.SqlClient;

using MedicalCare.Models;

namespace MedicalCare.Data
{
    public class DoctorData
    {

        //create doctor method - retorna el ID insertado o 0 si falla
        public static int InsertDoctor(DoctorModel doctor)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
                conn.Open();

                // Insertar especialidad y obtener su ID generado automáticamente
                string insertSpecialityQuery = @"
                            INSERT INTO Speciality (
                                Speciality_Name, Speciality_Description,
                                Speciality_CreatedBy, Speciality_CreatedAt,
                                Speciality_ModifiedBy, Speciality_ModifiedAt
                            ) VALUES (
                                @Name, @Description,
                                @CreatedBy, @CreatedAt,
                                @ModifiedBy, @ModifiedAt
                            );
                            SELECT CAST(SCOPE_IDENTITY() AS int);";

                int specialityId;

                using (SqlCommand insertSpecialityCmd = new SqlCommand(insertSpecialityQuery, conn))
                {
                    insertSpecialityCmd.Parameters.AddWithValue("@Name", doctor.Speciality_Name ?? (object)DBNull.Value);
                    insertSpecialityCmd.Parameters.AddWithValue("@Description", doctor.Speciality_Description ?? (object)DBNull.Value);
                    insertSpecialityCmd.Parameters.AddWithValue("@CreatedBy", doctor.Doctor_CreatedBy ?? (object)DBNull.Value);
                    insertSpecialityCmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                    insertSpecialityCmd.Parameters.AddWithValue("@ModifiedBy", doctor.Doctor_ModifiedBy ?? (object)DBNull.Value);
                    insertSpecialityCmd.Parameters.AddWithValue("@ModifiedAt", doctor.Doctor_ModifiedAt ?? (object)DBNull.Value);

                    specialityId = (int)insertSpecialityCmd.ExecuteScalar();
                }

                // Insertar doctor con el specialityId recién creado
                string insertDoctorQuery = @"
                            INSERT INTO Doctor (
                                Doctor_FirstName, Doctor_LastName, Doctor_Email,
                                Doctor_Phone, Doctor_LicenseNumber, SpecialityId,
                                Doctor_CreatedBy, Doctor_CreatedAt,
                                Doctor_ModifiedBy, Doctor_ModifiedAt
                            ) VALUES (
                                @FirstName, @LastName, @Email,
                                @Phone, @LicenseNumber, @SpecialityId,
                                @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt
                            );
                            SELECT CAST(SCOPE_IDENTITY() AS int);";

                using SqlCommand insertDoctorCmd = new SqlCommand(insertDoctorQuery, conn);
                insertDoctorCmd.Parameters.AddWithValue("@FirstName", doctor.Doctor_FirstName);
                insertDoctorCmd.Parameters.AddWithValue("@LastName", doctor.Doctor_LastName);
                insertDoctorCmd.Parameters.AddWithValue("@Email", doctor.Doctor_Email ?? (object)DBNull.Value);
                insertDoctorCmd.Parameters.AddWithValue("@Phone", doctor.Doctor_Phone ?? (object)DBNull.Value);
                insertDoctorCmd.Parameters.AddWithValue("@LicenseNumber", doctor.Doctor_LicenseNumber);
                insertDoctorCmd.Parameters.AddWithValue("@SpecialityId", specialityId);
                insertDoctorCmd.Parameters.AddWithValue("@CreatedBy", doctor.Doctor_CreatedBy);
                insertDoctorCmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                insertDoctorCmd.Parameters.AddWithValue("@ModifiedBy", doctor.Doctor_ModifiedBy ?? (object)DBNull.Value);
                insertDoctorCmd.Parameters.AddWithValue("@ModifiedAt", doctor.Doctor_ModifiedAt ?? (object)DBNull.Value);

                object result = insertDoctorCmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar doctor: {ex.Message}");
                return 0;
            }
        }
        // Read Doctors list (getAll))
        public static List<DoctorModel> GetAllDoctors()
        {
            List<DoctorModel> list = new();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionesData.Conexion()))
                {
                    string query = @"
                        SELECT 
                            d.Doctor_Id,
                            d.Doctor_FirstName,
                            d.Doctor_LastName,
                            d.Doctor_Email,
                            d.Doctor_Phone,
                            d.Doctor_LicenseNumber,
                            d.SpecialityId,
                            d.Doctor_CreatedBy,
                            d.Doctor_CreatedAt,
                            d.Doctor_ModifiedBy,
                            d.Doctor_ModifiedAt,
                            s.Speciality_Name,
                            s.Speciality_Description
                        FROM Doctor d
                        LEFT JOIN Speciality s ON d.SpecialityId = s.Speciality_Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    using SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new DoctorModel
                        {
                            Doctor_Id = (int)reader["Doctor_Id"],
                            Doctor_FirstName = reader["Doctor_FirstName"].ToString(),
                            Doctor_LastName = reader["Doctor_LastName"].ToString(),
                            Doctor_Email = reader["Doctor_Email"] is DBNull ? null : reader["Doctor_Email"].ToString(),
                            Doctor_Phone = reader["Doctor_Phone"] is DBNull ? null : reader["Doctor_Phone"].ToString(),
                            Doctor_LicenseNumber = reader["Doctor_LicenseNumber"].ToString(),
                            Speciality_Id = (int)reader["SpecialityId"],
                            Speciality_Description = reader["Speciality_Description"] is DBNull ? null : reader["Speciality_Description"].ToString(),
                            Speciality_Name = reader["Speciality_Name"] is DBNull ? null : reader["Speciality_Name"].ToString(),
                            Doctor_CreatedBy = reader["Doctor_CreatedBy"].ToString(),
                            Doctor_CreatedAt = reader["Doctor_CreatedAt"].ToString(),
                            Doctor_ModifiedBy = reader["Doctor_ModifiedBy"] is DBNull ? null : reader["Doctor_ModifiedBy"].ToString(),
                            Doctor_ModifiedAt = reader["Doctor_ModifiedAt"] is DBNull ? null : reader["Doctor_ModifiedAt"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los doctores: {ex.Message}");
            }

            return list;
        }


        //Get Doctors by Id
        public static DoctorModel? GetDoctorById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionesData.Conexion()))
                {
                    string query = @"
                        SELECT 
                            d.Doctor_Id,
                            d.Doctor_FirstName,
                            d.Doctor_LastName,
                            d.Doctor_Email,
                            d.Doctor_Phone,
                            d.Doctor_LicenseNumber,
                            d.SpecialityId,
                            d.Doctor_CreatedBy,
                            d.Doctor_CreatedAt,
                            d.Doctor_ModifiedBy,
                            d.Doctor_ModifiedAt,
                            s.Speciality_Name,
                            s.Speciality_Description
                        FROM Doctor d
                        LEFT JOIN Speciality s ON d.SpecialityId = s.Speciality_Id
                        WHERE d.Doctor_Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new DoctorModel
                        {
                            Doctor_Id = (int)reader["Doctor_Id"],
                            Doctor_FirstName = reader["Doctor_FirstName"].ToString(),
                            Doctor_LastName = reader["Doctor_LastName"].ToString(),
                            Doctor_Email = reader["Doctor_Email"] is DBNull ? null : reader["Doctor_Email"].ToString(),
                            Doctor_Phone = reader["Doctor_Phone"] is DBNull ? null : reader["Doctor_Phone"].ToString(),
                            Doctor_LicenseNumber = reader["Doctor_LicenseNumber"].ToString(),
                            Speciality_Id = (int)reader["SpecialityId"],
                            Speciality_Name = reader["Speciality_Name"] is DBNull ? null : reader["Speciality_Name"].ToString(),
                            Speciality_Description = reader["Speciality_Description"] is DBNull ? null : reader["Speciality_Description"].ToString(),
                            Doctor_CreatedBy = reader["Doctor_CreatedBy"].ToString(),
                            Doctor_CreatedAt = reader["Doctor_CreatedAt"].ToString(),
                            Doctor_ModifiedBy = reader["Doctor_ModifiedBy"] is DBNull ? null : reader["Doctor_ModifiedBy"].ToString(),
                            Doctor_ModifiedAt = reader["Doctor_ModifiedAt"] is DBNull ? null : reader["Doctor_ModifiedAt"].ToString()
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


        //Update doctor Data
        public static bool UpdateDoctor(DoctorModel doctor)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionesData.Conexion()))
                {
                    conn.Open();

                    //Verificar existencia del doctor antes de llamar al SP
                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(1) FROM Doctor WHERE Doctor_Id = @DoctorId", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@DoctorId", doctor.Doctor_Id);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (exists == 0)
                            return false; // No existe, termina la funcion
                    }

                    //Ejecutar el procedimiento almacenado
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateDoctor", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@DoctorId", doctor.Doctor_Id);
                        cmd.Parameters.AddWithValue("@Doctor_FirstName", (object?)doctor.Doctor_FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Doctor_LastName", (object?)doctor.Doctor_LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Doctor_Email", (object?)doctor.Doctor_Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Doctor_Phone", (object?)doctor.Doctor_Phone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Doctor_LicenseNumber", (object?)doctor.Doctor_LicenseNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Speciality_Name", (object?)doctor.Speciality_Name ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Speciality_Description", (object?)doctor.Speciality_Description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Doctor_ModifiedBy", (object?)doctor.Doctor_ModifiedBy ?? DBNull.Value);

                        int result = Convert.ToInt32(cmd.ExecuteScalar());

                        return result == 1; //retorn 0 = falla en el sp
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar doctor: {ex.Message}");
                return false;
            }
        }


        //Delete Doctor

        //Nota : como recomendacion se sugiere trabajar con barridos lógicos de usuarios, es decir, env ez de eliminar directamente
        //       con DELETE From Where.., Se sugiere agregar un campo booleano que sea de nombre Active
        //       de ese modo seleccionamos users where Active = 1
        //       y para eliminar solamente el eliminamos demanera logica, cambiando el estado de 1 a 0 en Active para el usuario.
        //       se puede hacer, lo hubiese hecho, pero no sé si debia modificar el SQL.

        public static bool DeleteDoctor(int doctorId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionesData.Conexion()))
                {
                    string query = "DELETE FROM Doctor WHERE Doctor_Id = @DoctorId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar doctor: {ex.Message}");
                return false;
            }
        }
    }
}
