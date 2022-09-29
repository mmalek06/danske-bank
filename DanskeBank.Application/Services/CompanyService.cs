using DanskeBank.Application.Dtos.Request;
using DanskeBank.Domain.CompanyAggregate;
using DanskeBank.Domain.CompanyAggregate.Factories;

namespace DanskeBank.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly ICompanyFactory _companyFactory;
    private readonly ILogger<CompanyService> _logger;

    public CompanyService(
        ICompanyRepository repository,
        ICompanyFactory companyFactory,
        ILogger<CompanyService> logger)
    {
        _repository = repository;
        _companyFactory = companyFactory;
        _logger = logger;
    }

    public async Task<bool> CreateCompany(CreateCompanyRequest request)
    {
        _logger.LogInformation("Adding a company with id {CompanyId}.", request.Id);

        try
        {
            var company = _companyFactory.Create(
                request.Id,
                request.CompanyName,
                request.CompanyNumber,
                request.CompanyType,
                request.Market);

            _repository.Add(company);

            await _repository.UnitOfWork.SaveChangesAsync();

            return true;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "An exception occurred while adding a company with id {CompanyId}.", request.Id);
        }

        return false;
    }

    public async Task<Company?> Get(Guid id) =>
        await _repository.Get(id);
}
