using System.ComponentModel.DataAnnotations;

namespace Assessment.Application.Dtos;

public class CreatePaymentRequestDto
{
    [Required] public Guid ProviderId { get; set; }
    [Range(0.01, 1000000)] public decimal Amount { get; set; }
    [Required] public string? Currency { get; set; }
    public string? Reference { get; set; }
}
