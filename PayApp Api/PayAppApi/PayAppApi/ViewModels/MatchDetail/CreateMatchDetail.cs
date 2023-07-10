namespace PayAppApi.ViewModels.MatchDetail
{
    public class CreateMatchDetail
    {
        public DateTime MatchDate { get; set; }
        public decimal Amount { get; set; }
        public Guid PayerId { get; set; }
        public ICollection<Guid> UserId { get; set; }
    }
}
