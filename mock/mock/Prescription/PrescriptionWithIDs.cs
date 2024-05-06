namespace mock.Prescription;

public class PrescriptionWithIDs
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    public int IdPrescription { get; private set; }
}