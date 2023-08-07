using System.Net;

namespace LaRosalinaAPI.Models
{
    public class LoginReply
    {
        public LoginReply()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
