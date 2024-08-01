namespace ERP_System.Models.User
{
	public class UserAuth
	{
	}

	public class AuthData
	{
		public Maindata[]? Maindata { get; set; }
	}

	public class Maindata
	{
		public string? MenuId { get; set; }
		public string? MenuName { get; set; }
		public string? AuthMnuDetId { get; set; }
		public string? AuthType { get; set; }
		public string? UserId { get; set; }
		public string? UserName { get; set; }
		public DateOnly? UpdateDate { get; set; }
		public TimeOnly? CreateTS { get; set; }
		public TimeOnly? UpdateTS { get; set; }
		public DateOnly? CreateDate { get; set; }
		public string? CreatedBy { get; set; }
		public string? UpdatedBy { get; set; }

	}

}
