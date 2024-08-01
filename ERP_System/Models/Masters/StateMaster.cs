namespace ERP_System.Models.Masters
{
    public class StateMaster
    {
        public string? IsActive { get; set; }
        public string? StateId { get; set; }
        public string? StateCode { get; set; }
        public string? StateName { get; set; }
        public string? errormessage { get; set; }
        public string? CntryId { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
