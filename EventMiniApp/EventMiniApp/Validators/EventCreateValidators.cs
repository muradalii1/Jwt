using EventMiniApp.Dtos.EventDto;
using FluentValidation;

namespace EventMiniApp.Validators
{
    public class EventCreateValidators: AbstractValidator<EventCreateDto>
    {
        public EventCreateValidators()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title boş ola bilməz")
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.Date)
                .GreaterThan(DateTime.Now)
                .WithMessage("Event tarixi gələcəkdə olmalıdır");

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.OrganizerId)
                .GreaterThan(0);
        }
    }
}
