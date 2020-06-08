namespace Pren.Web.Business.ServicePlus.Models
{
    public class BizSubscriber
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Status { get; set; }
        public string RemovalDateString { get; set; }
    }
}
