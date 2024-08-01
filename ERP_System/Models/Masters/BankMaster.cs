namespace ERP_System.Models.Masters
{
    public class BankMaster
    {

        public string? IsActive { get; set; }
        public string? BankId { get; set; }
        public string? BankName { get; set; }
        public string? BankBranch { get; set; }
        public string? BankIFSCcd { get; set; }
        public string? BankSwiftcd { get; set; }
        public string? BankAddr { get; set; }
        public string? BankCity { get; set; }
        public string? CntryId { get; set; }
        public string? StateId { get; set; }
        public string? errormessage { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
