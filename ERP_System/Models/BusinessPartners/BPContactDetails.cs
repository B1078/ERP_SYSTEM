namespace ERP_System.Models.BusinessPartners
{
	public class BPContactDetails
	{
        public string? BPContDetId { get; set; }
        public string? BPId { get; set; }
        public string? Title { get; set; }
        public string? Fname { get; set; }
        public string? Mname { get; set; }
        public string? Lname { get; set; }
        public string? Addr { get; set; }
        public string?Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? MobNo { get; set; }
        public string?Email { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? BPCity { get; set; }
        public string? CntryId { get; set; }
        public string? StateId { get; set; }
        public string? IsActive { get; set; }
        public string? UpdatedBy { get; set; }

        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }
}
