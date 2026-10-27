using FluentValidation;

namespace AccessControl.Application.UseCases.AccessRecords.Queries.GetAccessHistory
{
    public sealed class GetAccessHistoryQueryValidator : AbstractValidator<GetAccessHistoryQuery>
    {
        public GetAccessHistoryQueryValidator()
        {
            RuleFor(query => query).Must(query => !query.From.HasValue || !query.To.HasValue || query.From <= query.To)
                                   .WithMessage("La fecha inicial no puede ser mayor que la fecha final.");

            RuleFor(query => query.Result).IsInEnum().When(query => query.Result.HasValue)
                                          .WithMessage("El resultado de validación no es válido (1 = Autorizado, 2 = Rechazado).");
        }
    }
}
