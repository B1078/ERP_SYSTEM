namespace ERP_System.Models.Inventory
{
    public class PriceIst_mst
    {
        public string? IsActive { get; set; }

        public string? PListId { get; set; }
        public string? PListName { get; set; }
        public string? IsDefaultPrice { get; set; }
 
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
