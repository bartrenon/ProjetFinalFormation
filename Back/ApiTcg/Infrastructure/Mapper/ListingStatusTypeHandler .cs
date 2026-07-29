using System.Data;
using Dapper;
using Domain.Enum;

namespace Infrastructure.Mapper;

public class ListingStatusTypeHandler : SqlMapper.TypeHandler<ListingStatus>
{
    public override void SetValue(IDbDataParameter parameter, ListingStatus value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value.ToString();
    }

    public override ListingStatus Parse(object value)
    {
        return Enum.Parse<ListingStatus>((string)value, ignoreCase: true);
    }
}
