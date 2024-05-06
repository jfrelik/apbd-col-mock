namespace mock.Prescription;

public static class Configuration
{
    public static void RegisterEndpointsForPrescription(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/prescriptions", async (IPrescriptionService service, String? doctorName) =>
        {
            var prescriptions = await service.GetPrescriptions(doctorName);
     
            return prescriptions.Count == 0 ? Results.NotFound() : Results.Ok(prescriptions);
        });

        app.MapPost("api/prescriptions", async (IPrescriptionService service, PrescriptionWithIDs presc) =>
        {
            var prescription = await service.AddPrescription(presc);
            return Results.Created("api/prescriptions", prescription);
        });
    }
}