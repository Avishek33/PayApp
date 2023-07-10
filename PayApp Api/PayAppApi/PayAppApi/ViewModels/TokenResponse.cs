namespace PayAppApi.ViewModels
{
    public class TokenResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string ExpiryDate { get; set; }
    }
}
