namespace HospitalManagementSystem.Models
{
    public class Bill
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public decimal ConsultationFee { get; set; }

        public decimal MedicineFee { get; set; }

        public decimal RoomCharge { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public string PaymentStatus { get; set; } = "Pending";
    }
}