using System.ComponentModel.DataAnnotations;
namespace DoctorAppointmentSystem.Models;
public class Appointment
{
    public int Id { get; set; }
    [Required] public string PatientId { get; set; } = string.Empty;
    public ApplicationUser? Patient { get; set; }
    [Required] public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    [Required, DataType(DataType.Date)] public DateTime AppointmentDate { get; set; }
    [Required, StringLength(5)] public string AppointmentTime { get; set; } = string.Empty;
    [StringLength(500)] public string? Reason { get; set; }
    [Required] public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
