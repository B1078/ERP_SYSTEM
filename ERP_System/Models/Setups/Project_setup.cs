namespace ERP_System.Models.Setups
{
	public class Project_setup
	{
		public string? PrjId { get; set; }
		public string? PrjCode { get; set; }
		public string? PrjName { get; set; }
		public string? ValidFrom { get; set; }
		public string? ValidTo { get; set; }
		public string? IsActive { get; set; }
        public string? errormessage { get; set; }
        public string? UpdatedBy { get; set; }
		public DateOnly? UpdateDate { get; set; }
		public DateOnly? CreateDate { get; set; }
		public string? CreatedBy { get; set; }
		public TimeOnly? CreateTS { get; set; }
		public TimeOnly? UpdateTS { get; set; }
	}
}
