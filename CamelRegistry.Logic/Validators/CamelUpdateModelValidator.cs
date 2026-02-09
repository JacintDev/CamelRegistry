using CamelRegistry.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelRegistry.Logic.Validators
{
    public class CamelUpdateModelValidator: AbstractValidator<CamelUpdateModel>
    {
        public CamelUpdateModelValidator()
        {
            RuleFor(x => x.Name)
                           .NotEmpty().WithMessage("Name is required")
                           .Length(1, 50).WithMessage("Name must be between 1 and 50 characters");
            RuleFor(x => x.HumpCount)
                .InclusiveBetween(1, 2).WithMessage("HumpCount must be between 1 and 2");
            RuleFor(x => x.Color)
                .MaximumLength(50).WithMessage("Color must be at most 50 characters");
            RuleFor(x => x.LastFed)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.LastFed.HasValue).WithMessage("LastFed cannot be in the future");

        }
    }
}
