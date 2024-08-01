namespace ERP_System.Models.Masters
{
    public class WarehouseMaster
    {
        public string? WhsId { get; set; }
        public string? WhsCode { get; set; }
        public string? WhsName { get; set; }
        public string? WhsAddr { get; set; }
        public string? BrchId { get; set; }
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
