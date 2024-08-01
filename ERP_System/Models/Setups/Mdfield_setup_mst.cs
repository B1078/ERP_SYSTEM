namespace ERP_System.Models.Setups
{

    public class Mdfield_setup_mst
    {
        public string? MenuId { get; set; }
        public string? MTMandId { get; set; }
        public string? TableName { get; set; }
        public string? FieldName { get; set; }
        public string? IsMandatory { get; set; }
        public string? errormessage { get; set; }
        public string? UpdatedBy { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }

}
