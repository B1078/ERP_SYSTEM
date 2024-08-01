namespace ERP_System.Models.BusinessPartners
{
	public class BPBankDetails
	{
        public string? BPBankDetId { get; set; }
        public string? BPId { get; set; }
        public string? BankId { get; set; }
        public string? ACType { get; set; }
        public string? ACNo { get; set; }
        public string? IsActive { get; set; }
        public string? UpdatedBy { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }
}
