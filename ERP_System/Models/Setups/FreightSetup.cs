namespace ERP_System.Models.Setups
{
    public class FreightSetup_mst
    {
        public string? FretId { get; set; }
        public string? FretName { get; set; }
        public string? RevAcct { get; set; }
        public string? ExpnsAcct { get; set; }
        public string? WTLiable { get; set; }
        public string? PrjId { get; set; }
        public string? errormessage { get; set; }
        public string? ServiceId { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Fret_type { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }
}
