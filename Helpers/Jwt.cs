namespace Movies_Api.Helpers
{
    public class Jwt
    {
        public string Key { set; get; }
        public string Issuer { set; get; }
        public string Audience { set; get; }
        public double DurationInDays { set; get; }
    }
}
