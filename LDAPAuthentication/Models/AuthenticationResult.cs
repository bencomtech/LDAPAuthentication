namespace LDAPAuthentication.Models
{
    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        private AuthenticationResult()
        {
        }

        public static AuthenticationResult SUCCESS()
        {
            return new AuthenticationResult()
            {
                IsSuccess = true
            };
        }

        public static AuthenticationResult FAILED(string message)
        {
            return new AuthenticationResult()
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}