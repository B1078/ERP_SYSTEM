namespace ERP_System.Models.BusinessPartners
{
    public class BusinessPartnerGroupMaster
    {
        public string? IsActive { get; set; }
        public string? BPGrpId { get; set; }
        public string? BPGrpName { get; set; }
        public string? BPGrpType { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
