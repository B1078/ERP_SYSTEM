using System.Data.SqlTypes;

namespace ERP_System.Models.User
{
    public class User
    {
        public string? UserId { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UserFname { get; set; }
        public string? UserMname { get; set; }
        public string? UserLname { get; set; }
        public string? UserFulname { get; set; }
        public string? UserName { get; set; }
        public string? UserDob { get; set; }
        public string? UserPwd { get; set; }
        public string? UserMobno { get; set; }
        public string? UserEmail { get; set; }
        public string? UserGender { get; set; }
        public string? Roleid { get; set; }
        public string? UserImage { get; set; }
        public string? dept_id { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public string? BrchId { get; set; }
        public string? UserAddr { get; set; }
        public string? CntryId { get; set; }
        public string? StateId { get; set; }
        public string? UserCity { get; set; }
    }

}
