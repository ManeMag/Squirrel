using FluentValidation;
using Squirrel.Constants.Validation;
using Squirrel.Requests.Category;

namespace Squirrel.Validators.Category
{
    public sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        // TODO: Carry out validation messages to constants
        public UpdateCategoryRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().NotNull().Length(0, 255)
                .WithMessage("{PropertyName} should not be between 0 and 255 characters length");
            RuleFor(c => c.Color).NotEmpty().NotNull().Matches(ValidationConstants.HexademicalColorRegexp)
                .WithMessage("{PropertyName} should be hexademical, for instance '#012AEF'");
        }
    }
}
