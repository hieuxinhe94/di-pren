namespace Di.Subscription.Logic.Reclaim.Types
{
    public class ReclaimType
    {
        public int Id { get; set; }
        public string ReclaimText { get; set; }
        public string ReclaimKind { get; set; }
        public bool Compensation { get; set; }
        public bool CarrierMessage { get; set; }
        public int OrderNumber { get; set; }
        public bool ReclaimPaper { get; set; }
    }
}
