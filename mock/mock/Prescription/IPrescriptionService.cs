namespace mock.Prescription;

public interface IPrescriptionService
{
    Task<List<PrescriptionWithNames>> GetPrescriptions(String? doctorName);
    
    Task<PrescriptionWithIDs> AddPrescription(PrescriptionWithIDs prescription);
}