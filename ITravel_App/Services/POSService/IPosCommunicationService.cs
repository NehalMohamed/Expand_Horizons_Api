namespace ITravel_App.Services.POSService
{
    public interface IPosCommunicationService
    {
        Task<string> SendAsync(string xml);
        Task<string> ConnectionTestAsync();

      

        bool IsConnected();
    }
}
