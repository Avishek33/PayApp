namespace PayAppApi.ViewModels.MatchDetail
{
    public class MatchDetailVm
    {
        public Guid Id { get; set; }
        public string MatchDateInString { get; set; }
        public DateTime MatchDate { get; set; }
        public decimal Amount { get; set; }
        public Guid PayerId { get; set; }
        public string PayerName { get; set; }
    }
}
