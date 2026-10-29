using System.ComponentModel.DataAnnotations;
namespace DoctorAppointmentSystem.Models;
public class Doctor
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Specialization { get; set; } = string.Empty;
    [Range(0, 70)] public int ExperienceYears { get; set; }
    [Range(typeof(decimal), "0", "1000000")] public decimal ConsultationFee { get; set; }
    [StringLength(500)] public string? Description { get; set; }
    public bool IsAvailable { get; set; } = true;
}
