namespace ERP_System.Models.Setups
{
    public class TaxCombinationSetup
    {

        public string? TaxCodeId { get; set; }
        public string? TaxCode { get; set; }
        public string? TaxFormula { get; set; }
        public string? TaxCalRate { get; set; }
        public string? IsActive { get; set; }
        public string? Freight_app { get; set; }
        public string? UpdatedBy { get; set; }
        public string? errormessage { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
    }  
    public class TaxCombibnationForm
    {

        public string? TaxRate { get; set; }
        public string? TaxTypeId { get; set; }
        public string? TaxCodeFormId { get; set; }
        public string? TaxCodeId { get; set; }
       
    }

}
