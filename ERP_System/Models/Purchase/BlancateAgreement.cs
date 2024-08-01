namespace ERP_System.Models.Purchase
{

    public class BlancateAgreement
    {
        public string? IsActive { get; set; }
        public string? BlKAId { get; set; }
        public string? BPId { get; set; }
        public string? ConnBPId { get; set; }
        public string? RefNo { get; set; }
        public string? BPCurrId { get; set; }
        public string? BPPhone1 { get; set; }
        public string? BPEmail { get; set; }
        public string? FinyrId { get; set; }
        public string? Num { get; set; }
        public string? AgrmtMthd { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Trans_type { get; set; }
        public string? PListId { get; set; }

        public string? PrjId { get; set; }
        public string? TermDate { get; set; }
        public string? SignDate { get; set; }
        public string? Description { get; set; }
        public string? AgrmtType { get; set; }

        public string? IgnorePrice { get; set; }
        public string? PymntTId { get; set; }
        public string? PymntMthd { get; set; }
        public string? ShipTypeId { get; set; }
        public string? SetProbPer { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public string? EmpId { get; set; }
        public string? Renewal { get; set; }
        public string? RemindVal { get; set; }
        public string? DocNum { get; set; }
        public string? DraftFlag { get; set; }
        public string? RemindUnit { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class BlKItem_Det_mst
    {
        public string? BlKAId { get; set; }
        public string? UomId { get; set; }
        public string? BlKItemId { get; set; }
        public string? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemGrpId { get; set; }
        public string? PlanQty { get; set; }
        public string? UnitPrice { get; set; }
        public string? ItemShipTypeId { get; set; }
        public string? ItemPrjId { get; set; }
        public string? CntryId { get; set; }
        public string? RetPortion { get; set; }
        public string? WrrtyEnd { get; set; }
        public string? RowStatus { get; set; }
        public string? BarCode { get; set; }
        public string? LineNum { get; set; }
        public string? OpenQty { get; set; }
        public string? PriceUpd { get; set; }
        public string? ActualPrice { get; set; }
        public string? ItemRemark { get; set; }
    }
    public class PurAgrementAttachment
    {

        public string? BlKAttachId { get; set; }
        public string? BlKAId { get; set; }
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

}
