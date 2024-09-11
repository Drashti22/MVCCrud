using System.ComponentModel.DataAnnotations;
using MvcPractical.Models;
using System.Linq;

public class UniqueEmailAttribute : ValidationAttribute
{
    private readonly MvcPracticalEntities _context;

    public UniqueEmailAttribute()
    {
        _context = new MvcPracticalEntities(); // Instantiate the DbContext
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var email = value as string;

        // Check if email already exists in the database
        var user = _context.Users.SingleOrDefault(u => u.Email == email);

        if (user != null)
        {
            return new ValidationResult("This email address is already in use.");
        }


        return ValidationResult.Success;
    }
}
