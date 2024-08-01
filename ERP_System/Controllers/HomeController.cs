using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ERP_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration; // Add this field
        private string ConnectionString;
        private string ConnectionStringSys;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; // Inject IConfiguration
             ConnectionString = _configuration["Erp_Db_Con"];
             ConnectionStringSys = _configuration["ErpSys_Db_Con"];
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult AccountProfile()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(LoginModel Obj)
        {
            if (string.IsNullOrEmpty(Obj.User_Name) && string.IsNullOrEmpty(Obj.Password) && string.IsNullOrEmpty(Obj.Comp_Code))
            {
                ViewData["ErrorMessageUserName"] = "Please enter your username.";
                ViewData["ErrorMessagePassword"] = "Please enter your Password.";
                ViewData["ErrorMessageCompany"] = "Select your Company Code.";
                return View();
            }
            if (string.IsNullOrEmpty(Obj.Comp_Code))
            {
                ViewData["ErrorMessageCompany"] = "Please Select your Company Code.";
                return View();
            }
            if (string.IsNullOrEmpty(Obj.User_Name))
            {
                ViewData["ErrorMessageUserName"] = "Please enter your username.";
                return View();
            }
            if (string.IsNullOrEmpty(Obj.Password))
            {
                ViewData["ErrorMessagePassword"] = "Please enter your Password.";
                return View();
            }
            try
            {
                bool IdvalidComp = false;
                string Query1 = @"Select * from cmp_Mst where  CmpCode='"+ Obj.Comp_Code+ "' And IsActive='Y'";
                using (SqlConnection con = new SqlConnection(ConnectionStringSys))
                {
                    using (SqlCommand cmd = new SqlCommand(Query1, con))
                    {
                        con.Open();
                        cmd.CommandText = Query1;
                        cmd.CommandTimeout = 300;
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            string NewCon = ReplaceDatabaseName(ConnectionString, rdr["db_name"].ToString());
							HttpContext.Session.SetString("ConnectionString", NewCon);
							HttpContext.Session.SetString("cmp_id", rdr["CmpId"].ToString());
							HttpContext.Session.SetString("cmp_name", rdr["CmpName"].ToString());
							HttpContext.Session.SetString("cmp_phone", rdr["CmpMobno"].ToString());
							HttpContext.Session.SetString("cmp_emailid", rdr["CmpEmailid"].ToString());
							HttpContext.Session.SetString("cmp_reg_addr", rdr["CmpRegAddr"].ToString());
							HttpContext.Session.SetString("cmp_gstin", rdr["CmpGstin"].ToString());
							HttpContext.Session.SetString("CmpPanno", rdr["CmpPanno"].ToString());
							HttpContext.Session.SetString("cmp_code", rdr["CmpCode"].ToString());
							HttpContext.Session.SetString("Cmp_ValidFrdt", rdr["Cmp_ValidFrdt"].ToString());
							HttpContext.Session.SetString("Cmp_ValidTodt", rdr["Cmp_ValidTodt"].ToString());
                            IdvalidComp = true;
                        }
                        else
                        {
                            ViewData["Wrong_Credeintials"] = "Please enter Valid Company Code.";
                            return View();


                        }
                    }
                }
                ConnectionString = HttpContext.Session.GetString("ConnectionString").ToString();
                if (IdvalidComp)
                {
                    string Query = @"select * From [User] where IsActive ='Y'  and    UserName ='" + Obj.User_Name + "' and  UserPwd='" + Obj.Password + "'";
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(Query, con))
                        {
                            con.Open();
                            cmd.CommandText = Query;
                            cmd.CommandTimeout = 300;
                            SqlDataReader rdr = cmd.ExecuteReader();
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    HttpContext.Session.SetString("UserName", rdr["UserName"].ToString());
                                    HttpContext.Session.SetString("UserFirstName", rdr["UserFname"].ToString());
                                    HttpContext.Session.SetString("UserLastName", rdr["UserLname"].ToString());
                                    HttpContext.Session.SetString("UserImage", rdr["UserImage"].ToString());
                                    HttpContext.Session.SetString("UserEmail", rdr["UserEmail"].ToString());
                                    HttpContext.Session.SetString("UserMoNo", rdr["UserMobno"].ToString());
                                    HttpContext.Session.SetString("User_Id", rdr["UserId"].ToString());
                                    HttpContext.Session.SetString("BrchId", rdr["BrchId"].ToString());
                                    HttpContext.Session.SetString("StateId", rdr["StateId"].ToString());
                                    HttpContext.Session.SetString("CntryId", rdr["CntryId"].ToString());
                                }
								var responce =GetMenuAuth();
                                if (responce)
                                {

                                    return RedirectToAction("DashBoard", "DashBoard");
                                }
                                else {
									ViewData["Wrong_Credeintials"] = "No Authorization Allocated To This User Contact With Admin";
									return View();
								}
                            }
                            else
                            {
                                ViewData["Wrong_Credeintials"] = "Please enter Correct Usename or Password.";
                                return View();
                            }
                        }
                    }
                }
                else
                {
                    ViewData["Wrong_Credeintials"] = "Please enter Valid Company Code.";
                    return View();
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(new { success = false, message = Ex.Message });
            }
        }
        static string ReplaceDatabaseName(string connectionString, string newDatabaseName)
        {
            string[] parts = connectionString.Split(';');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Trim().StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase))
                {
                    parts[i] = "Initial Catalog=" + newDatabaseName;
                    break;
                }
            }
            string modifiedConnectionString = string.Join(";", parts);
            return modifiedConnectionString;
        }
        public IActionResult Forgotpass() { 
            return View();
        }
		public dynamic GetMenuAuth() {
				string Query = @"
Select T1.AuthType,T0.MenuId,T0.MenuName,T0.MenuPos,T0.MenuUrl  FROM Menu_Mst T0
inner Join UserAuth_Mnu_det T1 on T0.MenuId = T1.MenuId and T0.ParentMenuId=0 And T0.IsActive ='Y'
And  T1.UserId='" + HttpContext.Session.GetString("User_Id") + "'And T1.AuthType  in ('F','R') order by T0.MenuPos Asc";
				List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
				using (SqlConnection con = new SqlConnection(ConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand(Query, con))
					{
						con.Open();
						cmd.CommandText = Query;
						cmd.CommandTimeout = 300;
						SqlDataReader rdr = cmd.ExecuteReader();
						if (rdr.HasRows)
						{
							while (rdr.Read())
							{
								Dictionary<string, dynamic> row = new Dictionary<string, dynamic>();
								for (int i = 0; i < rdr.FieldCount; i++)
								{
									row.Add(rdr.GetName(i), rdr[i].ToString().Trim());

								}
                              row.Add("ParentMenuData",GetMenuAuth(rdr["MenuID"].ToString()));
								dataList.Add(row);
							}
							var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dataList);
					       	HttpContext.Session.SetString("MenuData", jsonString);
						}
						else
						{
					    	return false;
				    	}
					}
				}
			    return true;
		}
		public dynamic GetMenuAuth(string MenuID)
		{
			string Query = @"
Select T1.AuthType,T0.MenuId,T0.MenuName,T0.MenuPos,T0.MenuUrl FROM Menu_Mst T0 "+
"inner Join UserAuth_Mnu_det T1 on T0.MenuId = T1.MenuId and T0.ParentMenuId='"+ MenuID + "' And T0.IsActive ='Y'"+
"And  T1.UserId='" + HttpContext.Session.GetString("User_Id") + "'And T1.AuthType  in ('F','R') order by T0.MenuPos Asc";
			List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
			using (SqlConnection con = new SqlConnection(ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand(Query, con))
				{
					con.Open();
					cmd.CommandText = Query;
					cmd.CommandTimeout = 300;
					   SqlDataReader rdr = cmd.ExecuteReader();
					 if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							Dictionary<string, object> row = new Dictionary<string, object>();
							for (int i = 0; i < rdr.FieldCount; i++)
							{
								row.Add(rdr.GetName(i), rdr[i].ToString().Trim());
							}
							dataList.Add(row);
						}
                        return (dataList);
					}
					else
					{
						return dataList;
					}
				}
			}
			return true;
		}
        public IActionResult UserData()
        {
            try
            {
                string connectionString = HttpContext.Session.GetString("ConnectionString");
                string userName = HttpContext.Session.GetString("UserName");

                if (string.IsNullOrEmpty(userName))
                {
                    return StatusCode(400, "UserName not found in session");
                }

                string query = @"SELECT USR.*, Dm.DeptName, RM.RoleName, SM.StateName, CM.CntryName,
                    (SELECT STRING_AGG(BM.BrchName, ',') 
                        FROM STRING_SPLIT(USR.BrchId, ',') AS s 
                        INNER JOIN Branch_Mst BM ON BM.BrchId = TRY_CAST(s.value AS INT)) AS BranchNames
                 FROM [User] USR WITH (NOLOCK)
                 INNER JOIN Role_mst RM ON RM.Roleid = USR.Roleid
                 INNER JOIN state_mst SM ON SM.StateId = USR.StateId
                 INNER JOIN Country_Mst CM ON CM.CntryId = USR.CntryId
                 INNER JOIN Dept_Mst DM ON DM.DeptId = USR.dept_id
                 WHERE USR.UserName = @UserName";

                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        con.Open();
                        cmd.CommandTimeout = 300;
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < rdr.FieldCount; i++)
                                {
                                    string columnName = rdr.GetName(i);
                                    object? value = rdr.IsDBNull(i) ? null : rdr.GetValue(columnName);
                                    // Convert DATE values to string format without time
                                    if (value is DateTime dateValue && dateValue.TimeOfDay == TimeSpan.Zero)
                                    {
                                        row[columnName] = dateValue.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        row[columnName] = value!;
                                    }
                                }
                                dataList.Add(row);
                            }
                        }
                        con.Close();
                    }
                }
                return Json(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}