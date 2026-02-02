using System.ComponentModel.DataAnnotations;

namespace Assessment.Application.Dtos;

public class CreatePaymentRequestDto : IValidatableObject
{
    [Required] public Guid ProviderId { get; set; }
    [Range(0.01, 1000000)] public decimal Amount { get; set; }
    [Required] public string? Currency { get; set; }
    public string? Reference { get; set; }

    // Card details (collected but only last4 + expiry stored)
    [Required]
    [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Card number must be 13-19 digits.")]
    public string? CardNumber { get; set; }

    [Range(1,12)]
    public int ExpiryMonth { get; set; }

    [Range(2020, 2100)]
    public int ExpiryYear { get; set; }

    [Required]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be exactly 3 digits.")]
    public string? Cvv { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ExpiryMonth < 1 || ExpiryMonth > 12)
        {
            yield return new ValidationResult("Expiry month must be between 1 and 12.", new[] { nameof(ExpiryMonth) });
        }

        var now = DateTime.UtcNow;
        var month = Math.Clamp(ExpiryMonth, 1, 12);
        bool parsed = true;
        DateTime expiryEnd = now;
        try
        {
            var lastDay = DateTime.DaysInMonth(ExpiryYear, month);
            expiryEnd = new DateTime(ExpiryYear, month, lastDay, 23, 59, 59, DateTimeKind.Utc);
        }
        catch { parsed = false; }

        if (!parsed)
        {
            yield return new ValidationResult("Invalid expiry date.", new[] { nameof(ExpiryMonth), nameof(ExpiryYear) });
        }
        else if (expiryEnd < now)
        {
            yield return new ValidationResult("Card has expired.", new[] { nameof(ExpiryMonth), nameof(ExpiryYear) });
        }

        // Luhn check for PAN (only if digit count looks valid)
        var num = CardNumber ?? string.Empty;
        var digitsOnly = new string(num.Where(char.IsDigit).ToArray());
        if (digitsOnly.Length >= 13 && digitsOnly.Length <= 19)
        {
            int sum = 0;
            bool dbl = false;
            for (int i = digitsOnly.Length - 1; i >= 0; i--)
            {
                int d = digitsOnly[i] - '0';
                if (dbl)
                {
                    d *= 2;
                    if (d > 9) d -= 9;
                }
                sum += d;
                dbl = !dbl;
            }
            if (sum % 10 != 0)
                yield return new ValidationResult("Invalid card number.", new[] { nameof(CardNumber) });
        }
    }
}
