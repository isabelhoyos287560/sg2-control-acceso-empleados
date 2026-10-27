using FluentValidation;

namespace AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class GetAccessPointsListQueryValidator : AbstractValidator<GetAccessPointsListQuery>
    {
        public GetAccessPointsListQueryValidator()
        {
            RuleFor(query => query.Type).IsInEnum().When(query => query.Type.HasValue)
                                        .WithMessage("El tipo de punto de acceso no es válido.");
        }
    }
}
