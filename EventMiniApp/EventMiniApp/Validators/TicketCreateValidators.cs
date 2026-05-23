using EventMiniApp.Dtos.TicketDto;
using FluentValidation;

namespace EventMiniApp.Validators
{
    public class TicketCreateValidators: AbstractValidator<TicketCreateDto>
    {
        public TicketCreateValidators()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.QuantityAvailable)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.EventId)
                .GreaterThan(0);
        }
    }
}
