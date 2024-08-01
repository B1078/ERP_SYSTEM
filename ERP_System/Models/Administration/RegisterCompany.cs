namespace ERP_System.Models.Administration
{
    public class RegisterCompany
    {
        public string? CmpId { get; set; }
        public string? CmpCode { get; set; }
        public string? CmpName { get; set; }
        public string? CmpPanno { get; set; }
        public string? CmpTanno { get; set; }
        public string? CmpEstbDt { get; set; }
        public string? CmpRegAddr { get; set; }
        public string? CmpRegCity { get; set; }
        public string? CmpRgstateId { get; set; }
        public string? CmpRegNo { get; set; }
        public string? cmpRgcntryId { get; set; }
        public string? CmpCaddr { get; set; }
        public string? CmpCcity { get; set; }
        public string? CmpCstId { get; set; }
        public string? CmpCcntId { get; set; }
        public string? CmpEmailid { get; set; }
        public string? CmpPhone { get; set; }
        public string? CmpWeburl { get; set; }
        public string? CmpMobno { get; set; }
        public string? CmpGstin { get; set; }
        public string? CmpCINno { get; set; }
        public string? Cmp_ValidFrdt { get; set; }
        public string? Cmp_ValidTodt { get; set; }
        public string? db_UserId { get; set; }
        public string? db_pwd { get; set; }
        public string? db_name { get; set; }
        public string? IsActive { get; set; }

        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
