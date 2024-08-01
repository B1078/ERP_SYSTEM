namespace ERP_System.Models.Setups
{
    public class VenderTypeSetup
    {
        public string? VendTypeId { get; set; }
        public string? VendType { get; set; }
        public string? VendDesc { get; set; }
        public string? errormessage { get; set; }
        public string? IsActive { get; set; }
        public string? UpdatedBy { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }
}
