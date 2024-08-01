namespace ERP_System.Models.User
{
  
    public class MenuMst
    {
        public string? MenuId { get; set; }
        public string? MenuName { get; set; }
        public string? MenuUrl { get; set; }
        public string? ParentMenuId { get; set; }
        public string? MenuPos { get; set; }
        public string? UpdatedBy { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }

}
