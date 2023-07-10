using PayAppApi.Common;

namespace PayAppApi.ViewModels.PaymentDetail
{
    public class PaymentDetailVm
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusInString { get; set; }
        public Guid MatchId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
