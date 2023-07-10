using PayAppApi.Common;

namespace PayAppApi.Models
{
    public class PaymentDetail
    {
        public Guid Id { get; set; } 
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Guid MatchId { get; set; }
        public MatchDetail MatchDetail { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
