namespace ERP_System.Models.Setups
{
    public class BankAllocationCodeSetup
    {
          public string? BankAllCodeId { get;set;}
          public string? Name { get;set;}
          public string? Code { get;set;}
          public string? IsActive { get; set; }
          public DateOnly? UpdateDate { get; set; }
        public string? errormessage { get; set; }
        public string? UpdatedBy { get; set; }
          public TimeOnly? UpdateTS { get; set; }
          public DateOnly? CreateDate { get; set; }
          public string? CreatedBy { get; set; }
          public TimeOnly? CreateTS { get; set; } 
    }
}
