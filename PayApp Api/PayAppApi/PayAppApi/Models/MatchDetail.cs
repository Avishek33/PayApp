namespace PayAppApi.Models
{
    public class MatchDetail
    {
        public Guid Id { get; set; }
        public DateTime MatchDate { get; set; } 
        public decimal Amount { get; set; } 
        public Guid PayerId { get; set; } 
        public User User { get; set; }
        public ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
