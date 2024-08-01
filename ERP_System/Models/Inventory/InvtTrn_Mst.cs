namespace ERP_System.Models.Inventory
{
    public class InvtTrn_Mst
    {
        public string? IsActive { get; set; }
        public string? InvTransId { get; set; }
        public string? BPId { get; set; }
        public string? BPContPerId { get; set; }
        public string? BPAddrId { get; set; }
        public string? FinyrId { get; set; }
        public string? FinyrNum { get; set; }
        public string? Status { get; set; }
        public string? PostingDt { get; set; }
        public string? DueDt { get; set; }
        public string? DocumentDt { get; set; }
        public string? SalsEmpId { get; set; }
        public string? PListId { get; set; }
        public string? DutyStatus { get; set; }
        public string? DocType { get; set; }
        public string? ShipToAddr { get; set; }
        public string? JrnlMemo { get; set; }
        public string? DocNum { get; set; }
        public string? Remarks { get; set; }
        public string? PickRmrk { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class InvTransDet_Mst
    {
        public string? InvTransDetId { get; set; }
        public string? InvTransId { get; set; }
        public string? ItemId { get; set; }
        public string? LineNum { get; set; }
        public string? Qty { get; set; }
        public string? QtyInWhs { get; set; }
        public string? Price { get; set; }
        public string? UomId { get; set; }
        public string? CntryId { get; set; }
        
        public string? WhsId { get; set; }
        public string? FromWhsId { get; set; }
        public string? LocName { get; set; }
        public string? BaseDocType { get; set; }
        public string? BaseDocEntry { get; set; }
        public string? BaseDocLineNum { get; set; }
        public string? RowStatus { get; set; }
        public string? BarCode { get; set; }
        public string? OpenQty { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class Item_TransAttchment
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
}

