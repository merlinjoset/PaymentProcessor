using System.ComponentModel.DataAnnotations;

namespace Assessment.Application.Dtos;

public class CreatePaymentRequestDto
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
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3-4 digits.")]
    public string? Cvv { get; set; }
}
