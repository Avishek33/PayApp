namespace PayAppApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<PaymentDetail> PaymentDetails { get; set; }
        public MatchDetail MatchDetail { get; set; }   
    }
}
