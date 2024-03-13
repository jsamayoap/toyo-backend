using code.interfaces;

namespace code.queries.sqlServer;

public interface IQDoctor : ISQLData {}

public sealed class qDoctor : IQDoctor
{
    private const string _selectAll = @"
SELECT [doctor]
      ,[nombres]
      ,[apellido_paterno]
      ,[apellido_materno]
      ,[fecha_nacimiento]
      ,[especialidad]
  FROM [dbo].[Doctor]";

      private const string _selectOne = @"
SELECT [doctor]
      ,[nombres]
      ,[apellido_paterno]
      ,[apellido_materno]
      ,[fecha_nacimiento]
      ,[especialidad]
  FROM [dbo].[Doctor]
  WHERE [dbo].[Doctor].[doctor]=@DOCTOR";

private const string _delete = @"
DELETE FROM [dbo].[Doctor]
WHERE [dbo].[Doctor].[doctor]=@DOCTOR";

private const string _update = @"
UPDATE [dbo].[Doctor]
SET [dbo].[Doctor].[nombres] = @NOMBRES
WHERE [dbo].[Doctor].[doctor]=@DOCTOR";

private const string _add = @"
INSERT INTO [dbo].[Doctor]
           ([nombres]
           ,[apellido_paterno]
           ,[apellido_materno]
           ,[fecha_nacimiento]
           ,[especialidad])
OUTPUT Inserted.[Doctor]
VALUES
    (@NOMBRES,
    @APELLIDO_PATERNO,
    @APELLIDO_MATERNO,
    @FECHA_NACIMIENTO,
    @ESPECIALIDAD)";

    public string SQLGetAll => _selectAll;

    public string SQLDataEntity => _selectOne;

    public string NewDataEntity => _add;

    public string DeleteDataEntity => _delete;

    public string UpdateWholeEntity => _update;
}