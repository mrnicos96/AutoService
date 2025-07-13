using AutoService.Core.DTOs;
using FluentValidation;

namespace AutoService.Application.Validators
{
    public class ReportRequestValidator : AbstractValidator<ReportDataDto>
    {
        public ReportRequestValidator()
        {
            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .WithMessage("Месяц должен быть от 1 до 12");

            RuleFor(x => x.Year)
                .InclusiveBetween(2000, 2100)
                .WithMessage("Некорректный год");
        }
    }
}