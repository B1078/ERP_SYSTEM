namespace ERP_System.Models.Setups
{
    public class Wtholding_mst
    {
        public string? IsActive { get; set; }

        public string? WTId { get; set; }
        public string? WTCode { get; set; }
        public string? WTName { get; set; }
        public string? Rate { get; set; }
        public string? EffecDate { get; set; }
        public string? Category { get; set; }
        public string? BaseType { get; set; }
        public string? errormessage { get; set; }
        public string? Section { get; set; }
        public string? LocId { get; set; }
        public string? ReturnType { get; set; }
        public string? TDSType { get; set; }
        public string? Assessee { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class Wtholding_mst_def
    {
        public string? WTDetId { get; set; }
        public string? Rate { get; set; }
        public string? EffecDate { get; set; }
        public string? WTId { get; set; }
        public string? TdsRate { get; set; }
       
        
    }
}
