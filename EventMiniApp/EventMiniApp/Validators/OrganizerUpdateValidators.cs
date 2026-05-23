using EventMiniApp.Dtos.OrganizerDto;
using FluentValidation;

namespace EventMiniApp.Validators
{
    public class OrganizerUpdateValidators : AbstractValidator<OrganizerUpdateDto>
    {
        public OrganizerUpdateValidators()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Phone)
                .MaximumLength(20);
        }
    }
}
