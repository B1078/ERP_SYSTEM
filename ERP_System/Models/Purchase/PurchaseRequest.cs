namespace ERP_System.Models.Purchase
{
    public class PurchaseRequest
    {
        public string? IsActive { get; set; }
        public string? PurReqId { get; set; }
        public string? DocNum { get; set; }
        public string? DraftFlag { get; set; }
        public string? ReqtId { get; set; }
        public string? ReqtType { get; set; }
        public string? EmailAddr { get; set; }
        public string? Reqtname { get; set; }
        public string? DeptId { get; set; }
        public string? EmpId { get; set; }
        public string? Notify { get; set; }
        public string? StateId { get; set; }
        public string? BrchId { get; set; }
        public string? FinyrId { get; set; }
        public string? BrchGSTINNo { get; set; }
        public string? FinyrNum { get; set; }
        public string? TotBefDis { get; set; }
        public string? Freight { get; set; }
        public string? Tax { get; set; }
        public string? TotPayDue { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public string? PostingDt { get; set; }
        public string? ValidUntilDt { get; set; }
        public string? DocumentDt { get; set; }
        public string? RequiredDt { get; set; }
        public string? DocType { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class purchase_de_mst
    {
        public string? PurRDetId { get; set; }
        public string? PurReqId { get; set; }
        public string? ItemId { get; set; }
        public string? BPId { get; set; }
        public string? ItemReqdDt { get; set; }
        public string? InfoPrice { get; set; }
        public string? Qty { get; set; }
        public string? ActualPrice { get; set; }
        public string? PriceUpd { get; set; }
        public string? OpenQty { get; set; }
        public string? RowStatus { get; set; }
        public string? TaxAmountLC { get; set; }
        public string? LineNum { get; set; }
        public string? TotalLC { get; set; }
        public string? DisPer { get; set; }
        public string? TaxCodeId { get; set; }
        public string? UomId { get; set; }
        public string? CntryId { get; set; }
        public string? WhsId { get; set; }
        public string? LocName { get; set; }
        public string? ItemBrchId { get; set; }
        public string? ItemHSNNo { get; set; }
        public string? ServiceId { get; set; }
        public string? BarCode { get; set; }
        //public string? MRP { get; set; }
        //public string? NetAmt { get; set; }
        public string? BaseDocType { get; set; }
        public string? BaseDocEntry { get; set; }
        public string? BaseDocLineNum { get; set; }
        public string? CreditMemoQty { get; set; }
        public string? GLAcctName { get; set; }
       
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class PurAggrementAttachment
    {

        public string? PurRAttchDetId { get; set; }
        public string? PurReqId { get; set; }
        public string? Remark { get; set; }
        public string? FileName { get; set; }
        public string? FileExt { get; set; }
        public string? FileData { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class Freight_Detail {

        public string? PurRFretId { get; set; }
        public string? PurReqId { get; set; }
        public string? FretId { get; set; }
        public string? Remarks { get; set; }
        public string? TaxCodeId { get; set; }
        public string? TotTaxAmount { get; set; }
        public string? NetAmount { get; set; }
        public string? Status { get; set; }
        public string? PrjId { get; set; }
        public string? GrossAmount { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
