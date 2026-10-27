using AccessControl.Application.UseCases.Credentials.Common;
using FluentValidation;

namespace AccessControl.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    public sealed class GetCredentialByCodeQueryValidator : AbstractValidator<GetCredentialByCodeQuery>
    {
        public GetCredentialByCodeQueryValidator()
        {
            RuleFor(query => query.Code).ValidCredentialCode();
        }
    }
}
