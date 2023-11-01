using System;
using System.Data;

namespace MTCG.DAL;

public static class DbExtensions
{
    public static void AddParameterWithValue(this IDbCommand command, string parameterName, DbType type, object value)
    {
        var parameter = command.CreateParameter();
        parameter.DbType = type;
        parameter.ParameterName = parameterName;
        parameter.Value = value ?? DBNull.Value;

        command.Parameters.Add(parameter);
    }

    public static int? GetNullableInt32(this IDataRecord record, int ordinal)
    {
        int? value = null;
        if (!record.IsDBNull(ordinal))
        {
            value = record.GetInt32(ordinal);
        }
        return value;
    }
}