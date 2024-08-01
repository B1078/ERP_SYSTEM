namespace ERP_System.Models.Setups
{
    public class Depart_Mst
    {
        public string? DeptId { get; set; }
        public string? DeptName { get; set; }
        public string? DeptDesc { get; set; }
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
