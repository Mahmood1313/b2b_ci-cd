using FluentValidation;

namespace B2BPriceAdmin.DTO
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(model => model.Email)
                .EmailAddress().WithMessage("Please check your email format.")
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Length(6, 50);
        }
    }
}
