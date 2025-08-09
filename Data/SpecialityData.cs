/*=============================================
    -- Author:       Guillermo Reyes
    -- Create date:  2025
    -- Description:  CRUD básico para la tabla Speciality
    --               Estos metodos que usamos en el controller de Specialities, esta desarrollado utilizando Dapper
    --               Se puede ver en la sintaxis mas sencilla, menos codigo y el mapeo automatico que esta bueno, desconocido para mi
    --               el codigo comentado resumido (ln.17) son los mismos metodos con ADO.NET, que suelo ocupar comunmente.
=============================================*/
using Dapper;
using MedicalCare.Models;
using Microsoft.Data.SqlClient;

namespace MedicalCare.Data
{
    public static class SpecialityData
    {
        //// insert speciality
        //public static int InsertSpeciality(SpecialityModel speciality)
        //{
        //    const string query = @"
        //            INSERT INTO Speciality 
        //            (Speciality_Name, Speciality_Description, Speciality_CreatedBy, Speciality_CreatedAt)
        //            OUTPUT INSERTED.Speciality_Id
        //            VALUES (@Name, @Description, @CreatedBy, @CreatedAt)";

        //    using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
        //    using SqlCommand cmd = new SqlCommand(query, conn);

        //    cmd.Parameters.AddWithValue("@Name", speciality.Speciality_Name);
        //    cmd.Parameters.AddWithValue("@Description", speciality.Speciality_Description ?? (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@CreatedBy", speciality.Speciality_CreatedBy);
        //    cmd.Parameters.AddWithValue("@CreatedAt", speciality.Speciality_CreatedAt);

        //    conn.Open();
        //    return (int)cmd.ExecuteScalar();
        //}

        //// get all specialities
        //public static List<SpecialityModel> GetAllSpecialities()
        //{
        //    var list = new List<SpecialityModel>();
        //    const string query = @"
        //        SELECT Speciality_Id, Speciality_Name, Speciality_Description, 
        //               Speciality_CreatedBy, Speciality_CreatedAt, 
        //               Speciality_ModifiedBy, Speciality_ModifiedAt
        //        FROM Speciality";

        //    using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
        //    using SqlCommand cmd = new SqlCommand(query, conn);

        //    conn.Open();
        //    using SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        list.Add(new SpecialityModel
        //        {
        //            Speciality_Id = reader.GetInt32(0),
        //            Speciality_Name = reader.GetString(1),
        //            Speciality_Description = reader.IsDBNull(2) ? null : reader.GetString(2),
        //            Speciality_CreatedBy = reader.GetString(3),
        //            Speciality_CreatedAt = reader.GetDateTime(4),
        //            Speciality_ModifiedBy = reader.IsDBNull(5) ? null : reader.GetString(5),
        //            Speciality_ModifiedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
        //        });
        //    }
        //    return list;
        //}

        //// get speciality by id
        //public static SpecialityModel? GetSpecialityById(int id)
        //{
        //    const string query = @"
        //        SELECT Speciality_Id, Speciality_Name, Speciality_Description, 
        //               Speciality_CreatedBy, Speciality_CreatedAt, 
        //               Speciality_ModifiedBy, Speciality_ModifiedAt
        //        FROM Speciality
        //        WHERE Speciality_Id = @Id";

        //    using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
        //    using SqlCommand cmd = new SqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("@Id", id);

        //    conn.Open();
        //    using SqlDataReader reader = cmd.ExecuteReader();
        //    if (reader.Read())
        //    {
        //        return new SpecialityModel
        //        {
        //            Speciality_Id = reader.GetInt32(0),
        //            Speciality_Name = reader.GetString(1),
        //            Speciality_Description = reader.IsDBNull(2) ? null : reader.GetString(2),
        //            Speciality_CreatedBy = reader.GetString(3),
        //            Speciality_CreatedAt = reader.GetDateTime(4),
        //            Speciality_ModifiedBy = reader.IsDBNull(5) ? null : reader.GetString(5),
        //            Speciality_ModifiedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
        //        };
        //    }
        //    return null;
        //}

        //// update speciality
        //public static bool UpdateSpeciality(SpecialityModel speciality)
        //{
        //    const string query = @"
        //        UPDATE Speciality
        //        SET Speciality_Name = @Name, 
        //            Speciality_Description = @Description,
        //            Speciality_ModifiedBy = @ModifiedBy,
        //            Speciality_ModifiedAt = @ModifiedAt
        //        WHERE Speciality_Id = @Id";

        //    using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
        //    using SqlCommand cmd = new SqlCommand(query, conn);

        //    cmd.Parameters.AddWithValue("@Name", speciality.Speciality_Name);
        //    cmd.Parameters.AddWithValue("@Description", speciality.Speciality_Description ?? (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@ModifiedBy", speciality.Speciality_ModifiedBy ?? (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@ModifiedAt", speciality.Speciality_ModifiedAt ?? (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@Id", speciality.Speciality_Id);

        //    conn.Open();
        //    return cmd.ExecuteNonQuery() > 0;
        //}

        //// delete 
        //public static bool DeleteSpeciality(int id)
        //{
        //    const string query = "DELETE FROM Speciality WHERE Speciality_Id = @Id";

        //    using SqlConnection conn = new SqlConnection(ConexionesData.Conexion());
        //    using SqlCommand cmd = new SqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("@Id", id);

        //    conn.Open();
        //    return cmd.ExecuteNonQuery() > 0;
        //}

        // Insertar y retornar ID
        public static int InsertSpeciality(SpecialityModel speciality)
        {
            const string query = @"
                INSERT INTO Speciality 
                (Speciality_Name, Speciality_Description, Speciality_CreatedBy, Speciality_CreatedAt)
                OUTPUT INSERTED.Speciality_Id
                VALUES (@Speciality_Name, @Speciality_Description, @Speciality_CreatedBy, @Speciality_CreatedAt)";

            using var conn = new SqlConnection(ConexionesData.Conexion());
            return conn.ExecuteScalar<int>(query, speciality);
        }

        // Obtener todas las especialidades
        public static List<SpecialityModel> GetAllSpecialities()
        {
            const string query = @"
                SELECT Speciality_Id, Speciality_Name, Speciality_Description, 
                       Speciality_CreatedBy, Speciality_CreatedAt, 
                       Speciality_ModifiedBy, Speciality_ModifiedAt
                FROM Speciality";

            using var conn = new SqlConnection(ConexionesData.Conexion());
            return conn.Query<SpecialityModel>(query).ToList();
        }

        // Obtener especialidad por ID
        public static SpecialityModel? GetSpecialityById(int id)
        {
            const string query = @"
                SELECT Speciality_Id, Speciality_Name, Speciality_Description, 
                       Speciality_CreatedBy, Speciality_CreatedAt, 
                       Speciality_ModifiedBy, Speciality_ModifiedAt
                FROM Speciality
                WHERE Speciality_Id = @Id";

            using var conn = new SqlConnection(ConexionesData.Conexion());
            return conn.QueryFirstOrDefault<SpecialityModel>(query, new { Id = id });
        }

        // Actualizar especialidad
        public static bool UpdateSpeciality(SpecialityModel speciality)
        {
            const string query = @"
                UPDATE Speciality
                SET Speciality_Name = @Speciality_Name, 
                    Speciality_Description = @Speciality_Description,
                    Speciality_ModifiedBy = @Speciality_ModifiedBy,
                    Speciality_ModifiedAt = @Speciality_ModifiedAt
                WHERE Speciality_Id = @Speciality_Id";

            using var conn = new SqlConnection(ConexionesData.Conexion());
            return conn.Execute(query, speciality) > 0;
        }

        // Eliminar especialidad
        public static bool DeleteSpeciality(int id)
        {
            const string query = "DELETE FROM Speciality WHERE Speciality_Id = @Id";

            using var conn = new SqlConnection(ConexionesData.Conexion());
            return conn.Execute(query, new { Id = id }) > 0;
        }
    }
}
