using FluentValidation;
using Squirrel.Constants.Validation;
using Squirrel.Requests.Category;

namespace Squirrel.Validators.Category
{
    public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(c => c.Name).NotNull().Length(1, 255)
                .WithMessage("{PropertyName} should not be between 0 and 255 characters length");
            RuleFor(c => c.Color).NotNull().Matches(ValidationConstants.HexademicalColorRegexp)
                .WithMessage("{PropertyName} should be hexademical, for instance '#012AEF'");
        }
    }
}
