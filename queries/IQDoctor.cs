using code.interfaces;

namespace code.queries;

public interface IQDoctor : ISQLData
{
    string CustomDoctores { get; }
}