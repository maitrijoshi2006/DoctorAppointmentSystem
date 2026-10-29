using DoctorAppointmentSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace DoctorAppointmentSystem.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Doctor>().Property(d => d.ConsultationFee).HasPrecision(10, 2);
        builder.Entity<Appointment>().HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.AppointmentTime });
        builder.Entity<Appointment>().HasOne(a => a.Patient).WithMany().HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Appointment>().HasOne(a => a.Doctor).WithMany().HasForeignKey(a => a.DoctorId).OnDelete(DeleteBehavior.Restrict);
    }
}
