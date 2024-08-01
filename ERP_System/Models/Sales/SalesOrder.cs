namespace ERP_System.Models.Sales
{
    public class SalesOrder
    {
        public string? IsActive { get; set; }
        public string? SalsOrderId { get; set; }
        public string? BPId { get; set; }
        public string? BPContPerId { get; set; }
        public string? VendorRefNo { get; set; }
        public string? CurSource { get; set; }
        public string? CntryId { get; set; }
        public string? StateId { get; set; }
        public string? BPAddrId { get; set; }
        public string? BrchId { get; set; }
        public string? BrchGSTINNo { get; set; }
        public string? FinyrId { get; set; }
        public string? FinyrNum { get; set; }
        public string? Status { get; set; }
        public string? PostingDt { get; set; }
        public string? DeliveryDt { get; set; }
        public string? RequiredDt { get; set; }
        public string? DocumentDt { get; set; }
        public string? SalsEmpId { get; set; }
        public string? EmpId { get; set; }
        public string? DocType { get; set; }
        public string? ShipToAddr { get; set; }
        public string? BillToAddr { get; set; }
        public string? ShipTypeId { get; set; }
        public string? JrnlMemo { get; set; }
        public string? PymntTId { get; set; }
        public string? PrjId { get; set; }
        public string? DutyStatus { get; set; }
        public string? TotBefDis { get; set; }
        public string? Freight { get; set; }
        public string? IsRounding { get; set; }
        public string? Tax { get; set; }
        public string? DisPer { get; set; }
        public string? DisAmt { get; set; }
        public string? TotPayDue { get; set; }
        public string? DocNum { get; set; }
        public string? DraftFlag { get; set; }
        public string? Remarks { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class SalesOrder_Item
    {
        public string? SalsODetId { get; set; }
        public string? SalsOrderId { get; set; }
        public string? ItemId { get; set; }
        public string? LineNum { get; set; }
        public string? InfoPrice { get; set; }
        public string? PriceAftDis { get; set; }
        public string? Qty { get; set; }
        public string? RowStatus { get; set; }
        public string? ActualPrice { get; set; }
        public string? PriceUpd { get; set; }
        public string? PrevQty { get; set; }
        public string? Editflag { get; set; }
        public string? OpenQty { get; set; }
        //public string? QtyInWhs { get; set; }
        public string? TaxAmountLC { get; set; }
        public string? TotalLC { get; set; }
        public string? CalcTotalLC { get; set; }
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
        //public string? Weight { get; set; }
        public string? BlKAId { get; set; }
        public string? BaseDocType { get; set; }
        public string? BaseDocEntry { get; set; }
        public string? BaseDocLineNum { get; set; }
        public string? CreditMemoQty { get; set; }
        public string? GLAcctName { get; set; }
        public string? Remarks { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class SalesOrderAttchment
    {

        public string? AttchDetId { get; set; }
        public string? DocId { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? FileExt { get; set; }
        public string? FileData { get; set; }
        public string? Remark { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class SalesOrder_Freight
    {

        public string? SalsOFretId { get; set; }
        public string? SalsOrderId { get; set; }
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
    public class SOTaxinfo_Mst
    {
        public string? SalsOTaxDetId { get; set; }
        public string? SalsOrderId { get; set; }
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
        public string? GSTINNo { get; set; }
        public string? GSTINType { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }

    }
}
