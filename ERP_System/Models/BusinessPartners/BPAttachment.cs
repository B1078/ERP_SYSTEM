namespace ERP_System.Models.BusinessPartners
{
	public class BPAttachment
	{
       public string? BPAttachId {get;set;}
       public string? BPId {get;set;}
       public string? FileName {get;set;}
       public string? FileExt {get;set;}
       public string? FileData {get;set;}
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
