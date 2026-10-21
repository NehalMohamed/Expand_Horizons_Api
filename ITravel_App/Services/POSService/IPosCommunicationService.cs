using ITravel_App.Models.POS;

namespace ITravel_App.Services.POSService
{
    public interface IPosCommunicationService
    {
        Task<PosPaymentResponse> ProcessPaymentAsync(
            PosPaymentRequest request);
} 
}
