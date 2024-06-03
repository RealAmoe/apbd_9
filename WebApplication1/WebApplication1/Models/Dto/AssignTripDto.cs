using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Dto;

public class AssignTripDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }
    [Required]
    [MaxLength(120)]
    public string Email { get; set; }
    [Required]
    [MaxLength(120)]
    public string Telephone { get; set; }
    [Required]
    [MaxLength(120)]
    public string Pesel { get; set; }
    [Required]
    public int IdTrip { get; set; }
    [Required]
    [MaxLength(120)]
    public string TripName { get; set; }
    public DateTime? PaymentDate { get; set; }
}