using AdminGA.Models.Student;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AdminGA.Validation.Student
{
    public class PreadmissionModelValidator : AbstractValidator<PreadmissionModel>
    {
        public PreadmissionModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .Matches("^[a-zA-Z ]+$").WithMessage("First Name should not contain special characters");

            RuleFor(x => x.MiddleName)
                .NotEmpty().WithMessage("Middle Name is required")
                .Matches("^[a-zA-Z ]+$").WithMessage("Middle Name should not contain special characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .Matches("^[a-zA-Z ]+$").WithMessage("Last Name should not contain special characters");

            RuleFor(x => x.ClassID)
                .NotEmpty().WithMessage("Class is required")
                .Must(guid => guid != Guid.Empty).WithMessage("Invalid Class selected");

            RuleFor(x => x.MediumID)
                .NotEmpty().WithMessage("Medium is required")
                .Must(guid => guid != Guid.Empty).WithMessage("Invalid Medium selected");



            RuleFor(x => x.Fees)
                .NotEmpty().WithMessage("Fees is required");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile Number is required")
                .Must(mobileNumber => mobileNumber != null && Regex.IsMatch(mobileNumber, @"^\d{10}$")).WithMessage("Mobile Number should contain exactly 10 digits")
                .Must(mobileNumber => mobileNumber == null || !mobileNumber.StartsWith("+91")).WithMessage("Mobile Number should not start with +91");



        }


    }
}
