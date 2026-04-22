namespace HospitalManagementSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public string Status { get; set; } = "Booked";
    }
}