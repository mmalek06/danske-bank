using DanskeBank.Application.Dtos.Request;
using DanskeBank.Application.Dtos.Response;
using DanskeBank.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanskeBank.Application.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService) =>
        _companyService = companyService;

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompanyFoundResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyFoundResponse>> Get(Guid id)
    {
        var company = await _companyService.Get(id);

        if (company is null)
            return NotFound();

        // One could argue that creating this response may be better suited for the application service
        // however nothing in the application service tells it should be used exclusively by controllers.
        // Therefore for cheap future proofing of the application, I think it's better to let the service
        // return the bare model and let the layer above it worry about mapping it to the required format.
        var response = new CompanyFoundResponse(
            company.Id, 
            company.Schedule.Select(n => n.SendingDate));

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(CreateCompanyRequest request)
    {
        var companyCreated = await _companyService.CreateCompany(request);

        return companyCreated
            ? Ok()
            : BadRequest();
    }
}