using System.Data.SqlClient;

namespace mock.Prescription;

public class PrescriptionService : IPrescriptionService
{
    private readonly IConfiguration _configuration;

    public PrescriptionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<Prescription>> GetPrescriptions(String? doctorName)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();

        command.CommandText =
            "select IdPrescription, Date, DueDate, P.LastName, D.LastName from Prescription left join dbo.Doctor D on D.IdDoctor = Prescription.IdDoctor left join dbo.Patient P on P.IdPatient = Prescription.IdPatient ";

        if (doctorName != null)
        {
            command.CommandText += " where D.LastName = @doctorName";
            command.Parameters.AddWithValue("@doctorName", doctorName);
        }
        
        command.CommandText += " order by Date desc";

        var reader = await command.ExecuteReaderAsync();
        var prescriptions = new List<Prescription>();

        while (await reader.ReadAsync())
        {
            var prescription = new Prescription
            {
                IdPrescription = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                DueDate = reader.GetDateTime(2),
                PatientLastName = reader.GetString(3),
                DoctorLastName = reader.GetString(4)
            };
            prescriptions.Add(prescription);
        }

        return prescriptions;
    }
    
    public async Task<PrescriptionWithIDs> AddPrescription(PrescriptionWithIDs prescription)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync() as SqlTransaction;
        
        await using var command = connection.CreateCommand();
        command.Transaction = transaction; 

        command.CommandText = "INSERT INTO Prescription (Date, DueDate, IdDoctor, IdPatient) VALUES (@Date, @DueDate, @IdDoctor, @IdPatient); SELECT SCOPE_IDENTITY();";
        command.Parameters.AddWithValue("@Date", prescription.Date);
        command.Parameters.AddWithValue("@DueDate", prescription.DueDate);
        command.Parameters.AddWithValue("@IdDoctor", prescription.IdDoctor);
        command.Parameters.AddWithValue("@IdPatient", prescription.IdPatient);

        try
        {
            var result = await command.ExecuteScalarAsync();
            if (result != null && result != DBNull.Value)
            await transaction.CommitAsync();
            return prescription;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}