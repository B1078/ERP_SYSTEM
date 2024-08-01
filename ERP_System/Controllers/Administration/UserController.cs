using ERP_System.Models;
using ERP_System.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography.Xml;

namespace ERP_System.Controllers.Administration
{
	public class UserController : Controller
	{
		[HttpGet]
		public IActionResult User()
		{
			if (HttpContext.Session.GetString("User_Id") == null)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}


//        public IActionResult GETUSERDATA()
//        {
//            try
//            {
//                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
//                string Query = @"SELECT USR.*,Dm.DeptName, RM.RoleName,SM.StateName,CM.CntryName,
//(SELECT STRING_AGG(BM.BrchName, ',') 
// FROM STRING_SPLIT(USR.BrchId, ',') AS s 
// INNER JOIN Branch_Mst BM ON BM.BrchId = TRY_CAST(s.value AS INT)) AS BranchNames
//FROM [User] USR WITH (NOLOCK)
//INNER JOIN Role_mst RM ON RM.Roleid = USR.Roleid
//left outer join state_mst SM on SM.StateId = USR.StateId
//left outer join Country_Mst CM on CM.CntryId = USR.CntryId
//inner join Dept_Mst  DM on  Dm.DeptId =USR.dept_id";
//                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
//                using (SqlConnection con = new SqlConnection(ConnectionString))
//                {
//                    using (SqlCommand cmd = new SqlCommand(Query, con))
//                    {
//                        con.Open();
//                        cmd.CommandText = Query;
//                        cmd.CommandTimeout = 300;
//                        SqlDataReader rdr = cmd.ExecuteReader();
//                        if (rdr.HasRows)
//                        {
//                            while (rdr.Read())
//                            {
//                                Dictionary<string, object> row = new Dictionary<string, object>();
//                                for (int i = 0; i < rdr.FieldCount; i++)
//                                {
//                                    string columnName = rdr.GetName(i);
//                                    object? value = rdr.IsDBNull(i) ? null : rdr.GetValue(columnName);
//                                    // Convert DATE values to string format without time
//                                    if (value is DateTime dateValue && dateValue.TimeOfDay == TimeSpan.Zero)
//                                    {
//                                        row[columnName] = dateValue.ToString("dd/MM/yyyy");
//                                    }
//                                    else
//                                    {
//                                        row[columnName] = value!;
//                                    }
//                                }
//                                dataList.Add(row);
//                            }
//                        }
//                        con.Close();
//                    }
//                }
//                return Json(dataList);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message.ToString());
//            }
//        }
        public IActionResult GETUSERDATA()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT USR.*,Dm.DeptName, RM.RoleName,SM.StateName,CM.CntryName,
(SELECT STRING_AGG(BM.BrchName, ',') 
 FROM STRING_SPLIT(USR.BrchId, ',') AS s 
 INNER JOIN Branch_Mst BM ON BM.BrchId = TRY_CAST(s.value AS INT)) AS BranchNames
FROM [User] USR WITH (NOLOCK)
INNER JOIN Role_mst RM ON RM.Roleid = USR.Roleid
left outer join state_mst SM on SM.StateId = USR.StateId
left outer join Country_Mst CM on CM.CntryId = USR.CntryId
inner join Dept_Mst  DM on  Dm.DeptId =USR.dept_id";
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
                                    string columnName = rdr.GetName(i);
                                    object? value = rdr.IsDBNull(i) ? null : rdr.GetValue(columnName);
                                    // Convert DATE values to string format without time
                                    if (value is DateTime dateValue && dateValue.TimeOfDay == TimeSpan.Zero)
                                    {
                                        row[columnName] = dateValue.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        // Replace UserPwd with 'XXXXXXX'
                                        if (columnName == "UserPwd")
                                        {
                                            row[columnName] = "XXXXXXX";
                                        }
                                        else
                                        {
                                            row[columnName] = value!;
                                        }
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

        public IActionResult GETMINOTARY()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"Select T1.FieldName, * From Menu_Tab_Det T0
Left Join Menu_TabMand_Det T1 on T0.MenuId =T1.MenuId
where T0.TableName ='User' And T1.IsMandatory='Y'";
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
        [HttpPost]
		public IActionResult POSTDATA(User Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
			{
				TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
				Data.UserFulname = Data.UserFname + "" + Data.UserMname + "" + Data.UserLname;
				Data.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                Data.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                Data.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                Data.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date); ;
                Data.CreatedBy = HttpContext.Session.GetString("UserName");
                Genrate_Query genrate = new Genrate_Query();
                string insertQuery = genrate.GenerateInsertQuery(Data, "[User]","UserId");
                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        con.Open();
                        cmd.CommandText = insertQuery;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT * FROM [User] WHERE UserId = SCOPE_IDENTITY()";
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
        public IActionResult UPDATEDATA(User Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                string cmp_id = HttpContext.Session.GetString("cmp_id");
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                Data.UserFulname = Data.UserFname + "" + Data.UserMname + "" + Data.UserLname;
                Data.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                Data.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                Genrate_Query genrate = new Genrate_Query();
                string Query = genrate.GenerateUpdateQuery(Data,"[User]","UserId",Data.UserId,"");
                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        con.Open();
                        cmd.CommandText = Query;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        con.Close();
                    }
                }
                return Json(new { Success=true,Message="Data Updated Successfully..!"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult DELETEUSERDATA(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
             
                string Query = "Delete from [User] where UserId='"+ Id + "'" +
					"; Delete from [UserAuth_Mnu_det] where UserId='" + Id + "'";
                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        con.Open();
                        cmd.CommandText = Query;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        con.Close();
                    }
                }
                return Json(new { success=true,message="Data Deleted Successfully..!"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

    }
}
