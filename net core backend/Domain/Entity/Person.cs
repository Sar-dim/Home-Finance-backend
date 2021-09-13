namespace net_core_backend.Models
{
    public class Person : Base.Base
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
