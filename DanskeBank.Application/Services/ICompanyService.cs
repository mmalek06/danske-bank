using DanskeBank.Application.Dtos.Request;
using DanskeBank.Domain.CompanyAggregate;

namespace DanskeBank.Application.Services;

public interface ICompanyService
{
    Task<bool> CreateCompany(CreateCompanyRequest request);

    Task<Company?> Get(Guid id);
}
