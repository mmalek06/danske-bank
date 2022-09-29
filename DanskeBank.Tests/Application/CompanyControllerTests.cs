using DanskeBank.Application.Controllers;
using DanskeBank.Application.Dtos.Response;
using DanskeBank.Application.Services;
using DanskeBank.Domain.CompanyAggregate;
using DanskeBank.Domain.CompanyAggregate.Enums;
using DanskeBank.Tests.Tools;
using Microsoft.AspNetCore.Mvc;

namespace DanskeBank.Tests.Application;

public class CompanyControllerTests
{
    private readonly Mock<ICompanyService> _companyService;

    public CompanyControllerTests() =>
        _companyService = new Mock<ICompanyService>();

    [Fact]
    public async Task GivenAGetRequest_WhenNoCompanyExists_Then404IsReturned()
    {
        _companyService
            .Setup(s => s.Get(It.IsAny<Guid>()))
            .ReturnsAsync((Company?)null);

        var sut = new CompanyController(_companyService.Object);

        var result = await sut.Get(Guid.Empty);

        result.Result.Should().BeEquivalentTo(new NotFoundResult());
    }

    [Fact]
    public async Task GivenAGetRequest_WhenACompanyExists_ThenItsIdAndNotificationDatesOnlyAreReturned()
    {
        var company = new Company(Guid.Empty, "test", "test", CompanyType.Small, Market.Denmark);

        company.SetPrivateProperty(
            c => c.Schedule, 
            new List<Notification> { new Notification(DateOnly.FromDateTime(DateTime.UtcNow)) });
        _companyService
            .Setup(s => s.Get(It.IsAny<Guid>()))
            .ReturnsAsync(company);

        var sut = new CompanyController(_companyService.Object);
        var responseModel = new CompanyFoundResponse(company.Id, company.Schedule.Select(s => s.SendingDate));

        var result = await sut.Get(Guid.Empty);

        result.Result.Should().BeEquivalentTo(new OkObjectResult(responseModel));
    }
}
