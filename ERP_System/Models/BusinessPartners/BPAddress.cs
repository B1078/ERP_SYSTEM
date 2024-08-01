
namespace ERP_System.Models.BusinessPartners
{
	public class BPAddress
	{
        public string? BPAddrId { get; set; }
        public string? BPId { get; set; }
        public string? Addr { get; set; }
        public string? AddrType { get; set; }
        public string? BPCity { get; set; }
        public string? CntryId { get; set; }
        public string? StateId { get; set; }
        public string? GSTINNo { get; set; }
        public string? GSTINType { get; set; }


        public string? IsActive { get; set; }
        public string? UpdatedBy { get; set; }

        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }
}
