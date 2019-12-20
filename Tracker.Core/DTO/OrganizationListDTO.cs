namespace Tracker.Core.DTO
{
    public class OrganizationListDTO
    {
        public int Id { get; set; }
        public int NewPEID { get; set; }
        public int OldPEID { get; set; }
        public StaticTypes.OrganizationStanding Standing { get; set; }
    }
}
