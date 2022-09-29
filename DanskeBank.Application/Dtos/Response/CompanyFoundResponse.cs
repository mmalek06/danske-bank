namespace DanskeBank.Application.Dtos.Response;

public record CompanyFoundResponse(Guid CompanyId, IEnumerable<DateOnly> Notifications);
