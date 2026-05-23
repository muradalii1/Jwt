using EventMiniApp.Dtos.OrganizerDto;
using FluentValidation;

namespace EventMiniApp.Validators
{
    public class OrganizerCreateValidators: AbstractValidator<OrganizerCreateDto>
    {
        public OrganizerCreateValidators()
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
