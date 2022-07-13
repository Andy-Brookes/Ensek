using Ensek.Test.Domain.Entities;

namespace Ensek.Test.Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetById(int accountId);
    }
}
