namespace mock.Prescription;

public interface IPrescriptionService
{
    Task<List<Prescription>> GetPrescriptions(String? doctorName);
    
    Task<PrescriptionWithIDs> AddPrescription(PrescriptionWithIDs prescription);
}