using System.Data;

namespace Products.SharedKernel
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbConnection GetSharedConnection();
    }
}
