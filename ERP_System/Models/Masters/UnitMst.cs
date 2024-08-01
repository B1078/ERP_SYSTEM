namespace ERP_System.Models.Masters
{
    public class UnitMst 
    {
        public string? UomId { get; set; }
        public string? UomCode { get; set; }
        public string? UomName { get; set; }
        public string? IsActive { get; set; }
        public decimal? EWBUnit { get; set; }
        public decimal? Height { get; set; }
        public decimal? Length { get; set; }
        public string? errormessage { get; set; }
        public string? IsManual { get; set; }

        public decimal? Width { get; set; }
        public decimal? Volume { get; set; }
        public string? VolumeUom { get; set; }
        public decimal? Weight { get; set; }
        public string? UpdatedBy { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }
    public class UnitConversionSetup
    {
        public string? SetupId { get; set; }
        public int FromUom { get; set; }
        public int FromVolume { get; set; }
        public int ToUom { get; set; }
        public int ToVolume { get; set; }
        public string? IsActive { get; set; }
        public string? UpdatedBy { get; set; }
        public string? errormessage { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }
    public class EWBMst
    {
        public string? EWBUomId { get; set; }
        public string? EWBUomCode { get; set; }
        public string? EWBUomName { get; set; }
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
