namespace WebApplication1.Common.DTO
{
    public class PaymentSummaryDTO
    {
        public PaymentSummaryDTO(int paymentId, decimal ammount, DateTime date)
        {
            PaymentId = paymentId;
            Ammount = ammount;
            Date = date;
        }

        public int PaymentId { get; set; }
        public decimal Ammount { get; set; }
        public DateTime Date { get; set; }
    }
}
