namespace ITravel_App.Models.POS
{
    public class PosPaymentRequest
    {
        public string? TrxType { get; set; }
        public decimal Amount { get; set; }

        public string? RefNum { get; set; }

        public string? ReceiptNum { get; set; }
    }
}
