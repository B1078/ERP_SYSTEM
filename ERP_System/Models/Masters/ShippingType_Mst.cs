namespace ERP_System.Models.Masters
{
    public class ShippingType_Mst
    {
        public string? IsActive { get; set; }
        public string? ShipTypeId { get; set; }
        public string? ShipTypeName { get; set; }
        public string? errormessage { get; set; }

        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}