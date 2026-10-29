using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace DoctorAppointmentSystem.ViewModels;
public class BookAppointmentViewModel
{
    [Required] public int DoctorId { get; set; }
    [Required, DataType(DataType.Date)] public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);
    [Required] public string AppointmentTime { get; set; } = "09:00";
    [StringLength(500)] public string? Reason { get; set; }
    public List<SelectListItem> Doctors { get; set; } = new();
}
