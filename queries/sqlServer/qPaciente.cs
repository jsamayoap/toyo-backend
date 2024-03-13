namespace code.queries.sqlServer;

public sealed class qPaciente : IQPaciente
{
    private const string _selectAll = @"
SELECT [cliente]
      ,[nombres]
      ,[apellido_paterno]
      ,[apellido_materno]
      ,[fecha_nacimiento]
      ,[nacionalidad]
  FROM [dbo].[Cliente]";

      private const string _selectOne = @"
SELECT [cliente]
      ,[nombres]
      ,[apellido_paterno]
      ,[apellido_materno]
      ,[fecha_nacimiento]
      ,[nacionalidad]
  FROM [dbo].[Cliente]
  WHERE [dbo].[Cliente].[cliente]=@PACIENTE";

private const string _delete = @"
DELETE FROM [dbo].[Cliente]
WHERE [dbo].[Cliente].[cliente]=@PACIENTE";

private const string _update = @"
UPDATE [dbo].[Cliente]
SET [dbo].[Cliente].[nombres] = @NOMBRES
WHERE [dbo].[Cliente].[cliente]=@PACIENTE";

private const string _add = @"
INSERT INTO [dbo].[Cliente]
           ([nombres]
           ,[apellido_paterno]
           ,[apellido_materno]
           ,[fecha_nacimiento]
           ,[nacionalidad])
OUTPUT Inserted.[Cliente]
VALUES
    (@NOMBRES,
    @APELLIDO_PATERNO,
    @APELLIDO_MATERNO,
    @FECHA_NACIMIENTO,
    @NACIONALIDAD)";

    public string SQLGetAll => _selectAll;

    public string SQLDataEntity => _selectOne;

    public string NewDataEntity => _add;

    public string DeleteDataEntity => _delete;

    public string UpdateWholeEntity => _update;
}