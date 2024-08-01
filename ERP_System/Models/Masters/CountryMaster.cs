namespace ERP_System.Models.Masters
{
    public class CountryMaster
    {
        public string? IsActive { get; set; }
        public string? CntryId { get; set; }
        public string? CntryCode { get; set; }
        public string? CntryName { get; set; }
        public string? IsoCntryCode { get; set; }
        public string? CntryCurrName { get; set; }
        public string? CntryCurrCode { get; set; }
        public string? CntryCurrSymbol { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public string? errormessage { get; set; }
    }
}
