using System;

namespace HospitalManagementSystem.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;

        public string Medicines { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? FollowUpDate { get; set; }
    }
}