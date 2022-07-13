using System.Data;

namespace Ensek.Test.DataAccess.Factories.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}