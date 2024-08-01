namespace ERP_System.Models.BusinessPartners
{
    public class BusinessPartnerMaster
    {
        public string? IsActive { get; set; }
        public string? BPId { get; set; }
        public string? BPCode { get; set; }
        public string? BPName { get; set; }
        public string? BPAliasName { get; set; }
        public string? BPGrpId { get; set; }
        public string? BPCardType { get; set; }
        public string? BPAddr { get; set; }
        public string? BPCity { get; set; }
        public string? CntryId { get; set; }
        public string? StateId { get; set; }
        public string? BPMobNo { get; set; }
        public string? BPEmail { get; set; }
        public string? BPPhone1 { get; set; }
        public string? BPPhone2 { get; set; }
        public string? BPPANNo { get; set; }
        public string? BPITRFilling { get; set; }
        public string? BPACBal { get; set; }
        public string? BPCurrType { get; set; }
        public string? BPCurrId { get; set; }
        public string? FatherCardId { get; set; }
        public string? FatherType { get; set; }
        public string? ConnBPId { get; set; }
        public string? PlngGroup { get; set; }
        public string? PaymBlock { get; set; }
        public string? SinglePaym { get; set; }
        public string? PaymBlockRemark { get; set; }
        public string? BankAllCodeId { get; set; }
        public string? HouseBankId { get; set; }
        public string? WTLiable { get; set; }
        public string? WT_appid { get; set; }
        public string? Ass_WT_type { get; set; }
        public string? WTType { get; set; }
        public string? PymntTId { get; set; }
        public string? IntrstRate { get; set; }
        public string? PListId { get; set; }
        public string? Discount { get; set; }
        public string? CredLimit { get; set; }
        public string? CommitLimit { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class PaymentTerms_Mst
    {
        public string? IsActive { get; set; }

        public string? PymntTId { get; set; }
        public string? PymntCode { get; set; }
        public string? BslineDate { get; set; }
        public string? NoOfDays { get; set; }
        public string? TolDays { get; set; }
        public string? InstNo { get; set; }
        public string? OpenRcpt { get; set; }
        public string? DiscCode { get; set; }
        public string? TotDiscnt { get; set; }
        public string? LatePyChrg { get; set; }
        public string? CredLimit { get; set; }
        public string? CommitLimit { get; set; }
        public string? CrdMthd { get; set; }
        public string? VATFirst { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }


    }
    public class Installment_mst
    {
        public string? InstlmntId { get; set; }
        public string? InstNo { get; set; }
        public string? PymntTId { get; set; }
        public string? InstMonth { get; set; }
        public string? InstDays { get; set; }
        public string? InstPercnt { get; set; }
    }
    public class CashDis_Mst
    {
        public string? CashDisId { get; set; }
        public string? PymntTId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ByDate { get; set; }
        public string? Freight { get; set; }
        public string? BaseDate { get; set; }
    }
    public class CashDet_Mst
    {
        public string? CashDisDetId { get; set; }
        public string? CashDisId { get; set; }
        public string? NumOfDays { get; set; }
        public string? Discount { get; set; }
        public string? Day { get; set; }
        public string? Month { get; set; }

    }
    public class BPTaxinfo_Mst
    {
        public string? BPTaxId { get; set; }
        public string? BPId { get; set; }
        public string? AddrType { get; set; }
        public string? TaxId0 { get; set; }
        public string? TaxId1 { get; set; }
        public string? TaxId2 { get; set; }
        public string? TaxId3 { get; set; }
        public string? TaxId4 { get; set; }
        public string? TaxId5 { get; set; }
        public string? TaxId6 { get; set; }
        public string? TaxId7 { get; set; }
        public string? TaxId8 { get; set; }
        public string? TaxId9 { get; set; }
        public string? TaxId10 { get; set; }
        public string? TaxId11 { get; set; }
        public string? TaxId12 { get; set; }
        public string? TaxId13 { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }

    }

}
