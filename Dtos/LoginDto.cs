namespace HotelsManager.Dtos

{
    public class LoginDto
    {
        public string? ReturnUrl { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public LoginDto(string _rUrl, string _login, string _password) { 
            ReturnUrl = _rUrl;
            Login = _login;
            Password = _password;
        }
    }
}