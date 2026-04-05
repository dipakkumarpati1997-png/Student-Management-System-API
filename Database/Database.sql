
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUD_TBL_Student_Details](
	[Student_Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Name] [nvarchar](256) NOT NULL,
	[Student_Email] [nvarchar](128) NOT NULL,
	[Student_Age] [int] NOT NULL,
	[Student_Course] [nvarchar](128) NULL,
	[Created_Date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Student_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UA_TBL_User]    Script Date: 05-04-2026 09:34:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UA_TBL_User](
	[User_Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](64) NULL,
	[Password] [nvarchar](128) NULL,
PRIMARY KEY CLUSTERED 
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[STUD_SP_Student_Add]    Script Date: 05-04-2026 09:34:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[STUD_SP_Student_Add]
(
    @p_Student_Name NVARCHAR(256),
    @p_Student_Email NVARCHAR(128),
    @p_Student_Age INT,
    @p_Student_Course NVARCHAR(128)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID INT = 0
    DECLARE @IsSuccess BIT = 0
    DECLARE @Message VARCHAR(1024)

    BEGIN TRY
        BEGIN TRANSACTION

        IF EXISTS (SELECT 1 FROM STUD_TBL_Student_Details WHERE Student_Email = @p_Student_Email)
        BEGIN
            SET @Message = 'Email already exists!'
        END
        ELSE
        BEGIN
            INSERT INTO STUD_TBL_Student_Details
            (
                Student_Name,
                Student_Email,
                Student_Age,
                Student_Course,
                Created_Date
            )
            VALUES
            (
                @p_Student_Name,
                @p_Student_Email,
                @p_Student_Age,
                @p_Student_Course,
                GETDATE()
            )

            SET @ID = SCOPE_IDENTITY()
            SET @IsSuccess = 1
            SET @Message = 'Added successfully!'
        END

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @Message = ERROR_MESSAGE()
    END CATCH

    SELECT 
        @ID AS ID, 
        @IsSuccess AS IsSuccess, 
        @Message AS [Message]
END
GO
/****** Object:  StoredProcedure [dbo].[STUD_SP_Student_Delete]    Script Date: 05-04-2026 09:34:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[STUD_SP_Student_Delete]
    @p_Student_Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuccess BIT = 0
    DECLARE @Message VARCHAR(1024)

    BEGIN TRY
        BEGIN TRANSACTION

        DELETE FROM STUD_TBL_Student_Details
        WHERE Student_Id = @p_Student_Id

        SET @IsSuccess = 1
        SET @Message = 'Deleted successfully!'

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @Message = ERROR_MESSAGE()
    END CATCH

    SELECT @p_Student_Id AS ID, @IsSuccess AS IsSuccess, @Message AS [Message]
END
GO
/****** Object:  StoredProcedure [dbo].[STUD_SP_Student_Get]    Script Date: 05-04-2026 09:34:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[STUD_SP_Student_Get]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Student_Id,
        Student_Name,
        Student_Email,
        Student_Age,
        Student_Course,
        Convert(varchar(16),Created_Date,103) as Created_Date

    FROM STUD_TBL_Student_Details
    
END
GO
/****** Object:  StoredProcedure [dbo].[STUD_SP_Student_Update]    Script Date: 05-04-2026 09:34:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[STUD_SP_Student_Update]
    @p_Student_Id INT,
    @p_Student_Name NVARCHAR(256),
    @p_Student_Email NVARCHAR(128),
    @p_Student_Age INT,
    @p_Student_Course NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuccess BIT = 0
    DECLARE @Message VARCHAR(1024)

    BEGIN TRY
        BEGIN TRANSACTION

        -- ✅ Check duplicate email
        IF EXISTS (
            SELECT 1 
            FROM STUD_TBL_Student_Details 
            WHERE Student_Email = @p_Student_Email 
            AND Student_Id != @p_Student_Id
        )
        BEGIN
            SET @Message = 'Email already exists!'
            ROLLBACK TRANSACTION

            SELECT @p_Student_Id AS ID, @IsSuccess AS IsSuccess, @Message AS [Message]
            RETURN
        END

        UPDATE STUD_TBL_Student_Details
        SET
            Student_Name = @p_Student_Name,
            Student_Email = @p_Student_Email,
            Student_Age = @p_Student_Age,
            Student_Course = @p_Student_Course
        WHERE Student_Id = @p_Student_Id

        SET @IsSuccess = 1
        SET @Message = 'Updated successfully!'

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @Message = ERROR_MESSAGE()
    END CATCH

    SELECT @p_Student_Id AS ID, @IsSuccess AS IsSuccess, @Message AS [Message]
END
GO
/****** Object:  StoredProcedure [dbo].[UA_SP_User_Add]    Script Date: 05-04-2026 09:34:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[UA_SP_User_Add]
    @p_Username NVARCHAR(50),
    @p_Password NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
        DECLARE @ID INT = 0
    DECLARE @IsSuccess BIT = 0
    DECLARE @Message VARCHAR(1024)

    BEGIN TRY
        BEGIN TRANSACTION

        -- Check if user already exists
        IF EXISTS (SELECT 1 FROM UA_TBL_User WHERE Username = @p_Username)
        BEGIN
            SET @Message = 'Username already exists!'
        END
        ELSE
        BEGIN
            INSERT INTO UA_TBL_User (Username, Password)
            VALUES (@p_Username, @p_Password)

              SET @ID = SCOPE_IDENTITY()
            SET @IsSuccess = 1
            SET @Message = 'User created successfully!'
        END

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @Message = ERROR_MESSAGE()
    END CATCH

      SELECT @ID AS ID, @IsSuccess AS IsSuccess, @Message AS [Message]
END
GO
/****** Object:  StoredProcedure [dbo].[UA_SP_User_Get]    Script Date: 05-04-2026 09:34:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[UA_SP_User_Get]
@p_Username NVARCHAR(64),
 @p_Password NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Username,
        Password

    FROM UA_TBL_User
    WHERE Username = @p_Username 
    AND Password = @p_Password
END
GO
