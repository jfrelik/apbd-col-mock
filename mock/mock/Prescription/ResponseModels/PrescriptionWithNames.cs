namespace mock.Prescription;

public class PrescriptionWithNames
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public String DoctorLastName { get; set; }
    public String PatientLastName { get; set; }
}