using DanskeBank.Domain.CompanyAggregate.Enums;

namespace DanskeBank.Domain.CompanyAggregate.Factories;

public interface ICompanyFactory
{
    Company Create(
        Guid id,
        string companyName,
        string companyNumber,
        CompanyType companyType,
        Market market);
}
