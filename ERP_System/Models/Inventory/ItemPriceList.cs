namespace ERP_System.Models.Inventory
{
    public class ItemPriceList
    {
        public string? IsActive { get; set; }
        public string? PListDetId { get; set; }
        public string? ItemId { get; set; }
        public string? PListId { get; set; }
        public string? BasePrice { get; set; }
        public string? Factor { get; set; }
        public string? Discount { get; set; }
        public string? Price { get; set; }
        public string? IsManual { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
