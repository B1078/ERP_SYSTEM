namespace ERP_System.Models.Inventory
{
    public class ItemMaster
    {
        public string? IsActive { get; set; }
        public string? ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public string? ItemGrpId { get; set; }
        public string? UomGrpId { get; set; }
        public string? UnitPrice { get; set; }
        public string? CntryId { get; set; }
        public string? PListId { get; set; }
        public string? BarCodeId { get; set; }
        public string? Remarks { get; set; }
        public string? ItemInvnt { get; set; }
        public string? ItemSale { get; set; }
        public string? ItemPur { get; set; }
        public string? ItemFixAsset { get; set; }
        public string? WHTaxLiable { get; set; }
        public string? ManufId { get; set; }
        public string? ShipTypeId { get; set; }
        public string? ItemManageBy { get; set; }
        public string? ItemExcisable { get; set; }
        public string? ItemGST { get; set; }
        public string? ItemMatType { get; set; }
        public string? HSNNo { get; set; }
        public string? TaxCategory { get; set; }
        public string? BPId { get; set; }
        public string? PurUOM { get; set; }
        public string? ItemPerPurUnit { get; set; }
        public string? PurPackingUOM { get; set; }
        public string? PurQtyPerPack { get; set; }
        public string? PurLength { get; set; }
        public string? Purwidth { get; set; }
        public string? PurHeight { get; set; }
        public string? PurVolume { get; set; }
        public string? PurVUomId { get; set; }
        public string? PurWeight { get; set; }
        public string? SalsUOM { get; set; }
        public string? ItemPerSalsUnit { get; set; }
        public string? SalsPackingUOM { get; set; }
        public string? SalsQtyPerPack { get; set; }
        public string? SalsLength { get; set; }
        public string? Salswidth { get; set; }
        public string? SalsHeight { get; set; }
        public string? SalsVolume { get; set; }
        public string? SalsVUomId { get; set; }
        public string? SalsWeight { get; set; }
        public string? ItemInvntMthd { get; set; }
        public string? InvntUOM { get; set; }
        public string? ReordrQty { get; set; }
        public string? MinimumLvl { get; set; }
        public string? ValueMthd { get; set; }
        public string? MaximumLvl { get; set; }
        public string? PlanMthd { get; set; }
        public string? ProcMthd { get; set; }
        public string? MinOrdrQty { get; set; }
        public string? LeadTime { get; set; }
        public string? TolrDays { get; set; }
        public string? ItemPhantom { get; set; }
        public string? IssueMthd { get; set; }
        public string? ProdStdCost { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class ItemAttachmentlist
    {
        public List<ItemAttachment>? Data { get; set; }
    }
    public class ItemAttachment
    {

        public string? AttachDetId { get; set; }
        public string? ItemId { get; set; }
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


    public class InventoryTbl
    {
        public string? InvtDetId { get; set; }
        public string? WhsLocked { get; set; }
        public string? MinStock { get; set; }
        public string? MaxStock { get; set; }
        public string? MinOrder { get; set; }
        public string? ItemId { get; set; }
        public string? WhsId { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
    public class BarCodetbl {
        public string? BarCodeDetId { get; set; }
        public string? ItemId { get; set; }
        public string? BarCode { get; set; }
        public string? UomId { get; set; }
        public string? Remark { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }

    }
}
