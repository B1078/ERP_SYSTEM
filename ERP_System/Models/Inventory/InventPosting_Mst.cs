namespace ERP_System.Models.Inventory
{
    public class InventPosting_Mst
    {
        public string? IsActive { get; set; }
        public string? InvPostId { get; set; }
        public string? PostingDate { get; set; }
        public string? CountDate { get; set; }
        public string? CountTime { get; set; }
        public string? PriceSrc { get; set; }
        public string? PListId { get; set; }
        public string? FinyrId { get; set; }
        public string? FinyrNum { get; set; }
        public string? EOFiscalYearDt { get; set; }
        public string? RefText { get; set; }
        public string? DocNum { get; set; }
        public string? Remarks { get; set; }
        public string? JournalRemarks { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class InvPostingDet_Mst
    {
        public string? InvPostDetId { get; set; }
        public string? InvPostId { get; set; }
        public string? ItemId { get; set; }
        public string? BarCode { get; set; }
        public string? LineNum { get; set; }
        public string? CountedQty { get; set; }
        public string? UomId { get; set; }
        public string? Diff { get; set; }
        public string? Diff_Per { get; set; }
        public string? WhsId { get; set; }
        public string? WhsQty { get; set; }
        public string? BaseDocType { get; set; }
        public string? BaseDocEntry { get; set; }
        public string? BaseDocLineNum { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class InvetPosting_MstAttchment
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
