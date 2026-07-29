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
        if (value is null || !Enum.TryParse(value.ToString(), ignoreCase: true, out ListingStatus status)
            || !Enum.IsDefined(status))
        {
            throw new DataException("Le statut de l'annonce enregistré est invalide.");
        }

        return status;
    }
}
