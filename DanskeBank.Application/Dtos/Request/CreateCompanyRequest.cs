using DanskeBank.Domain.CompanyAggregate.Enums;
using System.ComponentModel.DataAnnotations;
using static DanskeBank.Application.Dtos.Request.ValidationCodes;

namespace DanskeBank.Application.Dtos.Request;

public record CreateCompanyRequest(
    [Required(ErrorMessage = FieldRequired)]
    Guid Id,

    [Required(ErrorMessage = FieldRequired)]
    [StringLength(128, MinimumLength = 2, ErrorMessage = WrongCompanyNameLength)]
    string CompanyName,

    [Required(ErrorMessage = FieldRequired)]
    [StringLength(10, MinimumLength = 10, ErrorMessage = WrongCompanyNumberLength)]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = WrongCompanyNumberFormat)]
    string CompanyNumber,

    [NonZeroEnum(ErrorMessage = FieldRequired)]
    CompanyType CompanyType,

    [NonZeroEnum(ErrorMessage = FieldRequired)]
    Market Market);
