using DanskeBank.Domain.SeedWork;

namespace DanskeBank.Domain.CompanyAggregate;

public interface ICompanyRepository : IRepository<Company>
{
    void Add(Company company);

    Task<Company?> Get(Guid id);
}
