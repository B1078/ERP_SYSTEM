namespace ERP_System.Models.Masters
{

    public class BranchMaster
    {
        public string? BrchId { get; set; }
        public string? LocId { get; set; }
        public string? BrchName { get; set; }
        public string? BrchCity { get; set; }
        public string? StateId { get; set; }
        public string? CntryId { get; set; }
        public string? errormessage { get; set; }
        public string? BrchAddr { get; set; }
        public string? BrchGSTINNo { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }

}
