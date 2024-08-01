namespace ERP_System.Models.Masters
{
    public class LocationMaster
    {

        public string? LocId { get; set; }
        public string? LocCode { get; set; }
        public string? LocName { get; set; }
        public string? errormessage { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
