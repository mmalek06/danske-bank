using DanskeBank.Domain.CompanyAggregate;
using DanskeBank.Domain.SeedWork;

namespace DanskeBank.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    public IUnitOfWork UnitOfWork => _ctx;

    private readonly CompanyContext _ctx;

    public CompanyRepository(CompanyContext ctx) =>
        _ctx = ctx;

    public void Add(Company company) =>
        _ctx.Companies.Add(company);

    public async Task<Company?> Get(Guid id) =>
        await _ctx.Companies.FindAsync(id);
}
