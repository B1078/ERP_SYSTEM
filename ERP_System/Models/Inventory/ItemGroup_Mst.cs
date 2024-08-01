namespace ERP_System.Models.Inventory
{
    public class ItemGroup_Mst
    {
        public string? IsActive { get; set; }
        public string? ItemGrpId { get; set; }
        public string? ItemGrpname { get; set; }
        public string? ItemGrpType { get; set; }

        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }

    }
}
