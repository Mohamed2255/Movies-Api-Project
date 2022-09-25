namespace Movies_Api.Model
{
    public class Authmodel
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }   
        public  bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }    
    }
}
