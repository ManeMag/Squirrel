﻿using FluentValidation;
using Squirrel.Constants.Validation;
using Squirrel.Requests.Transaction;

namespace Squirrel.Validators.Transaction
{
    using static ValidationConstants;

    public sealed class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        private static Func<DateTime, bool> _beInFuture = date => date > DateTime.UtcNow;

        public CreateTransactionRequestValidator()
        {
            // TODO: Union common validation logic into an abstract base validator
            RuleFor(t => t.Amount).InclusiveBetween(TransactionMinValue, TransactionMaxValue)
                .WithMessage("{PropertyName} should be inclusive between -1 000 000 and 1 000 000");

            RuleFor(t => t.Time).Must(NotBeInFuture)
                .WithMessage("{PropertyName} should not be in future");

            RuleFor(t => t.Description).Length(0, 1000).
                WithMessage("{PropertyName} should not be between 0 and 1000 characters");
        }

        private Func<DateTime, bool> NotBeInFuture = (date) => !_beInFuture(date);
    }
}
