/****** Object:  Database [FichaClinica]    Script Date: 08-08-2025 23:19:58 ******/
CREATE DATABASE [FichaClinica] 
GO
ALTER DATABASE [FichaClinica] SET COMPATIBILITY_LEVEL = 170
GO
ALTER DATABASE [FichaClinica] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FichaClinica] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FichaClinica] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FichaClinica] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FichaClinica] SET ARITHABORT OFF 
GO
ALTER DATABASE [FichaClinica] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FichaClinica] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FichaClinica] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FichaClinica] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FichaClinica] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FichaClinica] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FichaClinica] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FichaClinica] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FichaClinica] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [FichaClinica] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FichaClinica] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [FichaClinica] SET  MULTI_USER 
GO
ALTER DATABASE [FichaClinica] SET ENCRYPTION ON
GO
ALTER DATABASE [FichaClinica] SET QUERY_STORE = ON
GO

/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 08-08-2025 23:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointment](
	[Appointment_Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[DoctorId] [int] NOT NULL,
	[Appointment_StartUtc] [datetime2](7) NOT NULL,
	[Appointment_EndUtc] [datetime2](7) NOT NULL,
	[Appointment_Diagnosis] [nvarchar](500) NULL,
	[Appointment_Room] [nvarchar](30) NULL,
	[Appointment_Status] [nvarchar](20) NULL,
	[Appointment_CreatedBy] [nvarchar](128) NOT NULL,
	[Appointment_CreatedAt] [datetime2](7) NOT NULL,
	[Appointment_ModifiedBy] [nvarchar](128) NULL,
	[Appointment_ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[Appointment_Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctor]    Script Date: 08-08-2025 23:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctor](
	[Doctor_Id] [int] IDENTITY(1,1) NOT NULL,
	[Doctor_FirstName] [nvarchar](100) NOT NULL,
	[Doctor_LastName] [nvarchar](100) NOT NULL,
	[Doctor_Email] [nvarchar](255) NULL,
	[Doctor_Phone] [nvarchar](50) NULL,
	[Doctor_LicenseNumber] [nvarchar](50) NOT NULL,
	[SpecialityId] [int] NOT NULL,
	[Doctor_CreatedBy] [nvarchar](128) NOT NULL,
	[Doctor_CreatedAt] [datetime2](7) NOT NULL,
	[Doctor_ModifiedBy] [nvarchar](128) NULL,
	[Doctor_ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Doctor] PRIMARY KEY CLUSTERED 
(
	[Doctor_Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Doctor_LicenseNumber] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 08-08-2025 23:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patient](
	[Patient_Id] [int] IDENTITY(1,1) NOT NULL,
	[Patient_FirstName] [nvarchar](100) NOT NULL,
	[Patient_LastName] [nvarchar](100) NOT NULL,
	[Patient_RUT] [varchar](12) NOT NULL,
	[Patient_DateOfBirth] [date] NOT NULL,
	[Patient_Gender] [char](1) NULL,
	[Patient_Phone] [nvarchar](50) NULL,
	[Patient_Email] [nvarchar](255) NULL,
	[Patient_AddressLine1] [nvarchar](150) NULL,
	[Patient_AddressLine2] [nvarchar](150) NULL,
	[Patient_City] [nvarchar](100) NULL,
	[Patient_State] [nvarchar](100) NULL,
	[Patient_PostalCode] [nvarchar](20) NULL,
	[Patient_CreatedBy] [nvarchar](128) NOT NULL,
	[Patient_CreatedAt] [datetime2](7) NOT NULL,
	[Patient_ModifiedBy] [nvarchar](128) NULL,
	[Patient_ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[Patient_Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Patient_RUT] UNIQUE NONCLUSTERED 
(
	[Patient_RUT] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Speciality]    Script Date: 08-08-2025 23:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Speciality](
	[Speciality_Id] [int] IDENTITY(1,1) NOT NULL,
	[Speciality_Name] [nvarchar](100) NOT NULL,
	[Speciality_Description] [nvarchar](250) NULL,
	[Speciality_CreatedBy] [nvarchar](128) NOT NULL,
	[Speciality_CreatedAt] [datetime2](7) NOT NULL,
	[Speciality_ModifiedBy] [nvarchar](128) NULL,
	[Speciality_ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Speciality] PRIMARY KEY CLUSTERED 
(
	[Speciality_Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Speciality_Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Appointment_Doctor_Time]    Script Date: 08-08-2025 23:19:58 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Appointment_Doctor_Time] ON [dbo].[Appointment]
(
	[DoctorId] ASC,
	[Appointment_StartUtc] ASC,
	[Appointment_EndUtc] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Appointment] ADD  DEFAULT (sysutcdatetime()) FOR [Appointment_CreatedAt]
GO
ALTER TABLE [dbo].[Doctor] ADD  DEFAULT (sysutcdatetime()) FOR [Doctor_CreatedAt]
GO
ALTER TABLE [dbo].[Patient] ADD  DEFAULT (sysutcdatetime()) FOR [Patient_CreatedAt]
GO
ALTER TABLE [dbo].[Speciality] ADD  DEFAULT (sysutcdatetime()) FOR [Speciality_CreatedAt]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Doctor] FOREIGN KEY([DoctorId])
REFERENCES [dbo].[Doctor] ([Doctor_Id])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Doctor]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patient] ([Patient_Id])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Patient]
GO
ALTER TABLE [dbo].[Doctor]  WITH CHECK ADD  CONSTRAINT [FK_Doctor_Speciality] FOREIGN KEY([SpecialityId])
REFERENCES [dbo].[Speciality] ([Speciality_Id])
GO
ALTER TABLE [dbo].[Doctor] CHECK CONSTRAINT [FK_Doctor_Speciality]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [CK_Appointment_NoOverlap] CHECK  (([Appointment_EndUtc]>[Appointment_StartUtc]))
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [CK_Appointment_NoOverlap]
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckAppointmentAvailability]    Script Date: 08-08-2025 23:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:     Guillermo reyes
-- Create Date: 07-08-2025
-- Description: Procedimiento almacenado para revisar disponibilidad de horario
--				recibe fecha inicio, fecha fin, id's de doctor y paciente 
--				si el doctor esta ocupado en ese horario, entrega un mensaje
--				si el paciente tiene una hora asignada en ese horaio, entrega otro mensaje
--				cuando es correcto.
--					- se agregan 10 minutos al inico y al final porconcepto de demora de paciente 
--					  o por atraso de doctor.
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckAppointmentAvailability]
    @StartUtc DATETIME2(7),
    @EndUtc DATETIME2(7),
    @DoctorId INT,
    @PatientId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResultMessage NVARCHAR(200);

    -- Validar si el doctor tiene otra cita en ese rango
    IF EXISTS (
        SELECT 1
        FROM Appointment
        WHERE DoctorId = @DoctorId
          AND Appointment_StartUtc < DATEADD(MINUTE, 10, @EndUtc)
          AND Appointment_EndUtc > DATEADD(MINUTE, -10, @StartUtc)
    )
    BEGIN
        SET @ResultMessage = 'Doctor ocupado en ese horario';
        SELECT 0 AS IsAvailable, @ResultMessage AS Message;
        RETURN;
    END

    -- Validar si el paciente tiene otra cita en ese rango
    IF EXISTS (
        SELECT 1
        FROM Appointment
        WHERE PatientId = @PatientId
          AND Appointment_StartUtc < DATEADD(MINUTE, 10, @EndUtc)
          AND Appointment_EndUtc > DATEADD(MINUTE, -10, @StartUtc)
    )
    BEGIN
        SET @ResultMessage = 'Paciente ya tiene una cita en ese horario';
        SELECT 0 AS IsAvailable, @ResultMessage AS Message;
        RETURN;
    END

    -- Horario Disponible
    SET @ResultMessage = 'Disponible';
    SELECT 1 AS IsAvailable, @ResultMessage AS Message;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateDoctor]    Script Date: 08-08-2025 23:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:     Guillermo reyes
-- Create Date: 07-08-2025
-- Description: Procedimiento almacenado para actualizacion de usuarios
--				Se usa el procedimiento porque primero revisamos si la especialidad cambia, 
--				si cambia, insertamos una nueva, cambia cuando se inserta nombre o descripcion
--				el paso siguiente es insertar los nuevos datos a actualizar, solo si estos vienen.

--				hay prints para debug en el sp,  los dejaré comentados.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateDoctor]
    @DoctorId INT,
    @Doctor_FirstName NVARCHAR(100) = NULL,
    @Doctor_LastName NVARCHAR(100) = NULL,
    @Doctor_Email NVARCHAR(255) = NULL,
    @Doctor_Phone NVARCHAR(50) = NULL,
    @Doctor_LicenseNumber NVARCHAR(50) = NULL,
    @Speciality_Name NVARCHAR(100) = NULL,
    @Speciality_Description NVARCHAR(250) = NULL,
    @Doctor_ModifiedBy NVARCHAR(128) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        --PRINT '--- sp_UpdateDoctor: START ---';

        DECLARE @NewSpecialityId INT = NULL;                              
        DECLARE @ModifiedAt DATETIME2 = SYSUTCDATETIME();                
        DECLARE @ModifiedByFinal NVARCHAR(128) = ISNULL(@Doctor_ModifiedBy, 'admin'); 
        DECLARE @CurrentSpecialityId INT;
        DECLARE @CurrentSpecialityName NVARCHAR(100);
        DECLARE @CurrentSpecialityDesc NVARCHAR(250);

        --PRINT CONCAT('Parameters -> DoctorId=', @DoctorId, 
        --             ' FirstName=', ISNULL(@Doctor_FirstName,'(NULL)'),
        --             ' LastName=', ISNULL(@Doctor_LastName,'(NULL)'),
        --             ' Email=', ISNULL(@Doctor_Email,'(NULL)'),
        --             ' Phone=', ISNULL(@Doctor_Phone,'(NULL)'),
        --             ' License=', ISNULL(@Doctor_LicenseNumber,'(NULL)'),
        --             ' SpecName=', ISNULL(@Speciality_Name,'(NULL)'),
        --             ' SpecDesc=', ISNULL(@Speciality_Description,'(NULL)'),
        --             ' ModifiedBy=', @ModifiedByFinal);

        SELECT 
            @CurrentSpecialityId = SpecialityId,
            @CurrentSpecialityName = S.Speciality_Name,
            @CurrentSpecialityDesc = S.Speciality_Description
        FROM Doctor D
        INNER JOIN Speciality S ON S.Speciality_Id = D.SpecialityId
        WHERE Doctor_Id = @DoctorId;

        --PRINT CONCAT('CurrentSpecialityId = ', ISNULL(CONVERT(varchar(10), @CurrentSpecialityId),'NULL'));

        -- Validar cambios en la especialidad
        IF (LTRIM(RTRIM(ISNULL(@Speciality_Name, ''))) = '' AND LTRIM(RTRIM(ISNULL(@Speciality_Description, ''))) = '')
        BEGIN
            --PRINT '-> No speciality changes requested (both empty).';
            SELECT 1; -- dummy command para evitar bloque vacío
        END
        ELSE IF (ISNULL(@Speciality_Name, @CurrentSpecialityName) = @CurrentSpecialityName
                AND ISNULL(@Speciality_Description, @CurrentSpecialityDesc) = @CurrentSpecialityDesc)
        BEGIN
            --PRINT '-> No speciality changes requested (values equal to current).';
            SELECT 1; -- dummy command para evitar bloque vacío
        END
        ELSE
        BEGIN
            -- Si nombre o descripción son diferentes y no vacíos, insertar nueva especialidad
            IF 
                (@Speciality_Name IS NOT NULL AND LTRIM(RTRIM(@Speciality_Name)) <> '' AND @Speciality_Name <> @CurrentSpecialityName)
                OR
                (@Speciality_Description IS NOT NULL AND LTRIM(RTRIM(@Speciality_Description)) <> '' AND @Speciality_Description <> @CurrentSpecialityDesc)
            BEGIN
                --PRINT '-> Inserting new Speciality...';
                INSERT INTO Speciality 
                (
                    Speciality_Name, 
                    Speciality_Description, 
                    Speciality_CreatedBy, 
                    Speciality_CreatedAt,
                    Speciality_ModifiedBy,
                    Speciality_ModifiedAt
                )
                VALUES 
                (
                    ISNULL(@Speciality_Name, @CurrentSpecialityName),
                    ISNULL(@Speciality_Description, @CurrentSpecialityDesc),
                    @ModifiedByFinal,
                    @ModifiedAt,
                    @ModifiedByFinal,
                    @ModifiedAt
                );

                SET @NewSpecialityId = SCOPE_IDENTITY();
                --PRINT CONCAT('-> NewSpecialityId = ', CONVERT(varchar(20), @NewSpecialityId));
            END
            ELSE
            BEGIN
                --Actualizar especialidad existente si cambia parcialmente
                --PRINT '-> Updating existing Speciality...';
                UPDATE Speciality
                SET 
                    Speciality_Name = CASE 
                        WHEN @Speciality_Name IS NOT NULL AND LTRIM(RTRIM(@Speciality_Name)) <> '' 
                            AND @Speciality_Name <> @CurrentSpecialityName 
                        THEN @Speciality_Name ELSE Speciality_Name END,
                    Speciality_Description = CASE 
                        WHEN @Speciality_Description IS NOT NULL AND LTRIM(RTRIM(@Speciality_Description)) <> '' 
                            AND @Speciality_Description <> @CurrentSpecialityDesc
                        THEN @Speciality_Description ELSE Speciality_Description END,
                    Speciality_ModifiedBy = @ModifiedByFinal,
                    Speciality_ModifiedAt = @ModifiedAt
                WHERE Speciality_Id = @CurrentSpecialityId;

                --PRINT '-> Speciality update executed';
            END
        END

        -- Construir dinámicamente el UPDATE del Doctor
        DECLARE @SQL NVARCHAR(MAX) = N'UPDATE Doctor SET ';
        DECLARE @Params NVARCHAR(MAX) = N'@DoctorId INT, @ModifiedBy NVARCHAR(128), @ModifiedAt DATETIME2,
                                          @Doctor_FirstName NVARCHAR(100), @Doctor_LastName NVARCHAR(100), 
                                          @Doctor_Email NVARCHAR(255), @Doctor_Phone NVARCHAR(50), 
                                          @Doctor_LicenseNumber NVARCHAR(50), @NewSpecialityId INT';

        IF @Doctor_FirstName IS NOT NULL
            SET @SQL += 'Doctor_FirstName = @Doctor_FirstName, ';
        IF @Doctor_LastName IS NOT NULL
            SET @SQL += 'Doctor_LastName = @Doctor_LastName, ';
        IF @Doctor_Email IS NOT NULL
            SET @SQL += 'Doctor_Email = @Doctor_Email, ';
        IF @Doctor_Phone IS NOT NULL
            SET @SQL += 'Doctor_Phone = @Doctor_Phone, ';
        IF @Doctor_LicenseNumber IS NOT NULL
            SET @SQL += 'Doctor_LicenseNumber = @Doctor_LicenseNumber, ';
        IF @NewSpecialityId IS NOT NULL
            SET @SQL += 'SpecialityId = @NewSpecialityId, ';

        SET @SQL += 'Doctor_ModifiedBy = @ModifiedBy, Doctor_ModifiedAt = @ModifiedAt 
                     WHERE Doctor_Id = @DoctorId';

        --PRINT '-> Dynamic SQL to execute:';
        --PRINT @SQL;
        --PRINT CONCAT('-> Exec params: ModifiedBy=', @ModifiedByFinal, 
        --             ' ModifiedAt=', CONVERT(varchar(30), @ModifiedAt, 126));

        EXEC sp_executesql 
            @SQL,
            @Params,
            @DoctorId = @DoctorId,
            @ModifiedBy = @ModifiedByFinal,
            @ModifiedAt = @ModifiedAt,
            @Doctor_FirstName = @Doctor_FirstName,
            @Doctor_LastName = @Doctor_LastName,
            @Doctor_Email = @Doctor_Email,
            @Doctor_Phone = @Doctor_Phone,
            @Doctor_LicenseNumber = @Doctor_LicenseNumber,
            @NewSpecialityId = @NewSpecialityId;

        --PRINT '-> Doctor update executed';

        COMMIT TRANSACTION;

        --PRINT '--- sp_UpdateDoctor: COMMIT OK ---';
        --PRINT '--- sp_UpdateDoctor: SUCCESS ---';
        SELECT 1 AS Result;
        RETURN;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum INT = ERROR_NUMBER();
        --PRINT CONCAT('--- sp_UpdateDoctor: ERROR (', @ErrNum, ') ', @ErrMsg);

        ROLLBACK TRANSACTION;
        SELECT 0 AS Result;
        RETURN;
    END CATCH
END;
GO
ALTER DATABASE [FichaClinica] SET  READ_WRITE 
GO
